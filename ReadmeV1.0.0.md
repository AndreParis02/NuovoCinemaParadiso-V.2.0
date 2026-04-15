# NuovoCinemaParadiso

## App Cinema
- X Sale, X Movie, X posti.
- Acquirenti comprano i biglietti.
- Promozioni/abbonamenti, bundle biglietti, carnet.
- Ruoli: admin, acquirente, gestore della sala(editor).

# Organizzazione del lavoro:

## convenzioni:

- Chiamare i branch con tipologiaFile-Nome (es. Model-Movie, Controller-Movie ecc...);
- variabili e classi tutte in Italiano (es. usare Utente e non User), per controller, service, ecc.. (ControllerUtente, ServiceUtente, ecc...);

# Diagramma ER

```Mermaid
erDiagram
    Movie ||--o{ Acquisto : Acquisti
    Movie ||--o{ GenereMovie : GeneriMovie
    Sala ||--o{ Acquisto : Acquisti
    Sala ||--o{ TipologiaSala : TipologieSala
    Acquisto ||--o{ Utente : Utenti
    Utente ||--|| Ruolo : Ruoli

    Acquisto {
        int Id
        string MovieId
        Movie Movie
        string SalaId
        Sala Sala
        string UtenteId
        Utente Utente
        int NumeroBiglietti
        DateTimeOffset OrarioCreazione
        decimal PrezzoFinale
    }
    FasciaOraria {
        string Id
        TimeOnly OraInizio
        TimeOnly OraFine
        string Nome
        List Sale
    }
    GenereMovie {
        int Id 
        string Genere
        List Movies
    }
    Movie {
        int Id
        string Titolo
        string Descrizione
        int DurataMinuti
        decimal PrezzoMovie
        List Acquisti
        string GenereMovieId
        GenereMovie GenereMovie
    }
    Ruoli {
        string Gestore
        string Operatore
        string Utente
        string GestoreOrOperatore
    }   
    Sala {
        int Id
        string Nome
        int Capienza
        string FasciaOrariaId
        FasciaOraria FasciaOraria
        List Acquisti
        string TipologiaSalaId
        TipologiaSala TipologiaSala
    }
    TipologiaSala {
        int Id 
        string Nome
        decimal MaggiorazionePrezzo
        List Sale
    }
    Utente {
        string NomeCompleto
        int Eta
        List Acquisti
    } 
```

# Models:

## Acquisto.cs

```c#
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace NuovoCinemaParadiso.Models;

// Mappa questa classe alla tabella "Acquisto" nel database
[Table("Acquisto")]
public class Acquisto
{
    // Chiave primaria dell'acquisto
    // Viene generata automaticamente come stringa GUID
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString();

    // ID del film acquistato (FK obbligatoria)
    [Required]
    public string MovieId { get; set; } = string.Empty;

    // Navigazione verso l'entità Movie
    // [ForeignKey] specifica che MovieId è la chiave esterna
    [ForeignKey("MovieId")]
    public Movie Movie { get; set; }

    // ID della sala in cui avviene la proiezione (FK obbligatoria)
    [Required]
    public string SalaId { get; set; } = string.Empty;

    // Navigazione verso l'entità Sala
    [ForeignKey("SalaId")]
    public Sala Sala { get; set; }

    // ID dell'utente che ha effettuato l'acquisto (FK obbligatoria)
    [Required]
    public string UtenteId { get; set; } = string.Empty;

    // Navigazione verso l'entità Utente
    [ForeignKey("UtenteId")]
    public Utente Utente { get; set; }

    // Numero Biglietti acquistati dall'utente (obbligatorio)
    [Required]
    public int NumeroBiglietti {get;set;}

    // Data e ora in cui è stato creato l'acquisto
    // Impostato automaticamente all'ora UTC corrente
    public DateTimeOffset OrarioCreazione { get; set; } = DateTimeOffset.UtcNow;

    // Prezzo finale Acquisto tenendo conto del prezzo del film,
    // maggiorazione prezzo della tipologia sala e il numero di biglietti 
    //acquistati (obbligatorio)    
    [Required]
    public decimal PrezzoFinale {get; set;}
}
``` 

## FasciaOraria.cs

```c#
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NuovoCinemaParadiso.Models;

// Mappa questa classe alla tabella "FasciaOraria" nel database
[Table("FasciaOraria")]
public class FasciaOraria
{
    // Chiave primaria della fascia oraria
    // Viene generata automaticamente come stringa GUID
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString();

    // Orario di inizio della fascia (es: 14:00)
    // Campo obbligatorio
    [Required]
    public TimeOnly OraInizio { get; set; }

    // Orario di fine della fascia (es: 18:00)
    // Campo obbligatorio
    [Required]
    public TimeOnly OraFine { get; set; }

    // Nome descrittivo della fascia oraria (es: "Pomeriggio", "Sera")
    // Lunghezza massima 50 caratteri
    [StringLength(50)]
    public string Nome { get; set; } = string.Empty;

    // Relazione 1-N: una fascia oraria può essere associata a più sale
    // EF Core popolerà questa lista quando necessario
    public List<Sala> Sale { get; set; } = new();
}
```
## GenereMovie.cs

```c#
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NuovoCinemaParadiso.Models;

// Mappa questa classe alla tabella "Generi" nel database
[Table("Generi")]
public class GenereMovie
{
    // Chiave primaria del genere
    // Viene generata automaticamente come stringa GUID
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString();

    // Nome del genere (es: "Horror", "Azione", "Commedia")
    // Campo obbligatorio, massimo 15 caratteri
    [Required]
    [StringLength(15)]
    public string Genere { get; set; } = string.Empty;

    // Relazione 1-N: un genere può essere associato a più film
    // EF Core popolerà questa lista quando necessario
    public List<Movie> Movies { get; set; } = new List<Movie>();
}
``` 

## LogAzioni.cs

```c#
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NuovoCinemaParadiso.Models;

[Table("LogsAzioni")]
public class LogAzioni
{
    // Chiave primaria della tabella
    // Viene generata automaticamente come stringa GUID
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString();

    // Identificativo dell'utente che ha eseguito l'azione
    public string IdUtente { get; set; } = string.Empty;

    // Nome dell'azione eseguita (es: "Login", "AcquistoBiglietto")
    public string NomeAzione { get; set; } = string.Empty;

    // Indica se l'azione è stata completata con successo (true) o meno (false)
    public bool Effettuato { get; set; }

    // Messaggio descrittivo dell'esito dell'azione (errore o conferma)
    public string Messaggio { get; set; } = string.Empty;

    // Data e ora in cui è stata registrata l'azione
    public DateTimeOffset TimeStamp { get; set; }
}
```

## Movie.cs

```c#
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NuovoCinemaParadiso.Models;

// Mappa questa classe alla tabella "Movie" nel database
[Table("Movie")]
public class Movie
{
    // Chiave primaria del film
    // Viene generata automaticamente come stringa GUID
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString();

    // Titolo del film
    // Campo obbligatorio, massimo 50 caratteri
    [Required]
    [StringLength(50)]
    public string Titolo { get; set; } = string.Empty;

    // Descrizione del film
    // Campo obbligatorio, massimo 200 caratteri
    [Required]
    [StringLength(200)]
    public string Descrizione { get; set; } = string.Empty;

    // Durata del film in minuti (es: 120)
    public int DurataMinuti { get; set; }

    // Prezzo del biglietto per questo film
    public decimal PrezzoMovie { get; set; }

    // Relazione 1-N: un film può avere molti acquisti
    public List<Acquisto> Acquisti { get; set; } = new List<Acquisto>();

    // Chiave esterna che collega il film al suo genere
    public string GenereId { get; set; } = string.Empty;

    // Navigazione verso l'entità GenereMovie
    // [ForeignKey] specifica che GenereId è la chiave esterna
    [ForeignKey("GenereId")]
    public GenereMovie? Genere { get; set; }
}
``` 

## Ruoli.cs

```c#
// Classe statica che contiene tutti i ruoli utilizzati nel sistema
// Viene usata per evitare "stringhe scritte a mano" sparse nel codice
public static class Ruoli
{
    // Ruolo con privilegi massimi: può gestire tutto (film, sale, utenti, acquisti)
    public const string Gestore = "Gestore";

    // Ruolo con privilegi intermedi: può operare sulle risorse ma non gestire utenti
    public const string Operatore = "Operatore";

    // Ruolo base: utente normale che può acquistare e visualizzare contenuti
    public const string Utente = "Utente";

    // Ruolo combinato usato spesso negli attributi [Authorize]
    // Significa: accesso consentito se l'utente è Gestore OPPURE Operatore
    public const string GestoreOrOperatore = "Gestore, Operatore";
}
```

## Sala.cs

```c#
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NuovoCinemaParadiso.Models;

// Mappa questa classe alla tabella "Sala" nel database
[Table("Sala")]
public class Sala
{
    // Chiave primaria della sala
    // Viene generata automaticamente come stringa GUID
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString();

    // Nome della sala (es: "Sala 1", "Sala IMAX")
    // Campo obbligatorio, massimo 100 caratteri
    [Required]
    [StringLength(100)]
    public string Nome { get; set; } = string.Empty;

    // Numero massimo di posti disponibili nella sala
    public int Capienza { get; set; }

    // Chiave esterna che collega la sala alla sua fascia oraria
    public string FasciaOrariaId { get; set; }

    // Navigazione verso l'entità FasciaOraria
    // Indica in quale fascia oraria la sala è disponibile
    [ForeignKey("FasciaOrariaId")]
    public FasciaOraria FasciaOraria { get; set; }

    // Relazione 1-N: una sala può avere molti acquisti
    // Ogni acquisto rappresenta un biglietto venduto per quella sala
    public List<Acquisto> Acquisti { get; set; } = new List<Acquisto>();

    // Chiave esterna che collega la sala alla sua tipologia (es: 3D, IMAX, Standard)
    public string TipologiaSalaId { get; set; } = string.Empty;

    // Navigazione verso l'entità TipologiaSala
    [ForeignKey("TipologiaSalaId")]
    public TipologiaSala TipologiaSala { get; set; }
}
``` 

## TipologiaSala.cs

```C#
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NuovoCinemaParadiso.Models;

// Mappa questa classe alla tabella "TipologiaSala" nel database
[Table("TipologiaSala")]
public class TipologiaSala
{
    // Chiave primaria della tipologia
    // Viene generata automaticamente come stringa GUID
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString();

    // Nome della tipologia della sala (es: "3D", "IMAX", "Standard")
    // Campo obbligatorio
    [Required]
    public string Nome { get; set; } = string.Empty;

    // Eventuale maggiorazione di prezzo associata alla tipologia
    // Esempio: IMAX +3€, 3D +2€
    public decimal MaggiorazionePrezzo { get; set; }

    // Relazione 1-N: una tipologia può essere assegnata a più sale
    // EF Core popolerà questa lista quando necessario
    public List<Sala> Sale { get; set; } = new List<Sala>();
}
``` 

## Utente.cs

```c#
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NuovoCinemaParadiso.Models;

// Mappa questa classe alla tabella "Utente" nel database
// Estende IdentityUser, quindi eredita tutte le proprietà standard:
// Id, UserName, Email, PasswordHash, PhoneNumber, ecc.
[Table("Utente")]
public class Utente : IdentityUser
{
    // Nome completo dell'utente (es: "Mario Rossi")
    // Campo obbligatorio, massimo 100 caratteri
    [Required]
    [StringLength(100)]
    public string NomeCompleto { get; set; } = string.Empty;

    // Età dell'utente
    // Deve essere compresa tra 14 e 100 anni
    // La validazione viene gestita tramite l'attributo [Range]
    [Required]
    [Range(14, 100, ErrorMessage = "L'età deve essere compresa tra 14 e 100")]
    public int Eta { get; set; }

    // Relazione 1-N: un utente può effettuare molti acquisti
    // EF Core popolerà questa lista quando necessario
    public List<Acquisto> Acquisti { get; set; } = new List<Acquisto>();
}
```

# Helpers:

## CalcolaPrezzoHelper.cs
```c#
namespace NuovoCinemaParadiso.Helpers;

// Classe statica che contiene metodi di utilità per il calcolo dei prezzi
public static class CalcolaPrezzo
{
    // Calcola il prezzo finale del biglietto
    // Somma il prezzo base del film e la maggiorazione della sala e   
    // li moltiplica per il numero dei biglietti acquistati
    // Esempio: film 8€, sala IMAX +3€, numero biglietti 3 → totale 33€
        public static Decimal CalcolaPrezzoFinale(Decimal prezzoMovie, Decimal prezzoSala, int numeroBiglietti)
    {
        // Somma dei due valori e li moltiplica per il numero biglietti
        Decimal prezzoFinale = (prezzoMovie + prezzoSala) * numeroBiglietti;

        // Restituisce il totale
        return prezzoFinale;
    }
}
```

## JwtHelper.cs:

```c#
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using NuovoCinemaParadiso.Models;

namespace NuovoCinemaParadiso.Helpers;

// Classe helper che si occupa di generare i token JWT
public class JwtHelper
{
    private readonly IConfiguration _configurazione;

    // Il costruttore riceve la configurazione dell'app (appsettings.json)
    public JwtHelper(IConfiguration configurazione) 
    {
        _configurazione = configurazione; 
    }

    // Metodo che genera un token JWT per un utente e i suoi ruoli
    public string GenerateToken(Utente utente, IList<string> ruoli) 
    {
        // Recupera i valori dal file appsettings.json
        string? key = _configurazione["Jwt:Key"];
        string? issuer = _configurazione["Jwt:Issuer"]; 
        string? audience = _configurazione["Jwt:Audience"];  

        // Controllo che i valori siano presenti
        if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(issuer) || string.IsNullOrEmpty(audience))
        {
            throw new Exception("Configurazione JWT mancante.");
        }

        // Lista dei claim che verranno inseriti nel token
        List<Claim> claims = new List<Claim>();
        
        // Claim base dell'utente
        // Identificatore univoco dell'utente
        claims.Add(new Claim(ClaimTypes.NameIdentifier, utente.Id));

        // Username dell'utente
        claims.Add(new Claim(ClaimTypes.Name, utente.UserName ?? ""));

        // Email dell'utente
        claims.Add(new Claim(ClaimTypes.Email, utente.Email ?? ""));

        // Aggiunta dei ruoli come claim
        for (int i = 0; i < ruoli.Count; i++)
        {
            claims.Add(new Claim(ClaimTypes.Role, ruoli[i]));
        }

        // Creazione della chiave di sicurezza simmetrica
        // Viene usata per firmare il token
        SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

        // Credenziali di firma usando algoritmo HMAC-SHA256
        SigningCredentials credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        // Creazione del token JWT
        JwtSecurityToken token = new JwtSecurityToken(
            issuer: issuer,          // Chi ha generato il token
            audience: audience,      // Chi può usare il token
            claims: claims,          // Informazioni contenute nel token
            expires: DateTimeOffset.UtcNow.AddHours(1), // Scadenza del token
            signingCredentials: credentials        // Firma del token
        );

        // Converte l'oggetto token in una stringa JWT
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
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
    // Il DbContext principale dell'applicazione.
    // Estende IdentityDbContext per integrare ASP.NET Identity.
    // IdentityDbContext gestisce automaticamente:
    // - Utenti
    // - Ruoli
    // - UserRoles
    // - Claims
    // - Logins esterni
    // - Token
    //
    // Specificando <Utente, IdentityRole, string> diciamo:
    // - Usa la nostra classe Utente come modello utente
    // - Usa IdentityRole per i ruoli
    // - Usa string come tipo della chiave primaria
    public class ContestoDb : IdentityDbContext<Utente, IdentityRole, string>
    {
        // Costruttore che riceve le opzioni del DbContext (connection string, provider, ecc.)
        public ContestoDb(DbContextOptions<ContestoDb> opzioni)
            : base(opzioni)
        {
        }

        // Le nostre tabelle personalizzate:

        // Tabella dei Movies
        public DbSet<Movie> Movies { get; set; }

        // Tabella dei generi dei Movies
        public DbSet<GenereMovie> GeneriMovies { get; set; }

        // Tabella delle sale del cinema
        public DbSet<Sala> Sale { get; set; }

        // Tabella delle tipologie di sala (IMAX, 3D, Standard...)
        public DbSet<TipologiaSala> TipologieSala { get; set; }

        // Tabella delle fasce orarie (Mattina, Pomeriggio, Sera...)
        public DbSet<FasciaOraria> FasceOrarie { get; set; }

        // Tabella degli acquisti 
        public DbSet<Acquisto> Acquisti { get; set; }

        // Tabella degli utenti personalizzata (sostituisce AspNetUsers)
        public DbSet<Utente> Utenti { get; set; }

        // Tabella Logger delle azioni 
        public DbSet<LogAzioni> LogAzioni {get;set;}
    }
}
```

