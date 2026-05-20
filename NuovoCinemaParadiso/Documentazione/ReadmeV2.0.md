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

## Movie.cs

```c#
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NuovoCinemaParadiso.Models;

[Table("Movies")]
public class Movie
{
    [Key]
    public string Id {get;set;} = Guid.NewGuid().ToString();
    [Required]
    [StringLength(50)]
    public string Titolo {get;set;} = string.Empty;
    [Required]
    [StringLength(2000)]
    public string Descrizione {get;set;} = string.Empty;
    [Range(1, int.MaxValue)]
    public int DurataMinuti {get;set;} 
    [Range(typeof(decimal), "0.01", "999999999")]
    public decimal PrezzoMovie {get;set;}
    public List<Proiezione> Proiezioni {get;set;} = new List<Proiezione>();
    public string GenereId {get;set;} = string.Empty;
    
    [ForeignKey("GenereId")]
    public GenereMovie? Genere {get;set;}
}
```


## Proiezione.cs

```c#
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace NuovoCinemaParadiso.Models;

[Table("Proiezioni")]
public class Proiezione
{
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString();

    [Required]
    public DateOnly DataProiezione { get; set; }

    [Required]
    public string MovieId { get; set; } = string.Empty;

    [ForeignKey("MovieId")]
    public Movie? Movie { get; set; }

    [Required]
    public string SalaId { get; set; } = string.Empty;

    [ForeignKey("SalaId")]
    public Sala? Sala { get; set; }

    [Required]
    public string TurnoId { get; set; } = string.Empty;

    [ForeignKey("TurnoId")]
    public Turno? Turno { get; set; }

    public List<Biglietto> Biglietti { get; set; } = new List<Biglietto>();

    public bool Attivo { get; set; } = true;
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

    // Biglietti associati all’utente.
    public List<Biglietto> Biglietti { get; set; } = new List<Biglietto>();

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

## Biglietto.cs

```c#
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace NuovoCinemaParadiso.Models;

/// <summary>
/// Rappresenta un biglietto effettuato da un utente per una specifica proiezione.
/// Contiene informazioni su biglietti, prezzo finale, metodo di pagamento e timestamp.
/// </summary>
[Table("Biglietti")] 
public class Biglietto
{
    /// <summary>
    /// Identificativo univoco dell'biglietto.
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
    /// FK verso l'utente che ha effettuato l'biglietto.
    /// </summary>
    [Required]
    public string UtenteId { get; set; } = string.Empty;

    /// <summary>
    /// Navigazione verso l'utente proprietario dell'biglietto.
    /// </summary>
    [ForeignKey("UtenteId")]
    public Utente Utente { get; set; }

    /// <summary>
    /// Numero di biglietti acquistati per questa proiezione.
    /// Decorator per far sì che i biglietti non possano andare in numero negativo e nemmeno superare i 100 massimi di capienza della sala.
    /// </summary>
    [Required]
    [Range(1, 100, ErrorMessage = "Il numero di biglietti deve essere maggiore di zero e massimo 100.")]
    public int NumeroBiglietti { get; set; }

    /// <summary>
    /// Timestamp di creazione dell'biglietto.
    /// Utilizza DateTimeOffset per mantenere il fuso orario.
    /// </summary>
    public DateTimeOffset OrarioCreazione { get; set; } = DateTimeOffset.UtcNow;

    /// <summary>
    /// Prezzo finale calcolato al momento dell'biglietto.
    /// Include eventuali maggiorazioni, sconti e quantità di biglietti.
    /// </summary>
    [Required]
    public decimal PrezzoFinale { get; set; }

    /// <summary>
    /// Metodo di pagamento scelto dall'utente per l'biglietto.
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
        // tabella degli biglietti (scontrini) relativi ad una proiezione di un film
        public DbSet<Biglietto> Biglietti { get; set; }
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
            modelBuilder.Entity<Biglietto>()
                .HasOne(a => a.Proiezione) // ogni biglietto appartiene ad una sola proiezione
                .WithMany(p => p.Biglietti) // ad ogni proiezione appartengono più biglietti
                .HasForeignKey(a => a.ProiezioneId) // la chiave esterna è ProiezioneId
                .OnDelete(DeleteBehavior.Restrict); 
                // 'Restrict' impedisce la cancellazione della proiezione se esistono degli biglietti relativi ad essa
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
/// È la versione "di output" mostrata a client, gestore o UI.
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
    public string Id {get; set; } = string.Empty;
    public DateOnly DataProiezione {get; set; }
    public string MovieId {get; set; } = string.Empty;
    public string SalaId {get; set; } = string.Empty;
    public string TurnoId {get; set; } = string.Empty;

    public bool Attivo { get; set; }=true;
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

## DtoBiglietto.cs

```c#
namespace NuovoCinemaParadiso.Dtos;

/// <summary>
/// DTO utilizzato per restituire i dati di un biglietto al client.
/// Contiene informazioni essenziali come prezzo finale, metodo di pagamento,
/// numero di biglietti e timestamp.
/// </summary>
public class DtoBiglietto
{
    /// <summary>
    /// Identificativo univoco dell'biglietto.
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// Identificativo della proiezione acquistata.
    /// </summary>
    public string ProiezioneId { get; set; } = string.Empty;

    /// <summary>
    /// Identificativo dell'utente che ha effettuato l'biglietto.
    /// </summary>
    public string UtenteId { get; set; } = string.Empty;

    /// <summary>
    /// Prezzo finale calcolato lato server.
    /// Include eventuali sconti, maggiorazioni e quantità di biglietti.
    /// </summary>
    public decimal PrezzoFinale { get; set; }

    /// <summary>
    /// Timestamp di creazione dell'biglietto.
    /// Utilizza DateTimeOffset per mantenere il fuso orario.
    /// </summary>
    public DateTimeOffset OrarioCreazione { get; set; }

    /// <summary>
    /// Numero di biglietti acquistati.
    /// </summary>
    public int NumeroBiglietti { get; set; }

    /// <summary>
    /// Metodo di pagamento utilizzato per l'biglietto.
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

## DtoCreazioneBiglietto.cs

```c#
using System.ComponentModel.DataAnnotations;

namespace NuovoCinemaParadiso.Dtos;

/// <summary>
/// DTO utilizzato dal client per creare un nuovo biglietto.
/// Contiene solo i dati necessari per effettuare l’operazione:
/// - la proiezione scelta
/// - il numero di biglietti
/// - il metodo di pagamento selezionato dall’utente
/// </summary>
public class DtoCreazioneBiglietto
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
    /// Decorator per far sì che i biglietti non possano andare in numero negativo e nemmeno superare i 100 massimi di capienza della sala.
    /// </summary>
    [Required]
    [Range(1, 100, ErrorMessage = "Il numero di biglietti deve essere maggiore di zero e massimo 100.")]
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

## DtoRicaricaGiftCard.cs
```c#
namespace NuovoCinemaParadiso.Dtos;

public class DtoRicaricaGiftCard
{
    public int Importo {get;set;}
}
```

# Controllers

## TipologiaSalaController.cs

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
    [Authorize(Roles = Ruoli.Operatore)]
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
    [Authorize(Roles = Ruoli.Operatore)]
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
    [Authorize(Roles = Ruoli.Operatore)]
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

