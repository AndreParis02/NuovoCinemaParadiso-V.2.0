using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NuovoCinemaParadiso.Data;
using NuovoCinemaParadiso.Dtos;
using NuovoCinemaParadiso.Models;
using NuovoCinemaParadiso.Helpers;
using NuovoCinemaParadiso.Exceptions;

namespace NuovoCinemaParadiso.Services;

public class OperatoreService
{
    private readonly ContestoDb _contesto;
    private readonly UserManager<Utente> _gestioneUtenti;
    public OperatoreService(ContestoDb contestoDb, UserManager<Utente> gestioneUtenti)
    {
        _contesto = contestoDb;
        _gestioneUtenti = gestioneUtenti;
    }

    public async Task<bool> RicaricaAsync(DtoRicaricaSaldoUtente dtoRicaricaSaldoUtente)
    {
        Utente? utente = await _gestioneUtenti.FindByEmailAsync(dtoRicaricaSaldoUtente.Email);
        if (utente == null)
            return false;
        utente.Saldo += dtoRicaricaSaldoUtente.Ricarica;
        await _gestioneUtenti.UpdateAsync(utente);
        return true;
    }

    public async Task<List<DtoUtente>> OttieniUtentiAsync()
    {
        List<Utente> utenti = await _contesto.Utenti
            .Include(u => u.UtentiAbbonamenti)
            .ThenInclude(ua => ua.Abbonamento)
            .ToListAsync();

        return utenti
            .Select(utenteCorrente =>
            {
                var abbonamentoAttivo = utenteCorrente.UtentiAbbonamenti
                    .Where(ua => ua.DataInizioAbbonamento <= DateTimeOffset.UtcNow && ua.DataFine > DateTimeOffset.UtcNow)
                    .OrderByDescending(ua => ua.DataInizioAbbonamento)
                    .FirstOrDefault();

                return new DtoUtente
                {
                    Id = utenteCorrente.Id,
                    Email = utenteCorrente.Email ?? string.Empty,
                    NomeCompleto = utenteCorrente.NomeCompleto ?? string.Empty,
                    Eta = utenteCorrente.Eta,
                    AbbonamentoId = abbonamentoAttivo?.AbbonamentoId ?? string.Empty,
                    SeAbbonato = abbonamentoAttivo != null,
                    DataInizioAbbonamento = abbonamentoAttivo?.DataInizioAbbonamento,
                    TipoAbbonamento = abbonamentoAttivo?.Abbonamento?.Nome ?? string.Empty,
                    Saldo = utenteCorrente.Saldo
                };
            })
            .ToList();
    }

    public async Task<DtoUtente?> OttieniUtenteTramiteIdAsync(string id)
    {
        Utente? utente = await _contesto.Utenti
            .Include(u => u.UtentiAbbonamenti)
            .ThenInclude(ua => ua.Abbonamento)
            .FirstOrDefaultAsync(u => u.Id == id)
            ?? throw new NotFoundException("Utente", id);

        var abbonamentoAttivo = utente.UtentiAbbonamenti
            .Where(ua => ua.DataInizioAbbonamento <= DateTimeOffset.UtcNow && ua.DataFine > DateTimeOffset.UtcNow)
            .OrderByDescending(ua => ua.DataInizioAbbonamento)
            .FirstOrDefault();

        DtoUtente dto = new DtoUtente();
        dto.Id = utente.Id;
        dto.Email = utente.Email ?? string.Empty;
        dto.NomeCompleto = utente.NomeCompleto ?? string.Empty;
        dto.Eta = utente.Eta;
        dto.SeAbbonato = abbonamentoAttivo != null;
        dto.AbbonamentoId = abbonamentoAttivo?.AbbonamentoId ?? string.Empty;
        dto.DataInizioAbbonamento = abbonamentoAttivo?.DataInizioAbbonamento;
        dto.TipoAbbonamento = abbonamentoAttivo?.Abbonamento?.Nome ?? string.Empty;
        dto.Saldo = utente.Saldo;

        return dto;
    }

