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
    // lista degli acquisti relativi alla proiezione  (relazione 1-N)
    public List<Acquisto> Acquisti { get; set; } = new List<Acquisto>();
}
```

## Utente.cs

```c#

[Table("Utenti")]
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
    [Required]
    public bool PossiedeGiftCard { get; set; } = false;

    public DateTimeOffset DataInizioAbbonamento { get; set; }

    public DateTimeOffset DataInizioGiftCard { get; set; }

    public List<Acquisto> Acquisti { get; set; } = new List<Acquisto>();

    public string? AbbonamentoId { get; set; }

    [ForeignKey("AbbonamentoId")]
    public Abbonamento? Abbonamento { get; set; }

    public string? GiftCardId { get; set; }

    [ForeignKey("GiftCardId")]
    public GiftCard? GiftCard { get; set; }
}
```

## Turno.cs

```c#
// Mappa la classe alla tabella "Turni"
[Table("Turni")]
public class Turno
{
    // Chiave primaria generata come GUID stringa
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString();
    
    // Orario di inizio del turno (obbligatorio)
    [Required]
    public TimeOnly OraInizio { get; set; }

    // Orario di fine del turno (obbligatorio)
    [Required]
    public TimeOnly OraFine { get; set; }

    // Nome descrittivo del turno, max 50 caratteri
    [StringLength(50)]
    public string Nome { get; set; } = string.Empty;

    // Relazione 1‑a‑molti con le sale
    public List<Sala> Sale { get; set; } = new();
}
```

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
# Data 

## ContestoDb.cs
```c#
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NuovoCinemaParadiso.Models;

