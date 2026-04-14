# NuovoCinemaParadisoV2.0

## Implementazione abbonamenti.

Implementare una funzionalità che permette agli utenti di acquistare un abbonamento che permette una scontistica sul prezzo dei biglietti.
Nella tabella utente ci sarà la foreign key che collega la corrispettiva tabella alla tabella abbonamenti.
Gli abbonamenti saranno di tre livelli: mensile, semestrale, annuale, ognuno con il suo prezzo e la sua data di inizio e di scadenza.
- mensile (25%).
- semestrale (50%).
- annuale (75%).

# Models

## Abbonamento.cs

```c#
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NuovoCinemaParadiso.Models;

// Mappa la classe alla tabella "Abbonamenti" nel database
[Table("Abbonamenti")]
public class Abbonamento
{
    // Chiave primaria dell'abbonamento, generata automaticamente come stringa GUID
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString();

    // Nome dell'abbonamento (es. "Mensile", "Annuale"), obbligatorio e max 50 caratteri
    [Required]
    [StringLength(50)]
    public string Nome { get; set; } = string.Empty;

    // Durata dell'abbonamento espressa in giorni (o mesi, a seconda della logica di business)
    [Required]
    public int Durata { get; set; }

    // Data di inizio dell'abbonamento, default: data attuale in UTC
    [Required]
    public DateTime DataInizio { get; set; } = DateTime.UtcNow;

    // Data di fine dell'abbonamento, calcolata in base alla durata
    public DateTime DataFine { get; set; }

    // Prezzo dell'abbonamento, obbligatorio
    [Required]
    public decimal Prezzo { get; set; }

    // Percentuale di sconto applicata all'abbonamento (0–100), obbligatoria
    [Required]
    public int Sconto { get; set; }

    // Lista degli utenti che possiedono questo abbonamento (relazione 1-N)
    public List<Utente> Utenti { get; set; } = new List<Utente>();
}
```

# Dtos

## DtoAbbonamento.cs

```c#
namespace NuovoCinemaParadiso.Dtos;

// DTO utilizzato per trasferire i dati dell'abbonamento verso/da il client
public class DtoAbbonamento
{
    // Identificativo univoco dell'abbonamento (stringa GUID)
    public string id { get; set; }

    // Nome dell'abbonamento (es. "Mensile", "Annuale")
    public string Nome { get; set; } = string.Empty;

    // Data di inizio dell'abbonamento
    public DateTime DataDiInizio { get; set; }

    // Durata dell'abbonamento (giorni o mesi, in base alla logica di business)
    public int Durata { get; set; }

    // Data di fine dell'abbonamento
    public DateTime DataDiFine { get; set; }

    // Prezzo dell'abbonamento
    public decimal Prezzo { get; set; }

    // Percentuale di sconto applicata (0–100)
    public int Sconto { get; set; }
}
```

## DtoCreazioneAbbonamento.cs

```c#
namespace NuovoCinemaParadiso.Dtos;

// DTO utilizzato per la creazione di un nuovo abbonamento.
// Contiene solo i campi necessari in input dal client.
public class DtoCreazioneAbbonamento
{
    // Nome dell'abbonamento (es. "Mensile", "Annuale")
    public string Nome { get; set; } = string.Empty;

    // Durata dell'abbonamento (giorni o mesi, in base alla logica di business)
    public int Durata { get; set; }

    // Data di inizio dell'abbonamento
    public DateTime DataInizio { get; set; }

    // Data di fine dell'abbonamento (può essere calcolata lato server)
    public DateTime DataFine { get; set; }

    // Prezzo dell'abbonamento
    public decimal Prezzo { get; set; }

    // Percentuale di sconto applicata (0–100)
    public int Sconto { get; set; }
}
```

# Controllers

## AbbonamentiController.cs

