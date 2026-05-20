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
        using IServiceScope scope = serviceProvider.CreateScope(); // Crea scope per i servizi

        DateTime oggi = DateTime.Today; // Data odierna
        ContestoDb contestoDb = scope.ServiceProvider.GetRequiredService<ContestoDb>(); // DbContext
        UserManager<Utente> gestioneUtenti = scope.ServiceProvider.GetRequiredService<UserManager<Utente>>(); // Gestione utenti
        RoleManager<IdentityRole> gestioneRuoli = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>(); // Gestione ruoli

        await AssicuraEsistenzaRuoloAsync(gestioneRuoli, Ruoli.Gestore);   // Crea ruolo se non esiste
        await AssicuraEsistenzaRuoloAsync(gestioneRuoli, Ruoli.Operatore);
        await AssicuraEsistenzaRuoloAsync(gestioneRuoli, Ruoli.Utente);

        // Crea utenti base
        Utente gestore = await AssicuraEsistenzaUtenteAsync(gestioneUtenti, "gestore@gmail.com", "123456", "Gestore", 60, false);
        Utente operatore = await AssicuraEsistenzaUtenteAsync(gestioneUtenti, "operatore@gmail.com", "123456", "Operatore", 35, false);
        Utente utente = await AssicuraEsistenzaUtenteAsync(gestioneUtenti, "utente1@gmail.com", "123456", "Utente Uno", 15, true);

        // Crea conto cinema se non esiste
        ContoCinema contoCinema = await AssicuraEsistenzaConto(contestoDb, "IT60X0542811101000000123456", "Gestore", 200);

        // Imposta ruolo unico per ogni utente
        await ImpostaRuoloUnicoAsync(gestioneUtenti, gestore, Ruoli.Gestore);
        await ImpostaRuoloUnicoAsync(gestioneUtenti, operatore, Ruoli.Operatore);
        await ImpostaRuoloUnicoAsync(gestioneUtenti, utente, Ruoli.Utente);

        // Generi film
        var genereAzione       = await AssicuraEsistenzaGenereMovie(contestoDb, "Azione");
        var genereHorror       = await AssicuraEsistenzaGenereMovie(contestoDb, "Horror");
        var genereCommedia     = await AssicuraEsistenzaGenereMovie(contestoDb, "Commedia");
        var genereAnimazione   = await AssicuraEsistenzaGenereMovie(contestoDb, "Animazione");
        var genereAvventura    = await AssicuraEsistenzaGenereMovie(contestoDb, "Avventura");
        var genereDocumentario = await AssicuraEsistenzaGenereMovie(contestoDb, "Documentario");
        var genereDrammatico   = await AssicuraEsistenzaGenereMovie(contestoDb, "Drammatico");
        var genereFantascienza = await AssicuraEsistenzaGenereMovie(contestoDb, "Fantascienza");
        var genereFantasy      = await AssicuraEsistenzaGenereMovie(contestoDb, "Fantasy");
        var genereMusical      = await AssicuraEsistenzaGenereMovie(contestoDb, "Musical");
        var genereRomantico    = await AssicuraEsistenzaGenereMovie(contestoDb, "Romantico");
        var genereThriller     = await AssicuraEsistenzaGenereMovie(contestoDb, "Thriller");
        var genereWestern      = await AssicuraEsistenzaGenereMovie(contestoDb, "Western");

        // Film
        var movie1 = await AssicuraEsistenzaMovie(contestoDb, "Movie1", "Film del drago", 60, 10, genereAzione.Id);
        var movie2 = await AssicuraEsistenzaMovie(contestoDb, "Movie2", "Film del lupo", 80, 12, genereHorror.Id);
        var movie3 = await AssicuraEsistenzaMovie(contestoDb, "Movie3", "Film del cane", 100, 14, genereCommedia.Id);

        // Tipologie sala
        var tipologia2D = await AssicuraEsistenzaTipologiaSala(contestoDb, "2D", 2);
        var tipologia3D = await AssicuraEsistenzaTipologiaSala(contestoDb, "3D", 3);
        var tipologiaImax = await AssicuraEsistenzaTipologiaSala(contestoDb, "IMAX", 4);

        // Turni
        var turnoMattina = await AssicuraEsistenzaTurno(contestoDb, new TimeOnly(10, 0), new TimeOnly(13, 0), "Mattina");
        var turnoPomeriggio = await AssicuraEsistenzaTurno(contestoDb, new TimeOnly(13, 0), new TimeOnly(18, 0), "Pomeriggio");
        var turnoSera = await AssicuraEsistenzaTurno(contestoDb, new TimeOnly(18, 0), new TimeOnly(22, 0), "Sera");

        // Sale
        var sala1 = await AssicuraEsistenzaSala(contestoDb, "Sala1", 30, tipologia2D.Id);
        var sala2 = await AssicuraEsistenzaSala(contestoDb, "Sala2", 40, tipologia3D.Id);
        var sala3 = await AssicuraEsistenzaSala(contestoDb, "Sala3", 50, tipologiaImax.Id);

        // Abbonamenti
        await AssicuraEsistenzaAbbonamento(contestoDb, "Mensile", 70, 25, 1);
        await AssicuraEsistenzaAbbonamento(contestoDb, "Semestrale", 210, 50, 6);
        await AssicuraEsistenzaAbbonamento(contestoDb, "Annuale", 300, 75, 12);

        // Proiezioni
        var proiezione1 = await AssicuraEsistenzaProiezione(contestoDb, new DateOnly(2027, 1, 1), movie1.Id, sala1.Id, turnoMattina.Id);
        var proiezione2 = await AssicuraEsistenzaProiezione(contestoDb, new DateOnly(2027, 1, 1), movie2.Id, sala2.Id, turnoPomeriggio.Id);
        var proiezione3 = await AssicuraEsistenzaProiezione(contestoDb, new DateOnly(2027, 1, 1), movie3.Id, sala3.Id, turnoSera.Id);

        // Biglietti di esempio
        await AssicuraEsistenzaBiglietto(contestoDb, proiezione1.Id, utente.Id, 1, DateTimeOffset.Now, 10);
        await AssicuraEsistenzaBiglietto(contestoDb, proiezione2.Id, utente.Id, 2, DateTimeOffset.Now, 20);
        await AssicuraEsistenzaBiglietto(contestoDb, proiezione3.Id, utente.Id, 3, DateTimeOffset.Now, 30);

        // Crea un secondo conto cinema (duplicato)
        await AssicuraEsistenzaContoCinema(contestoDb, "IT60X0542811101000000123456", "Gestore", 200);
    }

    private static async Task AssicuraEsistenzaRuoloAsync(RoleManager<IdentityRole> managerRuolo, string nomeRuolo)
    {
        bool seEsiste = await managerRuolo.RoleExistsAsync(nomeRuolo); // Controlla se il ruolo esiste
        if (!seEsiste)
        {
            IdentityRole ruolo = new IdentityRole { Name = nomeRuolo }; // Crea nuovo ruolo
            await managerRuolo.CreateAsync(ruolo);
        }
    }

    private static async Task<Utente> AssicuraEsistenzaUtenteAsync(
        UserManager<Utente> gestioneUtenti,
        string email,
        string password,
        string nomeCompleto,
        int eta,
        bool abbonato)
    {
        Utente? utenteEsistente = await gestioneUtenti.FindByEmailAsync(email); // Cerca utente

        if (utenteEsistente != null)
            return utenteEsistente; // Se esiste lo restituisce

        Utente utente = new Utente
        {
            UserName = email,
            Email = email,
            NomeCompleto = nomeCompleto,
            Eta = eta,
            SeAbbonato = abbonato,
            AbbonamentoId = null
        };

        IdentityResult risultato = await gestioneUtenti.CreateAsync(utente, password); // Crea utente

        if (!risultato.Succeeded)
        {
            List<string> errori = new List<string>();
            foreach (IdentityError errore in risultato.Errors)
                errori.Add(errore.Description);

            throw new Exception($"Errore durante il seed dell'utente {email} : {string.Join("|", errori)}");
        }

        return utente;
    }

    private static async Task<ContoCinema> AssicuraEsistenzaConto(ContestoDb context, string iban, string titolareConto, int conto)
    {
        ContoCinema? contoEsistente = await context.ContoCinema.FirstOrDefaultAsync(); // Prende il primo conto se esiste
        if (contoEsistente != null)
            return contoEsistente;

        ContoCinema nuovoContoCinema = new ContoCinema
        {
            Iban = iban,
            TitolareConto = titolareConto,
            Conto = conto
        };

        context.ContoCinema.Add(nuovoContoCinema); // Crea nuovo conto
        await context.SaveChangesAsync();

        return nuovoContoCinema;
    }

    private static async Task ImpostaRuoloUnicoAsync(UserManager<Utente> gestioneUtenti, Utente utente, string ruoloTarget)
    {
        IList<string> ruoliCorrenti = await gestioneUtenti.GetRolesAsync(utente); // Ruoli attuali

        // Rimuove eventuali ruoli cinema
        for (int i = 0; i < ruoliCorrenti.Count; i++)
        {
            string ruoloCorrente = ruoliCorrenti[i];

            if (ruoloCorrente == Ruoli.Gestore || ruoloCorrente == Ruoli.Operatore || ruoloCorrente == Ruoli.Utente)
                await gestioneUtenti.RemoveFromRoleAsync(utente, ruoloCorrente);
        }

        bool alreadyInTargetRole = await gestioneUtenti.IsInRoleAsync(utente, ruoloTarget);

        if (!alreadyInTargetRole)
            await gestioneUtenti.AddToRoleAsync(utente, ruoloTarget); // Assegna ruolo finale
    }

    private static async Task<GenereMovie> AssicuraEsistenzaGenereMovie(ContestoDb context, string genere)
    {
        List<GenereMovie> generiMovies = await context.GeneriMovies.ToListAsync(); // Tutti i generi

        for (int i = 0; i < generiMovies.Count; i++)
        {
            GenereMovie genereCorrente = generiMovies[i];

            bool nomeUguale = string.Equals(genereCorrente.Genere, genere, StringComparison.OrdinalIgnoreCase);
            if (nomeUguale)
                return genereCorrente; // Già esiste
        }

        GenereMovie nuovoGenere = new GenereMovie { Genere = genere };

        context.GeneriMovies.Add(nuovoGenere); // Crea nuovo genere
        await context.SaveChangesAsync();

        return nuovoGenere;
    }

    private static async Task<Movie> AssicuraEsistenzaMovie(
        ContestoDb context,
        string titolo,
        string descrizione,
        int durataMinuti,
        int prezzoMovie,
        string genereId)
    {
        List<Movie> movies = await context.Movies.ToListAsync(); // Tutti i film

        for (int i = 0; i < movies.Count; i++)
        {
            Movie movieCorrente = movies[i];

            bool nomeUguale = string.Equals(movieCorrente.Titolo, titolo, StringComparison.OrdinalIgnoreCase);
            if (nomeUguale)
                return movieCorrente; // Film già presente
        }

        Movie nuovoMovie = new Movie
        {
            Titolo = titolo,
            Descrizione = descrizione,
            DurataMinuti = durataMinuti,
            PrezzoMovie = prezzoMovie,
            GenereId = genereId
        };

        context.Movies.Add(nuovoMovie); // Crea film
        await context.SaveChangesAsync();

        return nuovoMovie;
    }

    private static async Task<TipologiaSala> AssicuraEsistenzaTipologiaSala(
        ContestoDb context,
        string nome,
        int maggiorazioneprezzo)
    {
        List<TipologiaSala> tipologieSala = await context.TipologieSala.ToListAsync(); // Tutte le tipologie

        for (int i = 0; i < tipologieSala.Count; i++)
        {
            TipologiaSala tipologiaCorrente = tipologieSala[i];

            bool nomeUguale = string.Equals(tipologiaCorrente.Nome, nome, StringComparison.OrdinalIgnoreCase);
            if (nomeUguale)
                return tipologiaCorrente;
        }

        TipologiaSala nuovaTipologia = new TipologiaSala
        {
            Nome = nome,
            MaggiorazionePrezzo = maggiorazioneprezzo
        };

        context.TipologieSala.Add(nuovaTipologia); // Crea tipologia
        await context.SaveChangesAsync();

        return nuovaTipologia;
    }

    private static async Task<Sala> AssicuraEsistenzaSala(
        ContestoDb context,
        string nome,
        int capienza,
        string tipologiaSalaId)
    {
        List<Sala> sale = await context.Sale.ToListAsync(); // Tutte le sale

        for (int i = 0; i < sale.Count; i++)
        {
            Sala salaCorrente = sale[i];

            bool nomeUguale = string.Equals(salaCorrente.Nome, nome, StringComparison.OrdinalIgnoreCase);
            if (nomeUguale)
                return salaCorrente;
        }

        Sala nuovaSala = new Sala
        {
            Nome = nome,
            Capienza = capienza,
            TipologiaSalaId = tipologiaSalaId
        };

        context.Sale.Add(nuovaSala); // Crea sala
        await context.SaveChangesAsync();

        return nuovaSala;
    }

    private static async Task<Turno> AssicuraEsistenzaTurno(
        ContestoDb context,
        TimeOnly oraInizio,
        TimeOnly oraFine,
        string nome)
    {
        List<Turno> turni = await context.Turni.ToListAsync(); // Tutti i turni

        for (int i = 0; i < turni.Count; i++)
        {
            Turno turnoCorrente = turni[i];

            bool nomeUguale = string.Equals(turnoCorrente.Nome, nome, StringComparison.OrdinalIgnoreCase);

            bool stessoOrario = turnoCorrente.OraInizio == oraInizio && turnoCorrente.OraFine == oraFine;

            if (nomeUguale || stessoOrario)
                return turnoCorrente;
        }

        Turno nuovoTurno = new Turno
        {
            Nome = nome,
            OraInizio = oraInizio,
            OraFine = oraFine
        };

        context.Turni.Add(nuovoTurno); // Crea turno
        await context.SaveChangesAsync();

        return nuovoTurno;
    }

    private static async Task AssicuraEsistenzaAbbonamento(
        ContestoDb context,
        string nome,
        int prezzo,
        int sconto,
        int durata)
    {
        List<Abbonamento> abbonamenti = await context.Abbonamenti.ToListAsync(); // Tutti gli abbonamenti

        for (int i = 0; i < abbonamenti.Count; i++)
        {
            Abbonamento abbonamentoCorrente = abbonamenti[i];

            bool nomeUguale = string.Equals(abbonamentoCorrente.Nome, nome, StringComparison.OrdinalIgnoreCase);
            if (nomeUguale)
                return; // Già esiste
        }

        Abbonamento nuovoAbbonamento = new Abbonamento
        {
            Nome = nome,
            Prezzo = prezzo,
            Sconto = sconto,
            Durata = durata
        };

        context.Abbonamenti.Add(nuovoAbbonamento); // Crea abbonamento
        await context.SaveChangesAsync();
    }

    private static async Task<Proiezione> AssicuraEsistenzaProiezione(
        ContestoDb context,
        DateOnly dataProiezione,
        string movieId,
        string salaId,
        string turnoId)
    {
        List<Proiezione> proiezioni = await context.Proiezioni.ToListAsync(); // Tutte le proiezioni

        for (int i = 0; i < proiezioni.Count; i++)
        {
            Proiezione proiezione = proiezioni[i];

            bool stessaSala = proiezione.SalaId == salaId;
            bool stessoMovie = proiezione.MovieId == movieId;
            bool stessoTurno = proiezione.TurnoId == turnoId;

            if (stessaSala && stessoMovie && stessoTurno)
                return proiezione; // Già esiste
        }

        Proiezione nuovaProiezione = new Proiezione
        {
            DataProiezione = dataProiezione,
            MovieId = movieId,
            SalaId = salaId,
            TurnoId = turnoId,
            Attivo = true
        };

        context.Proiezioni.Add(nuovaProiezione); // Crea proiezione
        await context.SaveChangesAsync();

        return nuovaProiezione;
    }

        private static async Task AssicuraEsistenzaBiglietto(
        ContestoDb context,
        string proiezioneId,
        string utenteId,
        int numeroBiglietti,
        DateTimeOffset orarioCreazione,
        int prezzoFinale)
    {
        List<Biglietto> biglietti = await context.Biglietti.ToListAsync(); 
        // Recupera tutti i biglietti (non usato, ma presente nel metodo)

        Biglietto nuovoBiglietto = new Biglietto
        {
            ProiezioneId = proiezioneId,       // FK proiezione
            UtenteId = utenteId,               // FK utente
            NumeroBiglietti = numeroBiglietti, // Quantità acquistata
            OrarioCreazione = orarioCreazione, // Timestamp creazione
            PrezzoFinale = prezzoFinale        // Prezzo totale
        };

        context.Biglietti.Add(nuovoBiglietto); // Inserisce il biglietto
        await context.SaveChangesAsync();      // Salva su DB
    }

    private static async Task AssicuraEsistenzaContoCinema(
        ContestoDb context,
        string iban,
        string titolareConto,
        int conto)
    {
        ContoCinema nuovoContoCinema = new ContoCinema
        {
            Iban = iban,               // IBAN del conto
            TitolareConto = titolareConto, // Nome titolare
            Conto = conto              // Importo iniziale
        };

        context.ContoCinema.Add(nuovoContoCinema); // Crea nuovo conto
        await context.SaveChangesAsync();          // Salva su DB
    }
}
```

# Controllers

## OperatoreController.cs

```c#
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace NuovoCinemaParadiso.Models;