namespace NuovoCinemaParadiso.Data
{
    public class ContestoDb : IdentityDbContext<Utente, IdentityRole, string>
    {
        public ContestoDb(DbContextOptions<ContestoDb> opzioni)
            : base(opzioni)
        {
        }
        // tabella dei movies
        public DbSet<Movie> Movies { get; set; }
        // tabella dei generi dei movies
        public DbSet<GenereMovie> GeneriMovies { get; set; }
        // tabella delle sale del cinema
        public DbSet<Sala> Sale { get; set; }
        // tabella delle tipologie di sala
        public DbSet<TipologiaSala> TipologieSala { get; set; }
        // tabella dei vari turni dove possono essere programmate le proiezioni
        public DbSet<Turno> Turni { get; set; }
        // tabella degli acquisti (scontrini) relativi ad una proiezione di un film
        public DbSet<Acquisto> Acquisti { get; set; }
        // tabella defli utenti
        public DbSet<Utente> Utenti { get; set; }
        // tabella degli abbonamenti degli utenti
        public DbSet<Abbonamento> Abbonamenti {get;set;}
        // tabella del Log
        public DbSet<LogAzioni> LogAzioni {get;set;}
        // tabella delle proiezioni dei film 
        public DbSet<Proiezione> Proiezioni {get;set;}
        // tabella delle gift card
        public DbSet<GiftCard> GiftCards {get;set;}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            /*quando si vuole eliminare una proiezione, bisognerà prima rimborsare tutti 
            gli utenti che hanno gia comprato un biglietto per essa, di conseguenza bisogna inserire una restrizione*/
            modelBuilder.Entity<Acquisto>()
                .HasOne(a => a.Proiezione) // ogni acquisto appartiene ad una sola proiezione
                .WithMany(p => p.Acquisti) // ad ogni proiezione appartengono più acquisti
                .HasForeignKey(a => a.ProiezioneId) // la chiave esterna è ProiezioneId
                .OnDelete(DeleteBehavior.Restrict); 
                // 'Restrict' impedisce la cancellazione della proiezione se esistono degli acquisti relativi ad essa
        }
    }
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
    public string TipoGiftCard { get; set; } = string.Empty;
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
## DtoTurno.cs
```c#
// DTO usato per esporre i dati essenziali di un turno
public class DtoTurno
{
    // Identificativo del turno (stringa GUID)
    public string Id { get; set; }

    // Orario di inizio del turno (solo ora, senza data)
    public TimeOnly OraInizio { get; set; }

    // Orario di fine del turno (solo ora, senza data)
    public TimeOnly OraFine { get; set; }

    // Nome descrittivo del turno (es. "Sera", "Pomeriggio")
    public string Nome { get; set; } = string.Empty;
}
```
## DtoCreazioneTurno.cs
```c#
// DTO usato per creare un nuovo turno: contiene solo i campi richiesti in input
public class DtoCreazioneTurno
{    
    // Orario di inizio del turno (obbligatorio)
    [Required]
    public TimeOnly OraInizio { get; set; }

    // Orario di fine del turno (obbligatorio)
    [Required]
    public TimeOnly OraFine { get; set; }
    
    // Nome descrittivo del turno, max 50 caratteri (es. "Sera", "Pomeriggio")
    [StringLength(50)]
    public string Nome { get; set; } = string.Empty;
}
```
## DtoAcquisto.cs
```c#
// DTO restituito al client per rappresentare un acquisto già registrato
public class DtoAcquisto
{
    // Identificativo univoco dell’acquisto
    public string Id { get; set; }

    // Id della proiezione associata all’acquisto
    public string ProiezioneId { get; set; } = string.Empty;

    // Id dell’utente che ha effettuato l’acquisto
    public string UtenteId { get; set; } = string.Empty;

    // Prezzo finale calcolato lato server (film + tipologia sala + quantità)
    public decimal PrezzoFinale { get; set; }

    // Timestamp di creazione dell’acquisto (UTC)
    public DateTimeOffset OrarioCreazione { get; set; }

    // Numero di biglietti acquistati
    public int NumeroBiglietti { get; set; }
}
```
## DtoCreazioneAcquisto.cs
```c#
// DTO usato per creare un nuovo acquisto: contiene solo i dati forniti dal client
public class DtoCreazioneAcquisto
{
    // Id della proiezione scelta dall’utente (obbligatorio)
    [Required]
    public string ProiezioneId { get; set; } = string.Empty;

    // Numero di biglietti richiesti (obbligatorio)
    [Required]
    public int NumeroBiglietti { get; set; }
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

    // Endpoint: POST api/utente/abbonati
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

    [HttpPost("giftCard")] // Endpoint: POST api/utente/giftCard
    public async Task<IActionResult> GiftCard([FromBody] DtoUtente dto)
    {
        // Recupera l'ID dell'utente autenticato dal token (claims)
        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        // Validazione input: controlla che il DTO esista e che GiftCardId non sia nullo o vuoto
        if (dto == null || string.IsNullOrEmpty(dto.GiftCardId))
        {
            // Logga l'azione come fallita
            await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
            {
                IdUtente = utenteId,
                NomeAzione = "GiftCard",
                Effettuato = false,
                Messaggio = "Operazione fallita"
            });

            // Restituisce errore 400 Bad Request
            return BadRequest("Dati non validi");
        }

        // Chiama il servizio per eseguire la logica della gift card
        var risultato = await _utenteService.GiftCardAsync(dto.GiftCardId, utenteId);

        // Se il risultato è nullo, significa che qualcosa non è stato trovato (utente o gift card)
        if (risultato == null)
        {
            // Logga l'azione come fallita
            await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
            {
                IdUtente = utenteId,
                NomeAzione = "GiftCard",
                Effettuato = false,
                Messaggio = "Operazione fallita"
            });

            // Restituisce errore 404 Not Found
            return NotFound("Utente o GiftCard non trovata");
        }

        // Logga l'azione come riuscita
        await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
        {
            IdUtente = utenteId,
            NomeAzione = "GiftCard",
            Effettuato = true,
            Messaggio = "Operazione eseguita"
        });

        // Restituisce 200 OK con il risultato dell'operazione
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
    // 3) OTTIENI TUTTI GLI ACQUISTI
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
    // 4) OTTIENI ACQUISTO TRAMITE ID
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
    // 5) OTTIENI UTENTI TRAMITE ABBONAMENTO
    // ------------------------------------------------------------
    [HttpGet("utenti/{abbonamentoid}")]
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

    [HttpGet("giftcard/{id}")]
    [Authorize(Roles = Ruoli.GestoreOrOperatore)]
    public async Task<IActionResult> OttieniGiftCardTramiteIdPerAdmin(string id)
    {
        // Recupera la gift card tramite ID usando il service dedicato agli admin
        var risultato = await _adminService.OttieniGiftCardTramiteIdPerAdminAsync(id);

        // Recupera l'id dell'utente autenticato dal token JWT
        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        // Se la gift card non esiste, logga l’operazione come fallita e ritorna 404
        if (risultato == null)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
            {
                IdUtente = utenteId,
                NomeAzione = "Ottieni giftcard tramite id admin",
                Effettuato = false,
                Messaggio = "Operazione fallita"
            });

            return NotFound($"Giftcard con id {id} non trovato");
        }

        // Se la gift card esiste, logga l’operazione come riuscita
        await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
        {
            IdUtente = utenteId,
            NomeAzione = "Ottieni giftcard tramite id admin",
            Effettuato = true,
            Messaggio = "Operazione eseguita"
        });

        // Restituisce la gift card trovata
        return Ok(risultato);
}

    [HttpGet("utenti/{giftcardId}")]
    [Authorize(Roles = Ruoli.GestoreOrOperatore)]
    public async Task<ActionResult<List<DtoUtente>>> OttieniUtentiTramiteGiftCardPerAdmin(string giftcardId)
    {
        // Recupera l'id dell’utente autenticato
        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        // Controllo base sul parametro ricevuto
        if (string.IsNullOrWhiteSpace(giftcardId))
        {
            // Log operazione fallita per parametro non valido
            await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
            {
                IdUtente = utenteId,
                NomeAzione = "Ottieni gli utenti per giftcard",
                Effettuato = false,
                Messaggio = "Operazione fallita"
            });

            return BadRequest("giftcardId non valido");
        }

        // Recupera gli utenti associati alla gift card
        var risultato = await _adminService.OttieniUtentiTramiteGiftCardAsync(giftcardId);

        // Se non ci sono utenti associati, logga fallimento e ritorna 404
        if (risultato == null)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
            {
                IdUtente = utenteId,
                NomeAzione = "Ottieni gli utenti per giftcard",
                Effettuato = false,
                Messaggio = "Operazione fallita"
            });

            return NotFound("Nessun utente trovato per questa giftcard");
        }

        // Log operazione riuscita
        await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
        {
            IdUtente = utenteId,
            NomeAzione = "Ottieni gli utenti per giftcard",
            Effettuato = true,
            Messaggio = "Operazione eseguita"
        });

        // Restituisce la lista degli utenti trovati
        return Ok(risultato);
}

    [HttpGet("log")]
    [Authorize(Roles = Ruoli.Gestore)]
    public async Task<IActionResult> OttieniLogAzioni()
    {
        // Recupera tutti i log delle azioni dal sistema
        List<DtoLogAzioni> risultatiLog = await _logAzioniService.LetturaLogAzioneAsync();

        // Restituisce la lista dei log
        return Ok(risultatiLog);
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
## TurnoController.cs
```c#
// Controller API per la gestione dei turni: richiede autenticazione
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TurnoController : ControllerBase
{
    // Servizi applicativi usati dal controller
    private readonly TurnoService _turnoService;
    private readonly LogAzioniService _logAzioniService;

    // Iniezione dei servizi necessari
    public TurnoController(TurnoService turnoService, LogAzioniService logAzioniService)
    {
        _turnoService = turnoService;
        _logAzioniService = logAzioniService;
    }

    // Restituisce tutti i turni presenti nel sistema
    [HttpGet]
    public async Task<IActionResult> OttieniTutti()
    {
        List<DtoTurno> turni = await _turnoService.OttieniTuttoAsync();
        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
        {
            IdUtente = utenteId,
            NomeAzione = "Ottieni tutte i turni",
            Effettuato = true,
            Messaggio = "Operazione eseguita"
        });

        return Ok(turni);
    }

    // Restituisce un turno tramite il suo id
    [HttpGet("{id}")]
    public async Task<IActionResult> OttieniTramiteId(string id)
    {
        var risultato = await _turnoService.OttieniTramiteIdAsync(id);
        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (risultato == null)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
            {
                IdUtente = utenteId,
                NomeAzione = "Ottieni turno tramite id",
                Effettuato = false,
                Messaggio = "Operazione fallita"
            });

            return NotFound($"Turno con id {id} non trovato");
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
        {
            IdUtente = utenteId,
            NomeAzione = "Ottieni turno tramite id",
            Effettuato = true,
            Messaggio = "Operazione eseguita"
        });

        return Ok(risultato);
    }

    // Crea un nuovo turno (solo Gestore o Operatore)
    [HttpPost]
    [Authorize(Roles = Ruoli.GestoreOrOperatore)]
    public async Task<IActionResult> Creazione([FromBody] DtoCreazioneTurno dto)
    {
        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        List<DtoTurno> turni = await _turnoService.OttieniTuttoAsync(); // lettura turni esistenti

        // Verifica unicità del nome turno
        foreach (var turno in turni)
        {
            bool stringheUguali = string.Equals(turno.Nome, dto.Nome, StringComparison.OrdinalIgnoreCase);
            if (stringheUguali)
            {
                await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
                {
                    IdUtente = utenteId,
                    NomeAzione = "Creazione turno",
                    Effettuato = false,
                    Messaggio = "Operazione fallita"
                });

                return BadRequest(new { messaggio = "Turno già presente." });
            }
        }

        // Creazione del turno tramite servizio applicativo
        DtoTurno? risultato = await _turnoService.CreazioneAsync(dto);

        if (risultato == null)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
            {
                IdUtente = utenteId,
                NomeAzione = "Creazione turno",
                Effettuato = false,
                Messaggio = "Operazione fallita"
            });

            return BadRequest(new { messaggio = "Turno non valido." });
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
        {
            IdUtente = utenteId,
            NomeAzione = "Creazione turno",
            Effettuato = true,
            Messaggio = "Operazione eseguita"
        });

        return Ok(risultato);
    }

    // Modifica un turno esistente
    [HttpPut("{id}")]
    [Authorize(Roles = Ruoli.GestoreOrOperatore)]
    public async Task<IActionResult> Modifica(string id, [FromBody] DtoCreazioneTurno dto)
    {
        DtoTurno? risultato = await _turnoService.ModificaAsync(id, dto);
        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (risultato == null)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
            {
                IdUtente = utenteId,
                NomeAzione = "Modifica turno",
                Effettuato = false,
                Messaggio = "Operazione fallita"
            });

            return NotFound(new { messaggio = "Turno non trovato." });
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
        {
            IdUtente = utenteId,
            NomeAzione = "Modifica turno",
            Effettuato = true,
            Messaggio = "Operazione eseguita"
        });

        return Ok(risultato);
    }

    // Elimina un turno tramite id
    [HttpDelete("{id}")]
    [Authorize(Roles = Ruoli.GestoreOrOperatore)]
    public async Task<IActionResult> Elimina(string id)
    {
        bool eliminato = await _turnoService.EliminaAsync(id);
        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!eliminato)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
            {
                IdUtente = utenteId,
                NomeAzione = "Elimina turno",
                Effettuato = false,
                Messaggio = "Operazione fallita"
            });

            return NotFound(new { messaggio = "Turno non trovato." });
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
        {
            IdUtente = utenteId,
            NomeAzione = "Elimina turno",
            Effettuato = true,
            Messaggio = "Operazione eseguita"
        });

        return NoContent();
    }
}
```
## AcquistoController.cs
```c#
// Controller API per la gestione degli acquisti: richiede autenticazione
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AcquistoController : ControllerBase
{
    // Servizi applicativi utilizzati dal controller
    private readonly AcquistoService _acquistoService;
    private readonly LogAzioniService _logAzioniService;

    // Iniezione dei servizi tramite costruttore
    public AcquistoController(AcquistoService acquistoService, LogAzioniService logAzioniService)
    {
        _acquistoService = acquistoService;
        _logAzioniService = logAzioniService;
    }

    // Restituisce tutti gli acquisti dell’utente autenticato
    [HttpGet]
    public async Task<IActionResult> OttieniTutti()
    {
        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        List<DtoAcquisto> acquisti = await _acquistoService.OttieniTutto(utenteId);

        // Log dell’operazione
        await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
        {
            IdUtente = utenteId,
            NomeAzione = "Ottieni tutti gli acquisti utente",
            Effettuato = true,
            Messaggio = "Operazione eseguita"
        });

        return Ok(acquisti);
    }

    // Restituisce un acquisto tramite id, solo se appartiene all’utente
    [HttpGet("{id}")]
    public async Task<IActionResult> OttieniTramiteId(string id)
    {
        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var risultato = await _acquistoService.OttieniTramiteIdAsync(id, utenteId);

        // Se non trovato o non appartenente all’utente
        if (risultato == null)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
            {
                IdUtente = utenteId,
                NomeAzione = "Ottieni acquisti tramite id utente",
                Effettuato = false,
                Messaggio = "Operazione fallita"
            });

            return NotFound($"Acquisto con id {id} non trovato");
        }

        // Log operazione riuscita
        await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
        {
            IdUtente = utenteId,
            NomeAzione = "Ottieni acquisti tramite id utente",
            Effettuato = true,
            Messaggio = "Operazione eseguita"
        });

        return Ok(risultato);
    }

    // Crea un nuovo acquisto per l’utente autenticato
    [HttpPost]
    public async Task<IActionResult> Creazione([FromBody] DtoCreazioneAcquisto dto)
    {
        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        DtoAcquisto? risultato = await _acquistoService.CreazioneAsync(dto, utenteId);

        // Se la creazione fallisce
        if (risultato == null)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
            {
                IdUtente = utenteId,
                NomeAzione = "Creazione acquisto",
                Effettuato = false,
                Messaggio = "Operazione fallita"
            });

            return BadRequest(new { messaggio = "Acquisto già presente oppure non valido." });
        }

        // Log operazione riuscita
        await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
        {
            IdUtente = utenteId,
            NomeAzione = "Creazione acquisto",
            Effettuato = true,
            Messaggio = "Operazione eseguita"
        });

        return Ok(risultato);
    }

    // Modifica un acquisto esistente (solo Gestore o Operatore)
    [HttpPut("{id}")]
    [Authorize(Roles = Ruoli.GestoreOrOperatore)]
    public async Task<IActionResult> Modifica(string id, [FromBody] DtoCreazioneAcquisto dto)
    {
        DtoAcquisto? risultato = await _acquistoService.ModificaAsync(id, dto);
        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        // Se l’acquisto non esiste
        if (risultato == null)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
            {
                IdUtente = utenteId,
                NomeAzione = "Modifica acquisto",
                Effettuato = false,
                Messaggio = "Operazione fallita"
            });

            return NotFound(new { messaggio = "Acquisto non trovato." });
        }

        // Log operazione riuscita
        await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
        {
            IdUtente = utenteId,
            NomeAzione = "Modifica acquisto",
            Effettuato = true,
            Messaggio = "Operazione eseguita"
        });

        return Ok(risultato);
    }

    // Elimina un acquisto tramite id (solo Gestore o Operatore)
    [HttpDelete("{id}")]
    [Authorize(Roles = Ruoli.GestoreOrOperatore)]
    public async Task<IActionResult> Elimina(string id)
    {
        bool eliminato = await _acquistoService.EliminazioneAsync(id);
        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        // Se non trovato
        if (!eliminato)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
            {
                IdUtente = utenteId,
                NomeAzione = "Elimina acquisto",
                Effettuato = false,
                Messaggio = "Operazione fallita"
            });

            return NotFound(new { messaggio = "Acquisto non trovato." });
        }

        // Log operazione riuscita
        await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
        {
            IdUtente = utenteId,
            NomeAzione = "Elimina acquisto",
            Effettuato = true,
            Messaggio = "Operazione eseguita"
        });

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