    public async Task<bool> EliminaUtentePerIdAsync(string id)
    {
        Utente? utente = await _gestioneUtenti.FindByIdAsync(id);
        if (utente == null)
        {
            throw new NotFoundException("Utente", id);
        }
        IdentityResult risultato = await _gestioneUtenti.DeleteAsync(utente);

        return true;
    }

    public async Task<List<DtoBiglietto>> OttieniBiglietti()
    {
        List<Biglietto> biglietti = await _contesto.Biglietti.ToListAsync();

        var risultato = await Task.WhenAll(biglietti.Select(async bigliettoCorrente =>
        {
            Proiezione? proiezione = await _contesto.Proiezioni.FindAsync(bigliettoCorrente.ProiezioneId)
                ?? throw new NotFoundException("Proiezione", bigliettoCorrente.ProiezioneId);
            Movie? movie = await _contesto.Movies.FindAsync(proiezione.MovieId)
                ?? throw new NotFoundException("Movie", proiezione.MovieId);
            Sala? sala = await _contesto.Sale.FindAsync(proiezione.SalaId)
                ?? throw new NotFoundException("Sala", proiezione.SalaId);
            Utente? utente = await _contesto.Utenti
                .Include(u => u.UtentiAbbonamenti)
                .ThenInclude(ua => ua.Abbonamento)
                .FirstOrDefaultAsync(u => u.Id == bigliettoCorrente.UtenteId)
                ?? throw new NotFoundException("Utente", bigliettoCorrente.UtenteId);
            TipologiaSala? tipologiaSala = await _contesto.TipologieSala.FindAsync(sala.TipologiaSalaId)
                ?? throw new NotFoundException("TipologiaSala", sala.TipologiaSalaId);
            Turno? turno = await _contesto.Turni.FindAsync(proiezione.TurnoId)
                ?? throw new NotFoundException("Turno", proiezione.TurnoId);

            var abbonamentoAttivo = utente.UtentiAbbonamenti
                .Where(ua => ua.DataInizioAbbonamento <= DateTimeOffset.UtcNow && ua.DataFine > DateTimeOffset.UtcNow)
                .OrderByDescending(ua => ua.DataInizioAbbonamento)
                .FirstOrDefault();

            return new DtoBiglietto
            {
                Id = bigliettoCorrente.Id,
                ProiezioneId = bigliettoCorrente.ProiezioneId,
                UtenteId = bigliettoCorrente.UtenteId,
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
                    abbonamentoAttivo?.Abbonamento,
                    abbonamentoAttivo?.DataInizioAbbonamento
                )
            };
        }));

