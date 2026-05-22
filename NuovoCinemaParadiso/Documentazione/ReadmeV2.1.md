# NuovoCinemaParadisoV2.0

# Convenzioni

1. La modifica del codice deve essere riportata sul readme come una versione indipendente dello stesso ma con l'aggiunta del codice     modificato commentato;

2. Il commit deve, a livello descrittivo in modo sintetico, rimandare alla modifica/implementazione fatta;

Esempio: 

## Versione 1.0
```c#
```

## Versione 1.1 
Utente:
Data:
Descrizione:


# implementazioni 

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

## ContoCinema.cs

```c#
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;


namespace NuovoCinemaParadiso.Models;

// Attributo Entity Framework:
// specifica che questa classe sarà associata alla tabella "ContoCinema"
[Table("ContoCinema")]


public class ContoCinema
{
    // Indica che la proprietà Id è la chiave primaria della tabella
    [Key]

    // Proprietà stringa che rappresenta l'identificativo univoco del conto
    // Viene inizializzata automaticamente con un GUID convertito in stringa
    public string Id { get; set; } = Guid.NewGuid().ToString();
    
    // Indica che il campo è obbligatorio
    [Required]

    // Imposta la lunghezza minima e massima della stringa
    // In questo caso l'IBAN deve avere tra 22 e 34 caratteri
    // Se la validazione fallisce viene mostrato il messaggio personalizzato
    [StringLength(34, MinimumLength = 22, ErrorMessage = "La lunghezza dell'iban deve essere compresa tra 22 e 34.")]

    // Proprietà che contiene l'IBAN del conto
    public string Iban { get; set; }

    // Indica che il nome del titolare è obbligatorio
    [Required]

    // Limita la lunghezza massima del nome del titolare a 100 caratteri
    [StringLength(100)]

    // Proprietà che contiene il nome del titolare del conto
    public string TitolareConto {get;set;}

    // Indica che il campo è obbligatorio
    [Required]

    // Proprietà intera che rappresenta il conto del cinema
    public int Conto {get;set;}
}
```

### ContoCinema.cs Versione 1.1.1

Utente: Fabio Tammaro(github: FabTam)
Data: 21/05/2026
Descrizione: Modificato il nome della proprietà Conto in Saldo.
```c#
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;


namespace NuovoCinemaParadiso.Models;




public class ContoCinema
{
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString();
    
    [Required]
    [StringLength(34, MinimumLength = 22, ErrorMessage = "La lunghezza dell'iban deve essere compresa tra 22 e 34.")]

    public string Iban { get; set; }
    [Required]

    [StringLength(100)]
    public string TitolareConto {get;set;}

// modificato il nome della proprietà.
    [Required]
    public int Saldo {get;set;}
}
```

## GenereMovie.cs

```c#
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NuovoCinemaParadiso.Models;

// Associa questa classe alla tabella "GeneriMovies" nel database
[Table("GeneriMovies")]

public class GenereMovie
{
    // Indica che la proprietà Id è la chiave primaria della tabella
    [Key]

    // Proprietà che contiene l'identificativo univoco del genere
    // Viene generato automaticamente un GUID convertito in stringa
    public string Id { get; set; } = Guid.NewGuid().ToString();

    // Indica che il campo Genere è obbligatorio
    [Required]

    // Limita la lunghezza massima della stringa a 15 caratteri
    [StringLength(15)]

    // Proprietà che rappresenta il nome del genere cinematografico
    // Viene inizializzata con una stringa vuota per evitare valori null
    public string Genere {get;set;} = string.Empty;

    // Lista di oggetti Movie associati a questo genere
    // Rappresenta una relazione uno-a-molti o molti-a-molti con la tabella Movie
    // La lista viene inizializzata vuota per evitare errori null reference
    public List<Movie> Movies {get;set;} = new List<Movie>();
}
```

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

## LogAzione.cs

```c#
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace NuovoCinemaParadiso.Models;

// Associa questa classe alla tabella "LogsAzioni" nel database
[Table("LogsAzioni")]

public class LogAzioni
{
    // Indica che la proprietà Id è la chiave primaria della tabella
    [Key]

    // Proprietà che rappresenta l'identificativo univoco del log
    // Viene generato automaticamente un GUID convertito in stringa
    public string Id { get; set; } = Guid.NewGuid().ToString();

    // Proprietà opzionale che contiene l'Id dell'utente che ha eseguito l'azione
    // Il ? indica che il valore può essere null
    // Viene inizializzata con una stringa vuota
    public string? IdUtente {get;set;} = string.Empty;

    // Proprietà che contiene il nome dell'azione eseguita
    // Esempio: "Login", "Creazione Film", "Eliminazione Utente"
    // Inizializzata con stringa vuota per evitare valori null
    public string NomeAzione {get;set;} = string.Empty;

    // Proprietà booleana che indica se l'azione è stata completata correttamente
    // true = operazione riuscita
    // false = operazione fallita
    public bool Effettuato {get;set;}

    // Proprietà che contiene un messaggio descrittivo del log
    // Può contenere dettagli sull'errore o sull'operazione effettuata
    public string Messaggio {get;set;} = string.Empty;

    // Proprietà che registra data e ora dell'evento
    // DateTimeOffset salva anche le informazioni sul fuso orario
    public DateTimeOffset TimeStamp {get;set;}
}
```

## Movie.cs

```c#
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NuovoCinemaParadiso.Models;

// Associa questa classe alla tabella "Movies" nel database
[Table("Movies")]

public class Movie
{
    // Indica che la proprietà Id è la chiave primaria della tabella
    [Key]

    // Proprietà che rappresenta l'identificativo univoco del film
    // Viene generato automaticamente un GUID convertito in stringa
    public string Id {get;set;} = Guid.NewGuid().ToString();

    // Indica che il titolo del film è obbligatorio
    [Required]

    // Limita la lunghezza massima del titolo a 50 caratteri
    [StringLength(50)]

    // Proprietà che contiene il titolo del film
    // Inizializzata con stringa vuota per evitare valori null
    public string Titolo {get;set;} = string.Empty;

    // Indica che la descrizione del film è obbligatoria
    [Required]

    // Limita la lunghezza massima della descrizione a 2000 caratteri
    [StringLength(2000)]

    // Proprietà che contiene la descrizione/trama del film
    public string Descrizione {get;set;} = string.Empty;

    // Valida che la durata sia maggiore o uguale a 1
    // int.MaxValue rappresenta il valore massimo possibile per un intero
    [Range(1, int.MaxValue)]

    // Proprietà che rappresenta la durata del film in minuti
    public int DurataMinuti {get;set;} 

    // Valida che il prezzo sia compreso tra 0.01 e 999999999
    // typeof(decimal) specifica che il controllo viene effettuato su valori decimali
    [Range(typeof(decimal), "0.01", "999999999")]

    // Proprietà che rappresenta il prezzo del film
    public int PrezzoMovie {get;set;}

    // Lista delle proiezioni associate a questo film
    // Rappresenta una relazione uno-a-molti tra Movie e Proiezione
    // La lista viene inizializzata vuota per evitare errori null reference
    public List<Proiezione> Proiezioni {get;set;} = new List<Proiezione>();

    // Proprietà che contiene la chiave esterna del genere del film
    public string GenereId {get;set;} = string.Empty;
    
    // Specifica che la proprietà Genere utilizza GenereId come chiave esterna
    [ForeignKey("GenereId")]

    // Proprietà di navigazione verso l'entità GenereMovie
    // Permette di accedere ai dati del genere associato al film
    // Il ? indica che il valore può essere null
    public GenereMovie? Genere {get;set;}
}
```


## Proiezione.cs

```c#
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace NuovoCinemaParadiso.Models;

// Associa questa classe alla tabella "Proiezioni" nel database
[Table("Proiezioni")]

public class Proiezione
{
    // Indica che la proprietà Id è la chiave primaria della tabella
    [Key]

    // Proprietà che rappresenta l'identificativo univoco della proiezione
    // Viene generato automaticamente un GUID convertito in stringa
    public string Id { get; set; } = Guid.NewGuid().ToString();

    // Indica che la data della proiezione è obbligatoria
    [Required]

    // Proprietà che contiene la data della proiezione
    // DateOnly salva solo la data senza orario
    public DateOnly DataProiezione { get; set; }

    // Indica che l'identificativo del film associato è obbligatorio
    [Required]

    // Proprietà che contiene la chiave esterna del film
    public string MovieId { get; set; } = string.Empty;

    // Specifica che la proprietà Movie utilizza MovieId come chiave esterna
    [ForeignKey("MovieId")]

    // Proprietà di navigazione verso l'entità Movie
    // Permette di accedere ai dati del film associato alla proiezione
    // Il ? indica che il valore può essere null
    public Movie? Movie { get; set; }

    // Indica che l'identificativo della sala è obbligatorio
    [Required]

    // Proprietà che contiene la chiave esterna della sala
    public string SalaId { get; set; } = string.Empty;

    // Specifica che la proprietà Sala utilizza SalaId come chiave esterna
    [ForeignKey("SalaId")]

    // Proprietà di navigazione verso l'entità Sala
    // Permette di accedere ai dati della sala associata alla proiezione
    public Sala? Sala { get; set; }

    // Indica che l'identificativo del turno è obbligatorio
    [Required]

    // Proprietà che contiene la chiave esterna del turno
    public string TurnoId { get; set; } = string.Empty;

    // Specifica che la proprietà Turno utilizza TurnoId come chiave esterna
    [ForeignKey("TurnoId")]

    // Proprietà di navigazione verso l'entità Turno
    // Permette di accedere ai dati del turno associato alla proiezione
    public Turno? Turno { get; set; }

    // Lista dei biglietti associati a questa proiezione
    // Rappresenta una relazione uno-a-molti tra Proiezione e Biglietto
    // La lista viene inizializzata vuota per evitare errori null reference
    public List<Biglietto> Biglietti { get; set; } = new List<Biglietto>();

    // Proprietà booleana che indica se la proiezione è attiva
    // true = proiezione disponibile
    // false = proiezione disattivata/non disponibile
    public bool Attivo { get; set; }
}
```

## Ruoli.cs

```c#

public static class Ruoli
{
    // Costante stringa che rappresenta il ruolo "Gestore"
    // const significa che il valore non può essere modificato
    public const string Gestore = "Gestore";

    // Costante stringa che rappresenta il ruolo "Operatore"
    public const string Operatore = "Operatore";

    // Costante stringa che rappresenta il ruolo "Utente"
    public const string Utente = "Utente";

    // Costante stringa che contiene più ruoli separati da virgola
    // Utile ad esempio per autorizzazioni multiple
    // (es. accesso consentito sia ai Gestori che agli Operatori)
    public const string GestoreOrOperatore = "Gestore, Operatore";
}
```

## Sala.cs

```c#
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NuovoCinemaParadiso.Models;

// Associa questa classe alla tabella "Sale" nel database
[Table("Sale")]

public class Sala
{
    // Indica che la proprietà Id è la chiave primaria della tabella
    [Key]

    // Proprietà che rappresenta l'identificativo univoco della sala
    // Viene generato automaticamente un GUID convertito in stringa
    public string Id {get; set;} = Guid.NewGuid().ToString();
    
    // Indica che il nome della sala è obbligatorio
    [Required]

    // Limita la lunghezza massima del nome a 100 caratteri
    [StringLength(100)]

    // Proprietà che contiene il nome della sala
    // Inizializzata con stringa vuota per evitare valori null
    public string Nome {get; set;} = string.Empty;

    // Proprietà che rappresenta il numero massimo di posti disponibili nella sala
    public int Capienza {get; set;}

    // Lista delle proiezioni associate a questa sala
    // Rappresenta una relazione uno-a-molti tra Sala e Proiezione
    // La lista viene inizializzata vuota per evitare errori null reference
    public List<Proiezione> Proiezioni {get;set;} = new List<Proiezione>();

    // Proprietà che contiene la chiave esterna della tipologia della sala
    public string TipologiaSalaId {get; set;} = string.Empty;

    // Specifica che la proprietà TipologiaSala utilizza TipologiaSalaId come chiave esterna
    [ForeignKey("TipologiaSalaId")]

    // Proprietà di navigazione verso l'entità TipologiaSala
    // Permette di accedere ai dati della tipologia associata alla sala
    // Il ? indica che il valore può essere null
    public TipologiaSala? TipologiaSala {get; set;}
}
```

## TipologiaSala.cs

```c#
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NuovoCinemaParadiso.Models;

// Associa questa classe alla tabella "TipologieSala" nel database
[Table("TipologieSala")]

public class TipologiaSala
{
    // Indica che la proprietà Id è la chiave primaria della tabella
    [Key]

    // Proprietà che rappresenta l'identificativo univoco della tipologia di sala
    // Viene generato automaticamente un GUID convertito in stringa
    public string Id { get; set; } = Guid.NewGuid().ToString();

    // Indica che il nome della tipologia è obbligatorio
    [Required]

    // Proprietà che contiene il nome della tipologia di sala
    // Esempi: "Standard", "IMAX", "3D", "VIP"
    // Inizializzata con stringa vuota per evitare valori null
    public string Nome { get; set; } = string.Empty;

    // Proprietà che rappresenta un eventuale aumento di prezzo
    // associato a questa tipologia di sala
    // Esempio: sala VIP o IMAX con costo aggiuntivo
    public int MaggiorazionePrezzo { get; set; }

    // Lista delle sale associate a questa tipologia
    // Rappresenta una relazione uno-a-molti tra TipologiaSala e Sala
    // La lista viene inizializzata vuota per evitare errori null reference
    public List<Sala> Sale { get; set; } = new List<Sala>();
}
```

## Turno.cs

```c#
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NuovoCinemaParadiso.Models;

// Associa questa classe alla tabella "Turni" nel database
[Table("Turni")]

public class Turno
{
    // Indica che la proprietà Id è la chiave primaria della tabella
    [Key]

    // Proprietà che rappresenta l'identificativo univoco del turno
    // Viene generato automaticamente un GUID convertito in stringa
    public string Id { get; set; } = Guid.NewGuid().ToString();
    
    // Indica che l'orario di inizio del turno è obbligatorio
    [Required]

    // Proprietà che contiene l'ora di inizio del turno
    // TimeOnly salva solamente l'orario senza la data
    public TimeOnly OraInizio { get; set; }

    // Indica che l'orario di fine del turno è obbligatorio
    [Required]

    // Proprietà che contiene l'ora di fine del turno
    public TimeOnly OraFine { get; set; }

    // Lista delle proiezioni associate a questo turno
    // Rappresenta una relazione uno-a-molti tra Turno e Proiezione
    // La lista viene inizializzata vuota per evitare errori null reference
    public List<Proiezione> Proiezioni {get;set;} = new List<Proiezione>();

    // Limita la lunghezza massima del nome del turno a 50 caratteri
    [StringLength(50)]

    // Proprietà che contiene il nome del turno
    // Esempi: "Mattina", "Pomeriggio", "Sera"
    // Inizializzata con stringa vuota per evitare valori null
    public string Nome { get; set; } = string.Empty; // es: "Sera", "Pomeriggio"

}
```

## Utente.cs

```c#
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NuovoCinemaParadiso.Models;

// Associa questa classe alla tabella "Utenti" nel database
[Table("Utenti")]

// Definizione della classe Utente
// Eredita da IdentityUser, la classe di ASP.NET Identity
// che contiene proprietà e funzionalità per autenticazione e gestione utenti
public class Utente : IdentityUser
{
    // Indica che il nome completo è obbligatorio
    [Required]

    // Limita la lunghezza massima del nome completo a 100 caratteri
    [StringLength(100)]

    // Proprietà che contiene il nome completo dell'utente
    // Inizializzata con stringa vuota per evitare valori null
    public string NomeCompleto { get; set; } = string.Empty;

    // Indica che il campo età è obbligatorio
    [Required]

    // Valida che l'età sia compresa tra 14 e 100 anni
    // Se il valore non è valido viene mostrato il messaggio personalizzato
    [Range(14, 100, ErrorMessage = "L'età deve essere compresa tra 14 e 100")]

    // Proprietà che rappresenta l'età dell'utente
    public int Eta { get; set; }

    // Indica che il campo è obbligatorio
    [Required]

    // Proprietà booleana che indica se l'utente possiede un abbonamento
    // false = non abbonato
    // true = abbonato
    // Inizializzata di default a false
    public bool SeAbbonato { get; set; } = false;

    // Indica che la data di inizio abbonamento è obbligatoria
    [Required]

    // Proprietà che contiene la data e ora di inizio dell'abbonamento
    // DateTimeOffset salva anche il fuso orario
    public DateTimeOffset DataInizioAbbonamento { get; set; }

    // Lista dei biglietti acquistati dall'utente
    // Rappresenta una relazione uno-a-molti tra Utente e Biglietto
    // La lista viene inizializzata vuota per evitare errori null reference
    public List<Biglietto> Biglietti { get; set; } = new List<Biglietto>();

    // Proprietà che contiene la chiave esterna dell'abbonamento associato
    // Il ? indica che il valore può essere null
    public string? AbbonamentoId { get; set; }

    // Specifica che la proprietà Abbonamento utilizza AbbonamentoId come chiave esterna
    [ForeignKey("AbbonamentoId")]

    // Proprietà di navigazione verso l'entità Abbonamento
    // Permette di accedere ai dati dell'abbonamento associato all'utente
    public Abbonamento? Abbonamento { get; set; }

    // Valida che il saldo sia compreso tra 0 e 10000
    [Range(0,10000)]

    // Proprietà che rappresenta il saldo disponibile dell'utente
    public int Saldo {get;set;}
}
```

# Data 

## ContestoDb.cs
```c#
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NuovoCinemaParadiso.Models;

namespace NuovoCinemaParadiso.Data
{
    // Classe del DbContext principale dell'applicazione
    // Eredita da IdentityDbContext per integrare Identity con EF Core
    public class ContestoDb : IdentityDbContext<Utente, IdentityRole, string>
    {
        // Costruttore che riceve le opzioni di configurazione del DbContext
        // (stringa di connessione, provider, ecc.)
        public ContestoDb(DbContextOptions<ContestoDb> opzioni)
            : base(opzioni)
        {
        }

        // DbSet che rappresenta la tabella Movies nel database
        public DbSet<Movie> Movies { get; set; }

        // DbSet che rappresenta la tabella GeneriMovies nel database
        public DbSet<GenereMovie> GeneriMovies { get; set; }

        // DbSet che rappresenta la tabella Sale nel database
        public DbSet<Sala> Sale { get; set; }

        // DbSet che rappresenta la tabella TipologieSala nel database
        public DbSet<TipologiaSala> TipologieSala { get; set; }

        // DbSet che rappresenta la tabella Turni nel database
        public DbSet<Turno> Turni { get; set; }

        // DbSet che rappresenta la tabella Biglietti nel database
        public DbSet<Biglietto> Biglietti { get; set; }

        // DbSet che rappresenta la tabella Utenti nel database
        public DbSet<Utente> Utenti { get; set; }

        // DbSet che rappresenta la tabella Abbonamenti nel database
        public DbSet<Abbonamento> Abbonamenti {get;set;}

        // DbSet che rappresenta la tabella LogAzioni nel database
        public DbSet<LogAzioni> LogAzioni {get;set;}

        // DbSet che rappresenta la tabella Proiezioni nel database
        public DbSet<Proiezione> Proiezioni {get;set;}

        // DbSet che rappresenta la tabella GiftCards nel database
        public DbSet<GiftCard> GiftCards {get;set;}

        // DbSet che rappresenta la tabella ContoCinema nel database
        public DbSet<ContoCinema> ContoCinema {get;set;}

        // Metodo che permette di configurare il modello del database
        // (relazioni, vincoli, comportamenti di delete, ecc.)
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Chiama la configurazione base di Identity + EF Core
            base.OnModelCreating(modelBuilder);

            // Configurazione della relazione tra Biglietto e Proiezione
            modelBuilder.Entity<Biglietto>()
                // Un Biglietto ha una sola Proiezione
                .HasOne(a => a.Proiezione)

                // Una Proiezione può avere molti Biglietti
                .WithMany(p => p.Biglietti)

                // Chiave esterna che collega Biglietto a Proiezione
                .HasForeignKey(a => a.ProiezioneId)

                // Comportamento alla cancellazione:
                // impedisce di eliminare una Proiezione se esistono Biglietti collegati
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
```

# Dtos

## DtoAbbonamento.cs
```c#
namespace NuovoCinemaParadiso.Dtos;

public class DtoAbbonamento
{
    public string? Id { get; set; }          // Id univoco dell'abbonamento (stringa, può essere null in creazione)

    public string Nome { get; set; } = string.Empty;   // Nome dell'abbonamento (es. Mensile, Annuale)

    public int Durata { get; set; }          // Durata dell'abbonamento (giorni/mesi secondo logica)

    public int Prezzo { get; set; }          // Prezzo base prima dello sconto

    public int Sconto { get; set; }          // Percentuale di sconto applicata
}
```

## DtoAuthResponse.cs

```c#
namespace NuovoCinemaParadiso.Dtos;

public class DtoAuthResponse
{
    public string Id { get; set; } = string.Empty;                     // Id utente autenticato
    public string NomeCompleto { get; set; } = string.Empty;           // Nome completo dell'utente
    public string Token { get; set; } = string.Empty;                  // JWT generato al login
    public int Eta { get; set; }                                       // Età dell'utente
    public string Email { get; set; } = string.Empty;                  // Email dell'utente
    public string Ruolo { get; set; } = string.Empty;                  // Ruolo (es. Gestore, Cliente)
    public DateTimeOffset DataInizioAbbonamento { get; set; }          // Data di inizio dell'abbonamento
    public bool SeAbbonato { get; set; }                               // True se l'utente ha un abbonamento attivo
    public int Saldo { get; set; }                                     // Saldo residuo (es. credito gift card)
}
```

## DtoBiglietto.cs V1.0

```c#
namespace NuovoCinemaParadiso.Dtos;

public class DtoBiglietto
{
    public string? Id { get; set; }                          // Id del biglietto (null in creazione)

    public string ProiezioneId { get; set; } = string.Empty; // Id della proiezione associata

    public string UtenteId { get; set; } = string.Empty;     // Id dell'utente che acquista

    public int PrezzoFinale { get; set; }                    // Prezzo totale calcolato lato server

    public DateTimeOffset OrarioCreazione { get; set; }      // Timestamp di generazione del biglietto

    public int NumeroBiglietti { get; set; }                 // Numero di biglietti acquistati

    public string MetodoPagamento { get; set; } = "standard"; // Metodo di pagamento scelto
}
```

## DtoBiglietto.cs V1.1

Andrea Bruno 22-05-2026
Aggiunta campi nel DTO:

```c#
namespace NuovoCinemaParadiso.Dtos;
public class DtoBiglietto
{
    public string? Id { get; set; }
    public string UtenteId { get; set; } = string.Empty;
    public int PrezzoFinale {get;set;}
    public DateTimeOffset OrarioCreazione {get;set;} 
    public string ProiezioneId {get;set;} = string.Empty;
    public int NumeroBiglietti {get;set;}
    // Nome della sala in cui avviene la proiezione.
    // Usato per mostrare all’utente il luogo del film.
    public string NomeSala { get; set; } = string.Empty;

    // Titolo del film associato alla proiezione.
    // Evita di dover fare join aggiuntive lato frontend.
    public string TitoloMovie { get; set; } = string.Empty;

    // Nome della tipologia della sala (es. IMAX, 3D, Standard).
    // Serve per calcolare maggiorazioni e mostrare info complete.
    public string NomeTipologiaSala { get; set; } = string.Empty;

    // Orario di inizio della proiezione.
    // TimeOnly rappresenta solo l’ora, senza data.
    public TimeOnly OraInizio { get; set; }

    // Data della proiezione.
    // DateOnly rappresenta solo la data, senza orario.
    public DateOnly DataProiezione { get; set; }

}
```

## DtoContoCinema.cs

```c#
namespace NuovoCinemaParadiso.Dtos;

public class DtoContoCinema
{
    public string Id { get; set; } = Guid.NewGuid().ToString(); // Id univoco del conto cinema

    public string Iban { get; set; } = string.Empty;            // IBAN associato al conto

    public string TitolareConto { get; set; } = string.Empty;   // Nome del titolare del conto

    public int Conto { get; set; }                              // Saldo o valore del conto
}

```
### DtoContoCinema.cs Versione 1.1.1.

Utente: Fabio
Data: 21/05/2026
Descrizione: Modificata la proprietà Conto in Saldo.
```c#
namespace NuovoCinemaParadiso.Dtos;

public class DtoContoCinema
{
    public string Id { get; set; } = Guid.NewGuid().ToString(); 

    public string Iban { get; set; } = string.Empty;            

    public string TitolareConto { get; set; } = string.Empty;   

    // Modificata la proprietà Conto in Saldo.
    public int Saldo { get; set; }            
}                  
```

## DtoCreazioneAbbonamento.cs

```c#
namespace NuovoCinemaParadiso.Dtos;

public class DtoCreazioneAbbonamento
{
    public string Nome { get; set; } = string.Empty;   // Nome dell'abbonamento da creare

    public int Durata { get; set; }                    // Durata (giorni/mesi secondo logica)

    public int Prezzo { get; set; }                    // Prezzo base dell'abbonamento

    public int Sconto { get; set; }                    // Percentuale di sconto applicata
}
```

## DtoCreazioneBiglietto.cs

```c#
using System.ComponentModel.DataAnnotations;

namespace NuovoCinemaParadiso.Dtos;

public class DtoCreazioneBiglietto
{
    [Required]
    public string ProiezioneId { get; set; } = string.Empty;   // Id della proiezione scelta

    [Required]
    [Range(1, 100, ErrorMessage = "Il numero di biglietti deve essere maggiore di zero e massimo 100.")]
    public int NumeroBiglietti { get; set; }                   // Numero di biglietti richiesti

    public string MetodoPagamento { get; set; } = "standard";  // Metodo di pagamento selezionato
}
```

## DtoCreazioneContoCinema.cs

```c#
using System.ComponentModel.DataAnnotations;

namespace NuovoCinemaParadiso.Dtos;

public class DtoCreazioneContoCinema
{
    [Required]
    [StringLength(34, MinimumLength = 22, ErrorMessage = "La lunghezza dell'iban deve essere compresa tra 22 e 34.")]
    public string Iban { get; set; } = string.Empty;          // IBAN del conto cinema

    [Required]
    [StringLength(100)]
    public string TitolareConto { get; set; } = string.Empty; // Nome del titolare del conto

    [Required]
    public int Conto { get; set; }                            // Valore/saldo iniziale del conto
}
```

## DtoCreazioneGenereMovie.cs

```c#
using System.ComponentModel.DataAnnotations;

namespace NuovoCinemaParadiso.Dtos;

public class DtoCreazioneGenereMovie
{
    [Required]
    [StringLength(15)]
    public string Genere { get; set; } = string.Empty;   // Nome del genere (max 15 caratteri)
}
```

## DtoCreazioneGiftCard.cs

```c#
namespace NuovoCinemaParadiso.Dtos;

public class DtoCreazioneGiftCard
{
    public string Nome { get; set; } = string.Empty;          // Nome della gift card
    public int Valore { get; set; }                           // Valore/credito della gift card
    public string CodiceRiscatto { get; set; } = string.Empty; // Codice univoco per il riscatto
}
```

## DtoCreazioneLogAzioni.cs

```c#
namespace NuovoCinemaParadiso.Dtos;

public class DtoCreazioneLogAzioni
{
    public string? Id { get; set; }                     // Id del log (può essere null in creazione)

    public string IdUtente { get; set; } = string.Empty; // Id dell'utente che ha eseguito l'azione

    public string NomeAzione { get; set; } = string.Empty; // Nome dell'azione registrata

    public bool Effettuato { get; set; }                 // True se l'azione è andata a buon fine

    public string Messaggio { get; set; } = string.Empty; // Messaggio descrittivo dell'azione

    public DateTimeOffset TimeStamp { get; set; }        // Data e ora dell'evento
}
```

## DtoCreazioneMovie.cs

```c#
using System.ComponentModel.DataAnnotations;

namespace NuovoCinemaParadiso.Dtos;

public class DtoCreazioneMovie
{
    [Required]
    [StringLength(50)]
    public string Titolo { get; set; } = string.Empty;          // Titolo del film

    [Required]
    [StringLength(200)]
    public string Descrizione { get; set; } = string.Empty;     // Breve descrizione del film

    [Range(1, int.MaxValue, ErrorMessage = "La durata deve essere un numero intero positivo maggiore di 0.")]
    public int DurataMinuti { get; set; }                       // Durata del film in minuti

    [Range(0, double.MaxValue, ErrorMessage = "Il prezzo deve essere un numero positivo.")]
    public int PrezzoMovie { get; set; }                        // Prezzo base del film

    [Required]
    public string GenereId { get; set; } = string.Empty;        // Id del genere associato
}
```

## DtoCreazioneProiezione.cs

```c#
namespace NuovoCinemaParadiso.Dtos;

public class DtoCreazioneProiezione
{
    public string MovieId { get; set; } = string.Empty;     // Id del film da proiettare
    public string SalaId { get; set; } = string.Empty;      // Id della sala scelta
    public string TurnoId { get; set; } = string.Empty;     // Id del turno/orario selezionato
    public DateOnly DataProiezione { get; set; }            // Data della proiezione
}
```

## DtoCreazioneSala.cs

```c#
using System.ComponentModel.DataAnnotations;

namespace NuovoCinemaParadiso.Dtos;

public class DtoCreazioneSala
{
    [Required]
    [StringLength(100)]
    public string Nome { get; set; } = string.Empty;   // Nome della sala

    public int Capienza { get; set; }                  // Numero massimo di posti

    public string TipologiaSalaId { get; set; } = string.Empty; // Id della tipologia sala
}
```

## DtoCreazioneTipologiaSala.cs

```c#
namespace NuovoCinemaParadiso.Dtos;

public class DtoCreazioneTipologiaSala
{
    [Required]
    public string Nome { get; set; } = string.Empty;   // Nome della tipologia sala (es. Standard, IMAX)

    public int MaggiorazionePrezzo { get; set; }       // Maggiorazione applicata al prezzo base
}
```

## DtoCreazioneTurno.cs

```c#
using System.ComponentModel.DataAnnotations;

namespace NuovoCinemaParadiso.Dtos;

public class DtoCreazioneTurno
{    
    [Required]
    public TimeOnly OraInizio { get; set; }      // Orario di inizio del turno

    [Required]
    public TimeOnly OraFine { get; set; }        // Orario di fine del turno
    
    [StringLength(50)]
    public string Nome { get; set; } = string.Empty; // Nome del turno (es. Sera, Pomeriggio)
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
    public string NomeCompleto { get; set; } = string.Empty;   // Nome completo dell'utente
    
    [Required]
    [Range(14, 100, ErrorMessage = "L'età deve essere compresa tra 14 e 100")]
    public int Eta { get; set; }                               // Età valida per la registrazione
}
```

## DtoGenereMovie.cs

```c#
namespace NuovoCinemaParadiso.Dtos;

public class DtoGenereMovie
{
    public string? Id { get; set; }                 // Id del genere (null quando non ancora creato)
    public string Genere { get; set; } = string.Empty; // Nome del genere (es. Azione, Horror)
}
```

## DtoGiftCard.cs

```c#
namespace NuovoCinemaParadiso.Dtos;

public class DtoGiftCard
{
    public string? Id { get; set; }                     // Id della gift card (null in creazione)
    public string Nome { get; set; } = string.Empty;    // Nome della gift card
    public int Valore { get; set; }                     // Valore/credito disponibile
    public string CodiceRiscatto { get; set; } = string.Empty; // Codice univoco per il riscatto
}
```

## DtoGiftCard.cs

```c#
namespace NuovoCinemaParadiso.Dtos;

public class DtoGiftCard
{
    public string? Id { get; set; }                     // Id della gift card (null in creazione)
    public string Nome { get; set; } = string.Empty;    // Nome della gift card
    public int Valore { get; set; }                     // Valore/credito disponibile
    public string CodiceRiscatto { get; set; } = string.Empty; // Codice univoco per il riscatto
}
```

## DtoLogAzioni.cs

```c#
namespace NuovoCinemaParadiso.Dtos;

public class DtoLogAzioni
{
    public string? Id { get; set; }                     // Id del log (null se non ancora assegnato)

    public string? IdUtente { get; set; } = string.Empty; // Id dell'utente che ha eseguito l'azione

    public string NomeAzione { get; set; } = string.Empty; // Nome dell'azione registrata

    public bool Effettuato { get; set; }                 // True se l'azione è stata completata con successo

    public string Messaggio { get; set; } = string.Empty; // Messaggio descrittivo dell'evento

    public DateTimeOffset TimeStamp { get; set; }        // Data e ora dell'azione
}
```

## DtoLogin.cs

```c#
using System.ComponentModel.DataAnnotations;

namespace NuovoCinemaParadiso.Dtos;

public class DtoLogin
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;      // Email dell'utente per il login

    [Required]
    [StringLength(100, MinimumLength = 6)]
    public string Password { get; set; } = string.Empty;   // Password dell'utente (min 6 caratteri)
}
```

## DtoModificaRuoloUtente.cs

```c#
using System.ComponentModel.DataAnnotations;

namespace NuovoCinemaParadiso.Dtos;

public class DtoModificaRuoloUtente
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;     // Email dell’utente a cui modificare il ruolo

    [Required]
    public string NuovoRuolo { get; set; } = string.Empty; // Nuovo ruolo da assegnare (es. Admin, Gestore, Utente)
}
```

## DtoMovie.cs

```c#
namespace NuovoCinemaParadiso.Dtos;

public class DtoMovie
{
    public string? Id { get; set; }                     // Id del film (null quando non ancora creato)

    public string Titolo { get; set; } = string.Empty;  // Titolo del film

    public string Descrizione { get; set; } = string.Empty; // Descrizione breve del film

    public int DurataMinuti { get; set; }               // Durata del film in minuti

    public int PrezzoMovie { get; set; }                // Prezzo base del film

    public string GenereId { get; set; } = string.Empty; // Id del genere associato

    public string Genere { get; set; } = string.Empty;   // Nome del genere (dato derivato)
}
```

## DtoProiezione.cs

```c#
namespace NuovoCinemaParadiso.Dtos;

public class DtoProiezione
{
    public string Id { get; set; } = string.Empty;      // Id della proiezione

    public DateOnly DataProiezione { get; set; }        // Data in cui avviene la proiezione

    public string MovieId { get; set; } = string.Empty; // Id del film proiettato

    public string SalaId { get; set; } = string.Empty;  // Id della sala utilizzata

    public string TurnoId { get; set; } = string.Empty; // Id del turno/orario

    public bool Attivo { get; set; } = true;            // True se la proiezione è attiva
}
```

## DtoRegistrazione.cs

```c#
using System.ComponentModel.DataAnnotations;

namespace NuovoCinemaParadiso.Dtos;

public class DtoRegistrazione
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;      // Email dell’utente per la registrazione

    [Required]
    [StringLength(100, MinimumLength = 6)]
    public string Password { get; set; } = string.Empty;   // Password (min 6 caratteri)

    [Required]
    [StringLength(100)]
    public string NomeCompleto { get; set; } = string.Empty; // Nome completo dell’utente

    public int Eta { get; set; }                           // Età dell’utente

    [Range(0, 10000)]
    public int Saldo { get; set; }                         // Saldo iniziale opzionale (default 0)
}
```

## DtoRicaricaGiftCard.cs

