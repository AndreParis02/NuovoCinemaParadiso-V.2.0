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

   // Durata dell'abbonamento (giorni o mesi, in base alla logica di business)
    public int Durata { get; set; }

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

    // Prezzo dell'abbonamento
    public decimal Prezzo { get; set; }

    // Percentuale di sconto applicata (0–100)
    public int Sconto { get; set; }
}
```

## DtoProiezione 

```c#
// DTO utilizzato per trasferire i dati di una proiezione verso/da il client 

namespace NuovoCinemaParadiso.Dtos;

public class DtoProiezione
{
    // codice identificativo della proiezione
    public string Id {get; set; }
    // film della proiezione
    public string MovieId {get; set; } 
    // sala dove avverrà la proiezione
    public string SalaId {get; set; }
    // turno di intervallo di tempo dove avverrà la proiezione
    public string TurnoId {get; set; }
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

    public async Task<List<DtoAbbonamento>> OttieniTutto()
{
    // 1. Recupero tutti gli abbonamenti dal database.
    //    ToListAsync() esegue la query e restituisce una lista completa.
    List<Abbonamento> abbonamenti = await _contesto.Abbonamenti.ToListAsync();

    // 2. Creo la lista che conterrà i DTO da restituire al chiamante.
    List<DtoAbbonamento> risultato = new List<DtoAbbonamento>();

    // 3. Ciclo manualmente su ogni elemento della lista di modelli.
    //    Questo approccio è trasparente e ti permette di controllare ogni passaggio.
    for (int i = 0; i < abbonamenti.Count; i++)
    {
        // Estraggo l'abbonamento corrente
        Abbonamento abbonamentoCorrente = abbonamenti[i];

        // 4. Creo un nuovo DTO e copio manualmente ogni proprietà.
        //    Questo evita automapper, LINQ o magie nascoste.
        DtoAbbonamento dto = new DtoAbbonamento();
        dto.Id = abbonamentoCorrente.Id;
        dto.Nome = abbonamentoCorrente.Nome;
        dto.Durata = abbonamentoCorrente.Durata;
        dto.Prezzo = abbonamentoCorrente.Prezzo;
        dto.Sconto = abbonamentoCorrente.Sconto;

        // 5. Aggiungo il DTO alla lista finale.
        risultato.Add(dto);
    }

    // 6. Restituisco la lista completa dei DTO.
    return risultato;
}

public async Task<DtoAbbonamento?> OttieniTramiteIdAsync(string id, string utenteId)
{
    // 1. Cerco nel database l'abbonamento con la chiave primaria uguale a 'id'.
    //    FindAsync usa direttamente la chiave primaria e quindi è molto efficiente.
    Abbonamento? abbonamento = await _contesto.Abbonamenti.FindAsync(id);

    // 2. Se l'abbonamento non esiste, restituisco null.
    //    Questo permette al chiamante di gestire il "non trovato".
    if (abbonamento == null)
    {
        return null;
    }

    // 3. Creo un nuovo DTO e copio manualmente tutte le proprietà rilevanti.
    //    Questo evita automapper, LINQ o logiche implicite.
    DtoAbbonamento risultato = new DtoAbbonamento();
    risultato.Id = abbonamento.Id;
    risultato.Nome = abbonamento.Nome;
    risultato.Durata = abbonamento.Durata;
    risultato.Prezzo = abbonamento.Prezzo;
    risultato.Sconto = abbonamento.Sconto;

    // 4. Restituisco il DTO completamente popolato.
    return risultato;
}

public async Task<DtoAbbonamento> OttieniTramiteIdPerAdminAsync(string id)
{
    // 1. Cerco nel database l'abbonamento con chiave primaria uguale a 'id'.
    //    FindAsync è molto efficiente perché usa direttamente la PK.
    Abbonamento? abbonamento = await _contesto.Abbonamenti.FindAsync(id);

    // 2. Se l'abbonamento non esiste, restituisco null.
    //    Il metodo è dichiarato come Task<DtoAbbonamento>, ma restituisce null:
    //    tecnicamente sarebbe più corretto usare Task<DtoAbbonamento?>.
    if (abbonamento == null)
    {
        return null;
    }

    // 3. Creo un nuovo DTO e copio manualmente tutte le proprietà.
    //    Questo garantisce massima trasparenza e nessuna magia nascosta.
    DtoAbbonamento dto = new DtoAbbonamento();
    dto.Id = abbonamento.Id;
    dto.Nome = abbonamento.Nome;
    dto.Durata = abbonamento.Durata;
    dto.Prezzo = abbonamento.Prezzo;
    dto.Sconto = abbonamento.Sconto;

    // 4. Restituisco il DTO popolato.
    return dto;
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

    public async Task<DtoAbbonamento> ModificaAsync(string id, DtoCreazioneAbbonamento dto)
{
    // 1. Cerco nel database l'abbonamento con chiave primaria uguale a 'id'.
    //    FindAsync è molto efficiente perché usa direttamente la PK.
    Abbonamento? abbonamento = await _contesto.Abbonamenti.FindAsync(id);

    // 2. Se l'abbonamento non esiste, restituisco null.
    //    Nota: il metodo ritorna Task<DtoAbbonamento>, ma qui ritorni null.
    //    Tecnicamente sarebbe più corretto usare Task<DtoAbbonamento?>.
    if (abbonamento == null)
        return null;

    // 3. Aggiorno manualmente ogni proprietà dell'entità.
    //    Questo approccio è chiaro, esplicito e ti permette di controllare
    //    esattamente cosa viene modificato.
    abbonamento.Nome    = dto.Nome;
    abbonamento.Durata  = dto.Durata;
    abbonamento.Prezzo  = dto.Prezzo;
    abbonamento.Sconto  = dto.Sconto;

    // 4. Salvo le modifiche nel database.
    //    SaveChangesAsync applica gli aggiornamenti all'entità tracciata.
    await _contesto.SaveChangesAsync();

    // 5. Creo e restituisco un DTO aggiornato.
    //    Questo evita di esporre direttamente il modello EF.
    return new DtoAbbonamento
    {
        Id     = abbonamento.Id,
        Nome   = abbonamento.Nome,
        Durata = abbonamento.Durata,
        Prezzo = abbonamento.Prezzo,
        Sconto = abbonamento.Sconto,
    };
}

public async Task<bool> EliminazioneAsync(string id)
{
    // 1. Cerco nel database l'abbonamento con chiave primaria uguale a 'id'.
    //    FindAsync è molto efficiente perché usa direttamente la PK.
    Abbonamento? abbonamento = await _contesto.Abbonamenti.FindAsync(id);

    // 2. Se l'abbonamento non esiste, restituisco false.
    //    Questo indica al chiamante che non c'era nulla da eliminare.
    if (abbonamento == null)
    {
        return false;
    }

    // 3. Rimuovo l'entità dal DbSet.
    //    EF Core la marca come "Deleted" nel ChangeTracker.
    _contesto.Abbonamenti.Remove(abbonamento);

    // 4. Applico le modifiche al database.
    await _contesto.SaveChangesAsync();

    // 5. Restituisco true per indicare che l'eliminazione è avvenuta con successo.
    return true;
}
}
```

# Models
## Utente.cs
```c#
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NuovoCinemaParadiso.Models;

[Table("Utente")]
public class Utente : IdentityUser
{
    [Required]
    [StringLength(100)]
    public string NomeCompleto { get; set; } = string.Empty;
    [Required]
    [Range(14, 100, ErrorMessage = "L'età deve essere compresa tra 14 e 100")]
    public int Eta { get; set; }
    [Required]
    public bool SeAbbonato { get; set; } = false;

