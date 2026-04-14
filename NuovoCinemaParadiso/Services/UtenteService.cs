using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileSystemGlobbing.Internal.PathSegments;
using NuovoCinemaParadiso.Data;
using NuovoCinemaParadiso.Dtos;
using NuovoCinemaParadiso.Models;

namespace NuovoCinemaParadiso.Services;

public class UtenteService
{
    private readonly ContestoDb _contesto;
    public UtenteService(ContestoDb contestoDb)
    {
        _contesto = contestoDb;
    }

    public async Task<DtoUtente> AbbonatiAsync(string abbonamentoId, string utenteId)
    {
        List<Abbonamento> abbonamenti = await _contesto.Abbonamenti.ToListAsync();
        Abbonamento? abbonamentoTrovato = null;

        for (int i = 0; i < abbonamenti.Count; i++)
        {
            Abbonamento abbonamentoCorrente = abbonamenti[i];

            if (abbonamentoCorrente.Id == abbonamentoId)
            {
                abbonamentoTrovato = abbonamentoCorrente;
                break;
            }
        }

        if (abbonamentoTrovato == null)
        {
            return null;
        }

        List<Utente> utenti = await _contesto.Utenti.ToListAsync();
        Utente? utenteTrovato = null;

        for (int i = 0; i < utenti.Count; i++)
        {
            if (utenti[i].Id == utenteId)
            {
                utenteTrovato = utenti[i];
                break;
            }
        }

        if (utenteTrovato == null)
        {
            return null;
        }

        utenteTrovato.AbbonamentoId = abbonamentoTrovato.Id;
        await _contesto.SaveChangesAsync();

        return new DtoUtente()
        {
            Id = utenteTrovato.Id,
            AbbonamentoId = abbonamentoTrovato.Id,
            TipoAbbonamento = abbonamentoTrovato.Nome
        };
    }
}