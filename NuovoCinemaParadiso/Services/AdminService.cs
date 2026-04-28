using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NuovoCinemaParadiso.Data;
using NuovoCinemaParadiso.Dtos;
using NuovoCinemaParadiso.Models;
using NuovoCinemaParadiso.Helpers;
using NuovoCinemaParadiso.Exceptions;

namespace NuovoCinemaParadiso.Services;

public class AdminService
{
    private readonly ContestoDb _contesto;
    private readonly UserManager<Utente> _gestioneUtenti;
    public AdminService(ContestoDb contestoDb, UserManager<Utente> gestioneUtenti)
    {
        _contesto = contestoDb;
        _gestioneUtenti = gestioneUtenti;

    }

    public async Task<List<DtoUtente>> OttieniUtentiAsync()
    {
        List<Utente> utenti = await _contesto.Utenti.ToListAsync();

        List<DtoUtente> risultato = new List<DtoUtente>();

        for (int i = 0; i < utenti.Count; i++)
        {
            Utente utenteCorrente = utenti[i];

            Abbonamento? abbonamento = await _contesto.Abbonamenti.FindAsync(utenteCorrente.AbbonamentoId);
            GiftCard? giftCard = await _contesto.GiftCards.FindAsync(utenteCorrente.GiftCardId);

            DtoUtente dto = new DtoUtente();
            dto.Id = utenteCorrente.Id;
            dto.Email = utenteCorrente.Email ?? string.Empty;
            dto.NomeCompleto = utenteCorrente.NomeCompleto ?? string.Empty;
            dto.Eta = utenteCorrente.Eta;
            dto.AbbonamentoId = utenteCorrente.AbbonamentoId ?? string.Empty;
            dto.GiftCardId = utenteCorrente.GiftCardId ?? string.Empty;
            dto.DataInizioAbbonamento = utenteCorrente.DataInizioAbbonamento;
            dto.DataInizioGiftCard = utenteCorrente.DataInizioGiftCard;
            dto.TipoAbbonamento = abbonamento?.Nome ?? string.Empty;
            dto.TipoGiftCard = giftCard?.Nome ?? string.Empty;

            risultato.Add(dto);
        }

        return risultato;
    }

    public async Task<DtoUtente?> OttieniUtenteTramiteIdAsync(string id)
    {
        Utente? utente = await _gestioneUtenti.FindByIdAsync(id)
            ?? throw new NotFoundException("Utente" , id);
        Abbonamento? abbonamento = await _contesto.Abbonamenti.FindAsync(utente.AbbonamentoId);
        GiftCard? giftCard = await _contesto.GiftCards.FindAsync(utente.GiftCardId);

        if (utente == null)
        {
            throw new NotFoundException("Utente" , id);
        }

        DtoUtente dto = new DtoUtente();
        dto.Id = utente.Id;
        dto.Email = utente.Email ?? string.Empty;
        dto.NomeCompleto = utente.NomeCompleto ?? string.Empty;
        dto.Eta = utente.Eta;
        dto.AbbonamentoId = utente.AbbonamentoId ?? string.Empty;
        dto.GiftCardId = utente.GiftCardId ?? string.Empty;
        dto.DataInizioAbbonamento = utente.DataInizioAbbonamento;
        dto.DataInizioGiftCard = utente.DataInizioGiftCard;
        dto.TipoAbbonamento = abbonamento?.Nome ?? string.Empty;
        dto.TipoGiftCard = giftCard?.Nome ?? string.Empty;

        return dto;
    }

    public async Task<IdentityResult> EliminaUtentePerIdAsync(string id)
    {
        Utente? utente = await _gestioneUtenti.FindByIdAsync(id);
        if (utente == null)
        {
            IdentityError errore = new IdentityError();  //
            return IdentityResult.Failed(errore);  //
        }
        IdentityResult risultato = await _gestioneUtenti.DeleteAsync(utente);

        return risultato;
    }

    public async Task<List<DtoAcquisto>> OttieniAcquisti()
    {
        List<Acquisto> acquisti = await _contesto.Acquisti.ToListAsync();

        List<DtoAcquisto> risultato = new List<DtoAcquisto>();

        for (int i = 0; i < acquisti.Count; i++)
        {
            Acquisto acquistoCorrente = acquisti[i];
            Proiezione? proiezione = await _contesto.Proiezioni.FindAsync(acquistoCorrente.ProiezioneId)
                ?? throw new NotFoundException("Proiezione", acquistoCorrente.ProiezioneId);
            Movie? movie = await _contesto.Movies.FindAsync(proiezione.MovieId)
                ?? throw new NotFoundException("Movie", proiezione.MovieId);
            Sala? sala = await _contesto.Sale.FindAsync(proiezione.SalaId)
                ?? throw new NotFoundException("Sala", proiezione.SalaId);
            Utente? utente = await _contesto.Utenti.FindAsync(acquistoCorrente.UtenteId)
                ?? throw new NotFoundException("Utente", acquistoCorrente.UtenteId);
            TipologiaSala? tipologiaSala = await _contesto.TipologieSala.FindAsync(sala.TipologiaSalaId)
                ?? throw new NotFoundException("TipologiaSala", sala.TipologiaSalaId);

            DtoAcquisto dto = new DtoAcquisto();
            dto.Id = acquistoCorrente.Id;
            dto.ProiezioneId = acquistoCorrente.ProiezioneId;
            dto.UtenteId = acquistoCorrente.UtenteId;
            dto.PrezzoFinale = Calcoli.CalcolaPrezzoFinale(movie.PrezzoMovie, tipologiaSala.MaggiorazionePrezzo, acquistoCorrente.NumeroBiglietti, utente, acquistoCorrente.MetodoPagamento);
            dto.OrarioCreazione = acquistoCorrente.OrarioCreazione;
            dto.NumeroBiglietti = acquistoCorrente.NumeroBiglietti;

            risultato.Add(dto);
        }

        return risultato;
    }