    public DateTime DataInizio { get; set; } = DateTime.UtcNow;
    public List<Acquisto> Acquisti { get; set; } = new List<Acquisto>();

    public string AbbonamentoId { get; set; } = string.Empty;
    [ForeignKey("AbbonamentoId")]
    public Abbonamento Abbonamento { get; set; }
}

```

# Dtos
## DtoUtente.cs
```c#
namespace NuovoCinemaParadiso.Dtos;

public class DtoUtente
{
    public string Id { get; set; } = string.Empty;
    public string NomeCompleto { get; set; } = string.Empty;
    public DateTime DataInizio {get; set;}
    public bool Abbonato {get; set;}
    public string Email { get; set; } = string.Empty;
    public int Eta { get; set; }
     public string AbbonamentoId {get; set;} = string.Empty;
    public string TipoAbbonamento {get; set;} = string.Empty;
} 
```

## DtoCreazioneUtente.cs
```c#
using System.ComponentModel.DataAnnotations;

namespace NuovoCinemaParadiso.Dtos;

public class DtoCreazioneUtente
{
    [Required]
    [StringLength(100)]
    public string NomeCompleto {get; set;} = string.Empty;
    
    [Required]
    [Range(14, 100, ErrorMessage = "L'età deve essere compresa tra 14 e 100")]
    public int Eta {get; set;} 
}
```

# Controller
## UtentiController.cs
```c#
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UtentiController : ControllerBase
{
    private readonly UtenteService _utenteService;
    private readonly LogAzioniService _logAzioniService;

    public UtentiController(UtenteService utenteService, LogAzioniService logAzioniService)
    {
        // Inietto i servizi necessari al controller.
        _utenteService = utenteService;
        _logAzioniService = logAzioniService;
    }

    // Endpoint: POST api/utenti/abbonati
    [HttpPost("abbonati")]
    public async Task<IActionResult> Abbonati([FromBody] DtoUtente dto)
    {
        // 1. Recupero l'ID dell'utente autenticato tramite il token JWT.
        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        // 2. Validazione dei dati in ingresso.
        //    Controllo che il DTO non sia null e che contenga gli ID necessari.
        if (dto == null || string.IsNullOrEmpty(dto.AbbonamentoId) || string.IsNullOrEmpty(dto.Id))
        {
            // 3. Registro nel log che l'operazione è fallita.
            await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
            {
                IdUtente = utenteId,
                NomeAzione = "Abbonati",
                Effettuato = false,
                Messaggio = "Operazione fallita"
            });

            // 4. Risposta HTTP 400: dati non validi.
            return BadRequest("Dati non validi");
        }

        // 5. Chiamo il servizio che gestisce la logica di abbonamento.
        var risultato = await _utenteService.AbbonatiAsync(dto.AbbonamentoId, dto.Id);

        // 6. Se il servizio restituisce null, significa che l'utente o l'abbonamento non esistono.
        if (risultato == null)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
            {
                IdUtente = utenteId,
                NomeAzione = "Abbonati",
                Effettuato = false,
                Messaggio = "Operazione fallita"
            });

