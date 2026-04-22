# NuovoCinemaParadisoV2.0

## Implementazione gift card.

Implementare una funzionalità che permette agli utenti di acquistare una gift card che comprende un numero di biglietti prestabilito, ogni volta che acquisterà un nuovo biglietto, questo verrà scalato dal totale dei biglietti della gift card arrivato a 0 la gift card viene disabilitata.

**Possibile implementazione: Funzionalità che permette agli utenti di regalare gift card ad altri utenti.** 

- 10 Film , Prezzo 85£.
- 25 Film , Prezzo 190£.
- 50 film , Prezzo 325£.

- Modello e riferimento nel ContestoDb.
- Dtos.
- Services e riferimento nel Program.cs.
- Controller.
- Calcolo nell'helper.

## Implementazione Proiezione.

Implementare una funzionalità che permette al gestore di creare, modificare o eliminare una proiezione.
Essa comprende il film, la sala, il turno e la data di trasmissione.

- Modello e riferimento nel ContestoDb.
- Dtos.
- Services e riferimento nel Program.cs.
- Controller.
- Calcolo nell'helper.

## Implementazione abbonamenti.

Implementare una funzionalità che permette agli utenti di acquistare un abbonamento che permette una scontistica sul prezzo dei biglietti.
Nella tabella utente ci sarà la foreign key che collega la corrispettiva tabella alla tabella abbonamenti.
Gli abbonamenti saranno di tre livelli: mensile, semestrale, annuale, ognuno con il suo prezzo e la sua data di inizio e di scadenza.

- mensile ( 25% Sconto ) , Prezzo 70£.
- semestrale ( 50% Sconto ) , Prezzo 210£.
- annuale ( 75% Sconto ) , Prezzo 300£.

- Modello e riferimento nel ContestoDb.
- Dtos.
- Services e riferimento nel Program.cs.
- Controller.
- Calcolo nell'helper.

# Models

## GiftCard.cs

```c#
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NuovoCinemaParadiso.Models;

[Table("GiftCard")]
// ✔ Mappa la classe alla tabella "GiftCard" nel database
public class GiftCard
{
    [Key]
    // ✔ Chiave primaria della Gift Card, generata automaticamente come GUID stringa
    public string Id { get; set; } = Guid.NewGuid().ToString();

    [Required]
    [StringLength(50)]
    // ✔ Nome identificativo della Gift Card (es. "Gift Card Silver")
    // ✔ Limitato a 50 caratteri per coerenza con la UI e il DB
    public string Nome { get; set; } = string.Empty;

    [Required]
    // ✔ Durata della Gift Card (giorni, settimane o mesi a seconda della logica di dominio)
    public int Durata { get; set; }

    [Required]
    // ✔ Prezzo base della Gift Card
    public decimal Prezzo { get; set; }

    [Required]
    // ✔ Numero di film inclusi nella Gift Card
    public int NumeroMovie { get; set; }

    // ✔ Lista degli utenti che possiedono questa Gift Card
    // ✔ Relazione uno-a-molti (una Gift Card → più utenti)
    public List<Utente> Utenti { get; set; } = new List<Utente>();
}
```

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

    /// <summary>
    /// Booleano se l'utente possiede una Gift Card.
    /// </summary>
    [Required]
    public bool PossiedeGiftCard { get; set; } = false;

    // Indica se l’utente ha un abbonamento attivo.
    [Required]
    public bool SeAbbonato { get; set; } = false;
    [Required]
    public bool PossiedeGiftCard { get; set; } = false;

    public DateTimeOffset DataInizioAbbonamento { get; set; }

    /// <summary>
    /// Data in cui la Gift Card è stata attivata dall'utente.
    /// Usa DateTimeOffset per mantenere il fuso orario.
    /// </summary>
    public DateTimeOffset DataInizioGiftCard { get; set; }

    // Acquisti associati all’utente.
    public List<Acquisto> Acquisti { get; set; } = new List<Acquisto>();

    public string? AbbonamentoId { get; set; }

    [ForeignKey("AbbonamentoId")]
    public Abbonamento? Abbonamento { get; set; }

    /// <summary>
    /// Chiave esterna verso la Gift Card posseduta dall'utente.
    /// Può essere null se l'utente non ha una Gift Card attiva.
    /// </summary>
    public string? GiftCardId { get; set; }

    [ForeignKey("GiftCardId")]
    /// <summary>
    /// Navigazione verso la Gift Card associata all'utente.
    /// Se GiftCardId è null, questa proprietà sarà null.
    /// </summary>
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

/// <summary>
/// Rappresenta un acquisto effettuato da un utente per una specifica proiezione.
/// Contiene informazioni su biglietti, prezzo finale, metodo di pagamento e timestamp.
/// </summary>
[Table("Acquisti")] 
public class Acquisto
{
    /// <summary>
    /// Identificativo univoco dell'acquisto.
    /// Viene generato automaticamente come stringa GUID.
    /// </summary>
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString();

    /// <summary>
    /// FK verso la proiezione acquistata.
    /// </summary>
    [Required]
    public string ProiezioneId { get; set; } = string.Empty;

    /// <summary>
    /// Navigazione verso la proiezione associata.
    /// </summary>
    [ForeignKey("ProiezioneId")]
    public Proiezione Proiezione { get; set; }

    /// <summary>
    /// FK verso l'utente che ha effettuato l'acquisto.
    /// </summary>
    [Required]
    public string UtenteId { get; set; } = string.Empty;

    /// <summary>
    /// Navigazione verso l'utente proprietario dell'acquisto.
    /// </summary>
    [ForeignKey("UtenteId")]
    public Utente Utente { get; set; }

    /// <summary>
    /// Numero di biglietti acquistati per questa proiezione.
    /// </summary>
    [Required]
    public int NumeroBiglietti { get; set; }

    /// <summary>
    /// Timestamp di creazione dell'acquisto.
    /// Utilizza DateTimeOffset per mantenere il fuso orario.
    /// </summary>
    public DateTimeOffset OrarioCreazione { get; set; } = DateTimeOffset.UtcNow;

    /// <summary>
    /// Prezzo finale calcolato al momento dell'acquisto.
    /// Include eventuali maggiorazioni, sconti e quantità di biglietti.
    /// </summary>
    [Required]
    public decimal PrezzoFinale { get; set; }

    /// <summary>
    /// Metodo di pagamento scelto dall'utente per l'acquisto.
    /// Valori possibili:
    /// - "standard" → pagamento a prezzo pieno
    /// - "abbonamento" → applica lo sconto previsto dal tipo di abbonamento
    /// - "giftcard" → utilizza i film disponibili nella gift card
    ///
    /// Campo obbligatorio: permette al server di applicare la logica corretta,
    /// soprattutto quando l'utente possiede sia un abbonamento che una gift card.
    /// </summary>
    [Required]
    public string MetodoPagamento { get; set; } = "standard";
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

## DtoCreazioneGiftCard.cs

```c#
namespace NuovoCinemaParadiso.Dtos;

/// <summary>
/// DTO utilizzato per la creazione di una nuova Gift Card.
/// Contiene i dati necessari per la validazione e la persistenza.
/// </summary>
public class DtoCreazioneGiftCard
{
    /// <summary>
    /// Nome identificativo della Gift Card (es. "Gift Card Silver").
    /// </summary>
    public string Nome { get; set; } = string.Empty;

    /// <summary>
    /// Durata della Gift Card espressa in giorni o mesi 
    /// (dipende dalla logica applicativa del dominio).
    /// </summary>
    public int Durata { get; set; }

    /// <summary>
    /// Prezzo base della Gift Card.
    /// </summary>
    public decimal Prezzo { get; set; }

    /// <summary>
    /// Numero di film inclusi nella Gift Card.
    /// </summary>
    public int NumeroMovie { get; set; }
}
```

## DtoGiftCard.cs

```c#
namespace NuovoCinemaParadiso.Dtos;

/// <summary>
/// DTO utilizzato per restituire i dati di una Gift Card.
/// È la versione "di output" mostrata a client, admin o UI.
/// </summary>
public class DtoGiftCard
{
    /// <summary>
    /// Identificativo univoco della Gift Card.
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// Nome descrittivo della Gift Card (es. "Gift Card Gold").
    /// </summary>
    public string Nome { get; set; } = string.Empty;

    /// <summary>
    /// Durata della Gift Card (giorni, settimane o mesi a seconda della logica di dominio).
    /// </summary>
    public int Durata { get; set; }

    /// <summary>
    /// Prezzo della Gift Card.
    /// </summary>
    public decimal Prezzo { get; set; }

    /// <summary>
    /// Numero di film inclusi nella Gift Card.
    /// </summary>
    public int NumeroMovie { get; set; }
}
```

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

/// <summary>
/// DTO che rappresenta i dati pubblici di un utente.
/// Viene utilizzato per restituire informazioni verso il frontend
/// senza esporre dettagli sensibili o proprietà interne di Identity.
/// </summary>
public class DtoUtente
{
    /// <summary>
    /// Identificativo univoco dell'utente (GUID generato da Identity).
    /// </summary>
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Nome e cognome dell'utente.
    /// </summary>
    public string NomeCompleto { get; set; } = string.Empty;

    /// <summary>
    /// Data di inizio dell'abbonamento dell'utente.
    /// Utilizza DateTimeOffset per mantenere il fuso orario.
    /// </summary>
    public DateTimeOffset DataInizio { get; set; }

    /// <summary>
    /// Data di attivazione della GiftCard dell'utente.
    /// </summary>
    public DateTimeOffset DataInizioGiftCard { get; set; }

    /// <summary>
    /// Indica se l'utente ha un abbonamento attivo.
    /// </summary>
    public bool SeAbbonato { get; set; }

    /// <summary>
    /// Indica se l'utente possiede una GiftCard attiva.
    /// </summary>
    public bool PossiedeGiftCard { get; set; } = false;

    /// <summary>
    /// Email dell'utente, utilizzata anche come username.
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Età dell'utente.
    /// </summary>
    public int Eta { get; set; }

    /// <summary>
    /// Identificativo dell'abbonamento associato all'utente.
    /// Vuoto se l'utente non è abbonato.
    /// </summary>
    public string AbbonamentoId { get; set; } = string.Empty;

    /// <summary>
    /// Identificativo della GiftCard associata all'utente.
    /// Vuoto se l'utente non possiede una GiftCard.
    /// </summary>
    public string GiftCardId { get; set; } = string.Empty;

    /// <summary>
    /// Nome o tipo dell'abbonamento (es. "Mensile", "Annuale").
    /// </summary>
    public string TipoAbbonamento { get; set; } = string.Empty;

    /// <summary>
    /// Nome o tipo della GiftCard (es. "Silver", "Gold", "Premium").
    /// </summary>
    public string TipoGiftCard { get; set; } = string.Empty;
}
```

## DtoCreazioneUtente.cs

```c#
using System.ComponentModel.DataAnnotations;

namespace NuovoCinemaParadiso.Dtos;

/// <summary>
/// DTO utilizzato per aggiornare o creare i dati anagrafici di un utente.
/// Non contiene informazioni sensibili come password o ruoli.
/// </summary>
public class DtoCreazioneUtente
{
    /// <summary>
    /// Nome completo dell'utente.
    /// Campo obbligatorio, massimo 100 caratteri.
    /// </summary>
    [Required]
    [StringLength(100)]
    public string NomeCompleto { get; set; } = string.Empty;
    
    /// <summary>
    /// Età dell'utente.
    /// Deve essere compresa tra 14 e 100 anni.
    /// </summary>
    [Required]
    [Range(14, 100, ErrorMessage = "L'età deve essere compresa tra 14 e 100")]
    public int Eta { get; set; }
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
namespace NuovoCinemaParadiso.Dtos;

/// <summary>
/// DTO utilizzato per restituire i dati di un acquisto al client.
/// Contiene informazioni essenziali come prezzo finale, metodo di pagamento,
/// numero di biglietti e timestamp.
/// </summary>
public class DtoAcquisto
{
    /// <summary>
    /// Identificativo univoco dell'acquisto.
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// Identificativo della proiezione acquistata.
    /// </summary>
    public string ProiezioneId { get; set; } = string.Empty;

    /// <summary>
    /// Identificativo dell'utente che ha effettuato l'acquisto.
    /// </summary>
    public string UtenteId { get; set; } = string.Empty;

    /// <summary>
    /// Prezzo finale calcolato lato server.
    /// Include eventuali sconti, maggiorazioni e quantità di biglietti.
    /// </summary>
    public decimal PrezzoFinale { get; set; }

    /// <summary>
    /// Timestamp di creazione dell'acquisto.
    /// Utilizza DateTimeOffset per mantenere il fuso orario.
    /// </summary>
    public DateTimeOffset OrarioCreazione { get; set; }

    /// <summary>
    /// Numero di biglietti acquistati.
    /// </summary>
    public int NumeroBiglietti { get; set; }

    /// <summary>
    /// Metodo di pagamento utilizzato per l'acquisto.
    /// Valori possibili:
    /// - "standard" → pagamento a prezzo pieno
    /// - "abbonamento" → applica lo sconto previsto dal tipo di abbonamento
    /// - "giftcard" → utilizza i film disponibili nella gift card
    ///
    /// Questo valore viene salvato nel database e restituito al client
    /// per mostrare come è stato effettuato il pagamento.
    /// </summary>
    public string MetodoPagamento { get; set; } = "standard";
}
```

## DtoCreazioneAcquisto.cs

```c#
using System.ComponentModel.DataAnnotations;

namespace NuovoCinemaParadiso.Dtos;

/// <summary>
/// DTO utilizzato dal client per creare un nuovo acquisto.
/// Contiene solo i dati necessari per effettuare l’operazione:
/// - la proiezione scelta
/// - il numero di biglietti
/// - il metodo di pagamento selezionato dall’utente
/// </summary>
public class DtoCreazioneAcquisto
{
    /// <summary>
    /// Identificativo della proiezione che l’utente desidera acquistare.
    /// Campo obbligatorio.
    /// </summary>
    [Required]
    public string ProiezioneId { get; set; } = string.Empty;

