using Microsoft.EntityFrameworkCore;
using NuovoCinemaParadiso.Data;
using NuovoCinemaParadiso.Dtos;
using NuovoCinemaParadiso.Models;

namespace NuovoCinemaParadiso.Services;

public class ProiezioneService
{
    private readonly ContestoDb _contesto;
    public ProiezioneService(ContestoDb contesto)
    {
        _contesto = contesto;
    }

    public async Task<List<DtoProiezione>> OttieniTuttoAsync()
    {
        List<Proiezione> proiezioni = await _contesto.Proiezioni.ToListAsync();
        List<DtoProiezione> risultato = new List<DtoProiezione>();

        for (int i = 0; i < proiezioni.Count; i++)
        {
            Proiezione proiezioneCorrente = proiezioni[i];

            Movie? film = await _contesto.Movies.FindAsync(proiezioneCorrente.FilmId);
            Sala? sala = await _contesto.Sale.FindAsync(proiezioneCorrente.SalaId);

            DtoProiezione dto = new DtoProiezione();
            dto.Id = proiezioneCorrente.Id;
            dto.OrarioInizio = proiezioneCorrente.OrarioInizio;
            dto.OrarioFine = proiezioneCorrente.OrarioFine;
            dto.NomeFilm = film?.Titolo ?? "";
            dto.NomeSala = sala?.Nome ?? "";

            risultato.Add(dto);
        }
        return risultato;
    }
}