# Dtos

## DtoAcquisto.cs

```c#
namespace NuovoCinemaParadiso.Dtos;

// DTO usato per restituire i dati di un acquisto al client
// Contiene solo le informazioni necessarie, evitando di esporre l'intero modello EF
public class DtoAcquisto
{
    // Identificatore dell'acquisto
    public string Id { get; set; }

    // ID del film acquistato
    public string MovieId { get; set; } = string.Empty;

    // Titolo del film (dato derivato, non presente nella tabella Acquisto)
    public string Titolo { get; set; } = string.Empty;

    // ID della sala in cui è stata effettuata la proiezione
    public string SalaId { get; set; } = string.Empty;

    // Nome della sala (dato derivato)
    public string Nome { get; set; } = string.Empty;

    // ID dell'utente che ha effettuato l'acquisto
    public string UtenteId { get; set; } = string.Empty;

    // Nome completo dell'utente (dato derivato)
    public string NomeCompleto { get; set; } = string.Empty;

    // Prezzo finale del acquisto (film + eventuale maggiorazione sala e 
    // numero biglietti)
    public decimal PrezzoFinale { get; set; }

    // Data e ora in cui è stato creato l'acquisto
    public DateTimeOffset OrarioCreazione { get; set; }

    // Numero biglietti acquistati dall'utente
    public int NumeroBiglietti {get;set;}
}
```

## DtoAuthResponse.cs

```c#
namespace NuovoCinemaParadiso.Dtos;

// DTO restituito al client dopo un login o una registrazione riuscita
// Contiene tutte le informazioni utili per identificare l'utente e gestire la sessione
public class DtoAuthResponse
{
    // ID univoco dell'utente (ereditato da IdentityUser)
    public string Id { get; set; } = string.Empty;

    // Nome completo dell'utente (es: "Mario Rossi")
    public string NomeCompleto { get; set; } = string.Empty;

    // Token JWT generato al momento dell'autenticazione
    // Il client lo userà per autenticarsi nelle richieste successive
    public string Token { get; set; } = string.Empty;

    // Età dell'utente
    public int Eta { get; set; }

    // Email dell'utente
    public string Email { get; set; } = string.Empty;

    // Ruolo principale dell'utente (Gestore, Operatore, Utente)
    public string Ruolo { get; set; } = string.Empty;
}
```

## DtoCreazioneAcquisto.cs

```c#
namespace NuovoCinemaParadiso.Dtos;

// DTO utilizzato quando il client vuole creare un nuovo acquisto
// Contiene solo i dati necessari per registrare l'acquisto nel sistema
public class DtoCreazioneAcquisto
{
    // ID del film che l'utente vuole acquistare
    [Required]
    public string MovieId { get; set; } = string.Empty;

    // ID della sala in cui avverrà la proiezione
    [Required]
    public string SalaId { get; set; } = string.Empty;

    // ID dell'utente che effettua l'acquisto
    // In molti sistemi questo valore viene preso dal token JWT, non dal client
    public string? UtenteId { get; set; } = string.Empty;

    // Numero di biglietti acquistati dall'utente
    [Required]
    public int NumeroBiglietti { get; set; }

    // Prezzo finale del biglietto 
    // ((film + eventuale maggiorazione sala) * numero biglietti)
    // Questo valore può essere calcolato lato server per maggiore sicurezza
    [Required]
    public decimal PrezzoFinale { get; set; }
}
```

## DtoCreazioneFasciaOraria.cs

```c#
using System.ComponentModel.DataAnnotations;

namespace NuovoCinemaParadiso.Dtos;

// DTO utilizzato per creare una nuova fascia oraria
// Contiene solo i dati necessari per registrare la fascia nel sistema
public class DtoCreazioneFasciaOraria
{
    // Orario di inizio della fascia (es: 14:00)
    // Campo obbligatorio
    [Required]
    public TimeOnly OraInizio { get; set; }

    // Orario di fine della fascia (es: 18:00)
    // Campo obbligatorio
    [Required]
    public TimeOnly OraFine { get; set; }

    // Nome descrittivo della fascia (es: "Sera", "Pomeriggio")
    // Lunghezza massima 50 caratteri
    [StringLength(50)]
    public string Nome { get; set; } = string.Empty;
}
```

## DtoCreazioneGenereMovie.cs

```c#
using System.ComponentModel.DataAnnotations;

namespace NuovoCinemaParadiso.Dtos;

// DTO utilizzato per creare un nuovo genere cinematografico
// Contiene solo il dato necessario per registrare il genere nel sistema
public class DtoCreazioneGenereMovie
{
    // Nome del genere (es: "Horror", "Azione", "Fantasy")
    // Campo obbligatorio, massimo 15 caratteri
    [Required]
    [StringLength(15)]
    public string Genere { get; set; } = string.Empty;
}
```

## DtoCreazioneLogAzioni

```c#
namespace NuovoCinemaParadiso.Dtos;

// DTO usato per trasferire i dati relativi alla creazione di un log
public class DtoCreazioneLogAzioni
{
    // Identificativo del log 
    public string Id { get; set; }

    // Identificativo dell'utente che ha eseguito l'azione
    public string IdUtente { get; set; } = string.Empty;

    // Nome dell'azione (es: "Login", "Prenotazione")
    public string NomeAzione { get; set; } = string.Empty;

    // Indica se l'azione è andata a buon fine
    public bool Effettuato { get; set; }

    // Messaggio descrittivo (errore o conferma)
    public string Messaggio { get; set; } = string.Empty;

    // Data e ora dell'azione
    public DateTimeOffset TimeStamp { get; set; }
}
```

## DtoCreazioneMovie.cs

```c#
using System.ComponentModel.DataAnnotations;

namespace NuovoCinemaParadiso.Dtos;

// DTO utilizzato per creare un nuovo film
// Contiene solo i dati necessari per registrare il film nel sistema
public class DtoCreazioneMovie
{
    // Titolo del film
    // Campo obbligatorio, massimo 50 caratteri
    [Required]
    [StringLength(50)]
    public string Titolo { get; set; } = string.Empty;

    // Descrizione del film
    // Campo obbligatorio, massimo 200 caratteri
    [Required]
    [StringLength(200)]
    public string Descrizione { get; set; } = string.Empty;

    // Durata del film in minuti (es: 120)
    public int DurataMinuti { get; set; }

    // Prezzo base del film (senza maggiorazioni della sala)
    public decimal PrezzoMovie { get; set; }

    // ID del genere a cui appartiene il film
    // Deve corrispondere a un GenereMovie esistente
    public string GenereId { get; set; } = string.Empty;
}
```

## DtoCreazioneSala.cs

```c#
using System.ComponentModel.DataAnnotations;

namespace NuovoCinemaParadiso.Dtos;

// DTO utilizzato per creare una nuova sala del cinema
// Contiene solo i dati necessari per registrare la sala nel sistema
public class DtoCreazioneSala
{
    // Nome della sala (es: "Sala 1", "Sala IMAX")
    // Campo obbligatorio, massimo 100 caratteri
    [Required]
    [StringLength(100)]
    public string Nome { get; set; } = string.Empty;

    // Numero massimo di posti disponibili nella sala
    public int Capienza { get; set; }

    // ID della tipologia della sala (Standard, 3D, IMAX, VIP...)
    // Deve corrispondere a una TipologiaSala esistente
    public string TipologiaSalaId { get; set; } = string.Empty;

    // ID della fascia oraria in cui la sala è disponibile
    // Deve corrispondere a una FasciaOraria esistente
    public string FasciaOrariaId { get; set; } = string.Empty;
}
```

## DtoCreazioneTipologiaSala.cs

```c#
using System.ComponentModel.DataAnnotations;

namespace NuovoCinemaParadiso.Dtos;

// DTO utilizzato per creare una nuova tipologia di sala
// Contiene solo i dati necessari per registrare la tipologia nel sistema
public class DtoCreazioneTipologiaSala
{
    // Nome della tipologia (es: "Standard", "3D", "IMAX", "VIP")
    // Campo obbligatorio
    [Required]
    public string Nome { get; set; } = string.Empty;

    // Maggiorazione di prezzo associata alla tipologia
    // Esempio: IMAX +3€, 3D +2€
    public decimal MaggiorazionePrezzo { get; set; }
}
```

## DtoCreazioneUtente.cs

```c#
using System.ComponentModel.DataAnnotations;

namespace NuovoCinemaParadiso.Dtos;

// DTO utilizzato per creare un nuovo utente nel sistema
// Contiene solo i dati necessari per la registrazione lato server
public class DtoCreazioneUtente
{
    // Nome completo dell'utente (es: "Mario Rossi")
    // Campo obbligatorio, massimo 100 caratteri
    [Required]
    [StringLength(100)]
    public string NomeCompleto { get; set; } = string.Empty;

    // Età dell'utente
    // Deve essere compresa tra 14 e 100 anni
    // La validazione impedisce registrazioni di minorenni o valori non realistici
    [Required]
    [Range(14, 100, ErrorMessage = "L'età deve essere compresa tra 14 e 100")]
    public int Eta { get; set; }
}
```

## DtoFasciaOraria.cs

```c#
namespace NuovoCinemaParadiso.Dtos;

// DTO utilizzato per restituire al client i dati di una fascia oraria
// Contiene tutte le informazioni necessarie per visualizzare la fascia
public class DtoFasciaOraria
{
    // Identificatore univoco della fascia oraria
    public string Id { get; set; }

    // Orario di inizio della fascia (es: 14:00)
    public TimeOnly OraInizio { get; set; }

    // Orario di fine della fascia (es: 18:00)
    public TimeOnly OraFine { get; set; }

    // Nome descrittivo della fascia (es: "Sera", "Pomeriggio")
    public string Nome { get; set; } = string.Empty;
}
```

## DtoGenereMovie.cs

```c#
namespace NuovoCinemaParadiso.Dtos;

// DTO utilizzato per restituire al client i dati di un genere cinematografico
// Contiene solo le informazioni essenziali, senza oggetti complessi
public class DtoGenereMovie
{
    // Identificatore univoco del genere (generato dal database)
    public string Id { get; set; }

    // Nome del genere (es: "Horror", "Azione", "Fantasy")
    public string Genere { get; set; } = string.Empty;
}
```

## DtoLogAzioni.cs

```c#
namespace NuovoCinemaParadiso.Dtos;

// DTO usato per restituire i dati dei log (ad esempio nelle API)
public class DtoLogAzioni
{
    // Identificativo del log
    public string Id { get; set; }

    // Identificativo dell'utente che ha eseguito l'azione
    public string IdUtente { get; set; } = string.Empty;

    // Nome dell'azione eseguita
    public string NomeAzione { get; set; } = string.Empty;

    // Indica se l'azione è stata completata con successo
    public bool Effettuato { get; set; }

    // Messaggio associato all'azione (errore o conferma)
    public string Messaggio { get; set; } = string.Empty;

    // Data e ora in cui è avvenuta l'azione
    public DateTimeOffset TimeStamp { get; set; }
}
```

## DtoLogin.cs

```c#
using System.ComponentModel.DataAnnotations;

namespace NuovoCinemaParadiso.Dtos;

// DTO utilizzato per effettuare il login
// Contiene le credenziali che l'utente deve fornire per autenticarsi
public class DtoLogin
{
    // Email dell'utente
    // Campo obbligatorio e deve essere in formato email valido
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    // Password dell'utente
    // Campo obbligatorio, minimo 6 caratteri, massimo 100
    [Required]
    [StringLength(100, MinimumLength = 6)]
    public string Password { get; set; } = string.Empty;
}
```

## DtoModificaRuoloUtente.cs

```c#
using System.ComponentModel.DataAnnotations;

namespace NuovoCinemaParadiso.Dtos;

// DTO utilizzato per modificare il ruolo di un utente esistente
// Viene inviato dal client quando un amministratore vuole cambiare il ruolo di un utente
public class DtoModificaRuoloUtente
{
    // Email dell'utente a cui modificare il ruolo
    // Campo obbligatorio e deve essere un indirizzo email valido
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    // Nuovo ruolo da assegnare all'utente
    // Campo obbligatorio
    // Deve corrispondere a un ruolo esistente nel sistema (es: "Admin", "Gestore", "Utente")
    [Required]
    public string NuovoRuolo { get; set; } = string.Empty;
}
```

## DtoMovie.cs

```c#
namespace NuovoCinemaParadiso.Dtos;

// DTO utilizzato per restituire al client i dati di un film
// Contiene sia le informazioni base del film che il nome del genere associato
public class DtoMovie
{
    // Identificatore univoco del film
    public string Id { get; set; }

    // Titolo del film
    public string Titolo { get; set; } = string.Empty;

    // Descrizione del film
    public string Descrizione { get; set; } = string.Empty;

    // Durata del film in minuti (es: 120)
    public int DurataMinuti { get; set; }

    // Prezzo base del film (senza maggiorazioni della sala)
    public decimal PrezzoMovie { get; set; }

    // ID del genere associato al film
    public string GenereId { get; set; } = string.Empty;

    // Nome del genere (dato derivato, utile per il frontend)
    public string Genere { get; set; } = string.Empty;
}
```

## DtoRegistrazione.cs

```c#
using System.ComponentModel.DataAnnotations;

namespace NuovoCinemaParadiso.Dtos;

// DTO utilizzato per registrare un nuovo utente nel sistema
// Contiene tutte le informazioni necessarie per creare l'account
public class DtoRegistrazione
{
    // Email dell'utente
    // Campo obbligatorio e deve essere in formato email valido
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    // Password scelta dall'utente
    // Campo obbligatorio, minimo 6 caratteri, massimo 100
    [Required]
    [StringLength(100, MinimumLength = 6)]
    public string Password { get; set; } = string.Empty;

    // Nome completo dell'utente (es: "Mario Rossi")
    // Campo obbligatorio, massimo 100 caratteri
    [Required]
    [StringLength(100)]
    public string NomeCompleto { get; set; } = string.Empty;

    // Età dell'utente
    // Nessuna validazione qui, ma può essere gestita lato controller o modello
    public int Eta { get; set; }
}
```

