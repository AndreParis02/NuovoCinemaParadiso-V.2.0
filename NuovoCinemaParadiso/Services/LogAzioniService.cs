using NuovoCinemaParadiso.Data;
using NuovoCinemaParadiso.Dtos;
using NuovoCinemaParadiso.Models;


namespace NuovoCinemaParadiso.Services;

public class LogAzioniService
{
  private readonly ContestoDb _contesto;
  public LogAzioniService(ContestoDb contesto) 
  {
    _contesto = contesto; 
  }

  
   public async Task<DtoLogAzioni> SalvataggioLogAzioneAsync(DtoCreazioneLogAzioni dto)
    {
        LogAzioni log = new LogAzioni();

        log.IdUtente = dto.IdUtente;
        log.NomeAzione = dto.NomeAzione;
        log.Effettuato = dto.Effettuato;
        log.Messaggio = dto.Messaggio;
        log.TimeStamp = DateTimeOffset.UtcNow;

       

        _contesto.LogAzioni.Add(log);
        await _contesto.SaveChangesAsync();

        DtoLogAzioni risultato = new DtoLogAzioni();
        risultato.IdUtente = log.IdUtente;
        risultato.NomeAzione = log.NomeAzione;
        risultato.Effettuato = log.Effettuato;
        risultato.Messaggio = log.Messaggio;
        risultato.TimeStamp = log.TimeStamp;


        return risultato;
    }

}