    /// <summary>
    /// Numero di biglietti richiesti dall’utente.
    /// Campo obbligatorio.
    /// </summary>
    [Required]
    public int NumeroBiglietti { get; set; }

    /// <summary>
    /// Metodo di pagamento scelto dall’utente.
    /// Valori possibili:
    /// - "standard" → pagamento a prezzo pieno
    /// - "abbonamento" → applica lo sconto previsto dal tipo di abbonamento
    /// - "giftcard" → utilizza i film disponibili nella gift card
    ///
    /// Questo campo permette all’utente di scegliere come pagare,
    /// soprattutto quando possiede sia un abbonamento che una gift card.
    /// </summary>
    public string MetodoPagamento { get; set; } = "standard";
}
```

# Controllers

## TipologiaSalaController.cs

```c#
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using NuovoCinemaParadiso.Services;
using NuovoCinemaParadiso.Dtos;
using NuovoCinemaParadiso.Models;

namespace NuovoCinemaParadiso.Controllers;

/// <summary>
/// Controller responsabile della gestione delle tipologie di sala.
/// Tutte le operazioni richiedono autenticazione.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TipologiaSalaController : ControllerBase
{
    private readonly TipologiaSalaService _tipologiaSalaService;
    private readonly LogAzioniService _logAzioniService;

    /// <summary>
    /// Costruttore del controller. Inietta i servizi necessari.
    /// </summary>
    public TipologiaSalaController(TipologiaSalaService tipologiaSalaService, LogAzioniService logAzioniService)
    {
        _tipologiaSalaService = tipologiaSalaService;
        _logAzioniService = logAzioniService;
    }

    /// <summary>
    /// Restituisce la lista completa delle tipologie di sala.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> OttieniTutti()
    {
        // Recupero di tutte le tipologie dal servizio
        List<DtoTipologiaSala> tipologieSala = await _tipologiaSalaService.OttieniTuttoAsync();

        // Identificativo dell'utente autenticato
        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        // Log dell'operazione
        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ottieni tutte le tipologie", true);

        return Ok(tipologieSala);
    }

    /// <summary>
    /// Restituisce una tipologia tramite il suo identificatore.
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> OttieniTramiteId(string id)
    {
        var risultato = await _tipologiaSalaService.OttieniTramiteIdAsync(id);
        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (risultato == null)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ottieni tipologie tramite id", false);
            return NotFound($"TipologiaSala con id {id} non trovato");
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ottieni tipologie tramite id", true);

        return Ok(risultato);
    }

    /// <summary>
    /// Crea una nuova tipologia di sala. Accessibile solo a Gestore o Operatore.
    /// </summary>
    [HttpPost]
    [Authorize(Roles = Ruoli.GestoreOrOperatore)]
    public async Task<IActionResult> Creazione([FromBody] DtoCreazioneTipologiaSala dto)
    {
        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        // Recupero di tutte le tipologie per verificare duplicati
        List<DtoTipologiaSala> tipologieSala = await _tipologiaSalaService.OttieniTuttoAsync();

        // Controllo duplicati (case-sensitive, Contains)
        foreach (var tipologiaSala in tipologieSala)
        {
            if (tipologiaSala.Nome.Contains(dto.Nome))
            {
                await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Creazione tipologia", false);
                return BadRequest(new { messaggio = "Tipologia sala già presente." });
            }
        }

        // Creazione della nuova tipologia
        DtoTipologiaSala? risultato = await _tipologiaSalaService.CreazioneAsync(dto);

        if (risultato == null)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Creazione tipologia", false);
            return BadRequest(new { messaggio = "Tipologia sala non valida." });
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Creazione tipologia", true);

        return Ok(risultato);
    }

    /// <summary>
    /// Modifica una tipologia esistente. Accessibile solo a Gestore o Operatore.
    /// </summary>
    [HttpPut("{id}")]
    [Authorize(Roles = Ruoli.GestoreOrOperatore)]
    public async Task<IActionResult> Modifica(string id, [FromBody] DtoCreazioneTipologiaSala dto)
    {
        DtoTipologiaSala? risultato = await _tipologiaSalaService.ModificaAsync(id, dto);
        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (risultato == null)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Modifica tipologia", false);
            return NotFound(new { messaggio = "Tipologia sala non trovata." });
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Modifica tipologia", true);

        return Ok(risultato);
    }

    /// <summary>
    /// Elimina una tipologia esistente. Accessibile solo a Gestore o Operatore.
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize(Roles = Ruoli.GestoreOrOperatore)]
    public async Task<IActionResult> Elimina(string id)
    {
        bool eliminato = await _tipologiaSalaService.EliminaAsync(id);
        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!eliminato)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Elimina tipologia", false);
            return NotFound(new { messaggio = "Tipologia sala non trovata." });
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Elimina tipologia", true);

        return NoContent();
    }

}
```

## SalaController.cs

```c#
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using NuovoCinemaParadiso.Services;
using NuovoCinemaParadiso.Dtos;

namespace NuovoCinemaParadiso.Controllers;

/// <summary>
/// Controller responsabile della gestione delle sale.
/// Tutte le operazioni richiedono autenticazione.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class SalaController : ControllerBase
{
    private readonly SalaService _salaService;
    private readonly LogAzioniService _logAzioniService;

    /// <summary>
    /// Costruttore del controller. Inietta i servizi necessari.
    /// </summary>
    public SalaController(SalaService salaService, LogAzioniService logAzioniService)
    {
        _salaService = salaService;
        _logAzioniService = logAzioniService;
    }

    /// <summary>
    /// Restituisce la lista completa delle sale.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> OttieniTutti()
    {
        // Recupero di tutte le sale dal servizio
        List<DtoSala> sale = await _salaService.OttieniTuttoAsync();

        // Identificativo dell'utente autenticato
        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        // Log dell'operazione
        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ottieni tutte le sale", true);

        return Ok(sale);
    }

    /// <summary>
    /// Restituisce tutte le sale appartenenti a una specifica tipologia.
    /// </summary>
    [HttpGet("tipologia/{tipologiaId}")]
    public async Task<ActionResult<List<DtoSala>>> OttieniPerTipologia(string tipologiaId)
    {
        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        // Validazione dell'input
        if (string.IsNullOrWhiteSpace(tipologiaId))
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ottieni sala per tipologia", false);
            return BadRequest("TipologiaId non valido");
        }

        // Recupero delle sale filtrate per tipologia
        var risultato = await _salaService.OttieniTramiteTipologiaAsync(tipologiaId);

        if (risultato == null || risultato.Count == 0)
        {
            await L_logAzioniService.SalvataggioLogAzioneAsyncog(utenteId, "Ottieni sala per tipologia", false);
            return NotFound("Nessuna sala trovata per questa tipologia");
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ottieni sala per tipologia", true);

        return Ok(risultato);
    }

    /// <summary>
    /// Restituisce una sala tramite il suo identificatore.
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> OttieniTramiteId(string id)
    {
        var risultato = await _salaService.OttieniTramiteIdAsync(id);
        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (risultato == null)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ottieni sala tramite id", false);
            return NotFound($"Sala con id {id} non trovato");
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ottieni sala tramite id", true);

        return Ok(risultato);
    }

    /// <summary>
    /// Crea una nuova sala. Accessibile solo a Gestore o Operatore.
    /// </summary>
    [HttpPost]
    [Authorize(Roles = Ruoli.GestoreOrOperatore)]
    public async Task<IActionResult> Creazione([FromBody] DtoCreazioneSala dto)
    {
        // Creazione della sala tramite servizio
        DtoSala? risultato = await _salaService.CreazioneAsync(dto);
        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (risultato == null)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Creazione sala", false);
            return BadRequest(new { messaggio = "Sala non valida." });
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Creazione sala", true);

        return Ok(risultato);
    }

    /// <summary>
    /// Modifica una sala esistente. Accessibile solo a Gestore o Operatore.
    /// </summary>
    [HttpPut("{id}")]
    [Authorize(Roles = Ruoli.GestoreOrOperatore)]
    public async Task<IActionResult> Modifica(string id, [FromBody] DtoCreazioneSala dto)
    {
        DtoSala? risultato = await _salaService.ModificaAsync(id, dto);
        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (risultato == null)
        {
            await L_logAzioniService.SalvataggioLogAzioneAsyncog(utenteId, "Modifica sala", false);
            return NotFound(new { messaggio = "Sala non trovata." });
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Modifica sala", true);

        return Ok(risultato);
    }

    /// <summary>
    /// Elimina una sala esistente. Accessibile solo a Gestore o Operatore.
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize(Roles = Ruoli.GestoreOrOperatore)]
    public async Task<IActionResult> Elimina(string id)
    {
        bool eliminato = await _salaService.EliminaAsync(id);
        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!eliminato)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Elimina sala", false);
            return NotFound(new { messaggio = "Sala non trovata." });
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Elimina sala", true);

        return NoContent();
    }
}
```

## GiftCardController.cs

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
[Authorize] // Tutte le azioni richiedono autenticazione
public class GiftCardController : ControllerBase
{
    private readonly GiftCardService _giftCardService;
    private readonly LogAzioniService _logAzioniService;

    public GiftCardController(GiftCardService giftCardService, LogAzioniService logAzioniService)
    {
        _giftCardService = giftCardService;
        _logAzioniService = logAzioniService;
    }

    // ---------------------------------------------------------
    // OTTIENI TUTTE LE GIFT CARD
    // ---------------------------------------------------------
    [HttpGet]
    public async Task<IActionResult> OttieniTutteLeGiftCard()
    {
        // Recupera tutte le gift card
        List<DtoGiftCard> giftCards = await _giftCardService.OttieniTutto();

        // Recupera ID dell’utente loggato dal token JWT
        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        // Registra log dell’azione
        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId,"Ottieni tutte le giftcards" ,true);


        return Ok(giftCards);
    }

    // ---------------------------------------------------------
    // OTTIENI GIFT CARD TRAMITE ID (solo se appartiene all’utente)
    // ---------------------------------------------------------
    [HttpGet("{id}")]
    public async Task<IActionResult> OttieniTramiteId(string id)
    {
        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        // Recupera gift card solo se appartiene all’utente
        var risultato = await _giftCardService.OttieniTramiteIdAsync(id, utenteId);

        if (risultato == null)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId,"Ottieni giftcard tramite id" ,true);
            return NotFound($"GiftCard con id {id} non trovato");
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId,"Ottieni giftcard tramite id" ,true);

        return Ok(risultato);
    }

    // ---------------------------------------------------------
    // CREAZIONE GIFT CARD (solo Gestore o Operatore)
    // ---------------------------------------------------------
    [HttpPost]
    [Authorize(Roles = Ruoli.GestoreOrOperatore)]
    public async Task<IActionResult> Creazione([FromBody] DtoCreazioneGiftCard dto)
    {
        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        // Crea nuova gift card
        DtoGiftCard? risultato = await _giftCardService.CreazioneAsync(dto);

        // Registra log
        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId,"Creazione di una giftcard" ,true);

        return Ok(risultato);
    }

    // ---------------------------------------------------------
    // MODIFICA GIFT CARD (solo Gestore o Operatore)
    // ---------------------------------------------------------
    [HttpPut("{id}")]
    [Authorize(Roles = Ruoli.GestoreOrOperatore)]
    public async Task<IActionResult> Modifica(string id, [FromBody] DtoCreazioneGiftCard dto)
    {
        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        // Modifica gift card
        DtoGiftCard? risultato = await _giftCardService.ModificaAsync(id, dto);

        if (risultato == null)
        {
          await _logAzioniService.SalvataggioLogAzioneAsync(utenteId,"Modifica giftcard" ,false);
          return NotFound(new { messaggio = "GiftCard non trovata." });
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Modifica GiftCard", true );

        return Ok(risultato);
    }

    // ---------------------------------------------------------
    // ELIMINA GIFT CARD (solo Gestore o Operatore)
    // ---------------------------------------------------------
    [HttpDelete("{id}")]
    [Authorize(Roles = Ruoli.GestoreOrOperatore)]
    public async Task<IActionResult> Elimina(string id)
    {
        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        // Elimina gift card
        bool eliminato = await _giftCardService.EliminazioneAsync(id);

        if (!eliminato)
         {
            await _logAzioniService.SalvataggioLogAzioneAsync (utenteId, "eliminazione GiftCard", false );


            return NotFound(new { messaggio = "GiftCard non trovata." });
         }

        await _logAzioniService.SalvataggioLogAzioneAsync (utenteId, "eliminazione GiftCard", true );

        return NoContent();
    }
}
```

