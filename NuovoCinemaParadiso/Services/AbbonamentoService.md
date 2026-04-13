```C#
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
        _contesto = contesto; // dependency injections per il database.
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

        DtoAbbonamento risultato = new DtoAbbonamento();
        risultato.Id         = abbonamento.Id;
        risultato.Nome       = abbonamento.NomeAzione;
        risultato.DataInizio = abbonamento.DataInizio.ToLocalTime();
        risultato.Durata     = abbonamento.Durata;
        risultato.DataFine   = abbonamento.DataInizio.AddMonths(abbonamento.Durata); // calcolo della durata dell'abbonamento
        risultato.Prezzo     = abbonamento.Prezzo
        risultato.Sconto     = abbonamento.Sconto;

        return risultato;
    }

    public async Task<DtoAbbonamento> ModificaAsync(string id, DtoCreazioneAbbonamento dto)
    {
        Abbonamento? abbonamento = await _contesto.Abbonamenti.FindAsync(id);

        if (abbonamento == null)
            return null;


        abbonamento.Nome    = dto.Nome;
        abbonamento.Durata  = dto.Durata;
        abbonamento.Prezzo  = dto.Prezzo;
        abbonamento.Sconto  = dto.Sconto;
        abbonamento.DataFine = abbonamento.DataInizio.AddMonths(dto.Durata); // ricalcolo della durata

        await _contesto.SaveChangesAsync();

        return new DtoAbboamento
        {
          Id             = abbonamento.Id,
          Nome           = abbonamento.Nome,
          DataInizio     = abbonamento.DataInizio.ToLocalTime(),
          Durata         = abbonamento.Durata,
          DataFine       = abbonamento.DataFine,
          Prezzo         = abbonamento.Prezzo,
          Sconto         = abbonamento.Sconto,
        };
    }
}

```