using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuovoCinemaParadiso.Data;
using NuovoCinemaParadiso.Dtos;
using NuovoCinemaParadiso.Models;
using NuovoCinemaParadiso.Services;
using NuovoCinemaParadiso.Exceptions;

namespace NuovoCinemaParadiso.Services;

public class ProiezioneService
{
    private readonly ContestoDb _contesto;
    private readonly BigliettoService _bigliettoService;
    public ProiezioneService(ContestoDb contesto, BigliettoService bigliettoService)
    {
        _contesto = contesto;
        _bigliettoService = bigliettoService;
    }

    public async Task<List<DtoProiezione>> OttieniTuttoAsync()
    {
        List<Proiezione> proiezioni = await _contesto.Proiezioni.ToListAsync();
        List<DtoProiezione> risultato = new List<DtoProiezione>();

        for (int i = 0; i < proiezioni.Count; i++)
        {
            Proiezione proiezioneCorrente = proiezioni[i];
            if (proiezioneCorrente.Attivo)
            {

                Movie? movie = await _contesto.Movies.FindAsync(proiezioneCorrente.MovieId)
                    ?? throw new NotFoundException("Movie", proiezioneCorrente.MovieId);
                Sala? sala = await _contesto.Sale.FindAsync(proiezioneCorrente.SalaId)
                    ?? throw new NotFoundException("Sala", proiezioneCorrente.SalaId);
                Turno? turno = await _contesto.Turni.FindAsync(proiezioneCorrente.TurnoId)
                    ?? throw new NotFoundException("Turno", proiezioneCorrente.TurnoId);

                DtoProiezione dto = new DtoProiezione();
                dto.Id = proiezioneCorrente.Id;
                dto.DataProiezione = proiezioneCorrente.DataProiezione;
                dto.MovieId = proiezioneCorrente.MovieId;
                dto.TitoloMovie = movie.Titolo;
                dto.SalaId = proiezioneCorrente.SalaId;
                dto.NomeSala = sala.Nome;
                dto.TurnoId = proiezioneCorrente.TurnoId;
                dto.NomeTurno = turno.Nome;
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

            Movie? movie = await _contesto.Movies.FindAsync(proiezioneCorrente.MovieId)
                   ?? throw new NotFoundException("Movie", proiezioneCorrente.MovieId);
            Sala? sala = await _contesto.Sale.FindAsync(proiezioneCorrente.SalaId)
                ?? throw new NotFoundException("Sala", proiezioneCorrente.SalaId);
            Turno? turno = await _contesto.Turni.FindAsync(proiezioneCorrente.TurnoId)
                ?? throw new NotFoundException("Turno", proiezioneCorrente.TurnoId);

            DtoProiezione dto = new DtoProiezione();
            dto.Id = proiezioneCorrente.Id;
            dto.DataProiezione = proiezioneCorrente.DataProiezione;
            dto.MovieId = proiezioneCorrente.MovieId;
            dto.TitoloMovie = movie.Titolo;
            dto.SalaId = proiezioneCorrente.SalaId;
            dto.NomeSala = sala.Nome;
            dto.TurnoId = proiezioneCorrente.TurnoId;
            dto.NomeTurno = turno.Nome;
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

        Movie? movie = await _contesto.Movies.FindAsync(proiezione.MovieId)
                   ?? throw new NotFoundException("Movie", proiezione.MovieId);
        Sala? sala = await _contesto.Sale.FindAsync(proiezione.SalaId)
            ?? throw new NotFoundException("Sala", proiezione.SalaId);
        Turno? turno = await _contesto.Turni.FindAsync(proiezione.TurnoId)
            ?? throw new NotFoundException("Turno", proiezione.TurnoId);



        DtoProiezione risultato = new DtoProiezione();
        risultato.Id = proiezione.Id;
        risultato.DataProiezione = proiezione.DataProiezione;
        risultato.MovieId = proiezione.MovieId;
        risultato.TitoloMovie = movie.Titolo;
        risultato.SalaId = proiezione.SalaId;
        risultato.NomeSala = sala.Nome;
        risultato.TurnoId = proiezione.TurnoId;
        risultato.NomeTurno = turno.Nome;
        risultato.Attivo = proiezione.Attivo;
        return risultato;
    }

    public async Task<List<DtoProiezione>> OttieniTramiteMovieAsync(string movieId)
    {
        List<DtoProiezione> risultato = new List<DtoProiezione>();
        List<Proiezione> proiezioni = await _contesto.Proiezioni.ToListAsync();

        for (int i = 0; i < proiezioni.Count; i++)
        {
            Proiezione proiezioneCorrente = proiezioni[i];
            Movie? movie = await _contesto.Movies.FindAsync(proiezioneCorrente.MovieId)
                   ?? throw new NotFoundException("Movie", proiezioneCorrente.MovieId);
            Sala? sala = await _contesto.Sale.FindAsync(proiezioneCorrente.SalaId)
                ?? throw new NotFoundException("Sala", proiezioneCorrente.SalaId);
            Turno? turno = await _contesto.Turni.FindAsync(proiezioneCorrente.TurnoId)
                ?? throw new NotFoundException("Turno", proiezioneCorrente.TurnoId);
            if (proiezioneCorrente.MovieId == movieId)
            {
                DtoProiezione dto = new DtoProiezione();
                dto.Id = proiezioneCorrente.Id;
                dto.DataProiezione = proiezioneCorrente.DataProiezione;
                dto.MovieId = proiezioneCorrente.MovieId;
                dto.TitoloMovie = movie.Titolo;
                dto.SalaId = proiezioneCorrente.SalaId;
                dto.NomeSala = sala.Nome;
                dto.TurnoId = proiezioneCorrente.TurnoId;
                dto.NomeTurno = turno.Nome;
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
            Movie? movie = await _contesto.Movies.FindAsync(proiezioneCorrente.MovieId)
                   ?? throw new NotFoundException("Movie", proiezioneCorrente.MovieId);
            Sala? sala = await _contesto.Sale.FindAsync(proiezioneCorrente.SalaId)
                ?? throw new NotFoundException("Sala", proiezioneCorrente.SalaId);
            Turno? turno = await _contesto.Turni.FindAsync(proiezioneCorrente.TurnoId)
                ?? throw new NotFoundException("Turno", proiezioneCorrente.TurnoId);
            if (proiezioneCorrente.SalaId == salaId)
            {
                DtoProiezione dto = new DtoProiezione();
                dto.Id = proiezioneCorrente.Id;
                dto.DataProiezione = proiezioneCorrente.DataProiezione;
                dto.MovieId = proiezioneCorrente.MovieId;
                dto.TitoloMovie = movie.Titolo;
                dto.SalaId = proiezioneCorrente.SalaId;
                dto.NomeSala = sala.Nome;
                dto.TurnoId = proiezioneCorrente.TurnoId;
                dto.NomeTurno = turno.Nome;
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

        for (int i = 0; i < proiezioni.Count; i++)
        {
            Proiezione proiezioneCorrente = proiezioni[i];
            Movie? movie = await _contesto.Movies.FindAsync(proiezioneCorrente.MovieId)
                   ?? throw new NotFoundException("Movie", proiezioneCorrente.MovieId);
            Sala? sala = await _contesto.Sale.FindAsync(proiezioneCorrente.SalaId)
                ?? throw new NotFoundException("Sala", proiezioneCorrente.SalaId);
            Turno? turno = await _contesto.Turni.FindAsync(proiezioneCorrente.TurnoId)
                ?? throw new NotFoundException("Turno", proiezioneCorrente.TurnoId);
            if (proiezioneCorrente.TurnoId == turnoId)
            {
                DtoProiezione dto = new DtoProiezione();
                dto.Id = proiezioneCorrente.Id;
                dto.DataProiezione = proiezioneCorrente.DataProiezione;
                dto.MovieId = proiezioneCorrente.MovieId;
                dto.TitoloMovie = movie.Titolo;
                dto.SalaId = proiezioneCorrente.SalaId;
                dto.NomeSala = sala.Nome;
                dto.TurnoId = proiezioneCorrente.TurnoId;
                dto.NomeTurno = turno.Nome;
                dto.Attivo = proiezioneCorrente.Attivo;
                risultato.Add(dto);
            }
        }
        return risultato;

    }

    public async Task<bool> CreazioneAsync(DtoCreazioneProiezione dto)
    {
        Proiezione proiezione = new Proiezione();

        Movie? film = await _contesto.Movies.FindAsync(dto.MovieId);
        Sala? sala = await _contesto.Sale.FindAsync(dto.SalaId);
        Turno? turno = await _contesto.Turni.FindAsync(dto.TurnoId);
        if (film == null)
        {
            throw new NotFoundException("Movie", dto.MovieId);
        }
        if (sala == null)
        {
            throw new NotFoundException("Sala", dto.SalaId);
        }
        if (turno == null)
        {
            throw new NotFoundException("Turno", dto.TurnoId);
        }

        proiezione.DataProiezione = dto.DataProiezione;
        proiezione.MovieId = dto.MovieId;
        proiezione.SalaId = dto.SalaId;
        proiezione.TurnoId = dto.TurnoId;
        proiezione.Attivo = true; // <- AGGIUNTA perché altrimenti andava direttamente nello storico e non nel get standard per le attive

        _contesto.Proiezioni.Add(proiezione);
        await _contesto.SaveChangesAsync();

        return true;
    }

    public async Task<bool> ModificaAsync(string id, DtoCreazioneProiezione dto)
    {
        Proiezione? proiezione = await _contesto.Proiezioni.FindAsync(id);

        if (proiezione == null)
        {
            return false;
        }

        proiezione.DataProiezione = dto.DataProiezione;
        proiezione.MovieId = dto.MovieId;
        proiezione.SalaId = dto.SalaId;
        proiezione.TurnoId = dto.TurnoId;

        await _contesto.SaveChangesAsync();

        return true;
    }

    public async Task<bool> EliminaAsync(string id)
    {
        Proiezione? proiezione = await _contesto.Proiezioni.FindAsync(id);
        List<DtoBiglietto> biglietti = await _bigliettoService.OttieniTramiteProiezioneAsync(id);

        foreach (DtoBiglietto biglietto in biglietti)
        {
            await _bigliettoService.EliminazioneAsync(biglietto.Id);
        }

        if (proiezione == null)
        {
            return false;
        }

        proiezione.Attivo = false; // <-- ci si ricollega al commento sul controller
        await _contesto.SaveChangesAsync();

        return true;
    }
}