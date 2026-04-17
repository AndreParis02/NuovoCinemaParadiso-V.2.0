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

## Proiezione.cs
```c#
[Table("Proiezioni")]
public class Proiezione
{
    // Identificativo univoco della proiezione (stringa perché usi GUID).
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString();

    // Data in cui avviene la proiezione (solo data, niente orario).
    [Required]
    public DateOnly DataProiezione { get; set; }

    // Riferimento al film proiettato.
    [Required]
    public string MovieId { get; set; } = string.Empty;

    // Navigazione verso il film.
    [ForeignKey("MovieId")]
    public Movie Movie { get; set; }

    // Riferimento alla sala in cui avviene la proiezione.
    [Required]
    public string SalaId { get; set; } = string.Empty;

    // Navigazione verso la sala.
    [ForeignKey("SalaId")]
    public Sala Sala { get; set; }

    // Riferimento al turno (fascia oraria).
    [Required]
    public string TurnoId { get; set; } = string.Empty;

    // Navigazione verso il turno.
    [ForeignKey("TurnoId")]
    public Utente Turno { get; set; }
}
```

## Utente.cs
```c#
[Table("Utente")]
public class Utente : IdentityUser
{
    // Nome completo dell’utente (obbligatorio, max 100 caratteri).
    [Required]
    [StringLength(100)]
    public string NomeCompleto { get; set; } = string.Empty;

    // Età dell’utente con vincolo minimo e massimo.
    [Required]
    [Range(14, 100)]
    public int Eta { get; set; }

    // Indica se l’utente ha un abbonamento attivo.
    [Required]
    public bool SeAbbonato { get; set; } = false;

    // Data di inizio dell’abbonamento (se presente).
    public DateTimeOffset DataInizioAbbonamento { get; set; }

    // Acquisti associati all’utente.
    public List<Acquisto> Acquisti { get; set; } = new List<Acquisto>();

    // Riferimento all’abbonamento (può essere nullo).
    public string? AbbonamentoId { get; set; }

    // Navigazione verso l’entità Abbonamento.
    [ForeignKey("AbbonamentoId")]
    public Abbonamento? Abbonamento { get; set; }
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
    //data in cui avverrà la proiezione
    public DateOnly DataProiezione {get; set; }
    // film della proiezione
    public string MovieId {get; set; } 
    // sala dove avverrà la proiezione
    public string SalaId {get; set; }
    // turno di intervallo di tempo dove avverrà la proiezione
    public string TurnoId {get; set; }
}
```
## DtoAcquisto

```c#
namespace NuovoCinemaParadiso.Dtos;

public class DtoAcquisto
{
    // Identificativo univoco dell'acquisto (GUID o stringa generata dal DB)
    public string Id { get; set; }

    // Identificativo della proiezione associata all'acquisto
    // Serve per collegare l'acquisto alla proiezione scelta dall'utente
    public string ProiezioneId { get; set; } = string.Empty;

    // Identificativo dell'utente che ha effettuato l'acquisto
    public string UtenteId { get; set; } = string.Empty;

    // Prezzo finale calcolato (prezzo base + maggiorazioni * numero biglietti)
    public decimal PrezzoFinale { get; set; }

    // Timestamp di creazione dell'acquisto
    // Usare DateTimeOffset garantisce correttezza rispetto ai fusi orari
    public DateTimeOffset OrarioCreazione { get; set; }

    // Numero di biglietti acquistati in questa transazione
    public int NumeroBiglietti { get; set; }
}
```

## DtoCreazioneAcquisto

```c#
using System.ComponentModel.DataAnnotations;

namespace NuovoCinemaParadiso.Dtos;

public class DtoCreazioneAcquisto
{
    // Identificativo della proiezione scelta dall'utente.
    // È obbligatorio perché l'acquisto deve sempre riferirsi a una proiezione valida.
    [Required]
    public string ProiezioneId { get; set; } = string.Empty;

    // Identificativo dell'utente che effettua l'acquisto.
    // Viene passato dal client, ma nel controller puoi anche sovrascriverlo
    // con l'utente autenticato per maggiore sicurezza.
    [Required]
    public string? UtenteId { get; set; } = string.Empty;

    // Numero di biglietti acquistati.
    // Deve essere >= 1, ma questo controllo può essere aggiunto con [Range].
    [Required]
    public int NumeroBiglietti { get; set; }

    // Prezzo finale calcolato lato server.
    // Nota: spesso NON si fa passare dal client per evitare manipolazioni,
    // ma si ricalcola nel backend usando i dati della proiezione.
    [Required]
    public decimal PrezzoFinale { get; set; }
}
```