        return Ok(new { messaggio = "Tipologia sala eliminata con successo!" });
    }

}
```

## SalaController.cs

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
// Impora i servizi per la gestione degli errori
using NuovoCinemaParadiso.Exceptions;

// Definisce il namespace del progetto per i controller
namespace NuovoCinemaParadiso.Controllers;

// Indica che questa classe è un controller API
[ApiController]
// Definisce la route base: /api/Sala
[Route("api/[controller]")]

[Authorize]
public class SalaController : ControllerBase
{
    // Campo per il servizio di autenticazione
    private readonly SalaService _salaService;
    // Campo per il servizio di log delle azioni
    private readonly LogAzioniService _logAzioniService;

    // Costruttore che riceve i servizi tramite dependency injection
    public SalaController(SalaService salaService, LogAzioniService logAzioniService)
    {
        // Assegna il servizio di autenticazione al campo privato
        _salaService = salaService;
        // Assegna il servizio di log al campo privato
        _logAzioniService = logAzioniService;
    }

    // Endpoint GET: restituisce tutte le sale
    [HttpGet]
    public async Task<IActionResult> OttieniTutti()
    {
        // Recupera l'Id dell'utente loggato
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");
            
        List<DtoSala> sale = await _salaService.OttieniTuttoAsync();
        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ottieni tutte le sale", true);

        return Ok(sale);
    }

    [HttpGet("tipologia/{tipologiaId}")]
    public async Task<ActionResult<List<DtoSala>>> OttieniPerTipologia(string tipologiaId)
    {
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");

        if (string.IsNullOrWhiteSpace(tipologiaId))
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ottieni sala per tipologia", false);

            return BadRequest("TipologiaId non valida");
        }

        var risultato = await _salaService.OttieniTramiteTipologiaAsync(tipologiaId);

        if (risultato == null || risultato.Count == 0)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ottieni sala per tipologia", false);

            return NotFound("Nessuna sala trovata per questa tipologia");
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ottieni sala per tipologia", true);

        return Ok(risultato);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> OttieniTramiteId(string id)
    {
        
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");
        var risultato = await _salaService.OttieniTramiteIdAsync(id);
        if (risultato == null)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ottieni sala tramite id", false);

            return NotFound($"Sala con id {id} non trovato");
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ottieni sala tramite id", true);

        return Ok(risultato);
    }

    [HttpPost]
    [Authorize(Roles = Ruoli.Operatore)]
    public async Task<IActionResult> Creazione([FromBody] DtoCreazioneSala dto)
    {
        
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");
        DtoSala? risultato = await _salaService.CreazioneAsync(dto);
        if (risultato == null)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Creazione sala", false);

            return BadRequest(new { messaggio = "Sala non valida." });
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Creazione sala", true);

        return Ok(risultato);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = Ruoli.Operatore)]
    public async Task<IActionResult> Modifica(string id, [FromBody] DtoCreazioneSala dto)
    {

        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");

        try
        {
            DtoSala? risultato = await _salaService.ModificaAsync(id, dto);
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Modifica sala", true);

            return Ok(risultato);
        }

        catch (ModificaException ex)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Modifica sala", false);
            return BadRequest(new { message = ex.Message });
        }

        catch (ItemNotFoundException ex)
        {

            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Modifica sala", false);

            return NotFound(new { message = ex.Message });

        }

        catch (NotFoundException ex)
        {

            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Modifica sala", false);

            return NotFound(new { message = ex.Message });

        }

    }

    [HttpDelete("{id}")]
    [Authorize(Roles = Ruoli.Operatore)]
    public async Task<IActionResult> Elimina(string id)
    {
        
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");
        bool eliminato = await _salaService.EliminaAsync(id);
        if (!eliminato)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Elimina sala", false);

            return NotFound(new { messaggio = "Sala non trovata." });
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Elimina sala", true);

        return Ok(new { messaggio = "Sala eliminata con successo!" });
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
    // RIMOSSO DATO LE SPECIFICHE DI HARDCODED
    // CREAZIONE GIFT CARD (Operatore)
    // ---------------------------------------------------------

    /*
    [HttpPost]
    [Authorize(Roles = Ruoli.Operatore)]
    public async Task<IActionResult> Creazione([FromBody] DtoCreazioneGiftCard dto)
    {
        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        // Crea nuova gift card
        DtoGiftCard? risultato = await _giftCardService.CreazioneAsync(dto);

        // Registra log
        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId,"Creazione di una giftcard" ,true);

        return Ok(risultato);
    }

*/

    // ---------------------------------------------------------
    // MODIFICA GIFT CARD (Operatore)
    // ---------------------------------------------------------
    [HttpPut("{id}")]
    [Authorize(Roles = Ruoli.Operatore)]
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
    // ELIMINA GIFT CARD (Operatore)
    // ---------------------------------------------------------
    [HttpDelete("{id}")]
    [Authorize(Roles = Ruoli.Operatore)]
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
    private readonly GestoreService _gestoreService;
    private readonly LogAzioniService _logAzioniService;

    // ✔ Iniezione dei servizi necessari al controller
    public AbbonamentoController(AbbonamentoService abbonamentoService, LogAzioniService logAzioniService, GestoreService gestoreService)
    {
        _abbonamentoService = abbonamentoService;
        _logAzioniService = logAzioniService;
        _gestoreService = gestoreService;
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
    // GET api/abbonamento/gestore/{id}
    // ✔ Endpoint riservato a Gestore/Operatore
    // ✔ Permette di ottenere un abbonamento tramite ID senza limiti
    // ------------------------------------------------------------
    [HttpGet("gestore/{id}")]
    [Authorize(Roles = Ruoli.Operatore)]
    public async Task<IActionResult> OttieniTramiteIdPerGestore(string id)
    {
        var risultato = await _gestoreService.OttieniAbbonamentoTramiteIdPerGestoreAsync(id);
        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (risultato == null)
        {
            // ❌ Log fallimento
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId,"Ottieni abbonamenti tramite id gestore",false);

            return NotFound($"Abbonamento con id {id} non trovato");
        }

        // ✔ Log successo
        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId,"Ottieni abbonamenti tramite id gestore",true);


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
    [Authorize(Roles = Ruoli.Operatore)]
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
    [Authorize(Roles = Ruoli.Operatore)]
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
    [Authorize(Roles = Ruoli.Operatore)]
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

## GestoreController.cs

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
// La route diventa: api/gestore
[Authorize]
// Richiede che l’utente sia autenticato per accedere a qualsiasi endpoint del controller.
public class GestoreController : ControllerBase
{
    private readonly GestoreService _gestoreService;
    private readonly LogAzioniService _logAzioniService;

    public GestoreController(GestoreService gestoreService, LogAzioniService logAzioniService)
    {
        // Iniezione dei servizi necessari al controller.
        _gestoreService = gestoreService;
        _logAzioniService = logAzioniService;
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

// Importa i modelli
using NuovoCinemaParadiso.Models;

namespace NuovoCinemaParadiso.Controllers;

// Indica che è un controller API 
[ApiController]
// Route base: /api/Proiezione
[Route("api/[controller]")]
[Authorize]
public class ProiezioneController : ControllerBase
{
    // Service per la gestione dei generi dei film
    private readonly ProiezioneService _proiezioneService;
    // Service per la registrazione dei log delle azioni
    private readonly LogAzioniService _logAzioniService;

    // Iniezione dei servizi tramite costruttore
    public ProiezioneController(ProiezioneService proiezioneService, LogAzioniService logAzioniService)
    {
        _proiezioneService = proiezioneService;
        _logAzioniService = logAzioniService;
    }

    // Endpoint GET: restituisce tutte le proiezioni
    [HttpGet]
    public async Task<IActionResult> OttieniTutteLeProiezioni()
    {
        List<DtoProiezione> proiezioni = await _proiezioneService.OttieniTuttoAsync();
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");

        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ottieni tutte le proieioni",true);

        return Ok(proiezioni);
    }
    // Endpoint GET: restituisce tutte le proiezioni anche quelle passate
    [HttpGet("storico")]
    public async Task<IActionResult> OttieniStoricoProiezioni()
    {
        List<DtoProiezione> proiezioni = await _proiezioneService.OttieniStoricoAsync();
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");

        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ottieni tutte le proieioni",true);

        return Ok(proiezioni);
    }

    // Endpoint GET: restituisce una proiezione tramite Id
    [HttpGet("{id}")]
    public async Task<IActionResult> OttieniTramiteId(string id)
    {
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");

        var risultato = await _proiezioneService.OttieniTramiteIdAsync(id);

        if (risultato == null)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ottieni proiezione tramite id",false);

            return NotFound($"Proiezione con id {id} non trovato");
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ottieni proiezione tramite id", true);

        return Ok(risultato);
    }

    // Endpoint GET: restituisce una proiezione tramite l'Id del turno
    [HttpGet("turno/{turnoId}")]
    public async Task<ActionResult<List<DtoProiezione>>> OttieniPerTurno(string turnoId)
    {
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");

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

    // Endpoint GET: restituisce una proiezione tramite l'Id della sal
    [HttpGet("sala/{salaId}")]
    public async Task<ActionResult<List<DtoProiezione>>> OttieniPerSala(string salaId)
    {
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");

        if (string.IsNullOrEmpty(salaId))
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ottieni proiezione per sala", false);
            return BadRequest("SalaId non valido");
        }

        var risultato = await _proiezioneService.OttieniTramiteSalaAsync(salaId);

        if (risultato == null)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ottieni proiezione per sala", false);
            return NotFound("Nessuna proiezione trovata per questa sala");
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ottieni proiezione per sala", true);

        return Ok(risultato);
    }

    // Endpoint GET: restituisce una proiezione tramite l'Id del movie (film)
    [HttpGet("movie/{movieId}")]
    public async Task<ActionResult<List<DtoProiezione>>>OttieniPerFilm(string movieId)
    {
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");

        if (string.IsNullOrEmpty(movieId))
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ottieni proiezioni per film", false);
            return BadRequest("MovieId non valido");
        }

        var risultato = await _proiezioneService.OttieniTramiteMovieAsync(movieId);

        if (risultato == null)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ottieni proiezioni per film", false);
            return NotFound("Nessuna proiezione trovata per questo film");
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ottieni proiezioni per film", true);

        return Ok(risultato);
    }

    // Endpoint POST: Crea una proiezione
    [HttpPost]
    [Authorize(Roles = Ruoli.Operatore)]
    public async Task<IActionResult> Creazione([FromBody] DtoCreazioneProiezione dto)
    {
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");

        List<DtoProiezione> proiezioni = await _proiezioneService.OttieniTuttoAsync();

        foreach (var proiezione in proiezioni)
        {
            if (proiezione.TurnoId == dto.TurnoId && proiezione.SalaId == dto.SalaId && proiezione.DataProiezione == dto.DataProiezione)
            {
                await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Crea proiezione", false);

                return BadRequest(new { messaggio = "Proiezione già presente." });
            }
        }

        DtoProiezione? risultato = await _proiezioneService.CreazioneAsync(dto);

        if (risultato == null)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Crea proiezione", false);
            return BadRequest(new { messaggio = "Proiezione non valida." });
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Crea proiezione", true);
        return Ok(risultato);
    }

    // Endpoint PUT: Modifica una proiezione
    [HttpPut("{id}")]
    [Authorize(Roles = Ruoli.Operatore)]
    public async Task<IActionResult> Modifica(string id, [FromBody] DtoCreazioneProiezione dto)
    {
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");

        DtoProiezione? risultato = await _proiezioneService.ModificaAsync(id,dto);

        if (risultato == null)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Modifica proiezione", false);

            return NotFound(new { messaggio = "Proiezione non trovata." });
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Modifica proiezione", true);

        return Ok(risultato);
    }

    // Endpoint GET: Modifica una proiezione disabilitandola al posto di eliminarla
    [HttpPut("elimina/{id}")]
    [Authorize(Roles = Ruoli.Operatore)]
    public async Task<IActionResult> Elimina(string id)
    {
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");

        bool eliminato = await _proiezioneService.EliminaAsync(id);

        if (!eliminato)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Elimina proiezione", false);

            return NotFound(new { messaggio = "Proiezione non trovata." });
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Elimina proiezione", true);

        return Ok(new { messaggio = "Proiezione eliminata con successo!" });
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

// Controller per la gestione delle operazioni CRUD sui Turni, protetto da autorizzazione globale
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TurnoController : ControllerBase
{
    private readonly TurnoService _turnoService;
    private readonly LogAzioniService _logAzioniService;

    // Inizializza il controller iniettando i servizi necessari per i turni e il tracciamento dei log
    public TurnoController(TurnoService turnoService, LogAzioniService logAzioniService)
    {
        _turnoService = turnoService;
        _logAzioniService = logAzioniService;
    }

    // Recupera la lista di tutti i turni registrati a sistema
    [HttpGet]
    public async Task<IActionResult> OttieniTutti()
    {
        // Verifica che l'utente che fa la richiesta sia autenticato
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null) return Unauthorized("Utente non autenticato.");

        // Recupera i dati, registra il successo dell'operazione nel log e restituisce la lista
        List<DtoTurno> turni = await _turnoService.OttieniTuttoAsync();
        
        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ottieni tutti i turni", true);
        return Ok(turni);
    }

    // Recupera i dettagli di un singolo turno specificandone l'ID
    [HttpGet("{id}")]
    public async Task<IActionResult> OttieniTramiteId(string id)
    {
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null) return Unauthorized("Utente non autenticato.");

        var risultato = await _turnoService.OttieniTramiteIdAsync(id);

        // Se il turno non esiste, traccia il fallimento nel log e restituisce un errore 404 (Not Found)
        if (risultato == null)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ottieni turno tramite id", false);
            return NotFound(new { messaggio = $"Turno con id {id} non trovato" });
        }

        // Se trovato, traccia il successo e restituisce il turno
        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ottieni turno tramite id", true);
        return Ok(risultato);
    }

    // Crea un nuovo turno. Rotta riservata unicamente agli utenti con ruolo Gestore o Operatore
    [HttpPost]
    [Authorize(Roles = Ruoli.Operatore)]
    public async Task<IActionResult> Creazione([FromBody] DtoCreazioneTurno dto)
    {
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null) return Unauthorized("Utente non autenticato.");

        var (risultato, errore) = await _turnoService.CreazioneAsync(dto);

        // Se ci sono errori di validazione (es. dati mancanti), fallisce con 400 (Bad Request)
        if (errore != null)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Creazione Turno", false);
            return BadRequest(new { messaggio = errore });
        }

        // Se fallisce per un problema generico del server, restituisce 500 (Internal Server Error)
        if (risultato == null)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Creazione Turno", false);
            return StatusCode(500, new { messaggio = "Errore generico durante la creazione." });
        }

        // Registra la creazione andata a buon fine e restituisce il nuovo oggetto turno
        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Creazione Turno", true);
        return Ok(risultato);
    }

    // Modifica le informazioni di un turno esistente tramite ID. Riservato a Gestore o Operatore
    [HttpPut("{id}")]
    [Authorize(Roles = Ruoli.Operatore)]
    public async Task<IActionResult> Modifica(string id, [FromBody] DtoCreazioneTurno dto)
    {
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null) return Unauthorized("Utente non autenticato.");

        var (risultato, errore) = await _turnoService.ModificaAsync(id, dto);

        // Controlla se il turno da modificare non esiste nel database
        if (errore == "Turno non trovato.")
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Modifica Turno", false);
            return NotFound(new { messaggio = errore });
        }
        // Controlla altri eventuali errori di logica/validazione passati dal servizio
        else if (errore != null)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Modifica Turno", false);
            return BadRequest(new { messaggio = errore });
        }

        // Operazione riuscita: salva il log e ritorna l'oggetto aggiornato
        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Modifica Turno", true);
        return Ok(risultato);
    }

    // Rimuove un turno dal sistema tramite il suo ID. Riservato a Gestore o Operatore
    [HttpDelete("{id}")]
    [Authorize(Roles = Ruoli.Operatore)]
    public async Task<IActionResult> Elimina(string id)
    {
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null) return Unauthorized("Utente non autenticato.");

        var (successo, errore) = await _turnoService.EliminaAsync(id);

        // Gestisce il caso di insuccesso dell'eliminazione e smista il tipo di errore HTTP appropriato
        if (!successo)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Elimina Turno", false);
            
            if (errore == "Turno non trovato.")
            {
                return NotFound(new { messaggio = errore });
            }
            else
            {
                return BadRequest(new { messaggio = errore });
            }
        }

        // Eliminazione completata con successo: traccia l'azione e ritorna 204 (No Content)
        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Elimina Turno", true);
        return NoContent();
    }
}
```

## BigliettoController.cs

```c#
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using NuovoCinemaParadiso.Services;
using NuovoCinemaParadiso.Dtos;

namespace NuovoCinemaParadiso.Controllers;

// Definisce il controller per le API degli biglietti, richiedendo l'autenticazione tramite token JWT
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class BigliettoController : ControllerBase
{
    private readonly BigliettoService _bigliettoService;
    private readonly LogAzioniService _logAzioniService;

    // Costruttore: inietta i servizi necessari (Biglietti e Log)
    public BigliettoController(BigliettoService bigliettoService, LogAzioniService logAzioniService)
    {
        _bigliettoService = bigliettoService;
        _logAzioniService = logAzioniService;
    }

    // GET: api/Biglietto - Recupera tutti gli biglietti dell'utente loggato
    [HttpGet]
    public async Task<IActionResult> OttieniTutti()
    {
        // Estrae l'ID dell'utente dal token JWT
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null) return Unauthorized("Utente non autenticato.");

        // Chiama il service per ottenere i dati e spacchetta la tupla
        var (risultato, errore) = await _bigliettoService.OttieniTutto(utenteId);
        
        // Gestione errori: se c'è un errore, registra il fallimento e restituisce 400 Bad Request
        if (errore != null) {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ottieni biglietti", false);
            return BadRequest(new { messaggio = errore });
        }

        // Caso di successo: registra l'azione e restituisce 200 OK con i dati
        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ottieni biglietti", true);
        return Ok(risultato);
    }

    // GET: api/Biglietto/{id} - Recupera un singolo biglietto dell'utente loggato
    [HttpGet("{id}")]
    public async Task<IActionResult> OttieniTramiteId(string id)
    {
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null) return Unauthorized("Utente non autenticato.");

        var (risultato, errore) = await _bigliettoService.OttieniTramiteIdAsync(id, utenteId);

        // Smistamento errori: 404 se non esiste, 400 per altri errori di logica/permessi
        if (errore != null) {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ottieni biglietto ID", false);
            if (errore.Contains("non trovato")) return NotFound(new { messaggio = errore });
            return BadRequest(new { messaggio = errore });
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ottieni biglietto ID", true);
        return Ok(risultato);
    }

    // POST: api/Biglietto - Crea un nuovo biglietto
    [HttpPost]
    public async Task<IActionResult> Creazione([FromBody] DtoCreazioneBiglietto dto)
    {
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null) return Unauthorized("Utente non autenticato.");

        var (risultato, errore) = await _bigliettoService.CreazioneAsync(dto, utenteId);

        if (errore != null) {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Creazione biglietto", false);
            if (errore.Contains("non trovat")) return NotFound(new { messaggio = errore });
            return BadRequest(new { messaggio = errore });
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Creazione biglietto", true);
        return Ok(risultato);
    }

    // PUT: api/Biglietto/{id} - Modifica i biglietti di un biglietto (Solo Gestore o Operatore)
    [HttpPut("{id}")]
    [Authorize(Roles = Ruoli.Operatore)]
    public async Task<IActionResult> Modifica(string id, [FromBody] DtoCreazioneBiglietto dto)
    {
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null) return Unauthorized("Utente non autenticato.");

        var (risultato, errore) = await _bigliettoService.ModificaAsync(id, dto);

        if (errore != null) {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Modifica biglietto", false);
            if (errore == "Biglietto non trovato.") return NotFound(new { messaggio = errore });
            return BadRequest(new { messaggio = errore });
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Modifica biglietto", true);
        return Ok(risultato);
    }

    // DELETE: api/Biglietto/{id} - Elimina un biglietto e da il rimborsa l'utente
    [HttpDelete("{id}")]
    
    public async Task<IActionResult> Elimina(string id)
    {
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null) return Unauthorized("Utente non autenticato.");

        var (successo, errore) = await _bigliettoService.EliminazioneAsync(id);

        // Se l'eliminazione fallisce, restituisce 404 Not Found
        if (!successo) {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Elimina biglietto", false);
            return NotFound(new { messaggio = errore });
        }

        // Restituisce 204 No Content per indicare il successo dell'eliminazione
        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Elimina biglietto", true);
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
    [Authorize(Roles = Ruoli.Operatore)]
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
    [Authorize(Roles = Ruoli.Operatore)]
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
    [Authorize(Roles = Ruoli.Operatore)]
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
        return Ok(new { messaggio = "Film eliminato con successo!" });
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

# Services

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
            // Costruisce un errore personalizzato e lo inserisce all'interno di una lista
            IdentityError errore = new IdentityError();
            errore.Description = "Utente già registrato.";

            List<IdentityError> errori = new List<IdentityError>();
            errori.Add(errore);

            return IdentityResult.Failed(errori.ToArray());
        }
        // Verifica se l'email è valida controllando se abbia un . all'interno 
        
        if(!dto.Email.Contains('.'))
        {
            throw new InvalidEmail(dto.Email);
        }
        // Creazione nuovo utente Identity
        Utente utente = new Utente();
        utente.UserName = dto.Email;
        utente.Email = dto.Email;
        utente.NomeCompleto = dto.NomeCompleto;
        dto.Saldo = 100;
        utente.Saldo = dto.Saldo;
        utente.Eta = dto.Eta;
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
using NuovoCinemaParadiso.Exceptions;
using NuovoCinemaParadiso.Helpers;
using System.Linq.Expressions;

namespace NuovoCinemaParadiso.Services;

// Servizio applicativo per le operazioni specifiche dell'utente
public class UtenteService
{
    // Riferimento al DbContext per operazioni sul database
    private readonly ContestoDb _contesto;
    // Riferimento a UserManager per la gestione degli utenti
    private readonly UserManager<Utente> _gestioneUtenti;

    // Iniezione delle dipendenze tramite costruttore
    public UtenteService(ContestoDb contestoDb, UserManager<Utente> gestioneUtenti)
    {
        _contesto = contestoDb;
        _gestioneUtenti = gestioneUtenti;
    }

    // Associa un abbonamento a un utente
    public async Task<DtoUtente> AbbonatiAsync(string abbonamentoId, string utenteId)
    {
        // Lettura della tabella Abbonamenti
        List<Abbonamento> abbonamenti = await _contesto.Abbonamenti.ToListAsync();
        Abbonamento? abbonamentoTrovato = null;

        // Ricerca manuale dell'abbonamento desiderato
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
            throw new NotFoundException("Abbonamento", abbonamentoId);
        }

        // Recupero dell'utente richiedente tramite UserManager
        Utente? utenteTrovato = await _gestioneUtenti.FindByIdAsync(utenteId);
        if (utenteTrovato == null)
        {
            throw new NotFoundException("Utente", utenteId);
        }   

        // Controllo per evitare abbonamenti multipli
        if(utenteTrovato.SeAbbonato)
        {
            throw new ItemAlredyexist("Abbonamento");
        }

        // Aggiornamento dei dati utente con il nuovo abbonamento
        utenteTrovato.AbbonamentoId = abbonamentoTrovato.Id;
        utenteTrovato.SeAbbonato = true;
        utenteTrovato.DataInizioAbbonamento = DateTimeOffset.UtcNow;
        
        // Salvataggio nel database
        await _contesto.SaveChangesAsync();

        // Mappatura e restituzione del risultato in formato DTO
        return new DtoUtente()
        {
            Id = utenteTrovato.Id,
            NomeCompleto = utenteTrovato.NomeCompleto,
            Email = utenteTrovato.Email,
            Eta = utenteTrovato.Eta,
            SeAbbonato = utenteTrovato.SeAbbonato,
            DataInizioAbbonamento = utenteTrovato.DataInizioAbbonamento,
            AbbonamentoId = utenteTrovato.AbbonamentoId,
            TipoAbbonamento = abbonamentoTrovato.Nome
        };
    }

    // Crea una GiftCard scalando l'importo dal saldo dell'utente
    public async Task<DtoGiftCard> RicaricaGiftCardAsync( string utenteId, DtoRicaricaGiftCard dto)
    {
        // Recupero utente
        Utente? utenteCorrente = await _gestioneUtenti.FindByIdAsync(utenteId);

        if (utenteCorrente?.Id == null)
        {
            throw new NotFoundException("Utente", utenteId);
        }

        // Validazione dell'importo
        if (dto.Importo <= 0)
        {
            throw new Exception("Impossibile ricaricare la giftcard. Importo non valido.");
        }
        
        // Controllo disponibilità economica sul saldo dell'utente
        if(utenteCorrente.Saldo < dto.Importo)
        {
            throw new Exception("Impossibile caricare la giftcard. Importo superiore al saldo");
        }
        
        // Deduzione dell'importo dal saldo
        utenteCorrente.Saldo -= dto.Importo;

        // Creazione dell'entità GiftCard
        GiftCard? nuovaGiftCard = new GiftCard()
        {
            Nome = "GiftCard",
            Valore = dto.Importo,
            CodiceRiscatto = GiftCardHelper.GeneraCodice()
        };
        
        // Salvataggio nel database
        await _contesto.GiftCards.AddAsync(nuovaGiftCard);
        await _contesto.SaveChangesAsync();

        // Mappatura dell'entità salvata in DTO
        return new DtoGiftCard()
        {
            Id = nuovaGiftCard.Id,
            Nome = nuovaGiftCard.Nome,
            Valore = nuovaGiftCard.Valore,
            CodiceRiscatto = nuovaGiftCard.CodiceRiscatto
        };
    }

    // Aggiunge il valore di una GiftCard al saldo dell'utente che la riscatta
    public async Task<DtoGiftCard> RiscattaGiftCardAsync(string giftCardCodiceRiscatto, string utenteId)
    {
        // Lettura della tabella GiftCards
        List<GiftCard> giftCards = await _contesto.GiftCards.ToListAsync();
        GiftCard? giftCardTrovata = null;

        // Recupero utente
        Utente? utenteCorrente = await _gestioneUtenti.FindByIdAsync(utenteId);

        if (utenteCorrente?.Id == null)
        {
            throw new NotFoundException("Utente", utenteId);
        }

        // Ricerca manuale della GiftCard tramite il codice di riscatto
        for (int i = 0; i < giftCards.Count; i++)
        {
            GiftCard giftCardCorrente = giftCards[i];

            if (giftCardCorrente.CodiceRiscatto == giftCardCodiceRiscatto)
            {
                giftCardTrovata = giftCardCorrente;
                break;
            }
        }
        
        if (giftCardTrovata == null)
        {
            throw new NotFoundException("GiftCard", giftCardCodiceRiscatto);
        }

        // Controllo per impedire utilizzi multipli dello stesso codice
        if(giftCardTrovata.Riscattata == true)
        {
            throw new Exception("Il codice riscatto è già stato utilizzato.");
        }
        
        // Accredito dell'importo e invalidazione della GiftCard
        utenteCorrente.Saldo += giftCardTrovata.Valore;
        giftCardTrovata.Riscattata = true;

        // Salvataggio nel database
        await _contesto.SaveChangesAsync();

        // Mappatura del risultato in DTO
        return new DtoGiftCard()
        {
            Id = giftCardTrovata.Id,
            Nome = giftCardTrovata.Nome,
            Valore = giftCardTrovata.Valore,
            CodiceRiscatto = giftCardTrovata.CodiceRiscatto
        };
    }
}
```