## AbbonamentoController.cs

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
                await _logAzioniService.SalvataggioLogAzioneAsync(utenteId,"Ottieni tutti gli abbonamenti utente" ,true);

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
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId,"Ottieni abbonamenti tramite id admin",false);

            return NotFound($"Abbonamento con id {id} non trovato");
        }

        // ✔ Log successo
        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId,"Ottieni abbonamenti tramite id admin",true);


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
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId,"Ottieni abbonamenti tramite id utente",false);


            return NotFound($"Abbonamento con id {id} non trovato");
        }

        // ✔ Log successo
        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId,"Ottieni abbonamenti tramite id utente",true);


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
            await _logAzioniService.SalvataggioLogAzioneAsync( utenteId,"Creazione abbonamento",false);


            return BadRequest(new { messaggio = "Abbonamento già presente oppure non valido." });
        }

        // ✔ Log successo
        await _logAzioniService.SalvataggioLogAzioneAsync( utenteId,"Creazione abbonamento",true);


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
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId,"Modifica abbonamento",false);


            return NotFound(new { messaggio = "Abbonamento non trovato." });
        }

        // ✔ Log successo
        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId,"Modifica abbonamento",true);

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
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId,"Elimina abbonamento", false);


            return NotFound(new { messaggio = "Abbonamento non trovato." });
        }

        // ✔ Log successo
        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId,"Elimina abbonamento", true);


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
        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId,"Ricerca profili",true);


        // Secondo log (puoi unificarli se vuoi).
        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ottieni tutti i profili",true);


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
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId,"Ricerca profilo",false);


            return NotFound(new { messaggio = "Utente non trovato." });
        }

        // Log successo.
        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId,"Ricerca profilo",true);


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
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Eliminazione profilo",false);


            return NotFound(new { messaggio = "Utente non trovato." });
        }

        // Log successo.
        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Eliminazione profilo",true);


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
        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId,"Ottieni tutti gli acquisti admin",true);



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
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId,"Ottieni acquisti tramite id admin",false);


            return NotFound($"Acquisto con id {id} non trovato");
        }

        // Log successo.
        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId,"Ottieni acquisti tramite id admin",true);


        return Ok(risultato);
    }

    // ------------------------------------------------------------
    // 6) OTTIENI UTENTI TRAMITE ABBONAMENTO
    // ------------------------------------------------------------
    [HttpGet("utenti/abbonamenti/{abbonamentoid}")]
    [Authorize(Roles = Ruoli.GestoreOrOperatore)]
    public async Task<ActionResult<List<DtoUtente>>> OttieniTramiteAbbonamentoAsync(string abbonamentoId)
    {
        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        // Validazione dell’ID.
        if (string.IsNullOrWhiteSpace(abbonamentoId))
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId,"Ottieni gli utenti per abbonamento",false);

            return BadRequest("AbbonamentoId non valido");
        }

        // Recupero gli utenti con quell’abbonamento.
        var risultato = await _adminService.OttieniTramiteAbbonamentoAsync(abbonamentoId);

        // Se nessuno trovato → log fallimento + 404.
        if (risultato == null || risultato.Count == 0)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId,"Ottieni gli utenti per abbonamento",false);


            return NotFound("Nessun utente trovato per questo abbonamento");
        }

        // Log successo.
        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId,"Ottieni gli utenti per abbonamento",true);


        return Ok(risultato);
    }
    // ------------------------------------------------------------
    // 7) OTTIENI ABBONAMENTO TRAMITE ID
    // ------------------------------------------------------------
    [HttpGet("abbonamento/{id}")]
    [Authorize(Roles = Ruoli.GestoreOrOperatore)]
    public async Task<IActionResult> OttieniAbbonamentoTramiteIdPerAdmin(string id)
    {
        // chiamo il service per trovare l'abbonamento
        var risultato = await _adminService.OttieniAbbonamentoTramiteIdPerAdminAsync(id);
        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        // validazione dell'ID
        if (risultato == null)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId,"Ottieni abbonamenti tramite id admin",false);
            // ritorno un 404 quando non riesco a trovarlo
            return NotFound($"Abbonamento con id {id} non trovato");
        }
        // log di successo
        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId,"Ottieni abbonamenti tramite id admin",true);

        return Ok(risultato);
    }

    // ------------------------------------------------------------
    // 8) OTTIENI GIFTCARD TRAMITE ID
    // ------------------------------------------------------------
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
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ottieni giftcard tramite id admin",false);

            return NotFound($"Giftcard con id {id} non trovato");
        }

        // Se la gift card esiste, logga l’operazione come riuscita
        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ottieni giftcard tramite id admin",true);


        // Restituisce la gift card trovata
        return Ok(risultato);
    }
    // ------------------------------------------------------------
    // 9) OTTIENI UTENTI TRAMITE L'ID DELLA GIFTCARD 
    // ------------------------------------------------------------
    [HttpGet("utenti/giftCard/{giftcardId}")]
    [Authorize(Roles = Ruoli.GestoreOrOperatore)]
    public async Task<ActionResult<List<DtoUtente>>> OttieniUtentiTramiteGiftCardPerAdmin(string giftcardId)
    {
        // Recupera l'id dell’utente autenticato
        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        // Controllo base sul parametro ricevuto
        if (string.IsNullOrWhiteSpace(giftcardId))
        {
            // Log operazione fallita per parametro non valido
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId,"Ottieni gli utenti per giftcard",false);


            return BadRequest("giftcardId non valido");
        }

        // Recupera gli utenti associati alla gift card
        var risultato = await _adminService.OttieniUtentiTramiteGiftCardAsync(giftcardId);

        // Se non ci sono utenti associati, logga fallimento e ritorna 404
        if (risultato == null)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId,"Ottieni gli utenti per giftcard",false);


            return NotFound("Nessun utente trovato per questa giftcard");
        }

        // Log operazione riuscita
        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId,"Ottieni gli utenti per giftcard",true);


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

       await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ottieni tutte le proieioni",true);

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
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ottieni proiezione tramite id",false);

            return NotFound($"Proiezione con id {id} non trovato");
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ottieni proiezione tramite id", true);

        return Ok(risultato);
    }

    // Restituisce tutte le proiezioni associate a un turno specifico.
    [HttpGet("turno/{turnoId}")]
    public async Task<ActionResult<List<DtoProiezione>>> OttieniPerTurno(string turnoId)
    {
        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(turnoId))
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ottieni proiezione per turnoId", false);

            return BadRequest("TurnoId non valido");
        }

        var risultato = await _proiezioneService.OttieniTramiteTurnoAsync(turnoId);

        if (risultato == null)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ottieni proiezione per turnoid", false);

            return NotFound("Nessuna proiezione trovata per questo turno");
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ottieni proiezione per turnoId", true);
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
            if (string.IsNullOrEmpty(salaId))
           {
             await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ottieni proiezione per sala", false);
             return BadRequest("SalaId non valido");
           }

            // Restituisce errore 400 Bad Request
            return BadRequest("SalaId non valido");
        }

        // Chiama il service per ottenere le proiezioni della sala
        var risultato = await _proiezioneService.OttieniTramiteSalaAsync(salaId);

        // Se non viene trovato nulla
        if (risultato == null)
        {
            // Log dell'azione fallita
            {
             await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ottieni proiezione per sala", false);
             return NotFound("Nessuna proiezione trovata per questa sala");
            }

            // Restituisce errore 404 Not Found
            return NotFound("Nessuna proiezione trovata per questa sala");
        }

        // Log dell'azione completata con successo
        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ottieni proiezione per sala", true);

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
        if (string.IsNullOrEmpty(movieId))
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ottieni proiezioni per film", false);
            return BadRequest("MovieId non valido");
        
        // Restituisce errore 400 Bad Request
        return BadRequest("MovieId non valido");
        }
    }

    // Chiama il service per ottenere le proiezioni del film
    var risultato = await _proiezioneService.OttieniTramiteMovieAsync(movieId);

    // Se non viene trovato nulla
    if (risultato == null)
    {
        // Log dell'azione fallita
        if (risultato == null)
        {
         await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ottieni proiezioni per film", false);
         // Restituisce errore 404 Not Found
         return NotFound("Nessuna proiezione trovata per questo film");
        }
    }

    // Log dell'azione completata con successo
    await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ottieni proiezioni per film", true);

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
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Crea proiezione", false);
            return BadRequest(new { messaggio = "Proiezione già presente." });
        }

        DtoProiezione? risultato = await _proiezioneService.CreazioneAsync(dto);

        if (risultato == null)
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Crea proiezione", false);
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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using NuovoCinemaParadiso.Dtos;
using NuovoCinemaParadiso.Services;

namespace NuovoCinemaParadiso.Controllers;

/// <summary>
/// Controller responsabile della gestione dei turni.
/// Tutte le operazioni richiedono autenticazione.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TurnoController : ControllerBase
{
    private readonly TurnoService _turnoService;
    private readonly LogAzioniService _logAzioniService;

    /// <summary>
    /// Costruttore del controller. Inietta i servizi necessari.
    /// </summary>
    public TurnoController(TurnoService turnoService, LogAzioniService logAzioniService)
    {
        _turnoService = turnoService;
        _logAzioniService = logAzioniService;
    }

    /// <summary>
    /// Restituisce la lista completa dei turni.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> OttieniTutti()
    {
        // Recupero di tutti i turni dal servizio
        List<DtoTurno> turni = await _turnoService.OttieniTuttoAsync();

        // Identificativo dell'utente autenticato
        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        // Log dell'operazione
        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ottieni tutti i turni", true);

        return Ok(turni);
    }

    /// <summary>
    /// Restituisce un turno tramite il suo identificatore.
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> OttieniTramiteId(string id)
    {
        var risultato = await _turnoService.OttieniTramiteIdAsync(id);
        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (risultato == null)
        {
            // Log operazione fallita
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ottieni turno tramite id", false);

            return NotFound($"Turno con id {id} non trovato");
        }

        // Log operazione riuscita
        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ottieni turno tramite id", true);

        return Ok(risultato);
    }

    /// <summary>
    /// Crea un nuovo turno. Accessibile solo a Gestore o Operatore.
    /// </summary>
    [HttpPost]
    [Authorize(Roles = Ruoli.GestoreOrOperatore)]
    public async Task<IActionResult> Creazione([FromBody] DtoCreazioneTurno dto)
    {
        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        // Recupero di tutti i turni per verificare duplicati
        List<DtoTurno> turni = await _turnoService.OttieniTuttoAsync();

        // Controllo se esiste già un turno con lo stesso nome (case-insensitive)
        foreach (var turno in turni)
        {
            bool stringheUguali = string.Equals(turno.Nome, dto.Nome, StringComparison.OrdinalIgnoreCase);
            if (stringheUguali)
            {
                await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Creazione Turno", false);
                return BadRequest(new { messaggio = "Turno già presente." });
            }
        }

        // Creazione del nuovo turno
        DtoTurno? risultato = await _turnoService.CreazioneAsync(dto);

        if (risultato == null)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Creazione Turno", false);
            return BadRequest(new { messaggio = "Turno non valido." });
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Creazione Turno", true);

        return Ok(risultato);
    }

    /// <summary>
    /// Modifica un turno esistente. Accessibile solo a Gestore o Operatore.
    /// </summary>
    [HttpPut("{id}")]
    [Authorize(Roles = Ruoli.GestoreOrOperatore)]
    public async Task<IActionResult> Modifica(string id, [FromBody] DtoCreazioneTurno dto)
    {
        DtoTurno? risultato = await _turnoService.ModificaAsync(id, dto);
        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (risultato == null)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Modifica Turno", false);
            return NotFound(new { messaggio = "Turno non trovato." });
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Modifica Turno", true);

        return Ok(risultato);
    }

    /// <summary>
    /// Elimina un turno esistente. Accessibile solo a Gestore o Operatore.
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize(Roles = Ruoli.GestoreOrOperatore)]
    public async Task<IActionResult> Elimina(string id)
    {
        bool eliminato = await _turnoService.EliminaAsync(id);
        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!eliminato)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Elimina Turno", false);
            return NotFound(new { messaggio = "Turno non trovato." });
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Elimina Turno", true);

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
        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId,"Ottieni tutti gli acquisti utente",true);


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
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId,"Ottieni acquisti tramite id utente",false);


            return NotFound($"Acquisto con id {id} non trovato");
        }

        // Log operazione riuscita
        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId,"Ottieni acquisti tramite id utente",true);


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
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Creazione acquisto",false);


            return BadRequest(new { messaggio = "Acquisto già presente oppure non valido." });
        }

        // Log operazione riuscita
        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Creazione acquisto",true);


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
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId,"Modifica acquisto",false);


            return NotFound(new { messaggio = "Acquisto non trovato." });
        }

        // Log operazione riuscita
        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId,"Modifica acquisto",true);


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
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId,"Elimina acquisto",false);


            return NotFound(new { messaggio = "Acquisto non trovato." });
        }

        // Log operazione riuscita
        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId,"Elimina acquisto",true);


        return NoContent();
    }
}
```
## MovieController.cs
```c#
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using NuovoCinemaParadiso.Services;
using NuovoCinemaParadiso.Dtos;

namespace NuovoCinemaParadiso.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class MovieController : ControllerBase
{
    private readonly MovieService _movieService;
    private readonly LogAzioniService _logAzioniService;

    public MovieController(MovieService movieService, LogAzioniService logAzioniService)
    {
        _movieService = movieService;
        _logAzioniService = logAzioniService;
    }

    [HttpGet]
    public async Task<IActionResult> OttieniTuttiIMovies()
    {
        // Recupera l'identificativo dell'utente autenticato
        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        // Recupera tutti i film dal servizio
        List<DtoMovie> movies = await _movieService.OttieniTutto();

        // Registra l'azione dell'utente nel log
        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId,"Ottieni tutti i movies" ,true);

