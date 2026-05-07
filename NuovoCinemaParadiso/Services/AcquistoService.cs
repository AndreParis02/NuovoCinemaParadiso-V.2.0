using Microsoft.EntityFrameworkCore;
using NuovoCinemaParadiso.Data;
using NuovoCinemaParadiso.Dtos;
using NuovoCinemaParadiso.Models;
using NuovoCinemaParadiso.Helpers;

namespace NuovoCinemaParadiso.Services;

public class AcquistoService
{
    private readonly ContestoDb _contesto;
    public AcquistoService(ContestoDb contesto)
    {
        _contesto = contesto;
    }

    public async Task<(List<DtoAcquisto>? Dati, string? Errore)> OttieniTutto(string utenteId)
    {
        List<Acquisto> acquisti = await _contesto.Acquisti.ToListAsync();
        List<DtoAcquisto> risultato = new List<DtoAcquisto>();

        foreach (var acquistoCorrente in acquisti)
        {
            if (acquistoCorrente.UtenteId == utenteId)
            {
                // Recupero entità per i calcoli
                var utente = await _contesto.Utenti.FindAsync(acquistoCorrente.UtenteId);
                var proiezione = await _contesto.Proiezioni.FindAsync(acquistoCorrente.ProiezioneId);
                
                if (utente == null || proiezione == null) continue; // Salta se mancano dati integri

                var movie = await _contesto.Movies.FindAsync(proiezione.MovieId);
                var sala = await _contesto.Sale.FindAsync(proiezione.SalaId);
                
                if (movie == null || sala == null) continue;

                var tipologiaSala = await _contesto.TipologieSala.FindAsync(sala.TipologiaSalaId);
                if (tipologiaSala == null) continue;

                DtoAcquisto dto = new DtoAcquisto
                {
                    Id = acquistoCorrente.Id,
                    UtenteId = utente.Id,
                    ProiezioneId = acquistoCorrente.ProiezioneId,
                    OrarioCreazione = acquistoCorrente.OrarioCreazione,
                    NumeroBiglietti = acquistoCorrente.NumeroBiglietti,
                    MetodoPagamento = acquistoCorrente.MetodoPagamento,
                    PrezzoFinale = Calcoli.CalcolaPrezzoFinale(
                        movie.PrezzoMovie,
                        tipologiaSala.MaggiorazionePrezzo,
                        acquistoCorrente.NumeroBiglietti,
                        utente,
                        acquistoCorrente.MetodoPagamento)
                };
                risultato.Add(dto);
            }
        }
        return (risultato, null);
    }

    public async Task<(DtoAcquisto? Dto, string? Errore)> OttieniTramiteIdAsync(string id, string utenteId)
    {
        Acquisto? acquisto = await _contesto.Acquisti.FindAsync(id);
        if (acquisto == null) return (null, "Acquisto non trovato.");

        if (acquisto.UtenteId != utenteId)
            return (null, "Accesso negato: questo acquisto non ti appartiene.");

        
        var utente = await _contesto.Utenti.FindAsync(acquisto.UtenteId);
        var proiezione = await _contesto.Proiezioni.FindAsync(acquisto.ProiezioneId);
        if (utente == null || proiezione == null) return (null, "Dati della proiezione o utente non trovati.");

        var movie = await _contesto.Movies.FindAsync(proiezione.MovieId);
        var sala = await _contesto.Sale.FindAsync(proiezione.SalaId);
        if (movie == null || sala == null) return (null, "Dati del film o sala non trovati.");

        var tipologiaSala = await _contesto.TipologieSala.FindAsync(sala.TipologiaSalaId);
        if (tipologiaSala == null) return (null, "Tipologia sala non trovata.");

        DtoAcquisto dto = new DtoAcquisto
        {
            Id = acquisto.Id,
            UtenteId = utente.Id,
            ProiezioneId = acquisto.ProiezioneId,
            OrarioCreazione = acquisto.OrarioCreazione,
            NumeroBiglietti = acquisto.NumeroBiglietti,
            MetodoPagamento = acquisto.MetodoPagamento,
            PrezzoFinale = Calcoli.CalcolaPrezzoFinale(movie.PrezzoMovie, tipologiaSala.MaggiorazionePrezzo, acquisto.NumeroBiglietti, utente, acquisto.MetodoPagamento)
        };

        return (dto, null);
    }