## GestoreService.cs

```c#
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NuovoCinemaParadiso.Data;
using NuovoCinemaParadiso.Dtos;
using NuovoCinemaParadiso.Models;
using NuovoCinemaParadiso.Helpers;

namespace NuovoCinemaParadiso.Services;

public class GestoreService
{
    private readonly ContestoDb _contesto;
    private readonly UserManager<Utente> _gestioneUtenti;

    // ✔ Iniezione del DbContext e del gestore utenti Identity
    public GestoreService(ContestoDb contestoDb, UserManager<Utente> gestioneUtenti)
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
    // ✔ Restituisce tutti gli biglietti
    // ✔ Caricamento manuale delle entità correlate (senza Include)
    // ------------------------------------------------------------
    public async Task<List<DtoBiglietto>> OttieniBiglietti()
    {
        List<Biglietto> biglietti = await _contesto.Biglietti.ToListAsync();
        List<DtoBiglietto> risultato = new List<DtoBiglietto>();

        for (int i = 0; i < biglietti.Count; i++)
        {
            Biglietto bigliettoCorrente = biglietti[i];

            // ✔ Caricamento manuale delle entità correlate
            Proiezione proiezione = await _contesto.Proiezioni.FindAsync(bigliettoCorrente.ProiezioneId);
            Movie? movie = await _contesto.Movies.FindAsync(proiezione.MovieId);
            Sala? sala = await _contesto.Sale.FindAsync(proiezione.SalaId);
            Utente? utente = await _contesto.Utenti.FindAsync(bigliettoCorrente.UtenteId);
            TipologiaSala? tipologiaSala = await _contesto.TipologieSala.FindAsync(sala.TipologiaSalaId);

            // ✔ Conversione in DTO
            DtoBiglietto dto = new DtoBiglietto();
            dto.Id = bigliettoCorrente.Id;
            dto.ProiezioneId = bigliettoCorrente.ProiezioneId;
            dto.UtenteId = bigliettoCorrente.UtenteId;
            dto.PrezzoFinale = Calcoli.CalcolaPrezzoFinale(
                movie.PrezzoMovie,
                tipologiaSala.MaggiorazionePrezzo,
                bigliettoCorrente.NumeroBiglietti,
                utente
            );
            dto.OrarioCreazione = bigliettoCorrente.OrarioCreazione;
            dto.NumeroBiglietti = bigliettoCorrente.NumeroBiglietti;

            risultato.Add(dto);
        }

        return risultato;
    }

    // ------------------------------------------------------------
    // ✔ Restituisce un biglietto tramite ID
    // ✔ Caricamento manuale delle entità correlate
    // ------------------------------------------------------------
    public async Task<DtoBiglietto> OttieniBigliettoTramiteIdAsync(string id)
    {
        Biglietto? biglietto = await _contesto.Biglietti.FindAsync(id);

        if (biglietto == null)
        {
            return null;
        }

        // ✔ Caricamento entità correlate
        Proiezione proiezione = await _contesto.Proiezioni.FindAsync(biglietto.ProiezioneId);
        Movie? movie = await _contesto.Movies.FindAsync(proiezione.MovieId);
        Sala? sala = await _contesto.Sale.FindAsync(proiezione.SalaId);
        Utente? utente = await _contesto.Users.FindAsync(biglietto.UtenteId);
        TipologiaSala? tipologiaSala = await _contesto.TipologieSala.FindAsync(sala.TipologiaSalaId);

        // ✔ Conversione in DTO
        DtoBiglietto dto = new DtoBiglietto();
        dto.Id = biglietto.Id;
        dto.UtenteId = biglietto.UtenteId;
        dto.ProiezioneId = biglietto.ProiezioneId;
        dto.PrezzoFinale = Calcoli.CalcolaPrezzoFinale(
            movie.PrezzoMovie,
            tipologiaSala.MaggiorazionePrezzo,
            biglietto.NumeroBiglietti,
            utente
        );
        dto.OrarioCreazione = biglietto.OrarioCreazione;
        dto.NumeroBiglietti = biglietto.NumeroBiglietti;

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
    // ✔ Restituisce un abbonamento tramite ID (per gestore)
    // ------------------------------------------------------------
    public async Task<DtoAbbonamento> OttieniAbbonamentoTramiteIdPerGestoreAsync(string id)
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
using Microsoft.EntityFrameworkCore;
using NuovoCinemaParadiso.Data;
using NuovoCinemaParadiso.Dtos;
using NuovoCinemaParadiso.Models;

namespace NuovoCinemaParadiso.Services;

public class ProiezioneService
{
    private readonly ContestoDb _contesto;
    public ProiezioneService(ContestoDb contesto)
    {
        _contesto = contesto;
    }

    public async Task<List<DtoProiezione>> OttieniTuttoAsync()
    {
        List<Proiezione> proiezioni = await _contesto.Proiezioni.ToListAsync();
        List<DtoProiezione> risultato = new List<DtoProiezione>();

        for (int i = 0; i < proiezioni.Count; i++)
        {
            Proiezione proiezioneCorrente = proiezioni[i];
            if(proiezioneCorrente.Attivo)
            {
                Movie? film = await _contesto.Movies.FindAsync(proiezioneCorrente.MovieId);
                Sala? sala = await _contesto.Sale.FindAsync(proiezioneCorrente.SalaId);

                DtoProiezione dto = new DtoProiezione();
                dto.Id = proiezioneCorrente.Id;
                dto.DataProiezione = proiezioneCorrente.DataProiezione;
                dto.MovieId = proiezioneCorrente.MovieId;
                dto.SalaId = proiezioneCorrente.SalaId;
                dto.TurnoId = proiezioneCorrente.TurnoId;
                dto.Attivo = proiezioneCorrente.Attivo;
                risultato.Add(dto);
            }
        }
        return risultato;
    }

        public async Task<List<DtoProiezione>> OttieniStoricoAsync()
    {
        List<Proiezione> proiezioni = await _contesto.Proiezioni.ToListAsync();
        List<DtoProiezione> risultato = new List<DtoProiezione>();

        for (int i = 0; i < proiezioni.Count; i++)
        {
            Proiezione proiezioneCorrente = proiezioni[i];

            Movie? film = await _contesto.Movies.FindAsync(proiezioneCorrente.MovieId);
            Sala? sala = await _contesto.Sale.FindAsync(proiezioneCorrente.SalaId);

            DtoProiezione dto = new DtoProiezione();
            dto.Id = proiezioneCorrente.Id;
            dto.DataProiezione = proiezioneCorrente.DataProiezione;
            dto.MovieId = proiezioneCorrente.MovieId;
            dto.SalaId = proiezioneCorrente.SalaId;
            dto.TurnoId = proiezioneCorrente.TurnoId;
            dto.Attivo = proiezioneCorrente.Attivo;
            risultato.Add(dto);
        }
        return risultato;
    }

    public async Task<DtoProiezione?> OttieniTramiteIdAsync(string id)
    {
        Proiezione? proiezione = await _contesto.Proiezioni.FindAsync(id);
        if (proiezione == null)
        {
            return null;
        }
       
        Movie? film = await _contesto.Movies.FindAsync(proiezione.MovieId);
        Sala? sala = await _contesto.Sale.FindAsync(proiezione.SalaId);
        Turno? turno = await _contesto.Turni.FindAsync(proiezione.TurnoId);

        
        DtoProiezione risultato = new DtoProiezione();
        risultato.Id = proiezione.Id;
        risultato.DataProiezione = proiezione.DataProiezione;
        risultato.MovieId = proiezione.MovieId;
        risultato.SalaId = proiezione.SalaId;
        risultato.TurnoId = proiezione.TurnoId;
        risultato.Attivo = proiezione.Attivo;
        return risultato;
    }

    public async Task<List<DtoProiezione>> OttieniTramiteMovieAsync (string movieId)
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
                dto.Attivo = proiezioneCorrente.Attivo;
                risultato.Add(dto);
            }

            
        }
        return risultato;
    }

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
                dto.Attivo = proiezioneCorrente.Attivo;
                risultato.Add(dto);
            }
        }
        return risultato;
    }

    public async Task<List<DtoProiezione>> OttieniTramiteTurnoAsync(string turnoId)
    {
        List<DtoProiezione> risultato = new List<DtoProiezione>();
        List<Proiezione> proiezioni = await _contesto.Proiezioni.ToListAsync();

        for(int i = 0; i < proiezioni.Count; i++)
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
                dto.Attivo = proiezioneCorrente.Attivo;
                risultato.Add(dto);
            }
        }
        return risultato;

    }

    public async Task<DtoProiezione?> CreazioneAsync(DtoCreazioneProiezione dto)
    {
        Proiezione proiezione = new Proiezione();

        
        proiezione.DataProiezione = dto.DataProiezione;
        proiezione.MovieId = dto.MovieId;
        proiezione.SalaId = dto.SalaId;
        proiezione.TurnoId = dto.TurnoId;
        
        _contesto.Proiezioni.Add(proiezione);
        await _contesto.SaveChangesAsync();

        Movie? film = await _contesto.Movies.FindAsync(proiezione.MovieId);
        Sala? sala = await _contesto.Sale.FindAsync(proiezione.SalaId);
        Turno? turno = await _contesto.Turni.FindAsync(proiezione.TurnoId);

        
        DtoProiezione risultato = new DtoProiezione();
        risultato.Id = proiezione.Id;
        risultato.DataProiezione = proiezione.DataProiezione;
        risultato.MovieId = proiezione.MovieId;
        risultato.SalaId = proiezione.SalaId;
        risultato.TurnoId = proiezione.TurnoId;
        risultato.Attivo = proiezione.Attivo;
        return risultato;
    }

    public async Task<DtoProiezione?> ModificaAsync(string id, DtoCreazioneProiezione dto)
    {
        Proiezione? proiezione = await _contesto.Proiezioni.FindAsync(id);

        if (proiezione == null)
        {
            return null;
        }

        proiezione.DataProiezione = dto.DataProiezione;
        proiezione.MovieId = dto.MovieId;
        proiezione.SalaId = dto.SalaId;
        proiezione.TurnoId = dto.TurnoId;

        await _contesto.SaveChangesAsync();

        Movie? film = await _contesto.Movies.FindAsync(proiezione.MovieId);
        Sala? sala = await _contesto.Sale.FindAsync(proiezione.SalaId);
        Turno? turno = await _contesto.Turni.FindAsync(proiezione.TurnoId);

        DtoProiezione risultato = new DtoProiezione();
        risultato.Id = proiezione.Id;
        risultato.DataProiezione = proiezione.DataProiezione;
        risultato.MovieId = proiezione.MovieId;
        risultato.SalaId = proiezione.SalaId;
        risultato.TurnoId = proiezione.TurnoId;
        risultato.Attivo = proiezione.Attivo;
        return risultato;


    }

    public async Task<bool> EliminaAsync(string id)
    {
        Proiezione? proiezione = await _contesto.Proiezioni.FindAsync(id);

        if (proiezione == null)
        {
            return false;
        }

        proiezione.Attivo = false;
        await _contesto.SaveChangesAsync();
        
        return true;
    }
}
```