        // Restituisce la lista dei film
        return Ok(movies);
    }

    [HttpGet("genere/{genereId}")]
    public async Task<ActionResult<List<DtoMovie>>> OttieniPerGenere(string genereId)
    {
        // Recupera l'identificativo dell'utente autenticato
        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        // Controlla che il parametro genereId sia valido
        if (string.IsNullOrWhiteSpace(genereId))
        {
            // Registra tentativo non valido nel log
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId,"Ottieni movie per genereId" ,false);

            // Restituisce errore 400 per parametro non valido
            return BadRequest("GenereId non valido");
        }

        // Recupera i film filtrati per genere
        var risultato = await _movieService.OttieniTramiteGenere(genereId);

        // Verifica se il risultato è nullo
        if (risultato == null)
        {
            // Registra nel log l'esito negativo
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ottieni movie per genereid", false);

            // Restituisce 404 se non ci sono risultati
            return NotFound("Nessun film trovato per questo genere");
        }

        // Verifica se la lista dei film è vuota
        if (risultato.Count == 0)
        {
            // Registra nel log l'azione completata
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ottieni movie per genereid", true);

            // Restituisce una lista vuota con stato 200
            return Ok(new List<DtoMovie>());
        }

        // Registra nel log l'azione completata con successo
        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ottieni movie per genereid", true);

        // Restituisce la lista dei film trovati
        return Ok(risultato);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> OttieniTramiteId(string id)
    {
        // Recupera l'identificativo dell'utente autenticato
        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        // Recupera il film tramite id
        var risultato = await _movieService.OttieniTramiteIdAsync(id);

        // Controlla se il film esiste
        if (risultato == null)
        {
            // Registra nel log il fallimento della ricerca
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ottieni movie per id", false);

            // Restituisce 404 se il film non esiste
            return NotFound($"Film con id {id} non trovato");
        }

        // Registra nel log il successo dell'operazione
        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ottieni movie per id", true);

        // Restituisce il film trovato
        return Ok(risultato);
    }

    [HttpPost]
    [Authorize(Roles = Ruoli.GestoreOrOperatore)]
    public async Task<IActionResult> Creazione([FromBody] DtoCreazioneMovie dto)
    {
        // Recupera l'identificativo dell'utente autenticato
        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        // Recupera tutti i film esistenti
        List<DtoMovie> movies = await _movieService.OttieniTutto();

        // Controlla se esiste già un film con titolo simile
        foreach (var movie in movies)
        {
            if (movie.Titolo.Contains(dto.Titolo))
            {
                // Registra nel log il fallimento della creazione
                await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Creazione movie", false);

                // Restituisce errore se il film esiste già
                return BadRequest(new { messaggio = "Film già presente." });
            }
        }

        // Crea un nuovo film tramite il servizio
        DtoMovie? risultato = await _movieService.CreazioneAsync(dto);

        // Controlla se la creazione è andata a buon fine
        if (risultato == null)
        {
            // Registra nel log il fallimento della creazione
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Creazione movie", false);

            // Restituisce errore di validazione
            return BadRequest(new { messaggio = "Film non valido." });
        }

        // Registra nel log la creazione avvenuta con successo
        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Creazione movie", true);

        // Restituisce il film creato
        return Ok(risultato);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = Ruoli.GestoreOrOperatore)]
    public async Task<IActionResult> Modifica(string id, [FromBody] DtoCreazioneMovie dto)
    {
        // Recupera l'identificativo dell'utente autenticato
        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        // Aggiorna il film con i nuovi dati
        DtoMovie? risultato = await _movieService.ModificaAsync(id, dto);

        // Controlla se il film esiste
        if (risultato == null)
        {
            // Registra nel log il fallimento della modifica
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Modifica movie", true);

            // Restituisce 404 se il film non esiste
            return NotFound(new { messaggio = "Film non trovato." });
        }

        // Registra nel log la modifica avvenuta con successo
        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Modifica movie", true);

        // Restituisce il film aggiornato
        return Ok(risultato);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = Ruoli.GestoreOrOperatore)]
    public async Task<IActionResult> Elimina(string id)
    {
        // Recupera l'identificativo dell'utente autenticato
        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        // Elimina il film tramite id
        bool eliminato = await _movieService.EliminaAsync(id);

        // Controlla se l'eliminazione è avvenuta
        if (!eliminato)
        {
            // Registra nel log il fallimento dell'eliminazione
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Eliminazione movie", false);

            // Restituisce 404 se il film non esiste
            return NotFound(new { messaggio = "Film non trovato." });
        }

        // Registra nel log l'eliminazione avvenuta con successo
        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Eliminazione movie", true);

        // Restituisce 204 senza contenuto
        return NoContent();
    }
}
```Service

## GiftCardService.cs 

```c#
using Microsoft.EntityFrameworkCore;
using NuovoCinemaParadiso.Data;
using NuovoCinemaParadiso.Dtos;
using NuovoCinemaParadiso.Models;

namespace NuovoCinemaParadiso.Services;

/// <summary>
/// Service dedicato alla gestione delle GiftCard.
/// Contiene operazioni CRUD e metodi di recupero.
/// </summary>
public class GiftCardService
{
    private readonly ContestoDb _contesto;

    public GiftCardService(ContestoDb contesto)
    {
        _contesto = contesto;
    }

    // ---------------------------------------------------------
    // OTTIENI TUTTE LE GIFT CARD
    // ---------------------------------------------------------
    /// <summary>
    /// Restituisce tutte le GiftCard presenti nel sistema.
    /// </summary>
    public async Task<List<DtoGiftCard>> OttieniTutto()
    {
        // Recupera tutte le gift card dal database
        List<GiftCard> giftCards = await _contesto.GiftCards.ToListAsync();

        List<DtoGiftCard> risultato = new List<DtoGiftCard>();

        // Mapping manuale GiftCard → DtoGiftCard
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

    // ---------------------------------------------------------
    // OTTIENI GIFT CARD TRAMITE ID (solo se appartiene all’utente)
    // ---------------------------------------------------------
    /// <summary>
    /// Restituisce una GiftCard tramite ID, ma solo se appartiene all’utente specificato.
    /// </summary>
    public async Task<DtoGiftCard?> OttieniTramiteIdAsync(string id, string utenteId)
    {
        // Recupera la gift card tramite chiave primaria
        GiftCard? giftCard = await _contesto.GiftCards.FindAsync(id);

        if (giftCard == null)
        {
            return null;
        }

        // Controlla se l’utente possiede questa gift card
        foreach (var utente in giftCard.Utenti)
        {
            if (utente.Id == utenteId)
            {
                return new DtoGiftCard
                {
                    Id = giftCard.Id,
                    Nome = giftCard.Nome,
                    Durata = giftCard.Durata,
                    Prezzo = giftCard.Prezzo,
                    NumeroMovie = giftCard.NumeroMovie
                };
            }
        }

        return null;
    }

    // ---------------------------------------------------------
    // CREAZIONE GIFT CARD
    // ---------------------------------------------------------
    /// <summary>
    /// Crea una nuova GiftCard nel sistema.
    /// </summary>
    public async Task<DtoGiftCard> CreazioneAsync(DtoCreazioneGiftCard dto)
    {
        // Crea nuova entità GiftCard
        GiftCard giftCard = new GiftCard();
        giftCard.Nome = dto.Nome;
        giftCard.Durata = dto.Durata;
        giftCard.Prezzo = dto.Prezzo;
        giftCard.NumeroMovie = dto.NumeroMovie;

        // Salva nel database
        _contesto.GiftCards.Add(giftCard);
        await _contesto.SaveChangesAsync();

        // Restituisce DTO della gift card creata
        return new DtoGiftCard
        {
            Id = giftCard.Id,
            Nome = giftCard.Nome,
            Durata = giftCard.Durata,
            Prezzo = giftCard.Prezzo,
            NumeroMovie = giftCard.NumeroMovie
        };
    }

    // ---------------------------------------------------------
    // MODIFICA GIFT CARD
    // ---------------------------------------------------------
    /// <summary>
    /// Modifica una GiftCard esistente tramite ID.
    /// </summary>
    public async Task<DtoGiftCard?> ModificaAsync(string id, DtoCreazioneGiftCard dto)
    {
        // Recupera gift card tramite ID
        GiftCard? giftCard = await _contesto.GiftCards.FindAsync(id);

        if (giftCard == null)
            return null;

        // Aggiorna i campi modificabili
        giftCard.Nome = dto.Nome;
        giftCard.Durata = dto.Durata;
        giftCard.Prezzo = dto.Prezzo;
        giftCard.NumeroMovie = dto.NumeroMovie;

        // Salva modifiche
        await _contesto.SaveChangesAsync();

        // Restituisce DTO aggiornato
        return new DtoGiftCard
        {
            Id = giftCard.Id,
            Nome = giftCard.Nome,
            Durata = giftCard.Durata,
            Prezzo = giftCard.Prezzo,
            NumeroMovie = giftCard.NumeroMovie
        };
    }

    // ---------------------------------------------------------
    // ELIMINA GIFT CARD
    // ---------------------------------------------------------
    /// <summary>
    /// Elimina una GiftCard tramite ID.
    /// </summary>
    public async Task<bool> EliminazioneAsync(string id)
    {
        // Recupera gift card tramite ID
        GiftCard? giftCard = await _contesto.GiftCards.FindAsync(id);

        if (giftCard == null)
        {
            return false;
        }

        // Rimuove dal database
        _contesto.GiftCards.Remove(giftCard);
        await _contesto.SaveChangesAsync();

        return true;
    }
}
```

## AuthService.cs 

```c#
using Microsoft.AspNetCore.Identity;
using NuovoCinemaParadiso.Models;
using NuovoCinemaParadiso.Dtos;
using NuovoCinemaParadiso.Helpers;
using NuovoCinemaParadiso.Data;
using Microsoft.EntityFrameworkCore;

namespace NuovoCinemaParadiso.Services;

/// <summary>
/// Service responsabile della gestione dell’autenticazione e della gestione utenti.
/// Include registrazione, login, modifica profilo, eliminazione e recupero dati.
/// </summary>
public class AuthService
{
    private readonly UserManager<Utente> _gestioneUtenti;
    private readonly SignInManager<Utente> _gestioneAccesso;
    private readonly JwtHelper _jwtHelper;
    private readonly ContestoDb _contesto;

    public AuthService(UserManager<Utente> gestioneUtenti, SignInManager<Utente> gestioneAccesso, JwtHelper jwtHelper, ContestoDb contesto)
    {
        _gestioneUtenti = gestioneUtenti;
        _gestioneAccesso = gestioneAccesso;
        _jwtHelper = jwtHelper;
        _contesto = contesto;
    }

    // ---------------------------------------------------------
    // REGISTRAZIONE UTENTE
    // ---------------------------------------------------------
    /// <summary>
    /// Registra un nuovo utente nel sistema.
    /// Controlla se l’email è già presente e assegna automaticamente il ruolo "Utente".
    /// </summary>
    public async Task<IdentityResult> RegistrazioneAsync(DtoRegistrazione dto)
    {
        // Verifica se esiste già un utente con la stessa email
        Utente? esisteUtente = await _gestioneUtenti.FindByEmailAsync(dto.Email);

        if (esisteUtente != null)
        {
            // Costruisce un errore personalizzato
            IdentityError errore = new IdentityError
            {
                Description = "Utente già registrato."
            };

            return IdentityResult.Failed(errore);
        }

        // Creazione nuovo utente Identity
        Utente utente = new Utente
        {
            UserName = dto.Email,
            Email = dto.Email,
            NomeCompleto = dto.NomeCompleto,
            Eta = dto.Eta
        };

        // Creazione utente con password
        IdentityResult risultato = await _gestioneUtenti.CreateAsync(utente, dto.Password);

        if (!risultato.Succeeded)
        {
            return risultato;
        }

        // Assegna ruolo base "Utente"
        IdentityResult aggiuntaRisultatoRuolo = await _gestioneUtenti.AddToRoleAsync(utente, Ruoli.Utente);

        if (!aggiuntaRisultatoRuolo.Succeeded)
            return aggiuntaRisultatoRuolo;

        return risultato;
    }

    // ---------------------------------------------------------
    // LOGIN UTENTE
    // ---------------------------------------------------------
    /// <summary>
    /// Effettua il login dell’utente verificando email, password e stato di abbonamento/gift card.
    /// Genera un token JWT contenente i ruoli dell’utente.
    /// </summary>
    public async Task<DtoAuthResponse?> LoginAsync(DtoLogin dto)
    {
        // Recupera utente tramite email
        Utente? utente = await _gestioneUtenti.FindByEmailAsync(dto.Email);
        
        if (utente == null)
        {
            return null;
        }
        
        // controllo su abbonamenti o giftcard collegati all'utente
        Abbonamento? abbonamento = null;
        GiftCard? giftCard = null;

        if (!string.IsNullOrEmpty(utente.AbbonamentoId))
        {
            abbonamento = await _contesto.Abbonamenti.FindAsync(utente.AbbonamentoId);
        }

        if (!string.IsNullOrEmpty(utente.GiftCardId))
        {
            giftCard = await _contesto.GiftCards.FindAsync(utente.GiftCardId);
        }

        // ---------------------------------------------------------
        // CONTROLLO SCADENZA ABBONAMENTO
        // ---------------------------------------------------------
        if (utente.SeAbbonato == true)
        {
            DateTimeOffset? scadenzaAbbonamento =
                Calcoli.CalcolaScadenza(utente.DataInizioAbbonamento, abbonamento.Durata);

            int giorniMancanti =
                Calcoli.GiorniAllaScadenza(utente.DataInizioAbbonamento, abbonamento.Durata);

            // Se scaduto → disattiva abbonamento
            if (giorniMancanti <= 0)
            {
                utente.SeAbbonato = false;
            }
        }

        // ---------------------------------------------------------
        // CONTROLLO SCADENZA GIFT CARD
        // ---------------------------------------------------------
        if (utente.PossiedeGiftCard == true)
        {
            DateTimeOffset? scadenzaGiftCard =
                Calcoli.CalcolaScadenza(utente.DataInizioGiftCard, utente.GiftCard.Durata);

            int giorniMancanti =
                Calcoli.GiorniAllaScadenza(utente.DataInizioGiftCard, utente.GiftCard.Durata);

            // Se scaduta → disattiva gift card
            if (giorniMancanti <= 0)
            {
                utente.PossiedeGiftCard = false;
            }
        }
        
        // ---------------------------------------------------------
        // VERIFICA PASSWORD
        // ---------------------------------------------------------
        SignInResult result =
            await _gestioneAccesso.CheckPasswordSignInAsync(utente, dto.Password, false);

        if (!result.Succeeded)
        {
            return null;
        }

        // Recupera ruoli dell’utente
        IList<string> ruoli = await _gestioneUtenti.GetRolesAsync(utente);

        // Genera token JWT
        string token = _jwtHelper.GenerateToken(utente, ruoli);

        // Costruisce risposta di autenticazione
        DtoAuthResponse response = new DtoAuthResponse
        {
            Token = token,
            Id = utente.Id,
            NomeCompleto = utente.NomeCompleto,
            Email = utente.Email ?? string.Empty,
            Ruolo = ruoli.Count > 0 ? ruoli[0] : ""
        };

        return response;
    }

