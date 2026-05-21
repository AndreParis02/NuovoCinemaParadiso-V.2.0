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
        List<Utente> utenti = await _contesto.Utenti.ToListAsync();

        List<DtoUtente> risultato = new List<DtoUtente>();

        for (int i = 0; i < utenti.Count; i++)
        {
            Utente utenteCorrente = utenti[i];

            Abbonamento? abbonamento = await _contesto.Abbonamenti.FindAsync(utenteCorrente.AbbonamentoId);

            DtoUtente dto = new DtoUtente();
            dto.Id = utenteCorrente.Id;
            dto.Email = utenteCorrente.Email ?? string.Empty;
            dto.NomeCompleto = utenteCorrente.NomeCompleto ?? string.Empty;
            dto.Eta = utenteCorrente.Eta;
            dto.AbbonamentoId = utenteCorrente.AbbonamentoId ?? string.Empty;
            dto.SeAbbonato = utenteCorrente.SeAbbonato;
            dto.DataInizioAbbonamento = utenteCorrente.DataInizioAbbonamento;
            dto.TipoAbbonamento = abbonamento?.Nome ?? string.Empty;

            risultato.Add(dto);
        }

        return risultato;
    }

    public async Task<DtoUtente?> OttieniUtenteTramiteIdAsync(string id)
    {
        Utente? utente = await _gestioneUtenti.FindByIdAsync(id)
            ?? throw new NotFoundException("Utente", id);
        Abbonamento? abbonamento = await _contesto.Abbonamenti.FindAsync(utente.AbbonamentoId);
        if (utente == null)
        {
            throw new NotFoundException("Utente", id);
        }

        DtoUtente dto = new DtoUtente();
        dto.Id = utente.Id;
        dto.Email = utente.Email ?? string.Empty;
        dto.NomeCompleto = utente.NomeCompleto ?? string.Empty;
        dto.Eta = utente.Eta;
        dto.SeAbbonato = utente.SeAbbonato;
        dto.AbbonamentoId = utente.AbbonamentoId ?? string.Empty;
        dto.DataInizioAbbonamento = utente.DataInizioAbbonamento;
        dto.TipoAbbonamento = abbonamento?.Nome ?? string.Empty;

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

        List<DtoBiglietto> risultato = new List<DtoBiglietto>();

        for (int i = 0; i < biglietti.Count; i++)
        {
            Biglietto bigliettoCorrente = biglietti[i];
            Proiezione? proiezione = await _contesto.Proiezioni.FindAsync(bigliettoCorrente.ProiezioneId)
                ?? throw new NotFoundException("Proiezione", bigliettoCorrente.ProiezioneId);
            Movie? movie = await _contesto.Movies.FindAsync(proiezione.MovieId)
                ?? throw new NotFoundException("Movie", proiezione.MovieId);
            Sala? sala = await _contesto.Sale.FindAsync(proiezione.SalaId)
                ?? throw new NotFoundException("Sala", proiezione.SalaId);
            Utente? utente = await _contesto.Utenti.FindAsync(bigliettoCorrente.UtenteId)
                ?? throw new NotFoundException("Utente", bigliettoCorrente.UtenteId);
            TipologiaSala? tipologiaSala = await _contesto.TipologieSala.FindAsync(sala.TipologiaSalaId)
                ?? throw new NotFoundException("TipologiaSala", sala.TipologiaSalaId);

            if (utente.AbbonamentoId == null)
                throw new NotFoundException("Abbonamento", "Nessun abbonamento associato all'utente");

            if (utente.Abbonamento == null)
                throw new NotFoundException("Abbonamento", utente.AbbonamentoId);

            DtoBiglietto dto = new DtoBiglietto();
            dto.Id = bigliettoCorrente.Id;
            dto.ProiezioneId = bigliettoCorrente.ProiezioneId;
            dto.UtenteId = bigliettoCorrente.UtenteId;
            dto.OrarioCreazione = bigliettoCorrente.OrarioCreazione;
            dto.NumeroBiglietti = bigliettoCorrente.NumeroBiglietti;
            dto.PrezzoFinale = Calcoli.CalcolaPrezzoFinale(movie.PrezzoMovie,
                tipologiaSala.MaggiorazionePrezzo,
                bigliettoCorrente.NumeroBiglietti,
                utente.Abbonamento,
                utente.DataInizioAbbonamento
            );

            risultato.Add(dto);
        }

        return risultato;
    }

    public async Task<DtoBiglietto> OttieniBigliettoTramiteIdAsync(string id)
    {
        Biglietto? biglietto = await _contesto.Biglietti.FindAsync(id)
            ?? throw new NotFoundException("Biglietto", id);
        Proiezione? proiezione = await _contesto.Proiezioni.FindAsync(biglietto.ProiezioneId)
            ?? throw new NotFoundException("Proiezione", biglietto.ProiezioneId);
        Movie? movie = await _contesto.Movies.FindAsync(proiezione.MovieId)
            ?? throw new NotFoundException("Movie", proiezione.MovieId);
        Sala? sala = await _contesto.Sale.FindAsync(proiezione.SalaId)
            ?? throw new NotFoundException("Sala", proiezione.SalaId);
        Utente? utente = await _contesto.Users.FindAsync(biglietto.UtenteId)
            ?? throw new NotFoundException("Utente", biglietto.UtenteId);
        TipologiaSala? tipologiaSala = await _contesto.TipologieSala.FindAsync(sala.TipologiaSalaId)
            ?? throw new NotFoundException("TipologiaSala", sala.TipologiaSalaId);

        if (utente.AbbonamentoId == null)
            throw new NotFoundException("Abbonamento", "Nessun abbonamento associato all'utente");

        if (utente.Abbonamento == null)
            throw new NotFoundException("Abbonamento", utente.AbbonamentoId);

        if (biglietto == null)
            throw new NotFoundException("Biglietto", id);

        DtoBiglietto dto = new DtoBiglietto();
        dto.Id = biglietto.Id;
        dto.UtenteId = biglietto.UtenteId;
        dto.ProiezioneId = biglietto.ProiezioneId;
        dto.OrarioCreazione = biglietto.OrarioCreazione;
        dto.NumeroBiglietti = biglietto.NumeroBiglietti;
        dto.PrezzoFinale = Calcoli.CalcolaPrezzoFinale(movie.PrezzoMovie,
            tipologiaSala.MaggiorazionePrezzo,
            biglietto.NumeroBiglietti,
            utente.Abbonamento,
            utente.DataInizioAbbonamento
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

        if (abbonamentoTrovato == null)
        {
            return new List<DtoUtente>();
        }

        List<DtoUtente> risultato = new List<DtoUtente>();

        for (int i = 0; i < utenti.Count; i++)
        {
            Utente utenteCorrente = utenti[i];
            Abbonamento? abbonamento = await _contesto.Abbonamenti.FindAsync(utenteCorrente.AbbonamentoId);

            if (utenteCorrente.Abbonamento == abbonamentoTrovato)
            {
                DtoUtente dto = new DtoUtente();
                dto.Id = utenteCorrente.Id;
                dto.NomeCompleto = utenteCorrente.NomeCompleto;
                dto.Email = utenteCorrente.Email ?? string.Empty;
                dto.Eta = utenteCorrente.Eta;
                dto.SeAbbonato = utenteCorrente.SeAbbonato;
                dto.AbbonamentoId = utenteCorrente.AbbonamentoId ?? string.Empty;
                dto.DataInizioAbbonamento = utenteCorrente.DataInizioAbbonamento;
                dto.TipoAbbonamento = abbonamento?.Nome ?? string.Empty;

                risultato.Add(dto);
            }
        }

        return risultato;
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