## TurnoService.cs

```c#
// Servizio applicativo per la gestione dei turni: incapsula la logica di accesso al DB
public class TurnoService
{
    // Riferimento al DbContext per operazioni CRUD
    private readonly ContestoDb _contesto;

    // Iniezione del contesto tramite costruttore
    public TurnoService(ContestoDb contesto)
    {
        _contesto = contesto;
    }

    // Restituisce tutti i turni mappandoli manualmente in DTO
    public async Task<List<DtoTurno>> OttieniTuttoAsync()
    {
        List<Turno> turni = await _contesto.Turni.ToListAsync();
        List<DtoTurno> risultato = new List<DtoTurno>();

        // Mappatura manuale per ogni turno
        for (int i = 0; i < turni.Count; i++)
        {
            Turno turnoCorrente = turni[i];

            DtoTurno dto = new DtoTurno();
            dto.Id = turnoCorrente.Id;
            dto.OraInizio = turnoCorrente.OraInizio;
            dto.OraFine = turnoCorrente.OraFine;
            dto.Nome = turnoCorrente.Nome;

            risultato.Add(dto);
        }

        return risultato;
    }

    // Restituisce un turno tramite id, oppure null se non trovato
    public async Task<DtoTurno> OttieniTramiteIdAsync(string id) 
    {
        Turno? turno = await _contesto.Turni.FindAsync(id);
        if (turno == null)
        {
            return null;
        }

        // Mappatura del singolo turno in DTO
        DtoTurno dto = new DtoTurno();
        dto.Id = turno.Id;
        dto.Nome = turno.Nome;
        dto.OraInizio = turno.OraInizio;
        dto.OraFine = turno.OraFine;

        return dto;
    }

    // Crea un nuovo turno a partire dal DTO di creazione
    public async Task<DtoTurno> CreazioneAsync(DtoCreazioneTurno dto)
    {
        // Creazione dell'entità da salvare
        Turno turno = new Turno();
        turno.Nome = dto.Nome;
        turno.OraInizio = dto.OraInizio;
        turno.OraFine = dto.OraFine;

        _contesto.Turni.Add(turno);
        await _contesto.SaveChangesAsync();

        // Restituzione del DTO risultante
        DtoTurno risultato = new DtoTurno();
        risultato.Id = turno.Id;
        risultato.Nome = turno.Nome;
        risultato.OraInizio = turno.OraInizio;
        risultato.OraFine = turno.OraFine;

        return risultato;
    }

    // Modifica un turno esistente tramite id e DTO di creazione
    public async Task<DtoTurno?> ModificaAsync(string id, DtoCreazioneTurno dto)
    {
        Turno? turnoEsistente = await _contesto.Turni.FindAsync(id);

        // Se non trovato, restituisce null
        if (turnoEsistente == null)
        {
            return null;
        }

        // Aggiornamento dei campi modificabili
        turnoEsistente.OraInizio = dto.OraInizio;
        turnoEsistente.OraFine = dto.OraFine;
        turnoEsistente.Nome = dto.Nome;

        await _contesto.SaveChangesAsync();

        // Mappatura del risultato aggiornato
        DtoTurno risultato = new DtoTurno();
        risultato.Id = turnoEsistente.Id;
        risultato.OraInizio = turnoEsistente.OraInizio;
        risultato.OraFine = turnoEsistente.OraFine;
        risultato.Nome = turnoEsistente.Nome;

        return risultato;
    }

    // Elimina un turno tramite id, restituisce true se eliminato
    public async Task<bool> EliminaAsync(string id) 
    {
        Turno? turno = await _contesto.Turni.FindAsync(id);

        // Se non esiste, operazione fallita
        if (turno == null)
        {
            return false;
        }

        _contesto.Turni.Remove(turno);
        await _contesto.SaveChangesAsync();

        return true;
    }
}
```

