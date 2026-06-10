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

    // -----------------------------
    // LETTURA (mantiene i DTO)
    // -----------------------------
    public async Task<List<DtoAbbonamento>> OttieniTutto()
    {
        List<Abbonamento> abbonamenti = await _contesto.Abbonamenti.ToListAsync();
        List<DtoAbbonamento> risultato = new List<DtoAbbonamento>();

        for (int i = 0; i < abbonamenti.Count; i++)
        {
            Abbonamento a = abbonamenti[i];

            risultato.Add(new DtoAbbonamento
            {
                Id = a.Id,
                Nome = a.Nome,
                Durata = a.Durata,
                Prezzo = a.Prezzo,
                Sconto = a.Sconto
            });
        }

        return risultato;
    }

    public async Task<DtoAbbonamento?> OttieniTramiteIdAsync(string id)
    {
        Abbonamento? abbonamento = await _contesto.Abbonamenti.FindAsync(id);
        if (abbonamento == null)
            return null;
            
        return new DtoAbbonamento
        {
            Id = abbonamento.Id,
            Nome = abbonamento.Nome,
            Durata = abbonamento.Durata,
            Prezzo = abbonamento.Prezzo,
            Sconto = abbonamento.Sconto
        };
    }

    public async Task<(bool Successo, string Messaggio)> CreazioneAsync(DtoCreazioneAbbonamento dto)
    {
        List<Abbonamento> abbonamenti = await _contesto.Abbonamenti.ToListAsync();

        for (int i = 0; i < abbonamenti.Count; i++)
        {
            if (abbonamenti[i].Nome.ToLower() == dto.Nome.ToLower())
            {
                return (false, "Esiste già un abbonamento con questo nome.");
            }
        }

        Abbonamento nuovo = new Abbonamento
        {
            Nome = dto.Nome,
            Durata = dto.Durata,
            Prezzo = dto.Prezzo,
            Sconto = dto.Sconto
        };

        _contesto.Abbonamenti.Add(nuovo);
        await _contesto.SaveChangesAsync();

        return (true, "Abbonamento creato correttamente.");
    }

   
    public async Task<(bool Successo, string Messaggio)> ModificaAsync(string id, DtoCreazioneAbbonamento dto)
    {
        Abbonamento? abbonamento = await _contesto.Abbonamenti.FindAsync(id);
        if (abbonamento == null)
        {
            return (false, "Abbonamento non trovato.");
        }

        List<Abbonamento> abbonamenti = await _contesto.Abbonamenti.ToListAsync();

        for (int i = 0; i < abbonamenti.Count; i++)
        {
            if (abbonamenti[i].Id != id &&
                abbonamenti[i].Nome.ToLower() == dto.Nome.ToLower())
            {
                return (false, "Esiste già un altro abbonamento con questo nome.");
            }
        }

        abbonamento.Nome = dto.Nome;
        abbonamento.Durata = dto.Durata;
        abbonamento.Prezzo = dto.Prezzo;
        abbonamento.Sconto = dto.Sconto;

        await _contesto.SaveChangesAsync();

        return (true, "Abbonamento modificato correttamente.");
    }

   
    public async Task<(bool Successo, string Messaggio)> EliminazioneAsync(string id)
    {
        Abbonamento? abbonamento = await _contesto.Abbonamenti.FindAsync(id);

        if (abbonamento == null)
        {
            return (false, "Abbonamento non trovato.");
        }

        _contesto.Abbonamenti.Remove(abbonamento);
        await _contesto.SaveChangesAsync();

        return (true, "Abbonamento eliminato correttamente.");
    }
}