            // Risposta HTTP 404: risorsa non trovata.
            return NotFound("Utente o abbonamento non trovato");
        }

        // 7. Se tutto è andato bene, registro l'azione come riuscita.
        await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
        {
            IdUtente = utenteId,
            NomeAzione = "Abbonati",
            Effettuato = true,
            Messaggio = "Operazione eseguita"
        });

        // 8. Risposta HTTP 200 con il risultato.
        return Ok(risultato);
    }
}

```

# Service
## UtenteService.cs
```c#
public async Task<DtoUtente> AbbonatiAsync(string abbonamentoId, string utenteId)
{
    // 1. Recupero tutti gli abbonamenti dal database.
    //    Non usi LINQ, quindi fai un ToListAsync e poi cerchi manualmente.
    List<Abbonamento> abbonamenti = await _contesto.Abbonamenti.ToListAsync();
    Abbonamento? abbonamentoTrovato = null;

    // 2. Ciclo manuale per trovare l'abbonamento con l'ID richiesto.
    for (int i = 0; i < abbonamenti.Count; i++)
    {
        Abbonamento abbonamentoCorrente = abbonamenti[i];

        if (abbonamentoCorrente.Id == abbonamentoId)
        {
            abbonamentoTrovato = abbonamentoCorrente;
            break; // appena trovato, esco dal ciclo
        }
    }

    // 3. Se non ho trovato l'abbonamento, interrompo e restituisco null.
    if (abbonamentoTrovato == null)
    {
        return null;
    }

    // 4. Recupero tutti gli utenti dal database.
    List<Utente> utenti = await _contesto.Utenti.ToListAsync();
    Utente? utenteTrovato = null;

    // 5. Ciclo manuale per trovare l'utente con l'ID richiesto.
    for (int i = 0; i < utenti.Count; i++)
    {
        if (utenti[i].Id == utenteId)
        {
            utenteTrovato = utenti[i];
            break;
        }
    }

    // 6. Se l'utente non esiste, restituisco null.
    if (utenteTrovato == null)
    {
        return null;
    }

    // 7. Aggiorno i campi dell'utente per segnare l'abbonamento.
    utenteTrovato.AbbonamentoId = abbonamentoTrovato.Id;
    utenteTrovato.SeAbbonato = true;
    utenteTrovato.DataInizio = DateTime.UtcNow;

    // 8. Salvo le modifiche nel database.
    await _contesto.SaveChangesAsync();

    // 9. Restituisco un DTO completo dell'utente aggiornato.
    return new DtoUtente()
    {
        Id = utenteTrovato.Id,
        NomeCompleto = utenteTrovato.NomeCompleto,
        Email = utenteTrovato.Email,
        Eta = utenteTrovato.Eta,
        Abbonato = utenteTrovato.SeAbbonato,
        DataInizio = utenteTrovato.DataInizio,
        AbbonamentoId = utenteTrovato.AbbonamentoId,
        TipoAbbonamento = abbonamentoTrovato.Nome
    };
}
```

# Controllers
## AdminController.cs 
```c#
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using NuovoCinemaParadiso.Services;
using NuovoCinemaParadiso.Dtos;
using NuovoCinemaParadiso.Models;