## AcquistoService.cs

```c#
// Servizio applicativo per la gestione degli acquisti: contiene la logica di business
public class AcquistoService
{
    // Riferimento al DbContext per operazioni sul database
    private readonly ContestoDb _contesto;

    // Iniezione del contesto tramite costruttore
    public AcquistoService(ContestoDb contesto)
    {
        _contesto = contesto;
    }

    // Restituisce tutti gli acquisti dell'utente specificato
    public async Task<List<DtoAcquisto>> OttieniTutto(string utenteId)
    {
        // Recupera tutti gli acquisti dal DB
        List<Acquisto> acquisti = await _contesto.Acquisti.ToListAsync();
        List<DtoAcquisto> risultato = new List<DtoAcquisto>();

        // Mappatura manuale con filtraggio per utente
        for (int i = 0; i < acquisti.Count; i++)
        {
            Acquisto acquistoCorrente = acquisti[i];

            // Recupero entità correlate necessarie al calcolo del prezzo
            Utente? utente = await _contesto.Utenti.FindAsync(acquistoCorrente.UtenteId);
            Proiezione proiezione = await _contesto.Proiezioni.FindAsync(acquistoCorrente.ProiezioneId);
            Movie movie = await _contesto.Movies.FindAsync(proiezione.MovieId);
            Sala sala = await _contesto.Sale.FindAsync(proiezione.SalaId);
            TipologiaSala tipologiaSala = await _contesto.TipologieSala.FindAsync(sala.TipologiaSalaId);

            // Considera solo gli acquisti dell'utente richiesto
            if (acquistoCorrente.UtenteId == utenteId)
            {
                // Costruzione DTO risultato
                DtoAcquisto dto = new DtoAcquisto();
                dto.Id = acquistoCorrente.Id;
                dto.ProiezioneId = acquistoCorrente.ProiezioneId;
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

    // Restituisce un acquisto tramite id, solo se appartiene all'utente
    public async Task<DtoAcquisto> OttieniTramiteIdAsync(string id, string utenteId)
    {
        // Recupero dell'acquisto
        Acquisto? acquisto = await _contesto.Acquisti.FindAsync(id);

        if (acquisto == null)
        {
            return null;
        }

        // Controllo che l'acquisto appartenga all'utente
        if (acquisto.UtenteId != utenteId)
        {
            return null;
        }

        // Recupero entità correlate
        Utente? utente = await _contesto.Utenti.FindAsync(acquisto.UtenteId);
        Proiezione proiezione = await _contesto.Proiezioni.FindAsync(acquisto.ProiezioneId);
        Movie movie = await _contesto.Movies.FindAsync(proiezione.MovieId);
        Sala sala = await _contesto.Sale.FindAsync(proiezione.SalaId);
        TipologiaSala tipologiaSala = await _contesto.TipologieSala.FindAsync(sala.TipologiaSalaId);

        // Costruzione DTO
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

    // Crea un nuovo acquisto per l'utente
    public async Task<DtoAcquisto> CreazioneAsync(DtoCreazioneAcquisto dto, string utenteId)
    {
        // Recupero entità necessarie al calcolo del prezzo
        Utente utente = await _contesto.Utenti.FindAsync(utenteId);
        Proiezione proiezione = await _contesto.Proiezioni.FindAsync(dto.ProiezioneId);
        Movie movie = await _contesto.Movies.FindAsync(proiezione.MovieId);
        Sala sala = await _contesto.Sale.FindAsync(proiezione.SalaId);
        TipologiaSala tipologiaSala = await _contesto.TipologieSala.FindAsync(sala.TipologiaSalaId);

        // Creazione dell'entità Acquisto
        Acquisto acquisto = new Acquisto();
        acquisto.UtenteId = utenteId;
        acquisto.ProiezioneId = proiezione.Id;
        acquisto.NumeroBiglietti = dto.NumeroBiglietti;
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

        // Costruzione DTO risultato
        DtoAcquisto risultato = new DtoAcquisto();
        risultato.Id = acquisto.Id;
        risultato.ProiezioneId = acquisto.ProiezioneId;
        risultato.UtenteId = acquisto.UtenteId;
        risultato.NumeroBiglietti = acquisto.NumeroBiglietti;
        risultato.PrezzoFinale = acquisto.PrezzoFinale;
        risultato.OrarioCreazione = acquisto.OrarioCreazione;

        return risultato;
    }

    // Modifica un acquisto esistente (solo numero biglietti e prezzo)
    public async Task<DtoAcquisto?> ModificaAsync(string id, DtoCreazioneAcquisto dto)
    {
        // Recupero dell'acquisto
        Acquisto? acquistoEsistente = await _contesto.Acquisti.FindAsync(id);

        if (acquistoEsistente == null)
        {
            return null;
        }

        // Aggiornamento numero biglietti
        acquistoEsistente.NumeroBiglietti = dto.NumeroBiglietti;

        // Recupero entità correlate aggiornate
        Proiezione proiezione = await _contesto.Proiezioni.FindAsync(dto.ProiezioneId);
        Movie? movie = await _contesto.Movies.FindAsync(proiezione.MovieId);
        Sala? sala = await _contesto.Sale.FindAsync(proiezione.SalaId);
        Utente? utente = await _contesto.Utenti.FindAsync(acquistoEsistente.UtenteId);
        TipologiaSala? tipologiaSala = await _contesto.TipologieSala.FindAsync(sala.TipologiaSalaId);

        // Ricalcolo del prezzo finale
        acquistoEsistente.PrezzoFinale = Calcoli.CalcolaPrezzoFinale(
            movie.PrezzoMovie,
            tipologiaSala.MaggiorazionePrezzo,
            acquistoEsistente.NumeroBiglietti,
            utente
        );

        await _contesto.SaveChangesAsync();

        // Costruzione DTO risultato
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

    // Elimina un acquisto tramite id
    public async Task<bool> EliminazioneAsync(string id)
    {
        Acquisto? acquisto = await _contesto.Acquisti.FindAsync(id);

        if (acquisto == null)
        {
            return false;
        }

        _contesto.Acquisti.Remove(acquisto);
        await _contesto.SaveChangesAsync();

        return true;
    }
}
```

