
```c#
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
}
public async Task<List<DtoUtente>> OttieniTramiteAbbonamentoAsync(string abbonamentoId)
{
    // 1. Carico tutti gli utenti dal database.
    //    EF Core qui carica anche le loro relazioni (come Abbonamento) se configurate correttamente.
    List<Utente> utenti = await _contesto.Utenti.ToListAsync();

    // 2. Carico tutti gli abbonamenti dal database.
    //    Questo serve per trovare l'abbonamento richiesto tramite il suo Id.
    List<Abbonamento> abbonamenti = await _contesto.Abbonamenti.ToListAsync();

    // 3. Variabile che conterrà l'abbonamento trovato.
    Abbonamento? abbonamentoTrovato = null;

    // 4. Ciclo manualmente tutti gli abbonamenti per trovare quello con l'Id richiesto.
    for (int i = 0; i < abbonamenti.Count; i++)
    {
        Abbonamento abbonamentoCorrente = abbonamenti[i];

        // Se l'Id corrisponde, salvo l'abbonamento e interrompo il ciclo.
        if (abbonamentoCorrente.Id == abbonamentoId)
        {
            abbonamentoTrovato = abbonamentoCorrente;
            break;
        }
    }

    // 5. Se non ho trovato nessun abbonamento con quell'Id, ritorno una lista vuota.
    if (abbonamentoTrovato == null)
    {
        return new List<DtoUtente>();
    }

    // 6. Lista finale dei DTO da restituire.
    List<DtoUtente> risultato = new List<DtoUtente>();

    // 7. Ciclo tutti gli utenti per verificare quali hanno questo abbonamento.
    for (int i = 0; i < utenti.Count; i++)
    {
        Utente utenteCorrente = utenti[i];

        // Confronto diretto tra l'abbonamento dell'utente e quello trovato.
        // Funziona solo se la relazione è configurata correttamente e EF ha caricato l'oggetto Abbonamento.
        if (utenteCorrente.Abbonamento == abbonamentoTrovato)
        {
            // 8. Creo il DTO dell'utente.
            DtoUtente dto = new DtoUtente();
            dto.Id = utenteCorrente.Id;
            dto.NomeCompleto = utenteCorrente.NomeCompleto;
            dto.Email = utenteCorrente.Email;
            dto.Eta = utenteCorrente.Eta;

            // 9. Aggiungo il DTO alla lista dei risultati.
            risultato.Add(dto);
        }
    }

    // 10. Ritorno la lista degli utenti che hanno l'abbonamento richiesto.
    return risultato;
}
```