```c#
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using NuovoCinemaParadiso.Services;
using NuovoCinemaParadiso.Dtos;
using NuovoCinemaParadiso.Models;

namespace NuovoCinemaParadiso.Controllers;

[ApiController] 
[Route("api/[controller]")]
[Authorize] // Richiede autenticazione per tutte le azioni del controller
public class AbbonamentiController : ControllerBase
{
    private readonly AbbonamentoService _abbonamentoService;
    private readonly LogAzioniService _logAzioniService;

    // Iniezione dei servizi necessari: gestione abbonamenti e logging azioni
    public AbbonamentiController(AbbonamentoService abbonamentoService, LogAzioniService logAzioniService)
    {
        _abbonamentoService = abbonamentoService;
        _logAzioniService = logAzioniService;
    }

    // ----------------------------------------------------------------------
    // GET ADMIN: Ottiene tutti gli abbonamenti (solo per Gestore/Operatore)
    // ----------------------------------------------------------------------
    [HttpGet("admin")]
    [Authorize(Roles = Ruoli.GestoreOrOperatore)]
    public async Task<IActionResult> OttieniTuttiGliAbbonamentiAdmin()
    {
        // Recupera tutti gli abbonamenti senza filtri utente
        List<DtoAbbonamento> abbonamenti = await _abbonamentoService.OttieniTuttoAdmin();

        // Recupera l'ID dell'utente autenticato
        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        // Registra l'azione nei log
        await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
        {
            IdUtente = utenteId,
            NomeAzione = "Ottieni tutti gli abbonamenti admin",
            Effettuato = true,
            Messaggio = "Operazione eseguita"
        });

        return Ok(abbonamenti);
    }

    // ----------------------------------------------------------------------
    // GET UTENTE: Ottiene gli abbonamenti dell'utente autenticato
    // ----------------------------------------------------------------------
    [HttpGet]
    public async Task<IActionResult> OttieniTuttiGliAbbonamentiUtente()
    {
        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        // Recupera solo gli abbonamenti dell'utente corrente
        List<DtoAbbonamento> abbonamenti = await _abbonamentoService.OttieniTutto(utenteId);

        // Log dell'azione
        await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
        {
            IdUtente = utenteId,
            NomeAzione = "Ottieni tutti gli abbonamenti utente",
            Effettuato = true,
            Messaggio = "Operazione eseguita"
        });

        return Ok(abbonamenti);
    }

    // ----------------------------------------------------------------------
    // GET ADMIN BY ID: Recupera un abbonamento tramite ID (solo admin)
    // ----------------------------------------------------------------------
    [HttpGet("admin/{id}")]
    [Authorize(Roles = Ruoli.GestoreOrOperatore)]
    public async Task<IActionResult> OttieniTramiteIdPerAdmin(string id)
    {
        var risultato = await _abbonamentoService.OttieniTramiteIdPerAdminAsync(id);
        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        // Se non trovato → log fallimento + 404
        if (risultato == null)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
            {
                IdUtente = utenteId,
                NomeAzione = "Ottieni abbonamenti tramite id admin",
                Effettuato = false,
                Messaggio = "Operazione fallita"
            });

            return NotFound($"Abbonamento con id {id} non trovato");
        }

        // Log successo
        await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
        {
            IdUtente = utenteId,
            NomeAzione = "Ottieni abbonamenti tramite id admin",
            Effettuato = true,
            Messaggio = "Operazione eseguita"
        });

        return Ok(risultato);
    }

    // ----------------------------------------------------------------------
    // GET UTENTE BY ID: Recupera un abbonamento dell'utente autenticato
    // ----------------------------------------------------------------------
    [HttpGet("{id}")]
    public async Task<IActionResult> OttieniTramiteIdPerUtente(string id)
    {
        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        // Recupera l'abbonamento solo se appartiene all'utente
        var risultato = await _abbonamentoService.OttieniTramiteIdAsync(id, utenteId);

        if (risultato == null)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
            {
                IdUtente = utenteId,
                NomeAzione = "Ottieni abbonamenti tramite id utente",
                Effettuato = false,
                Messaggio = "Operazione fallita"
            });

            return NotFound($"Abbonamento con id {id} non trovato");
        }

        // Log successo
        await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
        {
            IdUtente = utenteId,
            NomeAzione = "Ottieni abbonamenti tramite id utente",
            Effettuato = true,
            Messaggio = "Operazione eseguita"
        });

        return Ok(risultato);
    }

    // ----------------------------------------------------------------------
    // POST: Creazione di un nuovo abbonamento
    // ----------------------------------------------------------------------
    [HttpPost]
    public async Task<IActionResult> Creazione([FromBody] DtoCreazioneAbbonamento dto)
    {
        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        // Tenta la creazione dell'abbonamento
        DtoAbbonamento? risultato = await _abbonamentoService.CreazioneAsync(dto, utenteId);

        if (risultato == null)
        {
            // Log fallimento
            await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
            {
                IdUtente = utenteId,
                NomeAzione = "Creazione abbonamento",
                Effettuato = false,
                Messaggio = "Operazione fallita"
            });

            return BadRequest(new { messaggio = "Abbonamento già presente oppure non valido." });
        }

        // Log successo
        await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
        {
            IdUtente = utenteId,
            NomeAzione = "Creazione abbonamento",
            Effettuato = true,
            Messaggio = "Operazione eseguita"
        });

        return Ok(risultato);
    }

    // ----------------------------------------------------------------------
    // PUT: Modifica di un abbonamento esistente (solo admin)
    // ----------------------------------------------------------------------
    [HttpPut("{id}")]
    [Authorize(Roles = Ruoli.GestoreOrOperatore)]
    public async Task<IActionResult> Modifica(string id, [FromBody] DtoCreazioneAbbonamento dto)
    {
        DtoAbbonamento? risultato = await _abbonamentoService.ModificaAsync(id, dto);
        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (risultato == null)
        {
            // Log fallimento
            await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
            {
                IdUtente = utenteId,
                NomeAzione = "Modifica abbonamento",
                Effettuato = false,
                Messaggio = "Operazione fallita"
            });

            return NotFound(new { messaggio = "Abbonamento non trovato." });
        }

        // Log successo
        await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
        {
            IdUtente = utenteId,
            NomeAzione = "Modifica abbonamento",
            Effettuato = true,
            Messaggio = "Operazione eseguita"
        });

        return Ok(risultato);
    }

    // ----------------------------------------------------------------------
    // DELETE: Eliminazione di un abbonamento (solo admin)
    // ----------------------------------------------------------------------
    [HttpDelete("{id}")]
    [Authorize(Roles = Ruoli.GestoreOrOperatore)]
    public async Task<IActionResult> Elimina(string id)
    {
        bool eliminato = await _abbonamentoService.EliminazioneAsync(id);
        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!eliminato)
        {
            // Log fallimento
            await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
            {
                IdUtente = utenteId,
                NomeAzione = "Elimina abbonamento",
                Effettuato = false,
                Messaggio = "Operazione fallita"
            });

            return NotFound(new { messaggio = "Abbonamento non trovato." });
        }

        // Log successo
        await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
        {
            IdUtente = utenteId,
            NomeAzione = "Elimina abbonamento",
            Effettuato = true,
            Messaggio = "Operazione eseguita"
        });

        return NoContent();
    }
}
```