## TurnoService.cs

```c#
using Microsoft.EntityFrameworkCore;
using NuovoCinemaParadiso.Data;
using NuovoCinemaParadiso.Dtos;
using NuovoCinemaParadiso.Models;

namespace NuovoCinemaParadiso.Services
{
    // Servizio che gestisce la logica di business e l'accesso al database per l'entità Turno
    public class TurnoService
    {
        private readonly ContestoDb _contesto;

        // Inizializza il servizio iniettando il contesto del database (Entity Framework)
        public TurnoService(ContestoDb contesto)
        {
            _contesto = contesto;
        }

        // Recupera tutti i turni dal database e li converte in una lista di DTO (Data Transfer Object)
        public async Task<List<DtoTurno>> OttieniTuttoAsync()
        {
            List<Turno> turni = await _contesto.Turni.ToListAsync();
            List<DtoTurno> risultato = new List<DtoTurno>();

            // Mappatura manuale da entità di dominio a DTO
            foreach (var turnoCorrente in turni)
            {
                DtoTurno dto = new DtoTurno();
                dto.Id = turnoCorrente.Id;
                dto.OraInizio = turnoCorrente.OraInizio;
                dto.OraFine = turnoCorrente.OraFine;
                dto.Nome = turnoCorrente.Nome;
                risultato.Add(dto);
            }

            return risultato;
        }

        // Cerca un turno specifico tramite il suo ID. Restituisce null se non viene trovato
        public async Task<DtoTurno?> OttieniTramiteIdAsync(string id)
        {
            Turno? turno = await _contesto.Turni.FindAsync(id);
            if (turno == null)
            {
                return null;
            }

            DtoTurno dto = new DtoTurno();
            dto.Id = turno.Id;
            dto.Nome = turno.Nome;
            dto.OraInizio = turno.OraInizio;
            dto.OraFine = turno.OraFine;

            return dto;
        }

        // Crea un nuovo turno nel database, verificando prima che non ci siano duplicati nel nome
        public async Task<(DtoTurno? Dto, string? Errore)> CreazioneAsync(DtoCreazioneTurno dto)
        {
            // Controllo case-insensitive per evitare di creare turni con lo stesso nome
            bool turnoGiaPresente = await _contesto.Turni.AnyAsync(t => t.Nome.ToLower() == dto.Nome.ToLower());
            if (turnoGiaPresente)
            {
                return (null, "Turno già presente con questo nome.");
            }

            Turno turno = new Turno();
            turno.Nome = dto.Nome;
            turno.OraInizio = dto.OraInizio;
            turno.OraFine = dto.OraFine;

            _contesto.Turni.Add(turno);
            await _contesto.SaveChangesAsync();

            DtoTurno risultato = new DtoTurno();
            risultato.Id = turno.Id;
            risultato.Nome = turno.Nome;
            risultato.OraInizio = turno.OraInizio;
            risultato.OraFine = turno.OraFine;

            return (risultato, null);
        }

        // Modifica le informazioni di un turno esistente, applicando i dovuti controlli di unicità
        public async Task<(DtoTurno? Dto, string? Errore)> ModificaAsync(string id, DtoCreazioneTurno dto)
        {
            Turno? turnoEsistente = await _contesto.Turni.FindAsync(id);
            if (turnoEsistente == null)
            {
                return (null, "Turno non trovato.");
            }

            // Se il nome viene modificato, controlla che il nuovo nome non sia già usato da un ALTRO turno
            if (turnoEsistente.Nome.ToLower() != dto.Nome.ToLower())
            {
                bool nomeGiaUsato = await _contesto.Turni.AnyAsync(t => t.Nome.ToLower() == dto.Nome.ToLower() && t.Id != id);
                if (nomeGiaUsato)
                {
                    return (null, "Esiste già un altro turno con questo nome.");
                }
            }

            turnoEsistente.OraInizio = dto.OraInizio;
            turnoEsistente.OraFine = dto.OraFine;
            turnoEsistente.Nome = dto.Nome;

            await _contesto.SaveChangesAsync();

            DtoTurno risultato = new DtoTurno();
            risultato.Id = turnoEsistente.Id;
            risultato.OraInizio = turnoEsistente.OraInizio;
            risultato.OraFine = turnoEsistente.OraFine;
            risultato.Nome = turnoEsistente.Nome;

            return (risultato, null);
        }

        // Rimuove un turno dal database, bloccando l'operazione se ci sono relazioni pendenti
        public async Task<(bool Successo, string? Errore)> EliminaAsync(string id)
        {
            Turno? turno = await _contesto.Turni.FindAsync(id);
            if (turno == null)
            {
                return (false, "Turno non trovato.");
            }

            // Controllo di integrità: impedisce l'eliminazione se il turno è attualmente in uso per delle proiezioni
            bool haProiezioniCollegate = await _contesto.Proiezioni.AnyAsync(p => p.TurnoId == id);
            if (haProiezioniCollegate)
            {
                return (false, "Impossibile eliminare il turno: ci sono ancora delle proiezioni assegnate a questo orario.");
            }

            _contesto.Turni.Remove(turno);
            await _contesto.SaveChangesAsync();

            return (true, null);
        }
    }
}
```

## BigliettoService.cs

```c#
using Microsoft.EntityFrameworkCore;
using NuovoCinemaParadiso.Data;
using NuovoCinemaParadiso.Dtos;
using NuovoCinemaParadiso.Models;
using NuovoCinemaParadiso.Helpers;

namespace NuovoCinemaParadiso.Services;

// Servizio che gestisce la logica di business e le operazioni sul DB per l'entità Biglietto
public class BigliettoService
{
    private readonly ContestoDb _contesto;
    
    public BigliettoService(ContestoDb contesto)
    {
        _contesto = contesto;
    }

    // Recupera la lista di tutti gli biglietti legati a uno specifico utente
    public async Task<(List<DtoBiglietto>? Dati, string? Errore)> OttieniTutto(string utenteId)
    {
        List<Biglietto> biglietti = await _contesto.Biglietti.ToListAsync();
        List<DtoBiglietto> risultato = new List<DtoBiglietto>();

        foreach (var bigliettoCorrente in biglietti)
        {
            // Filtra solo gli biglietti dell'utente richiedente
            if (bigliettoCorrente.UtenteId == utenteId)
            {
                // Recupero delle entità correlate necessarie per calcolare il prezzo finale
                var utente = await _contesto.Utenti.FindAsync(bigliettoCorrente.UtenteId);
                var proiezione = await _contesto.Proiezioni.FindAsync(bigliettoCorrente.ProiezioneId);
                
                if (utente == null || proiezione == null) continue; // Salta iterazione se i dati sono corrotti

                var movie = await _contesto.Movies.FindAsync(proiezione.MovieId);
                var sala = await _contesto.Sale.FindAsync(proiezione.SalaId);
                
                if (movie == null || sala == null) continue;

                var tipologiaSala = await _contesto.TipologieSala.FindAsync(sala.TipologiaSalaId);
                if (tipologiaSala == null) continue;

                // Popolamento del DTO da restituire al client
                DtoBiglietto dto = new DtoBiglietto
                {
                    Id = bigliettoCorrente.Id,
                    UtenteId = utente.Id,
                    ProiezioneId = bigliettoCorrente.ProiezioneId,
                    OrarioCreazione = bigliettoCorrente.OrarioCreazione,
                    NumeroBiglietti = bigliettoCorrente.NumeroBiglietti,
                    MetodoPagamento = bigliettoCorrente.MetodoPagamento,
                    PrezzoFinale = Calcoli.CalcolaPrezzoFinale(
                        movie.PrezzoMovie,
                        tipologiaSala.MaggiorazionePrezzo,
                        bigliettoCorrente.NumeroBiglietti,
                        utente,
                        bigliettoCorrente.MetodoPagamento)
                };
                risultato.Add(dto);
            }
        }
        return (risultato, null);
    }

    // Recupera i dettagli di un singolo biglietto, verificandone la proprietà
    public async Task<(DtoBiglietto? Dto, string? Errore)> OttieniTramiteIdAsync(string id, string utenteId)
    {
        Biglietto? biglietto = await _contesto.Biglietti.FindAsync(id);
        if (biglietto == null) return (null, "Biglietto non trovato.");

        // Controllo di sicurezza: l'utente può vedere solo i propri biglietti
        if (biglietto.UtenteId != utenteId)
            return (null, "Accesso negato: questo biglietto non ti appartiene.");

        // Recupero dei dati relazionali per la costruzione del DTO
        var utente = await _contesto.Utenti.FindAsync(biglietto.UtenteId);
        var proiezione = await _contesto.Proiezioni.FindAsync(biglietto.ProiezioneId);
        if (utente == null || proiezione == null) return (null, "Dati della proiezione o utente non trovati.");

        var movie = await _contesto.Movies.FindAsync(proiezione.MovieId);
        var sala = await _contesto.Sale.FindAsync(proiezione.SalaId);
        if (movie == null || sala == null) return (null, "Dati del film o sala non trovati.");

        var tipologiaSala = await _contesto.TipologieSala.FindAsync(sala.TipologiaSalaId);
        if (tipologiaSala == null) return (null, "Tipologia sala non trovata.");

        DtoBiglietto dto = new DtoBiglietto
        {
            Id = biglietto.Id,
            UtenteId = utente.Id,
            ProiezioneId = biglietto.ProiezioneId,
            OrarioCreazione = biglietto.OrarioCreazione,
            NumeroBiglietti = biglietto.NumeroBiglietti,
            MetodoPagamento = biglietto.MetodoPagamento,
            PrezzoFinale = Calcoli.CalcolaPrezzoFinale(movie.PrezzoMovie, tipologiaSala.MaggiorazionePrezzo, biglietto.NumeroBiglietti, utente, biglietto.MetodoPagamento)
        };

        return (dto, null);
    }

    // Registra un nuovo biglietto nel database
    public async Task<(DtoBiglietto? Dto, string? Errore)> CreazioneAsync(DtoCreazioneBiglietto dto, string utenteId)
    {
        
        /*controlla che l'utente esista*/
        var utente = await _contesto.Utenti.FindAsync(utenteId);
        if (utente == null) return (null, "Utente non trovato.");

        /*controlla che la proiezione esista*/
        var proiezione = await _contesto.Proiezioni.FindAsync(dto.ProiezioneId);
        if (proiezione == null) return (null, "Proiezione non trovata.");

        /*controlla che ci siano abbastanza posti in sala per il numero di biglietti richiesti*/
        var sala = await _contesto.Sale.FindAsync(proiezione.SalaId);
        if (sala == null) return (null, "Sala non trovata.");
        int postiOccupati = await _contesto.Biglietti.Where(b => b.ProiezioneId == dto.ProiezioneId).SumAsync(b => b.NumeroBiglietti);
        if (postiOccupati + dto.NumeroBiglietti > sala.Capienza) return (null, "Posti insufficienti per la proiezione selezionata.");
        
        /*controlla che il numero di biglietti sia positivo e non superiore a 100*/
        if (dto.NumeroBiglietti <= 0 || dto.NumeroBiglietti > 100) return (null, "Il numero di biglietti deve essere compreso tra 1 e 100.");  

        /*controlla che l'utente abbia un saldo sufficiente*/
        if (utente.Saldo < Calcoli.CalcolaPrezzoFinale(movie.PrezzoMovie, tipologiaSala.MaggiorazionePrezzo, dto.NumeroBiglietti, utente, dto.MetodoPagamento))
            return (null, "Saldo insufficiente per acquistare i biglietti.");

        /*necessari per il calcolo del prezzo del biglietto*/
        var movie = await _contesto.Movies.FindAsync(proiezione.MovieId);
        var tipologiaSala = await _contesto.TipologieSala.FindAsync(sala.TipologiaSalaId);


        Biglietto biglietto = new Biglietto
        {
            UtenteId = utenteId,
            ProiezioneId = proiezione.Id,
            NumeroBiglietti = dto.NumeroBiglietti,
            OrarioCreazione = DateTimeOffset.UtcNow,
            MetodoPagamento = dto.MetodoPagamento,
            PrezzoFinale = Calcoli.CalcolaPrezzoFinale(movie.PrezzoMovie, tipologiaSala.MaggiorazionePrezzo, dto.NumeroBiglietti, utente, dto.MetodoPagamento)
        };


        _contesto.Biglietti.Add(biglietto);
        var contoCinema = await _contesto.ContoCinema.FirstOrDefaultAsync();
        var saldi = await Calcoli.CalcolaSaldo(biglietto.PrezzoFinale, utente,contoCinema);
        utente.Saldo = saldi[0]; //aggiorna il saldo dell'utente
        contoCinema.Conto = saldi[1];//aggiorna il saldo del cinema
        await _contesto.SaveChangesAsync();


        DtoBiglietto risultato = new DtoBiglietto
        {
            Id = biglietto.Id,
            ProiezioneId = biglietto.ProiezioneId,
            UtenteId = biglietto.UtenteId,
            NumeroBiglietti = biglietto.NumeroBiglietti,
            PrezzoFinale = biglietto.PrezzoFinale,
            OrarioCreazione = biglietto.OrarioCreazione,
            MetodoPagamento = biglietto.MetodoPagamento
        };

        return (risultato, null);
    }

    // Aggiorna la quantità di biglietti e ricalcola il prezzo di un biglietto esistente
    public async Task<(DtoBiglietto? Dto, string? Errore)> ModificaAsync(string id, DtoCreazioneBiglietto dto)
    {
        if (dto.NumeroBiglietti <= 0 || dto.NumeroBiglietti > 100)
            return (null, "Il numero di biglietti deve essere compreso tra 1 e 100.");

        var bigliettoEsistente = await _contesto.Biglietti.FindAsync(id);
        if (bigliettoEsistente == null) return (null, "Biglietto non trovato.");

        // Recupero entità per il ricalcolo del prezzo
        var proiezione = await _contesto.Proiezioni.FindAsync(dto.ProiezioneId);
        if (proiezione == null) return (null, "Proiezione non trovata.");

        var movie = await _contesto.Movies.FindAsync(proiezione.MovieId);
        var sala = await _contesto.Sale.FindAsync(proiezione.SalaId);
        var utente = await _contesto.Utenti.FindAsync(bigliettoEsistente.UtenteId);
        var tipologiaSala = await _contesto.TipologieSala.FindAsync(sala?.TipologiaSalaId);

        if (movie == null || sala == null || utente == null || tipologiaSala == null)
            return (null, "Dati correlati all'biglietto non trovati o non validi.");

        // Aggiornamento dei dati e ricalcolo
        bigliettoEsistente.NumeroBiglietti = dto.NumeroBiglietti;
        bigliettoEsistente.PrezzoFinale = Calcoli.CalcolaPrezzoFinale(movie.PrezzoMovie, tipologiaSala.MaggiorazionePrezzo, bigliettoEsistente.NumeroBiglietti, utente, bigliettoEsistente.MetodoPagamento);

        await _contesto.SaveChangesAsync();

        return (new DtoBiglietto {
            Id = bigliettoEsistente.Id,
            UtenteId = bigliettoEsistente.UtenteId,
            ProiezioneId = bigliettoEsistente.ProiezioneId,
            NumeroBiglietti = bigliettoEsistente.NumeroBiglietti,
            PrezzoFinale = bigliettoEsistente.PrezzoFinale,
            OrarioCreazione = bigliettoEsistente.OrarioCreazione,
            MetodoPagamento = bigliettoEsistente.MetodoPagamento
        }, null);
    }

    // Rimuove un biglietto dal database
    public async Task<(bool Successo, string? Errore)> EliminazioneAsync(string id)
    {
        //trova il biglietto da eliminare e controlla che esista
        var biglietto = await _contesto.Biglietti.FindAsync(id);
        if (biglietto == null) return (false, "Biglietto non trovato.");

        //prende l'utente relivo al biglietto
        var utente = await _contesto.Utenti.FindAsync(biglietto.UtenteId);
        //istanza del conto del cinema
        var contoCinema = await _contesto.ContoCinema.FirstOrDefaultAsync();

        //controlla che l'utente e il conto del cinema esistano
        if (utente == null || contoCinema == null)
        return (false, "Dati correlati all'biglietto non trovati.");

        //restituisce i crediti all'utende detraendoli dal conto del cinema    
        var saldi = await Calcoli.CalcolaSaldo(-biglietto.PrezzoFinale, utente, contoCinema);
        utente.Saldo = saldi[0];
        contoCinema.Conto = saldi[1];

        // rimiove il biglietto dal database e salva i cambiamenti
        _contesto.Biglietti.Remove(biglietto);
        await _contesto.SaveChangesAsync();
        return (true, null);
    }
}
```