## DtoSala.cs

```c#
using System.ComponentModel.DataAnnotations;

namespace NuovoCinemaParadiso.Dtos;

// DTO utilizzato per restituire al client i dati di una sala del cinema
// Include sia le informazioni base sia i dati derivati (tipologia e fascia oraria)
public class DtoSala
{
    // Identificatore univoco della sala
    public string Id { get; set; }

    // Nome della sala (es: "Sala 1", "Sala IMAX")
    // Non obbligatorio in output, ma limitato a 100 caratteri
    [StringLength(100)]
    public string Nome { get; set; } = string.Empty;

    // Numero massimo di posti disponibili nella sala
    public int Capienza { get; set; }

    // ID della fascia oraria associata alla sala
    public string FasciaOrariaId { get; set; } = string.Empty;

    // Nome della fascia oraria (dato derivato)
    // Es: "Pomeriggio", "Sera"
    public string FasciaOraria { get; set; } = string.Empty;

    // ID della tipologia della sala (Standard, 3D, IMAX, VIP...)
    public string TipologiaSalaId { get; set; } = string.Empty;

    // Nome della tipologia (dato derivato)
    // Es: "IMAX", "3D", "Standard"
    public string NomeTipologia { get; set; } = string.Empty;
}
```

## DtoTipologiaSala.cs

```c#
namespace NuovoCinemaParadiso.Dtos;

// DTO utilizzato per restituire al client i dati di una tipologia di sala
// Contiene tutte le informazioni necessarie per visualizzare la tipologia
public class DtoTipologiaSala
{
    // Identificatore univoco della tipologia (generato dal database)
    public string Id { get; set; }

    // Nome della tipologia (es: "Standard", "3D", "IMAX", "VIP")
    public string Nome { get; set; } = string.Empty;

    // Maggiorazione di prezzo associata alla tipologia
    // Esempio: IMAX +3€, 3D +2€
    public decimal MaggiorazionePrezzo { get; set; }
}
```

## DtoUtente.cs

```c#
namespace NuovoCinemaParadiso.Dtos;

// DTO utilizzato per restituire al client i dati di un utente
// Contiene solo le informazioni essenziali, senza dati sensibili
public class DtoUtente
{
    // Identificatore univoco dell'utente
    public string Id { get; set; } = string.Empty;

    // Nome completo dell'utente (es: "Mario Rossi")
    public string NomeCompleto { get; set; } = string.Empty;

    // Email dell'utente
    // Viene restituita solo come informazione, mai la password
    public string Email { get; set; } = string.Empty;

    // Età dell'utente
    public int Eta { get; set; }
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
/// Classe responsabile del popolamento iniziale del database (seed).
/// Crea ruoli, utenti e dati base dell'applicazione.
/// </summary>
public static class DataSeeder
{
    /// <summary>
    /// Metodo principale di seed eseguito all'avvio dell'applicazione.
    /// Crea un scope per recuperare i servizi necessari da DI.
    /// </summary>
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        // Creazione di uno scope per usare servizi scoped (DbContext, UserManager, ecc.)
        using IServiceScope scope = serviceProvider.CreateScope();

        // Recupero del contesto DB
        ContestoDb contestoDb = scope.ServiceProvider.GetRequiredService<ContestoDb>();

        // Identity: gestione utenti e ruoli
        UserManager<Utente> gestioneUtenti = scope.ServiceProvider.GetRequiredService<UserManager<Utente>>();
        RoleManager<IdentityRole> gestioneRuoli = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        // =======================
        // CREAZIONE RUOLI
        // =======================

        await AssicuraEsistenzaRuoloAsync(gestioneRuoli, Ruoli.Gestore);
        await AssicuraEsistenzaRuoloAsync(gestioneRuoli, Ruoli.Operatore);
        await AssicuraEsistenzaRuoloAsync(gestioneRuoli, Ruoli.Utente);

        // =======================
        // CREAZIONE UTENTI BASE
        // =======================

        Utente gestore = await AssicuraEsistenzaUtenteAsync(
            gestioneUtenti,
            "gestore@gmail.com",
            "123456",
            "Gestore",
            60);

        Utente operatore = await AssicuraEsistenzaUtenteAsync(
            gestioneUtenti,
            "operatore@gmail.com",
            "123456",
            "Operatore",
            35);

        Utente utente = await AssicuraEsistenzaUtenteAsync(
            gestioneUtenti,
            "utente1@gmail.com",
            "123456",
            "Utente Uno",
            15);

        // =======================
        // ASSEGNAZIONE RUOLI
        // =======================

        await ImpostaRuoloUnicoAsync(gestioneUtenti, gestore, Ruoli.Gestore);
        await ImpostaRuoloUnicoAsync(gestioneUtenti, operatore, Ruoli.Operatore);
        await ImpostaRuoloUnicoAsync(gestioneUtenti, utente, Ruoli.Utente);

        // =======================
        // DATI DI BASE (REFERENCE DATA)
        // =======================

        // Inserimento genere film se non esiste già
        await AssicuraEsistenzaGenereMovie(contestoDb, "Azione");

        // Inserimento tipologia sala se non esiste già
        await AssicuraEsistenzaTipologiaSala(contestoDb, "3D", 3);

        // Inserimento fascia oraria se non esiste già
        await AssicuraEsistenzaFasciaOraria(
            contestoDb,
            TimeOnly.FromHours(10),
            TimeOnly.FromHours(13),
            "Mattina");
    }

    /// <summary>
    /// Crea un ruolo se non esiste già nel sistema Identity.
    /// </summary>
    private static async Task AssicuraEsistenzaRuoloAsync(RoleManager<IdentityRole> managerRuolo, string nomeRuolo)
    {
        bool seEsiste = await managerRuolo.RoleExistsAsync(nomeRuolo);

        if (!seEsiste)
        {
            IdentityRole ruolo = new IdentityRole
            {
                Name = nomeRuolo
            };

            await managerRuolo.CreateAsync(ruolo);
        }
    }

    /// <summary>
    /// Crea un utente se non esiste già (controllo per email).
    /// </summary>
    private static async Task<Utente> AssicuraEsistenzaUtenteAsync(
        UserManager<Utente> gestioneUtenti,
        string email,
        string password,
        string nomeCompleto,
        int eta)
    {
        // Controllo esistenza utente
        Utente? utenteEsistente = await gestioneUtenti.FindByEmailAsync(email);

        if (utenteEsistente != null)
        {
            return utenteEsistente;
        }

        // Creazione nuovo utente
        Utente utente = new Utente
        {
            UserName = email,
            Email = email,
            NomeCompleto = nomeCompleto,
            Eta = eta
        };

        IdentityResult risultato = await gestioneUtenti.CreateAsync(utente, password);

        // Gestione errori creazione utente
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
    /// Assegna un solo ruolo all'utente rimuovendo eventuali ruoli precedenti.
    /// </summary>
    private static async Task ImpostaRuoloUnicoAsync(
        UserManager<Utente> gestioneUtenti,
        Utente utente,
        string ruoloTarget)
    {
        IList<string> ruoliCorrenti = await gestioneUtenti.GetRolesAsync(utente);

        // Rimozione di eventuali ruoli esistenti (Gestore, Operatore, Utente)
        for (int i = 0; i < ruoliCorrenti.Count; i++)
        {
            string ruoloCorrente = ruoliCorrenti[i];

            if (ruoloCorrente == Ruoli.Gestore ||
                ruoloCorrente == Ruoli.Operatore ||
                ruoloCorrente == Ruoli.Utente)
            {
                await gestioneUtenti.RemoveFromRoleAsync(utente, ruoloCorrente);
            }
        }

        // Assegna il ruolo target se non presente
        bool alreadyInTargetRole = await gestioneUtenti.IsInRoleAsync(utente, ruoloTarget);

        if (!alreadyInTargetRole)
        {
            await gestioneUtenti.AddToRoleAsync(utente, ruoloTarget);
        }
    }

    /// <summary>
    /// Crea un genere film se non esiste già (case-insensitive).
    /// </summary>
    private static async Task AssicuraEsistenzaGenereMovie(
        ContestoDb context,
        string genere)
    {
        // Carica tutti i generi (non ottimale per grandi dataset)
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
                return;
            }
        }

        GenereMovie nuovoGenere = new GenereMovie
        {
            Genere = genere
        };

        context.GeneriMovies.Add(nuovoGenere);
        await context.SaveChangesAsync();
    }

    /// <summary>
    /// Crea una tipologia sala se non esiste già.
    /// </summary>
    private static async Task AssicuraEsistenzaTipologiaSala(
        ContestoDb context,
        string nome,
        decimal maggiorazioneprezzo)
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
                return;
            }
        }

        TipologiaSala nuovaTipologia = new TipologiaSala
        {
            Nome = nome,
            MaggiorazionePrezzo = maggiorazioneprezzo,
        };

        context.TipologieSala.Add(nuovaTipologia);
        await context.SaveChangesAsync();
    }

    /// <summary>
    /// Crea una fascia oraria se non esiste già (controllo su nome o intervallo orario).
    /// </summary>
    private static async Task AssicuraEsistenzaFasciaOraria(
        ContestoDb context,
        TimeOnly oraInizio,
        TimeOnly oraFine,
        string nome)
    {
        List<FasciaOraria> fasceOrarie = await context.FasceOrarie.ToListAsync();

        for (int i = 0; i < fasceOrarie.Count; i++)
        {
            FasciaOraria fasciaOrariaCorrente = fasceOrarie[i];

            bool nomeUguale = string.Equals(
                fasciaOrariaCorrente.Nome,
                nome,
                StringComparison.OrdinalIgnoreCase);

            bool intervalloUguale =
                fasciaOrariaCorrente.OraInizio == oraInizio &&
                fasciaOrariaCorrente.OraFine == oraFine;

            if (nomeUguale || intervalloUguale)
            {
                return;
            }
        }

        FasciaOraria nuovaFasciaOraria = new FasciaOraria
        {
            Nome = nome,
            OraInizio = oraInizio,
            OraFine = oraFine
        };

        context.FasceOrarie.Add(nuovaFasciaOraria);
        await context.SaveChangesAsync();
    }
}
```

# Controllers

## AcquistiController.cs