## DtoCreazioneProiezione.cs
```c#
// DTO utilizzato per la creazione di una nuova proiezione.
// Contiene solo i campi necessari in input dal client.
namespace NuovoCinemaParadiso.Dtos;
public class DtoCreazioneProiezione
{
   // Identificativo del film da proiettare.
    // Viene passato come stringa perché probabilmente il tuo sistema usa GUID o ID non numerici.
    public string MovieId { get; set; } = string.Empty;

    // Identificativo della sala in cui avverrà la proiezione.
    // Anche qui stringa per coerenza con il resto del progetto.
    public string SalaId { get; set; } = string.Empty;

    // Identificativo del turno (es. mattina, pomeriggio, sera).
    // Serve per collegare la proiezione a una fascia oraria predefinita.
    public string TurnoId { get; set; } = string.Empty;

    // Data della proiezione.
    // Usi DateOnly perché ti interessa solo la data, non l'orario
    public DateOnly DataProiezione { get; set; }
}
```
## DtoUtente.cs
```c#
namespace NuovoCinemaParadiso.Dtos;

public class DtoUtente
{
    public string Id { get; set; } = string.Empty;
    public string NomeCompleto { get; set; } = string.Empty;
    public DateTimeOffset DataInizio {get; set;}
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
[Authorize] // ✔ Richiede autenticazione per tutte le azioni del controller
public class AbbonamentoController : ControllerBase
{
    private readonly AbbonamentoService _abbonamentoService;
    private readonly AdminService _adminService;
    private readonly LogAzioniService _logAzioniService;

    // ✔ Iniezione dei servizi necessari al controller
    public AbbonamentoController(AbbonamentoService abbonamentoService, LogAzioniService logAzioniService, AdminService adminService)
    {
        _abbonamentoService = abbonamentoService;
        _logAzioniService = logAzioniService;
        _adminService = adminService;
    }

    // ------------------------------------------------------------
    // GET api/abbonamento
    // ✔ Restituisce tutti gli abbonamenti dell’utente autenticato
    // ------------------------------------------------------------
    [HttpGet]
    public async Task<IActionResult> OttieniTuttiGliAbbonamenti()
    {
        // ✔ Recupera l'ID dell'utente dal token JWT
        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        // ✔ Ottiene tutti gli abbonamenti (filtrati nel service)
        List<DtoAbbonamento> abbonamenti = await _abbonamentoService.OttieniTutto();

        // ✔ Log dell’azione
        await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
        {
            IdUtente = utenteId,
            NomeAzione = "Ottieni tutti gli abbonamenti utente",
            Effettuato = true,
            Messaggio = "Operazione eseguita"
        });

        return Ok(abbonamenti);
    }

    // ------------------------------------------------------------
    // GET api/abbonamento/admin/{id}
    // ✔ Endpoint riservato a Gestore/Operatore
    // ✔ Permette di ottenere un abbonamento tramite ID senza limiti
    // ------------------------------------------------------------
    [HttpGet("admin/{id}")]
    [Authorize(Roles = Ruoli.GestoreOrOperatore)]
    public async Task<IActionResult> OttieniTramiteIdPerAdmin(string id)
    {
        var risultato = await _adminService.OttieniTramiteIdPerAdminAsync(id);
        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (risultato == null)
        {
            // ❌ Log fallimento
            await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
            {
                IdUtente = utenteId,
                NomeAzione = "Ottieni abbonamenti tramite id admin",
                Effettuato = false,
                Messaggio = "Operazione fallita"
            });

            return NotFound($"Abbonamento con id {id} non trovato");
        }

        // ✔ Log successo
        await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
        {
            IdUtente = utenteId,
            NomeAzione = "Ottieni abbonamenti tramite id admin",
            Effettuato = true,
            Messaggio = "Operazione eseguita"
        });

        return Ok(risultato);
    }

    // ------------------------------------------------------------
    // GET api/abbonamento/{id}
    // ✔ Restituisce un abbonamento SOLO se appartiene all’utente
    // ------------------------------------------------------------
    [HttpGet("{id}")]
    public async Task<IActionResult> OttieniTramiteIdPerUtente(string id)
    {
        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        // ✔ Il service applica il controllo di proprietà
        var risultato = await _abbonamentoService.OttieniTramiteIdAsync(id, utenteId);

        if (risultato == null)
        {
            // ❌ Log fallimento
            await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
            {
                IdUtente = utenteId,
                NomeAzione = "Ottieni abbonamenti tramite id utente",
                Effettuato = false,
                Messaggio = "Operazione fallita"
            });

            return NotFound($"Abbonamento con id {id} non trovato");
        }

        // ✔ Log successo
        await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
        {
            IdUtente = utenteId,
            NomeAzione = "Ottieni abbonamenti tramite id utente",
            Effettuato = true,
            Messaggio = "Operazione eseguita"
        });

        return Ok(risultato);
    }

    // ------------------------------------------------------------
    // POST api/abbonamento
    // ✔ Creazione di un nuovo abbonamento (solo Gestore/Operatore)
    // ------------------------------------------------------------
    [HttpPost]
    [Authorize(Roles = Ruoli.GestoreOrOperatore)]
    public async Task<IActionResult> Creazione([FromBody] DtoCreazioneAbbonamento dto)
    {
        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        // ✔ Creazione tramite service
        DtoAbbonamento? risultato = await _abbonamentoService.CreazioneAsync(dto);

        if (risultato == null)
        {
            // ❌ Log fallimento
            await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
            {
                IdUtente = utenteId,
                NomeAzione = "Creazione abbonamento",
                Effettuato = false,
                Messaggio = "Operazione fallita"
            });

            return BadRequest(new { messaggio = "Abbonamento già presente oppure non valido." });
        }

        // ✔ Log successo
        await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
        {
            IdUtente = utenteId,
            NomeAzione = "Creazione abbonamento",
            Effettuato = true,
            Messaggio = "Operazione eseguita"
        });

        return Ok(risultato);
    }

    // ------------------------------------------------------------
    // PUT api/abbonamento/{id}
    // ✔ Modifica di un abbonamento esistente
    // ------------------------------------------------------------
    [HttpPut("{id}")]
    [Authorize(Roles = Ruoli.GestoreOrOperatore)]
    public async Task<IActionResult> Modifica(string id, [FromBody] DtoCreazioneAbbonamento dto)
    {
        DtoAbbonamento? risultato = await _abbonamentoService.ModificaAsync(id, dto);
        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (risultato == null)
        {
            // ❌ Log fallimento
            await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
            {
                IdUtente = utenteId,
                NomeAzione = "Modifica abbonamento",
                Effettuato = false,
                Messaggio = "Operazione fallita"
            });

            return NotFound(new { messaggio = "Abbonamento non trovato." });
        }

        // ✔ Log successo
        await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
        {
            IdUtente = utenteId,
            NomeAzione = "Modifica abbonamento",
            Effettuato = true,
            Messaggio = "Operazione eseguita"
        });

        return Ok(risultato);
    }

    // ------------------------------------------------------------
    // DELETE api/abbonamento/{id}
    // ✔ Eliminazione di un abbonamento
    // ------------------------------------------------------------
    [HttpDelete("{id}")]
    [Authorize(Roles = Ruoli.GestoreOrOperatore)]
    public async Task<IActionResult> Elimina(string id)
    {
        bool eliminato = await _abbonamentoService.EliminazioneAsync(id);
        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!eliminato)
        {
            // ❌ Log fallimento
            await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
            {
                IdUtente = utenteId,
                NomeAzione = "Elimina abbonamento",
                Effettuato = false,
                Messaggio = "Operazione fallita"
            });

            return NotFound(new { messaggio = "Abbonamento non trovato." });
        }

        // ✔ Log successo
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
## ProiezioneController.cs
```c#
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProiezioneController : ControllerBase
{
    // Servizi principali: gestione proiezioni e logging azioni.
    private readonly ProiezioneService _proiezioneService;
    private readonly LogAzioniService _logAzioniService;

    public ProiezioneController(ProiezioneService proiezioneService, LogAzioniService logAzioniService)
    {
        _proiezioneService = proiezioneService;
        _logAzioniService = logAzioniService;
    }

    // Restituisce tutte le proiezioni presenti nel sistema.
    [HttpGet]
    public async Task<IActionResult> OttieniTutteLeProiezioni()
    {
        List<DtoProiezione> proiezioni = await _proiezioneService.OttieniTuttoAsync();
        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
        {
            IdUtente = utenteId,
            NomeAzione = "Ottieni tutte le proiezioni",
            Effettuato = true,
            Messaggio = "Operazione eseguita"
        });

        return Ok(proiezioni);
    }

    // Restituisce una singola proiezione tramite ID.
    [HttpGet("{id}")]
    public async Task<IActionResult> OttieniTramiteId(string id)
    {
        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var risultato = await _proiezioneService.OttieniTramiteIdAsync(id);

        if (risultato == null)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
            {
                IdUtente = utenteId,
                NomeAzione = "Ottieni proiezione tramite id",
                Effettuato = false,
                Messaggio = "Operazione fallita"
            });

            return NotFound($"Proiezione con id {id} non trovato");
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
        {
            IdUtente = utenteId,
            NomeAzione = "Ottieni proiezione tramite id",
            Effettuato = true,
            Messaggio = "Operazione eseguita"
        });

        return Ok(risultato);
    }

    // Restituisce tutte le proiezioni associate a un turno specifico.
    [HttpGet("turno/{turnoId}")]
    public async Task<ActionResult<List<DtoProiezione>>> OttieniPerTurno(string turnoId)
    {
        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(turnoId))
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
            {
                IdUtente = utenteId,
                NomeAzione = "Ottieni proiezioni per turno",
                Effettuato = false,
                Messaggio = "Operazione fallita"
            });

            return BadRequest("TurnoId non valido");
        }

        var risultato = await _proiezioneService.OttieniTramiteTurno(turnoId);

         if (risultato == null)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
            {
                IdUtente = utenteId,
                NomeAzione = "Ottieni proiezioni per turno",
                Effettuato = false,
                Messaggio = "Operazione fallita"
            });

            return NotFound("Nessuna proiezione trovata per questo turno");

        await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
        {
            IdUtente = utenteId,
            NomeAzione = "Ottieni proiezioni per turno",
            Effettuato = true,
            Messaggio = "Operazione eseguita"
        });

        return Ok(risultato);
    }

    // Endpoint GET: restituisce tutte le proiezioni associate a una sala specifica