## GiftCardService.cs

```c#
// Servizio applicativo per la gestione delle GiftCard
public class GiftCardService
{
    // Riferimento al DbContext per operazioni sul database
    private readonly ContestoDb _contesto;

    // Iniezione del contesto tramite costruttore
    public GiftCardService(ContestoDb contesto)
    {
        _contesto = contesto;
    }

    // Restituisce tutte le GiftCard presenti nel sistema
    public async Task<List<DtoGiftCard>> OttieniTutto()
    {
        // Lettura completa della tabella GiftCards
        List<GiftCard> giftCards = await _contesto.GiftCards.ToListAsync();

        List<DtoGiftCard> risultato = new List<DtoGiftCard>();

        // Mappatura manuale GiftCard → DTO
        for (int i = 0; i < giftCards.Count; i++)
        {
            GiftCard giftCardCorrente = giftCards[i];

            DtoGiftCard dto = new DtoGiftCard();
            dto.Id = giftCardCorrente.Id;
            dto.Nome = giftCardCorrente.Nome;
            dto.Durata = giftCardCorrente.Durata;
            dto.Prezzo = giftCardCorrente.Prezzo;
            dto.NumeroMovie = giftCardCorrente.NumeroMovie;

            risultato.Add(dto);
        }

        return risultato;
    }

    // Restituisce una GiftCard solo se appartiene all’utente richiesto
    public async Task<DtoGiftCard?> OttieniTramiteIdAsync(string id, string utenteId)
    {
        // Recupero GiftCard tramite chiave primaria
        GiftCard? giftCard = await _contesto.GiftCards.FindAsync(id);

        if (giftCard == null)
        {
            return null;
        }

        // Controllo che l’utente sia associato alla GiftCard
        foreach (var utente in giftCard.Utenti)
        {
            if (utente.Id == utenteId)
            {
                DtoGiftCard risultato = new DtoGiftCard();
                risultato.Id = giftCard.Id;
                risultato.Nome = giftCard.Nome;
                risultato.Durata = giftCard.Durata;
                risultato.Prezzo = giftCard.Prezzo;
                risultato.NumeroMovie = giftCard.NumeroMovie;

                return risultato;
            }
        }

        return null;
    }

    // Crea una nuova GiftCard
    public async Task<DtoGiftCard> CreazioneAsync(DtoCreazioneGiftCard dto)
    {
        // Costruzione dell’entità da salvare
        GiftCard giftCard = new GiftCard();
        giftCard.Nome = dto.Nome;
        giftCard.Durata = dto.Durata;
        giftCard.Prezzo = dto.Prezzo;
        giftCard.NumeroMovie = dto.NumeroMovie;

        // Salvataggio nel database
        _contesto.GiftCards.Add(giftCard);
        await _contesto.SaveChangesAsync();

        // Mappatura dell’entità salvata in DTO
        DtoGiftCard risultato = new DtoGiftCard();
        risultato.Id = giftCard.Id;
        risultato.Nome = giftCard.Nome;
        risultato.Durata = giftCard.Durata;
        risultato.Prezzo = giftCard.Prezzo;
        risultato.NumeroMovie = giftCard.NumeroMovie;

        return risultato;
    }

    // Modifica una GiftCard esistente
    public async Task<DtoGiftCard?> ModificaAsync(string id, DtoCreazioneGiftCard dto)
    {
        // Recupero GiftCard da modificare
        GiftCard? giftCard = await _contesto.GiftCards.FindAsync(id);

        if (giftCard == null)
            return null;

        // Aggiornamento dei campi modificabili
        giftCard.Nome = dto.Nome;
        giftCard.Durata = dto.Durata;
        giftCard.Prezzo = dto.Prezzo;
        giftCard.NumeroMovie = dto.NumeroMovie;

        await _contesto.SaveChangesAsync();

        // Restituzione DTO aggiornato
        return new DtoGiftCard
        {
            Id = giftCard.Id,
            Nome = giftCard.Nome,
            Durata = giftCard.Durata,
            Prezzo = giftCard.Prezzo,
            NumeroMovie = giftCard.NumeroMovie,
        };
    }

    // Elimina una GiftCard tramite id
    public async Task<bool> EliminazioneAsync(string id)
    {
        // Recupero GiftCard
        GiftCard? giftCard = await _contesto.GiftCards.FindAsync(id);

        if (giftCard == null)
        {
            return false;
        }

        // Eliminazione dal database
        _contesto.GiftCards.Remove(giftCard);
        await _contesto.SaveChangesAsync();

        return true;
    }
}

```c#
// Servizio applicativo per la gestione dei log: incapsula la logica di accesso al DB
public class LogAzioniService
{
    // Riferimento al DbContext per operazioni CRUD
    private readonly ContestoDb _contesto;

