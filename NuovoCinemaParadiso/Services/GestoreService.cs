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
        List<DtoLogAzioni> risultati = new List<DtoLogAzioni>();
        foreach (LogAzioni log in logs)
        {
            DtoLogAzioni risultato = new DtoLogAzioni
            {
                Id = log.Id,
                IdUtente = log.IdUtente,
                NomeAzione = log.NomeAzione,
                Effettuato = log.Effettuato,
                Messaggio = log.Messaggio,
                TimeStamp = log.TimeStamp
            };
            risultati.Add(risultato);
        }

        return risultati;
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
        Biglietto biglietto = new Biglietto();
        var proiezione = await _contesto.Proiezioni.FindAsync(biglietto.ProiezioneId)
            ?? throw new Exception("Proiezione non trovato");
        var movie = await _contesto.Movies.FindAsync(proiezione.MovieId)
            ?? throw new Exception("Movie non trovato");
        var sala = await _contesto.Sale.FindAsync(proiezione.SalaId)
            ?? throw new Exception("Sala non trovato");
        var tipologiaSala = await _contesto.TipologieSala.FindAsync(sala.TipologiaSalaId)
            ?? throw new Exception("TipologiaSala non trovato");
        var turno = await _contesto.Turni.FindAsync(proiezione.TurnoId)
            ?? throw new Exception("Turno non trovato");
        List<DtoBiglietto> risultato = new List<DtoBiglietto>();

        for (int i = 0; i < biglietti.Count; i++)
        {
            Biglietto bigliettoCorrente = biglietti[i];
            DtoBiglietto dto = new DtoBiglietto();
            dto.Id = bigliettoCorrente.Id;
            dto.ProiezioneId = bigliettoCorrente.ProiezioneId;
            dto.UtenteId = bigliettoCorrente.UtenteId;
            dto.PrezzoFinale = bigliettoCorrente.PrezzoFinale;
            dto.OrarioCreazione = bigliettoCorrente.OrarioCreazione;
            dto.NumeroBiglietti = bigliettoCorrente.NumeroBiglietti;
            dto.NomeSala = sala.Nome;
            dto.TitoloMovie = movie.Titolo;
            dto.NomeTipologiaSala = tipologiaSala.Nome;
            dto.OraInizio = turno.OraInizio;
            dto.DataProiezione = proiezione.DataProiezione;

            risultato.Add(dto);
        }
        return risultato;
    }
}