## GenereMovieService.cs
```c#
// Namespace che contiene i servizi dell’applicazione
namespace NuovoCinemaParadiso.Services;

// Classe di servizio dedicata alla gestione dei generi dei film
public class GenereMovieService
{
    // Campo privato che rappresenta il contesto del database
    private readonly ContestoDb _contesto;

    // Costruttore che riceve il contesto tramite dependency injection
    public GenereMovieService(ContestoDb contesto)
    {
        // Salva il contesto nel campo privato
        _contesto = contesto;
    }

    // Metodo che restituisce tutti i generi in formato DTO
    public async Task<List<DtoGenereMovie>> OttieniTuttoAsync()
    {
        // Crea la lista dei DTO da restituire
        List<DtoGenereMovie> risultato = new List<DtoGenereMovie>();

        // Recupera tutti i generi dal database
        List<GenereMovie> generiMovies = await _contesto.GeneriMovies.ToListAsync();

        // Cicla su ogni genere trovato
        for (int i = 0; i < generiMovies.Count; i++)
        {
            // Genere corrente del ciclo
            GenereMovie genereCorrente = generiMovies[i];

            // Crea un DTO per il genere corrente
            DtoGenereMovie dto = new DtoGenereMovie();
            // Copia l'Id nel DTO
            dto.Id = genereCorrente.Id;
            // Copia il nome del genere nel DTO
            dto.Genere = genereCorrente.Genere;

            // Aggiunge il DTO alla lista finale
            risultato.Add(dto);
        }

        // Restituisce la lista completa dei generi
        return risultato;
    }

    // Metodo che restituisce un genere tramite Id
    public async Task<DtoGenereMovie?> OttieniTramiteIdAsync(string id)
    {
        // Cerca il genere nel database tramite Id
        GenereMovie? genereMovie = await _contesto.GeneriMovies.FindAsync(id);

        // Se non esiste, restituisce null
        if (genereMovie == null)
        {
            return null;
        }

        // Crea il DTO da restituire
        DtoGenereMovie risultato = new DtoGenereMovie();
        // Imposta l'Id nel DTO
        risultato.Id = genereMovie.Id;
        // Imposta il nome del genere nel DTO
        risultato.Genere = genereMovie.Genere;

        // Restituisce il DTO
        return risultato;
    }

                            //(PER FUTURE IMPLEMENTAZIONI, METODI DI GESTIONE)

    // Metodo che crea un nuovo genere
    public async Task<DtoGenereMovie?> CreazioneAsync(DtoCreazioneGenereMovie dto)
    {
        // Recupera tutti i generi esistenti
        List<GenereMovie> generiMovies = _contesto.GeneriMovies.ToList();

        // Controlla se esiste già un genere con lo stesso nome
        for (int i = 0; i < generiMovies.Count; i++)
        {
            // Genere corrente del ciclo
            GenereMovie genereMoviecorrente = generiMovies[i];

            // Confronta i nomi ignorando maiuscole/minuscole
            bool stessoNome = string.Equals(
                genereMoviecorrente.Genere,
                dto.Genere,
                StringComparison.OrdinalIgnoreCase);

            // Se esiste già, non crea nulla
            if (stessoNome)
            {
                return null;
            }
        }

        // Crea un nuovo oggetto GenereMovie
        GenereMovie genereMovie = new GenereMovie();
        // Imposta il nome del genere
        genereMovie.Genere = dto.Genere;

        // Aggiunge il nuovo genere al database
        _contesto.GeneriMovies.Add(genereMovie);
        // Salva le modifiche
        await _contesto.SaveChangesAsync();

        // Crea il DTO da restituire
        DtoGenereMovie risultato = new DtoGenereMovie();
        // Imposta l'Id generato
        risultato.Id = genereMovie.Id;
        // Imposta il nome del genere
        risultato.Genere = genereMovie.Genere;

        // Restituisce il DTO
        return risultato;
    }

    // Metodo che modifica un genere esistente
    public async Task<DtoGenereMovie?> ModificaAsync(string id, DtoCreazioneGenereMovie dto)
    {
        // Cerca il genere tramite Id
        GenereMovie? genereMovie = await _contesto.GeneriMovies.FindAsync(id);

        // Se non esiste, lancia eccezione personalizzata
        if (genereMovie == null)
        {
            throw new ItemNotFoundException("Genere");
        }

        // Recupera tutti i generi esistenti
        List<GenereMovie> generiMovies = _contesto.GeneriMovies.ToList();

        // Controlla se esiste già un genere con lo stesso nome
        for (int i = 0; i < generiMovies.Count; i++)
        {
            // Genere corrente del ciclo
            GenereMovie genereMoviecorrente = generiMovies[i];

            // Confronta i nomi ignorando maiuscole/minuscole
            bool stessoNome = string.Equals(
                genereMoviecorrente.Genere,
                dto.Genere,
                StringComparison.OrdinalIgnoreCase);

            // Se esiste già, lancia eccezione di modifica non valida
            if (stessoNome)
            {
                throw new ModificaException("genere");
            }
        }

        // Aggiorna il nome del genere
        genereMovie.Genere = dto.Genere;

        // Salva le modifiche nel database
        await _contesto.SaveChangesAsync();

        // Crea il DTO da restituire
        DtoGenereMovie risultato = new DtoGenereMovie();
        // Imposta l'Id
        risultato.Id = genereMovie.Id;
        // Imposta il nome aggiornato
        risultato.Genere = genereMovie.Genere;

        // Restituisce il DTO aggiornato
        return risultato;
    }

    // Metodo che elimina un genere tramite Id
    public async Task<bool> EliminaAsync(string id)
    {
        // Cerca il genere da eliminare
        GenereMovie? genereMovie = await _contesto.GeneriMovies.FindAsync(id);

        // Se non esiste, restituisce false
        if (genereMovie == null)
        {
            return false;
        }

        // Rimuove il genere dal database
        _contesto.GeneriMovies.Remove(genereMovie);
        // Salva le modifiche
        await _contesto.SaveChangesAsync();

        // Restituisce true per confermare l'eliminazione
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

```
## OperatoreService.cs
```c#
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NuovoCinemaParadiso.Data;
using NuovoCinemaParadiso.Dtos;
using NuovoCinemaParadiso.Models;
using NuovoCinemaParadiso.Helpers;
using NuovoCinemaParadiso.Exceptions;

namespace NuovoCinemaParadiso.Services;

// Servizio applicativo per le operazioni gestite dall'operatore
public class OperatoreService
{
    // Riferimento al DbContext per operazioni sul database
    private readonly ContestoDb _contesto;
    // Riferimento a UserManager per la gestione degli utenti
    private readonly UserManager<Utente> _gestioneUtenti;

    // Iniezione delle dipendenze tramite costruttore
    public OperatoreService(ContestoDb contestoDb, UserManager<Utente> gestioneUtenti)
    {
        _contesto = contestoDb;
        _gestioneUtenti = gestioneUtenti;
    }

    // Ricarica il saldo di un utente tramite la sua email
    public async Task<bool> RicaricaAsync(DtoRicarica dtoRicarica)
    {
        // Recupero dell'utente tramite email
        Utente? utente = await _gestioneUtenti.FindByEmailAsync(dtoRicarica.Email);
        
        if(utente == null)
            return false;
            
        // Aggiornamento del saldo
        utente.Saldo += dtoRicarica.Ricarica;
        
        // Salvataggio delle modifiche
        await _gestioneUtenti.UpdateAsync(utente);
        
        return true;
    }

    // Restituisce tutti gli utenti presenti nel sistema
    public async Task<List<DtoUtente>> OttieniUtentiAsync()
    {
        // Lettura completa della tabella Utenti
        List<Utente> utenti = await _contesto.Utenti.ToListAsync();

        List<DtoUtente> risultato = new List<DtoUtente>();

        // Mappatura manuale Utente → DTO
        for (int i = 0; i < utenti.Count; i++)
        {
            Utente utenteCorrente = utenti[i];

            // Recupero dei dati dell'abbonamento associato all'utente
            Abbonamento? abbonamento = await _contesto.Abbonamenti.FindAsync(utenteCorrente.AbbonamentoId);

            DtoUtente dto = new DtoUtente();
            dto.Id = utenteCorrente.Id;
            dto.Email = utenteCorrente.Email ?? string.Empty;
            dto.NomeCompleto = utenteCorrente.NomeCompleto ?? string.Empty;
            dto.Eta = utenteCorrente.Eta;
            dto.AbbonamentoId = utenteCorrente.AbbonamentoId ?? string.Empty;
            dto.SeAbbonato = utenteCorrente.SeAbbonato;
            dto.DataInizioAbbonamento = utenteCorrente.DataInizioAbbonamento;
            dto.TipoAbbonamento = abbonamento?.Nome ?? string.Empty;

            risultato.Add(dto);
        }

        return risultato;
    }

    // Restituisce un utente specifico tramite id
    public async Task<DtoUtente?> OttieniUtenteTramiteIdAsync(string id)
    {
        // Recupero utente tramite UserManager o lancio eccezione se non trovato
        Utente? utente = await _gestioneUtenti.FindByIdAsync(id)
            ?? throw new NotFoundException("Utente", id);
            
        // Recupero dei dati dell'abbonamento associato
        Abbonamento? abbonamento = await _contesto.Abbonamenti.FindAsync(utente.AbbonamentoId);
        
        if (utente == null)
        {
            throw new NotFoundException("Utente", id);
        }

        // Mappatura dell'entità trovata in DTO
        DtoUtente dto = new DtoUtente();
        dto.Id = utente.Id;
        dto.Email = utente.Email ?? string.Empty;
        dto.NomeCompleto = utente.NomeCompleto ?? string.Empty;
        dto.Eta = utente.Eta;
        dto.SeAbbonato = utente.SeAbbonato;
        dto.AbbonamentoId = utente.AbbonamentoId ?? string.Empty;
        dto.DataInizioAbbonamento = utente.DataInizioAbbonamento;
        dto.TipoAbbonamento = abbonamento?.Nome ?? string.Empty;

        return dto;
    }

    // Elimina un utente tramite id
    public async Task<IdentityResult> EliminaUtentePerIdAsync(string id)
    {
        // Recupero utente da eliminare
        Utente? utente = await _gestioneUtenti.FindByIdAsync(id);
        
        if (utente == null)
        {
            throw new NotFoundException("Utente", id);
        }
        
        // Eliminazione tramite UserManager
        IdentityResult risultato = await _gestioneUtenti.DeleteAsync(utente);

        return risultato;
    }

    // Restituisce tutti i biglietti presenti nel sistema con il prezzo calcolato
    public async Task<List<DtoBiglietto>> OttieniBiglietti()
    {
        // Lettura completa della tabella Biglietti
        List<Biglietto> biglietti = await _contesto.Biglietti.ToListAsync();

        List<DtoBiglietto> risultato = new List<DtoBiglietto>();

        // Elaborazione manuale Biglietto → DTO
        for (int i = 0; i < biglietti.Count; i++)
        {
            Biglietto bigliettoCorrente = biglietti[i];
            
            // Recupero di tutte le entità correlate necessarie per il calcolo
            Proiezione? proiezione = await _contesto.Proiezioni.FindAsync(bigliettoCorrente.ProiezioneId)
                ?? throw new NotFoundException("Proiezione", bigliettoCorrente.ProiezioneId);
            Movie? movie = await _contesto.Movies.FindAsync(proiezione.MovieId)
                ?? throw new NotFoundException("Movie", proiezione.MovieId);
            Sala? sala = await _contesto.Sale.FindAsync(proiezione.SalaId)
                ?? throw new NotFoundException("Sala", proiezione.SalaId);
            Utente? utente = await _contesto.Utenti.FindAsync(bigliettoCorrente.UtenteId)
                ?? throw new NotFoundException("Utente", bigliettoCorrente.UtenteId);
            TipologiaSala? tipologiaSala = await _contesto.TipologieSala.FindAsync(sala.TipologiaSalaId)
                ?? throw new NotFoundException("TipologiaSala", sala.TipologiaSalaId);

            // Costruzione del DTO con calcolo dinamico del prezzo finale
            DtoBiglietto dto = new DtoBiglietto();
            dto.Id = bigliettoCorrente.Id;
            dto.ProiezioneId = bigliettoCorrente.ProiezioneId;
            dto.UtenteId = bigliettoCorrente.UtenteId;
            dto.PrezzoFinale = Calcoli.CalcolaPrezzoFinale(movie.PrezzoMovie, tipologiaSala.MaggiorazionePrezzo, bigliettoCorrente.NumeroBiglietti, utente, bigliettoCorrente.MetodoPagamento);
            dto.OrarioCreazione = bigliettoCorrente.OrarioCreazione;
            dto.NumeroBiglietti = bigliettoCorrente.NumeroBiglietti;

            risultato.Add(dto);
        }

        return risultato;
    }

    // Restituisce un biglietto specifico tramite id
    public async Task<DtoBiglietto> OttieniBigliettoTramiteIdAsync(string id)
    {
        // Recupero biglietto tramite chiave primaria
        Biglietto? biglietto = await _contesto.Biglietti.FindAsync(id)
            ?? throw new NotFoundException("Biglietto", id);
            
        // Recupero di tutte le entità correlate necessarie per il calcolo
        Proiezione? proiezione = await _contesto.Proiezioni.FindAsync(biglietto.ProiezioneId)
            ?? throw new NotFoundException("Proiezione", biglietto.ProiezioneId);
        Movie? movie = await _contesto.Movies.FindAsync(proiezione.MovieId)
            ?? throw new NotFoundException("Movie", proiezione.MovieId);
        Sala? sala = await _contesto.Sale.FindAsync(proiezione.SalaId)
            ?? throw new NotFoundException("Sala", proiezione.SalaId);
        Utente? utente = await _contesto.Users.FindAsync(biglietto.UtenteId)
            ?? throw new NotFoundException("Utente", biglietto.UtenteId);
        TipologiaSala? tipologiaSala = await _contesto.TipologieSala.FindAsync(sala.TipologiaSalaId)
            ?? throw new NotFoundException("TipologiaSala", sala.TipologiaSalaId);

        if (biglietto == null)
        {
            throw new NotFoundException("Biglietto", id);
        }

        // Mappatura dell'entità in DTO con calcolo dinamico del prezzo finale
        DtoBiglietto dto = new DtoBiglietto();
        dto.Id = biglietto.Id;
        dto.UtenteId = biglietto.UtenteId;
        dto.ProiezioneId = biglietto.ProiezioneId;
        dto.PrezzoFinale = Calcoli.CalcolaPrezzoFinale(movie.PrezzoMovie, tipologiaSala.MaggiorazionePrezzo, biglietto.NumeroBiglietti, utente, biglietto.MetodoPagamento);
        dto.OrarioCreazione = biglietto.OrarioCreazione;
        dto.NumeroBiglietti = biglietto.NumeroBiglietti;

        return dto;
    }

    // Restituisce tutti gli utenti associati a uno specifico abbonamento
    public async Task<List<DtoUtente>> OttieniUtentiTramiteAbbonamentoAsync(string abbonamentoId)
    {
        // Lettura delle tabelle necessarie
        List<Utente> utenti = await _contesto.Utenti.ToListAsync();
        List<Abbonamento> abbonamenti = await _contesto.Abbonamenti.ToListAsync();

        Abbonamento? abbonamentoTrovato = null;

        // Ricerca dell'abbonamento richiesto
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

        // Filtraggio e mappatura degli utenti che possiedono l'abbonamento trovato
        for (int i = 0; i < utenti.Count; i++)
        {
            Utente utenteCorrente = utenti[i];
            Abbonamento? abbonamento = await _contesto.Abbonamenti.FindAsync(utenteCorrente.AbbonamentoId);

            if (utenteCorrente.Abbonamento == abbonamentoTrovato)
            {
                DtoUtente dto = new DtoUtente();
                dto.Id = utenteCorrente.Id;
                dto.NomeCompleto = utenteCorrente.NomeCompleto;
                dto.Email = utenteCorrente.Email ?? string.Empty;
                dto.Eta = utenteCorrente.Eta;
                dto.SeAbbonato = utenteCorrente.SeAbbonato;
                dto.AbbonamentoId = utenteCorrente.AbbonamentoId ?? string.Empty;
                dto.DataInizioAbbonamento = utenteCorrente.DataInizioAbbonamento;
                dto.TipoAbbonamento = abbonamento?.Nome ?? string.Empty;

                risultato.Add(dto);
            }
        }

        return risultato;
    }

    // Crea una nuova GiftCard pre-ricaricata da consegnare al cliente in cassa
    public async Task<DtoGiftCard> RicaricaGiftCardAsync(DtoRicaricaGiftCard dto)
    {
        // Validazione dell'importo in ingresso
        if (dto.Importo <= 0)
        {
            throw new Exception("Impossibile ricaricare la giftcard. Importo non valido.");
        }

        // L'operatore crea la GiftCard "dal nulla", senza scalare un saldo, 
        // perché si presume che il pagamento sia stato gestito in cassa.
        GiftCard nuovaGiftCard = new GiftCard()
        {
            Nome = "GiftCard",
            Valore = dto.Importo,
            CodiceRiscatto = GiftCardHelper.GeneraCodice()
        };
        
        // Salvataggio nel database
        await _contesto.GiftCards.AddAsync(nuovaGiftCard);
        await _contesto.SaveChangesAsync();

        // Restituzione dei dati della GiftCard appena creata in formato DTO
        return new DtoGiftCard()
        {
            Id = nuovaGiftCard.Id,
            Nome = nuovaGiftCard.Nome,
            Valore = nuovaGiftCard.Valore,
            CodiceRiscatto = nuovaGiftCard.CodiceRiscatto
        };
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
    /// <summary>
    /// Restituisce il saldo dell'utente e il saldo del cinema aggiornato,
    ///  se il prezzo inserito è negativo si posso restituire i crediti agli utenti 
    /// </summary>
    public static async Task<int[]> CalcolaSaldo(int prezzo, Utente utente, ContoCinema contoCinema)
    {
        utente.Saldo = utente.Saldo - prezzo;
        contoCinema.Conto = contoCinema.Conto + prezzo;
        return new int[] { utente.Saldo, contoCinema.Conto };
    }
}
```

