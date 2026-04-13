using Microsoft.EntityFrameworkCore;
using NuovoCinemaParadiso.Data;
using NuovoCinemaParadiso.Dtos;
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
            FasciaOraria? fasciaOraria = await _contesto.FasceOrarie.FindAsync(salaCorrente.FasciaOrariaId);

            DtoSala dto = new DtoSala();
            dto.Id = salaCorrente.Id;
            dto.Nome = salaCorrente.Nome;
            dto.Capienza = salaCorrente.Capienza;
            dto.FasciaOraria = fasciaOraria?.Nome ?? "";
            dto.NomeTipologia = tipologiaSala?.Nome ?? "";

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
        FasciaOraria? fasciaOraria = await _contesto.FasceOrarie.FindAsync(sala.FasciaOrariaId);

        DtoSala risultato = new DtoSala
        {
            Id = sala.Id,
            Nome = sala.Nome,
            Capienza = sala.Capienza,
            NomeTipologia = tipologiaSala?.Nome ?? "",
            FasciaOraria = fasciaOraria?.Nome ?? ""
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
            FasciaOraria? fascia = await _contesto.FasceOrarie.FindAsync(sala.FasciaOrariaId);

            DtoSala dto = new DtoSala
            {
                Id = sala.Id,
                Nome = sala.Nome,
                Capienza = sala.Capienza,
                TipologiaSalaId = sala.TipologiaSalaId,
                NomeTipologia = tipologia?.Nome ?? "",
                FasciaOrariaId = sala.FasciaOrariaId,
                FasciaOraria = fascia?.Nome ?? ""
            };

            risultato.Add(dto);
        }
        return risultato;
    }
    public async Task<List<DtoSala>> OttieniTramiteFasciaOrariaAsync(string fasciaOrariaId)
    {
        List<Sala> tutteLeSale = await _contesto.Sale.ToListAsync();

        List<DtoSala> risultato = new List<DtoSala>();

        foreach (var sala in tutteLeSale)
        {
            if (sala.FasciaOrariaId != fasciaOrariaId)
                continue;

            TipologiaSala? tipologia = await _contesto.TipologieSala.FindAsync(sala.TipologiaSalaId);
            FasciaOraria? fascia = await _contesto.FasceOrarie.FindAsync(sala.FasciaOrariaId);

            DtoSala dto = new DtoSala
            {
                Id = sala.Id,
                Nome = sala.Nome,
                Capienza = sala.Capienza,
                TipologiaSalaId = sala.TipologiaSalaId,
                NomeTipologia = tipologia?.Nome ?? "",
                FasciaOrariaId = sala.FasciaOrariaId,
                FasciaOraria = fascia?.Nome ?? ""
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
            FasciaOrariaId = dto.FasciaOrariaId,
            TipologiaSalaId = dto.TipologiaSalaId
        };

        _contesto.Sale.Add(sala);
        await _contesto.SaveChangesAsync();

        FasciaOraria? fasciaOraria = await _contesto.FasceOrarie.FindAsync(sala.FasciaOrariaId);
        TipologiaSala? tipologiaSala = await _contesto.TipologieSala.FindAsync(sala.TipologiaSalaId);

        DtoSala risultato = new DtoSala
        {
            Id = sala.Id,
            Nome = sala.Nome,
            Capienza = sala.Capienza,
            FasciaOrariaId = sala.FasciaOrariaId,
            FasciaOraria = fasciaOraria?.Nome ?? "",
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

        salaEsistente.Nome = dto.Nome;
        salaEsistente.Capienza = dto.Capienza;
        salaEsistente.FasciaOrariaId = dto.FasciaOrariaId;
        salaEsistente.TipologiaSalaId = dto.TipologiaSalaId;

        await _contesto.SaveChangesAsync();

        TipologiaSala? tipologiaSala = await _contesto.TipologieSala.FindAsync(salaEsistente.TipologiaSalaId);
        FasciaOraria? fasciaOraria = await _contesto.FasceOrarie.FindAsync(salaEsistente.FasciaOrariaId);

        DtoSala risultato = new DtoSala();
        risultato.Id = salaEsistente.Id;
        risultato.Nome = salaEsistente.Nome;
        risultato.Capienza = salaEsistente.Capienza;
        risultato.FasciaOrariaId = salaEsistente.FasciaOrariaId;
        risultato.FasciaOraria = fasciaOraria.Nome;
        risultato.TipologiaSalaId = salaEsistente.TipologiaSalaId;
        risultato.NomeTipologia = tipologiaSala.Nome;

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