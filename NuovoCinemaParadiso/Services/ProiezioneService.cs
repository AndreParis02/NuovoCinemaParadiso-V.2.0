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

        var risultato = await Task.WhenAll(proiezioni
            .Where(proiezioneCorrente => proiezioneCorrente.Attivo)
            .Select(async proiezioneCorrente =>
            {
                Movie? movie = await _contesto.Movies.FindAsync(proiezioneCorrente.MovieId)
                    ?? throw new NotFoundException("Movie", proiezioneCorrente.MovieId);
                Sala? sala = await _contesto.Sale.FindAsync(proiezioneCorrente.SalaId)
                    ?? throw new NotFoundException("Sala", proiezioneCorrente.SalaId);
                Turno? turno = await _contesto.Turni.FindAsync(proiezioneCorrente.TurnoId)
                    ?? throw new NotFoundException("Turno", proiezioneCorrente.TurnoId);

                return new DtoProiezione
                {
                    Id = proiezioneCorrente.Id,
                    DataProiezione = proiezioneCorrente.DataProiezione,
                    MovieId = proiezioneCorrente.MovieId,
                    TitoloMovie = movie.Titolo,
                    SalaId = proiezioneCorrente.SalaId,
                    NomeSala = sala.Nome,
                    TurnoId = proiezioneCorrente.TurnoId,
                    NomeTurno = turno.Nome,
                    Attivo = proiezioneCorrente.Attivo
                };
            }));

        return risultato.ToList();
    }

    public async Task<List<DtoProiezione>> OttieniStoricoAsync()
    {
        List<Proiezione> proiezioni = await _contesto.Proiezioni.ToListAsync();

        var risultato = await Task.WhenAll(proiezioni.Select(async proiezioneCorrente =>
        {
            Movie? movie = await _contesto.Movies.FindAsync(proiezioneCorrente.MovieId)
                   ?? throw new NotFoundException("Movie", proiezioneCorrente.MovieId);
            Sala? sala = await _contesto.Sale.FindAsync(proiezioneCorrente.SalaId)
                ?? throw new NotFoundException("Sala", proiezioneCorrente.SalaId);
            Turno? turno = await _contesto.Turni.FindAsync(proiezioneCorrente.TurnoId)
                ?? throw new NotFoundException("Turno", proiezioneCorrente.TurnoId);

            return new DtoProiezione
            {
                Id = proiezioneCorrente.Id,
                DataProiezione = proiezioneCorrente.DataProiezione,
                MovieId = proiezioneCorrente.MovieId,
                TitoloMovie = movie.Titolo,
                SalaId = proiezioneCorrente.SalaId,
                NomeSala = sala.Nome,
                TurnoId = proiezioneCorrente.TurnoId,
                NomeTurno = turno.Nome,
                Attivo = proiezioneCorrente.Attivo
            };
        }));

        return risultato.ToList();
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
        List<Proiezione> proiezioni = await _contesto.Proiezioni.ToListAsync();

        var risultato = await Task.WhenAll(proiezioni
            .Where(proiezioneCorrente => proiezioneCorrente.MovieId == movieId)
            .Select(async proiezioneCorrente =>
            {
                Movie? movie = await _contesto.Movies.FindAsync(proiezioneCorrente.MovieId)
                       ?? throw new NotFoundException("Movie", proiezioneCorrente.MovieId);
                Sala? sala = await _contesto.Sale.FindAsync(proiezioneCorrente.SalaId)
                    ?? throw new NotFoundException("Sala", proiezioneCorrente.SalaId);
                Turno? turno = await _contesto.Turni.FindAsync(proiezioneCorrente.TurnoId)
                    ?? throw new NotFoundException("Turno", proiezioneCorrente.TurnoId);

                return new DtoProiezione
                {
                    Id = proiezioneCorrente.Id,
                    DataProiezione = proiezioneCorrente.DataProiezione,
                    MovieId = proiezioneCorrente.MovieId,
                    TitoloMovie = movie.Titolo,
                    SalaId = proiezioneCorrente.SalaId,
                    NomeSala = sala.Nome,
                    TurnoId = proiezioneCorrente.TurnoId,
                    NomeTurno = turno.Nome,
                    Attivo = proiezioneCorrente.Attivo
                };
            }));

        return risultato.ToList();
    }

    public async Task<List<DtoProiezione>> OttieniTramiteSalaAsync(string salaId)
    {
        List<Proiezione> proiezioni = await _contesto.Proiezioni.ToListAsync();

        var risultato = await Task.WhenAll(proiezioni
            .Where(proiezioneCorrente => proiezioneCorrente.SalaId == salaId)
            .Select(async proiezioneCorrente =>
            {
                Movie? movie = await _contesto.Movies.FindAsync(proiezioneCorrente.MovieId)
                       ?? throw new NotFoundException("Movie", proiezioneCorrente.MovieId);
                Sala? sala = await _contesto.Sale.FindAsync(proiezioneCorrente.SalaId)
                    ?? throw new NotFoundException("Sala", proiezioneCorrente.SalaId);
                Turno? turno = await _contesto.Turni.FindAsync(proiezioneCorrente.TurnoId)
                    ?? throw new NotFoundException("Turno", proiezioneCorrente.TurnoId);

                return new DtoProiezione
                {
                    Id = proiezioneCorrente.Id,
                    DataProiezione = proiezioneCorrente.DataProiezione,
                    MovieId = proiezioneCorrente.MovieId,
                    TitoloMovie = movie.Titolo,
                    SalaId = proiezioneCorrente.SalaId,
                    NomeSala = sala.Nome,
                    TurnoId = proiezioneCorrente.TurnoId,
                    NomeTurno = turno.Nome,
                    Attivo = proiezioneCorrente.Attivo
                };
            }));

        return risultato.ToList();
    }

    public async Task<List<DtoProiezione>> OttieniTramiteTurnoAsync(string turnoId)
    {
        List<Proiezione> proiezioni = await _contesto.Proiezioni.ToListAsync();

        var risultato = await Task.WhenAll(proiezioni
            .Where(proiezioneCorrente => proiezioneCorrente.TurnoId == turnoId)
            .Select(async proiezioneCorrente =>
            {
                Movie? movie = await _contesto.Movies.FindAsync(proiezioneCorrente.MovieId)
                       ?? throw new NotFoundException("Movie", proiezioneCorrente.MovieId);
                Sala? sala = await _contesto.Sale.FindAsync(proiezioneCorrente.SalaId)
                    ?? throw new NotFoundException("Sala", proiezioneCorrente.SalaId);
                Turno? turno = await _contesto.Turni.FindAsync(proiezioneCorrente.TurnoId)
                    ?? throw new NotFoundException("Turno", proiezioneCorrente.TurnoId);

                return new DtoProiezione
                {
                    Id = proiezioneCorrente.Id,
                    DataProiezione = proiezioneCorrente.DataProiezione,
                    MovieId = proiezioneCorrente.MovieId,
                    TitoloMovie = movie.Titolo,
                    SalaId = proiezioneCorrente.SalaId,
                    NomeSala = sala.Nome,
                    TurnoId = proiezioneCorrente.TurnoId,
                    NomeTurno = turno.Nome,
                    Attivo = proiezioneCorrente.Attivo
                };
            }));

        return risultato.ToList();

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
        Movie? film = await _contesto.Movies.FindAsync(dto.MovieId);
        Sala? sala = await _contesto.Sale.FindAsync(dto.SalaId);
        Turno? turno = await _contesto.Turni.FindAsync(dto.TurnoId);
        
        if (proiezione == null)
        {
            return false;
        }
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

        await _contesto.SaveChangesAsync();

        return true;
    }

    public async Task<bool> EliminaAsync(string id)
    {
        Proiezione? proiezione = await _contesto.Proiezioni.FindAsync(id);
        List<DtoBiglietto> biglietti = await _bigliettoService.OttieniTramiteProiezioneAsync(id);

        await Task.WhenAll(biglietti.Select(biglietto => _bigliettoService.EliminazioneAsync(biglietto.Id)));

        if (proiezione == null)
        {
            return false;
        }

        proiezione.Attivo = false; // <-- ci si ricollega al commento sul controller
        await _contesto.SaveChangesAsync();

        return true;
    }
}