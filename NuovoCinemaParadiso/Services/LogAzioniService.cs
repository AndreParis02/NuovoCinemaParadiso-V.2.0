using NuovoCinemaParadiso.Data;
using NuovoCinemaParadiso.Dtos;
using NuovoCinemaParadiso.Models;
using Microsoft.EntityFrameworkCore;


namespace NuovoCinemaParadiso.Services;

public class LogAzioniService
{
  private readonly ContestoDb _contesto;
  public LogAzioniService(ContestoDb contesto) 
  {
    _contesto = contesto; 
  }

  
    public async Task SalvataggioLogAzioneAsync(DtoCreazioneLogAzioni dto)
    {
        LogAzioni log = new LogAzioni();

        log.IdUtente = dto.IdUtente;
        log.NomeAzione = dto.NomeAzione;
        log.Effettuato = dto.Effettuato;
        log.Messaggio = dto.Messaggio;
        log.TimeStamp = DateTimeOffset.UtcNow;

      

        _contesto.LogAzioni.Add(log);
        await _contesto.SaveChangesAsync();
    }

    public async Task SalvataggioLogAzioneAsync(string idUtente, string azione, bool effettuato)
  {
      string messaggio = "operazione fallita";
      if(effettuato) messaggio = "operazione eseguita";

      LogAzioni log = new LogAzioni();

      log.IdUtente = idUtente;
      log.NomeAzione = azione;
      log.Effettuato = effettuato;
      log.Messaggio = messaggio;
      log.TimeStamp = DateTimeOffset.UtcNow;

        _contesto.LogAzioni.Add(log);
      await _contesto.SaveChangesAsync();
  }

    public async Task<List<DtoLogAzioni>> LetturaLogAzioneAsync()
    {
        List<LogAzioni> logs= await _contesto.LogAzioni.ToListAsync();
        List<DtoLogAzioni> risultati = new List<DtoLogAzioni>();
        foreach (LogAzioni log in logs)
        {
          DtoLogAzioni risultato = new DtoLogAzioni();
          risultato.Id = log.Id;
          risultato.IdUtente = log.IdUtente;
          risultato.NomeAzione = log.NomeAzione;
          risultato.Effettuato = log.Effettuato;
          risultato.Messaggio = log.Messaggio;
          risultato.TimeStamp = log.TimeStamp;
          risultati.Add(risultato);
        }


        return risultati;
    }

}