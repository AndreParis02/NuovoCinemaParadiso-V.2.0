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

        List<DtoSala> risultato = new List<DtoSala>();

        for (int i = 0; i < sale.Count; i++)
        {
            Sala salaCorrente = sale[i];

            TipologiaSala? tipologiaSala = await _contesto.TipologieSala.FindAsync(salaCorrente.TipologiaSalaId);
            DtoSala dto = new DtoSala();
            dto.Id = salaCorrente.Id;
            dto.Nome = salaCorrente.Nome;
            dto.Capienza = salaCorrente.Capienza;
            dto.NomeTipologia = tipologiaSala?.Nome ?? "";
            dto.TipologiaSalaId = tipologiaSala?.Id ?? "";

            risultato.Add(dto);
        }
        return risultato;
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
            TipologiaSalaId = tipologiaSala?.Id ?? ""
        };
        return risultato;
    }

    public async Task<List<DtoSala>> OttieniTramiteTipologiaAsync(string tipologiaId)
    {
        List<Sala> tutteLeSale = await _contesto.Sale.ToListAsync();

        List<DtoSala> risultato = new List<DtoSala>();

        foreach (var sala in tutteLeSale)
        {
            if (sala.TipologiaSalaId != tipologiaId)
                continue;

            TipologiaSala? tipologia = await _contesto.TipologieSala.FindAsync(sala.TipologiaSalaId);

            DtoSala dto = new DtoSala
            {
                Id = sala.Id,
                Nome = sala.Nome,
                Capienza = sala.Capienza,
                TipologiaSalaId = sala.TipologiaSalaId,
                NomeTipologia = tipologia?.Nome ?? "",
            };

            risultato.Add(dto);
        }
        return risultato;
    }

    public async Task<DtoSala> CreazioneAsync(DtoCreazioneSala dto)
    {
        Sala sala = new Sala
        {
            Nome = dto.Nome,
            Capienza = dto.Capienza,
            TipologiaSalaId = dto.TipologiaSalaId
        };

        _contesto.Sale.Add(sala);
        await _contesto.SaveChangesAsync();

        TipologiaSala? tipologiaSala = await _contesto.TipologieSala.FindAsync(sala.TipologiaSalaId);

        DtoSala risultato = new DtoSala
        {
            Id = sala.Id,
            Nome = sala.Nome,
            Capienza = sala.Capienza,
            TipologiaSalaId = tipologiaSala?.Id ?? "",
            NomeTipologia = sala.TipologiaSala?.Nome ?? ""
        };

        return risultato;
    }

   public async Task<DtoSala?> ModificaAsync(string id, DtoCreazioneSala dto)
{
    
    Sala? salaEsistente = await _contesto.Sale.FindAsync(id);

    if (salaEsistente == null)
    {
        return null;
    }

    
    List<Sala> listaSale = await _contesto.Sale.ToListAsync();

    for (int i = 0; i < listaSale.Count; i++)
    {
        Sala salaCorrente = listaSale[i];

       
        if (salaCorrente.Nome == dto.Nome && salaCorrente.Id != id)
        {
            
            throw new Exception("Esiste già una sala con questo nome.");
        }
    }

    
    salaEsistente.Nome = dto.Nome;
    salaEsistente.Capienza = dto.Capienza;
    salaEsistente.TipologiaSalaId = dto.TipologiaSalaId;

    await _contesto.SaveChangesAsync();

    TipologiaSala? tipologiaSala = await _contesto.TipologieSala.FindAsync(salaEsistente.TipologiaSalaId);

    
    DtoSala risultato = new DtoSala();
    risultato.Id = salaEsistente.Id;
    risultato.Nome = salaEsistente.Nome;
    risultato.Capienza = salaEsistente.Capienza;
    risultato.TipologiaSalaId = salaEsistente.TipologiaSalaId;
    risultato.NomeTipologia = tipologiaSala?.Nome ?? "";

    return risultato;
}



    public async Task<bool> EliminaAsync(string id)
    {
        Sala? sala = await _contesto.Sale.FindAsync(id);

        if (sala == null)
        {
            return false;
        }

        _contesto.Sale.Remove(sala);
        await _contesto.SaveChangesAsync();

        return true;
    }
}