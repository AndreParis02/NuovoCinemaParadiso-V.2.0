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
        List<LogAzioni> logs= await _contesto.LogAzioni.ToListAsync();
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
}