# Service

## Services/AbbonamentoService.cs

```c#
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using NuovoCinemaParadiso.Data;
using NuovoCinemaParadiso.Dtos;
using NuovoCinemaParadiso.Models;

namespace NuovoCinemaParadiso.Services;

// Service responsabile della gestione CRUD degli abbonamenti.
// Utilizza il ContestoDb per interagire con il database.
public class AbbonamentoService
{
    private readonly ContestoDb _contesto;

    // Iniezione del DbContext tramite dependency injection
    public AbbonamentoService(ContestoDb contesto)
    {
        _contesto = contesto;
    }

    // Metodo per creare un nuovo abbonamento nel database
    public async Task<DtoAbbonamento> CreazioneAsync(DtoCreazioneAbbonamento dto)
    {
        // Creazione dell'entità Abbonamento da salvare nel DB
        Abbonamento abbonamento = new Abbonamento();

        // ⚠️ ATTENZIONE: dto.Id NON ESISTE nel DTO che mi hai mandato
        // Questo assegnamento causerà errore di compilazione
        abbonamento.Id = dto.Id;

        // Nome dell'abbonamento
        abbonamento.Nome = dto.Nome;

        // Imposta la data di inizio come UTC corrente
        abbonamento.DataInizio = DateTime.UtcNow;

        // Durata dell'abbonamento
        abbonamento.Durata = dto.Durata;

        // Data di fine (inviata dal client)
        abbonamento.DataFine = dto.DataFine;

        // Prezzo dell'abbonamento
        abbonamento.Prezzo = dto.Prezzo;

        // Percentuale di sconto
        abbonamento.Sconto = dto.Sconto;

        // Aggiunge l'abbonamento al contesto EF
        _contesto.Abbonamenti.Add(abbonamento);

        // Salva le modifiche nel database
        await _contesto.SaveChangesAsync();

        // Creazione del DTO di risposta
        DtoAbbonamento risultato = new DtoAbbonamento();

        // Id generato
        risultato.Id = abbonamento.Id;

        // ⚠️ ERRORE: NomeAzione NON ESISTE nel model Abbonamento
        risultato.Nome = abbonamento.NomeAzione;

        // Converte la data in locale per il client
        risultato.DataInizio = abbonamento.DataInizio.ToLocalTime();

        // Durata
        risultato.Durata = abbonamento.Durata;

        // Calcolo della data di fine (qui viene ignorato DataFine del DB)
        risultato.DataFine = abbonamento.DataInizio.AddMonths(abbonamento.Durata);

        // Prezzo
        risultato.Prezzo = abbonamento.Prezzo;

        // Sconto
        risultato.Sconto = abbonamento.Sconto;

        return risultato;
    }
}
```

# Helpers

## CalcolaPrezzoHelper.cs

```c#
namespace NuovoCinemaParadiso.Helpers;

// Classe helper statica per il calcolo del prezzo finale dei biglietti
public static class CalcolaPrezzo
{
    // Calcola il prezzo finale in base al costo del film, della sala,
    // al numero di biglietti e all'eventuale abbonamento dell'utente.
    public static Decimal CalcolaPrezzoFinale(Decimal prezzoMovie, Decimal prezzoSala, int numeroBiglietti, Utente utente)
    {
        // Se l'utente NON ha un abbonamento → prezzo pieno
        if (utente.AbbonamentoId == null)
        {
            // Prezzo totale = (prezzo film + prezzo sala) * numero biglietti
            Decimal prezzoFinale = (prezzoMovie + prezzoSala) * numeroBiglietti;
            return prezzoFinale;
        }
        else
        {
            // Prezzo base del singolo biglietto
            Decimal prezzoBiglietto = prezzoMovie + prezzoSala;

            // Calcolo dello sconto in base alla percentuale dell'abbonamento
            // ATTENZIONE: qui stai calcolando SOLO lo sconto, non il prezzo scontato
            Decimal prezzoScontato = (prezzoBiglietto / 100) * utente.Abbonamento.Sconto;

            // Prezzo finale = sconto * numero biglietti
            // ⚠️ Questo restituisce il valore dello sconto totale, NON il prezzo finale scontato
            Decimal prezzoFinale = prezzoScontato * numeroBiglietti;

            return prezzoFinale;
        }
    }
}
```