namespace NuovoCinemaParadiso.Controllers;

[ApiController] 
// Indica che questo controller gestisce API REST e abilita funzionalità automatiche
// come la validazione del modello e il binding dei parametri.
[Route("api/[controller]")]
// La route diventa: api/admin
[Authorize]
// Richiede che l’utente sia autenticato per accedere a qualsiasi endpoint del controller.
public class AdminController : ControllerBase
{
    private readonly AdminService _adminService;
    private readonly LogAzioniService _logAzioniService;

    public AdminController(AdminService adminService, LogAzioniService logAzioniService)
    {
        // Iniezione dei servizi necessari al controller.
        _adminService = adminService;
        _logAzioniService = logAzioniService;
    }

    // ------------------------------------------------------------
    // 1) OTTIENI TUTTI I PROFILI UTENTE
    // ------------------------------------------------------------
    [HttpGet("ListaUtenti")]
    [Authorize(Roles = Ruoli.GestoreOrOperatore)]
    // Solo Gestore e Operatore possono accedere.
    public async Task<IActionResult> OttieniTuttiIProfili()
    {
        // Recupero tutti gli utenti tramite il servizio Admin.
        List<DtoUtente> utenti = await _adminService.OttieniUtentiAsync();

        // Recupero l’ID dell’utente autenticato dal token JWT.
        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        // Registro nel log che la ricerca è stata effettuata.
        await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
        {
            IdUtente = utenteId,
            NomeAzione = "Ricerca profili",
            Effettuato = true,
            Messaggio = "Ricerca avvenuta"
        });