```c#
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using NuovoCinemaParadiso.Services;
using NuovoCinemaParadiso.Dtos;
using NuovoCinemaParadiso.Models;

namespace NuovoCinemaParadiso.Controllers;

// Indica che questa classe è un controller API
[ApiController]
// Definisce la route base: api/Acquisti
[Route("api/[controller]")]
// Richiede autenticazione per tutti gli endpoint
[Authorize]
public class AcquistiController : ControllerBase
{
    // Service per la gestione degli acquisti
    private readonly AcquistoService _acquistoService;

    // Service per il logging delle azioni
    private readonly LogAzioniService _logAzioniService;

    // Costruttore con Dependency Injection
    public AcquistiController(AcquistoService acquistoService, LogAzioniService logAzioniService)
    {
        _acquistoService = acquistoService;
        _logAzioniService = logAzioniService;
    }

    // ========================= ADMIN =========================

    // Ottiene tutti gli acquisti (solo admin/operatori)
    [HttpGet("admin")]
    [Authorize(Roles = Ruoli.GestoreOrOperatore)]
    public async Task<IActionResult> OttieniTuttiGliAcquistiAdmin()
    {
        List<DtoAcquisto> acquisti = await _acquistoService.OttieniTuttoAdmin();

        // Recupera l'ID dell'utente autenticato dal token
        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        // Log dell'azione
        await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
        {
            IdUtente = utenteId,
            NomeAzione = "Ottieni tutti gli acquisti admin",
            Effettuato = true,
            Messaggio = "Operazione eseguita"
        });

        return Ok(acquisti);
    }

    // ========================= UTENTE =========================

    // Ottiene tutti gli acquisti dell'utente autenticato
    [HttpGet]
    public async Task<IActionResult> OttieniTuttiGliAcquistiUtente()
    {
        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        List<DtoAcquisto> acquisti = await _acquistoService.OttieniTutto(utenteId);

        // Log
        await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
        {
            IdUtente = utenteId,
            NomeAzione = "Ottieni tutti gli acquisti utente",
            Effettuato = true,
            Messaggio = "Operazione eseguita"
        });

        return Ok(acquisti);
    }

    // ========================= DETTAGLIO =========================

    // Ottiene un acquisto per ID (admin)
    [HttpGet("admin/{id}")]
    [Authorize(Roles = Ruoli.GestoreOrOperatore)]
    public async Task<IActionResult> OttieniTramiteIdPerAdmin(string id)
    {
        var risultato = await _acquistoService.OttieniTramiteIdPerAdminAsync(id);
        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        // Se non trovato
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

        // Log successo
        await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
        {
            IdUtente = utenteId,
            NomeAzione = "Ottieni acquisti tramite id admin",
            Effettuato = true,
            Messaggio = "Operazione eseguita"
        });

        return Ok(risultato);
    }

    // Ottiene un acquisto per ID (utente)
    [HttpGet("{id}")]
    public async Task<IActionResult> OttieniTramiteIdPerUtente(string id)
    {
        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var risultato = await _acquistoService.OttieniTramiteIdAsync(id, utenteId);

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

        await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
        {
            IdUtente = utenteId,
            NomeAzione = "Ottieni acquisti tramite id utente",
            Effettuato = true,
            Messaggio = "Operazione eseguita"
        });

        return Ok(risultato);
    }

    // ========================= CREAZIONE =========================

    // Crea un nuovo acquisto
    [HttpPost]
    public async Task<IActionResult> Creazione([FromBody] DtoCreazioneAcquisto dto)
    {
        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        DtoAcquisto? risultato = await _acquistoService.CreazioneAsync(dto, utenteId);

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

        await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
        {
            IdUtente = utenteId,
            NomeAzione = "Creazione acquisto",
            Effettuato = true,
            Messaggio = "Operazione eseguita"
        });

        return Ok(risultato);
    }

    // ========================= MODIFICA =========================

    // Modifica un acquisto (solo admin/operatori)
    [HttpPut("{id}")]
    [Authorize(Roles = Ruoli.GestoreOrOperatore)]
    public async Task<IActionResult> Modifica(string id, [FromBody] DtoCreazioneAcquisto dto)
    {
        DtoAcquisto? risultato = await _acquistoService.ModificaAsync(id, dto);
        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

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

        await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
        {
            IdUtente = utenteId,
            NomeAzione = "Modifica acquisto",
            Effettuato = true,
            Messaggio = "Operazione eseguita"
        });

        return Ok(risultato);
    }

    // ========================= ELIMINAZIONE =========================

    // Elimina un acquisto (solo admin/operatori)
    [HttpDelete("{id}")]
    [Authorize(Roles = Ruoli.GestoreOrOperatore)]
    public async Task<IActionResult> Elimina(string id)
    {
        bool eliminato = await _acquistoService.EliminazioneAsync(id);
        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

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

## AuthController.cs

```c#
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using NuovoCinemaParadiso.Services;
using NuovoCinemaParadiso.Dtos;
using Microsoft.AspNetCore.Authorization;

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

    // -----------------------------------------------------
    // POST: api/auth/registrazione
    // Registra un nuovo utente
    // Registra i logs dell'azione.
    // -----------------------------------------------------
    [HttpPost("registrazione")]
    public async Task<IActionResult> Registrazione(DtoRegistrazione dto)
    {
        IdentityResult result = await _authService.RegistrazioneAsync(dto);

        if (!result.Succeeded)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
            {
                NomeAzione = "Registrazione utente",
                Effettuato = false,
                Messaggio = "Registrazione fallita"
            });
            return BadRequest(result.Errors);
        }
        await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
        {
            NomeAzione = "Registrazione utente",
            Effettuato = true,
            Messaggio = "Registrazione avvenuta"
        });

        return Ok(new { messaggio = "Registrazione avvenuta con successo!" });
    }

    // -----------------------------------------------------
    // POST: api/auth/login
    // Effettua il login e restituisce token + info utente
    // Registra i logs dell'azione.
    // -----------------------------------------------------
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] DtoLogin dto)
    {
        DtoAuthResponse? risposta = await _authService.LoginAsync(dto);
        if (risposta == null)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
            {   
                NomeAzione = "Login",
                Effettuato = false,
                Messaggio  = "Login fallito"
            });
            return Unauthorized(new { messaggio = "Email o password non validi." });
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
        {
            IdUtente   = risposta.Id,
            NomeAzione = "Login",
            Effettuato = true,
            Messaggio = "Login avvenuto"
        });
        return Ok(risposta);
    }

    // -----------------------------------------------------
    // GET: api/auth
    // Restituisce la lista dei profili
    // -----------------------------------------------------
    [HttpGet]
    [Authorize(Roles = Ruoli.GestoreOrOperatore)]
    public async Task<IActionResult> OttieniTuttiIProfili()
    {
        List<DtoUtente> utenti = await _authService.OttieniTuttoAsync();
        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
            {   
                IdUtente   = utenteId,  
                NomeAzione = "Ricerca profili",
                Effettuato = true,
                Messaggio  = "Ricerca avvenuta"
            });

        return Ok(utenti); ;
    }

    // -----------------------------------------------------
    // GET: api/auth/profilo
    // Restituisce il profilo dell’utente loggato
    // -----------------------------------------------------
   [HttpGet("profilo")]
    public async Task<IActionResult> RicercaProfiloLoggato()
    {
        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        DtoUtente? utente = await _authService.OttieniTramiteIdAsync(utenteId);

        if (utente == null)
        {
            return NotFound(new { messaggio = "Utente non trovato." });
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
            {   
                IdUtente   = utente.Id,  
                NomeAzione = "Ricerca profilo loggato",
                Effettuato = true,
                Messaggio  = "Ricerca avvenuta"
            });

        return Ok(utente);
    }


    // -----------------------------------------------------
    // GET: api/auth/{id}
    // Solo Gestore/Operatore possono vedere profili altrui
    // -----------------------------------------------------
    [HttpGet("{id}")]
    [Authorize(Roles = Ruoli.GestoreOrOperatore)]
    public async Task<IActionResult> RicercaProfiloTramiteId(string id)
    {
        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        DtoUtente? utente = await _authService.OttieniTramiteIdAsync(id);

        if (utente == null)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
            {   
                IdUtente   = utenteId,
                NomeAzione = "Ricerca profilo",
                Effettuato = false,
                Messaggio  = "Ricerca fallita"
            });
            return NotFound(new { messaggio = "Utente non trovato." });
        }
        
        await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
            {   
                IdUtente   = utenteId,
                NomeAzione = "Ricerca profilo",
                Effettuato = true,
                Messaggio  = "Ricerca avvenuta"
            });
        return Ok(utente);
    }

    // -----------------------------------------------------
    // PUT: api/auth/modifica
    // Modifica il profilo dell’utente loggato
    // -----------------------------------------------------
    [HttpPut("modifica")]
    public async Task<IActionResult> Modifica([FromBody] DtoCreazioneUtente dto)
    {
        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var risultato = await _authService.ModificaAsync(dto, utenteId);

        if (risultato == null)
        {
            return NotFound(new { messaggio = "Utente non trovato." });
        }
        await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
            {   
                IdUtente   = utenteId,
                NomeAzione = "Modifica profilo",
                Effettuato = true,
                Messaggio  = "Modifica profilo avvenuta"
            });

        return Ok(risultato);
    }

    // -----------------------------------------------------
    // DELETE: api/auth/{id}
    // Solo Gestore/Operatore possono eliminare altri utenti
    // -----------------------------------------------------
     [HttpDelete("{id}")]
    [Authorize(Roles = Ruoli.GestoreOrOperatore)]
    public async Task<IActionResult> EliminaTramiteId(string Id)
    {
        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        var risultato = await _authService.EliminaAsync(Id);

        if (risultato == null)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
            {   
                IdUtente   = utenteId,
                NomeAzione = "Eliminazione profilo",
                Effettuato = false,
                Messaggio  = "Eliminazione profilo fallita"
            });

            return NotFound(new { messaggio = "Utente non trovato." });
        }
        await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
            {   
                IdUtente   = utenteId,
                NomeAzione = "Eliminazione profilo",
                Effettuato = true,
                Messaggio  = "Eliminazione profilo avvenuta"
            });
        return Ok(risultato);
    }

    // -----------------------------------------------------
    // DELETE: api/auth/elimina
    // L’utente elimina se stesso
    // -----------------------------------------------------
    [HttpDelete("elimina")]
    public async Task<IActionResult> Elimina()
    {
        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var risultato = await _authService.EliminaAsync(utenteId);

        if (risultato == null)
        {   
            return NotFound(new { messaggio = "Utente non trovato." });
        }
        
        await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
            {   
                IdUtente   = utenteId,
                NomeAzione = "Eliminazione profilo",
                Effettuato = true,
                Messaggio  = "Eliminazione profilo avvenuta"
            });
        return Ok(risultato);
    }
}
```

## FasceOrarieController.cs

```c#
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using NuovoCinemaParadiso.Dtos;
using NuovoCinemaParadiso.Services;

namespace NuovoCinemaParadiso.Controllers;

// Indica che questa classe è un controller API
[ApiController]
// Route base: api/FasciaOraria
[Route("api/[controller]")]
// Richiede autenticazione per tutti gli endpoint
[Authorize]
public class FasciaOrariaController : ControllerBase
{
    // Service per la gestione delle fasce orarie
    private readonly FasciaOrariaService _fasciaOrariaService;

    // Service per il logging delle azioni
    private readonly LogAzioniService _logAzioniService;

    // Costruttore con Dependency Injection
    public FasciaOrariaController(FasciaOrariaService fasciaOrariaService, LogAzioniService logAzioniService)
    {
        _fasciaOrariaService = fasciaOrariaService;
        _logAzioniService = logAzioniService;
    }

    // ========================= LETTURA =========================

    // Ottiene tutte le fasce orarie
    [HttpGet]
    public async Task<IActionResult> OttieniTutti()
    {
        // Recupera tutte le fasce orarie
        List<DtoFasciaOraria> fasceOrarie = await _fasciaOrariaService.OttieniTuttoAsync();

        // Recupera ID utente autenticato
        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        // Log operazione
        await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
        {
            IdUtente = utenteId,
            NomeAzione = "Ottieni tutte le fasce orarie",
            Effettuato = true,
            Messaggio = "Operazione eseguita"
        });

        return Ok(fasceOrarie);
    }

    // Ottiene una fascia oraria tramite ID
    [HttpGet("{id}")]
    public async Task<IActionResult> OttieniTramiteId(string id)
    {
        // Recupera la fascia oraria
        var risultato = await _fasciaOrariaService.OttieniTramiteIdAsync(id);

        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        // Se non trovata
        if (risultato == null)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
            {
                IdUtente = utenteId,
                NomeAzione = "Ottieni fascia oraria tramite id",
                Effettuato = false,
                Messaggio = "Operazione fallita"
            });

            // ⚠️ Nota: messaggio contiene "TipologiaSala" (probabile errore di copia)
            return NotFound($"TipologiaSala con id {id} non trovato");
        }

        // Log successo
        await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
        {
            IdUtente = utenteId,
            NomeAzione = "Ottieni fascia oraria tramite id",
            Effettuato = true,
            Messaggio = "Operazione eseguita"
        });

        return Ok(risultato);
    }

    // ========================= CREAZIONE =========================

    // Crea una nuova fascia oraria (solo admin/operatori)
    [HttpPost]
    [Authorize(Roles = Ruoli.GestoreOrOperatore)]
    public async Task<IActionResult> Creazione([FromBody] DtoCreazioneFasciaOraria dto)
    {
        // Crea la fascia oraria
        DtoFasciaOraria? risultato = await _fasciaOrariaService.CreazioneAsync(dto);

        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        // Se creazione fallita
        if (risultato == null)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
            {
                IdUtente = utenteId,
                NomeAzione = "Creazione fasce oraria",
                Effettuato = false,
                Messaggio = "Operazione fallita"
            });

            return BadRequest(new { messaggio = "Fascia oraria già presente oppure non valida." });
        }

        // Log successo
        await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
        {
            IdUtente = utenteId,
            NomeAzione = "Creazione fasce oraria",
            Effettuato = true,
            Messaggio = "Operazione eseguita"
        });

        return Ok(risultato);
    }

    // ========================= MODIFICA =========================

    // Modifica una fascia oraria (solo admin/operatori)
    [HttpPut("{id}")]
    [Authorize(Roles = Ruoli.GestoreOrOperatore)]
    public async Task<IActionResult> Modifica(string id, [FromBody] DtoCreazioneFasciaOraria dto)
    {
        // Modifica fascia oraria
        DtoFasciaOraria? risultato = await _fasciaOrariaService.ModificaAsync(id, dto);

        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        // Se non trovata
        if (risultato == null)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
            {
                IdUtente = utenteId,
                NomeAzione = "Modifica fasce oraria",
                Effettuato = false,
                Messaggio = "Operazione fallita"
            });

            return NotFound(new { messaggio = "Fascia oraria non trovata." });
        }

        // Log successo
        await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
        {
            IdUtente = utenteId,
            NomeAzione = "Modifica fasce oraria",
            Effettuato = true,
            Messaggio = "Operazione eseguita"
        });

        return Ok(risultato);
    }

    // ========================= ELIMINAZIONE =========================

    // Elimina una fascia oraria (solo admin/operatori)
    [HttpDelete("{id}")]
    [Authorize(Roles = Ruoli.GestoreOrOperatore)]
    public async Task<IActionResult> Elimina(string id)
    {
        // Elimina fascia oraria
        bool eliminato = await _fasciaOrariaService.EliminaAsync(id);

        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        // Se non trovata
        if (!eliminato)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
            {
                IdUtente = utenteId,
                NomeAzione = "Elimina fascia oraria",
                Effettuato = false,
                Messaggio = "Operazione fallita"
            });

            return NotFound(new { messaggio = "Fascia oraria non trovata." });
        }

        // Log successo
        await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
        {
            IdUtente = utenteId,
            NomeAzione = "Elimina fascia oraria",
            Effettuato = true,
            Messaggio = "Operazione eseguita"
        });

        return NoContent();
    }
}
```

## GeneriMoviesController.cs

```c#
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using NuovoCinemaParadiso.Dtos;
using NuovoCinemaParadiso.Services;

namespace NuovoCinemaParadiso.Controllers;

// Indica che questa classe è un controller API
[ApiController]
// Route base: api/GeneriMovies
[Route("api/[controller]")]
// Richiede autenticazione per tutti gli endpoint
[Authorize]
public class GeneriMoviesController : ControllerBase
{
    // Service per la gestione dei generi dei film
    private readonly GenereMovieService _genereMovieService;

    // Service per il logging delle azioni
    private readonly LogAzioniService _logAzioniService;

    // Costruttore con Dependency Injection
    public GeneriMoviesController(GenereMovieService genereMovieService, LogAzioniService logAzioniService)
    {
        _genereMovieService = genereMovieService;
        _logAzioniService = logAzioniService;
    }

    // ========================= LETTURA =========================

    // Ottiene tutti i generi dei film
    [HttpGet]
    public async Task<IActionResult> OttieniTutti()
    {
        // Recupera tutti i generi
        List<DtoGenereMovie> generiFilm = await _genereMovieService.OttieniTuttoAsync();

        // Recupera ID utente autenticato
        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        // Log operazione
        await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
        {
            IdUtente = utenteId,
            NomeAzione = "Ottieni tutti i generi",
            Effettuato = true,
            Messaggio = "Operazione eseguita"
        });

        return Ok(generiFilm);
    }

    // Ottiene un genere tramite ID
    [HttpGet("{id}")]
    public async Task<IActionResult> OttieniTramiteId(string id)
    {
        // Recupera il genere
        var risultato = await _genereMovieService.OttieniTramiteIdAsync(id);

        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        // Se non trovato
        if (risultato == null)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
            {
                IdUtente = utenteId,
                NomeAzione = "Ottieni genere tramite id",
                Effettuato = false,
                Messaggio = "Operazione fallita"
            });

            return NotFound($"GenereMovie con id {id} non trovato");
        }

        // Log successo
        await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
        {
            IdUtente = utenteId,
            NomeAzione = "Ottieni genere tramite id",
            Effettuato = true,
            Messaggio = "Operazione eseguita"
        });

        return Ok(risultato);
    }

    // ========================= CREAZIONE =========================

    // Crea un nuovo genere (solo admin/operatori)
    [HttpPost]
    [Authorize(Roles = Ruoli.GestoreOrOperatore)]
    public async Task<IActionResult> Creazione([FromBody] DtoCreazioneGenereMovie dto)
    {
        // Crea il genere
        DtoGenereMovie? risultato = await _genereMovieService.CreazioneAsync(dto);

        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        // Se creazione fallita
        if (risultato == null)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
            {
                IdUtente = utenteId,
                NomeAzione = "Creazione genere",
                Effettuato = false,
                Messaggio = "Operazione fallita"
            });

            return BadRequest(new { messaggio = "Genere già presente oppure non valido." });
        }

        // Log successo
        await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
        {
            IdUtente = utenteId,
            NomeAzione = "Creazione genere",
            Effettuato = true,
            Messaggio = "Operazione eseguita"
        });

        return Ok(risultato);
    }

    // ========================= MODIFICA =========================

    // Modifica un genere (solo admin/operatori)
    [HttpPut("{id}")]
    [Authorize(Roles = Ruoli.GestoreOrOperatore)]
    public async Task<IActionResult> Modifica(string id, [FromBody] DtoCreazioneGenereMovie dto)
    {
        // Modifica il genere
        DtoGenereMovie? risultato = await _genereMovieService.ModificaAsync(id, dto);

        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        // Se non trovato
        if (risultato == null)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
            {
                IdUtente = utenteId,
                NomeAzione = "Modifica genere",
                Effettuato = false,
                Messaggio = "Operazione fallita"
            });

            return NotFound(new { messaggio = "Genere non trovato." });
        }

        // Log successo
        await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
        {
            IdUtente = utenteId,
            NomeAzione = "Modifica genere",
            Effettuato = true,
            Messaggio = "Operazione eseguita"
        });

        return Ok(risultato);
    }

    // ========================= ELIMINAZIONE =========================

    // Elimina un genere (solo admin/operatori)
    [HttpDelete("{id}")]
    [Authorize(Roles = Ruoli.GestoreOrOperatore)]
    public async Task<IActionResult> Elimina(string id)
    {
        // Elimina il genere
        bool eliminato = await _genereMovieService.EliminaAsync(id);

        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        // Se non trovato
        if (!eliminato)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
            {
                IdUtente = utenteId,
                NomeAzione = "Elimina genere",
                Effettuato = false,
                Messaggio = "Operazione fallita"
            });

            return NotFound(new { messaggio = "Genere non trovato." });
        }

        // Log successo
        await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
        {
            IdUtente = utenteId,
            NomeAzione = "Elimina genere",
            Effettuato = true,
            Messaggio = "Operazione eseguita"
        });

        return NoContent();
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

