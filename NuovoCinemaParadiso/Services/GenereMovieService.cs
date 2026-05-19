using Microsoft.EntityFrameworkCore;
using NuovoCinemaParadiso.Dtos;
using NuovoCinemaParadiso.Data;
using NuovoCinemaParadiso.Models;
using NuovoCinemaParadiso.Exceptions;

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
        List<DtoGenereMovie> risultato = new List<DtoGenereMovie>();

        List<GenereMovie> generiMovies = await _contesto.GeneriMovies.ToListAsync();

        for (int i = 0; i < generiMovies.Count; i++)
        {
            GenereMovie genereCorrente = generiMovies[i];

            DtoGenereMovie dto = new DtoGenereMovie();
            dto.Id = genereCorrente.Id;
            dto.Genere = genereCorrente.Genere;

            risultato.Add(dto);
        }

        return risultato;
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