```c#
namespace NuovoCinemaParadiso.Dtos;

public class DtoRicaricaGiftCard
{
    public int Importo { get; set; }   // Importo da aggiungere al saldo della gift card
}
```

## DtoSala.cs

```c#
namespace NuovoCinemaParadiso.Dtos;

public class DtoSala
{
    public string? Id { get; set; }                     // Id della sala (null quando non ancora creata)

    [StringLength(100)]
    public string Nome { get; set; } = string.Empty;    // Nome della sala

    public int Capienza { get; set; }                   // Numero massimo di posti disponibili

    public string TipologiaSalaId { get; set; } = string.Empty; // Id della tipologia sala

    public string NomeTipologia { get; set; } = string.Empty;    // Nome della tipologia (dato derivato)
}
```

## DtoTipologiaSala.cs

```c#
namespace NuovoCinemaParadiso.Dtos;

public class DtoTipologiaSala
{
    public string? Id { get; set; }                     // Id della tipologia (null quando non ancora creata)

    public string Nome { get; set; } = string.Empty;    // Nome della tipologia (es. Standard, IMAX, VIP)

    public int MaggiorazionePrezzo { get; set; }        // Maggiorazione applicata al prezzo base
}
```

## DtoTurno.cs

```c#
namespace NuovoCinemaParadiso.Dtos;

public class DtoTurno
{
    public string? Id { get; set; }                 // Id del turno (null quando non ancora creato)

    public TimeOnly OraInizio { get; set; }         // Orario di inizio del turno

    public TimeOnly OraFine { get; set; }           // Orario di fine del turno

    public string Nome { get; set; } = string.Empty; // Nome del turno (es. Sera, Pomeriggio)
}
```

## DtoUtente.cs

```c#
namespace NuovoCinemaParadiso.Dtos;

public class DtoUtente
{
    public string Id { get; set; } = string.Empty;                 // Id univoco dell’utente

    public string NomeCompleto { get; set; } = string.Empty;       // Nome completo dell’utente

    public DateTimeOffset DataInizioAbbonamento { get; set; }      // Data di attivazione dell’abbonamento

    public bool SeAbbonato { get; set; }                           // True se l’utente ha un abbonamento attivo

    public string Email { get; set; } = string.Empty;              // Email dell’utente

    public int Eta { get; set; }                                   // Età dell’utente

    public string AbbonamentoId { get; set; } = string.Empty;      // Id dell’abbonamento associato

    public string TipoAbbonamento { get; set; } = string.Empty;    // Nome/tipo dell’abbonamento

    public int Saldo { get; set; }                                 // Saldo disponibile dell’utente
}
```


# Controllers

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
[Authorize]
public class AbbonamentoController : ControllerBase
{
    // Service che gestisce la logica sugli abbonamenti
    private readonly AbbonamentoService _abbonamentoService;

    // Service che registra le azioni dell’utente
    private readonly LogAzioniService _logAzioniService;

    // Service per la gestione dei gestori (non usato qui ma mantenuto)
    private readonly GestoreService _gestoreService;

    // Costruttore che riceve i servizi tramite dependency injection
    public AbbonamentoController(
        AbbonamentoService abbonamentoService,
        LogAzioniService logAzioniService,
        GestoreService gestoreService)
    {
        _abbonamentoService = abbonamentoService;
        _logAzioniService = logAzioniService;
        _gestoreService = gestoreService;
    }

    // -----------------------------
    // GET: Ottieni tutti gli abbonamenti
    // -----------------------------
    [HttpGet]
    public async Task<IActionResult> OttieniTuttiGliAbbonamenti()
    {
        // Recupera l’ID dell’utente autenticato
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        // Se non autenticato → errore
        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");

        // Recupera tutti gli abbonamenti dal service
        List<DtoAbbonamento> abbonamenti = await _abbonamentoService.OttieniTutto();

        // Registra il log dell’azione
        await _logAzioniService.SalvataggioLogAzioneAsync(
            utenteId,
            "Ottieni tutti gli abbonamenti utente",
            true
        );

        // Restituisce la lista
        return Ok(abbonamenti);
    }

    // -----------------------------
    // GET: Ottieni abbonamento tramite ID
    // -----------------------------
    [HttpGet("{id}")]
    public async Task<IActionResult> OttieniTramiteId(string id)
    {
        // Recupera l’ID dell’utente autenticato
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        // Se non autenticato → errore
        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");

        // Recupera l’abbonamento solo se appartiene all’utente
        var risultato = await _abbonamentoService.OttieniTramiteIdAsync(id, utenteId);

        // Se non trovato → errore + log
        if (risultato == null)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(
                utenteId,
                "Ottieni abbonamenti tramite id utente",
                false
            );

            return NotFound($"Abbonamento con id {id} non trovato");
        }

        // Log successo
        await _logAzioniService.SalvataggioLogAzioneAsync(
            utenteId,
            "Ottieni abbonamenti tramite id utente",
            true
        );

        // Restituisce il DTO
        return Ok(risultato);
    }

    // -----------------------------
    // POST: Creazione abbonamento
    // -----------------------------
    [HttpPost]
    [Authorize(Roles = Ruoli.Operatore)]
    public async Task<IActionResult> Creazione([FromBody] DtoCreazioneAbbonamento dto)
    {
        // Recupera l’ID dell’utente autenticato
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        // Se non autenticato → errore
        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");

        // Chiama il service che restituisce solo (bool, messaggio)
        var risultato = await _abbonamentoService.CreazioneAsync(dto);

        // Se fallito → log + errore
        if (!risultato.Successo)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(
                utenteId,
                "Creazione abbonamento",
                false
            );

            return BadRequest(new { messaggio = risultato.Messaggio });
        }

        // Log successo
        await _logAzioniService.SalvataggioLogAzioneAsync(
            utenteId,
            "Creazione abbonamento",
            true
        );

        // Restituisce solo messaggio
        return Ok(new { messaggio = risultato.Messaggio });
    }

    // -----------------------------
    // PUT: Modifica abbonamento
    // -----------------------------
    [HttpPut("{id}")]
    [Authorize(Roles = Ruoli.Operatore)]
    public async Task<IActionResult> Modifica(string id, [FromBody] DtoCreazioneAbbonamento dto)
    {
        // Recupera l’ID dell’utente autenticato
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        // Se non autenticato → errore
        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");

        // Chiama il service
        var risultato = await _abbonamentoService.ModificaAsync(id, dto);

        // Se fallito → log + errore
        if (!risultato.Successo)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(
                utenteId,
                "Modifica abbonamento",
                false
            );

            return NotFound(new { messaggio = risultato.Messaggio });
        }

        // Log successo
        await _logAzioniService.SalvataggioLogAzioneAsync(
            utenteId,
            "Modifica abbonamento",
            true
        );

        // Restituisce solo messaggio
        return Ok(new { messaggio = risultato.Messaggio });
    }

    // -----------------------------
    // DELETE: Elimina abbonamento
    // -----------------------------
    [HttpDelete("{id}")]
    [Authorize(Roles = Ruoli.Operatore)]
    public async Task<IActionResult> Elimina(string id)
    {
        // Recupera l’ID dell’utente autenticato
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        // Se non autenticato → errore
        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");

        // Chiama il service
        var risultato = await _abbonamentoService.EliminazioneAsync(id);

        // Se fallito → log + errore
        if (!risultato.Successo)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(
                utenteId,
                "Elimina abbonamento",
                false
            );

            return NotFound(new { messaggio = risultato.Messaggio });
        }

        // Log successo
        await _logAzioniService.SalvataggioLogAzioneAsync(
            utenteId,
            "Elimina abbonamento",
            true
        );

        // Restituisce 204 NoContent
        return NoContent();
    }
}
```

## GestoreController V1.0

Andrea Bruno 22-05-2026 
Aggiunta funzione OttieniTuttiBiglietti() e 
Centralizzazione dell'Authorize

```C#
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NuovoCinemaParadiso.Services;
using NuovoCinemaParadiso.Dtos;
using System.Security.Claims;

namespace NuovoCinemaParadiso.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = Ruoli.Gestore)] // Authorize centralizzato
public class GestoreController : ControllerBase
{
    private readonly GestoreService _gestoreService;
    private readonly LogAzioniService _logAzioniService;

    public GestoreController(GestoreService gestoreService, LogAzioniService logAzioniService)
    {
        _gestoreService = gestoreService;
        _logAzioniService = logAzioniService;
    }

    [HttpGet("conto")]
    public async Task<IActionResult> OttieniDatiConto()
    {
        DtoContoCinema contoCinema = await _gestoreService.OttieniDatiContoAsync();
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");

        if (contoCinema == null)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ottieni dati Conto", false);

            return BadRequest(new { messaggio = "Non è presente nessun conto." });
        }
        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ottieni dati Conto", true);
        
        return Ok(contoCinema);
    }

    [HttpGet("logs")]
    public async Task<IActionResult> OttieniLogAzioni()
    {
        List<DtoLogAzioni> risultatiLog = await _gestoreService.LetturaLogAzioneAsync();
        return Ok(risultatiLog);
    }

    [HttpGet]
    public async Task<IActionResult> OttieniTuttiBiglietti()
    {
        // Recupera l'ID dell'utente autenticato dal token JWT.
        // FindFirstValue può restituire null → uso string? per sicurezza.
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        // Se l'utente non è autenticato, ritorno 401 Unauthorized.
        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");

        // Recupera tutti i biglietti tramite il servizio Gestore.
        // Il metodo restituisce una lista di DTO già pronti per l'API.
        List<DtoBiglietto> biglietti = await _gestoreService.OttieniTuttiBigliettiAsync();

        // Salva nel log l'azione eseguita dall'utente.
        // Passo: ID utente, nome azione, esito (true = successo).
        await _logAzioniService.SalvataggioLogAzioneAsync(
            utenteId,
            "Ottieni tutti i biglietti",
            true
        );
        // Restituisce 200 OK con la lista dei biglietti.
        return Ok(biglietti);
    }
}
```

## AuthController.cs

```c#
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using NuovoCinemaParadiso.Services;
using NuovoCinemaParadiso.Dtos;
using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;
using NuovoCinemaParadiso.Exceptions;

namespace NuovoCinemaParadiso.Controllers;

[ApiController] // Indica che il controller è un’API REST
[Route("api/[controller]")] // Route base: api/Auth
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;
    private readonly LogAzioniService _logAzioniService;

    public AuthController(AuthService authService, LogAzioniService logAzioniService)
    {
        _authService = authService;
        _logAzioniService = logAzioniService;
    }

    [HttpPost("registrazione")] // POST api/Auth/registrazione
    public async Task<IActionResult> Registrazione(DtoRegistrazione dto)
    {
        try
        {
            IdentityResult result = await _authService.RegistrazioneAsync(dto);

            if (!result.Succeeded)
            {
                await _logAzioniService.SalvataggioLogAzioneAsync(null, "Registrazione utente", false);
                return BadRequest(result.Errors);
            }

            await _logAzioniService.SalvataggioLogAzioneAsync(null, "Registrazione utente", true);
            return Ok(new { messaggio = "Registrazione avvenuta con successo!" });
        }
        catch (InvalidEmail ex)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(null, "Registrazione utente", false);
            return BadRequest(new { errore = ex.Message });
        }
    }

    [HttpPost("login")] // POST api/Auth/login
    public async Task<IActionResult> Login([FromBody] DtoLogin dto)
    {
        try
        {
            DtoAuthResponse? risposta = await _authService.LoginAsync(dto);

            await _logAzioniService.SalvataggioLogAzioneAsync(risposta.Id, "Login", true);

            return Ok(risposta);
        }
        catch (NotFoundException ex)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(null, "Login", false);
            return NotFound(new { messaggio = ex.Message });
        }
        catch (ConflictException ex)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(null, "Login", false);
            return BadRequest(new { messaggio = ex.Message });
        }
    }

    [HttpGet("profilo")] // GET api/Auth/profilo
    public async Task<IActionResult> RicercaProfiloLoggato()
    {
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier); // id utente autenticato
        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");

        DtoUtente? utente = await _authService.OttieniTramiteIdAsync(utenteId);

        if (utente == null)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ricerca profilo loggato", false);
            return NotFound(new { messaggio = "Utente non trovato." });
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ricerca profilo loggato", true);
        return Ok(utente);
    }

    [HttpPut("modifica")] // PUT api/Auth/modifica
    public async Task<IActionResult> Modifica([FromBody] DtoCreazioneUtente dto)
    {
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");

        var risultato = await _authService.ModificaAsync(dto, utenteId);

        if (risultato == null)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Modifica utente", false);
            return NotFound(new { messaggio = "Utente non trovato." });
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Modifica utente", true);
        return Ok(risultato);
    }

    [HttpDelete("elimina")] // DELETE api/Auth/elimina
    public async Task<IActionResult> Elimina()
    {
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");

        var risultato = await _authService.EliminaAsync(utenteId);

        if (risultato == null)
        {
            return NotFound(new { messaggio = "Utente non trovato." });
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Eliminazione profilo", true);
        return Ok(risultato);
    }
}
```

Utente: Marco Strazzeri (github: FastCode85)
Data: 22/05/2026
Descrizione: Apportate modifiche ai risultati di ritorno degli endpoint, minima esposizione dei dati

```c#

using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using NuovoCinemaParadiso.Services;
using NuovoCinemaParadiso.Dtos;
using NuovoCinemaParadiso.Exceptions;