    // ---------------------------------------------------------
    // OTTIENI UTENTE TRAMITE ID
    // ---------------------------------------------------------
    /// <summary>
    /// Restituisce i dati principali dell’utente tramite ID.
    /// </summary>
    public async Task<DtoUtente?> OttieniTramiteIdAsync(string id)
    {
        Utente? utente = await _gestioneUtenti.FindByIdAsync(id);

        if (utente == null)
        {
            return null;
        }

        return new DtoUtente
        {
            Id = utente.Id,
            Email = utente.Email ?? string.Empty,
            NomeCompleto = utente.NomeCompleto ?? string.Empty,
            Eta = utente.Eta
        };
    }

    // ---------------------------------------------------------
    // MODIFICA DATI UTENTE
    // ---------------------------------------------------------
    /// <summary>
    /// Modifica i dati principali dell’utente (nome completo, età).
    /// </summary>
    public async Task<IdentityResult> ModificaAsync(DtoCreazioneUtente dto, string idUtente)
    {
        Utente? utente = await _gestioneUtenti.FindByIdAsync(idUtente);

        if (utente == null)
        {
            IdentityError error = new IdentityError();
            return IdentityResult.Failed(error);
        }

        utente.NomeCompleto = dto.NomeCompleto;
        utente.Eta = dto.Eta;

        // Aggiorna utente tramite Identity
        IdentityResult result = await _gestioneUtenti.UpdateAsync(utente);

        return result;
    }

    // ---------------------------------------------------------
    // ELIMINA UTENTE
    // ---------------------------------------------------------
    /// <summary>
    /// Elimina un utente dal sistema tramite ID.
    /// </summary>
    public async Task<IdentityResult> EliminaAsync(string userId)
    {
        Utente? utente = await _gestioneUtenti.FindByIdAsync(userId);

        if (utente == null)
        {
            IdentityError errore = new IdentityError();
            return IdentityResult.Failed(errore);
        }

        // Eliminazione tramite Identity
        IdentityResult risultato = await _gestioneUtenti.DeleteAsync(utente);

        return risultato;
    }
}
```

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
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using NuovoCinemaParadiso.Data;
using NuovoCinemaParadiso.Dtos;
using NuovoCinemaParadiso.Models;

namespace NuovoCinemaParadiso.Services;

/// <summary>
/// Service dedicato alla gestione delle operazioni sugli utenti
/// che non riguardano autenticazione o ruoli (gestiti da AuthService).
/// Include assegnazione abbonamenti e gift card.
/// </summary>
public class UtenteService
{
    private readonly ContestoDb _contesto;
    private readonly UserManager<Utente> _gestioneUtenti;

    public UtenteService(ContestoDb contestoDb, UserManager<Utente> gestioneUtenti)
    {
        _contesto = contestoDb;
        _gestioneUtenti = gestioneUtenti;
    }

    // ---------------------------------------------------------
    // ASSEGNAZIONE ABBONAMENTO A UN UTENTE
    // ---------------------------------------------------------
    /// <summary>
    /// Assegna un abbonamento a un utente.
    /// Imposta la data di inizio e abilita il flag SeAbbonato.
    /// </summary>
    public async Task<DtoUtente> AbbonatiAsync(string abbonamentoId, string utenteId)
    {
        // Recupera tutti gli abbonamenti dal database
        List<Abbonamento> abbonamenti = await _contesto.Abbonamenti.ToListAsync();
        Abbonamento? abbonamentoTrovato = null;

        // Ricerca manuale dell'abbonamento tramite ID
        for (int i = 0; i < abbonamenti.Count; i++)
        {
            Abbonamento abbonamentoCorrente = abbonamenti[i];

            if (abbonamentoCorrente.Id == abbonamentoId)
            {
                abbonamentoTrovato = abbonamentoCorrente;
                break;
            }
        }

        // Se non trovato → ritorna null
        if (abbonamentoTrovato == null)
        {
            return null;
        }

        // Recupera l’utente tramite Identity
        Utente? utenteTrovato = await _gestioneUtenti.FindByIdAsync(utenteId);

        if (utenteTrovato == null)
        {
            return null;
        }

        // Assegna abbonamento all’utente
        utenteTrovato.AbbonamentoId = abbonamentoTrovato.Id;
        utenteTrovato.SeAbbonato = true;
        utenteTrovato.DataInizioAbbonamento = DateTimeOffset.UtcNow;

        // Salva modifiche nel database
        await _contesto.SaveChangesAsync();

        // Restituisce DTO aggiornato
        return new DtoUtente()
        {
            Id = utenteTrovato.Id,
            NomeCompleto = utenteTrovato.NomeCompleto,
            Email = utenteTrovato.Email,
            Eta = utenteTrovato.Eta,
            SeAbbonato = utenteTrovato.SeAbbonato,
            DataInizio = utenteTrovato.DataInizioAbbonamento,
            AbbonamentoId = utenteTrovato.AbbonamentoId,
            TipoAbbonamento = abbonamentoTrovato.Nome
        };
    }

    // ---------------------------------------------------------
    // ASSEGNAZIONE GIFT CARD A UN UTENTE
    // ---------------------------------------------------------
    /// <summary>
    /// Assegna una gift card a un utente.
    /// Imposta la data di attivazione e abilita il flag PossiedeGiftCard.
    /// </summary>
    public async Task<DtoUtente> GiftCardAsync(string giftCardId, string utenteId)
    {
        // Recupera tutte le gift card dal database
        List<GiftCard> giftCards = await _contesto.GiftCards.ToListAsync();
        GiftCard? giftCardTrovata = null;

        // Ricerca manuale della gift card tramite ID
        for (int i = 0; i < giftCards.Count; i++)
        {
            GiftCard giftCardCorrente = giftCards[i];

            if (giftCardCorrente.Id == giftCardId)
            {
                giftCardTrovata = giftCardCorrente;
                break;
            }
        }

        // Se non trovata → ritorna null
        if (giftCardTrovata == null)
        {
            return null;
        }

        // Recupera l’utente tramite Identity
        Utente? utenteTrovato = await _gestioneUtenti.FindByIdAsync(utenteId);

        if (utenteTrovato == null)
        {
            return null;
        }

        // Assegna gift card all’utente
        utenteTrovato.GiftCardId = giftCardTrovata.Id;
        utenteTrovato.PossiedeGiftCard = true;
        utenteTrovato.DataInizioGiftCard = DateTimeOffset.UtcNow;

        // Salva modifiche nel database
        await _contesto.SaveChangesAsync();

        // Restituisce DTO aggiornato
        return new DtoUtente()
        {
            Id = utenteTrovato.Id,
            NomeCompleto = utenteTrovato.NomeCompleto,
            Email = utenteTrovato.Email,
            Eta = utenteTrovato.Eta,
            PossiedeGiftCard = utenteTrovato.PossiedeGiftCard,
            DataInizio = utenteTrovato.DataInizioGiftCard,
            GiftCardId = utenteTrovato.GiftCardId,
            TipoGiftCard = giftCardTrovata.Nome
        };
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
using Microsoft.EntityFrameworkCore;
using NuovoCinemaParadiso.Data;
using NuovoCinemaParadiso.Dtos;
using NuovoCinemaParadiso.Models;
using NuovoCinemaParadiso.Helpers;

namespace NuovoCinemaParadiso.Services;

/// <summary>
/// Service responsabile della gestione degli acquisti.
/// Include operazioni CRUD e calcolo del prezzo finale.
/// </summary>
public class AcquistoService
{
    private readonly ContestoDb _contesto;

    public AcquistoService(ContestoDb contesto)
    {
        _contesto = contesto;
    }

    // ---------------------------------------------------------
    // OTTIENI TUTTI GLI ACQUISTI DI UN UTENTE
    // ---------------------------------------------------------
    /// <summary>
    /// Restituisce tutti gli acquisti effettuati da un utente specifico.
    /// </summary>
    public async Task<List<DtoAcquisto>> OttieniTutto(string utenteId)
    {
        // Recupera tutti gli acquisti
        List<Acquisto> acquisti = await _contesto.Acquisti.ToListAsync();

        List<DtoAcquisto> risultato = new List<DtoAcquisto>();

        // Ciclo manuale per mappare ogni acquisto
        for (int i = 0; i < acquisti.Count; i++)
        {
            Acquisto acquistoCorrente = acquisti[i];

            // Recupera entità correlate
            Utente? utente = await _contesto.Utenti.FindAsync(acquistoCorrente.UtenteId);
            Proiezione proiezione = await _contesto.Proiezioni.FindAsync(acquistoCorrente.ProiezioneId);
            Movie movie = await _contesto.Movies.FindAsync(proiezione.MovieId);
            Sala sala = await _contesto.Sale.FindAsync(proiezione.SalaId);
            TipologiaSala tipologiaSala = await _contesto.TipologieSala.FindAsync(sala.TipologiaSalaId);

            // Filtra solo gli acquisti dell’utente richiesto
            if (acquistoCorrente.UtenteId == utenteId)
            {
                DtoAcquisto dto = new DtoAcquisto();
                dto.Id = acquistoCorrente.Id;
                dto.UtenteId = utente.Id;
                dto.ProiezioneId = acquistoCorrente.ProiezioneId;
                dto.OrarioCreazione = acquistoCorrente.OrarioCreazione;
                dto.NumeroBiglietti = acquistoCorrente.NumeroBiglietti;
                dto.MetodoPagamento = acquistoCorrente.MetodoPagamento;

                // Calcolo del prezzo finale lato server
                dto.PrezzoFinale = Calcoli.CalcolaPrezzoFinale(
                    movie.PrezzoMovie,
                    tipologiaSala.MaggiorazionePrezzo,
                    acquistoCorrente.NumeroBiglietti,
                    utente,
                    acquistoCorrente.MetodoPagamento);

                risultato.Add(dto);
            }
        }

        return risultato;
    }

    // ---------------------------------------------------------
    // OTTIENI ACQUISTO TRAMITE ID
    // ---------------------------------------------------------
    /// <summary>
    /// Restituisce un singolo acquisto se appartiene all’utente.
    /// </summary>
    public async Task<DtoAcquisto> OttieniTramiteIdAsync(string id, string utenteId)
    {
        Acquisto? acquisto = await _contesto.Acquisti.FindAsync(id);

        if (acquisto == null)
            return null;

        // L’utente può vedere solo i propri acquisti
        if (acquisto.UtenteId != utenteId)
            return null;

        // Recupera entità correlate
        Utente? utente = await _contesto.Utenti.FindAsync(acquisto.UtenteId);
        Proiezione proiezione = await _contesto.Proiezioni.FindAsync(acquisto.ProiezioneId);
        Movie movie = await _contesto.Movies.FindAsync(proiezione.MovieId);
        Sala sala = await _contesto.Sale.FindAsync(proiezione.SalaId);
        TipologiaSala tipologiaSala = await _contesto.TipologieSala.FindAsync(sala.TipologiaSalaId);

        // Mappa DTO
        DtoAcquisto dto = new DtoAcquisto();
        dto.Id = acquisto.Id;
        dto.UtenteId = utente.Id;
        dto.ProiezioneId = acquisto.ProiezioneId;
        dto.OrarioCreazione = acquisto.OrarioCreazione;
        dto.NumeroBiglietti = acquisto.NumeroBiglietti;
        dto.MetodoPagamento = acquisto.MetodoPagamento;

        dto.PrezzoFinale = Calcoli.CalcolaPrezzoFinale(
            movie.PrezzoMovie,
            tipologiaSala.MaggiorazionePrezzo,
            acquisto.NumeroBiglietti,
            utente,
            acquisto.MetodoPagamento);

        return dto;
    }

    // ---------------------------------------------------------
    // CREAZIONE ACQUISTO
    // ---------------------------------------------------------
    /// <summary>
    /// Crea un nuovo acquisto e calcola il prezzo finale lato server.
    /// </summary>
    public async Task<DtoAcquisto> CreazioneAsync(DtoCreazioneAcquisto dto, string utenteId)
    {
        // Recupera entità necessarie
        Utente utente = await _contesto.Utenti.FindAsync(utenteId);
        Proiezione proiezione = await _contesto.Proiezioni.FindAsync(dto.ProiezioneId);
        Movie movie = await _contesto.Movies.FindAsync(proiezione.MovieId);
        Sala sala = await _contesto.Sale.FindAsync(proiezione.SalaId);
        TipologiaSala tipologiaSala = await _contesto.TipologieSala.FindAsync(sala.TipologiaSalaId);

        // Crea l’acquisto
        Acquisto acquisto = new Acquisto();
        acquisto.UtenteId = utenteId;
        acquisto.ProiezioneId = proiezione.Id;
        acquisto.NumeroBiglietti = dto.NumeroBiglietti;
        acquisto.OrarioCreazione = DateTimeOffset.UtcNow;
        acquisto.MetodoPagamento = dto.MetodoPagamento;

        // Calcolo del prezzo finale
        acquisto.PrezzoFinale = Calcoli.CalcolaPrezzoFinale(
            movie.PrezzoMovie,
            tipologiaSala.MaggiorazionePrezzo,
            dto.NumeroBiglietti,
            utente,
            dto.MetodoPagamento);

        // Salva nel DB
        _contesto.Acquisti.Add(acquisto);
        await _contesto.SaveChangesAsync();

        // Restituisce DTO
        return new DtoAcquisto
        {
            Id = acquisto.Id,
            ProiezioneId = acquisto.ProiezioneId,
            UtenteId = acquisto.UtenteId,
            NumeroBiglietti = acquisto.NumeroBiglietti,
            PrezzoFinale = acquisto.PrezzoFinale,
            OrarioCreazione = acquisto.OrarioCreazione,
            MetodoPagamento = acquisto.MetodoPagamento
        };
    }

    // ---------------------------------------------------------
    // MODIFICA ACQUISTO
    // ---------------------------------------------------------
    /// <summary>
    /// Modifica un acquisto esistente e ricalcola il prezzo finale.
    /// </summary>
    public async Task<DtoAcquisto?> ModificaAsync(string id, DtoCreazioneAcquisto dto)
    {
        Acquisto? acquistoEsistente = await _contesto.Acquisti.FindAsync(id);

        if (acquistoEsistente == null)
            return null;

        // Aggiorna numero biglietti
        acquistoEsistente.NumeroBiglietti = dto.NumeroBiglietti;

        // Recupera entità correlate
        Proiezione proiezione = await _contesto.Proiezioni.FindAsync(dto.ProiezioneId);
        Movie? movie = await _contesto.Movies.FindAsync(proiezione.MovieId);
        Sala? sala = await _contesto.Sale.FindAsync(proiezione.SalaId);
        Utente? utente = await _contesto.Utenti.FindAsync(acquistoEsistente.UtenteId);
        TipologiaSala? tipologiaSala = await _contesto.TipologieSala.FindAsync(sala.TipologiaSalaId);

        // Ricalcola prezzo
        acquistoEsistente.PrezzoFinale = Calcoli.CalcolaPrezzoFinale(
            movie.PrezzoMovie,
            tipologiaSala.MaggiorazionePrezzo,
            acquistoEsistente.NumeroBiglietti,
            utente,
            acquistoEsistente.MetodoPagamento);

        await _contesto.SaveChangesAsync();

        // Restituisce DTO aggiornato
        return new DtoAcquisto
        {
            Id = acquistoEsistente.Id,
            UtenteId = acquistoEsistente.UtenteId,
            ProiezioneId = acquistoEsistente.ProiezioneId,
            NumeroBiglietti = acquistoEsistente.NumeroBiglietti,
            PrezzoFinale = acquistoEsistente.PrezzoFinale,
            OrarioCreazione = acquistoEsistente.OrarioCreazione,
            MetodoPagamento = acquistoEsistente.MetodoPagamento
        };
    }

    // ---------------------------------------------------------
    // ELIMINA ACQUISTO
    // ---------------------------------------------------------
    /// <summary>
    /// Elimina un acquisto tramite ID.
    /// </summary>
    public async Task<bool> EliminazioneAsync(string id)
    {
        Acquisto? acquisto = await _contesto.Acquisti.FindAsync(id);

        if (acquisto == null)
            return false;

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

## LogAzioniService

```c#
using NuovoCinemaParadiso.Data;
using NuovoCinemaParadiso.Dtos;
using NuovoCinemaParadiso.Models;
using Microsoft.EntityFrameworkCore;


namespace NuovoCinemaParadiso.Services;

public class LogAzioniService
{
  private readonly ContestoDb _contesto;
  public LogAzioniService(ContestoDb contesto) 
  {
    _contesto = contesto; 
  }