namespace NuovoCinemaParadiso.Controllers;

// Indica che questa classe è un controller API
[ApiController]
// Route base: api/GestoreUtenti
[Route("api/[controller]")]
// Accesso consentito solo agli utenti con ruolo "Gestore"
[Authorize(Roles = Ruoli.Gestore)]
public class GestoreUtentiController : ControllerBase
{
    // Service per la gestione dei ruoli utente
    private readonly RuoloUtenteService _ruoloUtenteService;

    // Service per il logging delle azioni
    private readonly LogAzioniService _logAzioniService;

    // Costruttore con Dependency Injection
    public GestoreUtentiController(RuoloUtenteService ruoloUtenteService, LogAzioniService logAzioniService)
    {
        _ruoloUtenteService = ruoloUtenteService;
        _logAzioniService = logAzioniService;
    }

    // ========================= MODIFICA RUOLO =========================

    // Endpoint per cambiare il ruolo di un utente
    [HttpPut("cambia-ruolo")]
    public async Task<IActionResult> CambiaRuolo([FromBody] DtoModificaRuoloUtente dto)
    {
        // Chiama il service per modificare il ruolo
        // Restituisce il nuovo ruolo oppure null se fallisce
        string? nuovoRuolo = await _ruoloUtenteService.ModificaRuoloUtente(dto);

        // Recupera ID dell'utente autenticato (chi sta facendo l'azione)
        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        // Se operazione fallita
        if (nuovoRuolo == null)
        {
            // Log fallimento
            await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
            {
                IdUtente = utenteId,
                NomeAzione = "Cambio ruolo",
                Effettuato = false,
                Messaggio = "Operazione fallita"
            });

            return BadRequest(new { messaggio = "Utente o ruolo non valido." });
        }

        // Log successo
        await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
        {
            IdUtente = utenteId,
            NomeAzione = "Cambio ruolo",
            Effettuato = true,
            Messaggio = "Operazione eseguita"
        });

        // Risposta con dettagli aggiornati
        return Ok(new
        {
            messaggio = "Ruolo aggiornato correttamente.",
            email = dto.Email,
            ruolo = nuovoRuolo
        });
    }
}
```

## MoviesController.cs

```c#
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using NuovoCinemaParadiso.Services;
using NuovoCinemaParadiso.Dtos;

namespace NuovoCinemaParadiso.Controllers;

// Indica che questa classe è un controller API
[ApiController]
// Route base: api/Movies
[Route("api/[controller]")]
// Richiede autenticazione per tutti gli endpoint
[Authorize]
public class MoviesController : ControllerBase
{
    // Service per la gestione dei film
    private readonly MovieService _movieService;

    // Service per il logging delle azioni
    private readonly LogAzioniService _logAzioniService;

    // Costruttore con Dependency Injection
    public MoviesController(MovieService movieService, LogAzioniService logAzioniService)
    {
        _movieService = movieService;
        _logAzioniService = logAzioniService;
    }

    // ========================= LETTURA =========================

    // Ottiene tutti i film
    [HttpGet]
    public async Task<IActionResult> OttieniTuttiIMovies()
    {
        // Recupera tutti i film
        List<DtoMovie> movies = await _movieService.OttieniTutto();

        // Recupera ID utente autenticato
        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        // Log operazione
        await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
        {
            IdUtente = utenteId,
            NomeAzione = "Ottieni tutti i movies",
            Effettuato = true,
            Messaggio = "Operazione eseguita"
        });

        return Ok(movies);
    }

    // Ottiene i film filtrati per genere
    [HttpGet("genere/{genereId}")]
    public async Task<ActionResult<List<DtoMovie>>> OttieniPerGenere(string genereId)
    {
        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        // Validazione input
        if (string.IsNullOrWhiteSpace(genereId))
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
            {
                IdUtente = utenteId,
                NomeAzione = "Ottieni movies per genere",
                Effettuato = false,
                Messaggio = "Operazione fallita"
            });

            return BadRequest("GenereId non valido");
        }

        // Recupera film per genere
        var risultato = await _movieService.OttieniTramiteGenere(genereId);

        // Se nessun risultato
        if (risultato == null || risultato.Count == 0)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
            {
                IdUtente = utenteId,
                NomeAzione = "Ottieni movies per genere",
                Effettuato = false,
                Messaggio = "Operazione fallita"
            });

            return NotFound("Nessun film trovato per questo genere");
        }

        // Log successo
        await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
        {
            IdUtente = utenteId,
            NomeAzione = "Ottieni movies per genere",
            Effettuato = true,
            Messaggio = "Operazione eseguita"
        });

        return Ok(risultato);
    }

    // Ottiene un film tramite ID
    [HttpGet("{id}")]
    public async Task<IActionResult> OttieniTramiteId(string id)
    {
        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        // Recupera film
        var risultato = await _movieService.OttieniTramiteIdAsync(id);

        // Se non trovato
        if (risultato == null)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
            {
                IdUtente = utenteId,
                NomeAzione = "Ottieni movie tramite id",
                Effettuato = false,
                Messaggio = "Operazione fallita"
            });

            return NotFound($"Film con id {id} non trovato");
        }

        // Log successo
        await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
        {
            IdUtente = utenteId,
            NomeAzione = "Ottieni movie tramite id",
            Effettuato = true,
            Messaggio = "Operazione eseguita"
        });

        return Ok(risultato);
    }

    // ========================= CREAZIONE =========================

    // Crea un nuovo film (solo admin/operatori)
    [HttpPost]
    [Authorize(Roles = Ruoli.GestoreOrOperatore)]
    public async Task<IActionResult> Creazione([FromBody] DtoCreazioneMovie dto)
    {
        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        // Crea il film
        DtoMovie? risultato = await _movieService.CreazioneAsync(dto);

        // Se fallisce
        if (risultato == null)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
            {
                IdUtente = utenteId,
                NomeAzione = "Creazione movie",
                Effettuato = false,
                Messaggio = "Operazione fallita"
            });

            return BadRequest(new { messaggio = "Film già presente oppure non valido." });
        }

        // Log successo
        await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
        {
            IdUtente = utenteId,
            NomeAzione = "Creazione movie",
            Effettuato = true,
            Messaggio = "Operazione eseguita"
        });

        return Ok(risultato);
    }

    // ========================= MODIFICA =========================

    // Modifica un film (solo admin/operatori)
    [HttpPut("{id}")]
    [Authorize(Roles = Ruoli.GestoreOrOperatore)]
    public async Task<IActionResult> Modifica(string id, [FromBody] DtoCreazioneMovie dto)
    {
        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        // Modifica film
        DtoMovie? risultato = await _movieService.ModificaAsync(id, dto);

        // Se non trovato
        if (risultato == null)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
            {
                IdUtente = utenteId,
                NomeAzione = "Modifica movie",
                Effettuato = false,
                Messaggio = "Operazione fallita"
            });

            return NotFound(new { messaggio = "Film non trovato." });
        }

        // Log successo
        await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
        {
            IdUtente = utenteId,
            NomeAzione = "Modifica movie",
            Effettuato = true,
            Messaggio = "Operazione eseguita"
        });

        return Ok(risultato);
    }

    // ========================= ELIMINAZIONE =========================

    // Elimina un film (solo admin/operatori)
    [HttpDelete("{id}")]
    [Authorize(Roles = Ruoli.GestoreOrOperatore)]
    public async Task<IActionResult> Elimina(string id)
    {
        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        // Elimina film
        bool eliminato = await _movieService.EliminaAsync(id);

        // Se non trovato
        if (!eliminato)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
            {
                IdUtente = utenteId,
                NomeAzione = "Elimina movie",
                Effettuato = false,
                Messaggio = "Operazione fallita"
            });

            return NotFound(new { messaggio = "Film non trovato." });
        }

        // Log successo
        await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
        {
            IdUtente = utenteId,
            NomeAzione = "Elimina movie",
            Effettuato = true,
            Messaggio = "Operazione eseguita"
        });

        return NoContent();
    }
}
```

## SaleController.cs

```c#
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using NuovoCinemaParadiso.Services;
using NuovoCinemaParadiso.Dtos;

namespace NuovoCinemaParadiso.Controllers;

// Indica che questa classe è un controller API
[ApiController]
// Route base: api/Sale
[Route("api/[controller]")]
// Richiede autenticazione per tutti gli endpoint
[Authorize]
public class SaleController : ControllerBase
{
    // Service per la gestione delle sale
    private readonly SalaService _salaService;

    // Service per il logging delle azioni
    private readonly LogAzioniService _logAzioniService;

    // Costruttore con Dependency Injection
    public SaleController(SalaService salaService, LogAzioniService logAzioniService)
    {
        _salaService = salaService;
        _logAzioniService = logAzioniService;
    }

    // ========================= LETTURA =========================

    // Ottiene tutte le sale
    [HttpGet]
    public async Task<IActionResult> OttieniTutti()
    {
        List<DtoSala> sale = await _salaService.OttieniTuttoAsync();

        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        // Log operazione
        await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
        {
            IdUtente = utenteId,
            NomeAzione = "Ottieni tutte le sale",
            Effettuato = true,
            Messaggio = "Operazione eseguita"
        });

        return Ok(sale);
    }

    // Ottiene sale filtrate per tipologia
    [HttpGet("tipologia/{tipologiaId}")]
    public async Task<ActionResult<List<DtoSala>>> OttieniPerTipologia(string tipologiaId)
    {
        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        // Validazione input
        if (string.IsNullOrWhiteSpace(tipologiaId))
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
            {
                IdUtente = utenteId,
                NomeAzione = "Ottieni le sale per tipologia",
                Effettuato = false,
                Messaggio = "Operazione fallita"
            });

            return BadRequest("TipologiaId non valido");
        }

        // Recupera sale per tipologia
        var risultato = await _salaService.OttieniTramiteTipologiaAsync(tipologiaId);

        // Se nessun risultato
        if (risultato == null || risultato.Count == 0)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
            {
                IdUtente = utenteId,
                NomeAzione = "Ottieni le sale per tipologia",
                Effettuato = false,
                Messaggio = "Operazione fallita"
            });

            return NotFound("Nessuna sala trovata per questa tipologia");
        }

        // Log successo
        await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
        {
            IdUtente = utenteId,
            NomeAzione = "Ottieni le sale per tipologia",
            Effettuato = true,
            Messaggio = "Operazione eseguita"
        });

        return Ok(risultato);
    }

    // Ottiene sale filtrate per fascia oraria
    [HttpGet("fascia-oraria/{fasciaOrariaId}")]
    public async Task<ActionResult<List<DtoSala>>> OttieniPerFasciaOraria(string fasciaOrariaId)
    {
        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        // Validazione input
        if (string.IsNullOrWhiteSpace(fasciaOrariaId))
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
            {
                IdUtente = utenteId,
                NomeAzione = "Ottieni le sale per fascia oraria",
                Effettuato = false,
                Messaggio = "Operazione fallita"
            });

            return BadRequest("FasciaOrariaId non valido");
        }

        // Recupera sale per fascia oraria
        var risultato = await _salaService.OttieniTramiteFasciaOrariaAsync(fasciaOrariaId);

        if (risultato == null || risultato.Count == 0)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
            {
                IdUtente = utenteId,
                NomeAzione = "Ottieni le sale per fascia oraria",
                Effettuato = false,
                Messaggio = "Operazione fallita"
            });

            return NotFound("Nessuna sala trovata per questa fascia oraria");
        }

        // Log successo
        await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
        {
            IdUtente = utenteId,
            NomeAzione = "Ottieni le sale per fascia oraria",
            Effettuato = true,
            Messaggio = "Operazione eseguita"
        });

        return Ok(risultato);
    }

    // Ottiene una sala tramite ID
    [HttpGet("{id}")]
    public async Task<IActionResult> OttieniTramiteId(string id)
    {
        var risultato = await _salaService.OttieniTramiteIdAsync(id);

        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        // Se non trovata
        if (risultato == null)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
            {
                IdUtente = utenteId,
                NomeAzione = "Ottieni sala tramite id",
                Effettuato = false,
                Messaggio = "Operazione fallita"
            });

            return NotFound($"Sala con id {id} non trovato");
        }

        // Log successo
        await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
        {
            IdUtente = utenteId,
            NomeAzione = "Ottieni sala tramite id",
            Effettuato = true,
            Messaggio = "Operazione eseguita"
        });

        return Ok(risultato);
    }

    // ========================= CREAZIONE =========================

    // Crea una nuova sala (solo admin/operatori)
    [HttpPost]
    [Authorize(Roles = Ruoli.GestoreOrOperatore)]
    public async Task<IActionResult> Creazione([FromBody] DtoCreazioneSala dto)
    {
        // Crea la sala
        DtoSala? risultato = await _salaService.CreazioneAsync(dto);

        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        // Se fallisce
        if (risultato == null)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
            {
                IdUtente = utenteId,
                NomeAzione = "Creazione sala",
                Effettuato = false,
                Messaggio = "Operazione fallita"
            });

            return BadRequest(new { messaggio = "Sala già presente oppure non valida." });
        }

        // Log successo
        await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
        {
            IdUtente = utenteId,
            NomeAzione = "Creazione sala",
            Effettuato = true,
            Messaggio = "Operazione eseguita"
        });

        return Ok(risultato);
    }

    // ========================= MODIFICA =========================

    // Modifica una sala (solo admin/operatori)
    [HttpPut("{id}")]
    [Authorize(Roles = Ruoli.GestoreOrOperatore)]
    public async Task<IActionResult> Modifica(string id, [FromBody] DtoCreazioneSala dto)
    {
        DtoSala? risultato = await _salaService.ModificaAsync(id, dto);

        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        // Se non trovata
        if (risultato == null)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
            {
                IdUtente = utenteId,
                NomeAzione = "Modifica sala",
                Effettuato = false,
                Messaggio = "Operazione fallita"
            });

            return NotFound(new { messaggio = "Sala non trovata." });
        }

        // Log successo
        await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
        {
            IdUtente = utenteId,
            NomeAzione = "Modifica sala",
            Effettuato = true,
            Messaggio = "Operazione eseguita"
        });

        return Ok(risultato);
    }

    // ========================= ELIMINAZIONE =========================

    // Elimina una sala (solo admin/operatori)
    [HttpDelete("{id}")]
    [Authorize(Roles = Ruoli.GestoreOrOperatore)]
    public async Task<IActionResult> Elimina(string id)
    {
        bool eliminato = await _salaService.EliminaAsync(id);

        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        // Se non trovata
        if (!eliminato)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
            {
                IdUtente = utenteId,
                NomeAzione = "Elimina sala",
                Effettuato = false,
                Messaggio = "Operazione fallita"
            });

            return NotFound(new { messaggio = "Sala non trovata." });
        }

        // Log successo
        await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
        {
            IdUtente = utenteId,
            NomeAzione = "Elimina sala",
            Effettuato = true,
            Messaggio = "Operazione eseguita"
        });

        return NoContent();
    }
}
```

## TipologieSalaController.cs

```c#
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using NuovoCinemaParadiso.Services;
using NuovoCinemaParadiso.Dtos;

namespace NuovoCinemaParadiso.Controllers;