namespace NuovoCinemaParadiso.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;
    private readonly LogAzioniService _logAzioniService;

    public AuthController(AuthService authService, LogAzioniService logAzioniService)
    {
        _authService = authService;
        _logAzioniService = logAzioniService;
    }

    [HttpPost("registrazione")]
    public async Task<IActionResult> Registrazione(DtoRegistrazione dto)
    {
        try
        {
            //ritorna true o false e un eventuale messaggio di errore
            var (result, errors) = await _authService.RegistrazioneAsync(dto);
            if (!result)
            {
                await _logAzioniService.SalvataggioLogAzioneAsync(null, "Registrazione utente", false);
                return BadRequest(errors);
            }
            await _logAzioniService.SalvataggioLogAzioneAsync(null, "Registrazione utente", true);
            return Ok(new { messaggio = "Registrazione avvenuta con successo!" });
        }
        catch (InvalidEmail ex)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(null, "Registrazione utente", false);
            return BadRequest(new { errore = ex.Message });
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] DtoLogin dto)
    {
        try
        {
            DtoAuthResponse? risposta = await _authService.LoginAsync(dto);
            if (risposta == null)
            {
                await _logAzioniService.SalvataggioLogAzioneAsync(null, "Login", false);
                return BadRequest(new { messaggio = "Credenziali non valide." });
            }
            await _logAzioniService.SalvataggioLogAzioneAsync(risposta.Id, "Login", true);
            return Ok(risposta);
        }
        catch (NotFoundException ex)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(null, "Login", false);
            return NotFound(new { messaggio = ex.Message });
        }
        catch (ConflictException ex)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(null, "Login", false);
            return BadRequest(new { messaggio = ex.Message });
        }
    }


    [HttpGet("profilo")]
    public async Task<IActionResult> RicercaProfiloLoggato()
    {
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");

        DtoUtente? utente = await _authService.OttieniTramiteIdAsync(utenteId);

        if (utente == null)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ricerca profilo loggato", false);
            return NotFound(new { messaggio = "Utente non trovato." });
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ricerca profilo loggato", true);
        return Ok(utente);
    }

    [HttpPut("modifica")]
    public async Task<IActionResult> Modifica([FromBody] DtoCreazioneUtente dto)
    {
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");

        var risultato = await _authService.ModificaAsync(dto, utenteId);

        if (risultato == null)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Modifica utente", false);
            return NotFound(new { messaggio = "Utente non trovato." });
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Modifica utente", true);
        //ritorna true se la modifica ha successo, altrimenti false
        return Ok(new { messaggio = "Utente modificato con successo." });
    }

    [HttpDelete("elimina")]
    public async Task<IActionResult> Elimina()
    {
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");

        var risultato = await _authService.EliminaAsync(utenteId);

        if (risultato == null)
        {
            return NotFound(new { messaggio = "Utente non trovato." });
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Eliminazione profilo", true);
        //ritorna true se la modifica ha successo, altrimenti false
        return Ok(new { messaggio = "Utente eliminato con successo." });
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

    // DELETE: api/Biglietto/{id} - Elimina un biglietto (Solo Gestore o Operatore)
    [HttpDelete("{id}")]
    [Authorize(Roles = Ruoli.Operatore)]
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

## VERSIONE 1.1.1

Utente: Fabio Tammaro
Data: 22/05/2026
Descrizione : Questa parte è stata rimossa in concomitanza con l'aggiunta del metodo di lettura nel conto nel dominio del gestore.

## ContoCinemaController.cs

```c#
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using NuovoCinemaParadiso.Dtos;
using NuovoCinemaParadiso.Services;
using NuovoCinemaParadiso.Exceptions;

namespace NuovoCinemaParadiso.Controllers;

[ApiController] // Indica che il controller espone API REST
[Route("api/[controller]")] // Route base: api/ContoCinema
[Authorize(Roles = Ruoli.Operatore)] // Accesso consentito solo agli Operatori
public class ContoCinemaController : ControllerBase
{
    private readonly ContoCinemaService _contoCinemaService;
    private readonly LogAzioniService _logAzioniService;

    public ContoCinemaController(
        ContoCinemaService contoCinemaService,
        LogAzioniService logAzioniService)
    {
        _contoCinemaService = contoCinemaService;
        _logAzioniService = logAzioniService;
    }

    [HttpGet] // GET api/ContoCinema
    public async Task<IActionResult> OttieniDatiConto()
    {
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier); // id utente autenticato
        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");

        DtoContoCinema contoCinema = await _contoCinemaService.OttieniDatiContoAsync();

        if (contoCinema == null)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(
                utenteId,
                "Ottieni dati Conto",
                false);

            return BadRequest(new { messaggio = "Non è presente nessun conto." });
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(
            utenteId,
            "Ottieni dati Conto",
            true);

        return Ok(contoCinema);
    }
}
```

## GenereMovieController.cs

```c#
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using NuovoCinemaParadiso.Dtos;
using NuovoCinemaParadiso.Services;
using NuovoCinemaParadiso.Exceptions;

namespace NuovoCinemaParadiso.Controllers;

[ApiController] // Indica che il controller espone API REST
[Route("api/[controller]")] // Route base: api/GenereMovie
[Authorize] // Richiede autenticazione per tutte le azioni del controller
public class GenereMovieController : ControllerBase
{
    private readonly GenereMovieService _genereMovieService;
    private readonly LogAzioniService _logAzioniService;

    public GenereMovieController(
        GenereMovieService genereMovieService,
        LogAzioniService logAzioniService)
    {
        _genereMovieService = genereMovieService;
        _logAzioniService = logAzioniService;
    }

    [HttpGet] // GET api/GenereMovie
    public async Task<IActionResult> OttieniTutti()
    {
        List<DtoGenereMovie> generiMovie = await _genereMovieService.OttieniTuttoAsync();

        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier); // id utente loggato
        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");

        await _logAzioniService.SalvataggioLogAzioneAsync(
            utenteId,
            "Ottieni tutti i generi",
            true);

        return Ok(generiMovie);
    }

    [HttpGet("{id}")] // GET api/GenereMovie/{id}
    public async Task<IActionResult> OttieniTramiteId(string id)
    {
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");

        var risultato = await _genereMovieService.OttieniTramiteIdAsync(id);

        if (risultato == null)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(
                utenteId,
                "Ottieni genere tramite id",
                false);

            return NotFound($"GenereMovie con id {id} non trovato");
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(
            utenteId,
            "Ottieni genere tramite id",
            true);

        return Ok(risultato);
    }

    /*
    [HttpPost]
    [Authorize(Roles = Ruoli.Operatore)]
    public async Task<IActionResult> Creazione([FromBody] DtoCreazioneGenereMovie dto)
    {
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");

        List<DtoGenereMovie> generiMovie = await _genereMovieService.OttieniTuttoAsync();

        foreach (var generiMovies in generiMovie)
        {
            if (generiMovies.Genere.Contains(dto.Genere))
            {
                await _logAzioniService.SalvataggioLogAzioneAsync(
                    utenteId,
                    "Creazione genere",
                    false);

                return BadRequest(new { messaggio = "Genere già presente." });
            }
        }

        DtoGenereMovie? risultato = await _genereMovieService.CreazioneAsync(dto);

        if (risultato == null)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(
                utenteId,
                "Creazione genere",
                false);

            return BadRequest(new { messaggio = "Genere non valido." });
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(
            utenteId,
            "Creazione genere",
            true);

        return Ok(risultato);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = Ruoli.Operatore)]
    public async Task<IActionResult> Modifica(string id, [FromBody] DtoCreazioneGenereMovie dto)
    {
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");

        try
        {
            DtoGenereMovie? risultato = await _genereMovieService.ModificaAsync(id, dto);

            await _logAzioniService.SalvataggioLogAzioneAsync(
                utenteId,
                "Modifica genere",
                true);

            return Ok(risultato);
        }
        catch (ModificaException ex)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(
                utenteId,
                "Modifica genere",
                false);

            return BadRequest(new { message = ex.Message });
        }
        catch (ItemNotFoundException ex)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(
                utenteId,
                "Modifica genere",
                false);

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

        bool eliminato = await _genereMovieService.EliminaAsync(id);

        if (!eliminato)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(
                utenteId,
                "Eliminazione genere",
                false);

            return NotFound(new { messaggio = "Genere non trovato." });
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(
            utenteId,
            "Eliminazione genere",
            true);

        return Ok(new { message = "Il Genere è stato eliminato correttamente" });
    }
    */
}
```

## GestoreController.cs

```c#
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using NuovoCinemaParadiso.Dtos;
using NuovoCinemaParadiso.Services;
using NuovoCinemaParadiso.Exceptions;

namespace NuovoCinemaParadiso.Controllers;

[ApiController] // Indica che il controller espone API REST
[Route("api/[controller]")] // Route base: api/Gestore
[Authorize(Roles = Ruoli.Gestore)] // Accesso consentito solo ai Gestori
public class GestoreController : ControllerBase
{
    private readonly GestoreService _gestoreService;
    private readonly LogAzioniService _logAzioniService;

    public GestoreController(
        GestoreService gestoreService,
        LogAzioniService logAzioniService)
    {
        _gestoreService = gestoreService;
        _logAzioniService = logAzioniService;
    }

    [HttpGet("log")] // GET api/Gestore/log
    [Authorize(Roles = Ruoli.Gestore)] // ridondante ma rinforza il controllo accessi
    public async Task<IActionResult> OttieniLogAzioni()
    {
        List<DtoLogAzioni> risultatiLog = await _logAzioniService.LetturaLogAzioneAsync();
        return Ok(risultatiLog);
    }
}
```
### GestoreController.cs V1.1.1

Utente: Fabio Tammaro(github: FabTam)
Data: 21/05/2026
Descrizione: Modificata la dependency injections del logAzioniService poichè ora la lettura di log è nel dominio del gestore.

```c#
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NuovoCinemaParadiso.Services;
using NuovoCinemaParadiso.Dtos;

namespace NuovoCinemaParadiso.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class GestoreController : ControllerBase
{
    private readonly GestoreService _gestoreService;

    public GestoreController(GestoreService gestoreService, LogAzioniService logAzioniService)
    {
        _gestoreService = gestoreService;
    }


    [HttpGet("logs")]
    [Authorize(Roles = Ruoli.Gestore)]
    public async Task<IActionResult> OttieniLogAzioni()
    {
        // richiamo al service del gestore per il metodo della lettura degli audit.
        List<DtoLogAzioni> risultatiLog = await _gestoreService.LetturaLogAzioneAsync();
        return Ok(risultatiLog);
    }
}
```
### GestoreController.cs V1.1.2

Utente: Fabio Tammaro(github: FabTam)
Data: 21/05/2026
Descrizione: Inserita la chiamata alla lettura dei dati del conto.

```c#
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NuovoCinemaParadiso.Services;
using NuovoCinemaParadiso.Dtos;
using System.Security.Claims;

namespace NuovoCinemaParadiso.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class GestoreController : ControllerBase
{
    private readonly GestoreService _gestoreService;

    // aggiunta la dependency injections necessaria al salvataggio degli audit.
    private readonly LogAzioniService _logAzioniService;

    public GestoreController(GestoreService gestoreService, LogAzioniService logAzioniService)
    {
        _gestoreService   = gestoreService;
        _logAzioniService = logAzioniService;
    }
    
    //Chiamata get all'endpoint per leggere i dati dal conto
    [HttpGet("conto")]
    public async Task<IActionResult> OttieniDatiConto()
    {
        DtoContoCinema contoCinema = await _gestoreService.OttieniDatiContoAsync();
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");


        if(contoCinema == null)
        {
             await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ottieni dati Conto", false);

            return BadRequest(new { messaggio = "Non è presente nessun conto." });
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ottieni dati Conto", true);


        return Ok(contoCinema);
    }

    [HttpGet("logs")]
    [Authorize(Roles = Ruoli.Gestore)]
    public async Task<IActionResult> OttieniLogAzioni()
    {
        List<DtoLogAzioni> risultatiLog = await _gestoreService.LetturaLogAzioneAsync();
        return Ok(risultatiLog);
    }
}
```

## GestoreUtentiController.cs

```c#
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using NuovoCinemaParadiso.Dtos;
using NuovoCinemaParadiso.Services;
using NuovoCinemaParadiso.Exceptions;

namespace NuovoCinemaParadiso.Controllers;

[ApiController] // Indica che il controller espone API REST
[Route("api/[controller]")] // Route base: api/GestoreUtenti
[Authorize(Roles = Ruoli.Gestore)] // Accesso consentito solo ai Gestori
public class GestoreUtentiController : ControllerBase
{
    private readonly RuoloUtenteService _ruoloUtenteService;
    private readonly LogAzioniService _logAzioniService;

    public GestoreUtentiController(
        RuoloUtenteService ruoloUtenteService,
        LogAzioniService logAzioniService)
    {
        _ruoloUtenteService = ruoloUtenteService;
        _logAzioniService = logAzioniService;
    }

    [HttpPut("cambia-ruolo")] // PUT api/GestoreUtenti/cambia-ruolo
    public async Task<IActionResult> CambiaRuolo([FromBody] DtoModificaRuoloUtente dto)
    {
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier); // id utente che esegue l'azione
        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");

        string? nuovoRuolo = await _ruoloUtenteService.ModificaRuoloUtente(dto);

        if (nuovoRuolo == null)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(
                utenteId,
                "Cambio ruolo",
                false);

            return BadRequest(new { messaggio = "Utente o ruolo non valido." });
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(
            utenteId,
            "Cambio ruolo",
            true);

        return Ok(new
        {
            messaggio = "Ruolo aggiornato correttamente.",
            email = dto.Email,
            ruolo = nuovoRuolo
        });
    }
}
```

## GiftCardController.cs (da copiare quando mergiato)

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

## Giftcard.cs

```c#
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using NuovoCinemaParadiso.Dtos;
using NuovoCinemaParadiso.Services;
using NuovoCinemaParadiso.Exceptions;

namespace NuovoCinemaParadiso.Controllers;

[ApiController] // Indica che il controller espone API REST
[Route("api/[controller]")] // Route base: api/GiftCard
[Authorize] // Richiede autenticazione per tutte le azioni
public class GiftCardController : ControllerBase
{
    private readonly GiftCardService _giftCardService;
    private readonly LogAzioniService _logAzioniService;

    public GiftCardController(
        GiftCardService giftCardService,
        LogAzioniService logAzioniService)
    {
        _giftCardService = giftCardService;
        _logAzioniService = logAzioniService;
    }

    [HttpGet] // GET api/GiftCard
    [Authorize(Roles = Ruoli.Gestore)] // solo Gestore può vedere tutte le giftcard
    public async Task<IActionResult> OttieniTutteLeGiftCard()
    {
        List<DtoGiftCard> giftCards = await _giftCardService.OttieniTutto();

        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier); // id utente loggato
        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");

        await _logAzioniService.SalvataggioLogAzioneAsync(
            utenteId,
            "Ottieni tutte le giftcard",
            true);

        return Ok(giftCards);
    }

    [HttpGet("{id}")] // GET api/GiftCard/{id}
    [Authorize(Roles = Ruoli.Gestore)] // solo Gestore
    public async Task<IActionResult> OttieniTramiteId(string id)
    {
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");

        var risultato = await _giftCardService.OttieniTramiteIdAsync(id, utenteId);

        if (risultato == null)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(
                utenteId,
                "Ottieni giftcard tramite id",
                false);

            return NotFound($"GiftCard con id {id} non trovato");
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(
            utenteId,
            "Ottieni giftcard tramite id",
            true);

        return Ok(risultato);
    }

    [HttpPut("{id}")] // PUT api/GiftCard/{id}
    [Authorize(Roles = Ruoli.GestoreOrOperatore)] // Gestore o Operatore
    public async Task<IActionResult> Modifica(string id, [FromBody] DtoCreazioneGiftCard dto)
    {
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");

        DtoGiftCard? risultato = await _giftCardService.ModificaAsync(id, dto);

        if (risultato == null)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(
                utenteId,
                "Modifica giftcard",
                false);

            return NotFound(new { messaggio = "GiftCard non trovata." });
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(
            utenteId,
            "Modifica GiftCard",
            true);

        return Ok(risultato);
    }

    [HttpDelete("{id}")] // DELETE api/GiftCard/{id}
    [Authorize(Roles = Ruoli.GestoreOrOperatore)] // Gestore o Operatore
    public async Task<IActionResult> Elimina(string id)
    {
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");

        bool eliminato = await _giftCardService.EliminazioneAsync(id);

        if (!eliminato)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(
                utenteId,
                "eliminazione GiftCard",
                false);

            return NotFound(new { messaggio = "GiftCard non trovata." });
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(
            utenteId,
            "eliminazione GiftCard",
            true);

        return NoContent();
    }
}
```

## MovieController.cs (versione 1.1)

Utente: Greg
Data: 21/05/2026
Descrizione: Modificare i valori di ritorno per non esporre dati superflui

```c#
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using NuovoCinemaParadiso.Services;
using NuovoCinemaParadiso.Dtos;

namespace NuovoCinemaParadiso.Controllers;

[ApiController] // Indica che il controller espone API REST
[Route("api/[controller]")] // Route base: api/MovieController
[Authorize] // Richiede autenticazione per tutte le azioni
public class MovieController : ControllerBase
{
    private readonly MovieService _movieService;
    private readonly LogAzioniService _logAzioniService;
    

    public MovieController(MovieService movieService, LogAzioniService logAzioniService)
    {
        _movieService = movieService;
        _logAzioniService = logAzioniService;
    }

    [HttpGet] // GET api/Movie
    public async Task<IActionResult> OttieniTuttiIMovies()
    {
        List<DtoMovie> movies = await _movieService.OttieniTutto();
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier); // id utente loggato
        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");

        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId,"Ottieni tutti i movies" ,true);
        return Ok(movies);
    }

    [HttpGet("genere/{genereId}")] // GET api/Movie/genere/{id}
    public async Task<ActionResult<List<DtoMovie>>> OttieniPerGenere(string genereId)
    {
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");

        if (string.IsNullOrWhiteSpace(genereId))
        {
          await _logAzioniService.SalvataggioLogAzioneAsync(utenteId,"Ottieni movie per genereId" ,false);
          return BadRequest("GenereId non valido");
        }

        var risultato = await _movieService.OttieniTramiteGenere(genereId);

        if (risultato.Count() == 0)
        {
          await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ottieni movie per genereid", false);

          return NotFound("Nessun film trovato per questo genere");
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ottieni movie per genereid", true);;

        return Ok(risultato);
    }

    [HttpGet("{id}")]  // GET api/Movie/{id}
    public async Task<IActionResult> OttieniTramiteId(string id)
    {
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");

        DtoMovie? risultato = await _movieService.OttieniTramiteIdAsync(id);

        if (risultato == null)
        {
          await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ottieni movie per id", false);
          return NotFound($"Film con id {id} non trovato");
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ottieni movie per id", true);
        return Ok(risultato);
    }

    [HttpPost] // POST api/Movie
    [Authorize(Roles = Ruoli.GestoreOrOperatore)]
    public async Task<IActionResult> Creazione([FromBody] DtoCreazioneMovie dto)
    {
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");

        List<DtoMovie> movies = await _movieService.OttieniTutto();

        foreach (var movie in movies)
        {
            if (movie.Titolo.Contains(dto.Titolo))
            {
              await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Creazione movie", false);
              return BadRequest(new { messaggio = "Film già presente." });
            }
        }
        
        bool risultato = await _movieService.CreazioneAsync(dto);

        if (!risultato)
        {
          await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Creazione movie", false);
          return BadRequest(new { messaggio = "id del genere non valido." });
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Creazione movie", true);
        return Ok();
    }

    [HttpPut("{id}")] // PUT api/Movie/{id}
    [Authorize(Roles = Ruoli.GestoreOrOperatore)]
    public async Task<IActionResult> Modifica(string id, [FromBody] DtoCreazioneMovie dto)
    {
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");

        List<DtoMovie> movies = await _movieService.OttieniTutto();

        foreach (var movie in movies)
        {
            if (movie.Titolo.Contains(dto.Titolo) && movie.Id != id)
            {
              await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Creazione movie", false);
              return BadRequest(new { messaggio = "non è possibile modificare il titolo con uno già esistente." });
            }
        }
        
        bool risultato = await _movieService.ModificaAsync(id, dto);

        if (!risultato)
        {
          await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Modifica movie", true);
          return NotFound(new { messaggio = "Film non trovato." });
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Modifica movie", true);
        return Ok();
    }

    [HttpDelete("{id}")] // DELETE api/Movie/{id}
    [Authorize(Roles = Ruoli.GestoreOrOperatore)]
    public async Task<IActionResult> Elimina(string id)
    {
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");

        bool eliminato = await _movieService.EliminaAsync(id);

        if (!eliminato)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Eliminazione movie", false);
            return NotFound(new { messaggio = "Film non trovato." });
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Eliminazione movie", true);
        return Ok(new { messaggio = "Film eliminato con successo!" });
    }
}
```

## MovieController.cs (versione 1.2)

Utente: Greg
Data: 21/05/2026 12:30
Descrizione: Fixato [HttpPut("{id}")] // PUT api/Movie/{id} gestendo gli errori inserendo exceptions  

```c#
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using NuovoCinemaParadiso.Services;
using NuovoCinemaParadiso.Dtos;
using NuovoCinemaParadiso.Exceptions;

namespace NuovoCinemaParadiso.Controllers;

[ApiController] // Indica che il controller espone API REST
[Route("api/[controller]")] // Route base: api/Movie
[Authorize] // Richiede autenticazione per tutte le azioni
public class MovieController : ControllerBase
{
    private readonly MovieService _movieService;
    private readonly LogAzioniService _logAzioniService;
    

    public MovieController(MovieService movieService, LogAzioniService logAzioniService)
    {
        _movieService = movieService;
        _logAzioniService = logAzioniService;
    }

    [HttpGet] // GET api/Movie
    public async Task<IActionResult> OttieniTuttiIMovies()
    {
        List<DtoMovie> movies = await _movieService.OttieniTutto();
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");

        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId,"Ottieni tutti i movies" ,true);
        return Ok(movies);
    }

    [HttpGet("genere/{genereId}")] // GET api/Movie/genere/{id}
    public async Task<ActionResult<List<DtoMovie>>> OttieniPerGenere(string genereId)
    {
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");

        if (string.IsNullOrWhiteSpace(genereId))
        {
          await _logAzioniService.SalvataggioLogAzioneAsync(utenteId,"Ottieni movie per genereId" ,false);
          return BadRequest("GenereId non valido");
        }

        var risultato = await _movieService.OttieniTramiteGenere(genereId);

        if (risultato.Count() == 0)
        {
          await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ottieni movie per genereid", false);

          return NotFound("Nessun film trovato per questo genere");
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ottieni movie per genereid", true);;

        return Ok(risultato);
    }

    [HttpGet("{id}")]  // GET api/Movie/{id}
    public async Task<IActionResult> OttieniTramiteId(string id)
    {
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");

        DtoMovie? risultato = await _movieService.OttieniTramiteIdAsync(id);

        if (risultato == null)
        {
          await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ottieni movie per id", false);
          return NotFound($"Film con id {id} non trovato");
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ottieni movie per id", true);
        return Ok(risultato);
    }

    [HttpPost] // POST api/Movie
    [Authorize(Roles = Ruoli.Operatore)]
    public async Task<IActionResult> Creazione([FromBody] DtoCreazioneMovie dto)
    {
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");

        List<DtoMovie> movies = await _movieService.OttieniTutto();

        foreach (var movie in movies)
        {
            if (movie.Titolo.Contains(dto.Titolo))
            {
              await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Creazione movie", false);
              return BadRequest(new { messaggio = "Film già presente." });
            }
        }
        
        bool risultato = await _movieService.CreazioneAsync(dto);

        if (!risultato)
        {
          await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Creazione movie", false);
          return BadRequest(new { messaggio = "id del genere non valido." });
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Creazione movie", true);
        return Ok();
    }

    [HttpPut("{id}")] // PUT api/Movie/{id}
    [Authorize(Roles = Ruoli.Operatore)]
    public async Task<IActionResult> Modifica(string id, [FromBody] DtoCreazioneMovie dto)
    {
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");

        List<DtoMovie> movies = await _movieService.OttieniTutto();

        foreach (var movie in movies)
        {
            if (movie.Titolo.Contains(dto.Titolo) && movie.Id != id)
            {
              await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Creazione movie", false);
              return BadRequest(new { messaggio = "non è possibile modificare il titolo con uno già esistente." });
            }
        }
        try
        {
            await _movieService.ModificaAsync(id, dto);
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Modifica movie", true);
            return Ok();
        }
        catch(NotFoundException ex)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Modifica movie", false);
            return NotFound(new { messaggio = ex.Message });
        }
    }

    [HttpDelete("{id}")] // DELETE api/Movie/{id}
    [Authorize(Roles = Ruoli.Operatore)]
    public async Task<IActionResult> Elimina(string id)
    {
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");

        bool eliminato = await _movieService.EliminaAsync(id);

        if (!eliminato)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Eliminazione movie", false);
            return NotFound(new { messaggio = "Film non trovato." });
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Eliminazione movie", true);
        return Ok(new { messaggio = "Film eliminato con successo!" });
    }
}
```

## OperatoreController.cs

```c#
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using NuovoCinemaParadiso.Dtos;
using NuovoCinemaParadiso.Services;
using NuovoCinemaParadiso.Exceptions;

namespace NuovoCinemaParadiso.Controllers;

[ApiController] // Indica che il controller espone API REST
[Route("api/[controller]")] // Route base: api/Operatore
[Authorize] // Richiede autenticazione per accedere al controller
public class OperatoreController : ControllerBase
{
    private readonly OperatoreService _operatoreService;
    private readonly LogAzioniService _logAzioniService;

    public OperatoreController(
        OperatoreService operatoreService,
        LogAzioniService logAzioniService)
    {
        _operatoreService = operatoreService;
        _logAzioniService = logAzioniService;
    }

    [HttpPost("ricarica")] // POST api/Operatore/ricarica
    [Authorize(Roles = Ruoli.Operatore)] // solo Operatore
    public async Task<IActionResult> Ricarica([FromBody] DtoRicarica dtoRicarica)
    {
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");

        bool successo = await _operatoreService.RicaricaAsync(dtoRicarica);

        if (!successo)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(
                utenteId,
                $"Ricarica fallita per {dtoRicarica.Email}",
                false);

            return NotFound($"Utente con email {dtoRicarica.Email} non trovato.");
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(
            utenteId,
            $"Ricarica riuscita per {dtoRicarica.Email}",
            true);

        return Ok("Ricarica effettuata con successo.");
    }

    [HttpGet("listaUtenti")] // GET api/Operatore/listaUtenti
    [Authorize(Roles = Ruoli.Operatore)]
    public async Task<IActionResult> OttieniTuttiIProfili()
    {
        List<DtoUtente> utenti = await _operatoreService.OttieniUtentiAsync();

        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");

        await _logAzioniService.SalvataggioLogAzioneAsync(
            utenteId,
            "Ottieni tutti i profili",
            true);

        return Ok(utenti);
    }

    [HttpGet("ricercaProfilo/{id}")] // GET api/Operatore/ricercaProfilo/{id}
    [Authorize(Roles = Ruoli.Operatore)]
    public async Task<IActionResult> RicercaProfiloTramiteId(string id)
    {
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");

        try
        {
            DtoUtente? utente = await _operatoreService.OttieniUtenteTramiteIdAsync(id);

            await _logAzioniService.SalvataggioLogAzioneAsync(
                utenteId,
                "Ricerca profilo",
                true);

            return Ok(utente);
        }
        catch
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(
                utenteId,
                "Ricerca profilo",
                false);

            return NotFound(new { messaggio = "Utente non trovato." });
        }
    }

    [HttpDelete("eliminaUtente/{id}")] // DELETE api/Operatore/eliminaUtente/{id}
    [Authorize(Roles = Ruoli.Operatore)]
    public async Task<IActionResult> EliminaTramiteId(string id)
    {
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");

        try
        {
            var risultato = await _operatoreService.EliminaUtentePerIdAsync(id);

            await _logAzioniService.SalvataggioLogAzioneAsync(
                utenteId,
                "Eliminazione profilo",
                true);

            return Ok(risultato);
        }
        catch
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(
                utenteId,
                "Eliminazione profilo",
                false);

            return NotFound(new { messaggio = "Utente non trovato." });
        }
    }

    [HttpGet("biglietto")] // GET api/Operatore/biglietto
    [Authorize(Roles = Ruoli.Operatore)]
    public async Task<IActionResult> OttieniTuttiGliBiglietti()
    {
        List<DtoBiglietto> biglietti = await _operatoreService.OttieniBiglietti();

        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");

        await _logAzioniService.SalvataggioLogAzioneAsync(
            utenteId,
            "Ottieni tutti i biglietti",
            true);

        return Ok(biglietti);
    }

    [HttpGet("biglietto/{id}")] // GET api/Operatore/biglietto/{id}
    [Authorize(Roles = Ruoli.Operatore)]
    public async Task<IActionResult> OttieniBigliettoTramiteId(string id)
    {
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");

        try
        {
            var risultato = await _operatoreService.OttieniBigliettoTramiteIdAsync(id);

            await _logAzioniService.SalvataggioLogAzioneAsync(
                utenteId,
                "Ottieni biglietto tramite id",
                true);

            return Ok(risultato);
        }
        catch
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(
                utenteId,
                "Ottieni biglietto tramite id",
                false);

            return NotFound($"Biglietto con id {id} non trovato");
        }
    }

    [HttpGet("utenti/abbonamento/{abbonamentoId}")] // GET utenti per abbonamento
    [Authorize(Roles = Ruoli.Operatore)]
    public async Task<ActionResult<List<DtoUtente>>> OttieniUtentiTramiteAbbonamentoAsync(string abbonamentoId)
    {
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");

        if (string.IsNullOrWhiteSpace(abbonamentoId))
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(
                utenteId,
                "Ottieni utenti per abbonamento",
                false);

            return BadRequest("AbbonamentoId non valido");
        }

        var risultato = await _operatoreService.OttieniUtentiTramiteAbbonamentoAsync(abbonamentoId);

        if (risultato == null || risultato.Count == 0)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(
                utenteId,
                "Ottieni utenti per abbonamento",
                false);

            return NotFound("Nessun utente trovato per questo abbonamento");
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(
            utenteId,
            "Ottieni utenti per abbonamento",
            true);

        return Ok(risultato);
    }

    [HttpPost("giftCard/ricarica")] // POST giftcard
    [Authorize(Roles = Ruoli.Operatore)]
    public async Task<IActionResult> RicaricaGiftCard([FromBody] DtoRicaricaGiftCard dto)
    {
        string? operatoreId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (operatoreId == null)
            return Unauthorized("Utente non autenticato.");

        if (dto == null || dto.Importo <= 0)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(
                operatoreId,
                "RicaricaGiftCard",
                false);

            return BadRequest(new { errore = "L'importo della Gift Card deve essere maggiore di zero." });
        }

        try
        {
            var risultato = await _operatoreService.RicaricaGiftCardAsync(dto);

            await _logAzioniService.SalvataggioLogAzioneAsync(
                operatoreId,
                "RicaricaGiftCard",
                true);

            return Ok(risultato);
        }
        catch (Exception ex)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(
                operatoreId,
                "RicaricaGiftCard",
                false);

            return BadRequest(new { errore = ex.Message });
        }
    }
}
```

## OperatoreController.cs (versione 1.1)

```c#
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using NuovoCinemaParadiso.Services;
using NuovoCinemaParadiso.Dtos;

namespace NuovoCinemaParadiso.Controllers;

[ApiController] // Indica che il controller espone API REST
[Route("api/[controller]")] // Route base: api/Operatore
[Authorize] // Richiede autenticazione per accedere al controller
public class OperatoreController : ControllerBase
{
    private readonly OperatoreService _operatoreService;
    private readonly LogAzioniService _logAzioniService;

    public OperatoreController(OperatoreService operatoreService, LogAzioniService logAzioniService)
    {
        _operatoreService = operatoreService;
        _logAzioniService = logAzioniService;
    }

    [HttpPost("ricarica")] // POST api/Operatore/ricarica
    [Authorize(Roles = Ruoli.Operatore)] // solo Operatore
    public async Task<IActionResult> RicaricaSaldoUtente([FromBody] DtoRicaricaSaldoUtente dtoRicaricaSaldoUtente)
    {
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");
        bool successo = await _operatoreService.RicaricaAsync(dtoRicaricaSaldoUtente);
        if (!successo)        {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, $"Ricarica fallita per {dtoRicaricaSaldoUtente.Email}", false);
            return NotFound($"Utente con email {dtoRicaricaSaldoUtente.Email} non trovato.");
        }
        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, $"Ricarica riuscita per {dtoRicaricaSaldoUtente.Email}", true);

        return Ok("Ricarica effettuata con successo.");
    }

    [HttpGet("listaUtenti")] // GET api/Operatore/listaUtenti
    [Authorize(Roles = Ruoli.Operatore)]
    public async Task<IActionResult> OttieniTuttiIProfili()
    {
        List<DtoUtente> utenti = await _operatoreService.OttieniUtentiAsync();

        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");

        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ricerca profili", true);

        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ottieni tutti i profili", true);

        return Ok(utenti);
    }

    [HttpGet("ricercaProfilo/{id}")] // GET api/Operatore/ricercaProfilo/{id}
    [Authorize(Roles = Ruoli.Operatore)]
    public async Task<IActionResult> RicercaProfiloTramiteId(string id)
    {
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");
        try
        {
            DtoUtente? utente = await _operatoreService.OttieniUtenteTramiteIdAsync(id);
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ricerca profilo", true);
            return Ok(utente);
        }
        catch
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ricerca profilo", false);
            return NotFound(new { messaggio = "Utente non trovato." });
        }
    }

    [HttpDelete("eliminaUtente/{id}")] // DELETE api/Operatore/eliminaUtente/{id}
    [Authorize(Roles = Ruoli.Operatore)]
    public async Task<IActionResult> EliminaTramiteId(string Id)
    {
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");
        try
        {
            await _operatoreService.EliminaUtentePerIdAsync(Id);
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Eliminazione profilo", true);
            return Ok();
        }
        catch
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Eliminazione profilo", false);
            return NotFound(new { messaggio = "Utente non trovato." });
        }
    }

    [HttpGet("biglietto")] // GET api/Operatore/biglietto
    [Authorize(Roles = Ruoli.Operatore)]
    public async Task<IActionResult> OttieniTuttiGliBiglietti()
    {
        List<DtoBiglietto> biglietti = await _operatoreService.OttieniBiglietti();

        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");

        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ottieni tutti gli biglietti gestore", true);
        return Ok(biglietti);
    }

    [HttpGet("biglietto/{id}")] // GET api/Operatore/biglietto/{id}
    [Authorize(Roles = Ruoli.Operatore)]
    public async Task<IActionResult> OttieniBigliettoTramiteId(string id)
    {

        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");
        try
        {
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

    [HttpGet("utenti/abbonamento/{abbonamentoId}")] // GET utenti per abbonamento
    [Authorize(Roles = Ruoli.Operatore)]
    public async Task<ActionResult<List<DtoUtente>>> OttieniUtentiTramiteAbbonamentoAsync(string abbonamentoId)
    {
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");

        if (string.IsNullOrWhiteSpace(abbonamentoId))
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ottieni gli utenti per abbonamento", false);

            return BadRequest("AbbonamentoId non valido");
        }

        var risultato = await _operatoreService.OttieniUtentiTramiteAbbonamentoAsync(abbonamentoId);

        if (risultato == null || risultato.Count == 0)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ottieni gli utenti per abbonamento", false);

            return NotFound("Nessun utente trovato per questo abbonamento");
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ottieni gli utenti per abbonamento", true);

        return Ok(risultato);
    }

    [HttpPost("giftCard/ricarica")] // POST giftcard
    [Authorize(Roles = Ruoli.Operatore)]
    public async Task<IActionResult> RicaricaGiftCard([FromBody] DtoRicaricaGiftCard dto)
    {
        string? operatoreId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        if (operatoreId == null)
        {
            return Unauthorized("Utente non autenticato.");
        }

        if (dto == null || dto.Importo <= 0)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(operatoreId, "RicaricaGiftCard", false);
            return BadRequest(new { errore = "L'importo della Gift Card deve essere maggiore di zero." });
        }

        try
        {
            await _operatoreService.RicaricaGiftCardAsync(dto);

            await _logAzioniService.SalvataggioLogAzioneAsync(operatoreId, "RicaricaGiftCard", true);
            
            return Ok();
        }
        catch (Exception ex)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(operatoreId, "RicaricaGiftCard", false);
            return BadRequest(new { errore = ex.Message });
        }
    }
}
```


## OperatoreUtentiController.cs

```c#
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using NuovoCinemaParadiso.Dtos;
using NuovoCinemaParadiso.Services;
using NuovoCinemaParadiso.Exceptions;

namespace NuovoCinemaParadiso.Controllers;

[ApiController] // Espone un controller API REST
[Route("api/[controller]")] // Route base: api/OperatoreUtenti
[Authorize(Roles = Ruoli.Operatore)] // Solo utenti con ruolo Operatore possono accedere
public class OperatoreUtentiController : ControllerBase
{
    private readonly RuoloUtenteService _ruoloUtenteService;
    private readonly LogAzioniService _logAzioniService;

    public OperatoreUtentiController(
        RuoloUtenteService ruoloUtenteService,
        LogAzioniService logAzioniService)
    {
        _ruoloUtenteService = ruoloUtenteService;
        _logAzioniService = logAzioniService;
    }

    [HttpPut("cambia-ruolo")] // PUT api/OperatoreUtenti/cambia-ruolo
    public async Task<IActionResult> CambiaRuolo([FromBody] DtoModificaRuoloUtente dto)
    {
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier); // id utente che esegue l'azione
        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");

        string? nuovoRuolo = await _ruoloUtenteService.ModificaRuoloUtente(dto);

        if (nuovoRuolo == null)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(
                utenteId,
                "Cambio ruolo",
                false);

            return BadRequest(new { messaggio = "Utente o ruolo non valido." });
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(
            utenteId,
            "Cambio ruolo",
            true);

        return Ok(new
        {
            messaggio = "Ruolo aggiornato correttamente.",
            email = dto.Email,
            ruolo = nuovoRuolo
        });
    }
}
```

## ProiezioneController.cs

```c#
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using NuovoCinemaParadiso.Dtos;
using NuovoCinemaParadiso.Services;
using NuovoCinemaParadiso.Exceptions;

namespace NuovoCinemaParadiso.Controllers;

[ApiController] // Espone un controller API REST
[Route("api/[controller]")] // Route base: api/Proiezione
[Authorize] // Richiede autenticazione per tutte le azioni
public class ProiezioneController : ControllerBase
{
    private readonly ProiezioneService _proiezioneService;
    private readonly LogAzioniService _logAzioniService;

    public ProiezioneController(
        ProiezioneService proiezioneService,
        LogAzioniService logAzioniService)
    {
        _proiezioneService = proiezioneService;
        _logAzioniService = logAzioniService;
    }

    [HttpGet] // GET api/Proiezione
    public async Task<IActionResult> OttieniTutteLeProiezioni()
    {
        List<DtoProiezione> proiezioni = await _proiezioneService.OttieniTuttoAsync();

        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");

        await _logAzioniService.SalvataggioLogAzioneAsync(
            utenteId,
            "Ottieni tutte le proiezioni",
            true);

        return Ok(proiezioni);
    }

    [HttpGet("storico")] // GET api/Proiezione/storico
    [Authorize(Roles = Ruoli.Operatore)] // solo Operatore
    public async Task<IActionResult> OttieniStoricoProiezioni()
    {
        List<DtoProiezione> proiezioni = await _proiezioneService.OttieniStoricoAsync();

        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");

        await _logAzioniService.SalvataggioLogAzioneAsync(
            utenteId,
            "Ottieni storico proiezioni",
            true);

        return Ok(proiezioni);
    }

    [HttpGet("{id}")] // GET api/Proiezione/{id}
    public async Task<IActionResult> OttieniTramiteId(string id)
    {
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");

        var risultato = await _proiezioneService.OttieniTramiteIdAsync(id);

        if (risultato == null)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(
                utenteId,
                "Ottieni proiezione tramite id",
                false);

            return NotFound($"Proiezione con id {id} non trovato");
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(
            utenteId,
            "Ottieni proiezione tramite id",
            true);

        return Ok(risultato);
    }

    [HttpGet("turno/{turnoId}")] // GET per turno
    public async Task<ActionResult<List<DtoProiezione>>> OttieniPerTurno(string turnoId)
    {
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");

        if (string.IsNullOrEmpty(turnoId))
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(
                utenteId,
                "Ottieni proiezione per turno",
                false);

            return BadRequest("TurnoId non valido");
        }

        var risultato = await _proiezioneService.OttieniTramiteTurnoAsync(turnoId);

        if (risultato == null)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(
                utenteId,
                "Ottieni proiezione per turno",
                false);

            return NotFound("Nessuna proiezione trovata per questo turno");
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(
            utenteId,
            "Ottieni proiezione per turno",
            true);

        return Ok(risultato);
    }

    [HttpGet("sala/{salaId}")] // GET per sala
    public async Task<ActionResult<List<DtoProiezione>>> OttieniPerSala(string salaId)
    {
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");

        if (string.IsNullOrEmpty(salaId))
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(
                utenteId,
                "Ottieni proiezione per sala",
                false);

            return BadRequest("SalaId non valido");
        }

        var risultato = await _proiezioneService.OttieniTramiteSalaAsync(salaId);

        if (risultato == null)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(
                utenteId,
                "Ottieni proiezione per sala",
                false);

            return NotFound("Nessuna proiezione trovata per questa sala");
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(
            utenteId,
            "Ottieni proiezione per sala",
            true);

        return Ok(risultato);
    }

    [HttpGet("movie/{movieId}")] // GET per film
    public async Task<ActionResult<List<DtoProiezione>>> OttieniPerFilm(string movieId)
    {
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");

        if (string.IsNullOrEmpty(movieId))
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(
                utenteId,
                "Ottieni proiezioni per film",
                false);

            return BadRequest("MovieId non valido");
        }

        var risultato = await _proiezioneService.OttieniTramiteMovieAsync(movieId);

        if (risultato == null)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(
                utenteId,
                "Ottieni proiezioni per film",
                false);

            return NotFound("Nessuna proiezione trovata per questo film");
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(
            utenteId,
            "Ottieni proiezioni per film",
            true);

        return Ok(risultato);
    }

    [HttpPost] // POST api/Proiezione
    [Authorize(Roles = Ruoli.Operatore)] // solo Operatore
    public async Task<IActionResult> Creazione([FromBody] DtoCreazioneProiezione dto)
    {
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");

        // Controllo duplicati (stessa sala, turno e data)
        List<DtoProiezione> proiezioni = await _proiezioneService.OttieniTuttoAsync();

        foreach (var proiezione in proiezioni)
        {
            if (proiezione.TurnoId == dto.TurnoId &&
                proiezione.SalaId == dto.SalaId &&
                proiezione.DataProiezione == dto.DataProiezione)
            {
                await _logAzioniService.SalvataggioLogAzioneAsync(
                    utenteId,
                    "Crea proiezione",
                    false);

                return BadRequest(new { messaggio = "Proiezione già presente." });
            }
        }

        DtoProiezione? risultato = await _proiezioneService.CreazioneAsync(dto);

        if (risultato == null)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(
                utenteId,
                "Crea proiezione",
                false);

            return BadRequest(new { messaggio = "Proiezione non valida." });
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(
            utenteId,
            "Crea proiezione",
            true);

        return Ok(risultato);
    }

    [HttpPut("{id}")] // PUT api/Proiezione/{id}
    [Authorize(Roles = Ruoli.Operatore)]
    public async Task<IActionResult> Modifica(string id, [FromBody] DtoCreazioneProiezione dto)
    {
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");

        DtoProiezione? risultato = await _proiezioneService.ModificaAsync(id, dto);

        if (risultato == null)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(
                utenteId,
                "Modifica proiezione",
                false);

            return NotFound(new { messaggio = "Proiezione non trovata." });
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(
            utenteId,
            "Modifica proiezione",
            true);

        return Ok(risultato);
    }

    [HttpPut("elimina/{id}")] // PUT usato come soft-delete o logica custom
    [Authorize(Roles = Ruoli.Operatore)]
    public async Task<IActionResult> Elimina(string id)
    {
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");

        bool eliminato = await _proiezioneService.EliminaAsync(id);

        if (!eliminato)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(
                utenteId,
                "Elimina proiezione",
                false);

            return NotFound(new { messaggio = "Proiezione non trovata." });
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(
            utenteId,
            "Elimina proiezione",
            true);

        return Ok(new { messaggio = "Proiezione eliminata con successo!" });
    }
}

```
## ProiezioneController.cs Versione 1.1 
- Utente: Simeone
- Data: 21/05/2026
- Descrizione: eseguo la task assegnatami sul service e conseguentemente su questo controller: "MODIFICARE CONTROLLER E SERVICE CHE NON HANNO LA NECESSITà DI AVERE DTO IN USCITA CHE PORTINO DATI NON NECESSARI AL FRONTEND.
DI CONSEGUENZA ALCUNI AZIONI RIPORTERANNO SOLO UN TRUE O UN FALSE CON UN MESSAGGIO DI RIUSCITA O FALLIMENTO."
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
[Authorize]
public class ProiezioneController : ControllerBase
{
    private readonly ProiezioneService _proiezioneService;
    private readonly LogAzioniService _logAzioniService;

    public ProiezioneController(ProiezioneService proiezioneService, LogAzioniService logAzioniService)
    {
        _proiezioneService = proiezioneService;
        _logAzioniService = logAzioniService;
    }

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

    [HttpGet("storico")]
    [Authorize(Roles = Ruoli.Operatore)]
    public async Task<IActionResult> OttieniStoricoProiezioni()
    {
        List<DtoProiezione> proiezioni = await _proiezioneService.OttieniStoricoAsync();
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");

        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ottieni tutte le proieioni",true);

        return Ok(proiezioni);
    }

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

        bool creato = await _proiezioneService.CreazioneAsync(dto); // modificato in bool per evitare di restituire troppi dati non necessari

        if (!creato)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Crea proiezione", false);
            return BadRequest(new { messaggio = "Proiezione non valida." });
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Crea proiezione", true);
        return Ok(new { messaggio = "Creazione avvenuta con successo!" }); // messaggio di conferma pulito
    }

    [HttpPut("{id}")]
    [Authorize(Roles = Ruoli.Operatore)]
    public async Task<IActionResult> Modifica(string id, [FromBody] DtoCreazioneProiezione dto)
    {
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");

        bool modificato = await _proiezioneService.ModificaAsync(id,dto); // modificato in bool per evitare di restituire troppi dati non necessari

        if (!modificato)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Modifica proiezione", false);

            return NotFound(new { messaggio = "Proiezione non trovata." });
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Modifica proiezione", true);

        return Ok(new { messaggio = "Proiezione modificata con successo!"}); // messaggio di conferma pulito
    }

    [HttpPut("elimina/{id}")] //<- usiamo PUT con /elimina/id e non DELETE con /id perché il nostro obbiettivo non è eliminare il campo, bensì renderlo inattivo, in modo che possa comunque apparire nello storico proiezioni
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
##  SalaController.cs

```c#
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using NuovoCinemaParadiso.Dtos;
using NuovoCinemaParadiso.Services;
using NuovoCinemaParadiso.Exceptions;

namespace NuovoCinemaParadiso.Controllers;

[ApiController] // Controller API REST
[Route("api/[controller]")] // Route base: api/Sala
[Authorize] // Richiede autenticazione per tutte le azioni
public class SalaController : ControllerBase
{
    private readonly SalaService _salaService;
    private readonly LogAzioniService _logAzioniService;

    public SalaController(
        SalaService salaService,
        LogAzioniService logAzioniService)
    {
        _salaService = salaService;
        _logAzioniService = logAzioniService;
    }

    [HttpGet] // GET api/Sala
    public async Task<IActionResult> OttieniTutti()
    {
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");

        List<DtoSala> sale = await _salaService.OttieniTuttoAsync();

        await _logAzioniService.SalvataggioLogAzioneAsync(
            utenteId,
            "Ottieni tutte le sale",
            true);

        return Ok(sale);
    }

    [HttpGet("tipologia/{tipologiaId}")] // GET per tipologia sala
    public async Task<ActionResult<List<DtoSala>>> OttieniPerTipologia(string tipologiaId)
    {
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");

        if (string.IsNullOrWhiteSpace(tipologiaId))
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(
                utenteId,
                "Ottieni sala per tipologia",
                false);

            return BadRequest("TipologiaId non valida");
        }

        var risultato = await _salaService.OttieniTramiteTipologiaAsync(tipologiaId);

        if (risultato == null || risultato.Count == 0)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(
                utenteId,
                "Ottieni sala per tipologia",
                false);

            return NotFound("Nessuna sala trovata per questa tipologia");
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(
            utenteId,
            "Ottieni sala per tipologia",
            true);

        return Ok(risultato);
    }

    [HttpGet("{id}")] // GET api/Sala/{id}
    public async Task<IActionResult> OttieniTramiteId(string id)
    {
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");

        var risultato = await _salaService.OttieniTramiteIdAsync(id);

        if (risultato == null)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(
                utenteId,
                "Ottieni sala tramite id",
                false);

            return NotFound($"Sala con id {id} non trovato");
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(
            utenteId,
            "Ottieni sala tramite id",
            true);

        return Ok(risultato);
    }

    [HttpPost] // POST api/Sala
    [Authorize(Roles = Ruoli.Operatore)] // solo Operatore
    public async Task<IActionResult> Creazione([FromBody] DtoCreazioneSala dto)
    {
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");

        DtoSala? risultato = await _salaService.CreazioneAsync(dto);

        if (risultato == null)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(
                utenteId,
                "Creazione sala",
                false);

            return BadRequest(new { messaggio = "Sala non valida." });
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(
            utenteId,
            "Creazione sala",
            true);

        return Ok(risultato);
    }

    [HttpPut("{id}")] // PUT api/Sala/{id}
    [Authorize(Roles = Ruoli.Operatore)]
    public async Task<IActionResult> Modifica(string id, [FromBody] DtoCreazioneSala dto)
    {
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");

        try
        {
            DtoSala? risultato = await _salaService.ModificaAsync(id, dto);

            await _logAzioniService.SalvataggioLogAzioneAsync(
                utenteId,
                "Modifica sala",
                true);

            return Ok(risultato);
        }
        catch (ModificaException ex)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(
                utenteId,
                "Modifica sala",
                false);

            return BadRequest(new { message = ex.Message });
        }
        catch (ItemNotFoundException ex)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(
                utenteId,
                "Modifica sala",
                false);

            return NotFound(new { message = ex.Message });
        }
        catch (NotFoundException ex)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(
                utenteId,
                "Modifica sala",
                false);

            return NotFound(new { message = ex.Message });
        }
    }

    [HttpDelete("{id}")] // DELETE api/Sala/{id}
    [Authorize(Roles = Ruoli.Operatore)]
    public async Task<IActionResult> Elimina(string id)
    {
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");

        bool eliminato = await _salaService.EliminaAsync(id);

        if (!eliminato)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(
                utenteId,
                "Elimina sala",
                false);

            return NotFound(new { messaggio = "Sala non trovata." });
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(
            utenteId,
            "Elimina sala",
            true);

        return Ok(new { messaggio = "Sala eliminata con successo!" });
    }
}
```
## SalaControllerV1.2

Francesco Lorenzi
22/05/2026

modificati i tipi di risposta accettabili per la creazione e la modifica
```c#
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using NuovoCinemaParadiso.Dtos;
using NuovoCinemaParadiso.Services;
using NuovoCinemaParadiso.Exceptions;

namespace NuovoCinemaParadiso.Controllers;

[ApiController] 
[Route("api/[controller]")] 
[Authorize]
public class SalaController : ControllerBase
{
    private readonly SalaService _salaService;
    private readonly LogAzioniService _logAzioniService;

    public SalaController(
        SalaService salaService,
        LogAzioniService logAzioniService)
    {
        _salaService = salaService;
        _logAzioniService = logAzioniService;
    }

    [HttpGet] 
    public async Task<IActionResult> OttieniTutti()
    {
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");

        List<DtoSala> sale = await _salaService.OttieniTuttoAsync();

        await _logAzioniService.SalvataggioLogAzioneAsync(
            utenteId,
            "Ottieni tutte le sale",
            true);

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
            await _logAzioniService.SalvataggioLogAzioneAsync(
                utenteId,
                "Ottieni sala per tipologia",
                false);

            return BadRequest("TipologiaId non valida");
        }

        var risultato = await _salaService.OttieniTramiteTipologiaAsync(tipologiaId);

        if (risultato == null || risultato.Count == 0)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(
                utenteId,
                "Ottieni sala per tipologia",
                false);

            return NotFound("Nessuna sala trovata per questa tipologia");
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(
            utenteId,
            "Ottieni sala per tipologia",
            true);

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
            await _logAzioniService.SalvataggioLogAzioneAsync(
                utenteId,
                "Ottieni sala tramite id",
                false);

            return NotFound($"Sala con id {id} non trovato");
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(
            utenteId,
            "Ottieni sala tramite id",
            true);

        return Ok(risultato);
    }

    [HttpPost] 
    [Authorize(Roles = Ruoli.Operatore)] 
    public async Task<IActionResult> Creazione([FromBody] DtoCreazioneSala dto)
    {
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");

        var risultato = await _salaService.CreazioneAsync(dto);//ora accetta un var

        if (risultato == null)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(
                utenteId,
                "Creazione sala",
                false);

            return BadRequest(new { messaggio = "Sala non valida." });
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(
            utenteId,
            "Creazione sala",
            true);

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
            var risultato = await _salaService.ModificaAsync(id, dto);// ora accetta un var

            await _logAzioniService.SalvataggioLogAzioneAsync(
                utenteId,
                "Modifica sala",
                true);

            return Ok(risultato);
        }
        catch (ModificaException ex)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(
                utenteId,
                "Modifica sala",
                false);

            return BadRequest(new { message = ex.Message });
        }
        catch (ItemNotFoundException ex)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(
                utenteId,
                "Modifica sala",
                false);

            return NotFound(new { message = ex.Message });
        }
        catch (NotFoundException ex)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(
                utenteId,
                "Modifica sala",
                false);

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
            await _logAzioniService.SalvataggioLogAzioneAsync(
                utenteId,
                "Elimina sala",
                false);

            return NotFound(new { messaggio = "Sala non trovata." });
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(
            utenteId,
            "Elimina sala",
            true);

        return Ok(new { messaggio = "Sala eliminata con successo!" });
    }
}
```
##  TipologiaSalaController.cs

```c#
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using NuovoCinemaParadiso.Dtos;
using NuovoCinemaParadiso.Services;
using NuovoCinemaParadiso.Exceptions;

namespace NuovoCinemaParadiso.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using NuovoCinemaParadiso.Services;
using NuovoCinemaParadiso.Dtos;
using NuovoCinemaParadiso.Models;

namespace NuovoCinemaParadiso.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TipologiaSalaController : ControllerBase
{
    private readonly TipologiaSalaService _tipologiaSalaService;
    private readonly LogAzioniService _logAzioniService;

    public TipologiaSalaController(TipologiaSalaService tipologiaSalaService, LogAzioniService logAzioniService)
    {
        _tipologiaSalaService = tipologiaSalaService;
        _logAzioniService = logAzioniService;
    }

    [HttpGet]
    public async Task<IActionResult> OttieniTutti()
    {
        List<DtoTipologiaSala> tipologieSala = await _tipologiaSalaService.OttieniTuttoAsync();

        // Recupera l'ID dell'utente autenticato dal token JWT
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");

        // Log dell’operazione riuscita
        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ottieni tutte le tipologie", true);

        return Ok(tipologieSala);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> OttieniTramiteId(string id)
    {
        var risultato = await _tipologiaSalaService.OttieniTramiteIdAsync(id);

        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");

        // Se non esiste la risorsa, viene registrato anche il fallimento nel log
        if (risultato == null)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ottieni tipologie tramite id", false);
            return NotFound($"TipologiaSala con id {id} non trovato");
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ottieni tipologie tramite id", true);

        return Ok(risultato);
    }

    [HttpPost]
    [Authorize(Roles = Ruoli.Operatore)]
    public async Task<IActionResult> Creazione([FromBody] DtoCreazioneTipologiaSala dto)
    {
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");

        // Controllo duplicati prima della creazione
        List<DtoTipologiaSala> tipologieSala = await _tipologiaSalaService.OttieniTuttoAsync();

        foreach (var tipologiaSala in tipologieSala)
        {
            if (tipologiaSala.Nome.Contains(dto.Nome))
            {
                await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Creazione tipologia", false);
                return BadRequest(new { messaggio = "Tipologia sala già presente." });
            }
        }

        DtoTipologiaSala? risultato = await _tipologiaSalaService.CreazioneAsync(dto);

        if (risultato == null)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Creazione tipologia", false);
            return BadRequest(new { messaggio = "Tipologia sala non valida." });
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Creazione tipologia", true);

        return Ok(risultato);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = Ruoli.Operatore)]
    public async Task<IActionResult> Modifica(string id, [FromBody] DtoCreazioneTipologiaSala dto)
    {
        DtoTipologiaSala? risultato = await _tipologiaSalaService.ModificaAsync(id, dto);

        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");

        // Se la modifica non trova o non aggiorna la risorsa
        if (risultato == null)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Modifica tipologia", false);
            return NotFound(new { messaggio = "Tipologia sala non trovata." });
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Modifica tipologia", true);

        return Ok(risultato);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = Ruoli.Operatore)]
    public async Task<IActionResult> Elimina(string id)
    {
        bool eliminato = await _tipologiaSalaService.EliminaAsync(id);

        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");

        // Se l'eliminazione fallisce, logga il fallimento
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

##  TurnoController.cs

```c#
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using NuovoCinemaParadiso.Dtos;
using NuovoCinemaParadiso.Services;
using NuovoCinemaParadiso.Exceptions;

namespace NuovoCinemaParadiso.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using NuovoCinemaParadiso.Dtos;
using NuovoCinemaParadiso.Services;

namespace NuovoCinemaParadiso.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TurnoController : ControllerBase
{
    private readonly TurnoService _turnoService;
    private readonly LogAzioniService _logAzioniService;

    public TurnoController(TurnoService turnoService, LogAzioniService logAzioniService)
    {
        _turnoService = turnoService;
        _logAzioniService = logAzioniService;
    }

    [HttpGet]
    public async Task<IActionResult> OttieniTutti()
    {
        // Identifica l’utente dal token JWT (necessario per il logging delle azioni)
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null) return Unauthorized("Utente non autenticato.");

        List<DtoTurno> turni = await _turnoService.OttieniTuttoAsync();
        
        // Log operazione di lettura
        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ottieni tutti i turni", true);

        return Ok(turni);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> OttieniTramiteId(string id)
    {
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null) return Unauthorized("Utente non autenticato.");

        var risultato = await _turnoService.OttieniTramiteIdAsync(id);

        // Se il turno non esiste viene registrato fallimento nel log
        if (risultato == null)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ottieni turno tramite id", false);
            return NotFound(new { messaggio = $"Turno con id {id} non trovato" });
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ottieni turno tramite id", true);
        return Ok(risultato);
    }

    [HttpPost]
    [Authorize(Roles = Ruoli.Operatore)]
    public async Task<IActionResult> Creazione([FromBody] DtoCreazioneTurno dto)
    {
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null) return Unauthorized("Utente non autenticato.");

        // Il service restituisce sia risultato che eventuale errore
        var (risultato, errore) = await _turnoService.CreazioneAsync(dto);

        if (errore != null)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Creazione Turno", false);
            return BadRequest(new { messaggio = errore });
        }

        // Caso di errore inatteso lato server
        if (risultato == null)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Creazione Turno", false);
            return StatusCode(500, new { messaggio = "Errore generico durante la creazione." });
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Creazione Turno", true);
        return Ok(risultato);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = Ruoli.Operatore)]
    public async Task<IActionResult> Modifica(string id, [FromBody] DtoCreazioneTurno dto)
    {
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null) return Unauthorized("Utente non autenticato.");

        var (risultato, errore) = await _turnoService.ModificaAsync(id, dto);

        if (errore == "Turno non trovato.")
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Modifica Turno", false);
            return NotFound(new { messaggio = errore });
        }
        else if (errore != null)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Modifica Turno", false);
            return BadRequest(new { messaggio = errore });
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Modifica Turno", true);
        return Ok(risultato);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = Ruoli.Operatore)]
    public async Task<IActionResult> Elimina(string id)
    {
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null) return Unauthorized("Utente non autenticato.");

        var (successo, errore) = await _turnoService.EliminaAsync(id);

        if (!successo)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Elimina Turno", false);
            
            // Distinzione tra errore "non trovato" e altri errori
            if (errore == "Turno non trovato.")
                return NotFound(new { messaggio = errore });

            return BadRequest(new { messaggio = errore });
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Elimina Turno", true);
        return NoContent();
    }
}
```

##  UtenteController.cs

```c#
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using NuovoCinemaParadiso.Services;
using NuovoCinemaParadiso.Dtos;
using NuovoCinemaParadiso.Exceptions;

