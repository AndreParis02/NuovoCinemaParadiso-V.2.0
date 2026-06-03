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
        var biglietti = await _contesto.Biglietti.ToListAsync();
        List<DtoBiglietto> listaBiglietti = new List<DtoBiglietto>();

        for (int i = 0; i < biglietti.Count; i++)
        {
            Biglietto bigliettoCorrente = biglietti[i];
            if (bigliettoCorrente.UtenteId == utenteId)
            {
                Proiezione? proiezione = await _contesto.Proiezioni.FindAsync(bigliettoCorrente.ProiezioneId);
                Movie? movie = await _contesto.Movies.FindAsync(proiezione.MovieId);
                Sala? sala = await _contesto.Sale.FindAsync(proiezione.SalaId);
                TipologiaSala? tipologiaSala = await _contesto.TipologieSala.FindAsync(sala.TipologiaSalaId);
                Turno? turno = await _contesto.Turni.FindAsync(proiezione.TurnoId)
                    ?? throw new NotFoundException("Turno", proiezione.TurnoId);

                DtoBiglietto dto = new DtoBiglietto
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
                    PrezzoFinale = Calcoli.CalcolaPrezzoFinale(movie.PrezzoMovie, tipologiaSala.MaggiorazionePrezzo, bigliettoCorrente.NumeroBiglietti, (await _gestioneUtenti.FindByIdAsync(utenteId)).Abbonamento, (await _gestioneUtenti.FindByIdAsync(utenteId)).DataInizioAbbonamento)
                };
                listaBiglietti.Add(dto);
            }
        }
        return listaBiglietti;
    }

    public async Task<(bool Successo, string Messaggio)> AbbonatiAsync(string abbonamentoId, string utenteId)
    {
        List<Abbonamento> abbonamenti = await _contesto.Abbonamenti.ToListAsync();
        Abbonamento? abbonamentoTrovato = null;

        for (int i = 0; i < abbonamenti.Count; i++)
        {
            if (abbonamenti[i].Id == abbonamentoId)
            {
                abbonamentoTrovato = abbonamenti[i];
                break;
            }
        }

        if (abbonamentoTrovato == null)
            return (false, "Abbonamento non trovato.");

        Utente? utenteTrovato = await _gestioneUtenti.FindByIdAsync(utenteId);
        if (utenteTrovato == null)
            return (false, "Utente non trovato.");

        if (utenteTrovato.SeAbbonato)
            return (false, "L'utente è già abbonato.");

        if (utenteTrovato.Saldo < abbonamentoTrovato.Prezzo)
            return (false, "Credito insufficiente per abbonarsi.");

        utenteTrovato.Saldo -= abbonamentoTrovato.Prezzo;
        utenteTrovato.AbbonamentoId = abbonamentoTrovato.Id;
        utenteTrovato.SeAbbonato = true;
        utenteTrovato.DataInizioAbbonamento = DateTimeOffset.UtcNow;

        await _contesto.SaveChangesAsync();

        return (true, "Abbonamento attivato correttamente.");
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

        utenteCorrente.Saldo -= dto.Importo;

        GiftCard nuovaGiftCard = new GiftCard
        {
            Nome = "GiftCard",
            Valore = dto.Importo,
            CodiceRiscatto = GiftCardHelper.GeneraCodice(),
            Riscattata = false
        };

        await _contesto.GiftCards.AddAsync(nuovaGiftCard);

        await _contesto.SaveChangesAsync();

        return (true, "Gift card creata correttamente.");
    }

    public async Task<(bool Successo, string Messaggio, DtoCreazioneGiftCard? Dto)> RiscattaGiftCardAsync(DtoCodiceRiscatto dto)
    {
        List<GiftCard> tutte = _contesto.GiftCards.ToList();
        GiftCard? trovata = null;

        // Cerco la gift card SENZA lambda
        foreach (GiftCard g in tutte)
        {
            if (g.CodiceRiscatto == dto.CodiceRiscatto)
            {
                trovata = g;
                break;
            }
        }

        if (trovata == null)
            return (false, "Codice non valido.", null);

        if (trovata.Riscattata)
            return (false, "Gift card già riscattata.", null);

        trovata.Riscattata = true;
        await _contesto.SaveChangesAsync();

        DtoCreazioneGiftCard risposta = new DtoCreazioneGiftCard
        {
            Nome = trovata.Nome,
            Valore = trovata.Valore,
            CodiceRiscatto = trovata.CodiceRiscatto
        };

        return (true, "Gift card riscattata.", risposta);
    }
}