    public async Task<(DtoAcquisto? Dto, string? Errore)> CreazioneAsync(DtoCreazioneAcquisto dto, string utenteId)
    {
        
        if (dto.NumeroBiglietti <= 0 || dto.NumeroBiglietti > 100)
            return (null, "Il numero di biglietti deve essere compreso tra 1 e 100.");

        var utente = await _contesto.Utenti.FindAsync(utenteId);
        if (utente == null) return (null, "Utente non trovato.");

        var proiezione = await _contesto.Proiezioni.FindAsync(dto.ProiezioneId);
        if (proiezione == null) return (null, "Proiezione non trovata.");

        var movie = await _contesto.Movies.FindAsync(proiezione.MovieId);
        var sala = await _contesto.Sale.FindAsync(proiezione.SalaId);
        if (movie == null || sala == null) return (null, "Dati del film o della sala non validi.");

        var tipologiaSala = await _contesto.TipologieSala.FindAsync(sala.TipologiaSalaId);
        if (tipologiaSala == null) return (null, "Tipologia sala non trovata.");

        Acquisto acquisto = new Acquisto
        {
            UtenteId = utenteId,
            ProiezioneId = proiezione.Id,
            NumeroBiglietti = dto.NumeroBiglietti,
            OrarioCreazione = DateTimeOffset.UtcNow,
            MetodoPagamento = dto.MetodoPagamento,
            PrezzoFinale = Calcoli.CalcolaPrezzoFinale(movie.PrezzoMovie, tipologiaSala.MaggiorazionePrezzo, dto.NumeroBiglietti, utente, dto.MetodoPagamento)
        };

        _contesto.Acquisti.Add(acquisto);
        await _contesto.SaveChangesAsync();

        DtoAcquisto risultato = new DtoAcquisto
        {
            Id = acquisto.Id,
            ProiezioneId = acquisto.ProiezioneId,
            UtenteId = acquisto.UtenteId,
            NumeroBiglietti = acquisto.NumeroBiglietti,
            PrezzoFinale = acquisto.PrezzoFinale,
            OrarioCreazione = acquisto.OrarioCreazione,
            MetodoPagamento = acquisto.MetodoPagamento
        };

        return (risultato, null);
    }

    public async Task<(DtoAcquisto? Dto, string? Errore)> ModificaAsync(string id, DtoCreazioneAcquisto dto)
    {
        if (dto.NumeroBiglietti <= 0 || dto.NumeroBiglietti > 100)
            return (null, "Il numero di biglietti deve essere compreso tra 1 e 100.");

        var acquistoEsistente = await _contesto.Acquisti.FindAsync(id);
        if (acquistoEsistente == null) return (null, "Acquisto non trovato.");

        var proiezione = await _contesto.Proiezioni.FindAsync(dto.ProiezioneId);
        if (proiezione == null) return (null, "Proiezione non trovata.");

        var movie = await _contesto.Movies.FindAsync(proiezione.MovieId);
        var sala = await _contesto.Sale.FindAsync(proiezione.SalaId);
        var utente = await _contesto.Utenti.FindAsync(acquistoEsistente.UtenteId);
        var tipologiaSala = await _contesto.TipologieSala.FindAsync(sala?.TipologiaSalaId);

        if (movie == null || sala == null || utente == null || tipologiaSala == null)
            return (null, "Dati correlati all'acquisto non trovati o non validi.");

        acquistoEsistente.NumeroBiglietti = dto.NumeroBiglietti;
        acquistoEsistente.PrezzoFinale = Calcoli.CalcolaPrezzoFinale(movie.PrezzoMovie, tipologiaSala.MaggiorazionePrezzo, acquistoEsistente.NumeroBiglietti, utente, acquistoEsistente.MetodoPagamento);

        await _contesto.SaveChangesAsync();

        return (new DtoAcquisto {
            Id = acquistoEsistente.Id,
            UtenteId = acquistoEsistente.UtenteId,
            ProiezioneId = acquistoEsistente.ProiezioneId,
            NumeroBiglietti = acquistoEsistente.NumeroBiglietti,
            PrezzoFinale = acquistoEsistente.PrezzoFinale,
            OrarioCreazione = acquistoEsistente.OrarioCreazione,
            MetodoPagamento = acquistoEsistente.MetodoPagamento
        }, null);
    }

    public async Task<(bool Successo, string? Errore)> EliminazioneAsync(string id)
    {
        var acquisto = await _contesto.Acquisti.FindAsync(id);
        if (acquisto == null) return (false, "Acquisto non trovato.");

        _contesto.Acquisti.Remove(acquisto);
        await _contesto.SaveChangesAsync();
        return (true, null);
    }
}