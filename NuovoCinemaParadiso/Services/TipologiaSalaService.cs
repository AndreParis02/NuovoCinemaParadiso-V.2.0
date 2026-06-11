using Microsoft.EntityFrameworkCore;
using NuovoCinemaParadiso.Dtos;
using NuovoCinemaParadiso.Data;
using NuovoCinemaParadiso.Models;

namespace NuovoCinemaParadiso.Services;

public class TipologiaSalaService
{
    private readonly ContestoDb _contesto;
    public TipologiaSalaService(ContestoDb contesto)
    {
        _contesto = contesto;
    }

    public async Task<List<DtoTipologiaSala>> OttieniTuttoAsync()
    {
        List<TipologiaSala> tipologieSala = await _contesto.TipologieSala.ToListAsync();

        return tipologieSala
            .Select(tipologiaSala => new DtoTipologiaSala
            {
                Id = tipologiaSala.Id,
                Nome = tipologiaSala.Nome,
                MaggiorazionePrezzo = tipologiaSala.MaggiorazionePrezzo,
                IsDeleted = tipologiaSala.isDeleted
            })
            .ToList();
    }

    public async Task<DtoTipologiaSala?> OttieniTramiteIdAsync(string id)
    {
        TipologiaSala? tipologiaSala = await _contesto.TipologieSala.FindAsync(id);

        if (tipologiaSala == null)
        {
            return null;
        }

        DtoTipologiaSala risultato = new DtoTipologiaSala();
        risultato.Id = tipologiaSala.Id;
        risultato.Nome = tipologiaSala.Nome;
        risultato.MaggiorazionePrezzo = tipologiaSala.MaggiorazionePrezzo;

        return risultato;
    }

    public async Task<bool> CreazioneAsync(DtoCreazioneTipologiaSala dto)
    {

        TipologiaSala tipologiaSala = new TipologiaSala();
        tipologiaSala.Nome = dto.Nome;
        tipologiaSala.MaggiorazionePrezzo = dto.MaggiorazionePrezzo;

        _contesto.TipologieSala.Add(tipologiaSala);
        await _contesto.SaveChangesAsync();

        return true;
    }

    public async Task<bool> ModificaAsync(string id, DtoCreazioneTipologiaSala dto)
    {
        TipologiaSala? tipologiaSala = await _contesto.TipologieSala.FindAsync(id);

        if (tipologiaSala == null)
        {
            return false;
        }

        tipologiaSala.Nome = dto.Nome;
        tipologiaSala.MaggiorazionePrezzo = dto.MaggiorazionePrezzo;

        await _contesto.SaveChangesAsync();

        return true;
    }

    public async Task<bool> EliminaAsync(string id)
    {
        TipologiaSala? tipologiaSala = await _contesto.TipologieSala.FindAsync(id);

        if (tipologiaSala == null || tipologiaSala.isDeleted)
        {
            return false;
        }

        tipologiaSala.isDeleted = true;
        await _contesto.SaveChangesAsync();

        return true;
    }
}