    public async Task<DtoAcquisto> OttieniAcquistoTramiteIdAsync(string id)
    {
        Acquisto? acquisto = await _contesto.Acquisti.FindAsync(id)
            ?? throw new NotFoundException("Acquisto", id);
        Proiezione? proiezione = await _contesto.Proiezioni.FindAsync(acquisto.ProiezioneId)
            ?? throw new NotFoundException("Proiezione", acquisto.ProiezioneId);
        Movie? movie = await _contesto.Movies.FindAsync(proiezione.MovieId)
            ?? throw new NotFoundException("Movie", proiezione.MovieId);
        Sala? sala = await _contesto.Sale.FindAsync(proiezione.SalaId)
            ?? throw new NotFoundException("Sala", proiezione.SalaId);
        Utente? utente = await _contesto.Users.FindAsync(acquisto.UtenteId)
            ?? throw new NotFoundException("Utente", acquisto.UtenteId);
        TipologiaSala? tipologiaSala = await _contesto.TipologieSala.FindAsync(sala.TipologiaSalaId)
            ?? throw new NotFoundException("TipologiaSala", sala.TipologiaSalaId);

        if (acquisto == null)
        {
            throw new NotFoundException("Acquisto", id);
        }

        DtoAcquisto dto = new DtoAcquisto();
        dto.Id = acquisto.Id;
        dto.UtenteId = acquisto.UtenteId;
        dto.ProiezioneId = acquisto.ProiezioneId;
        dto.PrezzoFinale = Calcoli.CalcolaPrezzoFinale(movie.PrezzoMovie, tipologiaSala.MaggiorazionePrezzo, acquisto.NumeroBiglietti, utente, acquisto.MetodoPagamento);
        dto.OrarioCreazione = acquisto.OrarioCreazione;
        dto.NumeroBiglietti = acquisto.NumeroBiglietti;

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

        if (abbonamentoTrovato == null)
        {
            return new List<DtoUtente>();
        }

        List<DtoUtente> risultato = new List<DtoUtente>();

        for (int i = 0; i < utenti.Count; i++)
        {
            Utente utenteCorrente = utenti[i];
            Abbonamento? abbonamento = await _contesto.Abbonamenti.FindAsync(utenteCorrente.AbbonamentoId);
            GiftCard? giftCard = await _contesto.GiftCards.FindAsync(utenteCorrente.GiftCardId);

            if (utenteCorrente.Abbonamento == abbonamentoTrovato)
            {
                DtoUtente dto = new DtoUtente();
                dto.Id = utenteCorrente.Id;
                dto.NomeCompleto = utenteCorrente.NomeCompleto;
                dto.Email = utenteCorrente.Email ?? string.Empty;
                dto.Eta = utenteCorrente.Eta;
                dto.SeAbbonato = utenteCorrente.SeAbbonato;
                dto.PossiedeGiftCard = utenteCorrente.PossiedeGiftCard;
                dto.AbbonamentoId = utenteCorrente.AbbonamentoId ?? string.Empty;
                dto.GiftCardId = utenteCorrente.GiftCardId ?? string.Empty;
                dto.DataInizioAbbonamento = utenteCorrente.DataInizioAbbonamento;
                dto.DataInizioGiftCard = utenteCorrente.DataInizioGiftCard;
                dto.TipoAbbonamento = abbonamento?.Nome ?? string.Empty;
                dto.TipoGiftCard = giftCard?.Nome ?? string.Empty;

                risultato.Add(dto);
            }
        }

        return risultato;
    }
    
    public async Task<List<DtoUtente>> OttieniUtentiTramiteGiftCardAsync(string giftCardId)
    {

        List<Utente> utenti = await _contesto.Utenti.ToListAsync();
        List<GiftCard> giftCards = await _contesto.GiftCards.ToListAsync();

        GiftCard? giftCardTrovata = null;

        for (int i = 0; i < giftCards.Count; i++)
        {
            GiftCard giftCardCorrente = giftCards[i];

            if (giftCardCorrente.Id == giftCardId)
            {
                giftCardTrovata = giftCardCorrente;
                break;
            }
        }

        if (giftCardTrovata == null)
        {
            throw new NotFoundException("GiftCard" , giftCardId);
        }

        List<DtoUtente> risultato = new List<DtoUtente>();

        for (int i = 0; i < utenti.Count; i++)
        {
            Utente utenteCorrente = utenti[i];
            Abbonamento? abbonamento = await _contesto.Abbonamenti.FindAsync(utenteCorrente.AbbonamentoId);
            GiftCard? giftCard = await _contesto.GiftCards.FindAsync(utenteCorrente.GiftCardId);

            if (utenteCorrente.GiftCard == giftCardTrovata)
            {
                DtoUtente dto = new DtoUtente();
                dto.Id = utenteCorrente.Id;
                dto.NomeCompleto = utenteCorrente.NomeCompleto;
                dto.Email = utenteCorrente.Email ?? string.Empty;
                dto.Eta = utenteCorrente.Eta;
                dto.AbbonamentoId = utenteCorrente.AbbonamentoId ?? string.Empty;
                dto.GiftCardId = utenteCorrente.GiftCardId ?? string.Empty;
                dto.DataInizioAbbonamento = utenteCorrente.DataInizioAbbonamento;
                dto.DataInizioGiftCard = utenteCorrente.DataInizioGiftCard;
                dto.TipoAbbonamento = abbonamento?.Nome ?? string.Empty;
                dto.TipoGiftCard = giftCard?.Nome ?? string.Empty;

                risultato.Add(dto);
            }
        }

        return risultato;
    }
}