    public async Task SalvataggioLogAzioneAsync(string idUtente, string azione, bool effettuato)
  {
      string messaggio = "operazione fallita";
      if(effettuato) messaggio = "operazione eseguita";

      LogAzioni log = new LogAzioni();

      log.IdUtente = idUtente;
      log.NomeAzione = azione;
      log.Effettuato = effettuato;
      log.Messaggio = messaggio;
      log.TimeStamp = DateTimeOffset.UtcNow;

        _contesto.LogAzioni.Add(log);
      await _contesto.SaveChangesAsync();
  }

    public async Task<List<DtoLogAzioni>> LetturaLogAzioneAsync()
    {
        List<LogAzioni> logs= await _contesto.LogAzioni.ToListAsync();
        List<DtoLogAzioni> risultati = new List<DtoLogAzioni>();
        foreach (LogAzioni log in logs)
        {
          DtoLogAzioni risultato = new DtoLogAzioni();
          risultato.Id = log.Id;
          risultato.IdUtente = log.IdUtente;
          risultato.NomeAzione = log.NomeAzione;
          risultato.Effettuato = log.Effettuato;
          risultato.Messaggio = log.Messaggio;
          risultato.TimeStamp = log.TimeStamp;
          risultati.Add(risultato);
        }


        return risultati;
    }

}
```

# Helpers

## CalcoliHelper.cs

```c#
using NuovoCinemaParadiso.Models;

namespace NuovoCinemaParadiso.Helpers;

/// <summary>
/// Classe statica che contiene funzioni di utilità per calcoli
/// relativi a prezzi, scadenze e gestione temporale.
/// </summary>
public static class Calcoli
{
    /// <summary>
    /// Calcola il prezzo finale dei biglietti in base al metodo di pagamento scelto.
    /// Supporta tre modalità:
    /// - "abbonamento": applica lo sconto previsto dal tipo di abbonamento
    /// - "giftcard": scala i film disponibili e calcola eventuali biglietti rimanenti
    /// - default: prezzo pieno
    /// 
    /// Aggiorna automaticamente lo stato della gift card (numero film rimanenti).
    /// </summary>
    public static Decimal CalcolaPrezzoFinale(
        decimal prezzoMovie,
        decimal maggiorazione,
        int numeroBiglietti,
        Utente utente,
        string metodoPagamento)
    {
        // Prezzo base del singolo biglietto (film + eventuale maggiorazione sala)
        decimal prezzoBiglietto = prezzoMovie + maggiorazione;

        // ---------------------------------------------------------
        // PAGAMENTO CON ABBONAMENTO
        // ---------------------------------------------------------
        if (metodoPagamento == "abbonamento" && utente.SeAbbonato)
        {
            // Calcolo dello sconto percentuale
            decimal sconto = (prezzoBiglietto / 100) * utente.Abbonamento.Sconto;

            // Prezzo del biglietto dopo lo sconto
            decimal prezzoScontato = prezzoBiglietto - sconto;

            // Prezzo finale per il numero di biglietti richiesti
            return prezzoScontato * numeroBiglietti;
        }

        // ---------------------------------------------------------
        // PAGAMENTO CON GIFT CARD
        // ---------------------------------------------------------
        if (metodoPagamento == "giftcard" && utente.PossiedeGiftCard)
        {
            // Caso 1: la gift card copre più biglietti di quelli richiesti
            if (utente.GiftCard.NumeroMovie > numeroBiglietti)
            {
                utente.GiftCard.NumeroMovie -= numeroBiglietti;
                return 0;
            }

            // Caso 2: la gift card copre esattamente i biglietti richiesti
            else if (utente.GiftCard.NumeroMovie == numeroBiglietti)
            {
                utente.GiftCard.NumeroMovie = 0;
                utente.PossiedeGiftCard = false;
                return 0;
            }

            // Caso 3: la gift card copre solo una parte dei biglietti
            else
            {
                int bigliettiRimanenti = numeroBiglietti - utente.GiftCard.NumeroMovie;

                // La gift card viene completamente consumata
                utente.GiftCard.NumeroMovie = 0;
                utente.PossiedeGiftCard = false;

                // Si paga solo per i biglietti non coperti
                return prezzoBiglietto * bigliettiRimanenti;
            }
        }

        // ---------------------------------------------------------
        // PAGAMENTO STANDARD (prezzo pieno)
        // ---------------------------------------------------------
        return prezzoBiglietto * numeroBiglietti;
    }

    /// <summary>
    /// Calcola la data di scadenza aggiungendo un numero di mesi
    /// alla data di inizio (usato per abbonamenti e gift card).
    /// </summary>
    public static DateTimeOffset? CalcolaScadenza(DateTimeOffset dataInizio, int durata)
    {
        return dataInizio.AddMonths(durata);
    }

    /// <summary>
    /// Restituisce il numero di giorni rimanenti alla scadenza.
    /// Se il valore è 0, significa che l'abbonamento/gift card è scaduto.
    /// </summary>
    public static int GiorniAllaScadenza(DateTimeOffset dataInizio, int durata)
    {
        DateTimeOffset dataScadenza = dataInizio.AddMonths(durata);

        // Differenza tra la data di scadenza e la data attuale
        TimeSpan differenza = dataScadenza - DateTime.Now;

        return (int)differenza.TotalDays;
    }
}
```

# Seed

## DataSeeder.cs

```c#
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NuovoCinemaParadiso.Data;
using NuovoCinemaParadiso.Models;

namespace NuovoCinemaParadiso.Seed;

/// <summary>
/// Classe responsabile del popolamento iniziale del database.
/// Crea ruoli, utenti, generi, tipologie sala, turni, abbonamenti e gift card.
/// Viene eseguita all'avvio dell'applicazione.
/// </summary>
public static class DataSeeder
{
    /// <summary>
    /// Metodo principale che avvia tutte le operazioni di seed.
    /// </summary>
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        using IServiceScope scope = serviceProvider.CreateScope();

        DateTime oggi = DateTime.Today;

        // Recupero dei servizi necessari
        ContestoDb contestoDb = scope.ServiceProvider.GetRequiredService<ContestoDb>();
        UserManager<Utente> gestioneUtenti = scope.ServiceProvider.GetRequiredService<UserManager<Utente>>();
        RoleManager<IdentityRole> gestioneRuoli = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        // ---------------------------------------------------------
        // SEED RUOLI
        // ---------------------------------------------------------
        await AssicuraEsistenzaRuoloAsync(gestioneRuoli, Ruoli.Gestore);
        await AssicuraEsistenzaRuoloAsync(gestioneRuoli, Ruoli.Operatore);
        await AssicuraEsistenzaRuoloAsync(gestioneRuoli, Ruoli.Utente);

        // ---------------------------------------------------------
        // SEED UTENTI
        // ---------------------------------------------------------
        Utente gestore = await AssicuraEsistenzaUtenteAsync(
            gestioneUtenti,
            "gestore@gmail.com",
            "123456",
            "Gestore",
            60,
            false);

        Utente operatore = await AssicuraEsistenzaUtenteAsync(
            gestioneUtenti,
            "operatore@gmail.com",
            "123456",
            "Operatore",
            35,
            false);

        Utente utente = await AssicuraEsistenzaUtenteAsync(
            gestioneUtenti,
            "utente1@gmail.com",
            "123456",
            "Utente Uno",
            15,
            false);

        // Assegna ruolo unico a ciascun utente
        await ImpostaRuoloUnicoAsync(gestioneUtenti, gestore, Ruoli.Gestore);
        await ImpostaRuoloUnicoAsync(gestioneUtenti, operatore, Ruoli.Operatore);
        await ImpostaRuoloUnicoAsync(gestioneUtenti, utente, Ruoli.Utente);

        // ---------------------------------------------------------
        // SEED GENERI MOVIE
        // ---------------------------------------------------------
        await AssicuraEsistenzaGenereMovie(contestoDb, "Azione");
        await AssicuraEsistenzaGenereMovie(contestoDb, "Horror");
        await AssicuraEsistenzaGenereMovie(contestoDb, "Commedia");

        // ---------------------------------------------------------
        // SEED TIPOLOGIE SALA
        // ---------------------------------------------------------
        await AssicuraEsistenzaTipologiaSala(contestoDb, "2D", 2);
        await AssicuraEsistenzaTipologiaSala(contestoDb, "3D", 3);
        await AssicuraEsistenzaTipologiaSala(contestoDb, "IMAX", 4);

        // ---------------------------------------------------------
        // SEED TURNI
        // ---------------------------------------------------------
        await AssicuraEsistenzaTurno(contestoDb, new TimeOnly(10, 0, 0), new TimeOnly(13, 0, 0), "Mattina");
        await AssicuraEsistenzaTurno(contestoDb, new TimeOnly(13, 0, 0), new TimeOnly(18, 0, 0), "Pomeriggio");
        await AssicuraEsistenzaTurno(contestoDb, new TimeOnly(18, 0, 0), new TimeOnly(22, 0, 0), "Sera");

        // ---------------------------------------------------------
        // SEED ABBONAMENTI
        // ---------------------------------------------------------
        await AssicuraEsistenzaAbbonamento(contestoDb, "Mensile", 70, 25, 1);
        await AssicuraEsistenzaAbbonamento(contestoDb, "Semestrale", 210, 50, 6);
        await AssicuraEsistenzaAbbonamento(contestoDb, "Annuale", 300, 75, 12);

