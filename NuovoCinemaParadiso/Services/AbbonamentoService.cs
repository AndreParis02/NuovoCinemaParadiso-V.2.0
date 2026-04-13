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

    public async Task<List<DtoAbbonamento>> OttieniTutto()
    {
        List<Abbonamento> abbonamenti = await _contesto.Abbonamenti.ToListAsync();

        List<DtoAbbonamento> risultato = new List<DtoAbbonamento>();

        for (int i = 0; i < abbonamenti.Count; i++)
        {
            Abbonamento abbonamentoCorrente = abbonamenti[i];

            DtoAbbonamento dto = new DtoAbbonamento();
            dto.Id = abbonamentoCorrente.Id;
            dto.Nome = abbonamentoCorrente.Nome;
            dto.DataInizio = abbonamentoCorrente.DataInizio;
            dto.Durata = abbonamentoCorrente.Durata;
            dto.DataFine = abbonamentoCorrente.DataFine;
            dto.Prezzo = abbonamentoCorrente.Prezzo;
            dto.Sconto = abbonamentoCorrente.Sconto;

            risultato.Add(dto);
        }

        return risultato;
    }
    public async Task<DtoAbbonamento?> OttieniTramiteIdAsync(string id, string utenteId)
    {
        Abbonamento? abbonamento = await _contesto.Abbonamenti.FindAsync(id);

        if (abbonamento == null)
        {
            return null;
        }

        DtoAbbonamento risultato = new DtoAbbonamento();
        risultato.Id = abbonamento.Id;
        risultato.Nome = abbonamento.Nome;
        risultato.DataInizio = abbonamento.DataInizio.ToLocalTime();
        risultato.Durata = abbonamento.Durata;
        risultato.DataFine = abbonamento.DataInizio.AddMonths(abbonamento.Durata);
        risultato.Prezzo = abbonamento.Prezzo;
        risultato.Sconto = abbonamento.Sconto;

        return risultato;
    }

    public async Task<DtoAbbonamento> OttieniTramiteIdPerAdminAsync(string id)
    {
        Abbonamento? abbonamento = await _contesto.Abbonamenti.FindAsync(id);

        if (abbonamento == null)
        {
            return null;
        }

        DtoAbbonamento dto = new DtoAbbonamento();
        dto.Id = abbonamento.Id;
        dto.Nome = abbonamento.Nome;
        dto.DataInizio = abbonamento.DataInizio;
        dto.Durata = abbonamento.Durata;
        dto.DataFine = abbonamento.DataFine;
        dto.Prezzo = abbonamento.Prezzo;
        dto.Sconto = abbonamento.Sconto;

        return dto;
    }

    public async Task<DtoAbbonamento> CreazioneAsync(DtoCreazioneAbbonamento dto)
    {
        Abbonamento abbonamento = new Abbonamento();
        abbonamento.Nome        = dto.Nome;
        abbonamento.DataInizio  = DateTime.UtcNow;
        abbonamento.Durata      = dto.Durata;
        abbonamento.DataFine    = dto.DataFine;
        abbonamento.Prezzo      = dto.Prezzo;
        abbonamento.Sconto      = dto.Sconto;

        _contesto.Abbonamenti.Add(abbonamento);
        await _contesto.SaveChangesAsync();

        DtoAbbonamento risultato = new DtoAbbonamento();
        risultato.Id             = abbonamento.Id;
        risultato.Nome           = abbonamento.Nome;
        risultato.DataInizio     = abbonamento.DataInizio.ToLocalTime();
        risultato.Durata         = abbonamento.Durata;
        risultato.DataFine       = abbonamento.DataInizio.AddMonths(abbonamento.Durata);
        risultato.Prezzo         = abbonamento.Prezzo;
        risultato.Sconto         = abbonamento.Sconto;

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

        return new DtoAbbonamento
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