        // Secondo log (puoi unificarli se vuoi).
        await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
        {
            IdUtente = utenteId,
            NomeAzione = "Ottieni tutti i profili",
            Effettuato = true,
            Messaggio = "Operazione eseguita"
        });

        // Restituisco la lista degli utenti.
        return Ok(utenti);
    }

    // ------------------------------------------------------------
    // 2) RICERCA PROFILO TRAMITE ID
    // ------------------------------------------------------------
    [HttpGet("ricercaProfilo/{id}")]
    [Authorize(Roles = Ruoli.GestoreOrOperatore)]
    public async Task<IActionResult> RicercaProfiloTramiteId(string id)
    {
        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        // Recupero l’utente tramite ID.
        DtoUtente? utente = await _adminService.OttieniUtenteTramiteIdAsync(id);

        // Se non trovato → log fallimento + 404.
        if (utente == null)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
            {
                IdUtente = utenteId,
                NomeAzione = "Ricerca profilo",
                Effettuato = false,
                Messaggio = "Ricerca fallita"
            });

            return NotFound(new { messaggio = "Utente non trovato." });
        }

        // Log successo.
        await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
        {
            IdUtente = utenteId,
            NomeAzione = "Ricerca profilo",
            Effettuato = true,
            Messaggio = "Ricerca avvenuta"
        });

        return Ok(utente);
    }

    // ------------------------------------------------------------
    // 3) ELIMINA UTENTE TRAMITE ID
    // ------------------------------------------------------------
    [HttpDelete("eliminaUtente/{id}")]
    [Authorize(Roles = Ruoli.GestoreOrOperatore)]
    public async Task<IActionResult> EliminaTramiteId(string Id)
    {
        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        // Chiamo il servizio per eliminare l’utente.
        var risultato = await _adminService.EliminaUtentePerIdAsync(Id);

        // Se non trovato → log fallimento + 404.
        if (risultato == null)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
            {
                IdUtente = utenteId,
                NomeAzione = "Eliminazione profilo",
                Effettuato = false,
                Messaggio = "Eliminazione profilo fallita"
            });

            return NotFound(new { messaggio = "Utente non trovato." });
        }

        // Log successo.
        await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
        {
            IdUtente = utenteId,
            NomeAzione = "Eliminazione profilo",
            Effettuato = true,
            Messaggio = "Eliminazione profilo avvenuta"
        });

        return Ok(risultato);
    }

    // ------------------------------------------------------------
    // 4) OTTIENI TUTTI GLI ACQUISTI
    // ------------------------------------------------------------
    [HttpGet("acquisti")]
    [Authorize(Roles = Ruoli.GestoreOrOperatore)]
    public async Task<IActionResult> OttieniTuttiGliAcquisti()
    {
        // Recupero tutti gli acquisti.
        List<DtoAcquisto> acquisti = await _adminService.OttieniAcquisti();

        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        // Log dell’operazione.
        await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
        {
            IdUtente = utenteId,
            NomeAzione = "Ottieni tutti gli acquisti admin",
            Effettuato = true,
            Messaggio = "Operazione eseguita"
        });

        return Ok(acquisti);
    }

    // ------------------------------------------------------------
    // 5) OTTIENI ACQUISTO TRAMITE ID
    // ------------------------------------------------------------
    [HttpGet("acquisto/{id}")]
    [Authorize(Roles = Ruoli.GestoreOrOperatore)]
    public async Task<IActionResult> OttieniTramiteIdPerAdmin(string id)
    {
        var risultato = await _adminService.OttieniAcquistoTramiteIdAsync(id);
        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        // Se non trovato → log fallimento + 404.
        if (risultato == null)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
            {
                IdUtente = utenteId,
                NomeAzione = "Ottieni acquisti tramite id admin",
                Effettuato = false,
                Messaggio = "Operazione fallita"
            });

            return NotFound($"Acquisto con id {id} non trovato");
        }

        // Log successo.
        await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
        {
            IdUtente = utenteId,
            NomeAzione = "Ottieni acquisti tramite id admin",
            Effettuato = true,
            Messaggio = "Operazione eseguita"
        });

        return Ok(risultato);
    }

    // ------------------------------------------------------------
    // 6) OTTIENI UTENTI TRAMITE ABBONAMENTO
    // ------------------------------------------------------------
    [HttpGet("abbonamento/{id}")]
    [Authorize(Roles = Ruoli.GestoreOrOperatore)]
    public async Task<ActionResult<List<DtoUtente>>> OttieniTramiteAbbonamentoAsync(string abbonamentoId)
    {
        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        // Validazione dell’ID.
        if (string.IsNullOrWhiteSpace(abbonamentoId))
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
            {
                IdUtente= utenteId,
                NomeAzione = "Ottieni gli utenti per abbonamento",
                Effettuato = false,
                Messaggio = "Operazione fallita"
            });

            return BadRequest("AbbonamentoId non valido");
        }

        // Recupero gli utenti con quell’abbonamento.
        var risultato = await _adminService.OttieniTramiteAbbonamentoAsync(abbonamentoId);

        // Se nessuno trovato → log fallimento + 404.
        if (risultato == null || risultato.Count == 0)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
            {
                IdUtente = utenteId,
                NomeAzione = "Ottieni gli utenti per abbonamento",
                Effettuato = false,
                Messaggio = "Operazione fallita"
            });

            return NotFound("Nessun utente trovato per questo abbonamento");
        }

        // Log successo.
        await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
        {
            IdUtente = utenteId,
            NomeAzione = "Ottieni gli utenti per abbonamento",
            Effettuato = true,
            Messaggio = "Operazione eseguita"
        });

        return Ok(risultato);
    }
}
```

# Services
## AdminService.cs
```c#
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NuovoCinemaParadiso.Data;
using NuovoCinemaParadiso.Dtos;
using NuovoCinemaParadiso.Models;
using NuovoCinemaParadiso.Helpers;