    // Iniezione del contesto tramite costruttore
    public LogAzioniService(ContestoDb contesto) 
    {
      _contesto = contesto; 
    }

    // Salva i log passati come dto all'interno del database
    public async Task SalvataggioLogAzioneAsync(DtoCreazioneLogAzioni dto)
    {
        // Trasforma il log passato com dto nel modello LogAzioni
        LogAzioni log = new LogAzioni();
        log.IdUtente = dto.IdUtente;
        log.NomeAzione = dto.NomeAzione;
        log.Effettuato = dto.Effettuato;
        log.Messaggio = dto.Messaggio;
        log.TimeStamp = DateTimeOffset.UtcNow;

        // Salvataggio all'interno del database
        _contesto.LogAzioni.Add(log);
        await _contesto.SaveChangesAsync();
    }
    // Prende tutti i log dal database e li passa passa come una lista di dto log
    public async Task<List<DtoLogAzioni>> LetturaLogAzioneAsync()
    {   // Prende tutti i log dal database e li racchiude in una lista
        List<LogAzioni> logs= await _contesto.LogAzioni.ToListAsync();
        // Prepara una lista di dto per contenere tutti i log
        List<DtoLogAzioni> risultati = new List<DtoLogAzioni>();
        // Per ogni log presente nel DB
        foreach (LogAzioni log in logs)
        {
          // Inserimento di ogni dato del log all'interno del dto 
          DtoLogAzioni risultato = new DtoLogAzioni();
          risultato.Id = log.Id;
          risultato.IdUtente = log.IdUtente;
          risultato.NomeAzione = log.NomeAzione;
          risultato.Effettuato = log.Effettuato;
          risultato.Messaggio = log.Messaggio;
          risultato.TimeStamp = log.TimeStamp;
          // Inserisce il dto dentro la lista da ritornare
          risultati.Add(risultato);
        }

        // Ritorna la lista con tutti i log passati tramite dto
        return risultati;
    }

}
```

# Helpers

## CalcoliHelper.cs

```c#
// Classe helper che contiene funzioni di calcolo riutilizzabili nel progetto
public static class Calcoli
{
    // Calcola il prezzo finale di un acquisto in base a:
    // - prezzo base del film
    // - maggiorazione della sala
    // - numero di biglietti
    // - eventuale sconto dell’abbonamento dell’utente
    public static decimal CalcolaPrezzoFinale(decimal prezzoMovie, decimal maggiorazione, int numeroBiglietti, Utente utente)
    {
        // Caso: utente NON abbonato → prezzo pieno
        if (utente.SeAbbonato == false)
        {
            decimal prezzoFinale = (prezzoMovie + maggiorazione) * numeroBiglietti;
            return prezzoFinale;
        }
        else
        {
            // Caso: utente abbonato → applica sconto percentuale
            decimal prezzoBiglietto = prezzoMovie + maggiorazione;
            decimal sconto = (prezzoBiglietto / 100) * utente.Abbonamento.Sconto;
            decimal prezzoScontato = prezzoBiglietto - sconto;
            decimal prezzoFinale = prezzoScontato * numeroBiglietti;
            return prezzoFinale;
        }
    }

