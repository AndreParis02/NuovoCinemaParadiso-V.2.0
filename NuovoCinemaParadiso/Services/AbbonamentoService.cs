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

    public async Task<DtoAbbonamento?> OttieniTramiteIdAsync(string utenteId)
    {
        Abbonamento? abbonamento = await _contesto.Abbonamenti.FindAsync(utenteId);

        if (abbonamento == null)
        {
            return null;
        }

        DtoAbbonamento risultato = new DtoAbbonamento();
        risultato.id = abbonamento.Id;
        risultato.Nome = abbonamento.Nome;
        risultato.DataDiInizio = abbonamento.DataInizio.ToLocalTime();
        risultato.Durata = abbonamento.Durata;
        risultato.DataDiFine = abbonamento.DataInizio.AddMonths(abbonamento.Durata);
        risultato.Prezzo = abbonamento.Prezzo;
        risultato.Sconto = abbonamento.Sconto;

        return risultato;
    }

    public async Task<DtoAbbonamento> CreazioneAsync(DtoCreazioneAbbonamento dto)
    {
        Abbonamento abbonamento = new Abbonamento();
        abbonamento.Id = dto.Id;
        abbonamento.Nome = dto.Nome;
        abbonamento.DataInizio = DateTime.UtcNow;
        abbonamento.Durata = dto.Durata;
        abbonamento.DataFine = dto.DataFine;
        abbonamento.Prezzo = dto.Prezzo;
        abbonamento.Sconto = dto.Sconto;

        _contesto.Abbonamenti.Add(abbonamento);
        await _contesto.SaveChangesAsync();

        DtoAbbonamento risultato = new DtoAbbonamento();
        risultato.Id = abbonamento.Id;
        risultato.Nome = abbonamento.NomeAzione;
        risultato.DataInizio = abbonamento.DataInizio.ToLocalTime();
        risultato.Durata = abbonamento.Durata;
        risultato.DataFine = abbonamento.DataInizio.AddMonths(abbonamento.Durata);
        risultato.Prezzo = abbonamento.Prezzo;
        risultato.Sconto = abbonamento.Sconto;

        return risultato;
    }
}