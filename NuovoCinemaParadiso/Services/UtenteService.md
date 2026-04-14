```c#
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileSystemGlobbing.Internal.PathSegments;
using NuovoCinemaParadiso.Data;
using NuovoCinemaParadiso.Dtos;
using NuovoCinemaParadiso.Models;

namespace NuovoCinemaParadiso.Services;

// Service che gestisce la logica legata agli utenti
public class UtenteService
{
    // Contesto del database (Entity Framework)
    private readonly ContestoDb _contesto;

    // Costruttore con Dependency Injection
    public UtenteService(ContestoDb contestoDb)
    {
        _contesto = contestoDb;
    }

    // Metodo per abbonare un utente a un abbonamento
    public async Task<DtoUtente> AbbonatiAsync(string abbonamentoId, string utenteId)
    {
        // Carica tutti gli abbonamenti dal database
        List<Abbonamento> abbonamenti = await _contesto.Abbonamenti.ToListAsync();

        // Variabile per salvare l'abbonamento trovato
        Abbonamento? abbonamentoTrovato = null;

        // Ciclo per cercare l'abbonamento per ID
        for (int i = 0; i < abbonamenti.Count; i++)
        {
            Abbonamento abbonamentoCorrente = abbonamenti[i];

            if (abbonamentoCorrente.Id == abbonamentoId)
            {
                abbonamentoTrovato = abbonamentoCorrente;
                break;
            }
        }

        // Se non esiste, ritorna null
        if (abbonamentoTrovato == null)
        {
            return null;
        }

        // Carica tutti gli utenti dal database
        List<Utente> utenti = await _contesto.Utenti.ToListAsync();

        // Variabile per salvare l'utente trovato
        Utente? utenteTrovato = null;

        // Ciclo per cercare l'utente per ID
        for (int i = 0; i < utenti.Count; i++)
        {
            if (utenti[i].Id == utenteId)
            {
                utenteTrovato = utenti[i];
                break;
            }
        }

        // Se utente non trovato
        if (utenteTrovato == null)
        {
            return null;
        }

        // Associa l'abbonamento all'utente
        utenteTrovato.AbbonamentoId = abbonamentoTrovato.Id;

        // Salva le modifiche nel database
        await _contesto.SaveChangesAsync();

        // Ritorna un DTO con i dati essenziali
        return new DtoUtente()
        {
            Id = utenteTrovato.Id,
            AbbonamentoId = abbonamentoTrovato.Id,
            TipoAbbonamento = abbonamentoTrovato.Nome
        };
    }

    // Metodo per ottenere tutti gli utenti con uno specifico abbonamento
    public async Task<List<DtoUtente>> OttieniTramiteAbbonamentoAsync(string abbonamentoId)
    {
        // Carica utenti e abbonamenti
        List<Utente> utenti = await _contesto.Utenti.ToListAsync();
        List<Abbonamento> abbonamenti = await _contesto.Abbonamenti.ToListAsync();

        // Cerca l'abbonamento
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

        // Se non esiste, ritorna lista vuota
        if (abbonamentoTrovato == null)
        {
            return new List<DtoUtente>();
        }

        // Lista risultato
        List<DtoUtente> risultato = new List<DtoUtente>();

        // Filtra utenti con quell'abbonamento
        for (int i = 0; i < utenti.Count; i++)
        {
            Utente utenteCorrente = utenti[i];

            // Controlla se l'abbonamento dell'utente è quello cercato
            if (utenteCorrente.Abbonamento == abbonamentoTrovato)
            {
                // Mappa Utente -> DTO
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

```