        // ---------------------------------------------------------
        // SEED GIFT CARD
        // ---------------------------------------------------------
        await AssicuraEsistenzaGiftCard(contestoDb, "10 Film", 85, 10, 12);
        await AssicuraEsistenzaGiftCard(contestoDb, "25 Film", 190, 25, 12);
        await AssicuraEsistenzaGiftCard(contestoDb, "50 Film", 325, 50, 12);
    }

    // ---------------------------------------------------------
    // METODI DI SUPPORTO
    // ---------------------------------------------------------

    /// <summary>
    /// Crea un ruolo se non esiste già.
    /// </summary>
    private static async Task AssicuraEsistenzaRuoloAsync(RoleManager<IdentityRole> managerRuolo, string nomeRuolo)
    {
        bool seEsiste = await managerRuolo.RoleExistsAsync(nomeRuolo);
        if (!seEsiste)
        {
            IdentityRole ruolo = new IdentityRole { Name = nomeRuolo };
            await managerRuolo.CreateAsync(ruolo);
        }
    }

    /// <summary>
    /// Crea un utente se non esiste già.
    /// </summary>
    private static async Task<Utente> AssicuraEsistenzaUtenteAsync(
        UserManager<Utente> gestioneUtenti,
        string email,
        string password,
        string nomeCompleto,
        int eta,
        bool abbonato)
    {
        Utente? utenteEsistente = await gestioneUtenti.FindByEmailAsync(email);

        if (utenteEsistente != null)
            return utenteEsistente;

        Utente utente = new Utente
        {
            UserName = email,
            Email = email,
            NomeCompleto = nomeCompleto,
            Eta = eta,
            SeAbbonato = abbonato,
            AbbonamentoId = null
        };

        IdentityResult risultato = await gestioneUtenti.CreateAsync(utente, password);

        if (!risultato.Succeeded)
        {
            List<string> errori = risultato.Errors.Select(e => e.Description).ToList();
            string messaggio = string.Join("|", errori);
            throw new Exception($"Errore durante il seed dell'utente {email} : {messaggio}");
        }

        return utente;
    }

    /// <summary>
    /// Rimuove eventuali ruoli precedenti e assegna un ruolo unico all'utente.
    /// </summary>
    private static async Task ImpostaRuoloUnicoAsync(UserManager<Utente> gestioneUtenti, Utente utente, string ruoloTarget)
    {
        IList<string> ruoliCorrenti = await gestioneUtenti.GetRolesAsync(utente);

        // Rimuove tutti i ruoli validi
        foreach (string ruoloCorrente in ruoliCorrenti)
        {
            if (ruoloCorrente == Ruoli.Gestore || ruoloCorrente == Ruoli.Operatore || ruoloCorrente == Ruoli.Utente)
            {
                await gestioneUtenti.RemoveFromRoleAsync(utente, ruoloCorrente);
            }
        }

        // Assegna il ruolo target se non già presente
        if (!await gestioneUtenti.IsInRoleAsync(utente, ruoloTarget))
        {
            await gestioneUtenti.AddToRoleAsync(utente, ruoloTarget);
        }
    }

    /// <summary>
    /// Crea un genere movie se non esiste già.
    /// </summary>
    private static async Task AssicuraEsistenzaGenereMovie(ContestoDb context, string genere)
    {
        if (await context.GeneriMovies.AnyAsync(g => g.Genere.ToLower() == genere.ToLower()))
            return;

        context.GeneriMovies.Add(new GenereMovie { Genere = genere });
        await context.SaveChangesAsync();
    }

    /// <summary>
    /// Crea una tipologia sala se non esiste già.
    /// </summary>
    private static async Task AssicuraEsistenzaTipologiaSala(ContestoDb context, string nome, decimal maggiorazioneprezzo)
    {
        if (await context.TipologieSala.AnyAsync(t => t.Nome.ToLower() == nome.ToLower()))
            return;

        context.TipologieSala.Add(new TipologiaSala
        {
            Nome = nome,
            MaggiorazionePrezzo = maggiorazioneprezzo
        });

        await context.SaveChangesAsync();
    }

    /// <summary>
    /// Crea un turno se non esiste già.
    /// </summary>
    private static async Task AssicuraEsistenzaTurno(ContestoDb context, TimeOnly oraInizio, TimeOnly oraFine, string nome)
    {
        if (await context.Turni.AnyAsync(t =>
            t.Nome.ToLower() == nome.ToLower() ||
            (t.OraInizio == oraInizio && t.OraFine == oraFine)))
            return;

        context.Turni.Add(new Turno
        {
            Nome = nome,
            OraInizio = oraInizio,
            OraFine = oraFine
        });

        await context.SaveChangesAsync();
    }

    /// <summary>
    /// Crea un abbonamento se non esiste già.
    /// </summary>
    private static async Task AssicuraEsistenzaAbbonamento(ContestoDb context, string nome, decimal prezzo, int sconto, int durata)
    {
        if (await context.Abbonamenti.AnyAsync(a => a.Nome.ToLower() == nome.ToLower()))
            return;

        context.Abbonamenti.Add(new Abbonamento
        {
            Nome = nome,
            Prezzo = prezzo,
            Sconto = sconto,
            Durata = durata
        });

        await context.SaveChangesAsync();
    }

    /// <summary>
    /// Crea una gift card se non esiste già.
    /// </summary>
    private static async Task AssicuraEsistenzaGiftCard(ContestoDb context, string nome, decimal prezzo, int numeroMovie, int durata)
    {
        if (await context.GiftCards.AnyAsync(g => g.Nome.ToLower() == nome.ToLower()))
            return;

        context.GiftCards.Add(new GiftCard
        {
            Nome = nome,
            Prezzo = prezzo,
            NumeroMovie = numeroMovie,
            Durata = durata
        });

        await context.SaveChangesAsync();
    }
}
```

# Program.cs

```c#
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NuovoCinemaParadiso.Data;
using NuovoCinemaParadiso.Helpers;
using NuovoCinemaParadiso.Models;
using NuovoCinemaParadiso.Services;
using NuovoCinemaParadiso.Seed;

var builder = WebApplication.CreateBuilder(args);

// ---------------------------------------------------------
// CONFIGURAZIONE CONTROLLERS
// ---------------------------------------------------------
builder.Services.AddControllers();