namespace NuovoCinemaParadiso.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UtenteController : ControllerBase
{
    private readonly UtenteService _utenteService;
    private readonly LogAzioniService _logAzioniService;

    public UtenteController(UtenteService utenteService, LogAzioniService logAzioniService)
    {
        _utenteService = utenteService;
        _logAzioniService = logAzioniService;
    }

    // Endpoint HTTP GET che permette all’utente autenticato di visualizzare
// tutti i biglietti che ha acquistato. 
// La route finale sarà:  GET /api/Utente/biglietti
[HttpGet("biglietti")]
public async Task<IActionResult> OttieniTuttiBiglietti()
{
    // Recupera l'ID dell'utente attualmente autenticato leggendo il token JWT.
    // Questo ID è fondamentale per filtrare i biglietti appartenenti al singolo utente.
    string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);    

    // Se per qualche motivo il token non contiene l'ID, l'utente non è considerato autenticato.
    if(utenteId == null)
    {
        return Unauthorized("Utente non autenticato.");
    }

    // Chiede al servizio UtenteService di recuperare TUTTI i biglietti
    // appartenenti a questo utente. Il service si occupa della logica di filtraggio
    // e della costruzione dei DTO.
    var biglietti = await _utenteService.OttieniTuttiBigliettiAsync(utenteId);

    // Registra nel sistema di logging che l’utente ha richiesto la lista dei suoi biglietti.
    // Questo serve per audit, tracciamento e sicurezza.
    await _logAzioniService.SalvataggioLogAzioneAsync(
        utenteId, 
        "Ottieni tutti i biglietti", 
        true
    );

    // Restituisce al client la lista dei biglietti in formato JSON.
    // Se l’utente non ha biglietti, verrà restituita una lista vuota (comportamento corretto).
    return Ok(biglietti);
}

    [HttpPost("abbonati")]
    public async Task<IActionResult> Abbonati([FromBody] DtoUtente dto)
    {
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        // Controllo autenticazione tramite JWT
        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");

        // Validazione minima input
        if (dto == null || string.IsNullOrEmpty(dto.AbbonamentoId))
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Abbonati", false);
            return BadRequest("Dati non validi");
        }

        try
        {
            var risultato = await _utenteService.AbbonatiAsync(dto.AbbonamentoId, utenteId);

            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Abbonati", true);

            return Ok(risultato);
        }
        catch (NotFoundException ex)
        {
            // ATTENZIONE: qui viene loggato dto.AbbonamentoId invece di utenteId (probabile bug logico)
            await _logAzioniService.SalvataggioLogAzioneAsync(dto.AbbonamentoId, "Abbonati", false);
            return NotFound(new { errore = ex.Message });
        }
        catch (ItemAlredyexist ex)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(dto.AbbonamentoId, "Abbonati", false);
            return NotFound(new { errore = ex.Message });
        }
    }

    [HttpPut("giftCard/riscatta")]
    public async Task<IActionResult> RiscattaGiftCard([FromBody] string giftCardCodiceRiscatto)
    {
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");

        if (string.IsNullOrEmpty(giftCardCodiceRiscatto))
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "RiscattoGiftCard", false);
            return BadRequest("Codice riscatto non valido.");
        }

        try
        {
            DtoGiftCard risultato =
                await _utenteService.RiscattaGiftCardAsync(giftCardCodiceRiscatto, utenteId);

            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "RiscattoGiftCard", true);

            return Ok(risultato);
        }
        catch (NotFoundException ex)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "RiscattoGiftCard", false);
            return NotFound(new { errore = ex.Message });
        }
        catch (Exception ex)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "RiscattoGiftCard", false);
            return BadRequest(new { errore = ex.Message });
        }
    }

    [HttpPost("giftCard/ricarica")]
    public async Task<IActionResult> RicaricaGiftCard([FromBody] DtoRicaricaGiftCard dto)
    {
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");

        // Validazione importo
        if (dto == null || dto.Importo <= 0)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "RicaricaGiftCard", false);
            return BadRequest(new { errore = "L'importo della ricarica deve essere maggiore di zero." });
        }

        try
        {
            var risultato = await _utenteService.RicaricaGiftCardAsync(utenteId, dto);

            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "RicaricaGiftCard", true);

            return Ok(risultato);
        }
        catch (Exception ex)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "RicaricaGiftCard", false);
            return BadRequest(new { errore = ex.Message });
        }
    }
}
```







# Services

## AbbonamentoService.cs

```c#
using Microsoft.EntityFrameworkCore;
using NuovoCinemaParadiso.Data;
using NuovoCinemaParadiso.Dtos;
using NuovoCinemaParadiso.Models;

namespace NuovoCinemaParadiso.Services;

public class AbbonamentoService
{
    // DbContext per accedere al database
    private readonly ContestoDb _contesto;

    // Inietta il DbContext nel service
    public AbbonamentoService(ContestoDb contesto)
    {
        _contesto = contesto;
    }

    // -----------------------------
    // OTTIENI TUTTI GLI ABBONAMENTI
    // -----------------------------
    public async Task<List<DtoAbbonamento>> OttieniTutto()
    {
        // Carica tutti gli abbonamenti dal database
        List<Abbonamento> abbonamenti = await _contesto.Abbonamenti.ToListAsync();

        // Lista DTO da restituire al frontend
        List<DtoAbbonamento> risultato = new List<DtoAbbonamento>();

        // Converte ogni entità in un DTO
        for (int i = 0; i < abbonamenti.Count; i++)
        {
            Abbonamento a = abbonamenti[i];

            DtoAbbonamento dto = new DtoAbbonamento();
            dto.Id = a.Id;
            dto.Nome = a.Nome;
            dto.Durata = a.Durata;
            dto.Prezzo = a.Prezzo;
            dto.Sconto = a.Sconto;

            risultato.Add(dto);
        }

        // Restituisce la lista completa
        return risultato;
    }

    // -----------------------------
    // OTTIENI ABBONAMENTO SOLO SE È DELL’UTENTE
    // -----------------------------
    public async Task<DtoAbbonamento?> OttieniTramiteIdAsync(string id, string utenteId)
    {
        // Cerca l’abbonamento richiesto
        Abbonamento? abbonamento = await _contesto.Abbonamenti.FindAsync(id);

        // Se non esiste → niente da restituire
        if (abbonamento == null)
            return null;

        // Carica tutti gli utenti per verificare la proprietà dell’abbonamento
        List<Utente> utenti = await _contesto.Utenti.ToListAsync();

        // Flag per capire se l’utente possiede l’abbonamento
        bool trovato = false;

        // Cerca un utente che abbia quell’abbonamento
        for (int i = 0; i < utenti.Count; i++)
        {
            if (utenti[i].Id == utenteId &&
                utenti[i].AbbonamentoId == abbonamento.Id)
            {
                trovato = true;
                break;
            }
        }

        // Se l’utente non lo possiede → non autorizzato a vederlo
        if (!trovato)
            return null;

        // Converte l’entità in DTO
        DtoAbbonamento risultato = new DtoAbbonamento();
        risultato.Id = abbonamento.Id;
        risultato.Nome = abbonamento.Nome;
        risultato.Durata = abbonamento.Durata;
        risultato.Prezzo = abbonamento.Prezzo;
        risultato.Sconto = abbonamento.Sconto;

        return risultato;
    }

    // -----------------------------
    // CREAZIONE ABBONAMENTO
    // -----------------------------
    public async Task<(bool Successo, string Messaggio)> CreazioneAsync(DtoCreazioneAbbonamento dto)
    {
        // Carica tutti gli abbonamenti per controllare duplicati
        List<Abbonamento> abbonamenti = await _contesto.Abbonamenti.ToListAsync();

        // Verifica se esiste già un abbonamento con lo stesso nome
        for (int i = 0; i < abbonamenti.Count; i++)
        {
            if (abbonamenti[i].Nome.ToLower() == dto.Nome.ToLower())
                return (false, "Esiste già un abbonamento con questo nome.");
        }

        // Crea un nuovo abbonamento
        Abbonamento nuovo = new Abbonamento();
        nuovo.Nome = dto.Nome;
        nuovo.Durata = dto.Durata;
        nuovo.Prezzo = dto.Prezzo;
        nuovo.Sconto = dto.Sconto;

        // Salva nel database
        _contesto.Abbonamenti.Add(nuovo);
        await _contesto.SaveChangesAsync();

        return (true, "Abbonamento creato correttamente.");
    }

    // -----------------------------
    // MODIFICA ABBONAMENTO
    // -----------------------------
    public async Task<(bool Successo, string Messaggio)> ModificaAsync(string id, DtoCreazioneAbbonamento dto)
    {
        // Cerca l’abbonamento da modificare
        Abbonamento? abbonamento = await _contesto.Abbonamenti.FindAsync(id);

        // Se non esiste → errore
        if (abbonamento == null)
            return (false, "Abbonamento non trovato.");

        // Carica tutti gli abbonamenti per controllare duplicati
        List<Abbonamento> abbonamenti = await _contesto.Abbonamenti.ToListAsync();

        // Verifica se un altro abbonamento usa lo stesso nome
        for (int i = 0; i < abbonamenti.Count; i++)
        {
            if (abbonamenti[i].Id != id &&
                abbonamenti[i].Nome.ToLower() == dto.Nome.ToLower())
            {
                return (false, "Esiste già un altro abbonamento con questo nome.");
            }
        }

        // Aggiorna i campi dell’abbonamento
        abbonamento.Nome = dto.Nome;
        abbonamento.Durata = dto.Durata;
        abbonamento.Prezzo = dto.Prezzo;
        abbonamento.Sconto = dto.Sconto;

        // Salva le modifiche
        await _contesto.SaveChangesAsync();

        return (true, "Abbonamento modificato correttamente.");
    }

    // -----------------------------
    // ELIMINAZIONE ABBONAMENTO
    // -----------------------------
    public async Task<(bool Successo, string Messaggio)> EliminazioneAsync(string id)
    {
        // Cerca l’abbonamento da eliminare
        Abbonamento? abbonamento = await _contesto.Abbonamenti.FindAsync(id);

        // Se non esiste → errore
        if (abbonamento == null)
            return (false, "Abbonamento non trovato.");

        // Rimuove l’abbonamento dal database
        _contesto.Abbonamenti.Remove(abbonamento);

        // Applica la modifica
        await _contesto.SaveChangesAsync();

        return (true, "Abbonamento eliminato correttamente.");
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

Utente: Marco Strazzeri
Data: 22/05/2026
Descrizione: Modificato valore di ritorno in registrazione

```c#

using Microsoft.AspNetCore.Identity;
using NuovoCinemaParadiso.Models;
using NuovoCinemaParadiso.Dtos;
using NuovoCinemaParadiso.Helpers;
using NuovoCinemaParadiso.Data;
using NuovoCinemaParadiso.Exceptions;

namespace NuovoCinemaParadiso.Services;

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

    public async Task<(bool successo, string? Errore)> RegistrazioneAsync(DtoRegistrazione dto)
    {
        Utente? esisteUtente = await _gestioneUtenti.FindByEmailAsync(dto.Email);

        if (esisteUtente != null)
        {
            IdentityError errore = new IdentityError();
            errore.Description = "Utente già registrato.";

            List<IdentityError> errori = new List<IdentityError>();
            errori.Add(errore);

            return (false, "Utente già registrato.");
        }

        if (!dto.Email.Contains('.'))
        {
            throw new InvalidEmail(dto.Email);
        }

        Utente utente = new Utente();
        utente.UserName = dto.Email;
        utente.Email = dto.Email;
        utente.NomeCompleto = dto.NomeCompleto;
        utente.Eta = dto.Eta;
        utente.Saldo = 1000;

        IdentityResult risultato = await _gestioneUtenti.CreateAsync(utente, dto.Password);

        if (!risultato.Succeeded)
        {
            return (false, "Errore durante la registrazione.");
        }
        await _gestioneUtenti.AddToRoleAsync(utente, Ruoli.Utente);
        //ritorna true perché la registrazione ha avuto successo
        return (true,null);
    }

    public async Task<DtoAuthResponse?> LoginAsync(DtoLogin dto)
    {
        Utente? utente = await _gestioneUtenti.FindByEmailAsync(dto.Email);

        if (utente == null)
        {
            throw new NotFoundException("Utente", dto.Email);
        }

        Abbonamento? abbonamento = null;
        if (!string.IsNullOrEmpty(utente.AbbonamentoId))
        {
            abbonamento = await _contesto.Abbonamenti.FindAsync(utente.AbbonamentoId);
        }

        if (utente.SeAbbonato == true && abbonamento != null)
        {
            DateTimeOffset? scadenzaAbbonamento = Calcoli.CalcolaScadenza(utente.DataInizioAbbonamento, abbonamento.Durata);
            int giorniMancanti = Calcoli.GiorniAllaScadenza(utente.DataInizioAbbonamento, abbonamento.Durata);

            if (giorniMancanti <= 0)
            {
                utente.SeAbbonato = false;
            }
        }

        SignInResult result = await _gestioneAccesso.CheckPasswordSignInAsync(utente, dto.Password, false);

        if (!result.Succeeded)
        {
            throw new ConflictException("Password errata");
        }

        IList<string> ruoli = await _gestioneUtenti.GetRolesAsync(utente);

        string token = _jwtHelper.GenerateToken(utente, ruoli);

        DtoAuthResponse response = new DtoAuthResponse();
        response.Token = token;
        response.Id = utente.Id;
        response.NomeCompleto = utente.NomeCompleto;
        response.Email = utente.Email ?? string.Empty;
        response.Eta = utente.Eta;
        response.DataInizioAbbonamento = utente.DataInizioAbbonamento;
        response.SeAbbonato = utente.SeAbbonato;
        response.Saldo = utente.Saldo;

        if (ruoli.Count > 0)
        {
            response.Ruolo = ruoli[0];
        }
        else
        {
            response.Ruolo = "";
        }

        return response;
    }

    public async Task<DtoUtente?> OttieniTramiteIdAsync(string id)
    {
        Utente? utente = await _gestioneUtenti.FindByIdAsync(id)
            ?? throw new NotFoundException("Utente", id);

        Abbonamento? abbonamento = await _contesto.Abbonamenti.FindAsync(utente.AbbonamentoId);

        DtoUtente dto = new DtoUtente();
        dto.Id = utente.Id;
        dto.Email = utente.Email ?? string.Empty;
        dto.NomeCompleto = utente.NomeCompleto ?? string.Empty;
        dto.Eta = utente.Eta;
        dto.SeAbbonato = utente.SeAbbonato;
        dto.AbbonamentoId = utente?.AbbonamentoId ?? string.Empty;
        dto.TipoAbbonamento = abbonamento?.Nome ?? string.Empty;
        dto.DataInizioAbbonamento = utente.DataInizioAbbonamento;

        return dto;
    }

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
        IdentityResult result = await _gestioneUtenti.UpdateAsync(utente);

        return result;
    }

    public async Task<IdentityResult> EliminaAsync(string userId)
    {
        Utente? utente = await _gestioneUtenti.FindByIdAsync(userId);

        if (utente == null)
        {
            IdentityError errore = new IdentityError();
            return IdentityResult.Failed(errore);
        }

        IdentityResult risultato = await _gestioneUtenti.DeleteAsync(utente);
        return risultato;
    }
}

```

## BigliettoService.cs V1.0

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

   public async Task<List<DtoBiglietto>> OttieniTutto()
{
    // 1️⃣ Recupera tutti i biglietti dal database
    List<Biglietto> biglietti = await _contesto.Biglietti.ToListAsync();

    // 2️⃣ Lista che conterrà i DTO finali da restituire
    List<DtoBiglietto> risultato = new List<DtoBiglietto>();

    // 3️⃣ Cicla ogni biglietto trovato nel database
    for (int i = 0; i < biglietti.Count; i++)
    {
        Biglietto bigliettoCorrente = biglietti[i];

        // 4️⃣ Recupera il genere del film (⚠️ probabilmente errato: FindAsync richiede una chiave primaria, non un oggetto)
        GenereMovie? genereMovie = await _contesto.GeneriMovies.FindAsync(bigliettoCorrente);

        // 5️⃣ Recupera la proiezione associata al biglietto
        Proiezione? proiezione = await _contesto.Proiezioni.FindAsync(bigliettoCorrente.ProiezioneId);

        // 6️⃣ Recupera il film della proiezione
        Movie? movie = await _contesto.Movies.FindAsync(proiezione.MovieId);

        // 7️⃣ Recupera la sala della proiezione
        Sala? sala = await _contesto.Sale.FindAsync(proiezione.SalaId);

        // 8️⃣ Recupera la tipologia della sala (serve per la maggiorazione)
        TipologiaSala? tipologiaSala = await _contesto.TipologieSala.FindAsync(sala.TipologiaSalaId);

        // 9️⃣ Recupera l’utente che ha acquistato il biglietto
        Utente? utente = await _contesto.Utenti.FindAsync(bigliettoCorrente.UtenteId);

        // 🔟 Crea il DTO da restituire al frontend
        DtoBiglietto dto = new DtoBiglietto();
        dto.Id = bigliettoCorrente.Id;
        dto.UtenteId = bigliettoCorrente.UtenteId;
        dto.ProiezioneId = bigliettoCorrente.ProiezioneId;
        dto.OrarioCreazione = bigliettoCorrente.OrarioCreazione;
        dto.NumeroBiglietti = bigliettoCorrente.NumeroBiglietti;

        // 1️⃣1️⃣ Calcola il prezzo finale usando la logica del tuo helper Calcoli
        dto.PrezzoFinale = Calcoli.CalcolaPrezzoFinale(
            movie.PrezzoMovie,
            tipologiaSala.MaggiorazionePrezzo,
            bigliettoCorrente.NumeroBiglietti,
            utente.Abbonamento
        );

        // 1️⃣2️⃣ Aggiunge il DTO alla lista finale
        risultato.Add(dto);
    }

    // 1️⃣3️⃣ Restituisce la lista completa dei biglietti convertiti in DTO
    return risultato;
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
        // Validazione della business logic sui limiti dei biglietti
        if (dto.NumeroBiglietti <= 0 || dto.NumeroBiglietti > 100)
            return (null, "Il numero di biglietti deve essere compreso tra 1 e 100.");

        // Verifica dell'esistenza delle entità correlate necessarie
        var utente = await _contesto.Utenti.FindAsync(utenteId);
        if (utente == null) return (null, "Utente non trovato.");

        var proiezione = await _contesto.Proiezioni.FindAsync(dto.ProiezioneId);
        if (proiezione == null) return (null, "Proiezione non trovata.");

        var movie = await _contesto.Movies.FindAsync(proiezione.MovieId);
        var sala = await _contesto.Sale.FindAsync(proiezione.SalaId);
        if (movie == null || sala == null) return (null, "Dati del film o della sala non validi.");

        var tipologiaSala = await _contesto.TipologieSala.FindAsync(sala.TipologiaSalaId);
        if (tipologiaSala == null) return (null, "Tipologia sala non trovata.");

        // Creazione del modello da salvare
        Biglietto biglietto = new Biglietto
        {
            UtenteId = utenteId,
            ProiezioneId = proiezione.Id,
            NumeroBiglietti = dto.NumeroBiglietti,
            OrarioCreazione = DateTimeOffset.UtcNow,
            MetodoPagamento = dto.MetodoPagamento,
            PrezzoFinale = Calcoli.CalcolaPrezzoFinale(movie.PrezzoMovie, tipologiaSala.MaggiorazionePrezzo, dto.NumeroBiglietti, utente, dto.MetodoPagamento)
        };

        // Salvataggio nel database
        _contesto.Biglietti.Add(biglietto);
        await _contesto.SaveChangesAsync();

        // Mappatura del risultato nel DTO
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
        var biglietto = await _contesto.Biglietti.FindAsync(id);
        if (biglietto == null) return (false, "Biglietto non trovato.");

        _contesto.Biglietti.Remove(biglietto);
        await _contesto.SaveChangesAsync();
        return (true, null);
    }
}
```
## BigliettoService.cs V1.1

Francesco lorenzi
22/05/2026
modifiche degli output di ritorno per la creazione e la modifica

```c#
using Microsoft.EntityFrameworkCore;
using NuovoCinemaParadiso.Data;
using NuovoCinemaParadiso.Dtos;
using NuovoCinemaParadiso.Models;
using NuovoCinemaParadiso.Helpers;
using NuovoCinemaParadiso.Exceptions;

namespace NuovoCinemaParadiso.Services;

public class BigliettoService
{
    private readonly ContestoDb _contesto;

    public BigliettoService(ContestoDb contesto)
    {
        _contesto = contesto;

    }

    public async Task<List<DtoBiglietto>> OttieniTutto()
    {
        List<Biglietto> biglietti = await _contesto.Biglietti.ToListAsync();
        List<DtoBiglietto> risultato = new List<DtoBiglietto>();
        for (int i = 0; i < biglietti.Count; i++)
        {
            Biglietto bigliettoCorrente = biglietti[i];

            DtoBiglietto dto = new DtoBiglietto();
            dto.Id = bigliettoCorrente.Id;
            dto.UtenteId = bigliettoCorrente.UtenteId;
            dto.ProiezioneId = bigliettoCorrente.ProiezioneId;
            dto.OrarioCreazione = bigliettoCorrente.OrarioCreazione;
            dto.NumeroBiglietti = bigliettoCorrente.NumeroBiglietti;
            dto.PrezzoFinale = bigliettoCorrente.PrezzoFinale;

            risultato.Add(dto);
        }
        return risultato;
    }

    public async Task<(DtoBiglietto? Dto, string? Errore)> OttieniTramiteIdAsync(string id)
    {
        Biglietto? biglietto = await _contesto.Biglietti.FindAsync(id);
        if (biglietto == null) return (null, "Biglietto non trovato.");

        DtoBiglietto dto = new DtoBiglietto
        {
            Id = biglietto.Id,
            UtenteId = biglietto.UtenteId,
            ProiezioneId = biglietto.ProiezioneId,
            OrarioCreazione = biglietto.OrarioCreazione,
            NumeroBiglietti = biglietto.NumeroBiglietti,
            PrezzoFinale = biglietto.PrezzoFinale
        };

        return (dto, null);
    }

    public async Task<List<DtoBiglietto>> OttieniTramiteProiezioneAsync(string proiezioneId)
    {
        List<Biglietto> biglietti = await _contesto.Biglietti.Where(b => b.ProiezioneId == proiezioneId).ToListAsync();
        List<DtoBiglietto> risultato = new List<DtoBiglietto>();

        for (int i = 0; i < biglietti.Count; i++)
        {
            Biglietto bigliettoCorrente = biglietti[i];
            DtoBiglietto dto = new DtoBiglietto
            {
                Id = bigliettoCorrente.Id,
                UtenteId = bigliettoCorrente.UtenteId,
                ProiezioneId = bigliettoCorrente.ProiezioneId,
                OrarioCreazione = bigliettoCorrente.OrarioCreazione,
                NumeroBiglietti = bigliettoCorrente.NumeroBiglietti,
                PrezzoFinale = bigliettoCorrente.PrezzoFinale
            };
            risultato.Add(dto);
        }
        return risultato;
    }

    public async Task<(string? successo, string? Errore)> CreazioneAsync(DtoCreazioneBiglietto dto, string utenteId)
    {

        /*controlla che l'utente esista*/
        var utente = await _contesto.Utenti.FindAsync(utenteId);
        if (utente == null) return (null, "Utente non trovato.");

        utente.Abbonamento = await _contesto.Abbonamenti.FindAsync(utente.AbbonamentoId);
        if (utente.Abbonamento == null) return (null, "Abbonamento non trovato.");

        var proiezione = await _contesto.Proiezioni.FindAsync(dto.ProiezioneId);
        if (proiezione == null) return (null, "Proiezione non trovata.");

        /*controlla che ci siano abbastanza posti in sala per il numero di biglietti richiesti*/
        var sala = await _contesto.Sale.FindAsync(proiezione.SalaId);
        if (sala == null) return (null, "Sala non trovata.");
        int postiOccupati = await _contesto.Biglietti.Where(b => b.ProiezioneId == dto.ProiezioneId).SumAsync(b => b.NumeroBiglietti);
        if (postiOccupati + dto.NumeroBiglietti > sala.Capienza) return (null, "Posti insufficienti per la proiezione selezionata.");

        /*controlla che il numero di biglietti sia positivo e non superiore a 100*/
        if (dto.NumeroBiglietti <= 0 || dto.NumeroBiglietti > 100) return (null, "Il numero di biglietti deve essere compreso tra 1 e 100.");

        var movie = await _contesto.Movies.FindAsync(proiezione.MovieId);
        if (movie == null) return (null, "Film non trovato.");

        var tipologiaSala = await _contesto.TipologieSala.FindAsync(sala.TipologiaSalaId);
        if (tipologiaSala == null) return (null, "Tipologia di sala non trovata.");

        if (utente.AbbonamentoId == null)
            throw new NotFoundException("Abbonamento", "Nessun abbonamento associato all'utente");
        if (utente.Abbonamento == null)
            throw new NotFoundException("Abbonamento", utente.AbbonamentoId);

        /*controlla che l'utente abbia un saldo sufficiente*/
        if (utente.Saldo < Calcoli.CalcolaPrezzoFinale(movie.PrezzoMovie, tipologiaSala.MaggiorazionePrezzo, dto.NumeroBiglietti, utente.Abbonamento, utente.DataInizioAbbonamento))
            return (null, "Saldo insufficiente per acquistare i biglietti.");

        Biglietto biglietto = new Biglietto
        {
            UtenteId = utenteId,
            ProiezioneId = proiezione.Id,
            NumeroBiglietti = dto.NumeroBiglietti,
            OrarioCreazione = DateTimeOffset.UtcNow,
            PrezzoFinale = Calcoli.CalcolaPrezzoFinale(movie.PrezzoMovie, tipologiaSala.MaggiorazionePrezzo, dto.NumeroBiglietti, utente.Abbonamento, utente.DataInizioAbbonamento)
        };


        _contesto.Biglietti.Add(biglietto);
        var contoCinema = await _contesto.ContoCinema.FirstOrDefaultAsync();
        var saldi = await Calcoli.CalcolaSaldo(biglietto.PrezzoFinale, utente, contoCinema);
        utente.Saldo = saldi[0];
        contoCinema.Conto = saldi[1];
        await _contesto.SaveChangesAsync();

        return ("Biglietto creato con successo.", null);//ora ritorna un messaggio invece di un DTO
    }

    public async Task<(string? successo, string? Errore)> ModificaAsync(string id, DtoCreazioneBiglietto dto)
    {
        var bigliettoEsistente = await _contesto.Biglietti.FindAsync(id);
        if (bigliettoEsistente == null) return (null, "Biglietto non trovato.");

        var utente = await _contesto.Utenti.FindAsync(bigliettoEsistente.UtenteId);
        if (utente == null) return (null, "Utente non trovato.");

        var proiezione = await _contesto.Proiezioni.FindAsync(dto.ProiezioneId);
        if (proiezione == null) return (null, "Proiezione non trovata.");


        var sala = await _contesto.Sale.FindAsync(proiezione.SalaId);
        if (sala == null) return (null, "Sala non trovata.");
        int postiOccupati = await _contesto.Biglietti.Where(b => b.ProiezioneId == dto.ProiezioneId).SumAsync(b => b.NumeroBiglietti);

        if (proiezione.Id != bigliettoEsistente.ProiezioneId)
        {
            if (postiOccupati + dto.NumeroBiglietti > sala.Capienza) return (null, "Posti insufficienti per la proiezione selezionata.");
        }else
        {
             postiOccupati = postiOccupati - bigliettoEsistente.NumeroBiglietti;
        }
        if (postiOccupati + dto.NumeroBiglietti > sala.Capienza) return (null, "Posti insufficienti per la proiezione selezionata.");

        if (dto.NumeroBiglietti <= 0 || dto.NumeroBiglietti > 100) return (null, "Il numero di biglietti deve essere compreso tra 1 e 100.");

        var movie = await _contesto.Movies.FindAsync(proiezione.MovieId);
        if (movie == null) return (null, "Film non trovato.");

        var tipologiaSala = await _contesto.TipologieSala.FindAsync(sala.TipologiaSalaId);
        if (tipologiaSala == null) return (null, "Tipologia di sala non trovata.");

        if (utente.AbbonamentoId == null)
            throw new NotFoundException("Abbonamento", "Nessun abbonamento associato all'utente");
        if (utente.Abbonamento == null)
            throw new NotFoundException("Abbonamento", utente.AbbonamentoId);

        if (utente.Saldo + bigliettoEsistente.PrezzoFinale < Calcoli.CalcolaPrezzoFinale(movie.PrezzoMovie, tipologiaSala.MaggiorazionePrezzo, dto.NumeroBiglietti, utente.Abbonamento, utente.DataInizioAbbonamento))
            return (null, "Saldo insufficiente per acquistare i biglietti.");

        Biglietto biglietto = new Biglietto
        {
            UtenteId = utente.Id,
            ProiezioneId = proiezione.Id,
            NumeroBiglietti = dto.NumeroBiglietti,
            OrarioCreazione = DateTimeOffset.UtcNow,
            PrezzoFinale = Calcoli.CalcolaPrezzoFinale(movie.PrezzoMovie, tipologiaSala.MaggiorazionePrezzo, dto.NumeroBiglietti, utente.Abbonamento, utente.DataInizioAbbonamento)
        };

        var differenzaPrezzo = biglietto.PrezzoFinale - bigliettoEsistente.PrezzoFinale;

        _contesto.Biglietti.Add(biglietto);
        var contoCinema = await _contesto.ContoCinema.FirstOrDefaultAsync();
        var saldi = await Calcoli.CalcolaSaldo(differenzaPrezzo, utente, contoCinema);
        utente.Saldo = saldi[0];
        contoCinema.Conto = saldi[1];
        await _contesto.SaveChangesAsync();

        return ("Biglietto modificato con successo.", null);//ora ritorna un messaggio invece di un DTO
    }

    public async Task<(bool Successo, string? Errore)> EliminazioneAsync(string id)
    {
        var biglietto = await _contesto.Biglietti.FindAsync(id);
        if (biglietto == null) return (false, "Biglietto non trovato.");

        var utente = await _contesto.Utenti.FindAsync(biglietto.UtenteId);
        var contoCinema = await _contesto.ContoCinema.FirstOrDefaultAsync();

        if (utente == null || contoCinema == null)
            return (false, "Dati correlati all'biglietto non trovati.");
        var saldi = await Calcoli.CalcolaSaldo(-biglietto.PrezzoFinale, utente, contoCinema);
        utente.Saldo = saldi[0];
        contoCinema.Conto = saldi[1];

        _contesto.Biglietti.Remove(biglietto);
        await _contesto.SaveChangesAsync();
        return (true, null);
    }
}
```
Aggiornate le chiamate a CalcolaPrezzoFinale
```c#

    /*
    dto.PrezzoFinale = Calcoli.CalcolaPrezzoFinale(
            movie.PrezzoMovie,
            tipologiaSala.MaggiorazionePrezzo,
            bigliettoCorrente.NumeroBiglietti,
            utente.Abbonamento
        );
    */
    //calcola il prezzo del biglietto
    dto.PrezzoFinale = Calcoli.CalcolaPrezzoFinale(
                movie.PrezzoMovie,
                tipologiaSala.MaggiorazionePrezzo,
                bigliettoCorrente.NumeroBiglietti,
                utente.Abbonamento,
                utente.DataInizioAbbonamento
            );

    /*
    PrezzoFinale = Calcoli.CalcolaPrezzoFinale(movie.PrezzoMovie, tipologiaSala.MaggiorazionePrezzo, biglietto.NumeroBiglietti, utente, biglietto.MetodoPagamento)
    */
    PrezzoFinale = Calcoli.CalcolaPrezzoFinale(movie.PrezzoMovie,
                tipologiaSala.MaggiorazionePrezzo,
                biglietto.NumeroBiglietti,
                utente.Abbonamento,
                utente.DataInizioAbbonamento
            )

