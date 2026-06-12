using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NuovoCinemaParadiso.Data;
using NuovoCinemaParadiso.Dtos;
using NuovoCinemaParadiso.Models;
using NuovoCinemaParadiso.Exceptions;

namespace NuovoCinemaParadiso.Services;

public class GestoreService
{
    private readonly ContestoDb _contesto;
    private readonly UserManager<Utente> _gestioneUtenti;
    public GestoreService(ContestoDb contestoDb, UserManager<Utente> gestioneUtenti)
    {
        _contesto = contestoDb;
        _gestioneUtenti = gestioneUtenti;
    }

    public async Task<List<DtoLogAzioni>> LetturaLogAzioneAsync()
    {
        List<LogAzioni> logs = await _contesto.LogAzioni.ToListAsync();

        return logs
            .Select(log => new DtoLogAzioni
            {
                Id = log.Id,
                IdUtente = log.IdUtente,
                NomeAzione = log.NomeAzione,
                Effettuato = log.Effettuato,
                Messaggio = log.Messaggio,
                TimeStamp = log.TimeStamp
            })
            .ToList();
    }

    public async Task<DtoContoCinema> OttieniDatiContoAsync()
    {
        ContoCinema contoCinema = await _contesto.ContoCinema.FirstOrDefaultAsync()
            ?? throw new NotFoundException("Conto Cinema", "");

        DtoContoCinema dto = new DtoContoCinema();
        dto.Id = contoCinema.Id;
        dto.Iban = contoCinema.Iban;
        dto.TitolareConto = contoCinema.TitolareConto;
        dto.Saldo = contoCinema.Saldo;
        return dto;
    }
    public async Task<List<DtoBiglietto>> OttieniTuttiBigliettiAsync()
    {
        List<Biglietto> biglietti = await _contesto.Biglietti.ToListAsync();
        var risultato = await Task.WhenAll(biglietti.Select(async bigliettoCorrente =>
        {
            var proiezione = await _contesto.Proiezioni.FindAsync(bigliettoCorrente.ProiezioneId)
                ?? throw new Exception("Proiezione non trovata");

            var movie = await _contesto.Movies.FindAsync(proiezione.MovieId)
                ?? throw new Exception("Movie non trovato");

            var sala = await _contesto.Sale.FindAsync(proiezione.SalaId)
                ?? throw new Exception("Sala non trovata");

            var tipologiaSala = await _contesto.TipologieSala.FindAsync(sala.TipologiaSalaId)
                ?? throw new Exception("TipologiaSala non trovata");

            var turno = await _contesto.Turni.FindAsync(proiezione.TurnoId)
                ?? throw new Exception("Turno non trovato");

            return new DtoBiglietto
            {
                Id = bigliettoCorrente.Id,
                ProiezioneId = bigliettoCorrente.ProiezioneId,
                UtenteId = bigliettoCorrente.UtenteId,
                PrezzoFinale = bigliettoCorrente.PrezzoFinale,
                OrarioCreazione = bigliettoCorrente.OrarioCreazione,
                NumeroBiglietti = bigliettoCorrente.NumeroBiglietti,
                NomeSala = sala.Nome,
                TitoloMovie = movie.Titolo,
                NomeTipologiaSala = tipologiaSala.Nome,
                OraInizio = turno.OraInizio,
                DataProiezione = proiezione.DataProiezione
            };
        }));

        return risultato.ToList();
    }
}