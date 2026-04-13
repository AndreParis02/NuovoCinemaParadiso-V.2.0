using Microsoft.EntityFrameworkCore;
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
    public async Task<List<DtoUtente>> OttieniTramiteAbbonamentoAsync(string abbonamentoId)
{
    
    List<Utente> utenti = await _contesto.Utenti.ToListAsync();
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
        return new List<DtoUtente>();
    }

    List<DtoUtente> risultato = new List<DtoUtente>();

    for (int i = 0; i < utenti.Count; i++)
    {
        Utente utenteCorrente = utenti[i];

        if (utenteCorrente.Abbonamento == abbonamentoTrovato)
        {
            DtoUtente dto = new DtoUtente();
            dto.Id = utenteCorrente.Id;
            dto.NomeCompleto = utenteCorrente.NomeCompleto;
            dto.Email = utenteCorrente.Email;
            dto.Eta = utenteCorrente.Eta;

            risultato.Add(dto);
        }
    }

    return risultato;
}


}