using Microsoft.EntityFrameworkCore;
using NuovoCinemaParadiso.Dtos;
using NuovoCinemaParadiso.Data;
using NuovoCinemaParadiso.Models;

namespace NuovoCinemaParadiso.Services;

public class GenereMovieService
{
    private readonly ContestoDb _contesto;
    public GenereMovieService(ContestoDb contesto)
    {
        _contesto = contesto;
    }
    public async Task<List<DtoGenereMovie>> OttieniTuttoAsync()
    {
        List<GenereMovie> generiMovies = await _contesto.GeneriMovies.ToListAsync();

        return generiMovies
            .Select(genereCorrente => new DtoGenereMovie
            {
                Id = genereCorrente.Id,
                Genere = genereCorrente.Genere
            })
            .ToList();
    }

    public async Task<DtoGenereMovie?> OttieniTramiteIdAsync(string id)
    {
        GenereMovie? genereMovie = await _contesto.GeneriMovies.FindAsync(id);

        if (genereMovie == null)
        {
            return null;
        }

        DtoGenereMovie risultato = new DtoGenereMovie();
        risultato.Id = genereMovie.Id;
        risultato.Genere = genereMovie.Genere;

        return risultato;
    }

   
}