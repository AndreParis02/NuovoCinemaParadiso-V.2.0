using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using NuovoCinemaParadiso.Data;
using NuovoCinemaParadiso.Dtos;
using NuovoCinemaParadiso.Models;
using NuovoCinemaParadiso.Exceptions;
using NuovoCinemaParadiso.Helpers;
using System.Security.Claims;

namespace NuovoCinemaParadiso.Services;

public class UtenteService
{
    private readonly ContestoDb _contesto;
    private readonly UserManager<Utente> _gestioneUtenti;

    public UtenteService(ContestoDb contestoDb, UserManager<Utente> gestioneUtenti)
    {
        _contesto = contestoDb;
        _gestioneUtenti = gestioneUtenti;
    }

    public async Task<List<DtoBiglietto>> OttieniTuttiBigliettiAsync(string utenteId)
    {
        var utente = await _gestioneUtenti.Users
            .Include(u => u.UtentiAbbonamenti)
            .ThenInclude(ua => ua.Abbonamento)
            .FirstOrDefaultAsync(u => u.Id == utenteId);

        var abbonamentoAttivo = utente?.UtentiAbbonamenti
            .Where(ua => ua.DataInizioAbbonamento <= DateTimeOffset.UtcNow && ua.DataFine > DateTimeOffset.UtcNow)
            .OrderByDescending(ua => ua.DataInizioAbbonamento)
            .FirstOrDefault();

        var biglietti = await _contesto.Biglietti.ToListAsync();
        var listaBiglietti = await Task.WhenAll(biglietti
            .Where(bigliettoCorrente => bigliettoCorrente.UtenteId == utenteId)
            .Select(async bigliettoCorrente =>
            {
                Proiezione? proiezione = await _contesto.Proiezioni.FindAsync(bigliettoCorrente.ProiezioneId);
                Movie? movie = await _contesto.Movies.FindAsync(proiezione!.MovieId);
                Sala? sala = await _contesto.Sale.FindAsync(proiezione.SalaId);
                TipologiaSala? tipologiaSala = await _contesto.TipologieSala.FindAsync(sala!.TipologiaSalaId);
                Turno? turno = await _contesto.Turni.FindAsync(proiezione.TurnoId)
                    ?? throw new NotFoundException("Turno", proiezione.TurnoId);

                var abbonamentoAcquisto = utente?.UtentiAbbonamenti
                    .Where(ua => ua.DataInizioAbbonamento <= bigliettoCorrente.OrarioCreazione
                              && ua.DataFine > bigliettoCorrente.OrarioCreazione)
                    .OrderByDescending(ua => ua.DataInizioAbbonamento)
                    .FirstOrDefault();

                return new DtoBiglietto
                {
                    Id = bigliettoCorrente.Id,
                    UtenteId = bigliettoCorrente.UtenteId,
                    ProiezioneId = bigliettoCorrente.ProiezioneId,
                    OrarioCreazione = bigliettoCorrente.OrarioCreazione,
                    NumeroBiglietti = bigliettoCorrente.NumeroBiglietti,
                    NomeSala = sala.Nome,
                    TitoloMovie = movie.Titolo,
                    NomeTipologiaSala = tipologiaSala.Nome,
                    OraInizio = turno.OraInizio,
                    DataProiezione = proiezione.DataProiezione,
                    PrezzoFinale = Calcoli.CalcolaPrezzoFinale(
                        movie.PrezzoMovie,
                        tipologiaSala.MaggiorazionePrezzo,
                        bigliettoCorrente.NumeroBiglietti,
                        abbonamentoAcquisto?.Abbonamento,
                        abbonamentoAcquisto?.DataInizioAbbonamento)
                };
            }));

        return listaBiglietti.ToList();
    }

    public async Task<(bool Successo, string Messaggio)> AbbonatiAsync(string abbonamentoId, string utenteId)
    {
        Abbonamento? abbonamentoTrovato = await _contesto.Abbonamenti.FindAsync(abbonamentoId);
        if (abbonamentoTrovato == null)
            return (false, "Abbonamento non trovato.");

        Utente? utenteTrovato = await _gestioneUtenti.Users
            .Include(u => u.UtentiAbbonamenti)
            .FirstOrDefaultAsync(u => u.Id == utenteId);
        if (utenteTrovato == null)
            return (false, "Utente non trovato.");

        var abbonamentoAttivo = utenteTrovato.UtentiAbbonamenti
            .Where(ua => ua.DataInizioAbbonamento <= DateTimeOffset.UtcNow && ua.DataFine > DateTimeOffset.UtcNow)
            .OrderByDescending(ua => ua.DataInizioAbbonamento)
            .FirstOrDefault();

        if (abbonamentoAttivo != null)
            return (false, "L'utente è già abbonato.");

        if (utenteTrovato.Saldo < abbonamentoTrovato.Prezzo)
            return (false, "Credito insufficiente per abbonarsi.");

        var contoCinema = await _contesto.ContoCinema.FirstOrDefaultAsync();
        var saldi = await Calcoli.CalcolaSaldo(abbonamentoTrovato.Prezzo, utenteTrovato, contoCinema);
        utenteTrovato.Saldo = saldi[0];
        contoCinema.Saldo = saldi[1];

        UtenteAbbonamento nuovoUtenteAbbonamento = new UtenteAbbonamento
        {
            UtenteId = utenteId,
            AbbonamentoId = abbonamentoId,
            DataInizioAbbonamento = DateTimeOffset.UtcNow,
            DataFine = DateTimeOffset.UtcNow.AddMonths(abbonamentoTrovato.Durata)
        };

        _contesto.UtenteAbbonamento.Add(nuovoUtenteAbbonamento);
        await _contesto.SaveChangesAsync();

        return (true, "Abbonamento attivato correttamente.");
    }