    /*
    aggiunto controllo sul saldo del conto per la creazione biglietto dopo
    var tipologiaSala = await _contesto.TipologieSala.FindAsync(sala.TipologiaSalaId);
    */
    /*controlla che l'utente abbia un saldo sufficiente*/
    if (utente.Saldo < Calcoli.CalcolaPrezzoFinale(movie.PrezzoMovie, tipologiaSala.MaggiorazionePrezzo, dto.NumeroBiglietti, utente.Abbonamento, utente.DataInizioAbbonamento))
        return (null, "Saldo insufficiente per acquistare i biglietti.");
    
```

## BigliettoService.cs V1.2

Andrea Bruno 22-05-2026

Query per la ricerca dei campi utili per il DtoBiglietto
Aggiunta campi nel DtoBiglietto di ritorno
Eliminazione OttieniTutto()

```c#
using Microsoft.EntityFrameworkCore;
using NuovoCinemaParadiso.Data;
using NuovoCinemaParadiso.Dtos;
using NuovoCinemaParadiso.Models;
using NuovoCinemaParadiso.Helpers;
using NuovoCinemaParadiso.Exceptions;

namespace NuovoCinemaParadiso.Services;

public class BigliettoService
{
    private readonly ContestoDb _contesto;

    public BigliettoService(ContestoDb contesto)
    {
        _contesto = contesto;
    }

    public async Task<(DtoBiglietto? Dto, string? Errore)> OttieniTramiteIdAsync(string id)
    {
        // Recupera il biglietto tramite ID.
        // Se non esiste, viene lanciata un'eccezione: il flusso non prosegue.
        Biglietto? biglietto = await _contesto.Biglietti.FindAsync(id)
            ?? throw new Exception("Biglietto non trovato");

        // Recupera la proiezione collegata al biglietto.
        // Se manca, significa che il database è in stato inconsistente.
        var proiezione = await _contesto.Proiezioni.FindAsync(biglietto.ProiezioneId)
            ?? throw new Exception("Proiezione non trovata");

        // Recupera il film associato alla proiezione.
        var movie = await _contesto.Movies.FindAsync(proiezione.MovieId)
            ?? throw new Exception("Movie non trovato");

        // Recupera la sala in cui avviene la proiezione.
        var sala = await _contesto.Sale.FindAsync(proiezione.SalaId)
            ?? throw new Exception("Sala non trovata");

        // Recupera la tipologia della sala (IMAX, 3D, Standard, ecc.).
        var tipologiaSala = await _contesto.TipologieSala.FindAsync(sala.TipologiaSalaId)
            ?? throw new Exception("TipologiaSala non trovata");

        // Recupera il turno (mattina, pomeriggio, sera) della proiezione.
        var turno = await _contesto.Turni.FindAsync(proiezione.TurnoId)
            ?? throw new Exception("Turno non trovato");

        if (biglietto == null) return (null, "Biglietto non trovato.");

        DtoBiglietto dto = new DtoBiglietto
        {
            Id = biglietto.Id,
            UtenteId = biglietto.UtenteId,
            ProiezioneId = biglietto.ProiezioneId,
            OrarioCreazione = biglietto.OrarioCreazione,
            NumeroBiglietti = biglietto.NumeroBiglietti,
            PrezzoFinale = biglietto.PrezzoFinale,
            NomeSala = sala.Nome,
            TitoloMovie = movie.Titolo,
            NomeTipologiaSala = tipologiaSala.Nome,
            OraInizio = turno.OraInizio,
            DataProiezione = proiezione.DataProiezione,
        };

        return (dto, null);
    }

    public async Task<List<DtoBiglietto>> OttieniTramiteProiezioneAsync(string proiezioneId)
    {
        List<Biglietto> biglietti = await _contesto.Biglietti.Where(b => b.ProiezioneId == proiezioneId).ToListAsync();
        // Recupera il biglietto tramite ID.
        // Se non esiste, viene lanciata un'eccezione: il flusso non prosegue.
        Biglietto? biglietto = await _contesto.Biglietti.FindAsync(id)
            ?? throw new Exception("Biglietto non trovato");

        // Recupera la proiezione collegata al biglietto.
        // Se manca, significa che il database è in stato inconsistente.
        var proiezione = await _contesto.Proiezioni.FindAsync(biglietto.ProiezioneId)
            ?? throw new Exception("Proiezione non trovata");

        // Recupera il film associato alla proiezione.
        var movie = await _contesto.Movies.FindAsync(proiezione.MovieId)
            ?? throw new Exception("Movie non trovato");

        // Recupera la sala in cui avviene la proiezione.
        var sala = await _contesto.Sale.FindAsync(proiezione.SalaId)
            ?? throw new Exception("Sala non trovata");

        // Recupera la tipologia della sala (IMAX, 3D, Standard, ecc.).
        var tipologiaSala = await _contesto.TipologieSala.FindAsync(sala.TipologiaSalaId)
            ?? throw new Exception("TipologiaSala non trovata");

        // Recupera il turno (mattina, pomeriggio, sera) della proiezione.
        var turno = await _contesto.Turni.FindAsync(proiezione.TurnoId)
            ?? throw new Exception("Turno non trovato");

        List<DtoBiglietto> risultato = new List<DtoBiglietto>();

        for (int i = 0; i < biglietti.Count; i++)
        {
            Biglietto bigliettoCorrente = biglietti[i];
            DtoBiglietto dto = new DtoBiglietto
            {
                Id = bigliettoCorrente.Id,
                UtenteId = bigliettoCorrente.UtenteId,
                ProiezioneId = bigliettoCorrente.ProiezioneId,
                OrarioCreazione = bigliettoCorrente.OrarioCreazione,
                NumeroBiglietti = bigliettoCorrente.NumeroBiglietti,
                PrezzoFinale = bigliettoCorrente.PrezzoFinale,
                NomeSala = sala.Nome,
                TitoloMovie = movie.Titolo,
                NomeTipologiaSala = tipologiaSala.Nome,
                OraInizio = turno.OraInizio,
                DataProiezione = proiezione.DataProiezione,
            };
            risultato.Add(dto);
        }
        return risultato;
    }

    public async Task<(string? successo, string? Errore)> CreazioneAsync(DtoCreazioneBiglietto dto, string utenteId)
    {
        /*controlla che l'utente esista*/
        var utente = await _contesto.Utenti.FindAsync(utenteId);
        if (utente == null) return (null, "Utente non trovato.");

        utente.Abbonamento = await _contesto.Abbonamenti.FindAsync(utente.AbbonamentoId);
        if (utente.Abbonamento == null) return (null, "Abbonamento non trovato.");

        var proiezione = await _contesto.Proiezioni.FindAsync(dto.ProiezioneId);
        if (proiezione == null) return (null, "Proiezione non trovata.");

        /*controlla che ci siano abbastanza posti in sala per il numero di biglietti richiesti*/
        var sala = await _contesto.Sale.FindAsync(proiezione.SalaId);
        if (sala == null) return (null, "Sala non trovata.");
        int postiOccupati = await _contesto.Biglietti.Where(b => b.ProiezioneId == dto.ProiezioneId).SumAsync(b => b.NumeroBiglietti);
        if (postiOccupati + dto.NumeroBiglietti > sala.Capienza) return (null, "Posti insufficienti per la proiezione selezionata.");

        /*controlla che il numero di biglietti sia positivo e non superiore a 100*/
        if (dto.NumeroBiglietti <= 0 || dto.NumeroBiglietti > 100) return (null, "Il numero di biglietti deve essere compreso tra 1 e 100.");

        var movie = await _contesto.Movies.FindAsync(proiezione.MovieId);
        if (movie == null) return (null, "Film non trovato.");

        var tipologiaSala = await _contesto.TipologieSala.FindAsync(sala.TipologiaSalaId);
        if (tipologiaSala == null) return (null, "Tipologia di sala non trovata.");

        if (utente.AbbonamentoId == null)
            throw new NotFoundException("Abbonamento", "Nessun abbonamento associato all'utente");
        if (utente.Abbonamento == null)
            throw new NotFoundException("Abbonamento", utente.AbbonamentoId);

        /*controlla che l'utente abbia un saldo sufficiente*/
        if (utente.Saldo < Calcoli.CalcolaPrezzoFinale(movie.PrezzoMovie, tipologiaSala.MaggiorazionePrezzo, dto.NumeroBiglietti, utente.Abbonamento, utente.DataInizioAbbonamento))
            return (null, "Saldo insufficiente per acquistare i biglietti.");

        Biglietto biglietto = new Biglietto
        {
            UtenteId = utenteId,
            ProiezioneId = proiezione.Id,
            NumeroBiglietti = dto.NumeroBiglietti,
            OrarioCreazione = DateTimeOffset.UtcNow,
            PrezzoFinale = Calcoli.CalcolaPrezzoFinale(movie.PrezzoMovie, tipologiaSala.MaggiorazionePrezzo, dto.NumeroBiglietti, utente.Abbonamento, utente.DataInizioAbbonamento)
        };

        _contesto.Biglietti.Add(biglietto);
        var contoCinema = await _contesto.ContoCinema.FirstOrDefaultAsync();
        var saldi = await Calcoli.CalcolaSaldo(biglietto.PrezzoFinale, utente, contoCinema);
        utente.Saldo = saldi[0];
        contoCinema.Saldo = saldi[1];
        await _contesto.SaveChangesAsync();

        return ("Biglietto creato con successo.", null);
    }

    public async Task<(string? successo, string? Errore)> ModificaAsync(string id, DtoCreazioneBiglietto dto)
    {
        var bigliettoEsistente = await _contesto.Biglietti.FindAsync(id);
        if (bigliettoEsistente == null) return (null, "Biglietto non trovato.");

        var utente = await _contesto.Utenti.FindAsync(bigliettoEsistente.UtenteId);
        if (utente == null) return (null, "Utente non trovato.");

        var proiezione = await _contesto.Proiezioni.FindAsync(dto.ProiezioneId);
        if (proiezione == null) return (null, "Proiezione non trovata.");


        var sala = await _contesto.Sale.FindAsync(proiezione.SalaId);
        if (sala == null) return (null, "Sala non trovata.");
        int postiOccupati = await _contesto.Biglietti.Where(b => b.ProiezioneId == dto.ProiezioneId).SumAsync(b => b.NumeroBiglietti);

        if (proiezione.Id != bigliettoEsistente.ProiezioneId)
        {
            if (postiOccupati + dto.NumeroBiglietti > sala.Capienza) return (null, "Posti insufficienti per la proiezione selezionata.");
        }
        else
        {
            postiOccupati = postiOccupati - bigliettoEsistente.NumeroBiglietti;
        }
        if (postiOccupati + dto.NumeroBiglietti > sala.Capienza) return (null, "Posti insufficienti per la proiezione selezionata.");

        if (dto.NumeroBiglietti <= 0 || dto.NumeroBiglietti > 100) return (null, "Il numero di biglietti deve essere compreso tra 1 e 100.");

        var movie = await _contesto.Movies.FindAsync(proiezione.MovieId);
        if (movie == null) return (null, "Film non trovato.");

        var tipologiaSala = await _contesto.TipologieSala.FindAsync(sala.TipologiaSalaId);
        if (tipologiaSala == null) return (null, "Tipologia di sala non trovata.");

        if (utente.AbbonamentoId == null)
            throw new NotFoundException("Abbonamento", "Nessun abbonamento associato all'utente");
        if (utente.Abbonamento == null)
            throw new NotFoundException("Abbonamento", utente.AbbonamentoId);

        if (utente.Saldo + bigliettoEsistente.PrezzoFinale < Calcoli.CalcolaPrezzoFinale(movie.PrezzoMovie, tipologiaSala.MaggiorazionePrezzo, dto.NumeroBiglietti, utente.Abbonamento, utente.DataInizioAbbonamento))
            return (null, "Saldo insufficiente per acquistare i biglietti.");

        Biglietto biglietto = new Biglietto
        {
            UtenteId = utente.Id,
            ProiezioneId = proiezione.Id,
            NumeroBiglietti = dto.NumeroBiglietti,
            OrarioCreazione = DateTimeOffset.UtcNow,
            PrezzoFinale = Calcoli.CalcolaPrezzoFinale(movie.PrezzoMovie, tipologiaSala.MaggiorazionePrezzo, dto.NumeroBiglietti, utente.Abbonamento, utente.DataInizioAbbonamento)
        };

        var differenzaPrezzo = biglietto.PrezzoFinale - bigliettoEsistente.PrezzoFinale;

        _contesto.Biglietti.Add(biglietto);
        var contoCinema = await _contesto.ContoCinema.FirstOrDefaultAsync();
        var saldi = await Calcoli.CalcolaSaldo(differenzaPrezzo, utente, contoCinema);
        utente.Saldo = saldi[0];
        contoCinema.Saldo = saldi[1];
        await _contesto.SaveChangesAsync();

        return ("Biglietto modificato con successo.", null);
    }

    public async Task<(bool Successo, string? Errore)> EliminazioneAsync(string id)
    {
        var biglietto = await _contesto.Biglietti.FindAsync(id);
        if (biglietto == null) return (false, "Biglietto non trovato.");

        var utente = await _contesto.Utenti.FindAsync(biglietto.UtenteId);
        var contoCinema = await _contesto.ContoCinema.FirstOrDefaultAsync();

        if (utente == null || contoCinema == null)
            return (false, "Dati correlati all'biglietto non trovati.");
        var saldi = await Calcoli.CalcolaSaldo(-biglietto.PrezzoFinale, utente, contoCinema);
        utente.Saldo = saldi[0];
        contoCinema.Saldo = saldi[1];

        _contesto.Biglietti.Remove(biglietto);
        await _contesto.SaveChangesAsync();
        return (true, null);
    }
}
```
## VERSIONE 1.1.1

Utente: Fabio Tammaro
Data: 22/05/2026
Descrizione : Questa parte è stata rimossa in concomitanza con l'aggiunta del metodo di lettura nel conto nel dominio del gestore.    
## ContoCinemaService.cs

```c#
using Microsoft.EntityFrameworkCore;
using NuovoCinemaParadiso.Dtos;
using NuovoCinemaParadiso.Data;
using NuovoCinemaParadiso.Models;
using NuovoCinemaParadiso.Exceptions;

namespace NuovoCinemaParadiso.Services;

/// <summary>
/// Service che gestisce le operazioni relative al conto del cinema.
/// Permette lettura, creazione, modifica ed eliminazione del conto.
/// </summary>
public class ContoCinemaService
{
    private readonly ContestoDb _contesto;

    public ContoCinemaService(ContestoDb contesto)
    {
        _contesto = contesto;
    }

    /// <summary>
    /// Restituisce i dati del conto del cinema.
    /// </summary>
    /// <returns>
    /// DTO del conto cinema oppure null se non esiste alcun conto configurato.
    /// </returns>
    public async Task<DtoContoCinema> OttieniDatiContoAsync()
    {
        ContoCinema contoCinema = await _contesto.ContoCinema.FirstOrDefaultAsync();

        if (contoCinema == null)
        {
            return null;
        }

        return new DtoContoCinema
        {
            Id = contoCinema.Id,
            Iban = contoCinema.Iban,
            TitolareConto = contoCinema.TitolareConto,
            Conto = contoCinema.Conto
        };
    }

    /// <summary>
    /// Crea il conto del cinema se non esiste già.
    /// </summary>
    /// <returns>
    /// DTO del conto creato oppure null se il conto esiste già.
    /// </returns>
    public async Task<DtoContoCinema?> CreazioneAsync(DtoCreazioneContoCinema dto)
    {
        ContoCinema esistente = await _contesto.ContoCinema.FirstOrDefaultAsync();

        if (esistente != null)
        {
            return null;
        }

        ContoCinema contoCinema = new ContoCinema
        {
            Iban = dto.Iban,
            TitolareConto = dto.TitolareConto,
            Conto = dto.Conto
        };

        _contesto.ContoCinema.Add(contoCinema);
        await _contesto.SaveChangesAsync();

        return new DtoContoCinema
        {
            Id = contoCinema.Id,
            Iban = contoCinema.Iban,
            TitolareConto = contoCinema.TitolareConto,
            Conto = contoCinema.Conto
        };
    }

    /// <summary>
    /// Modifica i dati del conto del cinema esistente.
    /// </summary>
    /// <param name="id">ID del conto da modificare.</param>
    /// <param name="dto">Nuovi dati del conto.</param>
    /// <returns>DTO aggiornato del conto.</returns>
    /// <exception cref="ItemNotFoundException">Sollevata se il conto non esiste.</exception>
    public async Task<DtoContoCinema?> ModificaAsync(string id, DtoCreazioneContoCinema dto)
    {
        ContoCinema? contoCinema = await _contesto.ContoCinema.FindAsync(id);

        if (contoCinema == null)
        {
            throw new ItemNotFoundException("Dati conto");
        }

        contoCinema.Iban = dto.Iban;
        contoCinema.TitolareConto = dto.TitolareConto;

        await _contesto.SaveChangesAsync();

        return new DtoContoCinema
        {
            Id = contoCinema.Id,
            Iban = contoCinema.Iban,
            TitolareConto = contoCinema.TitolareConto,
            Conto = contoCinema.Conto
        };
    }

    /// <summary>
    /// Elimina il conto del cinema dal database.
    /// </summary>
    /// <param name="id">ID del conto da eliminare.</param>
    /// <returns>
    /// true se l'eliminazione è avvenuta con successo, false se il conto non esiste.
    /// </returns>
    public async Task<bool> EliminaAsync(string id)
    {
        ContoCinema? contoCinema = await _contesto.ContoCinema.FindAsync(id);

        if (contoCinema == null)
        {
            return false;
        }

        _contesto.ContoCinema.Remove(contoCinema);
        await _contesto.SaveChangesAsync();

        return true;
    }
}
```

## ContoCinemaController.cs V 1.1

Utente: Andrea Paris
Data: 21/05/2026
Descrizione: Ho "Eliminato" i metodi di Creazione, Modifica e eliminazione e 
li ho commentati qui per una possibile implementazione futura.

```C#
using Microsoft.EntityFrameworkCore;
using NuovoCinemaParadiso.Dtos;
using NuovoCinemaParadiso.Data;
using NuovoCinemaParadiso.Models;
using NuovoCinemaParadiso.Exceptions;
using NuovoCinemaParadiso.Controllers;

namespace NuovoCinemaParadiso.Services;

public class ContoCinemaService
{
    private readonly ContestoDb _contesto;
    public ContoCinemaService(ContestoDb contesto)
    {
        _contesto = contesto;
    }
    public async Task<DtoContoCinema> OttieniDatiContoAsync()
    {

        ContoCinema contoCinema = await _contesto.ContoCinema.FirstOrDefaultAsync()
            ?? throw new NotFoundException("Conto Cinema", "");

        DtoContoCinema dto = new DtoContoCinema();

        dto.Id = contoCinema.Id;
        dto.Iban = contoCinema.Iban;
        dto.TitolareConto = contoCinema.TitolareConto;
        dto.Conto = contoCinema.Conto;

        return dto;
    }
}

//POSSIBILE IMPLEMENTAZIONE FUTURA

/*public async Task<bool> CreazioneAsync(DtoCreazioneContoCinema dto)
    {
        ContoCinema risutato = await _contesto.ContoCinema.FirstOrDefaultAsync()
            ?? throw new NotFoundException("Conto Cinema", "");


        if(risutato != null)
        {
            return false;
        }

        ContoCinema contoCinema = new ContoCinema();
        contoCinema.Iban = dto.Iban;
        contoCinema.TitolareConto = dto.TitolareConto;
        contoCinema.Conto = dto.Conto;

        _contesto.ContoCinema.Add(contoCinema);
        await _contesto.SaveChangesAsync();

        return true;
    } 

    public async Task<bool> ModificaAsync(string id, DtoCreazioneContoCinema dto)
    {
        ContoCinema? contoCinema = await _contesto.ContoCinema.FindAsync(id);

        if (contoCinema == null)
        {
            return false;
        }

        contoCinema.Iban = dto.Iban;
        contoCinema.TitolareConto = dto.TitolareConto;

        await _contesto.SaveChangesAsync();

        return true;
    }

    public async Task<bool> EliminaAsync(string id)
    {
        ContoCinema? contoCinema = await _contesto.ContoCinema.FindAsync(id);

        if (contoCinema == null)
        {
            return false;
        }

        _contesto.ContoCinema.Remove(contoCinema);
        await _contesto.SaveChangesAsync();

        return true;
    }*/
```

## GenereMovieService.cs

```c#
using Microsoft.EntityFrameworkCore;
using NuovoCinemaParadiso.Dtos;
using NuovoCinemaParadiso.Data;
using NuovoCinemaParadiso.Models;
using NuovoCinemaParadiso.Exceptions;

namespace NuovoCinemaParadiso.Services;

/// <summary>
/// Service che gestisce la logica di business relativa ai generi dei film.
/// Permette di ottenere la lista dei generi e di recuperare un genere tramite ID.
/// </summary>
public class GenereMovieService
{
    private readonly ContestoDb _contesto;

    public GenereMovieService(ContestoDb contesto)
    {
        _contesto = contesto;
    }

    /// <summary>
    /// Restituisce tutti i generi di film presenti nel sistema.
    /// </summary>
    /// <returns>Lista di DTO dei generi film.</returns>
    public async Task<List<DtoGenereMovie>> OttieniTuttoAsync()
    {
        List<GenereMovie> generiMovies = await _contesto.GeneriMovies.ToListAsync();

        List<DtoGenereMovie> risultato = new List<DtoGenereMovie>();

        for (int i = 0; i < generiMovies.Count; i++)
        {
            GenereMovie genereCorrente = generiMovies[i];

            risultato.Add(new DtoGenereMovie
            {
                Id = genereCorrente.Id,
                Genere = genereCorrente.Genere
            });
        }

        return risultato;
    }

    /// <summary>
    /// Restituisce un genere film tramite il suo ID.
    /// </summary>
    /// <param name="id">ID del genere da cercare.</param>
    /// <returns>
    /// DTO del genere se trovato, altrimenti null.
    /// </returns>
    public async Task<DtoGenereMovie?> OttieniTramiteIdAsync(string id)
    {
        GenereMovie? genereMovie = await _contesto.GeneriMovies.FindAsync(id);

        if (genereMovie == null)
        {
            return null;
        }

        return new DtoGenereMovie
        {
            Id = genereMovie.Id,
            Genere = genereMovie.Genere
        };
    }
}
```

## GestoreService.cs V1.0

```c#
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NuovoCinemaParadiso.Data;
using NuovoCinemaParadiso.Dtos;
using NuovoCinemaParadiso.Models;
using NuovoCinemaParadiso.Helpers;
using NuovoCinemaParadiso.Exceptions;

namespace NuovoCinemaParadiso.Services;

/// <summary>
/// Service dedicato alle funzionalità del ruolo Gestore.
/// Gestisce operazioni amministrative e accesso ai dati globali del sistema.
/// </summary>
public class GestoreService
{
    private readonly ContestoDb _contesto;
    private readonly UserManager<Utente> _gestioneUtenti;

    /// <summary>
    /// Inizializza una nuova istanza del servizio GestoreService.
    /// </summary>
    /// <param name="contestoDb">Contesto del database applicativo.</param>
    /// <param name="gestioneUtenti">Gestore Identity degli utenti.</param>
    public GestoreService(ContestoDb contestoDb, UserManager<Utente> gestioneUtenti)
    {
        _contesto = contestoDb;
        _gestioneUtenti = gestioneUtenti;
    }
}
```

### GestoreService V1.1.1

Utente: Fabio Tammaro(github: FabTam)
Data: 21/05/2026
Descrizione: Eliminata la lettura dei log poichè di competenza del gestore.

```c#
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NuovoCinemaParadiso.Data;
using NuovoCinemaParadiso.Dtos;
using NuovoCinemaParadiso.Models;

namespace NuovoCinemaParadiso.Services;

// Service dedicato alle operazioni riservate al gestore
public class GestoreService
{
    private readonly ContestoDb _contesto;
    private readonly UserManager<Utente> _gestioneUtenti;

    // Dependency injection del contesto database e della gestione utenti
    public GestoreService(ContestoDb contestoDb, UserManager<Utente> gestioneUtenti)
    {
        _contesto = contestoDb;
        _gestioneUtenti = gestioneUtenti;
    }

    // Lettura completa dei log delle azioni effettuate nel sistema
    public async Task<List<DtoLogAzioni>> LetturaLogAzioneAsync()
    {
        // Recupero dei log dal database
        List<LogAzioni> logs = await _contesto.LogAzioni.ToListAsync();

        // Lista dei DTO da restituire
        List<DtoLogAzioni> risultati = new List<DtoLogAzioni>();

        // Conversione entità -> DTO
        foreach (LogAzioni log in logs)
        {
            DtoLogAzioni risultato = new DtoLogAzioni
            {
                Id = log.Id,
                IdUtente = log.IdUtente,
                NomeAzione = log.NomeAzione,
                Effettuato = log.Effettuato,
                Messaggio = log.Messaggio,
                TimeStamp = log.TimeStamp
            };

            risultati.Add(risultato);
        }

        return risultati;
    }
}
```
### GestoreService.cs V1.1.2
Utente: Fabio Tammaro(github: FabTam)
Data: 21/05/2026
Descrizione: Inserita la lettura dei dati del conto.
```C#
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NuovoCinemaParadiso.Data;
using NuovoCinemaParadiso.Dtos;
using NuovoCinemaParadiso.Models;
using NuovoCinemaParadiso.Exceptions;


namespace NuovoCinemaParadiso.Services;

public class GestoreService
{
    private readonly ContestoDb _contesto;
    private readonly UserManager<Utente> _gestioneUtenti;
    public GestoreService(ContestoDb contestoDb, UserManager<Utente> gestioneUtenti)
    {
        _contesto = contestoDb;
        _gestioneUtenti = gestioneUtenti;

    }

    public async Task<List<DtoLogAzioni>> LetturaLogAzioneAsync()
    {
        List<LogAzioni> logs= await _contesto.LogAzioni.ToListAsync();
        List<DtoLogAzioni> risultati = new List<DtoLogAzioni>();
        foreach (LogAzioni log in logs)
        {
            DtoLogAzioni risultato = new DtoLogAzioni
            {
                Id = log.Id,
                IdUtente = log.IdUtente,
                NomeAzione = log.NomeAzione,
                Effettuato = log.Effettuato,
                Messaggio = log.Messaggio,
                TimeStamp = log.TimeStamp
            };
            risultati.Add(risultato);
        }

        return risultati;
    }

     public async Task<DtoContoCinema> OttieniDatiContoAsync()
    {

        ContoCinema contoCinema = await _contesto.ContoCinema.FirstOrDefaultAsync()
            ?? throw new NotFoundException("Conto Cinema", "");

        DtoContoCinema dto = new DtoContoCinema();

        dto.Id = contoCinema.Id;
        dto.Iban = contoCinema.Iban;
        dto.TitolareConto = contoCinema.TitolareConto;
        dto.Saldo = contoCinema.Saldo;

        return dto;
    }
}
```

## GestoreService.cs V1.1

Andrea Bruno 22-05-2026
Implementazione OttieniTuttiBigliettiAsync() 
Aggiunta campi e query nel DtoBiglietto di ritorno

```c#
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NuovoCinemaParadiso.Data;
using NuovoCinemaParadiso.Dtos;
using NuovoCinemaParadiso.Models;
using NuovoCinemaParadiso.Exceptions;

namespace NuovoCinemaParadiso.Services;

public class GestoreService
{
    private readonly ContestoDb _contesto;
    private readonly UserManager<Utente> _gestioneUtenti;
    public GestoreService(ContestoDb contestoDb, UserManager<Utente> gestioneUtenti)
    {
        _contesto = contestoDb;
        _gestioneUtenti = gestioneUtenti;
    }

    public async Task<List<DtoLogAzioni>> LetturaLogAzioneAsync()
    {
        List<LogAzioni> logs = await _contesto.LogAzioni.ToListAsync();
        List<DtoLogAzioni> risultati = new List<DtoLogAzioni>();
        foreach (LogAzioni log in logs)
        {
            DtoLogAzioni risultato = new DtoLogAzioni
            {
                Id = log.Id,
                IdUtente = log.IdUtente,
                NomeAzione = log.NomeAzione,
                Effettuato = log.Effettuato,
                Messaggio = log.Messaggio,
                TimeStamp = log.TimeStamp
            };
            risultati.Add(risultato);
        }

        return risultati;
    }

    public async Task<DtoContoCinema> OttieniDatiContoAsync()
    {

        ContoCinema contoCinema = await _contesto.ContoCinema.FirstOrDefaultAsync()
            ?? throw new NotFoundException("Conto Cinema", "");

        DtoContoCinema dto = new DtoContoCinema();

        dto.Id = contoCinema.Id;
        dto.Iban = contoCinema.Iban;
        dto.TitolareConto = contoCinema.TitolareConto;
        dto.Saldo = contoCinema.Saldo;

        return dto;
    }

    public async Task<List<DtoBiglietto>> OttieniTuttiBigliettiAsync()
    {
        // Recupera tutti i Biglietti dal database in modo asincrono.
        // ToListAsync() esegue la query e materializza i risultati in memoria.
        List<Biglietto> biglietti = await _contesto.Biglietti.ToListAsync();
        // Recupera il biglietto tramite ID.
        // Se non esiste, viene lanciata un'eccezione: il flusso non prosegue.
        Biglietto? biglietto = await _contesto.Biglietti.FindAsync(id)
            ?? throw new Exception("Biglietto non trovato");

        // Recupera la proiezione collegata al biglietto.
        // Se manca, significa che il database è in stato inconsistente.
        var proiezione = await _contesto.Proiezioni.FindAsync(biglietto.ProiezioneId)
            ?? throw new Exception("Proiezione non trovata");

        // Recupera il film associato alla proiezione.
        var movie = await _contesto.Movies.FindAsync(proiezione.MovieId)
            ?? throw new Exception("Movie non trovato");

        // Recupera la sala in cui avviene la proiezione.
        var sala = await _contesto.Sale.FindAsync(proiezione.SalaId)
            ?? throw new Exception("Sala non trovata");

        // Recupera la tipologia della sala (IMAX, 3D, Standard, ecc.).
        var tipologiaSala = await _contesto.TipologieSala.FindAsync(sala.TipologiaSalaId)
            ?? throw new Exception("TipologiaSala non trovata");

        // Recupera il turno (mattina, pomeriggio, sera) della proiezione.
        var turno = await _contesto.Turni.FindAsync(proiezione.TurnoId)
            ?? throw new Exception("Turno non trovato");

        // Lista che conterrà i DTO da restituire al controller.
        List<DtoBiglietto> risultato = new List<DtoBiglietto>();

        // Ciclo su ogni Biglietto per convertirlo nel corrispondente DTO.
        for (int i = 0; i < biglietti.Count; i++)
        {
            Biglietto bigliettoCorrente = biglietti[i];

            // Creazione del DTO e mappatura dei campi.
            DtoBiglietto dto = new DtoBiglietto();
            dto.Id = bigliettoCorrente.Id;
            dto.ProiezioneId = bigliettoCorrente.ProiezioneId;
            dto.UtenteId = bigliettoCorrente.UtenteId;
            dto.PrezzoFinale = bigliettoCorrente.PrezzoFinale;
            dto.OrarioCreazione = bigliettoCorrente.OrarioCreazione;
            dto.NumeroBiglietti = bigliettoCorrente.NumeroBiglietti;
            dto.NomeSala = sala.Nome;
            dto.TitoloMovie = movie.Titolo;
            dto.NomeTipologiaSala = tipologiaSala.Nome;
            dto.OraInizio = turno.OraInizio;
            dto.DataProiezione = proiezione.DataProiezione;

            // Aggiunge il DTO alla lista finale.
            risultato.Add(dto);
        }

        // Restituisce la lista completa dei DTO.
        return risultato;
    }
}
```

## GiftCardService.cs

```c#
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using NuovoCinemaParadiso.Data;
using NuovoCinemaParadiso.Dtos;
using NuovoCinemaParadiso.Models;

namespace NuovoCinemaParadiso.Services;

/// <summary>
/// Service responsabile della gestione delle GiftCard.
/// Permette operazioni di lettura, modifica ed eliminazione.
/// </summary>
public class GiftCardService
{
    private readonly ContestoDb _contesto;

    /// <summary>
    /// Inizializza una nuova istanza del servizio GiftCard.
    /// </summary>
    /// <param name="contesto">Contesto del database Entity Framework.</param>
    public GiftCardService(ContestoDb contesto)
    {
        _contesto = contesto;
    }

    /// <summary>
    /// Recupera tutte le GiftCard presenti nel sistema.
    /// </summary>
    /// <returns>Lista di DTO delle GiftCard.</returns>
    public async Task<List<DtoGiftCard>> OttieniTutto()
    {
        List<GiftCard> giftCards = await _contesto.GiftCards.ToListAsync();

        List<DtoGiftCard> risultato = new List<DtoGiftCard>();

        for (int i = 0; i < giftCards.Count; i++)
        {
            GiftCard giftCardCorrente = giftCards[i];

            DtoGiftCard dto = new DtoGiftCard
            {
                Id = giftCardCorrente.Id,
                Nome = giftCardCorrente.Nome,
                Valore = giftCardCorrente.Valore,
                CodiceRiscatto = giftCardCorrente.CodiceRiscatto
            };

            risultato.Add(dto);
        }

        return risultato;
    }

    /// <summary>
    /// Recupera una GiftCard tramite ID.
    /// </summary>
    /// <param name="id">ID della GiftCard.</param>
    /// <param name="utenteId">ID utente (attualmente non utilizzato ma previsto per controlli futuri).</param>
    /// <returns>DTO della GiftCard se trovata, altrimenti null.</returns>
    public async Task<DtoGiftCard?> OttieniTramiteIdAsync(string id, string utenteId)
    {
        GiftCard? giftCard = await _contesto.GiftCards.FindAsync(id);

        if (giftCard == null)
        {
            return null;
        }

        return new DtoGiftCard
        {
            Id = giftCard.Id,
            Nome = giftCard.Nome,
            Valore = giftCard.Valore,
            CodiceRiscatto = giftCard.CodiceRiscatto
        };
    }

    /// <summary>
    /// Modifica una GiftCard esistente.
    /// </summary>
    /// <param name="id">ID della GiftCard da modificare.</param>
    /// <param name="dto">Dati aggiornati della GiftCard.</param>
    /// <returns>DTO aggiornato oppure null se non trovata.</returns>
    public async Task<DtoGiftCard?> ModificaAsync(string id, DtoCreazioneGiftCard dto)
    {
        GiftCard? giftCard = await _contesto.GiftCards.FindAsync(id);

        if (giftCard == null)
            return null;

        giftCard.Nome = dto.Nome;
        giftCard.Valore = dto.Valore;
        giftCard.CodiceRiscatto = dto.CodiceRiscatto;

        await _contesto.SaveChangesAsync();

        return new DtoGiftCard
        {
            Id = giftCard.Id,
            Nome = giftCard.Nome,
            Valore = giftCard.Valore,
            CodiceRiscatto = giftCard.CodiceRiscatto
        };
    }