# Seed

## DataSeeder.cs

```c#
using System.ComponentModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NuovoCinemaParadiso.Data;
using NuovoCinemaParadiso.Models;

namespace NuovoCinemaParadiso.Seed;

public static class DataSeeder
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        using IServiceScope scope = serviceProvider.CreateScope();

        DateTime oggi = DateTime.Today;
        ContestoDb contestoDb = scope.ServiceProvider.GetRequiredService<ContestoDb>();
        UserManager<Utente> gestioneUtenti = scope.ServiceProvider.GetRequiredService<UserManager<Utente>>();
        RoleManager<IdentityRole> gestioneRuoli = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        await AssicuraEsistenzaRuoloAsync(gestioneRuoli, Ruoli.Gestore);
        await AssicuraEsistenzaRuoloAsync(gestioneRuoli, Ruoli.Operatore);
        await AssicuraEsistenzaRuoloAsync(gestioneRuoli, Ruoli.Utente);

        Utente gestore = await AssicuraEsistenzaUtenteAsync(
            gestioneUtenti,
            "gestore@gmail.com",
            "123456",
            "Gestore",
            60
            , false);

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

        await ImpostaRuoloUnicoAsync(gestioneUtenti, gestore, Ruoli.Gestore);
        await ImpostaRuoloUnicoAsync(gestioneUtenti, operatore, Ruoli.Operatore);
        await ImpostaRuoloUnicoAsync(gestioneUtenti, utente, Ruoli.Utente);

        var genereAzione = await AssicuraEsistenzaGenereMovie(contestoDb, "Azione");
        var genereHorror = await AssicuraEsistenzaGenereMovie(contestoDb, "Horror");
        var genereCommedia = await AssicuraEsistenzaGenereMovie(contestoDb, "Commedia");

        var movie1 = await AssicuraEsistenzaMovie(contestoDb, "Movie1", "Film del drago", 60, 10, genereAzione.Id);
        var movie2 = await AssicuraEsistenzaMovie(contestoDb, "Movie2", "Film del lupo", 80, 12, genereHorror.Id);
        var movie3 = await AssicuraEsistenzaMovie(contestoDb, "Movie3", "Film del cane", 100, 14, genereCommedia.Id);

        var tipologia2D = await AssicuraEsistenzaTipologiaSala(contestoDb, "2D", 2);
        var tipologia3D = await AssicuraEsistenzaTipologiaSala(contestoDb, "3D", 3);
        var tipologiaImax = await AssicuraEsistenzaTipologiaSala(contestoDb, "IMAX", 4);

        var turnoMattina = await AssicuraEsistenzaTurno(contestoDb, new TimeOnly(10, 0, 0), new TimeOnly(13, 0, 0), "Mattina");
        var turnoPomeriggio = await AssicuraEsistenzaTurno(contestoDb, new TimeOnly(13, 0, 0), new TimeOnly(18, 0, 0), "Pomeriggio");
        var turnoSera = await AssicuraEsistenzaTurno(contestoDb, new TimeOnly(18, 0, 0), new TimeOnly(22, 0, 0), "Sera");

        var sala1 = await AssicuraEsistenzaSala(contestoDb, "Sala1", 30, tipologia2D.Id);
        var sala2 = await AssicuraEsistenzaSala(contestoDb, "Sala2", 40, tipologia2D.Id);
        var sala3 = await AssicuraEsistenzaSala(contestoDb, "Sala3", 50, tipologia2D.Id);

        await AssicuraEsistenzaAbbonamento(contestoDb, "Mensile", 70, 25, 1);
        await AssicuraEsistenzaAbbonamento(contestoDb, "Semestrale", 210, 50, 6);
        await AssicuraEsistenzaAbbonamento(contestoDb, "Annuale", 300, 75, 12);

        await AssicuraEsistenzaGiftCard(contestoDb, "10 Film", 85, 10, 12);
        await AssicuraEsistenzaGiftCard(contestoDb, "25 Film", 190, 25, 12);
        await AssicuraEsistenzaGiftCard(contestoDb, "50 Film", 325, 50, 12);

        var proiezione1 = await AssicuraEsistenzaProiezione(contestoDb, new DateOnly(2027, 1, 1), movie1.Id, sala1.Id, turnoMattina.Id);
        var proiezione2 = await AssicuraEsistenzaProiezione(contestoDb, new DateOnly(2027, 1, 1), movie2.Id, sala2.Id, turnoPomeriggio.Id);
        var proiezione3 = await AssicuraEsistenzaProiezione(contestoDb, new DateOnly(2027, 1, 1), movie3.Id, sala3.Id, turnoSera.Id);

        await AssicuraEsistenzaBiglietto(contestoDb, proiezione1.Id, utente.Id, 1, new DateTimeOffset(DateTime.Now), 10, "standard");
        await AssicuraEsistenzaBiglietto(contestoDb, proiezione2.Id, utente.Id, 2, new DateTimeOffset(DateTime.Now), 20, "abbonamento");
        await AssicuraEsistenzaBiglietto(contestoDb, proiezione3.Id, utente.Id, 3, new DateTimeOffset(DateTime.Now), 30, "giftcard");

        await AssicuraEsistenzaContoCinema(contestoDb, "IT60X0542811101000000123456", "Gestore", 200);
    }

    private static async Task AssicuraEsistenzaRuoloAsync(RoleManager<IdentityRole> managerRuolo, string nomeRuolo)
    {
        bool seEsiste = await managerRuolo.RoleExistsAsync(nomeRuolo);
        if (!seEsiste)
        {
            IdentityRole ruolo = new IdentityRole();
            ruolo.Name = nomeRuolo;

            await managerRuolo.CreateAsync(ruolo);
        }
    }

    private static async Task<Utente> AssicuraEsistenzaUtenteAsync(
        UserManager<Utente> gestioneUtenti,
        string email,
        string password,
        string nomeCompleto,
        int eta,
        bool abbonato
        )
    {
        Utente? utenteEsistente = await gestioneUtenti.FindByEmailAsync(email);

        if (utenteEsistente != null)
        {
            return utenteEsistente;
        }

        Utente utente = new Utente();
        utente.UserName = email;
        utente.Email = email;
        utente.NomeCompleto = nomeCompleto;
        utente.Eta = eta;
        utente.SeAbbonato = abbonato;
        utente.AbbonamentoId = null;

        IdentityResult risultato = await gestioneUtenti.CreateAsync(utente, password);

        if (!risultato.Succeeded)
        {
            List<string> errori = new List<string>();

            foreach (IdentityError errore in risultato.Errors)
            {
                errori.Add(errore.Description);
            }
            string messaggio = string.Join("|", errori);
            throw new Exception($"Errore durante il seed dell'utente {email} : {messaggio}");
        }
        return utente;
    }

    private static async Task ImpostaRuoloUnicoAsync(UserManager<Utente> gestioneUtenti, Utente utente, string ruoloTarget)
    {
        IList<string> ruoliCorrenti = await gestioneUtenti.GetRolesAsync(utente);

        for (int i = 0; i < ruoliCorrenti.Count; i++)
        {
            string ruoloCorrente = ruoliCorrenti[i];

            if (ruoloCorrente == Ruoli.Gestore || ruoloCorrente == Ruoli.Operatore || ruoloCorrente == Ruoli.Utente)
            {
                await gestioneUtenti.RemoveFromRoleAsync(utente, ruoloCorrente);
            }
        }
        bool alreadyInTargetRole = await gestioneUtenti.IsInRoleAsync(utente, ruoloTarget);

        if (!alreadyInTargetRole)
        {
            await gestioneUtenti.AddToRoleAsync(utente, ruoloTarget);
        }
    }

    private static async Task<GenereMovie> AssicuraEsistenzaGenereMovie(
    ContestoDb context,
    string genere)
    {
        List<GenereMovie> generiMovies = await context.GeneriMovies.ToListAsync();
        for (int i = 0; i < generiMovies.Count; i++)
        {
            GenereMovie genereCorrente = generiMovies[i];
            bool nomeUguale = string.Equals(
                genereCorrente.Genere,
                genere,
                StringComparison.OrdinalIgnoreCase);
            if (nomeUguale)
            {
                return genereCorrente;
            }
        }

        GenereMovie nuovoGenere = new GenereMovie
        {
            Genere = genere
        };

        context.GeneriMovies.Add(nuovoGenere);
        await context.SaveChangesAsync();

        return nuovoGenere;
    }

    private static async Task<Movie> AssicuraEsistenzaMovie(
    ContestoDb context,
    string titolo,
    string descrizione,
    int durataMinuti,
    decimal prezzoMovie,
    string genereId)
    {
        List<Movie> movies = await context.Movies.ToListAsync();
        for (int i = 0; i < movies.Count; i++)
        {
            Movie movieCorrente = movies[i];
            bool nomeUguale = string.Equals(
                movieCorrente.Titolo,
                titolo,
                StringComparison.OrdinalIgnoreCase);
            if (nomeUguale)
            {
                return movieCorrente;
            }
        }

        Movie nuovoMovie = new Movie
        {
            Titolo = titolo,
            Descrizione = descrizione,
            DurataMinuti = durataMinuti,
            PrezzoMovie = prezzoMovie,
            GenereId = genereId,
        };

        context.Movies.Add(nuovoMovie);
        await context.SaveChangesAsync();

        return nuovoMovie;
    }

    private static async Task<TipologiaSala> AssicuraEsistenzaTipologiaSala(
     ContestoDb context,
     string nome, decimal maggiorazioneprezzo)
    {
        List<TipologiaSala> tipologieSala = await context.TipologieSala.ToListAsync();
        for (int i = 0; i < tipologieSala.Count; i++)
        {
            TipologiaSala tipologiaCorrente = tipologieSala[i];
            bool nomeUguale = string.Equals(
                tipologiaCorrente.Nome,
                nome,
                StringComparison.OrdinalIgnoreCase);
            if (nomeUguale)
            {
                return tipologiaCorrente;
            }
        }

        TipologiaSala nuovaTipologia = new TipologiaSala
        {
            Nome = nome,
            MaggiorazionePrezzo = maggiorazioneprezzo,
        };

        context.TipologieSala.Add(nuovaTipologia);
        await context.SaveChangesAsync();

        return nuovaTipologia;
    }

    private static async Task<Sala> AssicuraEsistenzaSala(
        ContestoDb context,
        string nome,
        int capienza,
        string tipologiaSalaId)
    {
        List<Sala> sale = await context.Sale.ToListAsync();
        for (int i = 0; i < sale.Count; i++)
        {
            Sala salaCorrente = sale[i];
            bool nomeUguale = string.Equals(
                salaCorrente.Nome,
                nome,
                StringComparison.OrdinalIgnoreCase);
            if (nomeUguale)
            {
                return salaCorrente;
            }
        }

        Sala nuovaSala = new Sala
        {
            Nome = nome,
            Capienza = capienza,
            TipologiaSalaId = tipologiaSalaId,
        };

        context.Sale.Add(nuovaSala);
        await context.SaveChangesAsync();

        return nuovaSala;
    }

    private static async Task<Turno> AssicuraEsistenzaTurno(
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
            if (nomeUguale || (turnoCorrente.OraInizio == oraInizio && turnoCorrente.OraFine == oraFine))
            {
                return turnoCorrente;
            }
        }

        Turno nuovoTurno = new Turno
        {
            Nome = nome,
            OraInizio = oraInizio,
            OraFine = oraFine
        };

        context.Turni.Add(nuovoTurno);
        await context.SaveChangesAsync();

        return nuovoTurno;
    }

    private static async Task AssicuraEsistenzaAbbonamento(ContestoDb context, string nome, decimal prezzo, int sconto, int durata)
    {
        List<Abbonamento> abbonamenti = await context.Abbonamenti.ToListAsync();
        for (int i = 0; i < abbonamenti.Count; i++)
        {
            Abbonamento abbonamentoCorrente = abbonamenti[i];
            bool nomeUguale = string.Equals(
                abbonamentoCorrente.Nome,
                nome,
                StringComparison.OrdinalIgnoreCase);
            if (nomeUguale)
            {
                return;
            }

        }

        Abbonamento nuovoAbbonamento = new Abbonamento
        {
            Nome = nome,
            Prezzo = prezzo,
            Sconto = sconto,
            Durata = durata
        };

        context.Abbonamenti.Add(nuovoAbbonamento);
        await context.SaveChangesAsync();
    }
    private static async Task AssicuraEsistenzaGiftCard(ContestoDb context, string nome, decimal prezzo, int numeroMovie, int durata)
    {
        List<GiftCard> giftCards = await context.GiftCards.ToListAsync();
        for (int i = 0; i < giftCards.Count; i++)
        {
            GiftCard giftCardCorrente = giftCards[i];
            bool nomeUguale = string.Equals(
                giftCardCorrente.Nome,
                nome,
                StringComparison.OrdinalIgnoreCase);
            if (nomeUguale)
            {
                return;
            }
        }

        GiftCard nuovaGiftCard = new GiftCard
        {
            Nome = nome,
            Prezzo = prezzo,
            NumeroMovie = numeroMovie,
            Durata = durata
        };

        context.GiftCards.Add(nuovaGiftCard);
        await context.SaveChangesAsync();
    }

    

    private static async Task<Proiezione> AssicuraEsistenzaProiezione(
    ContestoDb context,
    DateOnly dataProiezione,
    string movieId,
    string salaId,
    string turnoId)
    {
        List<Proiezione> proiezioni = await context.Proiezioni.ToListAsync();
        for (int i = 0; i < proiezioni.Count; i++)
        {
            Proiezione proiezione = proiezioni[i];

            bool stessaSala = proiezione.SalaId == salaId;
            bool stessoMovie = proiezione.MovieId == movieId;
            bool stessoTurno = proiezione.TurnoId == turnoId;

            if (stessaSala && stessoMovie && stessoTurno)
            {
                return proiezione;
            }
        }

        Proiezione nuovaProiezione = new Proiezione
        {
            DataProiezione = dataProiezione,
            MovieId = movieId,
            SalaId = salaId,
            TurnoId = turnoId,
            Attivo=true
        };

        context.Proiezioni.Add(nuovaProiezione);
        await context.SaveChangesAsync();

        return nuovaProiezione;
    }

    private static async Task AssicuraEsistenzaBiglietto(
    ContestoDb context,
    string proiezioneId,
    string utenteId,
    int numeroBiglietti,
    DateTimeOffset orarioCreazione,
    decimal prezzoFinale,
    string metodoPagamento)
    {
        List<Biglietto> biglietti = await context.Biglietti.ToListAsync();

        Biglietto nuovoBiglietto = new Biglietto
        {
            ProiezioneId = proiezioneId,
            UtenteId = utenteId,
            NumeroBiglietti = numeroBiglietti,
            OrarioCreazione = orarioCreazione,
            PrezzoFinale = prezzoFinale,
            MetodoPagamento = metodoPagamento,
        };

        context.Biglietti.Add(nuovoBiglietto);
        await context.SaveChangesAsync();
    }

    private static async Task AssicuraEsistenzaContoCinema(ContestoDb context, string iban, string titolareConto, int conto)
    {
        List <ContoCinema> ContiCinema = await context.ContoCinema.ToListAsync();

        ContoCinema nuovoContoCinema = new ContoCinema
        {
            Iban = iban,
            TitolareConto = titolareConto,
            Conto = conto
        };

        context.ContoCinema.Add(nuovoContoCinema);
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
builder.Services.AddScoped<BigliettoService>();
builder.Services.AddScoped<RuoloUtenteService>(); // Gestione ruoli utenti
builder.Services.AddScoped<JwtHelper>();
builder.Services.AddScoped<LogAzioniService>();
builder.Services.AddScoped<UtenteService>();
builder.Services.AddScoped<GestoreService>();
builder.Services.AddScoped<AbbonamentoService>();
builder.Services.AddScoped<ProiezioneService>();
builder.Services.AddScoped<GiftCardService>();
builder.Services.AddScoped<OperatoreService>();

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
using System.ComponentModel.DataAnnotations;
using NuovoCinemaParadiso.Exceptions;

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
        // Tentativo di effettuare una registrazione
        try
        {
            // Chiama il servizio di autenticazione per eseguire la registrazione
            IdentityResult result = await _authService.RegistrazioneAsync(dto);
                // Se la registrazione non è andata a buon fine
            if (!result.Succeeded)
            {   // Salva un log di registrazione fallita
                await _logAzioniService.SalvataggioLogAzioneAsync(null, "Registrazione utente", false);
                // Restituisce 400 BadRequest con il messaggio di errore
                return BadRequest(result.Errors);
            }
            // Salva un log di registrazione avvenuta con successo
            await _logAzioniService.SalvataggioLogAzioneAsync(null, "Registrazione utente", true);
                // Restituisce 200 OK come risposta dell'avvenuta registrazione
            return Ok(new { messaggio = "Registrazione avvenuta con successo!" });

        }
        // Gestione dell'errore nel caso l'email inserita non sia standard
        catch (InvalidEmail ex)
        {
            // Salva un log di registrazione fallita
            await _logAzioniService.SalvataggioLogAzioneAsync(null, "Registrazione utente", false);
            // Restituisce 400 BadRequest con il messaggio di errore
            return BadRequest(new { errore = ex.Message });
        }
    }

    [HttpPost("login")]
    // Metodo asincrono per eseguire il login
    public async Task<IActionResult> Login([FromBody] DtoLogin dto)
    {
        // Tentativo di effettuare il login
        try
        {
            // Chiama il servizio di autenticazione per eseguire il login
            DtoAuthResponse? risposta = await _authService.LoginAsync(dto);
            // Salva un log del login avvenuto con successo
            await _logAzioniService.SalvataggioLogAzioneAsync(risposta.Id, "Login", true);
            // Restituisce 200 OK come risposta dell'avvenuta registrazione
            return Ok(risposta);
        }
        // Gestione dell'errore in caso non si trovasse l'utente
        catch (NotFoundException ex)
        {
            // Salva un log del login fallito
            await _logAzioniService.SalvataggioLogAzioneAsync(null, "Login", false);
            // Restituisce 404 NotFound con il messaggio di errore
            return NotFound(new { messaggio = ex.Message });
        }
        // Gestione dell'errore nel caso la password sia sbagliata
        catch (ConflictException ex)
        {
            // Salva un log del login fallito
            await _logAzioniService.SalvataggioLogAzioneAsync(null, "Login", false);
            // Restituisce 400 BadRequest con il messaggio di errore
            return BadRequest(new { messaggio = ex.Message });
        }
    }

    [HttpGet("profilo")]
    // Metodo asincrono per ottenere il profilo dell'utente loggato
    public async Task<IActionResult> RicercaProfiloLoggato()
    {
        // Recupera l'Id utente dai claim del token JWT
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        // Verifica se l'utente è autenticato
        if (utenteId == null)
            // Restituisce 401 Unauthorized con il messaggio di errore
            return Unauthorized("Utente non autenticato.");
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
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        // Verifica se l'utente è autenticato
        if (utenteId == null)
            // Restituisce 401 Unauthorized con il messaggio di errore
            return Unauthorized("Utente non autenticato.");
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
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        // Verifica se l'utente è autenticato
        if (utenteId == null)
            // Restituisce 401 Unauthorized con il messaggio di errore
            return Unauthorized("Utente non autenticato.");
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

                         //(IMPLEMENTAZIONI FUTURE, METODI DI GESTIONE) 


    // Endpoint POST: crea un nuovo genere
    [HttpPost]
    [Authorize(Roles = Ruoli.Operatore)]
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
    [Authorize(Roles = Ruoli.Operatore)]
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
    [Authorize(Roles = Ruoli.Operatore)]
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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using NuovoCinemaParadiso.Services;
using NuovoCinemaParadiso.Dtos;
using NuovoCinemaParadiso.Exceptions;

namespace NuovoCinemaParadiso.Controllers;

// Controller per esporre le API destinate agli utenti finali
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UtenteController : ControllerBase
{
    // Servizi necessari per la logica di business e il tracciamento delle azioni
    private readonly UtenteService _utenteService;
    private readonly LogAzioniService _logAzioniService;

    // Iniezione tramite costruttore
    public UtenteController(UtenteService utenteService, LogAzioniService logAzioniService)
    {
        _utenteService = utenteService;
        _logAzioniService = logAzioniService;
    }

    // Endpoint per la sottoscrizione a un abbonamento
    [HttpPost("abbonati")]
    public async Task<IActionResult> Abbonati([FromBody] DtoUtente dto)
    {
        // Recupero ID utente autenticato tramite i Claims del token
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");

        // Controllo validità input
        if (dto == null || string.IsNullOrEmpty(dto.AbbonamentoId))
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Abbonati", false);
            return BadRequest("Dati non validi");
        }
        try
        {
            // Chiamata al service per effettuare l'abbonamento
            var risultato = await _utenteService.AbbonatiAsync(dto.AbbonamentoId, utenteId);
            
            // Salvataggio log di successo
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Abbonati", true);
            return Ok(risultato);
        }
        catch (NotFoundException ex)
        {
            // Salvataggio log di fallimento
            await _logAzioniService.SalvataggioLogAzioneAsync(dto.AbbonamentoId, "Abbonati", false);
            return NotFound(new { errore = ex.Message });
        }
        catch (ItemAlredyexist ex)
        {
            // Salvataggio log di fallimento
            await _logAzioniService.SalvataggioLogAzioneAsync(dto.AbbonamentoId, "Abbonati", false);
            return NotFound(new { errore = ex.Message });
        }
    }

    // Endpoint per incassare una GiftCard
    [HttpPut("giftCard/riscatta")]
    public async Task<IActionResult> RiscattaGiftCard(
    [FromBody] string giftCardCodiceRiscatto)
    {
        // Recupero ID utente autenticato
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        // Controllo autenticazione
        if (utenteId == null)
        {
            return Unauthorized("Utente non autenticato.");
        }

        // Controllo validità codice
        if (string.IsNullOrEmpty(giftCardCodiceRiscatto))
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(
                utenteId,
                "RiscattoGiftCard",
                false);

            return BadRequest("Codice riscatto non valido.");
        }

        try
        {
            // Chiamata al service
            DtoGiftCard risultato =
                await _utenteService.RiscattaGiftCardAsync(
                    giftCardCodiceRiscatto,
                    utenteId);

            // Salvataggio log successo
            await _logAzioniService.SalvataggioLogAzioneAsync(
                utenteId,
                "RiscattoGiftCard",
                true);

            // Restituzione risultato
            return Ok(risultato);
        }
        catch (NotFoundException ex)
        {
            // Log errore
            await _logAzioniService.SalvataggioLogAzioneAsync(
                utenteId,
                "RiscattoGiftCard",
                false);

            return NotFound(new { errore = ex.Message });
        }
        catch (Exception ex)
        {
            // Log errore generico
            await _logAzioniService.SalvataggioLogAzioneAsync(
                utenteId,
                "RiscattoGiftCard",
                false);

            return BadRequest(new { errore = ex.Message });
        }
    }

    // Endpoint per acquistare/generare una GiftCard scalando dal saldo dell'utente
    [HttpPost("giftCard/ricarica")]
    public async Task<IActionResult> RicaricaGiftCard([FromBody] DtoRicaricaGiftCard dto)
    {
        // Recupero ID utente autenticato
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        // Controllo autenticazione
        if (utenteId == null)
        {
            return Unauthorized("Utente non autenticato.");
        }

        // Controllo validità dell'importo inserito
        if (dto == null || dto.Importo <= 0)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "RicaricaGiftCard", false);
            return BadRequest(new { errore = "L'importo della ricarica deve essere maggiore di zero." });
        }

        try
        {
            // Chiamata al service per la generazione della GiftCard
            var risultato = await _utenteService.RicaricaGiftCardAsync(utenteId, dto);

            // Log del successo
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "RicaricaGiftCard", true);
            
            return Ok(risultato);
        }
        catch (Exception ex)
        {
            // Log del fallimento
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "RicaricaGiftCard", false);
            return BadRequest(new { errore = ex.Message });
        }
    }
}
```