    public async Task<(bool Successo, string Messaggio)> RimborsaAbbonamentoAsync(string utenteId)
    {
        Utente? utente = await _gestioneUtenti.Users
            .Include(u => u.UtentiAbbonamenti)
            .FirstOrDefaultAsync(u => u.Id == utenteId);

        if (utente == null)
            return (false, "Utente non trovato.");

        var abbonamentoAttivo = utente.UtentiAbbonamenti
            .Where(ua => ua.DataInizioAbbonamento <= DateTimeOffset.UtcNow && ua.DataFine > DateTimeOffset.UtcNow)
            .OrderByDescending(ua => ua.DataInizioAbbonamento)
            .FirstOrDefault();

        if (abbonamentoAttivo == null)
            return (false, "Nessun abbonamento attivo da rimborsare.");

        bool haBigliettiUtilizzati = await _contesto.Biglietti
            .AnyAsync(b => b.UtenteId == utenteId
                && b.OrarioCreazione >= abbonamentoAttivo.DataInizioAbbonamento
                && b.OrarioCreazione < abbonamentoAttivo.DataFine);

        if (haBigliettiUtilizzati)
            return (false, "Non è possibile richiedere il rimborso perché l'abbonamento è già stato utilizzato.");

        var abbonamento = await _contesto.Abbonamenti.FindAsync(abbonamentoAttivo.AbbonamentoId);
        if (abbonamento == null)
            return (false, "Abbonamento non trovato.");

        utente.Saldo += abbonamento.Prezzo;

        var contoCinema = await _contesto.ContoCinema.FirstOrDefaultAsync();
        if (contoCinema != null)
        {
            contoCinema.Saldo = Math.Max(0, contoCinema.Saldo - abbonamento.Prezzo);
        }

        _contesto.UtenteAbbonamento.Remove(abbonamentoAttivo);
        await _contesto.SaveChangesAsync();

        return (true, "Rimborso effettuato correttamente.");
    }

    public async Task<(bool Successo, string Messaggio)> RicaricaGiftCardAsync(string utenteId, DtoRicaricaGiftCard dto)
    {
        Utente? utenteCorrente = await _gestioneUtenti.FindByIdAsync(utenteId);

        if (utenteCorrente == null)
            return (false, "Utente non trovato.");

        if (dto.Importo <= 0)
            return (false, "Importo non valido.");

        if (utenteCorrente.Saldo < dto.Importo)
            return (false, "Saldo insufficiente.");

        

        GiftCard nuovaGiftCard = new GiftCard
        {
            Nome = "GiftCard",
            Valore = dto.Importo,
            CodiceRiscatto = GiftCardHelper.GeneraCodice(),
            Riscattata = false,
            UtenteId = utenteId // -> MANCAVA L'UTENTE!!!!!!
        };

        await _contesto.GiftCards.AddAsync(nuovaGiftCard);
        //modifico il credito dell'utente e del conto cinema
        var contoCinema=await _contesto.ContoCinema.FirstOrDefaultAsync();
        contoCinema.Saldo += dto.Importo;
        utenteCorrente.Saldo -= dto.Importo;
        await _contesto.SaveChangesAsync();

        return (true, $"Gift card creata correttamente: {nuovaGiftCard.CodiceRiscatto}");
    }

    public async Task<(bool Successo, string Messaggio, DtoCreazioneGiftCard? Dto)>
        RiscattaGiftCardAsync(string utenteId, DtoCodiceRiscatto dto)
    {
        GiftCard? trovata = await _contesto.GiftCards
            .FirstOrDefaultAsync(g => g.CodiceRiscatto == dto.CodiceRiscatto);

        if (trovata == null)
            return (false, "Codice non valido.", null);

        if (trovata.Riscattata)
            return (false, "Gift card già riscattata.", null);

        Utente? utenteCorrente = await _gestioneUtenti.FindByIdAsync(utenteId);
        if (utenteCorrente == null)
            return (false, "Utente non trovato.", null);

        trovata.Riscattata = true;
        trovata.UtenteId = utenteId; 
        utenteCorrente.Saldo += trovata.Valore; 

        await _contesto.SaveChangesAsync();

        DtoCreazioneGiftCard risposta = new DtoCreazioneGiftCard
        {
            Nome = trovata.Nome,
            Valore = trovata.Valore,
            CodiceRiscatto = trovata.CodiceRiscatto
        };

        return (true, "Gift card riscattata con successo! Saldo aggiornato.", risposta);
    }

}