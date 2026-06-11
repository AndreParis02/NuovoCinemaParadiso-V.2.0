using Microsoft.EntityFrameworkCore;
using NuovoCinemaParadiso.Data;
using NuovoCinemaParadiso.Dtos;
using NuovoCinemaParadiso.Exceptions;
using NuovoCinemaParadiso.Models;

namespace NuovoCinemaParadiso.Services;

public class SalaService
{
    private readonly ContestoDb _contesto;
    public SalaService(ContestoDb contesto)
    {
        _contesto = contesto;
    }

    public async Task<List<DtoSala>> OttieniTuttoAsync()
    {
        List<Sala> sale = await _contesto.Sale.ToListAsync();

        var risultato = await Task.WhenAll(sale.Select(async salaCorrente =>
        {
            TipologiaSala? tipologiaSala = await _contesto.TipologieSala.FindAsync(salaCorrente.TipologiaSalaId);
            return new DtoSala
            {
                Id = salaCorrente.Id,
                Nome = salaCorrente.Nome,
                Capienza = salaCorrente.Capienza,
                NomeTipologia = tipologiaSala?.Nome ?? "",
                TipologiaSalaId = tipologiaSala?.Id ?? "",
                IsDeleted = salaCorrente.IsDeleted
            };
        }));

        return risultato.ToList();
    }
    public async Task<DtoSala?> OttieniTramiteIdAsync(string id)
    {
        Sala? sala = await _contesto.Sale.FindAsync(id);

        if (sala == null)
        {
            return null;
        }

        TipologiaSala? tipologiaSala = await _contesto.TipologieSala.FindAsync(sala.TipologiaSalaId);
        Console.WriteLine(sala.TipologiaSalaId);
        DtoSala risultato = new DtoSala
        {
            Id = sala.Id,
            Nome = sala.Nome,
            Capienza = sala.Capienza,
            NomeTipologia = tipologiaSala?.Nome ?? "",
            TipologiaSalaId = tipologiaSala?.Id ?? "",
            IsDeleted = sala.IsDeleted
            
        };
        return risultato;
    }

    public async Task<List<DtoSala>> OttieniTramiteTipologiaAsync(string tipologiaId)
    {
        List<Sala> tutteLeSale = await _contesto.Sale.ToListAsync();

        var risultato = await Task.WhenAll(tutteLeSale
            .Where(sala => sala.TipologiaSalaId == tipologiaId)
            .Select(async sala =>
            {
                TipologiaSala? tipologia = await _contesto.TipologieSala.FindAsync(sala.TipologiaSalaId);

                return new DtoSala
                {
                    Id = sala.Id,
                    Nome = sala.Nome,
                    Capienza = sala.Capienza,
                    TipologiaSalaId = sala.TipologiaSalaId,
                    NomeTipologia = tipologia?.Nome ?? "",
                    IsDeleted = sala.IsDeleted
                };
            }));

        return risultato.ToList();
    }

    public async Task<string?> CreazioneAsync(DtoCreazioneSala dto)
    {
        Sala sala = new Sala
        {
            Nome = dto.Nome,
            Capienza = dto.Capienza,
            TipologiaSalaId = dto.TipologiaSalaId
        };

        _contesto.Sale.Add(sala);
        await _contesto.SaveChangesAsync();

        return "Sala creata con successo.";
    }

   public async Task<string?> ModificaAsync(string id, DtoCreazioneSala dto)
{
    
    Sala? salaEsistente = await _contesto.Sale.FindAsync(id);

    if (salaEsistente == null)
    {
         throw new ItemNotFoundException("Sala");
    }

    TipologiaSala? tipologia = await _contesto.TipologieSala.FindAsync(dto.TipologiaSalaId);
    if (tipologia == null)
    {
        throw new NotFoundException("TipologiaSala", dto.TipologiaSalaId);
    }

    
    salaEsistente.Nome = dto.Nome;
    salaEsistente.Capienza = dto.Capienza;
    salaEsistente.TipologiaSalaId = dto.TipologiaSalaId;

    await _contesto.SaveChangesAsync();

    return "Sala modificata con successo.";
}



    public async Task<bool> EliminaAsync(string id)
    {
        Sala? sala = await _contesto.Sale.FindAsync(id);

        if (sala == null || sala.IsDeleted)
        {
            return false;
        }

        sala.IsDeleted = true;
        //_contesto.Sale.Remove(sala);
        await _contesto.SaveChangesAsync();

        return true;
    }
}