using Microsoft.EntityFrameworkCore;
using NuovoCinemaParadiso.Data;
using NuovoCinemaParadiso.Dtos;
using NuovoCinemaParadiso.Models;
using NuovoCinemaParadiso.Exceptions;

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

        var risultato = await Task.WhenAll(movies
            .Where(movieCorrente => !movieCorrente.IsDeleted)
            .Select(async movieCorrente =>
            {
                GenereMovie? genereMovie = await _contesto.GeneriMovies.FindAsync(movieCorrente.GenereId);

                return new DtoMovie
                {
                    Id = movieCorrente.Id,
                    Titolo = movieCorrente.Titolo,
                    Descrizione = movieCorrente.Descrizione,
                    DurataMinuti = movieCorrente.DurataMinuti,
                    PrezzoMovie = movieCorrente.PrezzoMovie,
                    GenereId = movieCorrente.GenereId,
                    Genere = genereMovie?.Genere ?? ""
                };
            }));

        return risultato.ToList();
    }

        public async Task<List<DtoMovie>> OttieniTuttoStorico()
    {
        List<Movie> movies = await _contesto.Movies.ToListAsync();

        var risultato = await Task.WhenAll(movies.Select(async movieCorrente =>
        {
            GenereMovie? genereMovie = await _contesto.GeneriMovies.FindAsync(movieCorrente.GenereId);

            return new DtoMovie
            {
                Id = movieCorrente.Id,
                Titolo = movieCorrente.Titolo,
                Descrizione = movieCorrente.Descrizione,
                DurataMinuti = movieCorrente.DurataMinuti,
                PrezzoMovie = movieCorrente.PrezzoMovie,
                GenereId = movieCorrente.GenereId,
                Genere = genereMovie?.Genere ?? "",
                IsDeleted = movieCorrente.IsDeleted
            };
        }));

        return risultato.ToList();
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
            Genere = genereMovie?.Genere ?? "",
            IsDeleted = movie.IsDeleted
        };
        return risultato;
    }

    public async Task<List<DtoMovie>> OttieniTramiteGenere(string genereId)
    {
        List<DtoMovie> movies = await OttieniTutto();

        return movies
            .Where(movie => movie.GenereId.Trim() == genereId)
            .ToList();
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

        if (await _genereMovieService.OttieniTramiteIdAsync(dto.GenereId) == null)
        {
            Console.WriteLine("GENERE PROBLEMA");
            throw new NotFoundException("genere", dto.GenereId);

        }

        if (movieEsistente == null)
        {
            Console.WriteLine("PROBLEMA ESISTENZIALE");
            throw new NotFoundException("movie",id);
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
            return false;

        if(movie.IsDeleted)
            return false;

        movie.IsDeleted = true;

        List<Proiezione> Proiezioni = await _contesto.Proiezioni.Where(P => P.MovieId == movie.Id && P.DataProiezione >= DateOnly.FromDateTime(DateTime.Now)).ToListAsync();

        Proiezioni.ForEach(temp => temp.Attivo = false);

        await _contesto.SaveChangesAsync();

        return true;
    }
}