[Table("Biglietti")]
public class Biglietto
{
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString(); 
    // Id univoco del biglietto

    [Required]
    public string ProiezioneId { get; set; } = string.Empty; 
    // FK verso la proiezione

    [ForeignKey("ProiezioneId")]
    public Proiezione? Proiezione { get; set; } 
    // Navigazione verso la proiezione (nullable per EF)

    [Required]
    public string UtenteId { get; set; } = string.Empty; 
    // FK verso l’utente

    [ForeignKey("UtenteId")]
    public Utente? Utente { get; set; } 
    // Navigazione verso l’utente (nullable per EF)

    [Required]
    [Range(1, 100, ErrorMessage = "Il numero di biglietti deve essere maggiore di zero e massimo 100.")]
    public int NumeroBiglietti { get; set; } 
    // Numero di biglietti acquistati

    public DateTimeOffset OrarioCreazione { get; set; } = DateTimeOffset.UtcNow; 
    // Timestamp di creazione

    [Required]
    public int PrezzoFinale { get; set; } 
    // Prezzo totale pagato
}
```

# modelli

## Biglietto.cs

```c#
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace NuovoCinemaParadiso.Models;

[Table("Biglietti")]
public class Biglietto
{
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString(); 
    // Id univoco del biglietto

    [Required]
    public string ProiezioneId { get; set; } = string.Empty; 
    // FK verso la proiezione

    [ForeignKey("ProiezioneId")]
    public Proiezione? Proiezione { get; set; } 
    // Navigazione verso la proiezione (nullable per EF)

    [Required]
    public string UtenteId { get; set; } = string.Empty; 
    // FK verso l’utente

    [ForeignKey("UtenteId")]
    public Utente? Utente { get; set; } 
    // Navigazione verso l’utente (nullable per EF)

    [Required]
    [Range(1, 100, ErrorMessage = "Il numero di biglietti deve essere maggiore di zero e massimo 100.")]
    public int NumeroBiglietti { get; set; } 
    // Numero di biglietti acquistati

    public DateTimeOffset OrarioCreazione { get; set; } = DateTimeOffset.UtcNow; 
    // Timestamp di creazione

    [Required]
    public int PrezzoFinale { get; set; } 
    // Prezzo totale pagato
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

## DtoBiglietto.cs

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

## DtoRicaricaSaldoUtente.cs

```c#
namespace NuovoCinemaParadiso.Dtos;

public class DtoRicaricaSaldoUtente
{
    public string Email { get; set; } = string.Empty; // Email dell’utente da ricaricare
    public int Ricarica { get; set; }                 // Importo della ricarica da aggiungere al saldo
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