    /// <summary>
    /// Elimina una GiftCard dal sistema.
    /// </summary>
    /// <param name="id">ID della GiftCard da eliminare.</param>
    /// <returns>True se eliminata, false se non trovata.</returns>
    public async Task<bool> EliminazioneAsync(string id)
    {
        GiftCard? giftCard = await _contesto.GiftCards.FindAsync(id);

        if (giftCard == null)
        {
            return false;
        }

        _contesto.GiftCards.Remove(giftCard);
        await _contesto.SaveChangesAsync();

        return true;
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

// Servizio applicativo per la registrazione e lettura dei log delle azioni degli utenti
public class LogAzioniService
{
    // Riferimento al DbContext per operazioni sul database
    private readonly ContestoDb _contesto;

    // Iniezione delle dipendenze tramite costruttore
    public LogAzioniService(ContestoDb contesto) 
    {
        _contesto = contesto; 
    }

    // Crea e salva nel database un nuovo record di log per un'azione specifica
    public async Task SalvataggioLogAzioneAsync(string? idUtente, string azione, bool effettuato)
    {
        // Determinazione del messaggio testuale in base all'esito dell'azione
        string messaggio = "operazione fallita";
        if(effettuato) 
            messaggio = "operazione eseguita";

        // Costruzione dell'entità LogAzioni da salvare
        LogAzioni log = new LogAzioni();

        log.IdUtente = idUtente;
        log.NomeAzione = azione;
        log.Effettuato = effettuato;
        log.Messaggio = messaggio;
        log.TimeStamp = DateTimeOffset.UtcNow; // Impostazione data e ora correnti in UTC

        // Salvataggio nel database
        _contesto.LogAzioni.Add(log);
        await _contesto.SaveChangesAsync();
    }

    // Restituisce la lista completa di tutti i log registrati nel sistema
    public async Task<List<DtoLogAzioni>> LetturaLogAzioneAsync()
    {
        // Lettura completa della tabella LogAzioni
        List<LogAzioni> logs = await _contesto.LogAzioni.ToListAsync();
        List<DtoLogAzioni> risultati = new List<DtoLogAzioni>();
        
        // Mappatura manuale Entità → DTO per ogni record trovato
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

        // Restituzione della lista elaborata
        return risultati;
    }
}
```

### LogAzioniService.cs Versione 1.1

Utente: Fabio Tammaro(github: FabTam)
Data: 21/05/2026
Descrizione: Eliminata la lettura dei log poichè di competenza del gestore.

```c#
using NuovoCinemaParadiso.Data;
using NuovoCinemaParadiso.Models;

namespace NuovoCinemaParadiso.Services;

public class LogAzioniService
{
  private readonly ContestoDb _contesto;
  public LogAzioniService(ContestoDb contesto) 
  {
    _contesto = contesto; 
  }

// qui è stato eliminato il metodo che leggeva tutti i log.

    public async Task SalvataggioLogAzioneAsync(string? idUtente, string azione, bool effettuato)
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

    
}

```

## MovieService.cs

```c#
using Microsoft.EntityFrameworkCore;
using NuovoCinemaParadiso.Data;
using NuovoCinemaParadiso.Dtos;
using NuovoCinemaParadiso.Models;

namespace NuovoCinemaParadiso.Services;


public class MovieService
{
    private readonly ContestoDb _contesto;
    private readonly GenereMovieService _genereMovieService;

    public MovieService(ContestoDb contesto, GenereMovieService genereMovieService)
    {
        _contesto = contesto;
        _genereMovieService = genereMovieService;
    }

    public async Task<List<DtoMovie>> OttieniTutto()
    {
        List<Movie> movies = await _contesto.Movies.ToListAsync();

        List<DtoMovie> risultato = new List<DtoMovie>();

        foreach (var movieCorrente in movies)
        {
            GenereMovie? genereMovie = await _contesto.GeneriMovies.FindAsync(movieCorrente.GenereId);

            DtoMovie dto = new DtoMovie
            {
                Id = movieCorrente.Id,
                Titolo = movieCorrente.Titolo,
                Descrizione = movieCorrente.Descrizione,
                DurataMinuti = movieCorrente.DurataMinuti,
                PrezzoMovie = movieCorrente.PrezzoMovie,
                GenereId = movieCorrente.GenereId,
                Genere = genereMovie?.Genere ?? ""
            };

            risultato.Add(dto);
        }

        return risultato;
    }

    /// <summary>
    /// Recupera un film tramite ID.
    /// </summary>
    /// <param name="id">ID del film.</param>
    /// <returns>DTO del film se trovato, altrimenti null.</returns>
    public async Task<DtoMovie?> OttieniTramiteIdAsync(string id)
    {
        Movie? movie = await _contesto.Movies.FindAsync(id);

        if (movie == null)
            return null;

        GenereMovie? genereMovie = await _contesto.GeneriMovies.FindAsync(movie.GenereId);

        return new DtoMovie
        {
            Id = movie.Id,
            Titolo = movie.Titolo,
            Descrizione = movie.Descrizione,
            DurataMinuti = movie.DurataMinuti,
            PrezzoMovie = movie.PrezzoMovie,
            GenereId = movie.GenereId,
            Genere = genereMovie?.Genere ?? ""
        };
    }

    /// <summary>
    /// Recupera tutti i film appartenenti a un genere specifico.
    /// </summary>
    /// <param name="genereId">ID del genere.</param>
    /// <returns>Lista di film filtrati per genere.</returns>
    public async Task<List<DtoMovie>> OttieniTramiteGenere(string genereId)
    {
        List<DtoMovie> movies = await OttieniTutto();

        return movies
            .Where(m => m.GenereId.Trim() == genereId)
            .ToList();
    }

    /// <summary>
    /// Crea un nuovo film.
    /// </summary>
    /// <param name="dto">Dati del film da creare.</param>
    /// <returns>DTO del film creato oppure null se genere non valido.</returns>
    public async Task<DtoMovie?> CreazioneAsync(DtoCreazioneMovie dto)
    {
        if (await _genereMovieService.OttieniTramiteIdAsync(dto.GenereId) == null)
            return null;

        Movie movie = new Movie
        {
            Titolo = dto.Titolo,
            Descrizione = dto.Descrizione,
            DurataMinuti = dto.DurataMinuti,
            PrezzoMovie = dto.PrezzoMovie,
            GenereId = dto.GenereId
        };

        _contesto.Movies.Add(movie);
        await _contesto.SaveChangesAsync();

        GenereMovie? genereMovie = await _contesto.GeneriMovies.FindAsync(movie.GenereId);

        return new DtoMovie
        {
            Id = movie.Id,
            Titolo = movie.Titolo,
            Descrizione = movie.Descrizione,
            DurataMinuti = movie.DurataMinuti,
            PrezzoMovie = movie.PrezzoMovie,
            GenereId = movie.GenereId,
            Genere = genereMovie?.Genere ?? ""
        };
    }

    /// <summary>
    /// Modifica un film esistente.
    /// </summary>
    /// <param name="id">ID del film da modificare.</param>
    /// <param name="dto">Nuovi dati del film.</param>
    /// <returns>DTO aggiornato oppure null se non valido.</returns>
    public async Task<DtoMovie?> ModificaAsync(string id, DtoCreazioneMovie dto)
    {
        Movie? movieEsistente = await _contesto.Movies.FindAsync(id);

        if (movieEsistente == null ||
            await _genereMovieService.OttieniTramiteIdAsync(dto.GenereId) == null)
            return null;

        movieEsistente.Titolo = dto.Titolo;
        movieEsistente.Descrizione = dto.Descrizione;
        movieEsistente.DurataMinuti = dto.DurataMinuti;
        movieEsistente.PrezzoMovie = dto.PrezzoMovie;
        movieEsistente.GenereId = dto.GenereId;

        await _contesto.SaveChangesAsync();

        GenereMovie? genereMovie = await _contesto.GeneriMovies.FindAsync(movieEsistente.GenereId);

        return new DtoMovie
        {
            Id = movieEsistente.Id,
            Titolo = movieEsistente.Titolo,
            Descrizione = movieEsistente.Descrizione,
            DurataMinuti = movieEsistente.DurataMinuti,
            PrezzoMovie = movieEsistente.PrezzoMovie,
            GenereId = movieEsistente.GenereId,
            Genere = genereMovie?.Genere ?? ""
        };
    }

    /// <summary>
    /// Elimina un film dal sistema.
    /// </summary>
    /// <param name="id">ID del film da eliminare.</param>
    /// <returns>True se eliminato, false se non trovato.</returns>
    public async Task<bool> EliminaAsync(string id)
    {
        Movie? movie = await _contesto.Movies.FindAsync(id);

        if (movie == null)
            return false;

        _contesto.Movies.Remove(movie);
        await _contesto.SaveChangesAsync();

        return true;
    }
}
```

## MovieService.cs (Versione 1.1)

Utente: Greg
Data: 21/05/2026
Descrizione: Modificare i valori di ritorno per non esporre dati superflui

```c#
using Microsoft.EntityFrameworkCore;
using NuovoCinemaParadiso.Data;
using NuovoCinemaParadiso.Dtos;
using NuovoCinemaParadiso.Models;

namespace NuovoCinemaParadiso.Services;

/// <summary>
/// Service responsabile della gestione dei film.
/// Gestisce operazioni CRUD e associazione con i generi.
/// </summary>
public class MovieService
{
    private readonly ContestoDb _contesto;
    private readonly GenereMovieService _genereMovieService;

    /// <summary>
    /// Inizializza una nuova istanza del servizio Movie.
    /// </summary>
    /// <param name="contesto">Contesto database EF Core.</param>
    /// <param name="genereMovieService">Servizio per la gestione dei generi film.</param>
    public MovieService(ContestoDb contesto, GenereMovieService genereMovieService)
    {
        _contesto = contesto;
        _genereMovieService = genereMovieService;
    }

    /// <summary>
    /// Recupera tutti i film presenti nel sistema con informazioni sul genere.
    /// </summary>
    /// <returns>Lista di DTO dei film.</returns>
    public async Task<List<DtoMovie>> OttieniTutto()
    {
        List<Movie> movies = await _contesto.Movies.ToListAsync();

        List<DtoMovie> risultato = new List<DtoMovie>();

        for (int i = 0; i < movies.Count; i++)
        {
            Movie movieCorrente = movies[i];
            GenereMovie? genereMovie = await _contesto.GeneriMovies.FindAsync(movieCorrente.GenereId);

            DtoMovie dto = new DtoMovie();
            dto.Id = movieCorrente.Id;
            dto.Titolo = movieCorrente.Titolo;
            dto.Descrizione = movieCorrente.Descrizione;
            dto.DurataMinuti = movieCorrente.DurataMinuti;
            dto.PrezzoMovie = movieCorrente.PrezzoMovie;
            dto.GenereId = movieCorrente.GenereId;
            dto.Genere = genereMovie?.Genere ?? "";

            risultato.Add(dto);
        }
        return risultato;
    }

    /// <summary>
    /// Recupera un film tramite ID.
    /// </summary>
    /// <param name="id">ID del film.</param>
    /// <returns>DTO del film se trovato, altrimenti null.</returns>
    public async Task<DtoMovie?> OttieniTramiteIdAsync(string id)
    {
        
        Movie? movie = await _contesto.Movies.FindAsync(id);

        if (movie == null)
        {
            return null;
        }
        GenereMovie? genereMovie = await _contesto.GeneriMovies.FindAsync(movie.GenereId);

        DtoMovie risultato = new DtoMovie
        {
            Id = movie.Id,
            Titolo = movie.Titolo,
            Descrizione = movie.Descrizione,
            DurataMinuti = movie.DurataMinuti,
            PrezzoMovie = movie.PrezzoMovie,
            GenereId = movie.GenereId,
            Genere = genereMovie?.Genere ?? ""
        };
        return risultato;
    }

    /// <summary>
    /// Recupera tutti i film appartenenti a un genere specifico.
    /// </summary>
    /// <param name="genereId">ID del genere.</param>
    /// <returns>Lista di film filtrati per genere.</returns>
    public async Task<List<DtoMovie>> OttieniTramiteGenere(string genereId)
    {
        List<DtoMovie> risultato = new List<DtoMovie>();

        List<DtoMovie> movies = await OttieniTutto();

        foreach (var movie in movies)
        {
            if (movie.GenereId.Trim() == genereId)
            {
                risultato.Add(movie);
            }
        }
        return risultato;
    }
    
    /// <summary>
    /// Crea un nuovo film.
    /// </summary>
    /// <param name="dto">Dati del film da creare.</param>
    /// <returns>booleano che da conferma tramite true o false in caso il genere non sia valido</returns>
    public async Task<bool> CreazioneAsync(DtoCreazioneMovie dto)
    {

        if (await _genereMovieService.OttieniTramiteIdAsync(dto.GenereId) == null)
        {
            return false;
        }
        
        Movie movie = new Movie();

        movie.Titolo = dto.Titolo;
        movie.Descrizione = dto.Descrizione;
        movie.DurataMinuti = dto.DurataMinuti;
        movie.PrezzoMovie = dto.PrezzoMovie;
        movie.GenereId = dto.GenereId;

        _contesto.Movies.Add(movie);
        await _contesto.SaveChangesAsync();

        return true;
    }

    /// <summary>
    /// Modifica un film esistente.
    /// </summary>
    /// <param name="id">ID del film da modificare.</param>
    /// <param name="dto">Nuovi dati del film.</param>
    /// <returns>booleano che da conferma tramite true o false in caso il genere non sia valido</returns>
    public async Task<bool> ModificaAsync(string id, DtoCreazioneMovie dto)
    {
        Movie? movieEsistente = await _contesto.Movies.FindAsync(id);

        if (await _genereMovieService.OttieniTramiteIdAsync(dto.GenereId) == null || movieEsistente == null)
        {
            return false;
        }

        movieEsistente.Titolo = dto.Titolo;
        movieEsistente.Descrizione = dto.Descrizione;
        movieEsistente.DurataMinuti = dto.DurataMinuti;
        movieEsistente.PrezzoMovie = dto.PrezzoMovie;
        movieEsistente.GenereId = dto.GenereId;

        await _contesto.SaveChangesAsync();
        
        return true;
    }

    /// <summary>
    /// Elimina un film dal sistema.
    /// </summary>
    /// <param name="id">ID del film da eliminare.</param>
    /// <returns>True se eliminato, false se non trovato.</returns>
    public async Task<bool> EliminaAsync(string id)
    {
        Movie? movie = await _contesto.Movies.FindAsync(id);

        if (movie == null)
        {
            return false;
        }

        _contesto.Movies.Remove(movie);
        await _contesto.SaveChangesAsync();

        return true;
    }
}
```

## MovieService.cs (Versione 1.2)

Utente: Greg
Data: 21/05/2026 12:30
Descrizione: Modificare i valori di ritorno per non esporre dati superflui

```c#
using Microsoft.EntityFrameworkCore;
using NuovoCinemaParadiso.Data;
using NuovoCinemaParadiso.Dtos;
using NuovoCinemaParadiso.Models;
using NuovoCinemaParadiso.Exceptions;

namespace NuovoCinemaParadiso.Services;
/// <summary>
/// Service responsabile della gestione dei film.
/// Gestisce operazioni CRUD e associazione con i generi.
/// </summary>
public class MovieService
{
    private readonly ContestoDb _contesto;

    private readonly GenereMovieService _genereMovieService;
    /// <summary>
    /// Inizializza una nuova istanza del servizio Movie.
    /// </summary>
    /// <param name="contesto">Contesto database EF Core.</param>
    /// <param name="genereMovieService">Servizio per la gestione dei generi film.</param>
    public MovieService(ContestoDb contesto, GenereMovieService genereMovieService)
    {
        _contesto = contesto;
        _genereMovieService = genereMovieService;
    }

    /// <summary>
    /// Recupera tutti i film presenti nel sistema con informazioni sul genere.
    /// </summary>
    /// <returns>Lista di DTO dei film.</returns>
    public async Task<List<DtoMovie>> OttieniTutto()
    {
        List<Movie> movies = await _contesto.Movies.ToListAsync();

        List<DtoMovie> risultato = new List<DtoMovie>();

        for (int i = 0; i < movies.Count; i++)
        {
            Movie movieCorrente = movies[i];
            GenereMovie? genereMovie = await _contesto.GeneriMovies.FindAsync(movieCorrente.GenereId);

            DtoMovie dto = new DtoMovie();
            dto.Id = movieCorrente.Id;
            dto.Titolo = movieCorrente.Titolo;
            dto.Descrizione = movieCorrente.Descrizione;
            dto.DurataMinuti = movieCorrente.DurataMinuti;
            dto.PrezzoMovie = movieCorrente.PrezzoMovie;
            dto.GenereId = movieCorrente.GenereId;
            dto.Genere = genereMovie?.Genere ?? "";

            risultato.Add(dto);
        }
        return risultato;
    }

    /// <summary>
    /// Recupera un film tramite ID.
    /// </summary>
    /// <param name="id">ID del film.</param>
    /// <returns>DTO del film se trovato, altrimenti null.</returns>
    public async Task<DtoMovie?> OttieniTramiteIdAsync(string id)
    {
        
        Movie? movie = await _contesto.Movies.FindAsync(id);

        if (movie == null)
        {
            return null;
        }
        GenereMovie? genereMovie = await _contesto.GeneriMovies.FindAsync(movie.GenereId);

        DtoMovie risultato = new DtoMovie
        {
            Id = movie.Id,
            Titolo = movie.Titolo,
            Descrizione = movie.Descrizione,
            DurataMinuti = movie.DurataMinuti,
            PrezzoMovie = movie.PrezzoMovie,
            GenereId = movie.GenereId,
            Genere = genereMovie?.Genere ?? ""
        };
        return risultato;
    }

    /// <summary>
    /// Recupera tutti i film appartenenti a un genere specifico.
    /// </summary>
    /// <param name="genereId">ID del genere.</param>
    /// <returns>Lista di film filtrati per genere.</returns>
    public async Task<List<DtoMovie>> OttieniTramiteGenere(string genereId)
    {
        List<DtoMovie> risultato = new List<DtoMovie>();

        List<DtoMovie> movies = await OttieniTutto();

        foreach (var movie in movies)
        {
            if (movie.GenereId.Trim() == genereId)
            {
                risultato.Add(movie);
            }
        }
        return risultato;
    }

    /// <summary>
    /// Crea un nuovo film.
    /// </summary>
    /// <param name="dto">Dati del film da creare.</param>
    /// <returns>booleano che da conferma tramite true o false in caso il genere non sia valido</returns>
    public async Task<bool> CreazioneAsync(DtoCreazioneMovie dto)
    {

        if (await _genereMovieService.OttieniTramiteIdAsync(dto.GenereId) == null)
        {
            return false;
        }
        
        Movie movie = new Movie();

        movie.Titolo = dto.Titolo;
        movie.Descrizione = dto.Descrizione;
        movie.DurataMinuti = dto.DurataMinuti;
        movie.PrezzoMovie = dto.PrezzoMovie;
        movie.GenereId = dto.GenereId;

        _contesto.Movies.Add(movie);
        await _contesto.SaveChangesAsync();

        return true;
    }

    /// <summary>
    /// Modifica un film esistente nel database.
    /// </summary>
    /// <param name="id">ID del film da modificare.</param>
    /// <param name="dto">Nuovi dati del film.</param>
    /// <returns>Ritorna true se la modifica è avvenuta con successo.</returns>
    /// <exception cref="NotFoundException">
    /// Lanciata se il film specificato o il genere non vengono trovati nel database.
    /// </exception>
    public async Task<bool> ModificaAsync(string id, DtoCreazioneMovie dto)
    {
        Movie? movieEsistente = await _contesto.Movies.FindAsync(id);

        if (await _genereMovieService.OttieniTramiteIdAsync(dto.GenereId) == null)
        {
            Console.WriteLine("GENERE PROBLEMA");
            throw new NotFoundException("genere", dto.GenereId);

        }

        if (movieEsistente == null)
        {
            Console.WriteLine("PROBLEMA ESISTENZIALE");
            throw new NotFoundException("movie",id);
        }

        movieEsistente.Titolo = dto.Titolo;
        movieEsistente.Descrizione = dto.Descrizione;
        movieEsistente.DurataMinuti = dto.DurataMinuti;
        movieEsistente.PrezzoMovie = dto.PrezzoMovie;
        movieEsistente.GenereId = dto.GenereId;

        await _contesto.SaveChangesAsync();
        
        return true;
    }

    /// <summary>
    /// Elimina un film dal sistema.
    /// </summary>
    /// <param name="id">ID del film da eliminare.</param>
    /// <returns>True se eliminato, false se non trovato.</returns>
    public async Task<bool> EliminaAsync(string id)
    {
        Movie? movie = await _contesto.Movies.FindAsync(id);

        if (movie == null)
        {
            return false;
        }

        _contesto.Movies.Remove(movie);
        await _contesto.SaveChangesAsync();

        return true;
    }
}
```

## OperatoreService.cs V1.0

```c#
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NuovoCinemaParadiso.Data;
using NuovoCinemaParadiso.Dtos;
using NuovoCinemaParadiso.Models;
using NuovoCinemaParadiso.Helpers;
using NuovoCinemaParadiso.Exceptions;

namespace NuovoCinemaParadiso.Services;

/// <summary>
/// Service dedicato alle operazioni dell’operatore.
/// Gestisce utenti, biglietti, abbonamenti e GiftCard.
/// </summary>
public class OperatoreService
{
    private readonly ContestoDb _contesto;
    private readonly UserManager<Utente> _gestioneUtenti;

    /// <summary>
    /// Inizializza il servizio operatore.
    /// </summary>
    /// <param name="contestoDb">Contesto database EF Core.</param>
    /// <param name="gestioneUtenti">UserManager Identity per gestione utenti.</param>
    public OperatoreService(ContestoDb contestoDb, UserManager<Utente> gestioneUtenti)
    {
        _contesto = contestoDb;
        _gestioneUtenti = gestioneUtenti;
    }

    /// <summary>
    /// Ricarica il saldo di un utente tramite email.
    /// </summary>
    /// <param name="dtoRicarica">Dati di ricarica (email e importo).</param>
    /// <returns>True se l’operazione ha successo, false altrimenti.</returns>
    public async Task<bool> RicaricaAsync(DtoRicarica dtoRicarica)
    {
        Utente? utente = await _gestioneUtenti.FindByEmailAsync(dtoRicarica.Email);

        if (utente == null)
            return false;

        utente.Saldo += dtoRicarica.Ricarica;
        await _gestioneUtenti.UpdateAsync(utente);

        return true;
    }

    /// <summary>
    /// Recupera tutti gli utenti del sistema con informazioni sull’abbonamento.
    /// </summary>
    /// <returns>Lista di utenti DTO.</returns>
    public async Task<List<DtoUtente>> OttieniUtentiAsync()
    {
        List<Utente> utenti = await _contesto.Utenti.ToListAsync();
        List<DtoUtente> risultato = new List<DtoUtente>();

        for (int i = 0; i < utenti.Count; i++)
        {
            Utente utenteCorrente = utenti[i];

            Abbonamento? abbonamento =
                await _contesto.Abbonamenti.FindAsync(utenteCorrente.AbbonamentoId);

            DtoUtente dto = new DtoUtente
            {
                Id = utenteCorrente.Id,
                Email = utenteCorrente.Email ?? string.Empty,
                NomeCompleto = utenteCorrente.NomeCompleto ?? string.Empty,
                Eta = utenteCorrente.Eta,
                AbbonamentoId = utenteCorrente.AbbonamentoId ?? string.Empty,
                SeAbbonato = utenteCorrente.SeAbbonato,
                DataInizioAbbonamento = utenteCorrente.DataInizioAbbonamento,
                TipoAbbonamento = abbonamento?.Nome ?? string.Empty
            };

            risultato.Add(dto);
        }

        return risultato;
    }

    /// <summary>
    /// Recupera un utente tramite ID.
    /// </summary>
    /// <param name="id">ID dell’utente.</param>
    /// <returns>DTO dell’utente.</returns>
    /// <exception cref="NotFoundException">Se l’utente non esiste.</exception>
    public async Task<DtoUtente?> OttieniUtenteTramiteIdAsync(string id)
    {
        Utente? utente =
            await _gestioneUtenti.FindByIdAsync(id)
            ?? throw new NotFoundException("Utente", id);

        Abbonamento? abbonamento =
            await _contesto.Abbonamenti.FindAsync(utente.AbbonamentoId);

        return new DtoUtente
        {
            Id = utente.Id,
            Email = utente.Email ?? string.Empty,
            NomeCompleto = utente.NomeCompleto ?? string.Empty,
            Eta = utente.Eta,
            SeAbbonato = utente.SeAbbonato,
            AbbonamentoId = utente.AbbonamentoId ?? string.Empty,
            DataInizioAbbonamento = utente.DataInizioAbbonamento,
            TipoAbbonamento = abbonamento?.Nome ?? string.Empty
        };
    }

    /// <summary>
    /// Elimina un utente tramite ID.
    /// </summary>
    /// <param name="id">ID utente.</param>
    /// <returns>IdentityResult dell’operazione.</returns>
    /// <exception cref="NotFoundException">Se l’utente non esiste.</exception>
    public async Task<IdentityResult> EliminaUtentePerIdAsync(string id)
    {
        Utente? utente = await _gestioneUtenti.FindByIdAsync(id);

        if (utente == null)
            throw new NotFoundException("Utente", id);

        return await _gestioneUtenti.DeleteAsync(utente);
    }

    /// <summary>
    /// Recupera tutti i biglietti presenti nel sistema con dati completi.
    /// </summary>
    /// <returns>Lista di biglietti DTO.</returns>
    public async Task<List<DtoBiglietto>> OttieniBiglietti()
    {
        List<Biglietto> biglietti = await _contesto.Biglietti.ToListAsync();
        List<DtoBiglietto> risultato = new List<DtoBiglietto>();

        for (int i = 0; i < biglietti.Count; i++)
        {
            Biglietto bigliettoCorrente = biglietti[i];

            Proiezione proiezione =
                await _contesto.Proiezioni.FindAsync(bigliettoCorrente.ProiezioneId)
                ?? throw new NotFoundException("Proiezione", bigliettoCorrente.ProiezioneId);

            Movie movie =
                await _contesto.Movies.FindAsync(proiezione.MovieId)
                ?? throw new NotFoundException("Movie", proiezione.MovieId);

            Sala sala =
                await _contesto.Sale.FindAsync(proiezione.SalaId)
                ?? throw new NotFoundException("Sala", proiezione.SalaId);

            Utente utente =
                await _contesto.Utenti.FindAsync(bigliettoCorrente.UtenteId)
                ?? throw new NotFoundException("Utente", bigliettoCorrente.UtenteId);

            TipologiaSala tipologiaSala =
                await _contesto.TipologieSala.FindAsync(sala.TipologiaSalaId)
                ?? throw new NotFoundException("TipologiaSala", sala.TipologiaSalaId);

            DtoBiglietto dto = new DtoBiglietto
            {
                Id = bigliettoCorrente.Id,
                ProiezioneId = bigliettoCorrente.ProiezioneId,
                UtenteId = bigliettoCorrente.UtenteId,
                NumeroBiglietti = bigliettoCorrente.NumeroBiglietti,
                OrarioCreazione = bigliettoCorrente.OrarioCreazione,
                PrezzoFinale = Calcoli.CalcolaPrezzoFinale(
                    movie.PrezzoMovie,
                    tipologiaSala.MaggiorazionePrezzo,
                    bigliettoCorrente.NumeroBiglietti,
                    utente,
                    bigliettoCorrente.MetodoPagamento)
            };

            risultato.Add(dto);
        }

        return risultato;
    }

    /// <summary>
    /// Recupera un biglietto tramite ID.
    /// </summary>
    /// <param name="id">ID del biglietto.</param>
    /// <returns>DTO del biglietto.</returns>
    public async Task<DtoBiglietto> OttieniBigliettoTramiteIdAsync(string id)
    {
        Biglietto biglietto =
            await _contesto.Biglietti.FindAsync(id)
            ?? throw new NotFoundException("Biglietto", id);

        Proiezione proiezione =
            await _contesto.Proiezioni.FindAsync(biglietto.ProiezioneId)
            ?? throw new NotFoundException("Proiezione", biglietto.ProiezioneId);

        Movie movie =
            await _contesto.Movies.FindAsync(proiezione.MovieId)
            ?? throw new NotFoundException("Movie", proiezione.MovieId);

        Sala sala =
            await _contesto.Sale.FindAsync(proiezione.SalaId)
            ?? throw new NotFoundException("Sala", proiezione.SalaId);

        Utente utente =
            await _contesto.Users.FindAsync(biglietto.UtenteId)
            ?? throw new NotFoundException("Utente", biglietto.UtenteId);

        TipologiaSala tipologiaSala =
            await _contesto.TipologieSala.FindAsync(sala.TipologiaSalaId)
            ?? throw new NotFoundException("TipologiaSala", sala.TipologiaSalaId);

        return new DtoBiglietto
        {
            Id = biglietto.Id,
            UtenteId = biglietto.UtenteId,
            ProiezioneId = biglietto.ProiezioneId,
            NumeroBiglietti = biglietto.NumeroBiglietti,
            OrarioCreazione = biglietto.OrarioCreazione,
            PrezzoFinale = Calcoli.CalcolaPrezzoFinale(
                movie.PrezzoMovie,
                tipologiaSala.MaggiorazionePrezzo,
                biglietto.NumeroBiglietti,
                utente,
                biglietto.MetodoPagamento)
        };
    }

    /// <summary>
    /// Recupera utenti filtrati per abbonamento.
    /// </summary>
    /// <param name="abbonamentoId">ID abbonamento.</param>
    /// <returns>Lista utenti con quel abbonamento.</returns>
    public async Task<List<DtoUtente>> OttieniUtentiTramiteAbbonamentoAsync(string abbonamentoId)
    {
        List<Utente> utenti = await _contesto.Utenti.ToListAsync();
        List<Abbonamento> abbonamenti = await _contesto.Abbonamenti.ToListAsync();

        Abbonamento? abbonamentoTrovato = abbonamenti
            .FirstOrDefault(a => a.Id == abbonamentoId);

        if (abbonamentoTrovato == null)
            return new List<DtoUtente>();

        List<DtoUtente> risultato = new List<DtoUtente>();

        foreach (var utenteCorrente in utenti)
        {
            if (utenteCorrente.Abbonamento == abbonamentoTrovato)
            {
                risultato.Add(new DtoUtente
                {
                    Id = utenteCorrente.Id,
                    NomeCompleto = utenteCorrente.NomeCompleto,
                    Email = utenteCorrente.Email ?? string.Empty,
                    Eta = utenteCorrente.Eta,
                    SeAbbonato = utenteCorrente.SeAbbonato,
                    AbbonamentoId = utenteCorrente.AbbonamentoId ?? string.Empty,
                    DataInizioAbbonamento = utenteCorrente.DataInizioAbbonamento,
                    TipoAbbonamento = abbonamentoTrovato.Nome
                });
            }
        }

        return risultato;
    }

    /// <summary>
    /// Crea una nuova GiftCard con importo ricaricato.
    /// </summary>
    /// <param name="dto">Dati della ricarica GiftCard.</param>
    /// <returns>GiftCard creata.</returns>
    public async Task<DtoGiftCard> RicaricaGiftCardAsync(DtoRicaricaGiftCard dto)
    {
        if (dto.Importo <= 0)
            throw new Exception("Importo non valido.");

        GiftCard nuovaGiftCard = new GiftCard
        {
            Nome = "GiftCard",
            Valore = dto.Importo,
            CodiceRiscatto = GiftCardHelper.GeneraCodice()
        };

        await _contesto.GiftCards.AddAsync(nuovaGiftCard);
        await _contesto.SaveChangesAsync();

        return new DtoGiftCard
        {
            Id = nuovaGiftCard.Id,
            Nome = nuovaGiftCard.Nome,
            Valore = nuovaGiftCard.Valore,
            CodiceRiscatto = nuovaGiftCard.CodiceRiscatto
        };
    }
}
```

```c#

    /*
    Aggiornata la chiamata al calcolo prezzo finale abbonamento
    PrezzoFinale = Calcoli.CalcolaPrezzoFinale(
                    movie.PrezzoMovie,
                    tipologiaSala.MaggiorazionePrezzo,
                    bigliettoCorrente.NumeroBiglietti,
                    utente,
                    bigliettoCorrente.MetodoPagamento)
    */
    PrezzoFinale = Calcoli.CalcolaPrezzoFinale(movie.PrezzoMovie, tipologiaSala.MaggiorazionePrezzo, dto.NumeroBiglietti, utente.Abbonamento, utente.DataInizioAbbonamento)
```

## OperatoreService.cs V1.1

Andrea Bruno 22-05-2026
Aggiunta query turno 
Aggiunta campi nel DtoBiglietto di ritorno

```c#
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NuovoCinemaParadiso.Data;
using NuovoCinemaParadiso.Dtos;
using NuovoCinemaParadiso.Models;
using NuovoCinemaParadiso.Helpers;
using NuovoCinemaParadiso.Exceptions;

namespace NuovoCinemaParadiso.Services;

public class OperatoreService
{
    private readonly ContestoDb _contesto;
    private readonly UserManager<Utente> _gestioneUtenti;
    public OperatoreService(ContestoDb contestoDb, UserManager<Utente> gestioneUtenti)
    {
        _contesto = contestoDb;
        _gestioneUtenti = gestioneUtenti;
    }

    public async Task<bool> RicaricaAsync(DtoRicaricaSaldoUtente dtoRicaricaSaldoUtente)
    {
        Utente? utente = await _gestioneUtenti.FindByEmailAsync(dtoRicaricaSaldoUtente.Email);
        if (utente == null)
            return false;
        utente.Saldo += dtoRicaricaSaldoUtente.Ricarica;
        await _gestioneUtenti.UpdateAsync(utente);
        return true;
    }

    public async Task<List<DtoUtente>> OttieniUtentiAsync()
    {
        List<Utente> utenti = await _contesto.Utenti.ToListAsync();

        List<DtoUtente> risultato = new List<DtoUtente>();

        for (int i = 0; i < utenti.Count; i++)
        {
            Utente utenteCorrente = utenti[i];

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

    public async Task<DtoUtente?> OttieniUtenteTramiteIdAsync(string id)
    {
        Utente? utente = await _gestioneUtenti.FindByIdAsync(id)
            ?? throw new NotFoundException("Utente", id);
        Abbonamento? abbonamento = await _contesto.Abbonamenti.FindAsync(utente.AbbonamentoId);
        if (utente == null)
        {
            throw new NotFoundException("Utente", id);
        }

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

    public async Task<bool> EliminaUtentePerIdAsync(string id)
    {
        Utente? utente = await _gestioneUtenti.FindByIdAsync(id);
        if (utente == null)
        {
            throw new NotFoundException("Utente", id);
        }
        IdentityResult risultato = await _gestioneUtenti.DeleteAsync(utente);

        return true;
    }

    public async Task<List<DtoBiglietto>> OttieniBiglietti()
    {
        List<Biglietto> biglietti = await _contesto.Biglietti.ToListAsync();

        List<DtoBiglietto> risultato = new List<DtoBiglietto>();

        for (int i = 0; i < biglietti.Count; i++)
        {
            Biglietto bigliettoCorrente = biglietti[i];
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
            // Recupera il turno associato alla proiezione.
            // Se non esiste, lancia una NotFoundException specifica con nome entità e ID.
            // Questo mantiene il flusso null-safe e fornisce un errore REST chiaro.
            Turno? turno = await _contesto.Turni.FindAsync(proiezione.TurnoId)
                ?? throw new NotFoundException("Turno", proiezione.TurnoId);

            if (utente.AbbonamentoId == null)
                throw new NotFoundException("Abbonamento", "Nessun abbonamento associato all'utente");

            if (utente.Abbonamento == null)
                throw new NotFoundException("Abbonamento", utente.AbbonamentoId);

            DtoBiglietto dto = new DtoBiglietto();
            dto.Id = bigliettoCorrente.Id;
            dto.ProiezioneId = bigliettoCorrente.ProiezioneId;
            dto.UtenteId = bigliettoCorrente.UtenteId;
            dto.OrarioCreazione = bigliettoCorrente.OrarioCreazione;
            dto.NumeroBiglietti = bigliettoCorrente.NumeroBiglietti;
            dto.NomeSala = sala.Nome;
            dto.TitoloMovie = movie.Titolo;
            dto.NomeTipologiaSala = tipologiaSala.Nome;
            dto.OraInizio = turno.OraInizio;
            dto.DataProiezione = proiezione.DataProiezione;
            dto.PrezzoFinale = Calcoli.CalcolaPrezzoFinale(movie.PrezzoMovie,
                tipologiaSala.MaggiorazionePrezzo,
                bigliettoCorrente.NumeroBiglietti,
                utente.Abbonamento,
                utente.DataInizioAbbonamento
            );

            risultato.Add(dto);
        }

        return risultato;
    }

    public async Task<DtoBiglietto> OttieniBigliettoTramiteIdAsync(string id)
    {
        Biglietto? biglietto = await _contesto.Biglietti.FindAsync(id)
            ?? throw new NotFoundException("Biglietto", id);
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
        // Recupera il turno associato alla proiezione.
        // Se non esiste, lancia una NotFoundException specifica con nome entità e ID.
        // Questo mantiene il flusso null-safe e fornisce un errore REST chiaro.
        Turno? turno = await _contesto.Turni.FindAsync(proiezione.TurnoId)
            ?? throw new NotFoundException("Turno", proiezione.TurnoId);

        if (utente.AbbonamentoId == null)
            throw new NotFoundException("Abbonamento", "Nessun abbonamento associato all'utente");

        if (utente.Abbonamento == null)
            throw new NotFoundException("Abbonamento", utente.AbbonamentoId);

        if (biglietto == null)
            throw new NotFoundException("Biglietto", id);

        DtoBiglietto dto = new DtoBiglietto();
        dto.Id = biglietto.Id;
        dto.UtenteId = biglietto.UtenteId;
        dto.ProiezioneId = biglietto.ProiezioneId;
        dto.OrarioCreazione = biglietto.OrarioCreazione;
        dto.NumeroBiglietti = biglietto.NumeroBiglietti;
        dto.NomeSala = sala.Nome;
        dto.TitoloMovie = movie.Titolo;
        dto.NomeTipologiaSala = tipologiaSala.Nome;
        dto.OraInizio = turno.OraInizio;
        dto.DataProiezione = proiezione.DataProiezione;
        dto.PrezzoFinale = Calcoli.CalcolaPrezzoFinale(movie.PrezzoMovie,
            tipologiaSala.MaggiorazionePrezzo,
            biglietto.NumeroBiglietti,
            utente.Abbonamento,
            utente.DataInizioAbbonamento
        );

        return dto;
    }

    public async Task<List<DtoUtente>> OttieniUtentiTramiteAbbonamentoAsync(string abbonamentoId)
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

    public async Task<bool> RicaricaGiftCardAsync(DtoRicaricaGiftCard dto)
    {
        if (dto.Importo <= 0)
        {
            throw new Exception("Impossibile ricaricare la giftcard. Importo non valido.");
        }

        /* L'operatore crea la GiftCard "dal nulla", senza scalare un saldo, 
        perché si presume che il pagamento sia stato gestito in cassa.*/
        GiftCard nuovaGiftCard = new GiftCard()
        {
            Nome = "GiftCard",
            Valore = dto.Importo,
            CodiceRiscatto = GiftCardHelper.GeneraCodice()
        };

        await _contesto.GiftCards.AddAsync(nuovaGiftCard);
        await _contesto.SaveChangesAsync();

        return true;
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

/// <summary>
/// Servizio che gestisce le operazioni relative alle proiezioni dei film.
/// Include creazione, modifica, eliminazione e query filtrate per film, sala e turno.
/// </summary>
public class ProiezioneService
{
    private readonly ContestoDb _contesto;

    /// <summary>
    /// Inizializza il servizio delle proiezioni con il contesto del database.
    /// </summary>
    /// <param name="contesto">Contesto Entity Framework del database.</param>
    public ProiezioneService(ContestoDb contesto)
    {
        _contesto = contesto;
    }

    /// <summary>
    /// Restituisce tutte le proiezioni attive.
    /// </summary>
    /// <returns>Lista di proiezioni attive in formato DTO.</returns>
    public async Task<List<DtoProiezione>> OttieniTuttoAsync()
    {
        List<Proiezione> proiezioni = await _contesto.Proiezioni.ToListAsync();
        List<DtoProiezione> risultato = new List<DtoProiezione>();

        for (int i = 0; i < proiezioni.Count; i++)
        {
            Proiezione proiezioneCorrente = proiezioni[i];

            if (proiezioneCorrente.Attivo)
            {
                Movie? film = await _contesto.Movies.FindAsync(proiezioneCorrente.MovieId);
                Sala? sala = await _contesto.Sale.FindAsync(proiezioneCorrente.SalaId);

                DtoProiezione dto = new DtoProiezione
                {
                    Id = proiezioneCorrente.Id,
                    DataProiezione = proiezioneCorrente.DataProiezione,
                    MovieId = proiezioneCorrente.MovieId,
                    SalaId = proiezioneCorrente.SalaId,
                    TurnoId = proiezioneCorrente.TurnoId,
                    Attivo = proiezioneCorrente.Attivo
                };

                risultato.Add(dto);
            }
        }

        return risultato;
    }

    /// <summary>
    /// Restituisce tutte le proiezioni, comprese quelle inattive (storico).
    /// </summary>
    /// <returns>Lista completa delle proiezioni in formato DTO.</returns>
    public async Task<List<DtoProiezione>> OttieniStoricoAsync()
    {
        List<Proiezione> proiezioni = await _contesto.Proiezioni.ToListAsync();
        List<DtoProiezione> risultato = new List<DtoProiezione>();

        for (int i = 0; i < proiezioni.Count; i++)
        {
            Proiezione proiezioneCorrente = proiezioni[i];

            Movie? film = await _contesto.Movies.FindAsync(proiezioneCorrente.MovieId);
            Sala? sala = await _contesto.Sale.FindAsync(proiezioneCorrente.SalaId);

            DtoProiezione dto = new DtoProiezione
            {
                Id = proiezioneCorrente.Id,
                DataProiezione = proiezioneCorrente.DataProiezione,
                MovieId = proiezioneCorrente.MovieId,
                SalaId = proiezioneCorrente.SalaId,
                TurnoId = proiezioneCorrente.TurnoId,
                Attivo = proiezioneCorrente.Attivo
            };

            risultato.Add(dto);
        }

        return risultato;
    }

    /// <summary>
    /// Restituisce una proiezione tramite ID.
    /// </summary>
    /// <param name="id">ID della proiezione.</param>
    /// <returns>DTO della proiezione oppure null se non trovata.</returns>
    public async Task<DtoProiezione?> OttieniTramiteIdAsync(string id)
    {
        Proiezione? proiezione = await _contesto.Proiezioni.FindAsync(id);

        if (proiezione == null)
            return null;

        Movie? film = await _contesto.Movies.FindAsync(proiezione.MovieId);
        Sala? sala = await _contesto.Sale.FindAsync(proiezione.SalaId);
        Turno? turno = await _contesto.Turni.FindAsync(proiezione.TurnoId);

        return new DtoProiezione
        {
            Id = proiezione.Id,
            DataProiezione = proiezione.DataProiezione,
            MovieId = proiezione.MovieId,
            SalaId = proiezione.SalaId,
            TurnoId = proiezione.TurnoId,
            Attivo = proiezione.Attivo
        };
    }

    /// <summary>
    /// Restituisce tutte le proiezioni associate a un film.
    /// </summary>
    public async Task<List<DtoProiezione>> OttieniTramiteMovieAsync(string movieId)
    {
        List<DtoProiezione> risultato = new List<DtoProiezione>();
        List<Proiezione> proiezioni = await _contesto.Proiezioni.ToListAsync();

        foreach (var p in proiezioni)
        {
            if (p.MovieId == movieId)
            {
                risultato.Add(new DtoProiezione
                {
                    Id = p.Id,
                    DataProiezione = p.DataProiezione,
                    MovieId = p.MovieId,
                    SalaId = p.SalaId,
                    TurnoId = p.TurnoId,
                    Attivo = p.Attivo
                });
            }
        }

        return risultato;
    }

    /// <summary>
    /// Restituisce tutte le proiezioni associate a una sala.
    /// </summary>
    public async Task<List<DtoProiezione>> OttieniTramiteSalaAsync(string salaId)
    {
        List<DtoProiezione> risultato = new List<DtoProiezione>();
        List<Proiezione> proiezioni = await _contesto.Proiezioni.ToListAsync();

        foreach (var p in proiezioni)
        {
            if (p.SalaId == salaId)
            {
                risultato.Add(new DtoProiezione
                {
                    Id = p.Id,
                    DataProiezione = p.DataProiezione,
                    MovieId = p.MovieId,
                    SalaId = p.SalaId,
                    TurnoId = p.TurnoId,
                    Attivo = p.Attivo
                });
            }
        }

        return risultato;
    }

    /// <summary>
    /// Restituisce tutte le proiezioni associate a un turno.
    /// </summary>
    public async Task<List<DtoProiezione>> OttieniTramiteTurnoAsync(string turnoId)
    {
        List<DtoProiezione> risultato = new List<DtoProiezione>();
        List<Proiezione> proiezioni = await _contesto.Proiezioni.ToListAsync();

        foreach (var p in proiezioni)
        {
            if (p.TurnoId == turnoId)
            {
                risultato.Add(new DtoProiezione
                {
                    Id = p.Id,
                    DataProiezione = p.DataProiezione,
                    MovieId = p.MovieId,
                    SalaId = p.SalaId,
                    TurnoId = p.TurnoId,
                    Attivo = p.Attivo
                });
            }
        }

        return risultato;
    }

    /// <summary>
    /// Crea una nuova proiezione.
    /// </summary>
    public async Task<DtoProiezione?> CreazioneAsync(DtoCreazioneProiezione dto)
    {
        Proiezione proiezione = new Proiezione
        {
            DataProiezione = dto.DataProiezione,
            MovieId = dto.MovieId,
            SalaId = dto.SalaId,
            TurnoId = dto.TurnoId
        };

        _contesto.Proiezioni.Add(proiezione);
        await _contesto.SaveChangesAsync();

        return new DtoProiezione
        {
            Id = proiezione.Id,
            DataProiezione = proiezione.DataProiezione,
            MovieId = proiezione.MovieId,
            SalaId = proiezione.SalaId,
            TurnoId = proiezione.TurnoId,
            Attivo = proiezione.Attivo
        };
    }

    /// <summary>
    /// Modifica una proiezione esistente.
    /// </summary>
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

        return new DtoProiezione
        {
            Id = proiezione.Id,
            DataProiezione = proiezione.DataProiezione,
            MovieId = proiezione.MovieId,
            SalaId = proiezione.SalaId,
            TurnoId = proiezione.TurnoId,
            Attivo = proiezione.Attivo
        };
    }

    /// <summary>
    /// Disattiva (soft delete) una proiezione.
    /// </summary>
    public async Task<bool> EliminaAsync(string id)
    {
        Proiezione? proiezione = await _contesto.Proiezioni.FindAsync(id);

        if (proiezione == null)
            return false;

        proiezione.Attivo = false;
        await _contesto.SaveChangesAsync();

        return true;
    }
}
```
## ProiezioneService.cs Versione 1.1
- Utente: Simeone
- Data: 21/05/2026
- Descrizione: eseguo la task assegnatami su questo service e il conseguente controller: "MODIFICARE CONTROLLER E SERVICE CHE NON HANNO LA NECESSITà DI AVERE DTO IN USCITA CHE PORTINO DATI NON NECESSARI AL FRONTEND.
DI CONSEGUENZA ALCUNI AZIONI RIPORTERANNO SOLO UN TRUE O UN FALSE CON UN MESSAGGIO DI RIUSCITA O FALLIMENTO."
```c#
using Microsoft.AspNetCore.Mvc;
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

    public async Task<bool> CreazioneAsync(DtoCreazioneProiezione dto) // modificato in bool per evitare di restituire troppi dati non necessari
    {
        Proiezione proiezione = new Proiezione();

        
        proiezione.DataProiezione = dto.DataProiezione;
        proiezione.MovieId = dto.MovieId;
        proiezione.SalaId = dto.SalaId;
        proiezione.TurnoId = dto.TurnoId;
        proiezione.Attivo = true; // <- AGGIUNTA perché altrimenti andava direttamente nello storico e non nel get standard per le attive
        
        _contesto.Proiezioni.Add(proiezione);
        await _contesto.SaveChangesAsync();

        /// --- IL CODICE COMMENTATO FRA /**/ è CIò CHE ABBIAMO RIMOSSO !!! ---
        /*Movie? film = await _contesto.Movies.FindAsync(proiezione.MovieId);
        Sala? sala = await _contesto.Sale.FindAsync(proiezione.SalaId);
        Turno? turno = await _contesto.Turni.FindAsync(proiezione.TurnoId);

        
        DtoProiezione risultato = new DtoProiezione();
        risultato.Id = proiezione.Id;
        risultato.DataProiezione = proiezione.DataProiezione;
        risultato.MovieId = proiezione.MovieId;
        risultato.SalaId = proiezione.SalaId;
        risultato.TurnoId = proiezione.TurnoId;
        risultato.Attivo = proiezione.Attivo;
        return risultato;*/

        return true; // modificato in true poiché ora il task è bool per evitare di restituire troppi dati non necessari
    }

    public async Task<bool> ModificaAsync(string id, DtoCreazioneProiezione dto) // modificato in bool per evitare di restituire troppi dati non necessari
    {
        Proiezione? proiezione = await _contesto.Proiezioni.FindAsync(id);

        if (proiezione == null)
        {
            return false; // modificato in false poiché ora il task è bool per evitare di restituire troppi dati non necessari
        }

        proiezione.DataProiezione = dto.DataProiezione;
        proiezione.MovieId = dto.MovieId;
        proiezione.SalaId = dto.SalaId;
        proiezione.TurnoId = dto.TurnoId;

        await _contesto.SaveChangesAsync();

        /// --- IL CODICE COMMENTATO FRA /**/ è CIò CHE ABBIAMO RIMOSSO !!! ---
        /*Movie? film = await _contesto.Movies.FindAsync(proiezione.MovieId);
        Sala? sala = await _contesto.Sale.FindAsync(proiezione.SalaId);
        Turno? turno = await _contesto.Turni.FindAsync(proiezione.TurnoId);

        DtoProiezione risultato = new DtoProiezione();
        risultato.Id = proiezione.Id;
        risultato.DataProiezione = proiezione.DataProiezione;
        risultato.MovieId = proiezione.MovieId;
        risultato.SalaId = proiezione.SalaId;
        risultato.TurnoId = proiezione.TurnoId;
        risultato.Attivo = proiezione.Attivo;
        return risultato;*/

        return true; // modificato in true poiché ora il task è bool per evitare di restituire troppi dati non necessari


    }

    public async Task<bool> EliminaAsync(string id)
    {
        Proiezione? proiezione = await _contesto.Proiezioni.FindAsync(id);

        if (proiezione == null)
        {
            return false;
        }

        proiezione.Attivo = false; // <-- ci si ricollega al commento sul controller
        await _contesto.SaveChangesAsync();
        
        return true; 
    }
}
```
## RuoloUtenteService.cs

```c#
using Microsoft.AspNetCore.Identity;
using NuovoCinemaParadiso.Dtos;
using NuovoCinemaParadiso.Models;

namespace NuovoCinemaParadiso.Services;

/// <summary>
/// Servizio responsabile della gestione dei ruoli degli utenti.
/// Permette di modificare il ruolo assegnato a un utente.
/// </summary>
public class RuoloUtenteService
{
    private readonly UserManager<Utente> _gestioneUtenti;

    /// <summary>
    /// Inizializza il servizio di gestione ruoli utente.
    /// </summary>
    /// <param name="gestioneUtenti">UserManager di Identity per la gestione degli utenti.</param>
    public RuoloUtenteService(UserManager<Utente> gestioneUtenti)
    {
        _gestioneUtenti = gestioneUtenti;
    }

    /// <summary>
    /// Modifica il ruolo di un utente assegnandone uno nuovo.
    /// </summary>
    /// <param name="dto">DTO contenente email utente e nuovo ruolo da assegnare.</param>
    /// <returns>
    /// Il nuovo ruolo assegnato se l’operazione ha successo,
    /// altrimenti null se l’utente non esiste o il ruolo non è valido.
    /// </returns>
    public async Task<string?> ModificaRuoloUtente(DtoModificaRuoloUtente dto)
    {
        // Controllo validità del ruolo
        if (dto.NuovoRuolo != Ruoli.Gestore &&
            dto.NuovoRuolo != Ruoli.Operatore &&
            dto.NuovoRuolo != Ruoli.Utente)
        {
            return null;
        }

        // Recupero utente da email
        Utente? utente = await _gestioneUtenti.FindByEmailAsync(dto.Email);
        if (utente == null)
        {
            return null;
        }

        // Recupero ruoli attuali
        IList<string> ruoloCorrente = await _gestioneUtenti.GetRolesAsync(utente);

        // Rimozione dei ruoli precedenti validi
        for (int i = 0; i < ruoloCorrente.Count; i++)
        {
            string currentRole = ruoloCorrente[i];

            if (currentRole == Ruoli.Gestore ||
                currentRole == Ruoli.Operatore ||
                currentRole == Ruoli.Utente)
            {
                await _gestioneUtenti.RemoveFromRoleAsync(utente, currentRole);
            }
        }

        // Assegnazione nuovo ruolo
        IdentityResult addResult = await _gestioneUtenti.AddToRoleAsync(utente, dto.NuovoRuolo);

        if (!addResult.Succeeded)
        {
            return null;
        }

        return dto.NuovoRuolo;
    }
}
```

## SalaService.cs

```c#
using Microsoft.EntityFrameworkCore;
using NuovoCinemaParadiso.Data;
using NuovoCinemaParadiso.Dtos;
using NuovoCinemaParadiso.Exceptions;
using NuovoCinemaParadiso.Models;

namespace NuovoCinemaParadiso.Services;

/// <summary>
/// Servizio che gestisce le operazioni CRUD e le query sulle sale cinematografiche.
/// Permette la gestione delle sale e delle relative tipologie.
/// </summary>
public class SalaService
{
    private readonly ContestoDb _contesto;

    /// <summary>
    /// Inizializza una nuova istanza del servizio SalaService.
    /// </summary>
    /// <param name="contesto">Contesto del database EF Core.</param>
    public SalaService(ContestoDb contesto)
    {
        _contesto = contesto;
    }

    /// <summary>
    /// Restituisce tutte le sale presenti nel sistema.
    /// </summary>
    /// <returns>Lista di DTO delle sale.</returns>
    public async Task<List<DtoSala>> OttieniTuttoAsync()
    {
        List<Sala> sale = await _contesto.Sale.ToListAsync();
        List<DtoSala> risultato = new List<DtoSala>();

        for (int i = 0; i < sale.Count; i++)
        {
            Sala salaCorrente = sale[i];

            TipologiaSala? tipologiaSala =
                await _contesto.TipologieSala.FindAsync(salaCorrente.TipologiaSalaId);

            DtoSala dto = new DtoSala
            {
                Id = salaCorrente.Id,
                Nome = salaCorrente.Nome,
                Capienza = salaCorrente.Capienza,
                NomeTipologia = tipologiaSala?.Nome ?? "",
                TipologiaSalaId = tipologiaSala?.Id ?? ""
            };

            risultato.Add(dto);
        }

        return risultato;
    }

    /// <summary>
    /// Restituisce una sala tramite ID.
    /// </summary>
    /// <param name="id">ID della sala.</param>
    /// <returns>DTO della sala oppure null se non trovata.</returns>
    public async Task<DtoSala?> OttieniTramiteIdAsync(string id)
    {
        Sala? sala = await _contesto.Sale.FindAsync(id);

        if (sala == null)
            return null;

        TipologiaSala? tipologiaSala =
            await _contesto.TipologieSala.FindAsync(sala.TipologiaSalaId);

        return new DtoSala
        {
            Id = sala.Id,
            Nome = sala.Nome,
            Capienza = sala.Capienza,
            NomeTipologia = tipologiaSala?.Nome ?? "",
            TipologiaSalaId = tipologiaSala?.Id ?? ""
        };
    }

    /// <summary>
    /// Restituisce tutte le sale filtrate per tipologia.
    /// </summary>
    /// <param name="tipologiaId">ID della tipologia sala.</param>
    /// <returns>Lista di sale appartenenti alla tipologia specificata.</returns>
    public async Task<List<DtoSala>> OttieniTramiteTipologiaAsync(string tipologiaId)
    {
        List<Sala> tutteLeSale = await _contesto.Sale.ToListAsync();
        List<DtoSala> risultato = new List<DtoSala>();

        foreach (var sala in tutteLeSale)
        {
            if (sala.TipologiaSalaId != tipologiaId)
                continue;

            TipologiaSala? tipologia =
                await _contesto.TipologieSala.FindAsync(sala.TipologiaSalaId);

            risultato.Add(new DtoSala
            {
                Id = sala.Id,
                Nome = sala.Nome,
                Capienza = sala.Capienza,
                TipologiaSalaId = sala.TipologiaSalaId,
                NomeTipologia = tipologia?.Nome ?? ""
            });
        }

        return risultato;
    }

    /// <summary>
    /// Crea una nuova sala.
    /// </summary>
    /// <param name="dto">Dati della sala da creare.</param>
    /// <returns>DTO della sala creata.</returns>
    public async Task<DtoSala> CreazioneAsync(DtoCreazioneSala dto)
    {
        Sala sala = new Sala
        {
            Nome = dto.Nome,
            Capienza = dto.Capienza,
            TipologiaSalaId = dto.TipologiaSalaId
        };

        _contesto.Sale.Add(sala);
        await _contesto.SaveChangesAsync();

        TipologiaSala? tipologiaSala =
            await _contesto.TipologieSala.FindAsync(sala.TipologiaSalaId);

        return new DtoSala
        {
            Id = sala.Id,
            Nome = sala.Nome,
            Capienza = sala.Capienza,
            TipologiaSalaId = tipologiaSala?.Id ?? "",
            NomeTipologia = sala.TipologiaSala?.Nome ?? ""
        };
    }

    /// <summary>
    /// Modifica una sala esistente.
    /// </summary>
    /// <param name="id">ID della sala da modificare.</param>
    /// <param name="dto">Nuovi dati della sala.</param>
    /// <returns>DTO della sala aggiornata.</returns>
    /// <exception cref="ItemNotFoundException">Se la sala non esiste.</exception>
    /// <exception cref="NotFoundException">Se la tipologia sala non esiste.</exception>
    /// <exception cref="ModificaException">Se esiste già una sala con lo stesso nome.</exception>
    public async Task<DtoSala?> ModificaAsync(string id, DtoCreazioneSala dto)
    {
        Sala? salaEsistente = await _contesto.Sale.FindAsync(id);

        if (salaEsistente == null)
            throw new ItemNotFoundException("Sala");

        TipologiaSala? tipologia =
            await _contesto.TipologieSala.FindAsync(dto.TipologiaSalaId);

        if (tipologia == null)
            throw new NotFoundException("TipologiaSala", dto.TipologiaSalaId);

        List<Sala> listaSale = await _contesto.Sale.ToListAsync();

        foreach (var salaCorrente in listaSale)
        {
            if (string.Equals(salaCorrente.Nome, dto.Nome, StringComparison.OrdinalIgnoreCase))
                throw new ModificaException("sala");
        }

        salaEsistente.Nome = dto.Nome;
        salaEsistente.Capienza = dto.Capienza;
        salaEsistente.TipologiaSalaId = dto.TipologiaSalaId;

        await _contesto.SaveChangesAsync();

        TipologiaSala? tipologiaSala =
            await _contesto.TipologieSala.FindAsync(salaEsistente.TipologiaSalaId);

        return new DtoSala
        {
            Id = salaEsistente.Id,
            Nome = salaEsistente.Nome,
            Capienza = salaEsistente.Capienza,
            TipologiaSalaId = salaEsistente.TipologiaSalaId,
            NomeTipologia = tipologiaSala?.Nome ?? ""
        };
    }

    /// <summary>
    /// Elimina una sala dal sistema.
    /// </summary>
    /// <param name="id">ID della sala da eliminare.</param>
    /// <returns>True se eliminata, false se non trovata.</returns>
    public async Task<bool> EliminaAsync(string id)
    {
        Sala? sala = await _contesto.Sale.FindAsync(id);

        if (sala == null)
            return false;

        _contesto.Sale.Remove(sala);
        await _contesto.SaveChangesAsync();

        return true;
    }
}
```

## SalaServiceV1.2
Francesco lorenzi
22/05/2026

modificati gli outpu della creazione e la modifica
```c#
using Microsoft.EntityFrameworkCore;
using NuovoCinemaParadiso.Data;
using NuovoCinemaParadiso.Dtos;
using NuovoCinemaParadiso.Exceptions;
using NuovoCinemaParadiso.Models;

namespace NuovoCinemaParadiso.Services;

/// <summary>
/// Servizio che gestisce le operazioni CRUD e le query sulle sale cinematografiche.
/// Permette la gestione delle sale e delle relative tipologie.
/// </summary>
public class SalaService
{
    private readonly ContestoDb _contesto;

    public SalaService(ContestoDb contesto)
    {
        _contesto = contesto;
    }

    public async Task<List<DtoSala>> OttieniTuttoAsync()
    {
        List<Sala> sale = await _contesto.Sale.ToListAsync();
        List<DtoSala> risultato = new List<DtoSala>();

        for (int i = 0; i < sale.Count; i++)
        {
            Sala salaCorrente = sale[i];

            TipologiaSala? tipologiaSala =
                await _contesto.TipologieSala.FindAsync(salaCorrente.TipologiaSalaId);

            DtoSala dto = new DtoSala
            {
                Id = salaCorrente.Id,
                Nome = salaCorrente.Nome,
                Capienza = salaCorrente.Capienza,
                NomeTipologia = tipologiaSala?.Nome ?? "",
                TipologiaSalaId = tipologiaSala?.Id ?? ""
            };

            risultato.Add(dto);
        }

        return risultato;
    }
    public async Task<DtoSala?> OttieniTramiteIdAsync(string id)
    {
        Sala? sala = await _contesto.Sale.FindAsync(id);

        if (sala == null)
            return null;

        TipologiaSala? tipologiaSala =
            await _contesto.TipologieSala.FindAsync(sala.TipologiaSalaId);

        return new DtoSala
        {
            Id = sala.Id,
            Nome = sala.Nome,
            Capienza = sala.Capienza,
            NomeTipologia = tipologiaSala?.Nome ?? "",
            TipologiaSalaId = tipologiaSala?.Id ?? ""
        };
    }

    public async Task<List<DtoSala>> OttieniTramiteTipologiaAsync(string tipologiaId)
    {
        List<Sala> tutteLeSale = await _contesto.Sale.ToListAsync();
        List<DtoSala> risultato = new List<DtoSala>();

        foreach (var sala in tutteLeSale)
        {
            if (sala.TipologiaSalaId != tipologiaId)
                continue;

            TipologiaSala? tipologia =
                await _contesto.TipologieSala.FindAsync(sala.TipologiaSalaId);

            risultato.Add(new DtoSala
            {
                Id = sala.Id,
                Nome = sala.Nome,
                Capienza = sala.Capienza,
                TipologiaSalaId = sala.TipologiaSalaId,
                NomeTipologia = tipologia?.Nome ?? ""
            });
        }

        return risultato;
    }

    public async Task<string?> CreazioneAsync(DtoCreazioneSala dto)
    {
        Sala sala = new Sala
        {
            Nome = dto.Nome,
            Capienza = dto.Capienza,
            TipologiaSalaId = dto.TipologiaSalaId
        };

        _contesto.Sale.Add(sala);
        await _contesto.SaveChangesAsync();

        return "Sala creata con successo."; //ritorno modificato, ora ritorna una stringa messaggio
    }

    public async Task<string?> ModificaAsync(string id, DtoCreazioneSala dto)
    {
        Sala? salaEsistente = await _contesto.Sale.FindAsync(id);

        if (salaEsistente == null)
            throw new ItemNotFoundException("Sala");

        TipologiaSala? tipologia =
            await _contesto.TipologieSala.FindAsync(dto.TipologiaSalaId);

        if (tipologia == null)
            throw new NotFoundException("TipologiaSala", dto.TipologiaSalaId);

        List<Sala> listaSale = await _contesto.Sale.ToListAsync();

        foreach (var salaCorrente in listaSale)
        {
            if (string.Equals(salaCorrente.Nome, dto.Nome, StringComparison.OrdinalIgnoreCase))
                throw new ModificaException("sala");
        }

        salaEsistente.Nome = dto.Nome;
        salaEsistente.Capienza = dto.Capienza;
        salaEsistente.TipologiaSalaId = dto.TipologiaSalaId;

        await _contesto.SaveChangesAsync();

        return "Sala modificata con successo.";
    }


    public async Task<bool> EliminaAsync(string id)
    {
        Sala? sala = await _contesto.Sale.FindAsync(id);

        if (sala == null)
            return false;

        _contesto.Sale.Remove(sala);
        await _contesto.SaveChangesAsync();

        return true;
    }
}
```

## TipologiaSalaService.cs

```c#
using Microsoft.EntityFrameworkCore;
using NuovoCinemaParadiso.Dtos;
using NuovoCinemaParadiso.Data;
using NuovoCinemaParadiso.Models;

namespace NuovoCinemaParadiso.Services;

/// <summary>
/// Servizio che gestisce le operazioni CRUD per le tipologie di sala.
/// </summary>
public class TipologiaSalaService
{
    private readonly ContestoDb _contesto;

    /// <summary>
    /// Inizializza una nuova istanza del servizio TipologiaSalaService.
    /// </summary>
    /// <param name="contesto">Contesto del database EF Core.</param>
    public TipologiaSalaService(ContestoDb contesto)
    {
        _contesto = contesto;
    }

    /// <summary>
    /// Recupera tutte le tipologie di sala presenti nel sistema.
    /// </summary>
    /// <returns>Lista di DTO delle tipologie di sala.</returns>
    public async Task<List<DtoTipologiaSala>> OttieniTuttoAsync()
    {
        List<TipologiaSala> tipologieSala = await _contesto.TipologieSala.ToListAsync();

        List<DtoTipologiaSala> risultati = new List<DtoTipologiaSala>();

        foreach (var tipologiaSala in tipologieSala)
        {
            risultati.Add(new DtoTipologiaSala
            {
                Id = tipologiaSala.Id,
                Nome = tipologiaSala.Nome,
                MaggiorazionePrezzo = tipologiaSala.MaggiorazionePrezzo
            });
        }

        return risultati;
    }

    /// <summary>
    /// Recupera una tipologia di sala tramite ID.
    /// </summary>
    /// <param name="id">ID della tipologia sala.</param>
    /// <returns>DTO della tipologia oppure null se non trovata.</returns>
    public async Task<DtoTipologiaSala?> OttieniTramiteIdAsync(string id)
    {
        TipologiaSala? tipologiaSala = await _contesto.TipologieSala.FindAsync(id);

        if (tipologiaSala == null)
            return null;

        return new DtoTipologiaSala
        {
            Id = tipologiaSala.Id,
            Nome = tipologiaSala.Nome,
            MaggiorazionePrezzo = tipologiaSala.MaggiorazionePrezzo
        };
    }

    /// <summary>
    /// Crea una nuova tipologia di sala.
    /// </summary>
    /// <param name="dto">Dati della tipologia da creare.</param>
    /// <returns>DTO della tipologia creata.</returns>
    public async Task<DtoTipologiaSala> CreazioneAsync(DtoCreazioneTipologiaSala dto)
    {
        TipologiaSala tipologiaSala = new TipologiaSala
        {
            Nome = dto.Nome,
            MaggiorazionePrezzo = dto.MaggiorazionePrezzo
        };

        _contesto.TipologieSala.Add(tipologiaSala);
        await _contesto.SaveChangesAsync();

        return new DtoTipologiaSala
        {
            Id = tipologiaSala.Id,
            Nome = tipologiaSala.Nome,
            MaggiorazionePrezzo = tipologiaSala.MaggiorazionePrezzo
        };
    }

    /// <summary>
    /// Modifica una tipologia di sala esistente.
    /// </summary>
    /// <param name="id">ID della tipologia da modificare.</param>
    /// <param name="dto">Nuovi dati della tipologia.</param>
    /// <returns>DTO aggiornato oppure null se non trovato.</returns>
    public async Task<DtoTipologiaSala?> ModificaAsync(string id, DtoCreazioneTipologiaSala dto)
    {
        TipologiaSala? tipologiaSala = await _contesto.TipologieSala.FindAsync(id);

        if (tipologiaSala == null)
            return null;

        tipologiaSala.Nome = dto.Nome;
        tipologiaSala.MaggiorazionePrezzo = dto.MaggiorazionePrezzo;

        await _contesto.SaveChangesAsync();

        return new DtoTipologiaSala
        {
            Id = tipologiaSala.Id,
            Nome = tipologiaSala.Nome,
            MaggiorazionePrezzo = tipologiaSala.MaggiorazionePrezzo
        };
    }

    /// <summary>
    /// Elimina una tipologia di sala.
    /// </summary>
    /// <param name="id">ID della tipologia da eliminare.</param>
    /// <returns>True se eliminata, false se non trovata.</returns>
    public async Task<bool> EliminaAsync(string id)
    {
        TipologiaSala? tipologiaSala = await _contesto.TipologieSala.FindAsync(id);

        if (tipologiaSala == null)
            return false;

        _contesto.TipologieSala.Remove(tipologiaSala);
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

namespace NuovoCinemaParadiso.Services;

/// <summary>
/// Servizio che gestisce le operazioni CRUD relative ai turni di programmazione.
/// Un turno rappresenta una fascia oraria di proiezione.
/// </summary>
public class TurnoService
{
    private readonly ContestoDb _contesto;

    /// <summary>
    /// Inizializza una nuova istanza del servizio TurnoService.
    /// </summary>
    /// <param name="contesto">Contesto del database EF Core.</param>
    public TurnoService(ContestoDb contesto)
    {
        _contesto = contesto;
    }

    /// <summary>
    /// Restituisce tutti i turni presenti nel sistema.
    /// </summary>
    /// <returns>Lista di DTO dei turni.</returns>
    public async Task<List<DtoTurno>> OttieniTuttoAsync()
    {
        List<Turno> turni = await _contesto.Turni.ToListAsync();
        List<DtoTurno> risultato = new List<DtoTurno>();

        foreach (var turnoCorrente in turni)
        {
            risultato.Add(new DtoTurno
            {
                Id = turnoCorrente.Id,
                OraInizio = turnoCorrente.OraInizio,
                OraFine = turnoCorrente.OraFine,
                Nome = turnoCorrente.Nome
            });
        }

        return risultato;
    }

    /// <summary>
    /// Recupera un turno tramite ID.
    /// </summary>
    /// <param name="id">ID del turno.</param>
    /// <returns>DTO del turno oppure null se non trovato.</returns>
    public async Task<DtoTurno?> OttieniTramiteIdAsync(string id)
    {
        Turno? turno = await _contesto.Turni.FindAsync(id);

        if (turno == null)
            return null;

        return new DtoTurno
        {
            Id = turno.Id,
            Nome = turno.Nome,
            OraInizio = turno.OraInizio,
            OraFine = turno.OraFine
        };
    }

    /// <summary>
    /// Crea un nuovo turno.
    /// </summary>
    /// <param name="dto">Dati del turno da creare.</param>
    /// <returns>DTO del turno creato oppure errore se duplicato.</returns>
    public async Task<(DtoTurno? Dto, string? Errore)> CreazioneAsync(DtoCreazioneTurno dto)
    {
        bool turnoGiaPresente =
            await _contesto.Turni.AnyAsync(t => t.Nome.ToLower() == dto.Nome.ToLower());

        if (turnoGiaPresente)
            return (null, "Turno già presente con questo nome.");

        Turno turno = new Turno
        {
            Nome = dto.Nome,
            OraInizio = dto.OraInizio,
            OraFine = dto.OraFine
        };

        _contesto.Turni.Add(turno);
        await _contesto.SaveChangesAsync();

        return (new DtoTurno
        {
            Id = turno.Id,
            Nome = turno.Nome,
            OraInizio = turno.OraInizio,
            OraFine = turno.OraFine
        }, null);
    }

    /// <summary>
    /// Modifica un turno esistente.
    /// </summary>
    /// <param name="id">ID del turno da modificare.</param>
    /// <param name="dto">Nuovi dati del turno.</param>
    /// <returns>DTO aggiornato oppure errore.</returns>
    public async Task<(DtoTurno? Dto, string? Errore)> ModificaAsync(string id, DtoCreazioneTurno dto)
    {
        Turno? turnoEsistente = await _contesto.Turni.FindAsync(id);

        if (turnoEsistente == null)
            return (null, "Turno non trovato.");

        if (!turnoEsistente.Nome.Equals(dto.Nome, StringComparison.OrdinalIgnoreCase))
        {
            bool nomeGiaUsato =
                await _contesto.Turni.AnyAsync(t =>
                    t.Nome.ToLower() == dto.Nome.ToLower() && t.Id != id);

            if (nomeGiaUsato)
                return (null, "Esiste già un altro turno con questo nome.");
        }

        turnoEsistente.Nome = dto.Nome;
        turnoEsistente.OraInizio = dto.OraInizio;
        turnoEsistente.OraFine = dto.OraFine;

        await _contesto.SaveChangesAsync();

        return (new DtoTurno
        {
            Id = turnoEsistente.Id,
            Nome = turnoEsistente.Nome,
            OraInizio = turnoEsistente.OraInizio,
            OraFine = turnoEsistente.OraFine
        }, null);
    }

    /// <summary>
    /// Elimina un turno se non ha proiezioni collegate.
    /// </summary>
    /// <param name="id">ID del turno da eliminare.</param>
    /// <returns>True se eliminato, altrimenti false con messaggio errore.</returns>
    public async Task<(bool Successo, string? Errore)> EliminaAsync(string id)
    {
        Turno? turno = await _contesto.Turni.FindAsync(id);

        if (turno == null)
            return (false, "Turno non trovato.");

        bool haProiezioniCollegate =
            await _contesto.Proiezioni.AnyAsync(p => p.TurnoId == id);

        if (haProiezioniCollegate)
            return (false, "Impossibile eliminare il turno: ci sono proiezioni collegate.");

        _contesto.Turni.Remove(turno);
        await _contesto.SaveChangesAsync();

        return (true, null);
    }
}
```

## UtenteService.cs V1.0

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

public class UtenteService
{
    // Riferimento al database dell'applicazione
    private readonly ContestoDb _contesto;

    // Gestore utenti di Identity, usato per trovare e aggiornare gli utenti
    private readonly UserManager<Utente> _gestioneUtenti;

    // Costruttore: riceve database e gestore utenti tramite Dependency Injection
    public UtenteService(ContestoDb contestoDb, UserManager<Utente> gestioneUtenti)
    {
        _contesto = contestoDb;
        _gestioneUtenti = gestioneUtenti;
    }

    // Metodo che recupera tutti i biglietti appartenenti a un singolo utente
    public async Task<List<DtoBiglietto>> OttieniTuttiBigliettiAsync(string utenteId)
    {
        // Recupera tutti i biglietti dal database
        var biglietti = await _contesto.Biglietti.ToListAsync();

        // Lista finale che conterrà solo i biglietti dell'utente
        List<DtoBiglietto> listaBiglietti = new List<DtoBiglietto>();

        // Ciclo manuale su tutti i biglietti
        for (int i = 0; i < biglietti.Count; i++)
        {
            Biglietto bigliettoCorrente = biglietti[i];

            // Se il biglietto appartiene all'utente richiesto
            if (bigliettoCorrente.UtenteId == utenteId)
            {
                // Recupera la proiezione collegata al biglietto
                Proiezione? proiezione = await _contesto.Proiezioni.FindAsync(bigliettoCorrente.ProiezioneId);

                // Recupera il film della proiezione
                Movie? movie = await _contesto.Movies.FindAsync(proiezione.MovieId);

                // Recupera la sala della proiezione
                Sala? sala = await _contesto.Sale.FindAsync(proiezione.SalaId);

                // Recupera la tipologia della sala (per calcolare il prezzo)
                TipologiaSala? tipologiaSala = await _contesto.TipologieSala.FindAsync(sala.TipologiaSalaId);

                // Crea il DTO del biglietto con tutte le informazioni necessarie
                DtoBiglietto dto = new DtoBiglietto
                {
                    Id = bigliettoCorrente.Id,
                    UtenteId = bigliettoCorrente.UtenteId,
                    ProiezioneId = bigliettoCorrente.ProiezioneId,
                    OrarioCreazione = bigliettoCorrente.OrarioCreazione,
                    NumeroBiglietti = bigliettoCorrente.NumeroBiglietti,

                    // Calcolo del prezzo finale usando il metodo helper
                    PrezzoFinale = Calcoli.CalcolaPrezzoFinale(
                        movie.PrezzoMovie,
                        tipologiaSala.MaggiorazionePrezzo,
                        bigliettoCorrente.NumeroBiglietti,
                        (await _gestioneUtenti.FindByIdAsync(utenteId)).Abbonamento,
                        (await _gestioneUtenti.FindByIdAsync(utenteId)).DataInizioAbbonamento
                    )
                };

                // Aggiunge il DTO alla lista finale
                listaBiglietti.Add(dto);
            }
        }

        // Restituisce tutti i biglietti dell'utente
        return listaBiglietti;
    }

    // Metodo che permette a un utente di attivare un abbonamento
    public async Task<(bool Successo, string Messaggio)> AbbonatiAsync(string abbonamentoId, string utenteId)
    {
        // Recupera tutti gli abbonamenti dal database
        List<Abbonamento> abbonamenti = await _contesto.Abbonamenti.ToListAsync();
        Abbonamento? abbonamentoTrovato = null;

        // Cerca manualmente l'abbonamento richiesto
        for (int i = 0; i < abbonamenti.Count; i++)
        {
            if (abbonamenti[i].Id == abbonamentoId)
            {
                abbonamentoTrovato = abbonamenti[i];
                break;
            }
        }

        // Se non esiste → errore
        if (abbonamentoTrovato == null)
            return (false, "Abbonamento non trovato.");

        // Recupera l'utente che vuole abbonarsi
        Utente? utenteTrovato = await _gestioneUtenti.FindByIdAsync(utenteId);
        if (utenteTrovato == null)
            return (false, "Utente non trovato.");

        // Se è già abbonato → non può abbonarsi di nuovo
        if (utenteTrovato.SeAbbonato)
            return (false, "L'utente è già abbonato.");

        // Controlla che l'utente abbia abbastanza saldo
        if (utenteTrovato.Saldo < abbonamentoTrovato.Prezzo)
            return (false, "Credito insufficiente per abbonarsi.");

        // Scala il prezzo dal saldo dell'utente
        utenteTrovato.Saldo -= abbonamentoTrovato.Prezzo;

        // Imposta i dati dell'abbonamento
        utenteTrovato.AbbonamentoId = abbonamentoTrovato.Id;
        utenteTrovato.SeAbbonato = true;
        utenteTrovato.DataInizioAbbonamento = DateTimeOffset.UtcNow;

        // Salva le modifiche nel database
        await _contesto.SaveChangesAsync();

        return (true, "Abbonamento attivato correttamente.");
    }

    // Metodo che permette di ricaricare una gift card
    public async Task<(bool Successo, string Messaggio)> RicaricaGiftCardAsync(string utenteId, DtoRicaricaGiftCard dto)
    {
        // Recupera l'utente che vuole ricaricare la gift card
        Utente? utenteCorrente = await _gestioneUtenti.FindByIdAsync(utenteId);

        if (utenteCorrente == null)
            return (false, "Utente non trovato.");

        // Controlla che l'importo sia valido
        if (dto.Importo <= 0)
            return (false, "Importo non valido.");

        // Controlla che l'utente abbia abbastanza saldo
        if (utenteCorrente.Saldo < dto.Importo)
            return (false, "Saldo insufficiente per ricaricare la gift card.");

        // Scala l'importo dal saldo dell'utente
        utenteCorrente.Saldo -= dto.Importo;

        // Crea una nuova gift card con un codice generato automaticamente
        GiftCard nuovaGiftCard = new GiftCard
        {
            Nome = "GiftCard",
            Valore = dto.Importo,
            CodiceRiscatto = GiftCardHelper.GeneraCodice()
        };

        // Aggiunge la gift card al database
        await _contesto.GiftCards.AddAsync(nuovaGiftCard);
        await _contesto.SaveChangesAsync();

        return (true, "Gift card creata correttamente.");
    }

    // Metodo che permette di riscattare una gift card tramite codice
    public async Task<(bool Successo, string Messaggio)> RiscattaGiftCardAsync(DtoCodiceRiscatto dto, string utenteId)
    {
        // Recupera l'utente che vuole riscattare la gift card
        Utente? utenteCorrente = await _gestioneUtenti.FindByIdAsync(utenteId);

        if (utenteCorrente == null)
            return (false, "Utente non trovato.");

        // Recupera tutte le gift card dal database
        List<GiftCard> giftCards = await _contesto.GiftCards.ToListAsync();
        GiftCard? giftCardTrovata = null;

        // Cerca manualmente la gift card tramite codice
        for (int i = 0; i < giftCards.Count; i++)
        {
            if (giftCards[i].CodiceRiscatto == dto.CodiceRiscatto)
            {
                giftCardTrovata = giftCards[i];
                break;
            }
        }

        // Se non esiste → errore
        if (giftCardTrovata == null)
            return (false, "Codice riscatto non valido.");

        // Se è già stata usata → errore
        if (giftCardTrovata.Riscattata)
            return (false, "Il codice riscatto è già stato utilizzato.");

        // Aggiunge il valore della gift card al saldo dell'utente
        utenteCorrente.Saldo += giftCardTrovata.Valore;

        // Segna la gift card come riscattata
        giftCardTrovata.Riscattata = true;

        // Salva le modifiche
        await _contesto.SaveChangesAsync();

        return (true, "Gift card riscattata correttamente.");
    }
}

```

## UtenteService.cs V1.1

Andrea Bruno 22-05-2026
Aggiunta query turno
Aggiunti campi nel DtoBiglietto di ritorno

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

public class UtenteService
{
    private readonly ContestoDb _contesto;
    private readonly UserManager<Utente> _gestioneUtenti;

    public UtenteService(ContestoDb contestoDb, UserManager<Utente> gestioneUtenti)
    {
        _contesto = contestoDb;
        _gestioneUtenti = gestioneUtenti;
    }

    public async Task<List<DtoBiglietto>> OttieniTuttiBigliettiAsync(string utenteId)
    {
        var biglietti = await _contesto.Biglietti.ToListAsync();
        List<DtoBiglietto> listaBiglietti = new List<DtoBiglietto>();

        for (int i = 0; i < biglietti.Count; i++)
        {
            Biglietto bigliettoCorrente = biglietti[i];
            if (bigliettoCorrente.UtenteId == utenteId)
            {
                Proiezione? proiezione = await _contesto.Proiezioni.FindAsync(bigliettoCorrente.ProiezioneId);
                Movie? movie = await _contesto.Movies.FindAsync(proiezione.MovieId);
                Sala? sala = await _contesto.Sale.FindAsync(proiezione.SalaId);
                TipologiaSala? tipologiaSala = await _contesto.TipologieSala.FindAsync(sala.TipologiaSalaId);
                // Recupera il turno associato alla proiezione.
                // Se non esiste, lancia una NotFoundException specifica con nome entità e ID.
                // Questo mantiene il flusso null-safe e fornisce un errore REST chiaro.
                Turno? turno = await _contesto.Turni.FindAsync(proiezione.TurnoId)
                    ?? throw new NotFoundException("Turno", proiezione.TurnoId);

                DtoBiglietto dto = new DtoBiglietto
                {
                    Id = bigliettoCorrente.Id,
                    UtenteId = bigliettoCorrente.UtenteId,
                    ProiezioneId = bigliettoCorrente.ProiezioneId,
                    OrarioCreazione = bigliettoCorrente.OrarioCreazione,
                    NumeroBiglietti = bigliettoCorrente.NumeroBiglietti,
                    NomeSala = sala.Nome,
                    TitoloMovie = movie.Titolo,
                    NomeTipologiaSala = tipologiaSala.Nome,
                    OraInizio = turno.OraInizio,
                    DataProiezione = proiezione.DataProiezione,
                    PrezzoFinale = Calcoli.CalcolaPrezzoFinale(movie.PrezzoMovie, tipologiaSala.MaggiorazionePrezzo, bigliettoCorrente.NumeroBiglietti, (await _gestioneUtenti.FindByIdAsync(utenteId)).Abbonamento, (await _gestioneUtenti.FindByIdAsync(utenteId)).DataInizioAbbonamento)
                };
                listaBiglietti.Add(dto);
            }
        }
        return listaBiglietti;
    }

    public async Task<(bool Successo, string Messaggio)> AbbonatiAsync(string abbonamentoId, string utenteId)
    {
        List<Abbonamento> abbonamenti = await _contesto.Abbonamenti.ToListAsync();
        Abbonamento? abbonamentoTrovato = null;

        for (int i = 0; i < abbonamenti.Count; i++)
        {
            if (abbonamenti[i].Id == abbonamentoId)
            {
                abbonamentoTrovato = abbonamenti[i];
                break;
            }
        }

        if (abbonamentoTrovato == null)
            return (false, "Abbonamento non trovato.");

        Utente? utenteTrovato = await _gestioneUtenti.FindByIdAsync(utenteId);
        if (utenteTrovato == null)
            return (false, "Utente non trovato.");

        if (utenteTrovato.SeAbbonato)
            return (false, "L'utente è già abbonato.");

        if (utenteTrovato.Saldo < abbonamentoTrovato.Prezzo)
            return (false, "Credito insufficiente per abbonarsi.");

        utenteTrovato.Saldo -= abbonamentoTrovato.Prezzo;
        utenteTrovato.AbbonamentoId = abbonamentoTrovato.Id;
        utenteTrovato.SeAbbonato = true;
        utenteTrovato.DataInizioAbbonamento = DateTimeOffset.UtcNow;

        await _contesto.SaveChangesAsync();

        return (true, "Abbonamento attivato correttamente.");
    }


    public async Task<(bool Successo, string Messaggio)> RicaricaGiftCardAsync(string utenteId, DtoRicaricaGiftCard dto)
    {
        Utente? utenteCorrente = await _gestioneUtenti.FindByIdAsync(utenteId);

        if (utenteCorrente == null)
            return (false, "Utente non trovato.");

        if (dto.Importo <= 0)
            return (false, "Importo non valido.");

        if (utenteCorrente.Saldo < dto.Importo)
            return (false, "Saldo insufficiente per ricaricare la gift card.");

        utenteCorrente.Saldo -= dto.Importo;

        GiftCard nuovaGiftCard = new GiftCard
        {
            Nome = "GiftCard",
            Valore = dto.Importo,
            CodiceRiscatto = GiftCardHelper.GeneraCodice()
        };

        await _contesto.GiftCards.AddAsync(nuovaGiftCard);
        await _contesto.SaveChangesAsync();

        return (true, "Gift card creata correttamente.");
    }

    public async Task<(bool Successo, string Messaggio)> RiscattaGiftCardAsync(DtoCodiceRiscatto dto, string utenteId)
    {
        Utente? utenteCorrente = await _gestioneUtenti.FindByIdAsync(utenteId);

        if (utenteCorrente == null)
            return (false, "Utente non trovato.");

        List<GiftCard> giftCards = await _contesto.GiftCards.ToListAsync();
        GiftCard? giftCardTrovata = null;

        for (int i = 0; i < giftCards.Count; i++)
        {
            if (giftCards[i].CodiceRiscatto == dto.CodiceRiscatto)
            {
                giftCardTrovata = giftCards[i];
                break;
            }
        }

        if (giftCardTrovata == null)
            return (false, "Codice riscatto non valido.");

        if (giftCardTrovata.Riscattata)
            return (false, "Il codice riscatto è già stato utilizzato.");

        utenteCorrente.Saldo += giftCardTrovata.Valore;
        giftCardTrovata.Riscattata = true;

        await _contesto.SaveChangesAsync();

        return (true, "Gift card riscattata correttamente.");
    }
}
```
# Helpers (CalcoliHelper da aggiungere dopo il merge)

## CalcoliHelper.cs (Da aggiungere dopo il merge)

```c#

```

## GiftCard.Helper.cs

```c#

namespace NuovoCinemaParadiso.Helpers;

public static class GiftCardHelper
{
    public static string GeneraCodice()
    {
        return Guid.NewGuid()
        .ToString()
        .Substring(0, 8)
        .ToUpper();
    }
}
```

## JWTHelper.cs 

```c#
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using NuovoCinemaParadiso.Models;

namespace NuovoCinemaParadiso.Helpers;

/// <summary>
/// Helper per la generazione di token JWT per l'autenticazione degli utenti.
/// Gestisce la creazione del token includendo claims utente e ruoli.
/// </summary>
public class JwtHelper
{
    private readonly IConfiguration _configurazione;

    /// <summary>
    /// Inizializza una nuova istanza di JwtHelper con la configurazione dell'applicazione.
    /// </summary>
    /// <param name="configurazione">Configurazione contenente chiave, issuer e audience del JWT.</param>
    public JwtHelper(IConfiguration configurazione) 
    {
        _configurazione = configurazione; 
    }

    /// <summary>
    /// Genera un token JWT per un utente autenticato includendo i suoi ruoli.
    /// </summary>
    /// <param name="utente">Utente per il quale generare il token.</param>
    /// <param name="ruoli">Lista dei ruoli associati all'utente.</param>
    /// <returns>Token JWT serializzato come stringa.</returns>
    /// <exception cref="Exception">
    /// Viene lanciata se la configurazione JWT (Key, Issuer, Audience) non è presente.
    /// </exception>
    public string GenerateToken(Utente utente, IList<string> ruoli) 
    {
        string? key = _configurazione["Jwt:Key"];
        string? issuer = _configurazione["Jwt:Issuer"]; 
        string? audience = _configurazione["Jwt:Audience"];  

        if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(issuer) || string.IsNullOrEmpty(audience))
        {
            throw new Exception("Configurazione JWT mancante.");
        }

        List<Claim> claims = new List<Claim>();

        // Claim base dell'utente
        claims.Add(new Claim(ClaimTypes.NameIdentifier, utente.Id));
        claims.Add(new Claim(ClaimTypes.Name, utente.UserName ?? ""));
        claims.Add(new Claim(ClaimTypes.Email, utente.Email ?? ""));

        // Claim dei ruoli
        for (int i = 0; i < ruoli.Count; i++)
        {
            claims.Add(new Claim(ClaimTypes.Role, ruoli[i]));
        }

        // Chiave di firma simmetrica
        SymmetricSecurityKey securityKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(key)
        );

        // Credenziali di firma del token
        SigningCredentials credentials = new SigningCredentials(
            securityKey,
            SecurityAlgorithms.HmacSha256
        );

        // Creazione del token JWT
        JwtSecurityToken token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: credentials
        );

        // Serializzazione del token in stringa
        return new JwtSecurityTokenHandler().WriteToken(token);
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

/// <summary>
/// Seeder iniziale del database.
/// Si occupa di creare dati base (ruoli, utenti, film, sale, proiezioni, ecc.)
/// se non esistono già.
/// </summary>
public static class DataSeeder
{
    /// <summary>
    /// Esegue il seed iniziale dell'applicazione.
    /// Crea ruoli, utenti demo e tutti i dati fondamentali del cinema.
    /// </summary>
    /// <param name="serviceProvider">Service provider per risolvere le dipendenze.</param>
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        using IServiceScope scope = serviceProvider.CreateScope();

        DateTime oggi = DateTime.Today;

        // Recupero dipendenze dal container DI
        ContestoDb contestoDb = scope.ServiceProvider.GetRequiredService<ContestoDb>();
        UserManager<Utente> gestioneUtenti = scope.ServiceProvider.GetRequiredService<UserManager<Utente>>();
        RoleManager<IdentityRole> gestioneRuoli = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        // =========================
        // CREAZIONE RUOLI BASE
        // =========================
        await AssicuraEsistenzaRuoloAsync(gestioneRuoli, Ruoli.Gestore);
        await AssicuraEsistenzaRuoloAsync(gestioneRuoli, Ruoli.Operatore);
        await AssicuraEsistenzaRuoloAsync(gestioneRuoli, Ruoli.Utente);

        // =========================
        // CREAZIONE UTENTI DEMO
        // =========================
        Utente gestore = await AssicuraEsistenzaUtenteAsync(gestioneUtenti, "gestore@gmail.com", "123456", "Gestore", 60, false);
        Utente operatore = await AssicuraEsistenzaUtenteAsync(gestioneUtenti, "operatore@gmail.com", "123456", "Operatore", 35, false);
        Utente utente = await AssicuraEsistenzaUtenteAsync(gestioneUtenti, "utente1@gmail.com", "123456", "Utente Uno", 15, false);

        // =========================
        // CONTO CINEMA INIZIALE
        // =========================
        ContoCinema contoCinema = await AssicuraEsistenzaConto(contestoDb, "IT60X0542811101000000123456", "Gestore", 200);

        // =========================
        // ASSEGNAZIONE RUOLI
        // =========================
        await ImpostaRuoloUnicoAsync(gestioneUtenti, gestore, Ruoli.Gestore);
        await ImpostaRuoloUnicoAsync(gestioneUtenti, operatore, Ruoli.Operatore);
        await ImpostaRuoloUnicoAsync(gestioneUtenti, utente, Ruoli.Utente);

        // =========================
        // GENERI FILM
        // =========================
        var genereAzione = await AssicuraEsistenzaGenereMovie(contestoDb, "Azione");
        var genereHorror = await AssicuraEsistenzaGenereMovie(contestoDb, "Horror");
        var genereCommedia = await AssicuraEsistenzaGenereMovie(contestoDb, "Commedia");

        // =========================
        // FILM DEMO
        // =========================
        var movie1 = await AssicuraEsistenzaMovie(contestoDb, "Movie1", "Film del drago", 60, 10, genereAzione.Id);
        var movie2 = await AssicuraEsistenzaMovie(contestoDb, "Movie2", "Film del lupo", 80, 12, genereHorror.Id);
        var movie3 = await AssicuraEsistenzaMovie(contestoDb, "Movie3", "Film del cane", 100, 14, genereCommedia.Id);

        // =========================
        // TIPI SALA
        // =========================
        var tipologia2D = await AssicuraEsistenzaTipologiaSala(contestoDb, "2D", 2);
        var tipologia3D = await AssicuraEsistenzaTipologiaSala(contestoDb, "3D", 3);

        // =========================
        // TURNI
        // =========================
        var turnoMattina = await AssicuraEsistenzaTurno(contestoDb, new TimeOnly(10, 0), new TimeOnly(13, 0), "Mattina");

        // =========================
        // SALE
        // =========================
        var sala1 = await AssicuraEsistenzaSala(contestoDb, "Sala1", 30, tipologia2D.Id);

        // =========================
        // ABBONAMENTI
        // =========================
        await AssicuraEsistenzaAbbonamento(contestoDb, "Mensile", 70, 25, 1);

        // =========================
        // PROIEZIONI DEMO
        // =========================
        var proiezione1 = await AssicuraEsistenzaProiezione(contestoDb, new DateOnly(2027, 1, 1), movie1.Id, sala1.Id, turnoMattina.Id);

        // =========================
        // BIGLIETTI DEMO
        // =========================
        await AssicuraEsistenzaBiglietto(contestoDb, proiezione1.Id, utente.Id, 1, DateTimeOffset.Now, 10, "standard");

        // =========================
        // CONTO CINEMA (backup)
        // =========================
        await AssicuraEsistenzaContoCinema(contestoDb, "IT60X0542811101000000123456", "Gestore", 200);
    }

    /// <summary>
    /// Crea un ruolo se non esiste già nel sistema Identity.
    /// </summary>
    private static async Task AssicuraEsistenzaRuoloAsync(RoleManager<IdentityRole> managerRuolo, string nomeRuolo)
    {
        bool esiste = await managerRuolo.RoleExistsAsync(nomeRuolo);
        if (!esiste)
        {
            IdentityRole ruolo = new IdentityRole
            {
                Name = nomeRuolo
            };

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
        {
            return utenteEsistente;
        }

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

    /// <summary>
    /// Crea il conto cinema iniziale se non esiste.
    /// </summary>
    private static async Task<ContoCinema> AssicuraEsistenzaConto(ContestoDb context, string iban, string titolareConto, int conto)
    {
        ContoCinema contoEsistente = await context.ContoCinema.FirstOrDefaultAsync();

        if (contoEsistente != null)
        {
            return null;
        }

        ContoCinema nuovoContoCinema = new ContoCinema
        {
            Iban = iban,
            TitolareConto = titolareConto,
            Conto = conto
        };

        context.ContoCinema.Add(nuovoContoCinema);
        await context.SaveChangesAsync();

        return nuovoContoCinema;
    }

    /// <summary>
    /// Imposta un solo ruolo valido per utente rimuovendo eventuali altri ruoli.
    /// </summary>
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

    /// <summary>
    /// Crea un genere film se non esiste già.
    /// </summary>
    private static async Task<GenereMovie> AssicuraEsistenzaGenereMovie(ContestoDb context, string genere)
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

    /// <summary>
    /// Crea un film se non esiste già.
    /// </summary>
    private static async Task<Movie> AssicuraEsistenzaMovie(
        ContestoDb context,
        string titolo,
        string descrizione,
        int durataMinuti,
        int prezzoMovie,
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

    /// <summary>
    /// Crea una tipologia di sala se non esiste già.
    /// </summary>
    private static async Task<TipologiaSala> AssicuraEsistenzaTipologiaSala(
        ContestoDb context,
        string nome,
        int maggiorazioneprezzo)
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

    /// <summary>
    /// Crea una sala se non esiste già.
    /// </summary>
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

    /// <summary>
    /// Crea un turno se non esiste già.
    /// </summary>
    private static async Task<Turno> AssicuraEsistenzaTurno(
        ContestoDb context,
        TimeOnly oraInizio,
        TimeOnly oraFine,
        string nome)
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

    /// <summary>
    /// Crea un abbonamento se non esiste già.
    /// </summary>
    private static async Task AssicuraEsistenzaAbbonamento(ContestoDb context, string nome, int prezzo, int sconto, int durata)
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

    /// <summary>
    /// Crea una proiezione se non esiste già.
    /// </summary>
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
            Attivo = true
        };

        context.Proiezioni.Add(nuovaProiezione);
        await context.SaveChangesAsync();

        return nuovaProiezione;
    }

    /// <summary>
    /// Crea un biglietto se non esiste già.
    /// </summary>
    private static async Task AssicuraEsistenzaBiglietto(
        ContestoDb context,
        string proiezioneId,
        string utenteId,
        int numeroBiglietti,
        DateTimeOffset orarioCreazione,
        int prezzoFinale,
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

    /// <summary>
    /// Crea il conto cinema se non esiste già.
    /// </summary>
    private static async Task AssicuraEsistenzaContoCinema(
        ContestoDb context,
        string iban,
        string titolareConto,
        int conto)
    {
        List<ContoCinema> contiCinema = await context.ContoCinema.ToListAsync();

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

/// <summary>
/// Entry point dell'applicazione ASP.NET Core.
/// Configura servizi, autenticazione, database, dependency injection e pipeline HTTP.
/// </summary>
var builder = WebApplication.CreateBuilder(args); 

// ==========================
// REGISTRAZIONE CONTROLLERS
// ==========================
builder.Services.AddControllers();

/// <summary>
/// Configurazione del database SQLite tramite Entity Framework Core.
/// </summary>
builder.Services.AddDbContext<ContestoDb>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});

/// <summary>
/// Configurazione Identity per gestione utenti e ruoli.
/// </summary>
builder.Services.AddIdentityCore<Utente>(options =>
{
    options.Password.RequireDigit = false; 
    options.Password.RequireLowercase = false; 
    options.Password.RequireUppercase = false; 
    options.Password.RequireNonAlphanumeric = false; 
    options.Password.RequiredLength = 6; 
})
.AddRoles<IdentityRole>() // supporto ai ruoli
.AddSignInManager<SignInManager<Utente>>()
.AddEntityFrameworkStores<ContestoDb>()
.AddDefaultTokenProviders();

/// <summary>
/// Lettura configurazione JWT dal file appsettings.json.
/// </summary>
string? jwtKey = builder.Configuration["Jwt:Key"];
string? jwtIssuer = builder.Configuration["Jwt:Issuer"];
string? jwtAudience = builder.Configuration["Jwt:Audience"];

/// <summary>
/// Validazione configurazione JWT obbligatoria.
/// </summary>
if (string.IsNullOrWhiteSpace(jwtKey) ||
    string.IsNullOrWhiteSpace(jwtIssuer) ||
    string.IsNullOrWhiteSpace(jwtAudience))
{
    throw new Exception("Configurazione JWT mancante in appsettings.json");
}

/// <summary>
/// Configurazione autenticazione JWT Bearer.
/// </summary>
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

/// <summary>
/// Abilitazione autorizzazione globale.
/// </summary>
builder.Services.AddAuthorization();

/// <summary>
/// Configurazione CORS per consentire chiamate dal frontend Angular.
/// </summary>
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

/// <summary>
/// Dependency Injection dei servizi applicativi.
/// </summary>
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<GenereMovieService>();
builder.Services.AddScoped<TipologiaSalaService>();
builder.Services.AddScoped<TurnoService>();
builder.Services.AddScoped<SalaService>();
builder.Services.AddScoped<MovieService>();
builder.Services.AddScoped<BigliettoService>();
builder.Services.AddScoped<RuoloUtenteService>();
builder.Services.AddScoped<JwtHelper>();
builder.Services.AddScoped<LogAzioniService>();
builder.Services.AddScoped<UtenteService>();
builder.Services.AddScoped<GestoreService>();
builder.Services.AddScoped<AbbonamentoService>();
builder.Services.AddScoped<ProiezioneService>();
builder.Services.AddScoped<GiftCardService>();
builder.Services.AddScoped<OperatoreService>();
builder.Services.AddScoped<ContoCinemaService>();

/// <summary>
/// Costruzione dell'applicazione.
/// </summary>
var app = builder.Build();

/// <summary>
/// Abilitazione policy CORS.
/// </summary>
app.UseCors("AllowAngularApp");

/// <summary>
/// Redirect HTTPS.
/// </summary>
app.UseHttpsRedirection();

/// <summary>
/// Middleware autenticazione.
/// </summary>
app.UseAuthentication();

/// <summary>
/// Middleware autorizzazione.
/// </summary>
app.UseAuthorization();

/// <summary>
/// Mapping dei controller API.
/// </summary>
app.MapControllers();

/// <summary>
/// Applicazione delle migrazioni al database.
/// </summary>
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ContestoDb>();
    db.Database.Migrate();
}

/// <summary>
/// Esecuzione seed iniziale (ruoli, utenti, dati cinema).
/// </summary>
await DataSeeder.SeedAsync(app.Services);

/// <summary>
/// Avvio dell'applicazione.
/// </summary>
app.Run();
```

# Exceptions

## AppExceptions.cs (Gestisce gli errori tra i controller e i services)

```c#
namespace NuovoCinemaParadiso.Exceptions;

/// <summary>
/// Classe base per tutte le eccezioni personalizzate dell’applicazione.
/// Estende System.Exception per centralizzare la gestione degli errori.
/// </summary>
public abstract class AppException : Exception
{
    /// <summary>
    /// Costruttore base che inizializza il messaggio dell’eccezione.
    /// </summary>
    /// <param name="message">Messaggio descrittivo dell’errore.</param>
    protected AppException(string message) : base(message) { }
}

/// <summary>
/// Eccezione sollevata quando una risorsa non viene trovata nel sistema.
/// </summary>
public class NotFoundException : AppException
{
    /// <summary>
    /// Crea un'eccezione per risorsa non trovata.
    /// </summary>
    /// <param name="risorsa">Nome della risorsa.</param>
    /// <param name="id">Identificativo della risorsa.</param>
    public NotFoundException(string risorsa, string id)
        : base($"{risorsa} con ID '{id}' non trovato.") { }
}

/// <summary>
/// Eccezione sollevata quando un elemento è già associato o esistente.
/// </summary>
public class ItemAlredyexist : AppException
{
    /// <summary>
    /// Crea un'eccezione per elemento già esistente/associato.
    /// </summary>
    /// <param name="risorsa">Nome della risorsa coinvolta.</param>
    public ItemAlredyexist(string risorsa)
        : base($"Una {risorsa} è già collegata all'utente") { }
}

/// <summary>
/// Eccezione generica di conflitto.
/// </summary>
public class ConflictException : AppException
{
    /// <summary>
    /// Crea un'eccezione di conflitto con messaggio personalizzato.
    /// </summary>
    /// <param name="message">Descrizione del conflitto.</param>
    public ConflictException(string message) : base(message) { }
}

/// <summary>
/// Eccezione sollevata quando una modifica non è valida o duplicata.
/// </summary>
public class ModificaException : AppException
{
    /// <summary>
    /// Crea un'eccezione per errore di modifica (es. duplicato).
    /// </summary>
    /// <param name="message">Nome della risorsa coinvolta.</param>
    public ModificaException(string message) 
        : base($"E' gia presente un {message} con lo stesso nome") { }
}

/// <summary>
/// Eccezione sollevata quando un elemento non viene trovato.
/// </summary>
public class ItemNotFoundException : AppException
{
    /// <summary>
    /// Crea un'eccezione per elemento non trovato.
    /// </summary>
    /// <param name="message">Nome della risorsa non trovata.</param>
    public ItemNotFoundException(string message) 
        : base($"{message} non trovato.") { }
}

/// <summary>
/// Eccezione sollevata quando un indirizzo email non è valido.
/// </summary>
public class InvalidEmail : AppException
{
    /// <summary>
    /// Crea un'eccezione per email non valida.
    /// </summary>
    /// <param name="message">Email non valida.</param>
    public InvalidEmail(string message) 
        : base($"L'email {message} non è valida") { }
}
```