## OperatoreController.cs

```c#
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using NuovoCinemaParadiso.Services;
using NuovoCinemaParadiso.Dtos;

namespace NuovoCinemaParadiso.Controllers;

// Controller per esporre le API destinate agli operatori del cinema
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class OperatoreController : ControllerBase
{
    // Servizi per operazioni di backoffice e logging
    private readonly OperatoreService _operatoreService;
    private readonly LogAzioniService _logAzioniService;

    // Iniezione tramite costruttore
    public OperatoreController(OperatoreService operatoreService, LogAzioniService logAzioniService)
    {
        _operatoreService = operatoreService;
        _logAzioniService = logAzioniService;
    }

    // Endpoint per ricaricare il saldo di un utente
    [HttpPost("ricarica")]
    [Authorize(Roles = Ruoli.Operatore)] // Solo gli operatori possono accedere
    public async Task<IActionResult> Ricarica([FromBody] DtoRicarica dtoRicarica)
    {
        // Recupero ID operatore per tracciare l'azione
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");
            
        // Chiamata al service per gestire la ricarica
        bool successo = await _operatoreService.RicaricaAsync(dtoRicarica);
        if (!successo)        
        {
            // Log fallimento
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, $"Ricarica fallita per {dtoRicarica.Email}", false);
            return NotFound($"Utente con email {dtoRicarica.Email} non trovato.");
        }
        
        // Log successo
        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, $"Ricarica riuscita per {dtoRicarica.Email}", true);

        return Ok("Ricarica effettuata con successo.");
    }

    // Endpoint per ottenere l'elenco di tutti gli utenti registrati
    [HttpGet("listaUtenti")]
    [Authorize(Roles = Ruoli.Operatore)]
    public async Task<IActionResult> OttieniTuttiIProfili()
    {
        // Chiamata al service
        List<DtoUtente> utenti = await _operatoreService.OttieniUtentiAsync();

        // Recupero ID operatore per il logging
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");

        // Log delle operazioni
        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ricerca profili", true);
        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ottieni tutti i profili", true);

        return Ok(utenti);
    }

    // Endpoint per cercare un utente specifico tramite il suo ID
    [HttpGet("ricercaProfilo/{id}")]
    [Authorize(Roles = Ruoli.Operatore)]
    public async Task<IActionResult> RicercaProfiloTramiteId(string id)
    {
        // Recupero ID operatore per il logging
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");
            
        try
        {
            // Chiamata al service
            DtoUtente? utente = await _operatoreService.OttieniUtenteTramiteIdAsync(id);
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ricerca profilo", true);
            return Ok(utente);
        }
        catch
        {
            // Log in caso di mancato ritrovamento
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ricerca profilo", false);
            return NotFound(new { messaggio = "Utente non trovato." });
        }
    }

    // Endpoint per eliminare un utente dal sistema
    [HttpDelete("eliminaUtente/{id}")]
    [Authorize(Roles = Ruoli.Operatore)]
    public async Task<IActionResult> EliminaTramiteId(string Id)
    {
        // Recupero ID operatore
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");
            
        try
        {
            // Chiamata al service
            var risultato = await _operatoreService.EliminaUtentePerIdAsync(Id);
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Eliminazione profilo", true);
            return Ok(risultato);
        }
        catch
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Eliminazione profilo", false);
            return NotFound(new { messaggio = "Utente non trovato." });
        }
    }

    // Endpoint per ottenere tutti i biglietti venduti
    [HttpGet("biglietto")]
    [Authorize(Roles = Ruoli.Operatore)]
    public async Task<IActionResult> OttieniTuttiGliBiglietti()
    {
        // Chiamata al service
        List<DtoBiglietto> biglietti = await _operatoreService.OttieniBiglietti();

        // Recupero ID operatore per il logging
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");

        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ottieni tutti gli biglietti gestore", true);
        return Ok(biglietti);
    }

    // Endpoint per visualizzare il dettaglio di un singolo biglietto tramite ID
    [HttpGet("biglietto/{id}")]
    [Authorize(Roles = Ruoli.Operatore)]
    public async Task<IActionResult> OttieniBigliettoTramiteId(string id)
    {
        // Recupero ID operatore per il logging
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");
            
        try
        {
            // Chiamata al service
            var risultato = await _operatoreService.OttieniBigliettoTramiteIdAsync(id);
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ottieni biglietti tramite id gestore", true);
            return Ok(risultato);
        }
        catch
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ottieni biglietti tramite id gestore", false);
            return NotFound($"Biglietto con id {id} non trovato");
        }
    }

    // Endpoint per filtrare gli utenti iscritti a uno specifico abbonamento
    [HttpGet("utenti/abbonamento/{abbonamentoId}")]
    [Authorize(Roles = Ruoli.Operatore)]
    public async Task<ActionResult<List<DtoUtente>>> OttieniUtentiTramiteAbbonamentoAsync(string abbonamentoId)
    {
        // Recupero ID operatore
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");

        // Validazione input
        if (string.IsNullOrWhiteSpace(abbonamentoId))
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ottieni gli utenti per abbonamento", false);
            return BadRequest("AbbonamentoId non valido");
        }

        // Chiamata al service
        var risultato = await _operatoreService.OttieniUtentiTramiteAbbonamentoAsync(abbonamentoId);

        // Gestione caso in cui non ci siano risultati
        if (risultato == null || risultato.Count == 0)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ottieni gli utenti per abbonamento", false);
            return NotFound("Nessun utente trovato per questo abbonamento");
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ottieni gli utenti per abbonamento", true);
        return Ok(risultato);
    }

    // Endpoint utilizzato dall'operatore per generare una GiftCard (es. pagamento in contanti in cassa)
    [HttpPost("giftCard/ricarica")]
    [Authorize(Roles = Ruoli.Operatore)]
    public async Task<IActionResult> RicaricaGiftCard([FromBody] DtoRicaricaGiftCard dto)
    {
        // Recupero ID operatore
        string? operatoreId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        // Controllo autenticazione
        if (operatoreId == null)
        {
            return Unauthorized("Utente non autenticato.");
        }

        // Validazione importo in ingresso
        if (dto == null || dto.Importo <= 0)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(operatoreId, "RicaricaGiftCard", false);
            return BadRequest(new { errore = "L'importo della Gift Card deve essere maggiore di zero." });
        }

        try
        {
            // Chiamata al service (creazione entità da parte dell'operatore)
            var risultato = await _operatoreService.RicaricaGiftCardAsync(dto);

            // Log di successo
            await _logAzioniService.SalvataggioLogAzioneAsync(operatoreId, "RicaricaGiftCard", true);
            
            return Ok(risultato);
        }
        catch (Exception ex)
        {
            // Log di errore
            await _logAzioniService.SalvataggioLogAzioneAsync(operatoreId, "RicaricaGiftCard", false);
            return BadRequest(new { errore = ex.Message });
        }
    }
}
```