[HttpGet("sala/{salaId}")]
public async Task<ActionResult<List<DtoProiezione>>> OttieniPerSala(string salaId)
{
    // Recupera l'ID dell'utente autenticato dai claims (token JWT)
    string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

    // Controllo di validità del parametro salaId
    if (string.IsNullOrEmpty(salaId))
    {
        // Log dell'azione fallita
        await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
        {
            IdUtente = utenteId,
            NomeAzione = "Ottieni proiezioni per sala",
            Effettuato = false,
            Messaggio = "Operazione fallita"
        });

        // Restituisce errore 400 Bad Request
        return BadRequest("SalaId non valido");
    }

    // Chiama il service per ottenere le proiezioni della sala
    var risultato = await _proiezioneService.OttieniTramiteSalaAsync(salaId);

    // Se non viene trovato nulla
    if (risultato == null)
    {
        // Log dell'azione fallita
        await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
        {
            IdUtente = utenteId,
            NomeAzione = "Ottieni proiezioni per sala",
            Effettuato = false,
            Messaggio = "Operazione fallita"
        });

        // Restituisce errore 404 Not Found
        return NotFound("Nessuna proiezione trovata per questa sala");
    }

    // Log dell'azione completata con successo
    await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
    {
        IdUtente = utenteId,
        NomeAzione = "Ottieni proiezioni per sala",
        Effettuato = true,
        Messaggio = "Operazione eseguita"
    });

    // Restituisce risultato con status 200 OK
    return Ok(risultato);
}


// Endpoint GET: restituisce tutte le proiezioni associate a un film specifico
[HttpGet("movie/{movieId}")]
public async Task<ActionResult<List<DtoProiezione>>> OttieniPerFilm(string movieId)
{
    // Recupera l'ID dell'utente autenticato
    string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

    // Controllo di validità del parametro movieId
    if(string.IsNullOrEmpty(movieId))
    {
        // Log dell'azione fallita
        await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
        {
            IdUtente = utenteId,
            NomeAzione = "Ottieni proiezioni per film",
            Effettuato = false,
            Messaggio = "Operazione fallita"
        });

        // Restituisce errore 400 Bad Request
        return BadRequest("MovieId non valido");
    }

    // Chiama il service per ottenere le proiezioni del film
    var risultato = await _proiezioneService.OttieniTramiteMovieAsync(movieId);

    // Se non viene trovato nulla
    if (risultato == null)
    {
        // Log dell'azione fallita
        await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
        {
            IdUtente = utenteId,
            NomeAzione = "Ottieni proiezioni per film",
            Effettuato = false,
            Messaggio = "Operazione fallita"
        });

        // Restituisce errore 404 Not Found
        return NotFound("Nessuna proiezione trovata per questo film");
    }

    // Log dell'azione completata con successo
    await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
    {
        IdUtente = utenteId,
        NomeAzione = "Ottieni proiezioni per film",
        Effettuato = true,
        Messaggio = "Operazione eseguita"
    });

    // Restituisce risultato con status 200 OK
    return Ok(risultato);
}

    // Crea una nuova proiezione, con controllo duplicati.
    [HttpPost]
    [Authorize(Roles = Ruoli.GestoreOrOperatore)]
    public async Task<IActionResult> Creazione([FromBody] DtoCreazioneProiezione dto)
    {
        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        List<DtoProiezione> proiezioni = await _proiezioneService.OttieniTuttoAsync();

        // Controllo duplicati: stessa sala, stesso turno, stessa data.
        foreach (var proiezione in proiezioni)
        {
            if (proiezione.TurnoId == dto.TurnoId &&
                proiezione.SalaId == dto.SalaId &&
                proiezione.DataProiezione == dto.DataProiezione)
            {
                return BadRequest(new { messaggio = "Proiezione già presente." });
            }
        }

        DtoProiezione? risultato = await _proiezioneService.CreazioneAsync(dto);

        if (risultato == null)
            return BadRequest(new { messaggio = "Proiezione non valida." });

        return Ok(risultato);
    }

    // Modifica una proiezione esistente.
    [HttpPut("{id}")]
    [Authorize(Roles = Ruoli.GestoreOrOperatore)]
    public async Task<IActionResult> Modifica(string id, [FromBody] DtoCreazioneMovie dto)
    {
        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        DtoProiezione? risultato = await _proiezioneService.ModificaAsync(id, dto);

        if (risultato == null)
            return NotFound(new { messaggio = "Proiezione non trovata." });

        return Ok(risultato);
    }

    // Elimina una proiezione tramite ID.
    [HttpDelete("{id}")]
    [Authorize(Roles = Ruoli.GestoreOrOperatore)]
    public async Task<IActionResult> Elimina(string id)
    {
        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        bool eliminato = await _proiezioneService.EliminaAsync(id);

        if (!eliminato)
            return NotFound(new { messaggio = "Proiezione non trovato." });

        return NoContent();
    }
}
```


# Service

## AbbonamentoService.cs

```c#
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using NuovoCinemaParadiso.Data;
using NuovoCinemaParadiso.Dtos;
using NuovoCinemaParadiso.Models;

namespace NuovoCinemaParadiso.Services;

public class AbbonamentoService
{
    private readonly ContestoDb _contesto;

    // ✔ Iniezione del DbContext per accedere al database
    public AbbonamentoService(ContestoDb contesto)
    {
        _contesto = contesto;
    }

    // ------------------------------------------------------------
    // ✔ Restituisce tutti gli abbonamenti presenti nel sistema
    // ✔ Conversione manuale in DTO (senza LINQ)
    // ------------------------------------------------------------
    public async Task<List<DtoAbbonamento>> OttieniTutto()
    {
        List<Abbonamento> abbonamenti = await _contesto.Abbonamenti.ToListAsync();
        List<DtoAbbonamento> risultato = new List<DtoAbbonamento>();

        // ✔ Conversione iterativa in DTO
        for (int i = 0; i < abbonamenti.Count; i++)
        {
            Abbonamento abbonamentoCorrente = abbonamenti[i];

            DtoAbbonamento dto = new DtoAbbonamento();
            dto.Id = abbonamentoCorrente.Id;
            dto.Nome = abbonamentoCorrente.Nome;
            dto.Durata = abbonamentoCorrente.Durata;
            dto.Prezzo = abbonamentoCorrente.Prezzo;
            dto.Sconto = abbonamentoCorrente.Sconto;

            risultato.Add(dto);
        }

        return risultato;
    }

