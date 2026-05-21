using Microsoft.EntityFrameworkCore;
using NuovoCinemaParadiso.Data;
using NuovoCinemaParadiso.Dtos;
using NuovoCinemaParadiso.Models;

namespace NuovoCinemaParadiso.Services;

public class MovieService
{
    private readonly ContestoDb _contesto;

    private readonly GenereMovieService _genereMovieService;
    public MovieService(ContestoDb contesto, GenereMovieService genereMovieService)
    {
        _contesto = contesto;
        _genereMovieService = genereMovieService;
    }

    public async Task<List<DtoMovie>> OttieniTutto()
    {
        List<Movie> movies = await _contesto.Movies.ToListAsync();

        List<DtoMovie> risultato = new List<DtoMovie>();

        for (int i = 0; i < movies.Count; i++)
        {
            Movie movieCorrente = movies[i];
            GenereMovie? genereMovie = await _contesto.GeneriMovies.FindAsync(movieCorrente.GenereId);

            DtoMovie dto = new DtoMovie();
            dto.Id = movieCorrente.Id;
            dto.Titolo = movieCorrente.Titolo;
            dto.Descrizione = movieCorrente.Descrizione;
            dto.DurataMinuti = movieCorrente.DurataMinuti;
            dto.PrezzoMovie = movieCorrente.PrezzoMovie;
            dto.GenereId = movieCorrente.GenereId;
            dto.Genere = genereMovie?.Genere ?? "";

            risultato.Add(dto);
        }
        return risultato;
    }

    public async Task<DtoMovie?> OttieniTramiteIdAsync(string id)
    {
        
        Movie? movie = await _contesto.Movies.FindAsync(id);

        if (movie == null)
        {
            return null;
        }
        GenereMovie? genereMovie = await _contesto.GeneriMovies.FindAsync(movie.GenereId);

        DtoMovie risultato = new DtoMovie
        {
            Id = movie.Id,
            Titolo = movie.Titolo,
            Descrizione = movie.Descrizione,
            DurataMinuti = movie.DurataMinuti,
            PrezzoMovie = movie.PrezzoMovie,
            GenereId = movie.GenereId,
            Genere = genereMovie?.Genere ?? ""
        };
        return risultato;
    }

    public async Task<List<DtoMovie>> OttieniTramiteGenere(string genereId)
    {
        List<DtoMovie> risultato = new List<DtoMovie>();

        List<DtoMovie> movies = await OttieniTutto();

        foreach (var movie in movies)
        {
            if (movie.GenereId.Trim() == genereId)
            {
                risultato.Add(movie);
            }
        }
        return risultato;
    }

    public async Task<bool> CreazioneAsync(DtoCreazioneMovie dto)
    {

        if (await _genereMovieService.OttieniTramiteIdAsync(dto.GenereId) == null)
        {
            return false;
        }
        
        Movie movie = new Movie();

        movie.Titolo = dto.Titolo;
        movie.Descrizione = dto.Descrizione;
        movie.DurataMinuti = dto.DurataMinuti;
        movie.PrezzoMovie = dto.PrezzoMovie;
        movie.GenereId = dto.GenereId;

        _contesto.Movies.Add(movie);
        await _contesto.SaveChangesAsync();

        return true;
    }

    public async Task<bool> ModificaAsync(string id, DtoCreazioneMovie dto)
    {
        Movie? movieEsistente = await _contesto.Movies.FindAsync(id);

        if (await _genereMovieService.OttieniTramiteIdAsync(dto.GenereId) == null || movieEsistente == null)
        {
            return false;
        }

        movieEsistente.Titolo = dto.Titolo;
        movieEsistente.Descrizione = dto.Descrizione;
        movieEsistente.DurataMinuti = dto.DurataMinuti;
        movieEsistente.PrezzoMovie = dto.PrezzoMovie;
        movieEsistente.GenereId = dto.GenereId;

        await _contesto.SaveChangesAsync();
        
        return true;
    }

    public async Task<bool> EliminaAsync(string id)
    {
        Movie? movie = await _contesto.Movies.FindAsync(id);

        if (movie == null)
        {
            return false;
        }

        _contesto.Movies.Remove(movie);
        await _contesto.SaveChangesAsync();

        return true;
    }
}