// ---------------------------------------------------------
// CONFIGURAZIONE DATABASE (SQLite)
// ---------------------------------------------------------
builder.Services.AddDbContext<ContestoDb>(options =>
{
    // Usa la connection string definita in appsettings.json
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// ---------------------------------------------------------
// CONFIGURAZIONE IDENTITY (gestione utenti e ruoli)
// ---------------------------------------------------------
builder.Services.AddIdentityCore<Utente>(options =>
{
    // Requisiti password semplificati
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;
})
.AddRoles<IdentityRole>()                      // Abilita i ruoli
.AddSignInManager<SignInManager<Utente>>()      // Gestione login
.AddEntityFrameworkStores<ContestoDb>()         // Usa EF Core come store
.AddDefaultTokenProviders();                    // Necessario per reset password, ecc.

// ---------------------------------------------------------
// CONFIGURAZIONE JWT
// ---------------------------------------------------------
string? jwtKey = builder.Configuration["Jwt:Key"];
string? jwtIssuer = builder.Configuration["Jwt:Issuer"];
string? jwtAudience = builder.Configuration["Jwt:Audience"];

// Controllo che i parametri JWT siano presenti
if (string.IsNullOrWhiteSpace(jwtKey) ||
    string.IsNullOrWhiteSpace(jwtIssuer) ||
    string.IsNullOrWhiteSpace(jwtAudience))
{
    throw new Exception("Configurazione JWT mancante in appsettings.json");
}

// Abilita autenticazione tramite JWT Bearer
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        // Parametri di validazione del token
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,

            // Chiave segreta per validare la firma del token
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

// Abilita autorizzazione (necessaria per [Authorize])
builder.Services.AddAuthorization();

// ---------------------------------------------------------
// CONFIGURAZIONE CORS (per Angular in locale)
// ---------------------------------------------------------
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp", policy =>
    {
        policy.WithOrigins("http://localhost:4200") // Dominio Angular
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// ---------------------------------------------------------
// REGISTRAZIONE DEI SERVIZI (Dependency Injection)
// ---------------------------------------------------------
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<GenereMovieService>();
builder.Services.AddScoped<TipologiaSalaService>();
builder.Services.AddScoped<TurnoService>();
builder.Services.AddScoped<SalaService>();
builder.Services.AddScoped<MovieService>();
builder.Services.AddScoped<AcquistoService>();
builder.Services.AddScoped<RuoloUtenteService>(); // Gestione ruoli utenti
builder.Services.AddScoped<JwtHelper>();
builder.Services.AddScoped<LogAzioniService>();
builder.Services.AddScoped<UtenteService>();
builder.Services.AddScoped<AdminService>();
builder.Services.AddScoped<AbbonamentoService>();
builder.Services.AddScoped<ProiezioneService>();
builder.Services.AddScoped<GiftCardService>();

var app = builder.Build();

// ---------------------------------------------------------
// MIDDLEWARE PIPELINE
// ---------------------------------------------------------

// Abilita CORS per Angular
app.UseCors("AllowAngularApp");

// Redirect automatico a HTTPS
app.UseHttpsRedirection();

// Abilita autenticazione e autorizzazione
app.UseAuthentication();
app.UseAuthorization();

// Mappa i controller
app.MapControllers();

// ---------------------------------------------------------
// MIGRAZIONI AUTOMATICHE ALLO START
// ---------------------------------------------------------
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ContestoDb>();
    db.Database.Migrate(); // Applica automaticamente le migrazioni
}

// ---------------------------------------------------------
// SEED DATI INIZIALI (ruoli, utenti, interessi, ecc.)
// ---------------------------------------------------------
await DataSeeder.SeedAsync(app.Services);

// Avvia l'applicazione
app.Run();
```

# Controller
## AuthController.cs (leggere modifiche su logAzioni)
```c#
// Importa le funzionalità base per creare controller API MVC
using Microsoft.AspNetCore.Mvc;
// Importa le funzionalità per lavorare con i claim dell'utente loggato
using System.Security.Claims;
// Importa IdentityResult e altri tipi legati all'identità
using Microsoft.AspNetCore.Identity;
// Importa il servizio di autenticazione personalizzato
using NuovoCinemaParadiso.Services;
// Importa i DTO usati per scambiare dati con il client
using NuovoCinemaParadiso.Dtos;
// Importa gli attributi per la gestione dell'autorizzazione
using Microsoft.AspNetCore.Authorization;

// Definisce il namespace del progetto per i controller
namespace NuovoCinemaParadiso.Controllers;

// Indica che questa classe è un controller API
[ApiController]
// Definisce la route base: /api/Auth
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    // Campo per il servizio di autenticazione
    private readonly AuthService _authService;
    // Campo per il servizio di log delle azioni
    private readonly LogAzioniService _logAzioniService;

    // Costruttore che riceve i servizi tramite dependency injection
    public AuthController(AuthService authService, LogAzioniService logAzioniService)
    {
        // Assegna il servizio di autenticazione al campo privato
        _authService = authService;
        // Assegna il servizio di log al campo privato
        _logAzioniService = logAzioniService;
    }

    // Espone un endpoint POST su /api/Auth/registrazione
    [HttpPost("registrazione")]
    // Metodo asincrono per registrare un nuovo utente
    public async Task<IActionResult> Registrazione(DtoRegistrazione dto)
    {
        // Chiama il servizio di autenticazione per eseguire la registrazione
        IdentityResult result = await _authService.RegistrazioneAsync(dto);
        // Non c'è ancora un utente loggato, quindi l'Id è nullo
        string? utenteId = null;

        // Se la registrazione non è andata a buon fine
        if (!result.Succeeded)
        {
            // Salva un log di registrazione fallita
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Registrazione utente", false);
            // Restituisce BadRequest con gli errori di Identity
            return BadRequest(result.Errors);
        }

        // Salva un log di registrazione riuscita
        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Registrazione utente", true);
        // Restituisce 200 OK con un messaggio di conferma
        return Ok(new { messaggio = "Registrazione avvenuta con successo!" });
    }

    // Espone un endpoint POST su /api/Auth/login
    [HttpPost("login")]
    // Metodo asincrono per eseguire il login
    public async Task<IActionResult> Login([FromBody] DtoLogin dto)
    {
        // Chiama il servizio di autenticazione per validare le credenziali e generare il token
        DtoAuthResponse? risposta = await _authService.LoginAsync(dto);

        // Se le credenziali non sono valide o il login fallisce
        if (risposta == null)
        {
            // Salva un log di login fallito (senza Id utente perché non noto)
            await _logAzioniService.SalvataggioLogAzioneAsync(null, "Login", false);
            // Restituisce 401 Unauthorized con messaggio di errore
            return Unauthorized(new { messaggio = "Email o password non validi." });
        }

        // Salva un log di login riuscito usando l'Id utente restituito dal servizio
        await _logAzioniService.SalvataggioLogAzioneAsync(risposta.Id, "Login", true);
        // Restituisce 200 OK con il DTO di risposta (token, email, ruolo, ecc.)
        return Ok(risposta);
    }

    // Espone un endpoint GET su /api/Auth/profilo
    [HttpGet("profilo")]
    // Metodo asincrono per ottenere il profilo dell'utente loggato
    public async Task<IActionResult> RicercaProfiloLoggato()
    {
        // Recupera l'Id utente dai claim del token JWT
        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        // Chiede al servizio di autenticazione i dati dell'utente tramite Id
        DtoUtente? utente = await _authService.OttieniTramiteIdAsync(utenteId);

        // Se l'utente non esiste nel database
        if (utente == null)
        {
            // Salva un log di ricerca profilo fallita
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ricerca profilo loggato", false);
            // Restituisce 404 NotFound con messaggio
            return NotFound(new { messaggio = "Utente non trovato." });
        }

        // Salva un log di ricerca profilo riuscita
        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ricerca profilo loggato", true);
        // Restituisce 200 OK con il DTO dell'utente
        return Ok(utente);
    }

    // Espone un endpoint PUT su /api/Auth/modifica
    [HttpPut("modifica")]
    // Metodo asincrono per modificare il profilo dell'utente loggato
    public async Task<IActionResult> Modifica([FromBody] DtoCreazioneUtente dto)
    {
        // Recupera l'Id utente dai claim del token JWT
        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        // Chiede al servizio di autenticazione di modificare i dati dell'utente
        var risultato = await _authService.ModificaAsync(dto, utenteId);

        // Se l'utente non è stato trovato o la modifica non è riuscita
        if (risultato == null)
        {
            // Salva un log di modifica fallita
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Modifica utente", false);
            // Restituisce 404 NotFound con messaggio
            return NotFound(new { messaggio = "Utente non trovato." });
        }

        // Salva un log di modifica riuscita
        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Modifica utente", true);
        // Restituisce 200 OK con il risultato (DTO aggiornato)
        return Ok(risultato);
    }

    // Espone un endpoint DELETE su /api/Auth/elimina
    [HttpDelete("elimina")]
    // Metodo asincrono per eliminare il profilo dell'utente loggato
    public async Task<IActionResult> Elimina()
    {
        // Recupera l'Id utente dai claim del token JWT
        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        // Chiede al servizio di autenticazione di eliminare l'utente
        var risultato = await _authService.EliminaAsync(utenteId);

        // Se l'utente non è stato trovato
        if (risultato == null)
        {
            // Restituisce 404 NotFound con messaggio
            return NotFound(new { messaggio = "Utente non trovato." });
        }

        // Salva un log di eliminazione profilo riuscita
        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Eliminazione profilo", true);
        // Restituisce 200 OK con il risultato (eventuale DTO o conferma)
        return Ok(risultato);
    }
}
```

## GestoreUtentiController.cs (modifiche logAzioni)
```c#
// Importa autorizzazioni e gestione ruoli
using Microsoft.AspNetCore.Authorization;
// Importa funzionalità dei controller API
using Microsoft.AspNetCore.Mvc;
// Permette di leggere i claim dell’utente loggato
using System.Security.Claims;
// Importa i DTO utilizzati dal controller
using NuovoCinemaParadiso.Dtos;
// Importa i servizi applicativi
using NuovoCinemaParadiso.Services;

namespace NuovoCinemaParadiso.Controllers;

// Indica che è un controller API
[ApiController]
// Route base: /api/GestoreUtenti
[Route("api/[controller]")]
// Accesso consentito solo al ruolo Gestore
[Authorize(Roles = Ruoli.Gestore)]
public class GestoreUtentiController : ControllerBase
{
    // Service per modificare i ruoli degli utenti
    private readonly RuoloUtenteService _ruoloUtenteService;
    // Service per salvare i log delle azioni
    private readonly LogAzioniService _logAzioniService;

    // Iniezione dei servizi tramite costruttore
    public GestoreUtentiController(RuoloUtenteService ruoloUtenteService, LogAzioniService logAzioniService)
    {
        _ruoloUtenteService = ruoloUtenteService;
        _logAzioniService = logAzioniService;
    }

    // Endpoint PUT: modifica il ruolo di un utente
    [HttpPut("cambia-ruolo")]
    public async Task<IActionResult> CambiaRuolo([FromBody] DtoModificaRuoloUtente dto)
    {
        // Richiede al service di aggiornare il ruolo dell’utente
        string? nuovoRuolo = await _ruoloUtenteService.ModificaRuoloUtente(dto);

        // Recupera l’Id del gestore loggato tramite token
        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        // Se il ruolo non è stato aggiornato (utente o ruolo inesistente)
        if (nuovoRuolo == null)
        {
            // Registra log di operazione fallita
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Cambio ruolo", false);

            // Restituisce errore al client
            return BadRequest(new { messaggio = "Utente o ruolo non valido." });
        }

        // Registra log di operazione riuscita
        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Cambio ruolo", true);
        
        // Restituisce conferma al client con i dati aggiornati
        return Ok(new
        {
            messaggio = "Ruolo aggiornato correttamente.",
            email = dto.Email,
            ruolo = nuovoRuolo
        });
    }
}
```

## GenereMovieController.cs (modifiche logAzioni)
```c#
// Importa autorizzazioni e gestione ruoli
using Microsoft.AspNetCore.Authorization;
// Importa funzionalità dei controller API
using Microsoft.AspNetCore.Mvc;
// Permette di leggere i claim dell’utente loggato
using System.Security.Claims;
// Importa i DTO utilizzati dal controller
using NuovoCinemaParadiso.Dtos;
// Importa i servizi applicativi
using NuovoCinemaParadiso.Services;

namespace NuovoCinemaParadiso.Controllers;

// Indica che è un controller API
[ApiController]
// Route base: /api/GenereMovie
[Route("api/[controller]")]
// Richiede autenticazione per accedere agli endpoint
[Authorize]
public class GenereMovieController : ControllerBase
{
    // Service per la gestione dei generi dei film
    private readonly GenereMovieService _genereMovieService;
    // Service per la registrazione dei log delle azioni
    private readonly LogAzioniService _logAzioniService;

    // Iniezione dei servizi tramite costruttore
    public GenereMovieController(GenereMovieService genereMovieService, LogAzioniService logAzioniService)
    {
        _genereMovieService = genereMovieService;
        _logAzioniService = logAzioniService;
    }

    // Endpoint GET: restituisce tutti i generi
    [HttpGet]
    public async Task<IActionResult> OttieniTutti()
    {
        // Recupera tutti i generi tramite il service
        List<DtoGenereMovie> generiMovie = await _genereMovieService.OttieniTuttoAsync();
        // Recupera l'Id dell'utente loggato
        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        // Registra log di operazione riuscita
        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ottieni tutti i generi", true);

        // Restituisce la lista dei generi
        return Ok(generiMovie);
    }

    // Endpoint GET: restituisce un genere tramite Id
    [HttpGet("{id}")]
    public async Task<IActionResult> OttieniTramiteId(string id)
    {
        // Recupera il genere tramite Id
        var risultato = await _genereMovieService.OttieniTramiteIdAsync(id);
        // Recupera l'Id dell'utente loggato
        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        // Se il genere non esiste
        if (risultato == null)
        {
            // Registra log fallito
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ottieni genere tramite id", false);

            // Restituisce errore 404
            return NotFound($"GenereMovie con id {id} non trovato");
        }

        // Registra log riuscito
        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ottieni genere tramite id", true);

        // Restituisce il genere trovato
        return Ok(risultato);
    }

    // Endpoint POST: crea un nuovo genere
    [HttpPost]
    [Authorize(Roles = Ruoli.GestoreOrOperatore)]
    public async Task<IActionResult> Creazione([FromBody] DtoCreazioneGenereMovie dto)
    {
        // Recupera l'Id dell'utente loggato
        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        // Recupera tutti i generi esistenti
        List<DtoGenereMovie> generiMovie = await _genereMovieService.OttieniTuttoAsync();

        // Controlla se il genere esiste già
        foreach (var generiMovies in generiMovie)
        {
            if (generiMovies.Genere.Contains(dto.Genere))
            {
                // Log fallito
                await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Creazione genere", false);

                // Restituisce errore
                return BadRequest(new { messaggio = "Genere già presente." });
            }
        }
        
        // Crea il nuovo genere tramite service
        DtoGenereMovie? risultato = await _genereMovieService.CreazioneAsync(dto);

        // Se la creazione non è valida
        if (risultato == null)
        {
            // Log fallito
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Creazione genere", false);

            // Restituisce errore
            return BadRequest(new { messaggio = "Genere non valido." });
        }

        // Log riuscito
        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Creazione genere", true);

        // Restituisce il genere creato
        return Ok(risultato);
    }

    // Endpoint PUT: modifica un genere esistente
    [HttpPut("{id}")]
    [Authorize(Roles = Ruoli.GestoreOrOperatore)]
    public async Task<IActionResult> Modifica(string id, [FromBody] DtoCreazioneGenereMovie dto)
    {
        // Modifica il genere tramite service
        DtoGenereMovie? risultato = await _genereMovieService.ModificaAsync(id, dto);
        // Recupera l'Id dell'utente loggato
        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        // Se il genere non esiste
        if (risultato == null)
        {
            // Log fallito
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Modifica genere", false);

            // Restituisce errore
            return NotFound(new { messaggio = "Genere non trovato." });
        }

        // Log riuscito
        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Modifica genere", true);

        // Restituisce il genere modificato
        return Ok(risultato);
    }

    // Endpoint DELETE: elimina un genere tramite Id
    [HttpDelete("{id}")]
    [Authorize(Roles = Ruoli.GestoreOrOperatore)]
    public async Task<IActionResult> Elimina(string id)
    {
        // Richiede al service di eliminare il genere
        bool eliminato = await _genereMovieService.EliminaAsync(id);
        // Recupera l'Id dell'utente loggato
        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        // Se il genere non esiste
        if (!eliminato)
        {
            // Log fallito
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Eliminazione genere", false);

            // Restituisce errore
            return NotFound(new { messaggio = "Genere non trovato." });
        }

        // Log riuscito
        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Eliminazione genere", true);

        // Restituisce 204 NoContent
        return NoContent();
    }
}
```

## UtenteController.cs (modifica log azioni)

```c#
// Importa gli attributi per la gestione dell'autorizzazione
using Microsoft.AspNetCore.Authorization;
// Importa le funzionalità base dei controller API
using Microsoft.AspNetCore.Mvc;
// Permette di leggere i claim dell'utente loggato dal token
using System.Security.Claims;
// Importa il servizio applicativo per la gestione degli utenti
using NuovoCinemaParadiso.Services;
// Importa i DTO utilizzati per scambiare dati con il client
using NuovoCinemaParadiso.Dtos;
// Importa i modelli del dominio (se necessari in questo controller)
using NuovoCinemaParadiso.Models;

namespace NuovoCinemaParadiso.Controllers;

// Indica che questa classe è un controller API
[ApiController]
// Definisce la route base: /api/Utente
[Route("api/[controller]")]
// Richiede che l'utente sia autenticato per accedere a tutti gli endpoint del controller
[Authorize]
public class UtenteController : ControllerBase
{
    // Riferimento al servizio che gestisce la logica sugli utenti
    private readonly UtenteService _utenteService;
    // Riferimento al servizio che registra i log delle azioni
    private readonly LogAzioniService _logAzioniService;

    // Costruttore con dependency injection dei servizi necessari
    public UtenteController(UtenteService utenteService, LogAzioniService logAzioniService)
    {
        // Assegna il servizio utenti al campo privato
        _utenteService = utenteService;
        // Assegna il servizio log al campo privato
        _logAzioniService = logAzioniService;
    }

    // Endpoint POST: permette all'utente loggato di abbonarsi
    [HttpPost("abbonati")]
    public async Task<IActionResult> Abbonati([FromBody] DtoUtente dto)
    {
        // Recupera l'Id dell'utente loggato dai claim del token
        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        // Verifica che il DTO non sia nullo e che l'Id dell'abbonamento sia valorizzato
        if (dto == null || string.IsNullOrEmpty(dto.AbbonamentoId))
        {
            // Registra un log di operazione fallita per l'azione "Abbonati"
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Abbonati", false);
            // Restituisce una risposta 400 BadRequest con messaggio di errore
            return BadRequest("Dati non validi");
        }

        // Richiede al servizio di associare l'abbonamento all'utente loggato
        var risultato = await _utenteService.AbbonatiAsync(dto.AbbonamentoId, utenteId);

        // Se il servizio non trova utente o abbonamento, o l'operazione fallisce
        if (risultato == null)
        {
            // Registra un log di operazione fallita per l'azione "Abbonati"
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Abbonati", false);
            // Restituisce una risposta 404 NotFound con messaggio di errore
            return NotFound("Utente o abbonamento non trovato");
        }

        // Registra un log di operazione riuscita per l'azione "Abbonati"
        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Abbonati", true);
        // Restituisce una risposta 200 OK con il risultato (tipicamente un DTO utente aggiornato)
        return Ok(risultato);
    }

    // Endpoint POST: permette all'utente loggato di associare una GiftCard
    [HttpPost("giftCard")]
    public async Task<IActionResult> GiftCard([FromBody] DtoUtente dto)
    {
        // Recupera l'Id dell'utente loggato dai claim del token
        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        // Verifica che il DTO non sia nullo e che l'Id della GiftCard sia valorizzato
        if (dto == null || string.IsNullOrEmpty(dto.GiftCardId))
        {
            // Registra un log di operazione fallita per l'azione "GiftCard"
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "GiftCard", false);
            // Restituisce una risposta 400 BadRequest con messaggio di errore
            return BadRequest("Dati non validi");
        }

        // Richiede al servizio di associare la GiftCard all'utente loggato
        var risultato = await _utenteService.GiftCardAsync(dto.GiftCardId, utenteId);

        // Se il servizio non trova utente o GiftCard, o l'operazione fallisce
        if (risultato == null)
        {
            // Registra un log di operazione fallita per l'azione "GiftCard"
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "GiftCard", false);
            // Restituisce una risposta 404 NotFound con messaggio di errore
            return NotFound("Utente o GiftCard non trovata");
        }

        // Registra un log di operazione riuscita per l'azione "GiftCard"
        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "GiftCard", true);
        // Restituisce una risposta 200 OK con il risultato (tipicamente un DTO utente aggiornato)
        return Ok(risultato);
    }
}
```