    // ------------------------------------------------------------
    // ✔ Restituisce un abbonamento SOLO se appartiene all’utente
    // ✔ Nessun LINQ, nessun Include() (richiede Lazy Loading o dati già caricati)
    // ------------------------------------------------------------
    public async Task<DtoAbbonamento?> OttieniTramiteIdAsync(string id, string utenteId)
    {
        // ✔ Recupera l’abbonamento tramite chiave primaria
        Abbonamento? abbonamento = await _contesto.Abbonamenti.FindAsync(id);

        if (abbonamento == null)
        {
            return null;
        }

        // ✔ Controllo manuale della lista utenti (senza LINQ)
        foreach (var utente in abbonamento.Utenti)
        {
            if (utente.Id == utenteId)
            {
                // ✔ Conversione in DTO solo se l’utente è autorizzato
                DtoAbbonamento risultato = new DtoAbbonamento();
                risultato.Id = abbonamento.Id;
                risultato.Nome = abbonamento.Nome;
                risultato.Durata = abbonamento.Durata;
                risultato.Prezzo = abbonamento.Prezzo;
                risultato.Sconto = abbonamento.Sconto;

                return risultato;
            }
        }

        // ❌ L’utente non è associato all’abbonamento
        return null;
    }

    // ------------------------------------------------------------
    // ✔ Crea un nuovo abbonamento
    // ✔ Restituisce il DTO dell’oggetto appena creato
    // ------------------------------------------------------------
    public async Task<DtoAbbonamento> CreazioneAsync(DtoCreazioneAbbonamento dto)
    {
        Abbonamento abbonamento = new Abbonamento();
        abbonamento.Nome = dto.Nome;
        abbonamento.Durata = dto.Durata;
        abbonamento.Prezzo = dto.Prezzo;
        abbonamento.Sconto = dto.Sconto;

        // ✔ Inserimento nel database
        _contesto.Abbonamenti.Add(abbonamento);
        await _contesto.SaveChangesAsync();

        // ✔ Conversione in DTO
        DtoAbbonamento risultato = new DtoAbbonamento();
        risultato.Id = abbonamento.Id;
        risultato.Nome = abbonamento.Nome;
        risultato.Durata = abbonamento.Durata;
        risultato.Prezzo = abbonamento.Prezzo;
        risultato.Sconto = abbonamento.Sconto;

        return risultato;
    }

    // ------------------------------------------------------------
    // ✔ Modifica un abbonamento esistente
    // ✔ Restituisce null se non trovato
    // ------------------------------------------------------------
    public async Task<DtoAbbonamento> ModificaAsync(string id, DtoCreazioneAbbonamento dto)
    {
        Abbonamento? abbonamento = await _contesto.Abbonamenti.FindAsync(id);

        if (abbonamento == null)
            return null;

        // ✔ Aggiornamento dei campi
        abbonamento.Nome = dto.Nome;
        abbonamento.Durata = dto.Durata;
        abbonamento.Prezzo = dto.Prezzo;
        abbonamento.Sconto = dto.Sconto;

        await _contesto.SaveChangesAsync();

        // ✔ Restituzione del DTO aggiornato
        return new DtoAbbonamento
        {
            Id = abbonamento.Id,
            Nome = abbonamento.Nome,
            Durata = abbonamento.Durata,
            Prezzo = abbonamento.Prezzo,
            Sconto = abbonamento.Sconto,
        };
    }

    // ------------------------------------------------------------
    // ✔ Elimina un abbonamento tramite ID
    // ✔ Restituisce true/false per indicare l’esito
    // ------------------------------------------------------------
    public async Task<bool> EliminazioneAsync(string id)
    {
        Abbonamento? abbonamento = await _contesto.Abbonamenti.FindAsync(id);

        if (abbonamento == null)
        {
            return false;
        }

        _contesto.Abbonamenti.Remove(abbonamento);
        await _contesto.SaveChangesAsync();

        return true;
    }
}
```

# Models

## Acquisto.cs

```c#
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace NuovoCinemaParadiso.Models;

[Table("Acquisti")] 
// ✔ Mappa la classe alla tabella "Acquisti" nel database
public class Acquisto
{
    [Key]
    // ✔ Chiave primaria dell’acquisto, generata automaticamente come GUID stringa
    public string Id { get; set; } = Guid.NewGuid().ToString();

    [Required]
    // ✔ FK verso la proiezione acquistata
    public string ProiezioneId { get; set; } = string.Empty;

    [ForeignKey("ProiezioneId")]
    // ✔ Navigazione verso la proiezione associata
    public Proiezione Proiezione { get; set; }

    [Required]
    // ✔ FK verso l’utente che ha effettuato l’acquisto
    public string UtenteId { get; set; } = string.Empty;

    [ForeignKey("UtenteId")]
    // ✔ Navigazione verso l’utente proprietario dell’acquisto
    public Utente Utente { get; set; }

    [Required]
    // ✔ Numero di biglietti acquistati per questa proiezione
    public int NumeroBiglietti { get; set; }

    // ✔ Timestamp di creazione dell’acquisto
    // ✔ Usa DateTimeOffset per mantenere il fuso orario
    public DateTimeOffset OrarioCreazione { get; set; } = DateTimeOffset.UtcNow;