// Controller API per la gestione delle tipologie di sala
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TipologieSalaController : ControllerBase
{
    // Service per la gestione delle tipologie sala
    private readonly TipologiaSalaService _tipologiaSalaService;

    // Service per il logging delle azioni
    private readonly LogAzioniService _logAzioniService;

    // Costruttore con Dependency Injection
    public TipologieSalaController(
        TipologiaSalaService tipologiaSalaService,
        LogAzioniService logAzioniService)
    {
        _tipologiaSalaService = tipologiaSalaService;
        _logAzioniService = logAzioniService;
    }

    // ========================= LETTURA =========================

    // Ottiene tutte le tipologie di sala
    [HttpGet]
    public async Task<IActionResult> OttieniTutti()
    {
        List<DtoTipologiaSala> tipologieSala = await _tipologiaSalaService.OttieniTuttoAsync();

        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        // Log operazione
        await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
        {
            IdUtente = utenteId,
            NomeAzione = "Ottieni tutte le tipologie",
            Effettuato = true,
            Messaggio = "Operazione eseguita"
        });

        return Ok(tipologieSala);
    }

    // Ottiene una tipologia sala tramite ID
    [HttpGet("{id}")]
    public async Task<IActionResult> OttieniTramiteId(string id)
    {
        var risultato = await _tipologiaSalaService.OttieniTramiteIdAsync(id);

        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        // Se non trovata
        if (risultato == null)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
            {
                IdUtente = utenteId,
                NomeAzione = "Ottieni tipologia tramite id",
                Effettuato = false,
                Messaggio = "Operazione fallita"
            });

            return NotFound($"TipologiaSala con id {id} non trovato");
        }

        // Log successo
        await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
        {
            IdUtente = utenteId,
            NomeAzione = "Ottieni tipologia tramite id",
            Effettuato = true,
            Messaggio = "Operazione eseguita"
        });

        return Ok(risultato);
    }

    // ========================= CREAZIONE =========================

    // Crea una nuova tipologia sala (solo Gestore o Operatore)
    [HttpPost]
    [Authorize(Roles = Ruoli.GestoreOrOperatore)]
    public async Task<IActionResult> Creazione([FromBody] DtoCreazioneTipologiaSala dto)
    {
        DtoTipologiaSala? risultato = await _tipologiaSalaService.CreazioneAsync(dto);

        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        // Se fallisce la creazione
        if (risultato == null)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
            {
                IdUtente = utenteId,
                NomeAzione = "Creazione tipologia",
                Effettuato = false,
                Messaggio = "Operazione fallita"
            });

            return BadRequest(new
            {
                messaggio = "Tipologia sala già presente oppure non valida."
            });
        }

        // Log successo
        await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
        {
            IdUtente = utenteId,
            NomeAzione = "Creazione tipologia",
            Effettuato = true,
            Messaggio = "Operazione eseguita"
        });

        return Ok(risultato);
    }

    // ========================= MODIFICA =========================

    // Modifica una tipologia sala (solo Gestore o Operatore)
    [HttpPut("{id}")]
    [Authorize(Roles = Ruoli.GestoreOrOperatore)]
    public async Task<IActionResult> Modifica(string id, [FromBody] DtoCreazioneTipologiaSala dto)
    {
        DtoTipologiaSala? risultato = await _tipologiaSalaService.ModificaAsync(id, dto);

        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        // Se non trovata
        if (risultato == null)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
            {
                IdUtente = utenteId,
                NomeAzione = "Modifica tipologia",
                Effettuato = false,
                Messaggio = "Operazione fallita"
            });

            return NotFound(new { messaggio = "Tipologia sala non trovata." });
        }

        // Log successo
        await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
        {
            IdUtente = utenteId,
            NomeAzione = "Modifica tipologia",
            Effettuato = true,
            Messaggio = "Operazione eseguita"
        });

        return Ok(risultato);
    }

    // ========================= ELIMINAZIONE =========================

    // Elimina una tipologia sala (solo Gestore o Operatore)
    [HttpDelete("{id}")]
    [Authorize(Roles = Ruoli.GestoreOrOperatore)]
    public async Task<IActionResult> Elimina(string id)
    {
        bool eliminato = await _tipologiaSalaService.EliminaAsync(id);

        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        // Se non trovata
        if (!eliminato)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
            {
                IdUtente = utenteId,
                NomeAzione = "Elimina tipologia",
                Effettuato = false,
                Messaggio = "Operazione fallita"
            });

            return NotFound(new { messaggio = "Tipologia sala non trovata." });
        }

        // Log successo
        await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
        {
            IdUtente = utenteId,
            NomeAzione = "Elimina tipologia",
            Effettuato = true,
            Messaggio = "Operazione eseguita"
        });

        return NoContent();
    }
}
```

# Services

## AcquistoService

```c#
// Service che gestisce la logica degli acquisti (CRUD + DTO mapping)
public class AcquistoService
{
    private readonly ContestoDb _contesto;

    public AcquistoService(ContestoDb contesto)
    {
        _contesto = contesto;
    }

    // ================= ADMIN: tutti gli acquisti =================
    public async Task<List<DtoAcquisto>> OttieniTuttoAdmin()
    {
        List<Acquisto> acquisti = await _contesto.Acquisti.ToListAsync();

        List<DtoAcquisto> risultato = new();

        for (int i = 0; i < acquisti.Count; i++)
        {
            Acquisto acquistoCorrente = acquisti[i];

            // ⚠️ Query N+1: per ogni acquisto fai più query al DB
            Movie? movie = await _contesto.Movies.FindAsync(acquistoCorrente.MovieId);
            Sala? sala = await _contesto.Sale.FindAsync(acquistoCorrente.SalaId);
            Utente? utente = await _contesto.Utenti.FindAsync(acquistoCorrente.UtenteId);
            TipologiaSala? tipologiaSala = await _contesto.TipologieSala.FindAsync(sala.TipologiaSalaId);

            DtoAcquisto dto = new()
            {
                Id = acquistoCorrente.Id,
                MovieId = acquistoCorrente.MovieId,
                Titolo = movie.Titolo,
                SalaId = acquistoCorrente.SalaId,
                Nome = sala.Nome,
                UtenteId = acquistoCorrente.UtenteId,
                NomeCompleto = utente.NomeCompleto,
                PrezzoFinale = CalcolaPrezzo.CalcolaPrezzoFinale(
                    movie.PrezzoMovie,
                    tipologiaSala.MaggiorazionePrezzo,
                    acquistoCorrente.NumeroBiglietti),
                OrarioCreazione = acquistoCorrente.OrarioCreazione,
                NumeroBiglietti = acquistoCorrente.NumeroBiglietti
            };

            risultato.Add(dto);
        }

        return risultato;
    }

    // ================= UTENTE: propri acquisti =================
    public async Task<List<DtoAcquisto>> OttieniTutto(string utenteId)
    {
        List<Acquisto> acquisti = await _contesto.Acquisti.ToListAsync();

        List<DtoAcquisto> risultato = new();

        foreach (Acquisto acquistoCorrente in acquisti)
        {
            if (acquistoCorrente.UtenteId != utenteId)
                continue;

            Movie? movie = await _contesto.Movies.FindAsync(acquistoCorrente.MovieId);
            Sala? sala = await _contesto.Sale.FindAsync(acquistoCorrente.SalaId);
            TipologiaSala? tipologiaSala = await _contesto.TipologieSala.FindAsync(sala.TipologiaSalaId);

            risultato.Add(new DtoAcquisto
            {
                Id = acquistoCorrente.Id,
                MovieId = acquistoCorrente.MovieId,
                Titolo = movie.Titolo,
                SalaId = acquistoCorrente.SalaId,
                Nome = sala.Nome,
                PrezzoFinale = CalcolaPrezzo.CalcolaPrezzoFinale(
                    movie.PrezzoMovie,
                    tipologiaSala.MaggiorazionePrezzo,
                    acquistoCorrente.NumeroBiglietti),
                OrarioCreazione = acquistoCorrente.OrarioCreazione,
                NumeroBiglietti = acquistoCorrente.NumeroBiglietti
            });
        }

        return risultato;
    }

    // ================= ADMIN: acquisto per ID =================
    public async Task<DtoAcquisto> OttieniTramiteIdPerAdminAsync(string id)
    {
        Acquisto? acquisto = await _contesto.Acquisti.FindAsync(id);

        if (acquisto == null)
            return null;

        Movie? movie = await _contesto.Movies.FindAsync(acquisto.MovieId);
        Sala? sala = await _contesto.Sale.FindAsync(acquisto.SalaId);
        Utente? utente = await _contesto.Users.FindAsync(acquisto.UtenteId);
        TipologiaSala? tipologiaSala = await _contesto.TipologieSala.FindAsync(sala.TipologiaSalaId);

        return new DtoAcquisto
        {
            Id = acquisto.Id,
            MovieId = acquisto.MovieId,
            Titolo = movie.Titolo,
            SalaId = acquisto.SalaId,
            Nome = sala.Nome,
            UtenteId = acquisto.UtenteId,
            NomeCompleto = utente.NomeCompleto,
            PrezzoFinale = CalcolaPrezzo.CalcolaPrezzoFinale(
                movie.PrezzoMovie,
                tipologiaSala.MaggiorazionePrezzo,
                acquisto.NumeroBiglietti),
            OrarioCreazione = acquisto.OrarioCreazione,
            NumeroBiglietti = acquisto.NumeroBiglietti
        };
    }

    // ================= UTENTE: acquisto singolo (controllo ownership) =================
    public async Task<DtoAcquisto> OttieniTramiteIdAsync(string id, string utenteId)
    {
        Acquisto? acquisto = await _contesto.Acquisti.FindAsync(id);

        if (acquisto == null || acquisto.UtenteId != utenteId)
            return null;

        Movie? movie = await _contesto.Movies.FindAsync(acquisto.MovieId);
        Sala? sala = await _contesto.Sale.FindAsync(acquisto.SalaId);
        TipologiaSala? tipologiaSala = await _contesto.TipologieSala.FindAsync(sala.TipologiaSalaId);

        return new DtoAcquisto
        {
            Id = acquisto.Id,
            MovieId = acquisto.MovieId,
            Titolo = movie.Titolo,
            SalaId = acquisto.SalaId,
            Nome = sala.Nome,
            PrezzoFinale = CalcolaPrezzo.CalcolaPrezzoFinale(
                movie.PrezzoMovie,
                tipologiaSala.MaggiorazionePrezzo,
                acquisto.NumeroBiglietti),
            OrarioCreazione = acquisto.OrarioCreazione,
            NumeroBiglietti = acquisto.NumeroBiglietti
        };
    }

    // ================= CREAZIONE =================
    public async Task<DtoAcquisto> CreazioneAsync(DtoCreazioneAcquisto dto, string utenteId)
    {
        // ⚠️ inefficiente: carica TUTTI gli acquisti
        List<Acquisto> tuttiGliAcquisti = await _contesto.Acquisti.ToListAsync();

        // controllo duplicati
        foreach (var a in tuttiGliAcquisti)
        {
            if (a.UtenteId == utenteId && a.MovieId == dto.MovieId && a.SalaId == dto.SalaId)
                return null;
        }

        Movie movie = await _contesto.Movies.FindAsync(dto.MovieId);
        Sala sala = await _contesto.Sale.FindAsync(dto.SalaId);
        TipologiaSala tipologiaSala = await _contesto.TipologieSala.FindAsync(sala.TipologiaSalaId);
        Utente utente = await _contesto.Utenti.FindAsync(utenteId);

        decimal prezzoFinale = CalcolaPrezzo.CalcolaPrezzoFinale(
            movie.PrezzoMovie,
            tipologiaSala.MaggiorazionePrezzo,
            dto.NumeroBiglietti);

        Acquisto acquisto = new()
        {
            MovieId = dto.MovieId,
            SalaId = dto.SalaId,
            UtenteId = utenteId,
            NumeroBiglietti = dto.NumeroBiglietti,
            PrezzoFinale = prezzoFinale,
            OrarioCreazione = DateTimeOffset.UtcNow
        };

        _contesto.Acquisti.Add(acquisto);
        await _contesto.SaveChangesAsync();

        return new DtoAcquisto
        {
            Id = acquisto.Id,
            MovieId = acquisto.MovieId,
            SalaId = acquisto.SalaId,
            UtenteId = acquisto.UtenteId,
            NumeroBiglietti = acquisto.NumeroBiglietti,
            PrezzoFinale = acquisto.PrezzoFinale,
            Titolo = movie.Titolo,
            Nome = sala.Nome,
            NomeCompleto = utente.NomeCompleto,
            OrarioCreazione = acquisto.OrarioCreazione
        };
    }

    // ================= UPDATE =================
    public async Task<DtoAcquisto?> ModificaAsync(string id, DtoCreazioneAcquisto dto)
    {
        Acquisto? acquisto = await _contesto.Acquisti.FindAsync(id);

        if (acquisto == null)
            return null;

        acquisto.MovieId = dto.MovieId;
        acquisto.SalaId = dto.SalaId;
        acquisto.NumeroBiglietti = dto.NumeroBiglietti;

        Movie? movie = await _contesto.Movies.FindAsync(dto.MovieId);
        Sala? sala = await _contesto.Sale.FindAsync(dto.SalaId);
        TipologiaSala? tipologiaSala = await _contesto.TipologieSala.FindAsync(sala.TipologiaSalaId);

        acquisto.PrezzoFinale = CalcolaPrezzo.CalcolaPrezzoFinale(
            movie.PrezzoMovie,
            tipologiaSala.MaggiorazionePrezzo,
            acquisto.NumeroBiglietti);

        await _contesto.SaveChangesAsync();

        return new DtoAcquisto
        {
            Id = acquisto.Id,
            MovieId = acquisto.MovieId,
            SalaId = acquisto.SalaId,
            UtenteId = acquisto.UtenteId,
            NumeroBiglietti = acquisto.NumeroBiglietti,
            PrezzoFinale = acquisto.PrezzoFinale,
            Titolo = movie?.Titolo,
            Nome = sala?.Nome
        };
    }

