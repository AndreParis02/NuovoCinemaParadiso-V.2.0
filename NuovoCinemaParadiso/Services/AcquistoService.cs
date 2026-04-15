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

    public async Task<List<DtoAcquisto>> OttieniTutto(string utenteId)
    {
        List<Acquisto> acquisti = await _contesto.Acquisti.ToListAsync();

        List<DtoAcquisto> risultato = new List<DtoAcquisto>();

        for (int i = 0; i < acquisti.Count; i++)
        {
            Acquisto acquistoCorrente = acquisti[i];
            Movie? movie = await _contesto.Movies.FindAsync(acquistoCorrente.MovieId);
            Sala? sala = await _contesto.Sale.FindAsync(acquistoCorrente.SalaId);
            TipologiaSala? tipologiaSala = await _contesto.TipologieSala.FindAsync(sala.TipologiaSalaId);
            Utente? utente = await _contesto.Utenti.FindAsync(acquistoCorrente.UtenteId);

            if (acquistoCorrente.UtenteId == utenteId)
            {
                DtoAcquisto dto = new DtoAcquisto();
                dto.Id = acquistoCorrente.Id;
                dto.MovieId = acquistoCorrente.MovieId;
                dto.Titolo = movie.Titolo;
                dto.SalaId = acquistoCorrente.SalaId;
                dto.Nome = sala.Nome;
                dto.PrezzoFinale = Calcoli.CalcolaPrezzoFinale(movie.PrezzoMovie, tipologiaSala.MaggiorazionePrezzo, acquistoCorrente.NumeroBiglietti, utente);
                dto.OrarioCreazione = acquistoCorrente.OrarioCreazione;
                dto.NumeroBiglietti = acquistoCorrente.NumeroBiglietti;

                risultato.Add(dto);
            }
        }

        return risultato;
    }
    
    public async Task<DtoAcquisto> OttieniTramiteIdAsync(string id, string utenteId)
    {
        Acquisto? acquisto = await _contesto.Acquisti.FindAsync(id);
        Movie? movie = await _contesto.Movies.FindAsync(acquisto.MovieId);
        Sala? sala = await _contesto.Sale.FindAsync(acquisto.SalaId);
        TipologiaSala? tipologiaSala = await _contesto.TipologieSala.FindAsync(sala.TipologiaSalaId);
        Utente? utente = await _contesto.Utenti.FindAsync(acquisto.UtenteId);

        if (acquisto == null)
        {
            return null;
        }

        if (acquisto.UtenteId != utenteId)
        {
            return null;
        }

        DtoAcquisto dto = new DtoAcquisto();
        dto.Id = acquisto.Id;
        dto.MovieId = acquisto.MovieId;
        dto.Titolo = movie.Titolo;
        dto.SalaId = acquisto.SalaId;
        dto.Nome = sala.Nome;
        dto.PrezzoFinale = Calcoli.CalcolaPrezzoFinale(movie.PrezzoMovie, tipologiaSala.MaggiorazionePrezzo, acquisto.NumeroBiglietti, utente);
        dto.OrarioCreazione = acquisto.OrarioCreazione;
        dto.NumeroBiglietti = acquisto.NumeroBiglietti;

        return dto;
    }

    public async Task<DtoAcquisto> CreazioneAsync(DtoCreazioneAcquisto dto, string utenteId)
    {
        // Recupera tutti gli acquisti (in memoria) per controllare duplicati
        List<Acquisto> tuttiGliAcquisti = await _contesto.Acquisti.ToListAsync();
        foreach (Acquisto a in tuttiGliAcquisti)
        {
            if (a.UtenteId == utenteId && a.MovieId == dto.MovieId && a.SalaId == dto.SalaId)
            {
                return null; // acquisto duplicato
            }
        }

        // Recupera le entità necessarie
        Movie movie = await _contesto.Movies.FindAsync(dto.MovieId);
        Sala sala = await _contesto.Sale.FindAsync(dto.SalaId);
        TipologiaSala tipologiaSala = await _contesto.TipologieSala.FindAsync(sala.TipologiaSalaId);
        Utente utente = await _contesto.Utenti.FindAsync(utenteId);

        // Calcola il prezzo finale lato server
        decimal prezzoFinale = Calcoli.CalcolaPrezzoFinale(
            movie.PrezzoMovie,
            tipologiaSala.MaggiorazionePrezzo,
            dto.NumeroBiglietti,
            utente
        );

        // Crea l'acquisto
        Acquisto acquisto = new Acquisto();
        acquisto.MovieId = dto.MovieId;
        acquisto.SalaId = dto.SalaId;
        acquisto.UtenteId = utenteId;
        acquisto.NumeroBiglietti = dto.NumeroBiglietti;
        acquisto.PrezzoFinale = prezzoFinale;
        acquisto.OrarioCreazione = DateTime.UtcNow;

        // Salva nel DB
        _contesto.Acquisti.Add(acquisto);
        await _contesto.SaveChangesAsync();

        // Crea DTO da restituire
        DtoAcquisto risultato = new DtoAcquisto();
        risultato.Id = acquisto.Id;
        risultato.MovieId = acquisto.MovieId;
        risultato.SalaId = acquisto.SalaId;
        risultato.UtenteId = acquisto.UtenteId;
        risultato.NumeroBiglietti = acquisto.NumeroBiglietti;
        risultato.PrezzoFinale = acquisto.PrezzoFinale;
        risultato.Titolo = movie.Titolo;
        risultato.Nome = sala.Nome;
        risultato.NomeCompleto = utente.NomeCompleto;
        risultato.OrarioCreazione = acquisto.OrarioCreazione;

        return risultato;
    }

    public async Task<DtoAcquisto?> ModificaAsync(string id, DtoCreazioneAcquisto dto)
    {
        Acquisto? acquistoEsistente = await _contesto.Acquisti.FindAsync(id);

        // Aggiorna dati
        acquistoEsistente.MovieId = dto.MovieId;
        acquistoEsistente.SalaId = dto.SalaId;
        acquistoEsistente.NumeroBiglietti = dto.NumeroBiglietti;

        // Recupera dati aggiornati
        Movie? movie = await _contesto.Movies.FindAsync(dto.MovieId);
        Sala? sala = await _contesto.Sale.FindAsync(dto.SalaId);
        Utente? utente = await _contesto.Utenti.FindAsync(acquistoEsistente.UtenteId);
        TipologiaSala? tipologiaSala = await _contesto.TipologieSala.FindAsync(sala.TipologiaSalaId);

        // Aggiorna prezzo dal film
        acquistoEsistente.PrezzoFinale = Calcoli.CalcolaPrezzoFinale(movie.PrezzoMovie, tipologiaSala.MaggiorazionePrezzo, acquistoEsistente.NumeroBiglietti, utente);

        await _contesto.SaveChangesAsync();

        DtoAcquisto risultato = new DtoAcquisto
        {
            Id = acquistoEsistente.Id,
            MovieId = acquistoEsistente.MovieId,
            SalaId = acquistoEsistente.SalaId,
            UtenteId = acquistoEsistente.UtenteId,
            NumeroBiglietti = acquistoEsistente.NumeroBiglietti,
            PrezzoFinale = acquistoEsistente.PrezzoFinale,
            Titolo = movie?.Titolo,
            Nome = sala?.Nome,
            NomeCompleto = utente?.NomeCompleto,
            OrarioCreazione = acquistoEsistente.OrarioCreazione
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