    [Required]
    // ✔ Prezzo finale calcolato al momento dell’acquisto
    //   (include maggiorazioni sala, sconti, numero biglietti, ecc.)
    public decimal PrezzoFinale { get; set; }
}
```

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

    public DateTimeOffset DataInizio { get; set; } = DateTimeOffset.UtcNow;
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
    public DateTimeOffset DataInizio {get; set;}
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
    utenteTrovato.DataInizio = DateTimeOffset.UtcNow;

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
[Route("api/[controller]")]
[Authorize] // ✔ Tutti gli endpoint richiedono autenticazione
public class AdminController : ControllerBase
{
    private readonly AdminService _adminService;
    private readonly LogAzioniService _logAzioniService;

    // ✔ Iniezione dei servizi necessari al controller
    public AdminController(AdminService adminService, LogAzioniService logAzioniService)
    {
        _adminService = adminService;
        _logAzioniService = logAzioniService;
    }

    // ------------------------------------------------------------
    // GET api/admin/listaUtenti
    // ✔ Restituisce tutti i profili utente
    // ✔ Accessibile solo a Gestore/Operatore
    // ------------------------------------------------------------
    [HttpGet("listaUtenti")]
    [Authorize(Roles = Ruoli.GestoreOrOperatore)]
    public async Task<IActionResult> OttieniTuttiIProfili()
    {
        // ✔ Recupera tutti gli utenti dal service
        List<DtoUtente> utenti = await _adminService.OttieniUtentiAsync();

        // ✔ ID dell’utente che effettua l’operazione (per logging)
        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        // ✔ Log dell’azione (duplicato intenzionale secondo tua logica)
        await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
        {
            IdUtente = utenteId,
            NomeAzione = "Ricerca profili",
            Effettuato = true,
            Messaggio = "Ricerca avvenuta"
        });

        await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
        {
            IdUtente = utenteId,
            NomeAzione = "Ottieni tutti i profili",
            Effettuato = true,
            Messaggio = "Operazione eseguita"
        });

        return Ok(utenti);
    }

    // ------------------------------------------------------------
    // GET api/admin/ricercaProfilo/{id}
    // ✔ Ricerca un profilo tramite ID
    // ✔ Accessibile solo a Gestore/Operatore
    // ------------------------------------------------------------
    [HttpGet("ricercaProfilo/{id}")]
    [Authorize(Roles = Ruoli.GestoreOrOperatore)]
    public async Task<IActionResult> RicercaProfiloTramiteId(string id)
    {
        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        // ✔ Recupera il profilo tramite ID
        DtoUtente? utente = await _adminService.OttieniUtenteTramiteIdAsync(id);

        if (utente == null)
        {
            // ❌ Log fallimento
            await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
            {
                IdUtente = utenteId,
                NomeAzione = "Ricerca profilo",
                Effettuato = false,
                Messaggio = "Ricerca fallita"
            });

            return NotFound(new { messaggio = "Utente non trovato." });
        }

        // ✔ Log successo
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
    // DELETE api/admin/eliminaUtente/{id}
    // ✔ Elimina un utente tramite ID
    // ✔ Accessibile solo a Gestore/Operatore
    // ------------------------------------------------------------
    [HttpDelete("eliminaUtente/{id}")]
    [Authorize(Roles = Ruoli.GestoreOrOperatore)]
    public async Task<IActionResult> EliminaTramiteId(string Id)
    {
        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        // ✔ Tentativo di eliminazione
        var risultato = await _adminService.EliminaUtentePerIdAsync(Id);

        if (risultato == null)
        {
            // ❌ Log fallimento
            await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
            {
                IdUtente = utenteId,
                NomeAzione = "Eliminazione profilo",
                Effettuato = false,
                Messaggio = "Eliminazione profilo fallita"
            });

            return NotFound(new { messaggio = "Utente non trovato." });
        }

        // ✔ Log successo
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
    // GET api/admin/acquisti
    // ✔ Restituisce tutti gli acquisti registrati
    // ------------------------------------------------------------
    [HttpGet("acquisti")]
    [Authorize(Roles = Ruoli.GestoreOrOperatore)]
    public async Task<IActionResult> OttieniTuttiGliAcquisti()
    {
        List<DtoAcquisto> acquisti = await _adminService.OttieniAcquisti();
        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        // ✔ Log dell’azione
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
    // GET api/admin/acquisto/{id}
    // ✔ Restituisce un acquisto tramite ID
    // ------------------------------------------------------------
    [HttpGet("acquisto/{id}")]
    [Authorize(Roles = Ruoli.GestoreOrOperatore)]
    public async Task<IActionResult> OttieniAcquistoTramiteId(string id)
    {
        var risultato = await _adminService.OttieniAcquistoTramiteIdAsync(id);
        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (risultato == null)
        {
            // ❌ Log fallimento
            await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
            {
                IdUtente = utenteId,
                NomeAzione = "Ottieni acquisti tramite id admin",
                Effettuato = false,
                Messaggio = "Operazione fallita"
            });

            return NotFound($"Acquisto con id {id} non trovato");
        }

        // ✔ Log successo
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
    // GET api/admin/utenti/{id}
    // ✔ Restituisce tutti gli utenti associati a un abbonamento
    // ------------------------------------------------------------
    [HttpGet("utenti/{id}")]
    [Authorize(Roles = Ruoli.GestoreOrOperatore)]
    public async Task<ActionResult<List<DtoUtente>>> OttieniUtentiTramiteAbbonamentoAsync(string abbonamentoId)
    {
        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        // ✔ Validazione input
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

        // ✔ Recupera utenti associati all’abbonamento
        var risultato = await _adminService.OttieniUtentiTramiteAbbonamentoAsync(abbonamentoId);

        if (risultato == null || risultato.Count == 0)
        {
            // ❌ Log fallimento
            await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
            {
                IdUtente = utenteId,
                NomeAzione = "Ottieni gli utenti per abbonamento",
                Effettuato = false,
                Messaggio = "Operazione fallita"
            });

            return NotFound("Nessun utente trovato per questo abbonamento");
        }

        // ✔ Log successo
        await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
        {
            IdUtente = utenteId,
            NomeAzione = "Ottieni gli utenti per abbonamento",
            Effettuato = true,
            Messaggio = "Operazione eseguita"
        });

        return Ok(risultato);
    }

    // ------------------------------------------------------------
    // GET api/admin/abbonamento/{id}
    // ✔ Restituisce un abbonamento tramite ID (senza limiti utente)
    // ------------------------------------------------------------
    [HttpGet("abbonamento/{id}")]
    [Authorize(Roles = Ruoli.GestoreOrOperatore)]
    public async Task<IActionResult> OttieniAbbonamentoTramiteIdPerAdmin(string id)
    {
        var risultato = await _adminService.OttieniAbbonamentoTramiteIdPerAdminAsync(id);
        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (risultato == null)
        {
            // ❌ Log fallimento
            await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
            {
                IdUtente = utenteId,
                NomeAzione = "Ottieni abbonamenti tramite id admin",
                Effettuato = false,
                Messaggio = "Operazione fallita"
            });

            return NotFound($"Abbonamento con id {id} non trovato");
        }

        // ✔ Log successo
        await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
        {
            IdUtente = utenteId,
            NomeAzione = "Ottieni abbonamenti tramite id admin",
            Effettuato = true,
            Messaggio = "Operazione eseguita"
        });

        return Ok(risultato);
    }
}
```

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
    utenteTrovato.DataInizio = DateTimeOffset.UtcNow;

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

# Services

## AcquistoService.cs