    // ================= DELETE =================
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

## AuthServices.cs

```c#
using Microsoft.AspNetCore.Identity;
using NuovoCinemaParadiso.Models;
using NuovoCinemaParadiso.Dtos;
using NuovoCinemaParadiso.Helpers;

namespace NuovoCinemaParadiso.Services;

public class AuthService
{
    private readonly UserManager<Utente> _gestioneUtenti;
    private readonly SignInManager<Utente> _gestioneAccesso;
    private readonly JwtHelper _jwtHelper;

    public AuthService(
        UserManager<Utente> gestioneUtenti,
        SignInManager<Utente> gestioneAccesso,
        JwtHelper jwtHelper)
    {
        _gestioneUtenti = gestioneUtenti;
        _gestioneAccesso = gestioneAccesso;
        _jwtHelper = jwtHelper;
    }

    // -----------------------------------------------------
    // REGISTRAZIONE UTENTE
    // -----------------------------------------------------
    public async Task<IdentityResult> RegistrazioneAsync(DtoRegistrazione dto)
    {
        // Controllo se l'email è già registrata
        Utente? esisteUtente = await _gestioneUtenti.FindByEmailAsync(dto.Email);

        if (esisteUtente != null)
        {
            IdentityError errore = new IdentityError
            {
                Description = "Utente già registrato."
            };

            return IdentityResult.Failed(errore);
        }

        // Creazione nuovo utente
        Utente utente = new Utente
        {
            UserName = dto.Email,
            Email = dto.Email,
            NomeCompleto = dto.NomeCompleto,
            Eta = dto.Eta
        };

        IdentityResult risultato = await _gestioneUtenti.CreateAsync(utente, dto.Password);

        if (!risultato.Succeeded)
            return risultato;

        // Assegna ruolo "Utente" di default
        IdentityResult aggiuntaRisultatoRuolo =
            await _gestioneUtenti.AddToRoleAsync(utente, Ruoli.Utente);

        if (!aggiuntaRisultatoRuolo.Succeeded)
            return aggiuntaRisultatoRuolo;

        return risultato;
    }

    // -----------------------------------------------------
    // LOGIN UTENTE
    // -----------------------------------------------------
    public async Task<DtoAuthResponse?> LoginAsync(DtoLogin dto)
    {
        Utente? utente = await _gestioneUtenti.FindByEmailAsync(dto.Email);

        if (utente == null)
            return null;

        // Verifica password
        SignInResult result = await _gestioneAccesso.CheckPasswordSignInAsync(
            utente, dto.Password, false);

        if (!result.Succeeded)
            return null;

        // Recupera ruoli
        IList<string> ruoli = await _gestioneUtenti.GetRolesAsync(utente);

        // Genera token JWT
        string token = _jwtHelper.GenerateToken(utente, ruoli);

        // Costruisce risposta
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
    
    // -----------------------------------------------------
    // TUTTI I PROFILI
    // -----------------------------------------------------

    public async Task<List<DtoUtente>> OttieniTuttoAsync()
    {
        List<Utente> utenti = await _contesto.Utenti.ToListAsync();

        List<DtoUtente> risultato = new List<DtoUtente>();

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
    // -----------------------------------------------------
    // PROFILO UTENTE TRAMITE ID
    // -----------------------------------------------------
    public async Task<DtoUtente?> OttieniTramiteId(string id)
    {
        Utente? utente = await _gestioneUtenti.FindByIdAsync(id);

        if (utente == null)
            return null;

        return new DtoUtente
        {
            Id = utente.Id,
            Email = utente.Email ?? string.Empty,
            NomeCompleto = utente.NomeCompleto ?? string.Empty,
            Eta = utente.Eta
        };
    }

    // -----------------------------------------------------
    // MODIFICA PROFILO UTENTE
    // -----------------------------------------------------
    public async Task<IdentityResult> ModificaAsync(DtoCreazioneUtente dto, string idUtente)
    {
        Utente? utente = await _gestioneUtenti.FindByIdAsync(idUtente);

        if (utente == null)
        {
            IdentityError error = new IdentityError
            {
                Description = "Utente non trovato."
            };

            return IdentityResult.Failed(error);
        }

        utente.NomeCompleto = dto.NomeCompleto;
        utente.Eta = dto.Eta;

        return await _gestioneUtenti.UpdateAsync(utente);
    }

    // -----------------------------------------------------
    // ELIMINA UTENTE (PER SE STESSO O PER ADMIN)
    // -----------------------------------------------------
    public async Task<IdentityResult> EliminaAsync(string userId)
    {
        Utente? utente = await _gestioneUtenti.FindByIdAsync(userId);

        if (utente == null)
        {
            IdentityError errore = new IdentityError
            {
                Description = "Utente non trovato."
            };

            return IdentityResult.Failed(errore);
        }

        return await _gestioneUtenti.DeleteAsync(utente);
    }

    // -----------------------------------------------------
    // ELIMINA UTENTE TRAMITE ID (duplicato del metodo sopra)
    // -----------------------------------------------------
    public async Task<IdentityResult> EliminaPerIdAsync(string id)
    {
        Utente? utente = await _gestioneUtenti.FindByIdAsync(id);

        if (utente == null)
        {
            IdentityError errore = new IdentityError
            {
                Description = "Utente non trovato."
            };

            return IdentityResult.Failed(errore);
        }

        return await _gestioneUtenti.DeleteAsync(utente);
    }
}
```

## FasciaOrariaService.cs

```c#
using Microsoft.EntityFrameworkCore;
using NuovoCinemaParadiso.Data;
using NuovoCinemaParadiso.Dtos;
using NuovoCinemaParadiso.Models;

namespace NuovoCinemaParadiso.Services
{
    // Service che gestisce CRUD per le fasce orarie
    public class FasciaOrariaService
    {
        private readonly ContestoDb _contesto;

        public FasciaOrariaService(ContestoDb contesto)
        {
            _contesto = contesto;
        }

        // -----------------------------------------------------
        // OTTIENI TUTTE LE FASCE ORARIE
        // -----------------------------------------------------
        public async Task<List<DtoFasciaOraria>> OttieniTuttoAsync()
        {
            List<FasciaOraria> fasceOrarie = await _contesto.FasceOrarie.ToListAsync();

            List<DtoFasciaOraria> risultato = new List<DtoFasciaOraria>();

            // Mapping manuale verso DTO
            for (int i = 0; i < fasceOrarie.Count; i++)
            {
                FasciaOraria fasciaCorrente = fasceOrarie[i];

                DtoFasciaOraria dto = new DtoFasciaOraria();
                dto.Id = fasciaCorrente.Id;
                dto.OraInizio = fasciaCorrente.OraInizio;
                dto.OraFine = fasciaCorrente.OraFine;
                dto.Nome = fasciaCorrente.Nome;

                risultato.Add(dto);
            }

            return risultato;
        }

        // -----------------------------------------------------
        // OTTIENI FASCIA ORARIA TRAMITE ID
        // -----------------------------------------------------
        public async Task<DtoFasciaOraria> OttieniTramiteIdAsync(string id) 
        {
            FasciaOraria? fasciaOraria = await _contesto.FasceOrarie.FindAsync(id);

            if (fasciaOraria == null)
                return null;

            DtoFasciaOraria dto = new DtoFasciaOraria();
            dto.Id = fasciaOraria.Id;
            dto.Nome = fasciaOraria.Nome;
            dto.OraInizio = fasciaOraria.OraInizio;
            dto.OraFine = fasciaOraria.OraFine;

            return dto;
        }

        // -----------------------------------------------------
        // CREA UNA NUOVA FASCIA ORARIA
        // -----------------------------------------------------
        public async Task<DtoFasciaOraria> CreazioneAsync(DtoCreazioneFasciaOraria dto)
        {
            FasciaOraria fasciaOraria = new FasciaOraria();
            fasciaOraria.Nome = dto.Nome;
            fasciaOraria.OraInizio = dto.OraInizio;
            fasciaOraria.OraFine = dto.OraFine;

            _contesto.FasceOrarie.Add(fasciaOraria);
            await _contesto.SaveChangesAsync();

            DtoFasciaOraria risultato = new DtoFasciaOraria();
            risultato.Id = fasciaOraria.Id;
            risultato.Nome = fasciaOraria.Nome;
            risultato.OraInizio = fasciaOraria.OraInizio;
            risultato.OraFine = fasciaOraria.OraFine;

            return risultato;
        }

        // -----------------------------------------------------
        // MODIFICA FASCIA ORARIA ESISTENTE
        // -----------------------------------------------------
        public async Task<DtoFasciaOraria?> ModificaAsync(string id, DtoCreazioneFasciaOraria dto)
        {
            FasciaOraria? fasciaEsistente = await _contesto.FasceOrarie.FindAsync(id);

            if (fasciaEsistente == null)
                return null;

            fasciaEsistente.OraInizio = dto.OraInizio;
            fasciaEsistente.OraFine = dto.OraFine;
            fasciaEsistente.Nome = dto.Nome;

            await _contesto.SaveChangesAsync();

            DtoFasciaOraria risultato = new DtoFasciaOraria();
            risultato.Id = fasciaEsistente.Id;
            risultato.OraInizio = fasciaEsistente.OraInizio;
            risultato.OraFine = fasciaEsistente.OraFine;
            risultato.Nome = fasciaEsistente.Nome;

            return risultato;
        }

        // -----------------------------------------------------
        // ELIMINA FASCIA ORARIA
        // -----------------------------------------------------
        public async Task<bool> EliminaAsync(string id) 
        {
            FasciaOraria? fasciaOraria = await _contesto.FasceOrarie.FindAsync(id);

            if (fasciaOraria == null)
                return false;

            _contesto.FasceOrarie.Remove(fasciaOraria);
            await _contesto.SaveChangesAsync();

            return true;
        }
    }
}
```

## GenereMovieService.cs 

```c#
using Microsoft.EntityFrameworkCore;
using NuovoCinemaParadiso.Dtos;
using NuovoCinemaParadiso.Data;
using NuovoCinemaParadiso.Models;

namespace NuovoCinemaParadiso.Services;

public class GenereMovieService
{
    private readonly ContestoDb _contesto;

    public GenereMovieService(ContestoDb contesto)
    {
        _contesto = contesto;
    }

    // -----------------------------------------------------
    // OTTIENI TUTTI I GENERI
    // -----------------------------------------------------
    public async Task<List<DtoGenereMovie>> OttieniTuttoAsync()
    {
        List<DtoGenereMovie> risultato = new List<DtoGenereMovie>();

        // Recupera tutti i generi dal DB
        List<GenereMovie> generiMovies = await _contesto.GeneriMovies.ToListAsync();

        // Mapping manuale verso DTO
        for (int i = 0; i < generiMovies.Count; i++)
        {
            GenereMovie genereCorrente = generiMovies[i];

            DtoGenereMovie dto = new DtoGenereMovie();
            dto.Id = genereCorrente.Id;
            dto.Genere = genereCorrente.Genere;

            risultato.Add(dto);
        }

        return risultato;
    }

    // -----------------------------------------------------
    // OTTIENI GENERE TRAMITE ID
    // -----------------------------------------------------
    public async Task<DtoGenereMovie?> OttieniTramiteIdAsync(string id)
    {
        GenereMovie? genereMovie = await _contesto.GeneriMovies.FindAsync(id);

        if (genereMovie == null)
            return null;

        DtoGenereMovie risultato = new DtoGenereMovie();
        risultato.Id = genereMovie.Id;
        risultato.Genere = genereMovie.Genere;

        return risultato;
    }

    // -----------------------------------------------------
    // CREA UN NUOVO GENERE
    // -----------------------------------------------------
    public async Task<DtoGenereMovie?> CreazioneAsync(DtoCreazioneGenereMovie dto)
    {
        // Controllo duplicati (case-insensitive)
        List<GenereMovie> generiMovies = _contesto.GeneriMovies.ToList();

        for (int i = 0; i < generiMovies.Count; i++)
        {
            GenereMovie genereMoviecorrente = generiMovies[i];

            bool stessoNome = string.Equals(
                genereMoviecorrente.Genere,
                dto.Genere,
                StringComparison.OrdinalIgnoreCase
            );

            if (stessoNome)
                return null;
        }

        // Creazione nuovo genere
        GenereMovie genereMovie = new GenereMovie();
        genereMovie.Genere = dto.Genere;

        _contesto.GeneriMovies.Add(genereMovie);
        await _contesto.SaveChangesAsync();

        // Mapping verso DTO
        DtoGenereMovie risultato = new DtoGenereMovie();
        risultato.Id = genereMovie.Id;
        risultato.Genere = genereMovie.Genere;

        return risultato;
    }

    // -----------------------------------------------------
    // MODIFICA GENERE ESISTENTE
    // -----------------------------------------------------
    public async Task<DtoGenereMovie?> ModificaAsync(string id, DtoCreazioneGenereMovie dto)
    {
        GenereMovie? genereMovie = await _contesto.GeneriMovies.FindAsync(id);

        if (genereMovie == null)
            return null;

        genereMovie.Genere = dto.Genere;

        await _contesto.SaveChangesAsync();

        DtoGenereMovie risultato = new DtoGenereMovie();
        risultato.Id = genereMovie.Id;
        risultato.Genere = genereMovie.Genere;

        return risultato;
    }

    // -----------------------------------------------------
    // ELIMINA GENERE
    // -----------------------------------------------------
    public async Task<bool> EliminaAsync(string id)
    {
        GenereMovie? genereMovie = await _contesto.GeneriMovies.FindAsync(id);

        if (genereMovie == null)
            return false;

        _contesto.GeneriMovies.Remove(genereMovie);
        await _contesto.SaveChangesAsync();

        return true;
    }
}
```

## LogAzioniService.cs

```c#
using NuovoCinemaParadiso.Data;
using NuovoCinemaParadiso.Dtos;
using NuovoCinemaParadiso.Models;

namespace NuovoCinemaParadiso.Services;

// Service responsabile della scrittura dei log azioni nel database
public class LogAzioniService
{
    private readonly ContestoDb _contesto;

    public LogAzioniService(ContestoDb contesto)
    {
        _contesto = contesto;
    }

    // Salva un log di azione nel DB e restituisce il DTO salvato
    public async Task<DtoLogAzioni> SalvataggioLogAzioneAsync(DtoCreazioneLogAzioni dto)
    {
        // Creazione entity Log
        LogAzioni log = new LogAzioni
        {
            IdUtente = dto.IdUtente,
            NomeAzione = dto.NomeAzione,
            Effettuato = dto.Effettuato,
            Messaggio = dto.Messaggio,
            TimeStamp = DateTimeOffsetOffset.UtcNow
        };

        // Inserimento nel DB
        _contesto.LogAzioni.Add(log);
        await _contesto.SaveChangesAsync();

        // Mapping verso DTO di output
        return new DtoLogAzioni
        {
            IdUtente = log.IdUtente,
            NomeAzione = log.NomeAzione,
            Effettuato = log.Effettuato,
            Messaggio = log.Messaggio,
            TimeStamp = log.TimeStamp
        };
    }
}
```

## MovieService.cs 

```C#
using Microsoft.EntityFrameworkCore;
using NuovoCinemaParadiso.Dtos;
using NuovoCinemaParadiso.Data;
using NuovoCinemaParadiso.Models;

namespace NuovoCinemaParadiso.Services;

public class MovieService
{
    private readonly ContestoDb _contesto;

    public MovieService(ContestoDb contesto)
    {
        _contesto = contesto;
    }

    // -----------------------------------------------------
    // OTTIENI TUTTI I FILM
    // -----------------------------------------------------
    public async Task<List<DtoMovie>> OttieniTutto()
    {
        // Recupera tutti i film dal DB
        List<Movie> movies = await _contesto.Movies.ToListAsync();

        List<DtoMovie> risultato = new List<DtoMovie>();

        // Mapping manuale + recupero genere per ogni film
        for (int i = 0; i < movies.Count; i++)
        {
            Movie movieCorrente = movies[i];

            // Recupero genere (query separata per ogni film)
            GenereMovie? genereMovie = await _contesto.GeneriMovies.FindAsync(movieCorrente.GenereId);

            DtoMovie dto = new DtoMovie();
            dto.Id = movieCorrente.Id;
            dto.Titolo = movieCorrente.Titolo;
            dto.Descrizione = movieCorrente.Descrizione;
            dto.DurataMinuti = movieCorrente.DurataMinuti;
            dto.Prezzo = movieCorrente.Prezzo;
            dto.GenereId = movieCorrente.GenereId;
            dto.Genere = genereMovie?.Genere ?? "";

            risultato.Add(dto);
        }

        return risultato;
    }

    // -----------------------------------------------------
    // OTTIENI FILM TRAMITE ID
    // -----------------------------------------------------
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
            Prezzo = movie.Prezzo,
            GenereId = movie.GenereId,
            Genere = genereMovie?.Genere ?? ""
        };
    }

    // -----------------------------------------------------
    // OTTIENI FILM FILTRATI PER GENERE
    // -----------------------------------------------------
    public async Task<List<DtoMovie>> OttieniTramiteGenere(string genereId)
    {
        List<DtoMovie> risultato = new List<DtoMovie>();

        // Recupera tutti i film (anche quelli non necessari)
        List<Movie> movies = await _contesto.Movies.ToListAsync();

        for (int i = 0; i < movies.Count; i++)
        {
            Movie movieCorrente = movies[i];

            // Recupero genere per ogni film
            GenereMovie? genereMovie = await _contesto.GeneriMovies.FindAsync(movieCorrente.GenereId);

            if (movieCorrente.GenereId == genereId)
            {
                DtoMovie dto = new DtoMovie();
                dto.Id = movieCorrente.Id;
                dto.Titolo = movieCorrente.Titolo;
                dto.Descrizione = movieCorrente.Descrizione;
                dto.DurataMinuti = movieCorrente.DurataMinuti;
                dto.Prezzo = movieCorrente.Prezzo;
                dto.GenereId = movieCorrente.GenereId;
                dto.Genere = genereMovie?.Genere ?? "";

                risultato.Add(dto);
            }
        }

        return risultato;
    }

    // -----------------------------------------------------
    // CREA UN NUOVO FILM
    // -----------------------------------------------------
    public async Task<DtoMovie> CreazioneAsync(DtoCreazioneMovie dto)
    {
        Movie movie = new Movie
        {
            Titolo = dto.Titolo,
            Descrizione = dto.Descrizione,
            DurataMinuti = dto.DurataMinuti,
            Prezzo = dto.Prezzo,
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
            Prezzo = movie.Prezzo,
            GenereId = movie.GenereId,
            Genere = genereMovie?.Genere ?? ""
        };
    }

    // -----------------------------------------------------
    // MODIFICA FILM ESISTENTE
    // -----------------------------------------------------
    public async Task<DtoMovie?> ModificaAsync(string id, DtoCreazioneMovie dto)
    {
        Movie? movieEsistente = await _contesto.Movies.FindAsync(id);

        if (movieEsistente == null)
            return null;

        movieEsistente.Titolo = dto.Titolo;
        movieEsistente.Descrizione = dto.Descrizione;
        movieEsistente.DurataMinuti = dto.DurataMinuti;
        movieEsistente.Prezzo = dto.Prezzo;
        movieEsistente.GenereId = dto.GenereId;

        await _contesto.SaveChangesAsync();

        return new DtoMovie
        {
            Id = movieEsistente.Id,
            Titolo = movieEsistente.Titolo,
            Descrizione = movieEsistente.Descrizione,
            DurataMinuti = movieEsistente.DurataMinuti,
            Prezzo = movieEsistente.Prezzo,
            GenereId = movieEsistente.GenereId
        };
    }

    // -----------------------------------------------------
    // ELIMINA FILM
    // -----------------------------------------------------
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

## RuoloUtenteService.cs 

```C#
using Microsoft.AspNetCore.Identity;
using NuovoCinemaParadiso.Dtos;
using NuovoCinemaParadiso.Models;

namespace NuovoCinemaParadiso.Services;

public class RuoloUtenteService
{
    private readonly UserManager<Utente> _gestioneUtenti;

