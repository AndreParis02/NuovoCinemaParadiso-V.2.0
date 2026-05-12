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
            if(proiezioneCorrente.Attivo)
            {
                Movie? film = await _contesto.Movies.FindAsync(proiezioneCorrente.MovieId);
                Sala? sala = await _contesto.Sale.FindAsync(proiezioneCorrente.SalaId);

                DtoProiezione dto = new DtoProiezione();
                dto.Id = proiezioneCorrente.Id;
                dto.DataProiezione = proiezioneCorrente.DataProiezione;
                dto.MovieId = proiezioneCorrente.MovieId;
                dto.SalaId = proiezioneCorrente.SalaId;
                dto.TurnoId = proiezioneCorrente.TurnoId;
                dto.Attivo = proiezioneCorrente.Attivo;
                risultato.Add(dto);
            }
        }
        return risultato;
    }

        public async Task<List<DtoProiezione>> OttieniStoricoAsync()
    {
        List<Proiezione> proiezioni = await _contesto.Proiezioni.ToListAsync();
        List<DtoProiezione> risultato = new List<DtoProiezione>();

        for (int i = 0; i < proiezioni.Count; i++)
        {
            Proiezione proiezioneCorrente = proiezioni[i];

            Movie? film = await _contesto.Movies.FindAsync(proiezioneCorrente.MovieId);
            Sala? sala = await _contesto.Sale.FindAsync(proiezioneCorrente.SalaId);

            DtoProiezione dto = new DtoProiezione();
            dto.Id = proiezioneCorrente.Id;
            dto.DataProiezione = proiezioneCorrente.DataProiezione;
            dto.MovieId = proiezioneCorrente.MovieId;
            dto.SalaId = proiezioneCorrente.SalaId;
            dto.TurnoId = proiezioneCorrente.TurnoId;
            dto.Attivo = proiezioneCorrente.Attivo;
            risultato.Add(dto);
        }
        return risultato;
    }

    public async Task<DtoProiezione?> OttieniTramiteIdAsync(string id)
    {
        Proiezione? proiezione = await _contesto.Proiezioni.FindAsync(id);
        if (proiezione == null)
        {
            return null;
        }
       
        Movie? film = await _contesto.Movies.FindAsync(proiezione.MovieId);
        Sala? sala = await _contesto.Sale.FindAsync(proiezione.SalaId);
        Turno? turno = await _contesto.Turni.FindAsync(proiezione.TurnoId);

        
        DtoProiezione risultato = new DtoProiezione();
        risultato.Id = proiezione.Id;
        risultato.DataProiezione = proiezione.DataProiezione;
        risultato.MovieId = proiezione.MovieId;
        risultato.SalaId = proiezione.SalaId;
        risultato.TurnoId = proiezione.TurnoId;
        risultato.Attivo = proiezione.Attivo;
        return risultato;
    }

    public async Task<List<DtoProiezione>> OttieniTramiteMovieAsync (string movieId)
    {
        List<DtoProiezione> risultato = new List<DtoProiezione>();
        List<Proiezione> proiezioni = await _contesto.Proiezioni.ToListAsync();

        for (int i = 0; i < proiezioni.Count; i++)
        {
            Proiezione proiezioneCorrente = proiezioni[i];
            if (proiezioneCorrente.MovieId == movieId)
            {
               DtoProiezione dto = new DtoProiezione();
                dto.Id = proiezioneCorrente.Id;
                dto.DataProiezione = proiezioneCorrente.DataProiezione;
                dto.MovieId = proiezioneCorrente.MovieId;
                dto.SalaId = proiezioneCorrente.SalaId;
                dto.TurnoId = proiezioneCorrente.TurnoId;
                dto.Attivo = proiezioneCorrente.Attivo;
                risultato.Add(dto);
            }

            
        }
        return risultato;
    }

    public async Task<List<DtoProiezione>> OttieniTramiteSalaAsync(string salaId)
    {
        List<DtoProiezione> risultato = new List<DtoProiezione>();
        List<Proiezione> proiezioni = await _contesto.Proiezioni.ToListAsync();

        for (int i = 0; i < proiezioni.Count; i++)
        {
            Proiezione proiezioneCorrente = proiezioni[i];
            if (proiezioneCorrente.SalaId == salaId)
            {
                DtoProiezione dto = new DtoProiezione();
                dto.Id = proiezioneCorrente.Id;
                dto.DataProiezione = proiezioneCorrente.DataProiezione;
                dto.MovieId = proiezioneCorrente.MovieId;
                dto.SalaId = proiezioneCorrente.SalaId;
                dto.TurnoId = proiezioneCorrente.TurnoId;
                dto.Attivo = proiezioneCorrente.Attivo;
                risultato.Add(dto);
            }
        }
        return risultato;
    }

    public async Task<List<DtoProiezione>> OttieniTramiteTurnoAsync(string turnoId)
    {
        List<DtoProiezione> risultato = new List<DtoProiezione>();
        List<Proiezione> proiezioni = await _contesto.Proiezioni.ToListAsync();

        for(int i = 0; i < proiezioni.Count; i++)
        {
            Proiezione proiezioneCorrente = proiezioni[i];
            if (proiezioneCorrente.TurnoId == turnoId)
            {
                DtoProiezione dto = new DtoProiezione();
                dto.Id = proiezioneCorrente.Id;
                dto.DataProiezione = proiezioneCorrente.DataProiezione;
                dto.MovieId = proiezioneCorrente.MovieId;
                dto.SalaId = proiezioneCorrente.SalaId;
                dto.TurnoId = proiezioneCorrente.TurnoId;
                dto.Attivo = proiezioneCorrente.Attivo;
                risultato.Add(dto);
            }
        }
        return risultato;

    }

    public async Task<DtoProiezione?> CreazioneAsync(DtoCreazioneProiezione dto)
    {
        Proiezione proiezione = new Proiezione();

        
        proiezione.DataProiezione = dto.DataProiezione;
        proiezione.MovieId = dto.MovieId;
        proiezione.SalaId = dto.SalaId;
        proiezione.TurnoId = dto.TurnoId;
        
        _contesto.Proiezioni.Add(proiezione);
        await _contesto.SaveChangesAsync();

        Movie? film = await _contesto.Movies.FindAsync(proiezione.MovieId);
        Sala? sala = await _contesto.Sale.FindAsync(proiezione.SalaId);
        Turno? turno = await _contesto.Turni.FindAsync(proiezione.TurnoId);

        
        DtoProiezione risultato = new DtoProiezione();
        risultato.Id = proiezione.Id;
        risultato.DataProiezione = proiezione.DataProiezione;
        risultato.MovieId = proiezione.MovieId;
        risultato.SalaId = proiezione.SalaId;
        risultato.TurnoId = proiezione.TurnoId;
        risultato.Attivo = proiezione.Attivo;
        return risultato;
    }

    public async Task<DtoProiezione?> ModificaAsync(string id, DtoCreazioneProiezione dto)
    {
        Proiezione? proiezione = await _contesto.Proiezioni.FindAsync(id);

        if (proiezione == null)
        {
            return null;
        }

        proiezione.DataProiezione = dto.DataProiezione;
        proiezione.MovieId = dto.MovieId;
        proiezione.SalaId = dto.SalaId;
        proiezione.TurnoId = dto.TurnoId;

        await _contesto.SaveChangesAsync();

        Movie? film = await _contesto.Movies.FindAsync(proiezione.MovieId);
        Sala? sala = await _contesto.Sale.FindAsync(proiezione.SalaId);
        Turno? turno = await _contesto.Turni.FindAsync(proiezione.TurnoId);

        DtoProiezione risultato = new DtoProiezione();
        risultato.Id = proiezione.Id;
        risultato.DataProiezione = proiezione.DataProiezione;
        risultato.MovieId = proiezione.MovieId;
        risultato.SalaId = proiezione.SalaId;
        risultato.TurnoId = proiezione.TurnoId;
        risultato.Attivo = proiezione.Attivo;
        return risultato;


    }

    public async Task<bool> EliminaAsync(string id)
    {
        Proiezione? proiezione = await _contesto.Proiezioni.FindAsync(id);

        if (proiezione == null)
        {
            return false;
        }

        proiezione.Attivo = false;
        await _contesto.SaveChangesAsync();
        
        return true;
    }
}