```c#
using Microsoft.EntityFrameworkCore;
using NuovoCinemaParadiso.Data;
using NuovoCinemaParadiso.Dtos;
using NuovoCinemaParadiso.Models;
using NuovoCinemaParadiso.Helpers;

namespace NuovoCinemaParadiso.Services;

public class AcquistoService
{
    private readonly ContestoDb _contesto;

    public AcquistoService(ContestoDb contesto)
    {
        _contesto = contesto;
    }

    // Restituisce tutti gli acquisti dell'utente specificato
    public async Task<List<DtoAcquisto>> OttieniTutto(string utenteId)
    {
        // Recupera tutti gli acquisti dal DB
        // NOTA: qui si genera un potenziale N+1 perché poi carichi altre entità una per una
        List<Acquisto> acquisti = await _contesto.Acquisti.ToListAsync();

        List<DtoAcquisto> risultato = new List<DtoAcquisto>();

        for (int i = 0; i < acquisti.Count; i++)
        {
            Acquisto acquistoCorrente = acquisti[i];

            // Carica l'utente associato all'acquisto
            Utente? utente = await _contesto.Utenti.FindAsync(acquistoCorrente.UtenteId);

            // Carica la proiezione collegata
            Proiezione proiezione = await _contesto.Proiezioni.FindAsync(acquistoCorrente.ProiezioneId);

            // Carica il film della proiezione
            Movie movie = await _contesto.Movies.FindAsync(proiezione.MovieId);

            // Carica la sala della proiezione
            Sala sala = await _contesto.Sale.FindAsync(proiezione.SalaId);

            // Carica la tipologia della sala (per maggiorazioni)
            TipologiaSala tipologiaSala = await _contesto.TipologieSala.FindAsync(sala.TipologiaSalaId);

            // Filtra solo gli acquisti dell'utente richiesto
            if (acquistoCorrente.UtenteId == utenteId)
            {
                DtoAcquisto dto = new DtoAcquisto();
                dto.Id = acquistoCorrente.Id;
                dto.ProiezioneId = acquistoCorrente.ProiezioneId;

                // Calcolo del prezzo finale basato su film, sala e numero biglietti
                dto.PrezzoFinale = Calcoli.CalcolaPrezzoFinale(
                    movie.PrezzoMovie,
                    tipologiaSala.MaggiorazionePrezzo,
                    acquistoCorrente.NumeroBiglietti,
                    utente
                );

                dto.OrarioCreazione = acquistoCorrente.OrarioCreazione;
                dto.NumeroBiglietti = acquistoCorrente.NumeroBiglietti;

                risultato.Add(dto);
            }
        }

        return risultato;
    }

    // Restituisce un singolo acquisto tramite ID, solo se appartiene all'utente
    public async Task<DtoAcquisto> OttieniTramiteIdAsync(string id, string utenteId)
    {
        // Recupera l'acquisto
        Acquisto? acquisto = await _contesto.Acquisti.FindAsync(id);

        // Carica entità correlate
        Utente? utente = await _contesto.Utenti.FindAsync(acquisto.UtenteId);
        Proiezione proiezione = await _contesto.Proiezioni.FindAsync(acquisto.ProiezioneId);
        Movie movie = await _contesto.Movies.FindAsync(proiezione.MovieId);
        Sala sala = await _contesto.Sale.FindAsync(proiezione.SalaId);
        TipologiaSala tipologiaSala = await _contesto.TipologieSala.FindAsync(sala.TipologiaSalaId);

        // Se non esiste → null
        if (acquisto == null)
        {
            return null;
        }

        // Se l'acquisto non appartiene all'utente → null
        if (acquisto.UtenteId != utenteId)
        {
            return null;
        }

        // Mappa in DTO
        DtoAcquisto dto = new DtoAcquisto();
        dto.Id = acquisto.Id;
        dto.ProiezioneId = acquisto.ProiezioneId;
        dto.PrezzoFinale = Calcoli.CalcolaPrezzoFinale(
            movie.PrezzoMovie,
            tipologiaSala.MaggiorazionePrezzo,
            acquisto.NumeroBiglietti,
            utente
        );
        dto.OrarioCreazione = acquisto.OrarioCreazione;
        dto.NumeroBiglietti = acquisto.NumeroBiglietti;

        return dto;
    }

    // Crea un nuovo acquisto
    public async Task<DtoAcquisto> CreazioneAsync(DtoCreazioneAcquisto dto, string utenteId)
    {
        // Carica tutte le entità necessarie per il calcolo del prezzo
        Utente utente = await _contesto.Utenti.FindAsync(utenteId);
        Proiezione proiezione = await _contesto.Proiezioni.FindAsync(dto.ProiezioneId);
        Movie movie = await _contesto.Movies.FindAsync(proiezione.MovieId);
        Sala sala = await _contesto.Sale.FindAsync(proiezione.SalaId);
        TipologiaSala tipologiaSala = await _contesto.TipologieSala.FindAsync(sala.TipologiaSalaId);

        // Crea l'entità Acquisto
        Acquisto acquisto = new Acquisto();
        acquisto.UtenteId = utenteId;
        acquisto.ProiezioneId = proiezione.Id;
        acquisto.NumeroBiglietti = dto.NumeroBiglietti;

        // Calcolo del prezzo finale lato server (sicuro)
        acquisto.PrezzoFinale = Calcoli.CalcolaPrezzoFinale(
            movie.PrezzoMovie,
            tipologiaSala.MaggiorazionePrezzo,
            dto.NumeroBiglietti,
            utente
        );

        acquisto.OrarioCreazione = DateTimeOffset.UtcNow;

        // Salvataggio nel DB
        _contesto.Acquisti.Add(acquisto);
        await _contesto.SaveChangesAsync();

        // Mappa in DTO da restituire
        DtoAcquisto risultato = new DtoAcquisto();
        risultato.Id = acquisto.Id;
        risultato.ProiezioneId = acquisto.ProiezioneId;
        risultato.UtenteId = acquisto.UtenteId;
        risultato.NumeroBiglietti = acquisto.NumeroBiglietti;
        risultato.PrezzoFinale = acquisto.PrezzoFinale;
        risultato.OrarioCreazione = acquisto.OrarioCreazione;

        return risultato;
    }

    // Modifica un acquisto esistente
    public async Task<DtoAcquisto?> ModificaAsync(string id, DtoCreazioneAcquisto dto)
    {
        // Recupera l'acquisto da modificare
        Acquisto? acquistoEsistente = await _contesto.Acquisti.FindAsync(id);

        // Aggiorna numero biglietti
        acquistoEsistente.NumeroBiglietti = dto.NumeroBiglietti;

        // Carica entità correlate aggiornate
        Proiezione proiezione = await _contesto.Proiezioni.FindAsync(dto.ProiezioneId);
        Movie? movie = await _contesto.Movies.FindAsync(proiezione.MovieId);
        Sala? sala = await _contesto.Sale.FindAsync(proiezione.SalaId);
        Utente? utente = await _contesto.Utenti.FindAsync(acquistoEsistente.UtenteId);
        TipologiaSala? tipologiaSala = await _contesto.TipologieSala.FindAsync(sala.TipologiaSalaId);

        // Ricalcola il prezzo finale
        acquistoEsistente.PrezzoFinale = Calcoli.CalcolaPrezzoFinale(
            movie.PrezzoMovie,
            tipologiaSala.MaggiorazionePrezzo,
            acquistoEsistente.NumeroBiglietti,
            utente
        );

        await _contesto.SaveChangesAsync();

        // Mappa in DTO
        DtoAcquisto risultato = new DtoAcquisto
        {
            Id = acquistoEsistente.Id,
            UtenteId = acquistoEsistente.UtenteId,
            ProiezioneId = acquistoEsistente.ProiezioneId,
            NumeroBiglietti = acquistoEsistente.NumeroBiglietti,
            PrezzoFinale = acquistoEsistente.PrezzoFinale,
            OrarioCreazione = acquistoEsistente.OrarioCreazione
        };

        return risultato;
    }

    // Elimina un acquisto
    public async Task<bool> EliminazioneAsync(string id)
    {
        // Recupera l'acquisto
        Acquisto? acquisto = await _contesto.Acquisti.FindAsync(id);

        if (acquisto == null)
        {
            return false;
        }

        // Rimuove e salva
        _contesto.Acquisti.Remove(acquisto);
        await _contesto.SaveChangesAsync();

        return true;
    }
}
```

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

    // ✔ Iniezione del DbContext e del gestore utenti Identity
    public AdminService(ContestoDb contestoDb, UserManager<Utente> gestioneUtenti)
    {
        _contesto = contestoDb;
        _gestioneUtenti = gestioneUtenti;
    }

    // ------------------------------------------------------------
    // ✔ Restituisce tutti gli utenti registrati
    // ✔ Conversione manuale in DTO (senza LINQ)
    // ------------------------------------------------------------
    public async Task<List<DtoUtente>> OttieniUtentiAsync()
    {
        List<Utente> utenti = await _contesto.Utenti.ToListAsync();
        List<DtoUtente> risultato = new List<DtoUtente>();

        // ✔ Conversione iterativa in DTO
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
    // ✔ Restituisce un singolo utente tramite ID
    // ✔ Usa UserManager per compatibilità con Identity
    // ------------------------------------------------------------
    public async Task<DtoUtente?> OttieniUtenteTramiteIdAsync(string id)
    {
        Utente? utente = await _gestioneUtenti.FindByIdAsync(id);

        if (utente == null)
        {
            return null;
        }

        // ✔ Conversione in DTO
        DtoUtente dto = new DtoUtente();
        dto.Id = utente.Id;
        dto.Email = utente.Email ?? string.Empty;
        dto.NomeCompleto = utente.NomeCompleto ?? string.Empty;
        dto.Eta = utente.Eta;

        return dto;
    }

    // ------------------------------------------------------------
    // ✔ Elimina un utente tramite ID
    // ✔ Restituisce IdentityResult per gestire errori e successi
    // ------------------------------------------------------------
    public async Task<IdentityResult> EliminaUtentePerIdAsync(string id)
    {
        Utente? utente = await _gestioneUtenti.FindByIdAsync(id);

        if (utente == null)
        {
            // ✔ Identity richiede un errore formale per Failed()
            IdentityError errore = new IdentityError();
            return IdentityResult.Failed(errore);
        }

        // ✔ Eliminazione tramite Identity
        IdentityResult risultato = await _gestioneUtenti.DeleteAsync(utente);
        return risultato;
    }

    // ------------------------------------------------------------
    // ✔ Restituisce tutti gli acquisti
    // ✔ Caricamento manuale delle entità correlate (senza Include)
    // ------------------------------------------------------------
    public async Task<List<DtoAcquisto>> OttieniAcquisti()
    {
        List<Acquisto> acquisti = await _contesto.Acquisti.ToListAsync();
        List<DtoAcquisto> risultato = new List<DtoAcquisto>();

        for (int i = 0; i < acquisti.Count; i++)
        {
            Acquisto acquistoCorrente = acquisti[i];

            // ✔ Caricamento manuale delle entità correlate
            Proiezione proiezione = await _contesto.Proiezioni.FindAsync(acquistoCorrente.ProiezioneId);
            Movie? movie = await _contesto.Movies.FindAsync(proiezione.MovieId);
            Sala? sala = await _contesto.Sale.FindAsync(proiezione.SalaId);
            Utente? utente = await _contesto.Utenti.FindAsync(acquistoCorrente.UtenteId);
            TipologiaSala? tipologiaSala = await _contesto.TipologieSala.FindAsync(sala.TipologiaSalaId);

            // ✔ Conversione in DTO
            DtoAcquisto dto = new DtoAcquisto();
            dto.Id = acquistoCorrente.Id;
            dto.ProiezioneId = acquistoCorrente.ProiezioneId;
            dto.UtenteId = acquistoCorrente.UtenteId;
            dto.PrezzoFinale = Calcoli.CalcolaPrezzoFinale(
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
    // ✔ Restituisce un acquisto tramite ID
    // ✔ Caricamento manuale delle entità correlate
    // ------------------------------------------------------------
    public async Task<DtoAcquisto> OttieniAcquistoTramiteIdAsync(string id)
    {
        Acquisto? acquisto = await _contesto.Acquisti.FindAsync(id);

        if (acquisto == null)
        {
            return null;
        }

        // ✔ Caricamento entità correlate
        Proiezione proiezione = await _contesto.Proiezioni.FindAsync(acquisto.ProiezioneId);
        Movie? movie = await _contesto.Movies.FindAsync(proiezione.MovieId);
        Sala? sala = await _contesto.Sale.FindAsync(proiezione.SalaId);
        Utente? utente = await _contesto.Users.FindAsync(acquisto.UtenteId);
        TipologiaSala? tipologiaSala = await _contesto.TipologieSala.FindAsync(sala.TipologiaSalaId);

        // ✔ Conversione in DTO
        DtoAcquisto dto = new DtoAcquisto();
        dto.Id = acquisto.Id;
        dto.UtenteId = acquisto.UtenteId;
        dto.ProiezioneId = acquisto.ProiezioneId;
        dto.PrezzoFinale = Calcoli.CalcolaPrezzoFinale(
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
    // ✔ Restituisce tutti gli utenti associati a un abbonamento
    // ✔ Implementazione senza LINQ e senza Include()
    // ------------------------------------------------------------
    public async Task<List<DtoUtente>> OttieniUtentiTramiteAbbonamentoAsync(string abbonamentoId)
    {
        // ✔ Caricamento completo (senza Include)
        List<Utente> utenti = await _contesto.Utenti.ToListAsync();
        List<Abbonamento> abbonamenti = await _contesto.Abbonamenti.ToListAsync();

        // ✔ Ricerca manuale dell’abbonamento
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

        // ✔ Conversione utenti → DTO filtrando per abbonamento
        List<DtoUtente> risultato = new List<DtoUtente>();

        for (int i = 0; i < utenti.Count; i++)
        {
            Utente utenteCorrente = utenti[i];

            // ✔ Confronto diretto dell’oggetto (senza LINQ)
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

    // ------------------------------------------------------------
    // ✔ Restituisce un abbonamento tramite ID (per admin)
    // ------------------------------------------------------------
    public async Task<DtoAbbonamento> OttieniAbbonamentoTramiteIdPerAdminAsync(string id)
    {
        Abbonamento? abbonamento = await _contesto.Abbonamenti.FindAsync(id);

        if (abbonamento == null)
        {
            return null;
        }

        // ✔ Conversione in DTO
        DtoAbbonamento dto = new DtoAbbonamento();
        dto.Id = abbonamento.Id;
        dto.Nome = abbonamento.Nome;
        dto.Durata = abbonamento.Durata;
        dto.Prezzo = abbonamento.Prezzo;
        dto.Sconto = abbonamento.Sconto;

        return dto;
    }
}
```

## ProiezioneService.cs

```c#
public class ProiezioneService
{
    private readonly ContestoDb _contesto;
    public ProiezioneService(ContestoDb contesto)
    {
        _contesto = contesto;
    }

    // Restituisce tutte le proiezioni, mappandole manualmente in DTO.
    public async Task<List<DtoProiezione>> OttieniTuttoAsync()
    {
        List<Proiezione> proiezioni = await _contesto.Proiezioni.ToListAsync();
        List<DtoProiezione> risultato = new List<DtoProiezione>();

        for (int i = 0; i < proiezioni.Count; i++)
        {
            Proiezione proiezioneCorrente = proiezioni[i];

            // Recupero manuale delle entità collegate (non usate nel DTO).
            Movie? film = await _contesto.Movies.FindAsync(proiezioneCorrente.MovieId);
            Sala? sala = await _contesto.Sale.FindAsync(proiezioneCorrente.SalaId);

            // Mappatura esplicita.
            DtoProiezione dto = new DtoProiezione();
            dto.Id = proiezioneCorrente.Id;
            dto.DataProiezione = proiezioneCorrente.DataProiezione;
            dto.MovieId = proiezioneCorrente.MovieId;
            dto.SalaId = proiezioneCorrente.SalaId;
            dto.TurnoId = proiezioneCorrente.TurnoId;

            risultato.Add(dto);
        }
        return risultato;
    }

    // Restituisce una singola proiezione tramite ID.
    public async Task<DtoProiezione?> OttieniTramiteIdAsync(string id)
    {
        Proiezione? proiezione = await _contesto.Proiezioni.FindAsync(id);
        if (proiezione == null)
            return null;

        // Recupero manuale delle entità collegate.
        Movie? film = await _contesto.Movies.FindAsync(proiezione.MovieId);
        Sala? sala = await _contesto.Sale.FindAsync(proiezione.SalaId);
        Turno? turno = await _contesto.Turni.FindAsync(proiezione.TurnoId);

        // Mappatura esplicita.
        DtoProiezione risultato = new DtoProiezione();
        risultato.Id = proiezione.Id;
        risultato.DataProiezione = proiezione.DataProiezione;
        risultato.MovieId = proiezione.MovieId;
        risultato.SalaId = proiezione.SalaId;
        risultato.TurnoId = proiezione.TurnoId;

        return risultato;
    }

    // Filtra le proiezioni per MovieId (loop manuale).
    public async Task<List<DtoProiezione>> OttieniTramiteMovieAsync(string movieId)
    {
        List<DtoProiezione> risultato = new List<DtoProiezione>();
        List<Proiezione> proiezioni = await _contesto.Proiezioni.ToListAsync();

        for (int i = 0; i < proiezioni.Count; i++)
        {
            Proiezione proiezioneCorrente = proiezioni[i];
            if (proiezioneCorrente.MovieId == movieId)
            {
                DtoProiezione dto = new DtoProiezione();
                dto.Id = proiezioneCorrente.Id;
                dto.DataProiezione = proiezioneCorrente.DataProiezione;
                dto.MovieId = proiezioneCorrente.MovieId;
                dto.SalaId = proiezioneCorrente.SalaId;
                dto.TurnoId = proiezioneCorrente.TurnoId;

                risultato.Add(dto);
            }
        }
        return risultato;
    }

    // Filtra per SalaId.
    public async Task<List<DtoProiezione>> OttieniTramiteSalaAsync(string salaId)
    {
        List<DtoProiezione> risultato = new List<DtoProiezione>();
        List<Proiezione> proiezioni = await _contesto.Proiezioni.ToListAsync();

        for (int i = 0; i < proiezioni.Count; i++)
        {
            Proiezione proiezioneCorrente = proiezioni[i];
            if (proiezioneCorrente.SalaId == salaId)
            {
                DtoProiezione dto = new DtoProiezione();
                dto.Id = proiezioneCorrente.Id;
                dto.DataProiezione = proiezioneCorrente.DataProiezione;
                dto.MovieId = proiezioneCorrente.MovieId;
                dto.SalaId = proiezioneCorrente.SalaId;
                dto.TurnoId = proiezioneCorrente.TurnoId;

                risultato.Add(dto);
            }
        }
        return risultato;
    }

    // Filtra per TurnoId.
    public async Task<List<DtoProiezione>> OttieniTramiteTurnoAsync(string turnoId)
    {
        List<DtoProiezione> risultato = new List<DtoProiezione>();
        List<Proiezione> proiezioni = await _contesto.Proiezioni.ToListAsync();

        for (int i = 0; i < proiezioni.Count; i++)
        {
            Proiezione proiezioneCorrente = proiezioni[i];
            if (proiezioneCorrente.TurnoId == turnoId)
            {
                DtoProiezione dto = new DtoProiezione();
                dto.Id = proiezioneCorrente.Id;
                dto.DataProiezione = proiezioneCorrente.DataProiezione;
                dto.MovieId = proiezioneCorrente.MovieId;
                dto.SalaId = proiezioneCorrente.SalaId;
                dto.TurnoId = proiezioneCorrente.TurnoId;

                risultato.Add(dto);
            }
        }
        return risultato;
    }

    // Crea una nuova proiezione.
    public async Task<DtoProiezione?> CreazioneAsync(DtoCreazioneProiezione dto)
    {
        Proiezione proiezione = new Proiezione();
        proiezione.DataProiezione = dto.DataProiezione;
        proiezione.MovieId = dto.MovieId;
        proiezione.SalaId = dto.SalaId;
        proiezione.TurnoId = dto.TurnoId;

        _contesto.Proiezioni.Add(proiezione);
        await _contesto.SaveChangesAsync();

        // Recupero manuale delle entità collegate.
        Movie? film = await _contesto.Movies.FindAsync(proiezione.MovieId);
        Sala? sala = await _contesto.Sale.FindAsync(proiezione.SalaId);
        Turno? turno = await _contesto.Turni.FindAsync(proiezione.TurnoId);

        // Mappatura esplicita.
        DtoProiezione risultato = new DtoProiezione();
        risultato.Id = proiezione.Id;
        risultato.DataProiezione = proiezione.DataProiezione;
        risultato.MovieId = proiezione.MovieId;
        risultato.SalaId = proiezione.SalaId;
        risultato.TurnoId = proiezione.TurnoId;

        return risultato;
    }

    // Modifica una proiezione esistente.
    public async Task<DtoProiezione?> ModificaAsync(string id, DtoCreazioneProiezione dto)
    {
        Proiezione? proiezione = await _contesto.Proiezioni.FindAsync(id);
        if (proiezione == null)
            return null;

        proiezione.DataProiezione = dto.DataProiezione;
        proiezione.MovieId = dto.MovieId;
        proiezione.SalaId = dto.SalaId;
        proiezione.TurnoId = dto.TurnoId;

        await _contesto.SaveChangesAsync();

        // Recupero manuale delle entità collegate.
        Movie? film = await _contesto.Movies.FindAsync(proiezione.MovieId);
        Sala? sala = await _contesto.Sale.FindAsync(proiezione.SalaId);
        Turno? turno = await _contesto.Turni.FindAsync(proiezione.TurnoId);

        // Mappatura esplicita.
        DtoProiezione risultato = new DtoProiezione();
        risultato.Id = proiezione.Id;
        risultato.DataProiezione = proiezione.DataProiezione;
        risultato.MovieId = proiezione.MovieId;
        risultato.SalaId = proiezione.SalaId;
        risultato.TurnoId = proiezione.TurnoId;

        return risultato;
    }

    // Elimina una proiezione tramite ID.
    public async Task<bool> EliminaAsync(string id)
    {
        Proiezione? proiezione = await _contesto.Proiezioni.FindAsync(id);
        if (proiezione == null)
            return false;

        _contesto.Proiezioni.Remove(proiezione);
        await _contesto.SaveChangesAsync();

        return true;
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