        return risultato.ToList();
    }

    public async Task<DtoBiglietto> OttieniBigliettoTramiteIdAsync(string id)
    {
        Biglietto? biglietto = await _contesto.Biglietti
          .FirstOrDefaultAsync(b => b.Id == id)
          ?? throw new NotFoundException("Biglietto", id);
        Proiezione? proiezione = await _contesto.Proiezioni.FindAsync(biglietto.ProiezioneId)
            ?? throw new NotFoundException("Proiezione", biglietto.ProiezioneId);
        Movie? movie = await _contesto.Movies.FindAsync(proiezione.MovieId)
            ?? throw new NotFoundException("Movie", proiezione.MovieId);
        Sala? sala = await _contesto.Sale.FindAsync(proiezione.SalaId)
            ?? throw new NotFoundException("Sala", proiezione.SalaId);
        Utente? utente = await _contesto.Utenti
            .Include(u => u.UtentiAbbonamenti)
            .ThenInclude(ua => ua.Abbonamento)
            .FirstOrDefaultAsync(u => u.Id == biglietto.UtenteId)
            ?? throw new NotFoundException("Utente", biglietto.UtenteId);
        TipologiaSala? tipologiaSala = await _contesto.TipologieSala.FindAsync(sala.TipologiaSalaId)
            ?? throw new NotFoundException("TipologiaSala", sala.TipologiaSalaId);
        Turno? turno = await _contesto.Turni.FindAsync(proiezione.TurnoId)
            ?? throw new NotFoundException("Turno", proiezione.TurnoId);

        var abbonamentoAttivo = utente.UtentiAbbonamenti
            .Where(ua => ua.DataInizioAbbonamento <= DateTimeOffset.UtcNow && ua.DataFine > DateTimeOffset.UtcNow)
            .OrderByDescending(ua => ua.DataInizioAbbonamento)
            .FirstOrDefault();

        DtoBiglietto dto = new DtoBiglietto();
        dto.Id = biglietto.Id;
        dto.UtenteId = biglietto.UtenteId;
        dto.ProiezioneId = biglietto.ProiezioneId;
        dto.OrarioCreazione = biglietto.OrarioCreazione;
        dto.NumeroBiglietti = biglietto.NumeroBiglietti;
        dto.NomeSala = sala.Nome;
        dto.TitoloMovie = movie.Titolo;
        dto.NomeTipologiaSala = tipologiaSala.Nome;
        dto.OraInizio = turno.OraInizio;
        dto.DataProiezione = proiezione.DataProiezione;
        dto.PrezzoFinale = Calcoli.CalcolaPrezzoFinale(movie.PrezzoMovie,
            tipologiaSala.MaggiorazionePrezzo,
            biglietto.NumeroBiglietti,
            abbonamentoAttivo?.Abbonamento,
            abbonamentoAttivo?.DataInizioAbbonamento
        );

        return dto;
    }

    public async Task<List<DtoUtente>> OttieniUtentiTramiteAbbonamentoAsync(string abbonamentoId)
    {

        List<Utente> utenti = await _contesto.Utenti.ToListAsync();
        List<Abbonamento> abbonamenti = await _contesto.Abbonamenti.ToListAsync();

        Abbonamento? abbonamentoTrovato = null;

        for (int i = 0; i < abbonamenti.Count; i++)
        {
            Abbonamento abbonamentoCorrente = abbonamenti[i];

            if (abbonamentoCorrente.Id == abbonamentoId)
            {
                abbonamentoTrovato = abbonamentoCorrente;
                break;
            }
        }

        //ritorna un'eccezione se l'abbonamento con l'id specificato non è stato trovato
        if (abbonamentoTrovato == null)
        {
            throw new Exception($"Abbonamento con id {abbonamentoId} non trovato");
        }

        List<DtoUtente> risultato = new List<DtoUtente>();

        for (int i = 0; i < utenti.Count; i++)
        {
            Utente utenteCorrente = utenti[i];
            Abbonamento? abbonamento = await _contesto.Abbonamenti.FindAsync(utenteCorrente.AbbonamentoId);

            if (utenteCorrente.Abbonamento == abbonamentoTrovato)
            {
                Id = utenteAbbonamento.Utente!.Id,
                NomeCompleto = utenteAbbonamento.Utente.NomeCompleto,
                Email = utenteAbbonamento.Utente.Email ?? string.Empty,
                Eta = utenteAbbonamento.Utente.Eta,
                SeAbbonato = utenteAbbonamento.DataFine > DateTimeOffset.UtcNow,
                AbbonamentoId = utenteAbbonamento.AbbonamentoId,
                DataInizioAbbonamento = utenteAbbonamento.DataInizioAbbonamento,
                TipoAbbonamento = utenteAbbonamento.Abbonamento?.Nome ?? string.Empty
            })
            .ToList();
    }

    public async Task<bool> RicaricaGiftCardAsync(DtoRicaricaGiftCard dto)
    {
        if (dto.Importo <= 0)
        {
            throw new Exception("Impossibile ricaricare la giftcard. Importo non valido.");
        }

        /* L'operatore crea la GiftCard "dal nulla", senza scalare un saldo, 
        perché si presume che il pagamento sia stato gestito in cassa.*/
        GiftCard nuovaGiftCard = new GiftCard()
        {
            Nome = "GiftCard",
            Valore = dto.Importo,
            CodiceRiscatto = GiftCardHelper.GeneraCodice()
        };

        await _contesto.GiftCards.AddAsync(nuovaGiftCard);
        await _contesto.SaveChangesAsync();

        return true;
    }
}