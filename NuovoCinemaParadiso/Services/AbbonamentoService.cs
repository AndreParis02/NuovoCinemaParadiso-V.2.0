using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using NuovoCinemaParadiso.Data;
using NuovoCinemaParadiso.Dtos;
using NuovoCinemaParadiso.Models;

namespace NuovoCinemaParadiso.Services;

public class AbbonamentoService
{
    private readonly ContestoDb _contesto;
    public AbbonamentoService(ContestoDb contesto)
    {
        _contesto = contesto;
    }

    public async Task<DtoAbbonamento> CreazioneAsync(DtoCreazioneAbbonamento dto)
    {
        Abbonamento abbonamento = new Abbonamento();
        abbonamento.Id         = dto.Id;
        abbonamento.Nome       = dto.Nome;
        abbonamento.DataInizio = DateTime.UtcNow;
        abbonamento.Durata     = dto.Durata;
        abbonamento.DataFine   = dto.DataFine;
        abbonamento.Prezzo     = dto.Prezzo;
        abbonamento.Sconto     = dto.Sconto;

        _contesto.Abbonamenti.Add(abbonamento);
        await _contesto.SaveChangesAsync();

        DtoCreazioneAbbonamento risultato = new DtoAbbonamento();
        risultato.Id         = log.IdUtente;
        risultato.Nome       = log.NomeAzione;
        risultato.DataInizio = abbonamento.DataInizio.ToLocalTime();
        risultato.Durata     = abbonamento.Durata;
        risultato.DataFine   = abbonamento.DataInizio.AddMonths(Durata);
        risultato.Prezzo     = log.TimeStamp.ToLocalTime();
        risultato.Sconto     = dto.Sconto;

        return risultato;
    }
}