## ContoCinemaController.cs

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
// Importa i servizi per gestire le ecezzioni
using NuovoCinemaParadiso.Exceptions;

namespace NuovoCinemaParadiso.Controllers;

// Indica che è un controller API 
[ApiController]
// Route base: /api/ContoCinema
[Route("api/[controller]")]
// Richiede autenticazione per accedere agli endpoint
[Authorize (Roles = Ruoli.Operatore)]
public class ContoCinemaController : ControllerBase
{
    // Service per la gestione del conto del cinema
    private readonly ContoCinemaService _contoCinemaService;
    // Service per la registrazione dei log delle azioni
    private readonly LogAzioniService _logAzioniService;

    // Iniezione dei servizi tramite costruttore
    public ContoCinemaController(ContoCinemaService contoCinemaService, LogAzioniService logAzioniService)
    {
        _contoCinemaService = contoCinemaService;
        _logAzioniService = logAzioniService;
    }

    // Endpoint GET: restituisce il conto
    [HttpGet]
    public async Task<IActionResult> OttieniDatiConto()
    {
        // Recupera tutti i dati
        DtoContoCinema contoCinema = await _contoCinemaService.OttieniDatiContoAsync();

        // Recupera l'Id dell'utente loggato
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");

        // Verifico la presenza
        if(contoCinema == null)
        {
             await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ottieni dati Conto", false);

            return BadRequest(new { messaggio = "Non è presente nessun conto." });
        }

        // Registra log di operazione riuscita
        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ottieni dati Conto", true);

        // Restituisce il conto del cinema
        return Ok(contoCinema);
    }

//
// SECONDO LE SPECIFICHE DOVREBBE ESSERE HARD CODED
//

/*
    // Endpoint POST: crea un nuovo conto
    [HttpPost]
    public async Task<IActionResult> Creazione([FromBody] DtoCreazioneContoCinema dto)
    {
        // Recupera l'Id dell'utente loggato
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");
        // Recupera tutti i dati del conto
        DtoContoCinema contoCinema = await _contoCinemaService.OttieniDatiContoAsync();

        // Verifico la presenza del conto
        if(contoCinema != null)
        {
             await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Creazione dati conto", false);

            return BadRequest(new { messaggio = "Conto già presente." });
        }

        // Prova di creazione del conto
        DtoContoCinema? risultato = await _contoCinemaService.CreazioneAsync(dto);

        if (risultato == null)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Creazione dati conto", false);

            return BadRequest(new { messaggio = "Dati conto non validi." });
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Creazione dati conto", true);

        // Restituisce il conto
        return Ok(risultato);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Modifica(string id, [FromBody] DtoCreazioneContoCinema dto)
    {
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");

        try
        {
            DtoContoCinema? risultato = await _contoCinemaService.ModificaAsync(id, dto);
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Modifica dati conto", true);

            return Ok(risultato);

        }
        catch (ModificaException ex)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Modifica dati conto", false);
            return BadRequest(new { message = ex.Message });
        }

        catch (ItemNotFoundException ex)
        {

            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Modifica dati conto", false);

            return NotFound(new { message = ex.Message });

        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Elimina(string id)
    {
        bool eliminato = await _contoCinemaService.EliminaAsync(id);
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");

        if (!eliminato)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Eliminazione dati conto", false);

            return NotFound(new { messaggio = "Dati conto non trovati." });
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Eliminazione dati conto", true);

        return Ok(new { message = "I dati del conto sono stati eliminati correttamente" });
    }
    */
}

## OperatoreUtentiController.cs

```c#
// Importa gli attributi per la gestione dell'autorizzazione

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

namespace NuovoCinemaParadiso.Controllers;

// Indica che questa classe è un controller API
[ApiController]
// Definisce la route base: /api/OperatoreUtenti
[Route("api/[controller]")]
// Richiede che l'utente sia autenticato per accedere a tutti gli endpoint del controller
[Authorize(Roles = Ruoli.Operatore)]
public class OperatoreUtentiController : ControllerBase
{
    // Riferimento al servizio che gestisce la logica sugli utenti
    private readonly RuoloUtenteService _ruoloUtenteService;
    // Riferimento al servizio che registra i log delle azioni
    private readonly LogAzioniService _logAzioniService;

    // Costruttore con dependency injection dei servizi necessari
    public OperatoreUtentiController(RuoloUtenteService ruoloUtenteService, LogAzioniService logAzioniService)
    {
        // Assegna il servizio utenti al campo privato
        _ruoloUtenteService = ruoloUtenteService;
        // Assegna il servizio log al campo privato
        _logAzioniService = logAzioniService;
    }

    // ------------------------------------------------------------
    // 3) CAMBAI RUOLO DELL'UTENTE
    // ------------------------------------------------------------
    [HttpPut("cambia-ruolo")]
    public async Task<IActionResult> CambiaRuolo([FromBody] DtoModificaRuoloUtente dto)
    {
        
        // Recupero l’utente tramite ID.
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");
        
        // aseegno il nuovo ruolo
        string? nuovoRuolo = await _ruoloUtenteService.ModificaRuoloUtente(dto);
        if (nuovoRuolo == null)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId,"Cambio ruolo",false);

            return BadRequest(new { messaggio = "Utente o ruolo non valido." });
        }
        
        // Log successo.
        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId,"Cambio ruolo",true);
        
        return Ok(new
        {
            messaggio = "Ruolo aggiornato correttamente.",
            email = dto.Email,
            ruolo = nuovoRuolo
        });
    }
}
```

## AppExceptions.cs (Gestisce gli errori tra i controller e i services)

```c#

namespace NuovoCinemaParadiso.Exceptions;

public abstract class AppException : Exception
{
    protected AppException(string message) : base(message) { }
}

public class NotFoundException : AppException
{
    public NotFoundException(string risorsa, string id)
        : base($"{risorsa} con ID '{id}' non trovato.") { }
}

public class ItemAlredyexist : AppException
{
    public ItemAlredyexist(string risorsa)
        : base($"Una {risorsa} è già collegata all'utente") { }
}

public class ConflictException : AppException
{
    public ConflictException(string message) : base(message) { }
}

public class ModificaException : AppException
{
    public ModificaException(string message) : base($"E' gia presente un {message} con lo stesso nome") { }
}

public class ItemNotFoundException : AppException
{
    public ItemNotFoundException(string message) : base($"{message} non trovato.") { }
}

public class InvalidEmail : AppException
{
    public InvalidEmail(string message) : base($"L'email {message} non è valida") { }
}
```