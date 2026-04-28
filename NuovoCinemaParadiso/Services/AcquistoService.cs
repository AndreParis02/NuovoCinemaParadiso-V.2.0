using Microsoft.EntityFrameworkCore;
using NuovoCinemaParadiso.Data;
using NuovoCinemaParadiso.Dtos;
using NuovoCinemaParadiso.Models;
using NuovoCinemaParadiso.Helpers;
using NuovoCinemaParadiso.Exceptions;

namespace NuovoCinemaParadiso.Services;

public class AcquistoService
{
    private readonly ContestoDb _contesto;
    public AcquistoService(ContestoDb contesto)
    {
        _contesto = contesto;
    }

    public async Task<List<DtoAcquisto>> OttieniTutto(string utenteId)
    {
        List<Acquisto> acquisti = await _contesto.Acquisti.ToListAsync();

        List<DtoAcquisto> risultato = new List<DtoAcquisto>();

        for (int i = 0; i < acquisti.Count; i++)
        {
            Acquisto acquistoCorrente = acquisti[i];
            Utente? utente = await _contesto.Utenti.FindAsync(acquistoCorrente.UtenteId)
                ?? throw new NotFoundException("Utente", acquistoCorrente.UtenteId);
            Proiezione? proiezione = await _contesto.Proiezioni.FindAsync(acquistoCorrente.ProiezioneId)
                ?? throw new NotFoundException("Proiezione", acquistoCorrente.ProiezioneId);
            Movie? movie = await _contesto.Movies.FindAsync(proiezione.MovieId)
                ?? throw new NotFoundException("Movie", proiezione.MovieId);
            Sala? sala = await _contesto.Sale.FindAsync(proiezione.SalaId)
                ?? throw new NotFoundException("Sala", proiezione.SalaId);
            TipologiaSala? tipologiaSala = await _contesto.TipologieSala.FindAsync(sala.TipologiaSalaId)
                ?? throw new NotFoundException("TipologiaSala", sala.TipologiaSalaId);

            if (acquistoCorrente.UtenteId == utenteId)
            {
                DtoAcquisto dto = new DtoAcquisto();
                dto.Id = acquistoCorrente.Id;
                dto.UtenteId = utente.Id;
                dto.ProiezioneId = acquistoCorrente.ProiezioneId;
                dto.OrarioCreazione = acquistoCorrente.OrarioCreazione;
                dto.NumeroBiglietti = acquistoCorrente.NumeroBiglietti;
                dto.MetodoPagamento = acquistoCorrente.MetodoPagamento;
                dto.PrezzoFinale = Calcoli.CalcolaPrezzoFinale(
                    movie.PrezzoMovie,
                    tipologiaSala.MaggiorazionePrezzo,
                    acquistoCorrente.NumeroBiglietti,
                    utente,
                    acquistoCorrente.MetodoPagamento);

                risultato.Add(dto);
            }
        }

        return risultato;
    }

    public async Task<DtoAcquisto> OttieniTramiteIdAsync(string id, string utenteId)
    {
        Acquisto? acquisto = await _contesto.Acquisti.FindAsync(id)
            ?? throw new NotFoundException("Acquisto", id);
        Utente? utente = await _contesto.Utenti.FindAsync(acquisto.UtenteId)
            ?? throw new NotFoundException("Utente", acquisto.UtenteId);
        Proiezione proiezione = await _contesto.Proiezioni.FindAsync(acquisto.ProiezioneId)
            ?? throw new NotFoundException("Proiezione", acquisto.ProiezioneId);
        Movie movie = await _contesto.Movies.FindAsync(proiezione.MovieId)
            ?? throw new NotFoundException("Movie", proiezione.MovieId);
        Sala sala = await _contesto.Sale.FindAsync(proiezione.SalaId)
            ?? throw new NotFoundException("Sala", proiezione.SalaId);
        TipologiaSala tipologiaSala = await _contesto.TipologieSala.FindAsync(sala.TipologiaSalaId)
            ?? throw new NotFoundException("TipologiaSala", sala.TipologiaSalaId);

        if (acquisto == null)
        {
            throw new NotFoundException("Acquisto", id);
        }

        if (acquisto.UtenteId != utenteId)
        {
            throw new UnauthorizedAccessException("Non puoi accedere a questo acquisto.");
        }

        DtoAcquisto dto = new DtoAcquisto();
        dto.Id = acquisto.Id;
        dto.UtenteId = utente.Id;
        dto.ProiezioneId = acquisto.ProiezioneId;
        dto.OrarioCreazione = acquisto.OrarioCreazione;
        dto.NumeroBiglietti = acquisto.NumeroBiglietti;
        dto.MetodoPagamento = acquisto.MetodoPagamento;
        dto.PrezzoFinale = Calcoli.CalcolaPrezzoFinale(
            movie.PrezzoMovie,
            tipologiaSala.MaggiorazionePrezzo,
            acquisto.NumeroBiglietti,
            utente,
            acquisto.MetodoPagamento);

        return dto;
    }