    // Calcola la data di scadenza aggiungendo la durata in mesi
    public static DateTimeOffset? CalcolaScadenza(DateTimeOffset dataInizio, int durata)
    {
        return dataInizio.AddMonths(durata);
    }

    // Restituisce quanti giorni mancano alla scadenza 
    public static int GiorniAllaScadenza(DateTimeOffset dataInizio, int durata)
    {
        DateTimeOffset dataScadenza = dataInizio.AddMonths(durata);
        TimeSpan differenza = dataScadenza - DateTime.Now;
        return (int)differenza.TotalDays;
    }
}
```

# Seed
Nel DataSeeder è stata apportata una piccola modifica nel settaggio degli orari per i turni, i quali vanno inizializzati già nel data seeder siccome sqLite non legge i time only. Di seguito riporto le linee che sono state modificate e il suo metodo.

```c#
//Gli orari prima avevano solo 10, 13, 18; di seguito abbiamo aggiunto i minuti e i secondi che permettono di visualizzare gli orari corretti nella tabella
await AssicuraEsistenzaTurno(contestoDb,new TimeOnly(10, 0, 0), new TimeOnly(13, 0, 0),"Mattina");
await AssicuraEsistenzaTurno(contestoDb,new TimeOnly(13, 0, 0), new TimeOnly(18, 0, 0),"Pomeriggio");
await AssicuraEsistenzaTurno(contestoDb,new TimeOnly(18, 0, 0), new TimeOnly(22, 0, 0),"Sera");
```

## AssicuraEsistenzaTurno
```c#
 private static async Task AssicuraEsistenzaTurno(
   ContestoDb context,
   TimeOnly oraInizio, TimeOnly oraFine, string nome)
   {
    List<Turno> turni = await context.Turni.ToListAsync();
    for (int i = 0; i < turni.Count; i++)
    {
        Turno turnoCorrente = turni[i];
        bool nomeUguale = string.Equals(
            turnoCorrente.Nome,
            nome,
            StringComparison.OrdinalIgnoreCase);
        if(nomeUguale || (turnoCorrente.OraInizio == oraInizio && turnoCorrente.OraFine == oraFine))
        {
            return;
        }
    }

    Turno nuovoTurno = new Turno
    {
        Nome      = nome,
        OraInizio = oraInizio,
        OraFine   = oraFine
    };

    context.Turni.Add(nuovoTurno);
    await context.SaveChangesAsync();
   }
```