    public RuoloUtenteService(UserManager<Utente> gestioneUtenti)
    {
        // UserManager viene iniettato per gestire utenti e ruoli
        _gestioneUtenti = gestioneUtenti;
    }

    // -----------------------------------------------------
    // MODIFICA IL RUOLO DI UN UTENTE
    // -----------------------------------------------------
    public async Task<string?> ModificaRuoloUtente(DtoModificaRuoloUtente dto)
    {
        // 1. Validazione del ruolo richiesto
        if (dto.NuovoRuolo != Ruoli.Gestore &&
            dto.NuovoRuolo != Ruoli.Operatore &&
            dto.NuovoRuolo != Ruoli.Utente)
        {
            return null; // ruolo non valido
        }

        // 2. Recupero utente tramite email
        Utente? utente = await _gestioneUtenti.FindByEmailAsync(dto.Email);
        if (utente == null)
        {
            return null; // utente non trovato
        }

        // 3. Recupero ruoli attuali dell’utente
        IList<string> ruoloCorrente = await _gestioneUtenti.GetRolesAsync(utente);

        // 4. Rimozione dei ruoli "classici" (Gestore, Operatore, Utente)
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

        // 5. Assegnazione del nuovo ruolo
        IdentityResult addResult = await _gestioneUtenti.AddToRoleAsync(utente, dto.NuovoRuolo);

        if (!addResult.Succeeded)
        {
            return null; // errore durante l’assegnazione
        }

        return dto.NuovoRuolo; // ruolo aggiornato correttamente
    }
}
```

## SalaService.cs

```C#
using Microsoft.EntityFrameworkCore;
using NuovoCinemaParadiso.Data;
using NuovoCinemaParadiso.Dtos;
using NuovoCinemaParadiso.Models;

namespace NuovoCinemaParadiso.Services;

public class SalaService
{
    private readonly ContestoDb _contesto;

    public SalaService(ContestoDb contesto)
    {
        _contesto = contesto;
    }

    // -----------------------------------------------------
    // OTTIENI TUTTE LE SALE
    // -----------------------------------------------------
    public async Task<List<DtoSala>> OttieniTuttoAsync() 
    {
        List<Sala> sale = await _contesto.Sale.ToListAsync();
        List<DtoSala> risultato = new List<DtoSala>();

        for (int i = 0; i < sale.Count; i++)
        {
            Sala salaCorrente = sale[i];

            // Recupero manuale delle entità collegate (N+1 query)
            TipologiaSala? tipologiaSala = await _contesto.TipologieSala.FindAsync(salaCorrente.TipologiaSalaId);
            FasciaOraria? fasciaOraria = await _contesto.FasceOrarie.FindAsync(salaCorrente.FasciaOrariaId);

            DtoSala dto = new DtoSala();
            dto.Id = salaCorrente.Id;
            dto.Nome = salaCorrente.Nome;
            dto.Capienza = salaCorrente.Capienza;
            dto.FasciaOraria = fasciaOraria?.Nome ?? "";
            dto.NomeTipologia = tipologiaSala?.Nome ?? "";

            risultato.Add(dto);
        }

        return risultato;
    }

    // -----------------------------------------------------
    // OTTIENI SALA TRAMITE ID
    // -----------------------------------------------------
    public async Task<DtoSala?> OttieniTramiteIdAsync(string id)
    {
        Sala? sala = await _contesto.Sale.FindAsync(id);

        if (sala == null)
            return null;

        TipologiaSala? tipologiaSala = await _contesto.TipologieSala.FindAsync(sala.TipologiaSalaId);
        FasciaOraria? fasciaOraria = await _contesto.FasceOrarie.FindAsync(sala.FasciaOrariaId);

        return new DtoSala
        {
            Id = sala.Id,
            Nome = sala.Nome,
            Capienza = sala.Capienza,
            NomeTipologia = tipologiaSala?.Nome ?? "",
            FasciaOraria = fasciaOraria?.Nome ?? ""
        };
    }

    // -----------------------------------------------------
    // FILTRO PER TIPOLOGIA
    // -----------------------------------------------------
    public async Task<List<DtoSala>> OttieniTramiteTipologiaAsync(string tipologiaId)
    {
        List<Sala> tutteLeSale = await _contesto.Sale.ToListAsync();
        List<DtoSala> risultato = new List<DtoSala>();

        foreach (var sala in tutteLeSale)
        {
            if (sala.TipologiaSalaId != tipologiaId)
                continue;

            TipologiaSala? tipologia = await _contesto.TipologieSala.FindAsync(sala.TipologiaSalaId);
            FasciaOraria? fascia = await _contesto.FasceOrarie.FindAsync(sala.FasciaOrariaId);

            risultato.Add(new DtoSala
            {
                Id = sala.Id,
                Nome = sala.Nome,
                Capienza = sala.Capienza,
                TipologiaSalaId = sala.TipologiaSalaId,
                NomeTipologia = tipologia?.Nome ?? "",
                FasciaOrariaId = sala.FasciaOrariaId,
                FasciaOraria = fascia?.Nome ?? ""
            });
        }

        return risultato;
    }

    // -----------------------------------------------------
    // FILTRO PER FASCIA ORARIA
    // -----------------------------------------------------
    public async Task<List<DtoSala>> OttieniTramiteFasciaOrariaAsync(string fasciaOrariaId)
    {
        List<Sala> tutteLeSale = await _contesto.Sale.ToListAsync();
        List<DtoSala> risultato = new List<DtoSala>();

        foreach (var sala in tutteLeSale)
        {
            if (sala.FasciaOrariaId != fasciaOrariaId)
                continue;

            TipologiaSala? tipologia = await _contesto.TipologieSala.FindAsync(sala.TipologiaSalaId);
            FasciaOraria? fascia = await _contesto.FasceOrarie.FindAsync(sala.FasciaOrariaId);

            risultato.Add(new DtoSala
            {
                Id = sala.Id,
                Nome = sala.Nome,
                Capienza = sala.Capienza,
                TipologiaSalaId = sala.TipologiaSalaId,
                NomeTipologia = tipologia?.Nome ?? "",
                FasciaOrariaId = sala.FasciaOrariaId,
                FasciaOraria = fascia?.Nome ?? ""
            });
        }

        return risultato;
    }

    // -----------------------------------------------------
    // CREA SALA
    // -----------------------------------------------------
    public async Task<DtoSala> CreazioneAsync(DtoCreazioneSala dto)
    {
        Sala sala = new Sala
        {
            Nome = dto.Nome,
            Capienza = dto.Capienza,
            FasciaOrariaId = dto.FasciaOrariaId,
            TipologiaSalaId = dto.TipologiaSalaId
        };

        _contesto.Sale.Add(sala);
        await _contesto.SaveChangesAsync();

        FasciaOraria? fasciaOraria = await _contesto.FasceOrarie.FindAsync(sala.FasciaOrariaId);
        TipologiaSala? tipologiaSala = await _contesto.TipologieSala.FindAsync(sala.TipologiaSalaId);

        return new DtoSala
        {
            Id = sala.Id,
            Nome = sala.Nome,
            Capienza = sala.Capienza,
            FasciaOrariaId = sala.FasciaOrariaId,
            FasciaOraria = fasciaOraria?.Nome ?? "",
            TipologiaSalaId = sala.TipologiaSalaId,
            NomeTipologia = tipologiaSala?.Nome ?? ""
        };
    }

    // -----------------------------------------------------
    // MODIFICA SALA
    // -----------------------------------------------------
    public async Task<DtoSala?> ModificaAsync(string id, DtoCreazioneSala dto)
    {
        Sala? salaEsistente = await _contesto.Sale.FindAsync(id);

        if (salaEsistente == null)
            return null;

        salaEsistente.Nome = dto.Nome;
        salaEsistente.Capienza = dto.Capienza;
        salaEsistente.FasciaOrariaId = dto.FasciaOrariaId;
        salaEsistente.TipologiaSalaId = dto.TipologiaSalaId;

        await _contesto.SaveChangesAsync();

        return new DtoSala
        {
            Id = salaEsistente.Id,
            Nome = salaEsistente.Nome,
            Capienza = salaEsistente.Capienza,
            FasciaOrariaId = salaEsistente.FasciaOrariaId,
            FasciaOraria = salaEsistente.FasciaOrariaId, // ⚠️ BUG
            TipologiaSalaId = salaEsistente.TipologiaSalaId,
            NomeTipologia = salaEsistente.TipologiaSala?.Nome ?? ""
        };
    }

    // -----------------------------------------------------
    // ELIMINA SALA
    // -----------------------------------------------------
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

## TipologieSalaService.cs 

```c#
using Microsoft.EntityFrameworkCore;
using NuovoCinemaParadiso.Dtos;
using NuovoCinemaParadiso.Data;
using NuovoCinemaParadiso.Models;

namespace NuovoCinemaParadiso.Services;

public class TipologiaSalaService
{
    private readonly ContestoDb _contesto;

    public TipologiaSalaService(ContestoDb contesto)
    {
        _contesto = contesto;
    }

    // -----------------------------------------------------
    // OTTIENI TUTTE LE TIPOLOGIE DI SALA
    // -----------------------------------------------------
    public async Task<List<DtoTipologiaSala>> OttieniTuttoAsync()
    {
        List<TipologiaSala> tipologieSala = await _contesto.TipologieSala.ToListAsync();

        List<DtoTipologiaSala> risultati = new List<DtoTipologiaSala>();

        foreach (var tipologiaSala in tipologieSala)
        {
            DtoTipologiaSala dto = new DtoTipologiaSala();
            dto.Id = tipologiaSala.Id;
            dto.Nome = tipologiaSala.Nome;
            dto.MaggiorazionePrezzo = tipologiaSala.MaggiorazionePrezzo;

            risultati.Add(dto);
        }

        return risultati;
    }

    // -----------------------------------------------------
    // OTTIENI TIPOLOGIA TRAMITE ID
    // -----------------------------------------------------
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

    // -----------------------------------------------------
    // CREA UNA NUOVA TIPOLOGIA DI SALA
    // -----------------------------------------------------
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

    // -----------------------------------------------------
    // MODIFICA TIPOLOGIA DI SALA ESISTENTE
    // -----------------------------------------------------
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

    // -----------------------------------------------------
    // ELIMINA TIPOLOGIA DI SALA
    // -----------------------------------------------------
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

## Program.cs

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

builder.Services.AddControllers();

builder.Services.AddDbContext<ContestoDb>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddIdentityCore<Utente>(options =>
{
    
    options.Password.RequireDigit = false; 
    options.Password.RequireLowercase = false; 
    options.Password.RequireUppercase = false; 
    options.Password.RequireNonAlphanumeric = false; 
    options.Password.RequiredLength = 6; 
})
.AddRoles<IdentityRole>() // <-- fondamentale per i ruoli
.AddSignInManager<SignInManager<Utente>>()
.AddEntityFrameworkStores<ContestoDb>()
.AddDefaultTokenProviders();

string? jwtKey = builder.Configuration["Jwt:Key"];
string? jwtIssuer = builder.Configuration["Jwt:Issuer"];
string? jwtAudience = builder.Configuration["Jwt:Audience"];

if (string.IsNullOrWhiteSpace(jwtKey) ||
    string.IsNullOrWhiteSpace(jwtIssuer) ||
    string.IsNullOrWhiteSpace(jwtAudience))
{
    throw new Exception("Configurazione JWT mancante in appsettings.json");
}

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

builder.Services.AddAuthorization();

// Permette ad Angular locale di chiamare l'API
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<GenereMovieService>();
builder.Services.AddScoped<TipologiaSalaService>();
builder.Services.AddScoped<FasciaOrariaService>();
builder.Services.AddScoped<SalaService>();
builder.Services.AddScoped<MovieService>();
builder.Services.AddScoped<AcquistoService>();
builder.Services.AddScoped<RuoloUtenteService>(); // <-- nuovo servizio per gestire i ruoli degli utenti
builder.Services.AddScoped<JwtHelper>();
builder.Services.AddScoped<LogAzioniService>();

var app = builder.Build();

app.UseCors("AllowAngularApp");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ContestoDb>();
    db.Database.Migrate();
}

//seed ruoli + utenti + interessi
await DataSeeder.SeedAsync(app.Services);

app.Run();
```