namespace NuovoCinemaParadiso.Services;

public class AdminService
{
    private readonly ContestoDb _contesto;
    private readonly UserManager<Utente> _gestioneUtenti;

    public AdminService(ContestoDb contestoDb)
    {
        // Inietto il contesto del database.
        _contesto = contestoDb;

        // ATTENZIONE: _gestioneUtenti non viene inizializzato qui.
        // Dovresti iniettarlo nel costruttore per evitare NullReferenceException.
    }
    
    // ------------------------------------------------------------
    // 1) OTTIENI TUTTI GLI UTENTI
    // ------------------------------------------------------------
    public async Task<List<DtoUtente>> OttieniUtentiAsync()
    {
        // Recupero tutti gli utenti dal database.
        List<Utente> utenti = await _contesto.Utenti.ToListAsync();

        // Creo la lista dei DTO da restituire.
        List<DtoUtente> risultato = new List<DtoUtente>();

        // Ciclo manuale per convertire ogni Utente in DtoUtente.
        for (int i = 0; i < utenti.Count; i++)
        {
            Utente utenteCorrente = utenti[i];

            DtoUtente dto = new DtoUtente();
            dto.Id = utenteCorrente.Id;
            dto.Email = utenteCorrente.Email ?? string.Empty;
            dto.NomeCompleto = utenteCorrente.NomeCompleto ?? string.Empty;
            dto.Eta = utenteCorrente.Eta;

            risultato.Add(dto);
        }

        return risultato;
    }

    // ------------------------------------------------------------
    // 2) OTTIENI UTENTE TRAMITE ID
    // ------------------------------------------------------------
    public async Task<DtoUtente?> OttieniUtenteTramiteIdAsync(string id)
    {
        // Uso UserManager per cercare l’utente tramite ID.
        Utente? utente = await _gestioneUtenti.FindByIdAsync(id);

        // Se non esiste → restituisco null.
        if (utente == null)
        {
            return null;
        }

        // Converto l’utente in DTO.
        DtoUtente dto = new DtoUtente();
        dto.Id = utente.Id;
        dto.Email = utente.Email ?? string.Empty;
        dto.NomeCompleto = utente.NomeCompleto ?? string.Empty;
        dto.Eta = utente.Eta;

        return dto;
    }

    // ------------------------------------------------------------
    // 3) ELIMINA UTENTE TRAMITE ID
    // ------------------------------------------------------------
    public async Task<IdentityResult> EliminaUtentePerIdAsync(string id)
    {
        // Cerco l’utente tramite UserManager.
        Utente? utente = await _gestioneUtenti.FindByIdAsync(id);

        // Se non esiste → ritorno un errore IdentityResult.
        if (utente == null)
        {
            IdentityError errore = new IdentityError();
            return IdentityResult.Failed(errore);
        }

        // Elimino l’utente tramite Identity.
        IdentityResult risultato = await _gestioneUtenti.DeleteAsync(utente);

        return risultato;
    }