    public async Task<DtoAcquisto> CreazioneAsync(DtoCreazioneAcquisto dto, string utenteId)
    {
        // Recupera le entità necessarie
        Utente utente = await _contesto.Utenti.FindAsync(utenteId)
               ?? throw new NotFoundException("Utente", utenteId);
        Proiezione proiezione = await _contesto.Proiezioni.FindAsync(dto.ProiezioneId)
            ?? throw new NotFoundException("Proiezione", dto.ProiezioneId);
        Movie movie = await _contesto.Movies.FindAsync(proiezione.MovieId)
            ?? throw new NotFoundException("Movie", proiezione.MovieId);
        Sala sala = await _contesto.Sale.FindAsync(proiezione.SalaId)
            ?? throw new NotFoundException("Sala", proiezione.SalaId);
        TipologiaSala tipologiaSala = await _contesto.TipologieSala.FindAsync(sala.TipologiaSalaId)
            ?? throw new NotFoundException("TipologiaSala", sala.TipologiaSalaId);

        // Crea l'acquisto
        Acquisto acquisto = new Acquisto();
        acquisto.UtenteId = utenteId;
        acquisto.ProiezioneId = proiezione.Id;
        acquisto.NumeroBiglietti = dto.NumeroBiglietti;
        acquisto.OrarioCreazione = DateTimeOffset.UtcNow;
        acquisto.MetodoPagamento = dto.MetodoPagamento;
        acquisto.PrezzoFinale = Calcoli.CalcolaPrezzoFinale(
            movie.PrezzoMovie,
            tipologiaSala.MaggiorazionePrezzo,
            dto.NumeroBiglietti,
            utente,
            dto.MetodoPagamento);

        // Calcola il prezzo finale lato server

        // Salva nel DB
        _contesto.Acquisti.Add(acquisto);
        await _contesto.SaveChangesAsync();

        // Crea DTO da restituire
        DtoAcquisto risultato = new DtoAcquisto();
        risultato.Id = acquisto.Id;
        risultato.ProiezioneId = acquisto.ProiezioneId;
        risultato.UtenteId = acquisto.UtenteId;
        risultato.NumeroBiglietti = acquisto.NumeroBiglietti;
        risultato.PrezzoFinale = acquisto.PrezzoFinale;
        risultato.OrarioCreazione = acquisto.OrarioCreazione;
        risultato.MetodoPagamento = acquisto.MetodoPagamento;

        return risultato;
    }

    public async Task<DtoAcquisto?> ModificaAsync(string id, DtoCreazioneAcquisto dto)
    {
        Acquisto acquistoEsistente = await _contesto.Acquisti.FindAsync(id)
        ?? throw new NotFoundException("Acquisto", id);

        acquistoEsistente.NumeroBiglietti = dto.NumeroBiglietti;

        Proiezione proiezione = await _contesto.Proiezioni.FindAsync(dto.ProiezioneId)
            ?? throw new NotFoundException("Proiezione", dto.ProiezioneId);
        Movie movie = await _contesto.Movies.FindAsync(proiezione.MovieId)
            ?? throw new NotFoundException("Movie", proiezione.MovieId);
        Sala sala = await _contesto.Sale.FindAsync(proiezione.SalaId)
            ?? throw new NotFoundException("Sala", proiezione.SalaId);
        Utente utente = await _contesto.Utenti.FindAsync(acquistoEsistente.UtenteId)
            ?? throw new NotFoundException("Utente", acquistoEsistente.UtenteId);
        TipologiaSala tipologiaSala = await _contesto.TipologieSala.FindAsync(sala.TipologiaSalaId)
            ?? throw new NotFoundException("TipologiaSala", sala.TipologiaSalaId);

        // Aggiorna prezzo dal film
        acquistoEsistente.PrezzoFinale = Calcoli.CalcolaPrezzoFinale(
            movie.PrezzoMovie,
            tipologiaSala.MaggiorazionePrezzo,
            acquistoEsistente.NumeroBiglietti,
            utente,
            acquistoEsistente.MetodoPagamento);

        await _contesto.SaveChangesAsync();

        DtoAcquisto risultato = new DtoAcquisto
        {
            Id = acquistoEsistente.Id,
            UtenteId = acquistoEsistente.UtenteId,
            ProiezioneId = acquistoEsistente.ProiezioneId,
            NumeroBiglietti = acquistoEsistente.NumeroBiglietti,
            PrezzoFinale = acquistoEsistente.PrezzoFinale,
            OrarioCreazione = acquistoEsistente.OrarioCreazione,
            MetodoPagamento = acquistoEsistente.MetodoPagamento
        };

        return risultato;
    }

    public async Task<bool> EliminazioneAsync(string id)
    {
        Acquisto? acquisto = await _contesto.Acquisti.FindAsync(id);

        if (acquisto == null)
        {
            return false;
        }

        _contesto.Acquisti.Remove(acquisto);
        await _contesto.SaveChangesAsync();

        return true;
    }
}