    // ------------------------------------------------------------
    // 4) OTTIENI TUTTI GLI ACQUISTI
    // ------------------------------------------------------------
    public async Task<List<DtoAcquisto>> OttieniAcquisti()
    {
        // Recupero tutti gli acquisti.
        List<Acquisto> acquisti = await _contesto.Acquisti.ToListAsync();

        List<DtoAcquisto> risultato = new List<DtoAcquisto>();

        // Ciclo manuale su ogni acquisto.
        for (int i = 0; i < acquisti.Count; i++)
        {
            Acquisto acquistoCorrente = acquisti[i];

            // Recupero manualmente tutte le entità collegate.
            Movie? movie = await _contesto.Movies.FindAsync(acquistoCorrente.MovieId);
            Sala? sala = await _contesto.Sale.FindAsync(acquistoCorrente.SalaId);
            Utente? utente = await _contesto.Utenti.FindAsync(acquistoCorrente.UtenteId);
            TipologiaSala? tipologiaSala = await _contesto.TipologieSala.FindAsync(sala.TipologiaSalaId);

            // Creo il DTO dell’acquisto.
            DtoAcquisto dto = new DtoAcquisto();
            dto.Id = acquistoCorrente.Id;
            dto.MovieId = acquistoCorrente.MovieId;
            dto.Titolo = movie.Titolo;
            dto.SalaId = acquistoCorrente.SalaId;
            dto.Nome = sala.Nome;
            dto.UtenteId = acquistoCorrente.UtenteId;
            dto.NomeCompleto = utente.NomeCompleto;
            dto.PrezzoFinale = CalcolaPrezzo.CalcolaPrezzoFinale(
                movie.PrezzoMovie,
                tipologiaSala.MaggiorazionePrezzo,
                acquistoCorrente.NumeroBiglietti,
                utente
            );
            dto.OrarioCreazione = acquistoCorrente.OrarioCreazione;
            dto.NumeroBiglietti = acquistoCorrente.NumeroBiglietti;

            risultato.Add(dto);
        }

        return risultato;
    }

    // ------------------------------------------------------------
    // 5) OTTIENI ACQUISTO TRAMITE ID
    // ------------------------------------------------------------
    public async Task<DtoAcquisto> OttieniAcquistoTramiteIdAsync(string id)
    {
        // Recupero l’acquisto tramite ID.
        Acquisto? acquisto = await _contesto.Acquisti.FindAsync(id);

        // Se non esiste → restituisco null.
        if (acquisto == null)
        {
            return null;
        }

        // Recupero manualmente tutte le entità collegate.
        Movie? movie = await _contesto.Movies.FindAsync(acquisto.MovieId);
        Sala? sala = await _contesto.Sale.FindAsync(acquisto.SalaId);
        Utente? utente = await _contesto.Users.FindAsync(acquisto.UtenteId);
        TipologiaSala? tipologiaSala = await _contesto.TipologieSala.FindAsync(sala.TipologiaSalaId);

        // Creo il DTO.
        DtoAcquisto dto = new DtoAcquisto();
        dto.Id = acquisto.Id;
        dto.MovieId = acquisto.MovieId;
        dto.Titolo = movie.Titolo;
        dto.SalaId = acquisto.SalaId;
        dto.Nome = sala.Nome;
        dto.UtenteId = acquisto.UtenteId;
        dto.NomeCompleto = utente.NomeCompleto;
        dto.PrezzoFinale = CalcolaPrezzo.CalcolaPrezzoFinale(
            movie.PrezzoMovie,
            tipologiaSala.MaggiorazionePrezzo,
            acquisto.NumeroBiglietti,
            utente
        );
        dto.OrarioCreazione = acquisto.OrarioCreazione;
        dto.NumeroBiglietti = acquisto.NumeroBiglietti;

        return dto;
    }

    // ------------------------------------------------------------
    // 6) OTTIENI UTENTI TRAMITE ABBONAMENTO
    // ------------------------------------------------------------
    public async Task<List<DtoUtente>> OttieniTramiteAbbonamentoAsync(string abbonamentoId)
    {
        // Recupero tutti gli utenti e tutti gli abbonamenti.
        List<Utente> utenti = await _contesto.Utenti.ToListAsync();
        List<Abbonamento> abbonamenti = await _contesto.Abbonamenti.ToListAsync();

        Abbonamento? abbonamentoTrovato = null;

        // Cerco manualmente l’abbonamento richiesto.
        for (int i = 0; i < abbonamenti.Count; i++)
        {
            Abbonamento abbonamentoCorrente = abbonamenti[i];

            if (abbonamentoCorrente.Id == abbonamentoId)
            {
                abbonamentoTrovato = abbonamentoCorrente;
                break;
            }
        }

        // Se non esiste → restituisco lista vuota.
        if (abbonamentoTrovato == null)
        {
            return new List<DtoUtente>();
        }

        List<DtoUtente> risultato = new List<DtoUtente>();

        // Cerco manualmente gli utenti che hanno quell’abbonamento.
        for (int i = 0; i < utenti.Count; i++)
        {
            Utente utenteCorrente = utenti[i];

            // Confronto diretto tra oggetti (funziona perché EF traccia le entità).
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