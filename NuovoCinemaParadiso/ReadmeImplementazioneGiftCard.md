# Implementazione gift card

- Un utente può acquistare una gift card che comprende un numero di biglietti, ogni volta che acquisterà un biglietto questo verrà scalato dal totale dei biglietti della gift card.
- Modello
- Dtos
- Services
- Controller
- Calcolo nell'helper.

# AGGIORNARE IL README OGNI VOLTA CHE VIENE IMPLEMENTATO QUALCOSA.

# Models

## GiftCard.cs // INTERO

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

## Utente.cs // SOLO AGGIUNTA

```c#
/// <summary>
/// Booleano se l'utente possiede una Gift Card.
/// </summary>
[Required]
public bool PossiedeGiftCard { get; set; } = false;

/// <summary>
/// Data in cui la Gift Card è stata attivata dall'utente.
/// Usa DateTimeOffset per mantenere il fuso orario.
/// </summary>
public DateTimeOffset DataInizioGiftCard { get; set; }

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
```

# Dtos 

## DtoUtente.cs // SOLO AGGIUNTA

```c#
    /// <summary>
    /// Data di attivazione della Gift Card (se presente).
    /// </summary>
    public DateTimeOffset DataInizioGiftCard { get; set; }

    /// <summary>
    /// Indica se l'utente possiede una Gift Card attiva.
    /// </summary>
    public bool PossiedeGiftCard { get; set; } = false;

    /// <summary>
    /// Identificativo della Gift Card associata all'utente.
    /// Può essere string.Empty se l'utente non possiede una Gift Card.
    /// </summary>
    public string GiftCardId { get; set; } = string.Empty;
    
    /// <summary>
    /// Nome o tipologia della GiftCard (es. "10 film", "20 film").
    /// </summary>
    public string TipoGiftCard { get; set; } = string.Empty;
```

## DtoGiftCard.cs // INTERO

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

## DtoCreazioneGiftCard.cs // INTERO

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

# Services

## UtenteService.cs // SOLO AGGIUNTA

```c#
public async Task<DtoUtente> GiftCardAsync(string giftCardId, string utenteId)
{
    // ❌ Carichi tutte le gift card (inefficiente)
    List<GiftCard> giftCards = await _contesto.GiftCards.ToListAsync();
    GiftCard? giftCardTrovata = null;

    // ❌ Ricerca manuale con ciclo (inefficiente)
    for (int i = 0; i < giftCards.Count; i++)
    {
        GiftCard giftCardCorrente = giftCards[i];

        if (giftCardCorrente.Id == giftCardId)
        {
            giftCardTrovata = giftCardCorrente;
            break;
        }
    }

    if (giftCardTrovata == null)
    {
        return null;
    }

    // ✔ Recupero utente tramite Identity
    Utente? utenteTrovato = await _gestioneUtenti.FindByIdAsync(utenteId);

    if (utenteTrovato == null)
    {
        return null;
    }

    // ✔ Assegnazione GiftCard all’utente
    utenteTrovato.GiftCardId = giftCardTrovata.Id;
    utenteTrovato.PossiedeGiftCard = true;
    utenteTrovato.DataInizioGiftCard = DateTimeOffset.UtcNow;

    // ❌ Salvi solo il contesto EF, non l’utente Identity
    await _contesto.SaveChangesAsync();

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
```

## GiftCardService.cs // INTERO

```c#
using Microsoft.EntityFrameworkCore;
using NuovoCinemaParadiso.Dtos;
using NuovoCinemaParadiso.Data;
using NuovoCinemaParadiso.Models;

namespace NuovoCinemaParadiso.Services;

public class GiftCardService
{
    private readonly ContestoDb _contesto;

    public GiftCardService(ContestoDb contesto)
    {
        _contesto = contesto;
    }

    // -----------------------------------------------------
    // OTTIENI TUTTE LE GIFT CARD
    // -----------------------------------------------------
    public async Task<List<DtoGiftCard>> OttieniTutto()
    {
        // Recupera tutte le gift card dal database
        List<GiftCard> giftCards = await _contesto.GiftCards.ToListAsync();

        List<DtoGiftCard> risultato = new List<DtoGiftCard>();

        // Mapping manuale verso DTO
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

    // -----------------------------------------------------
    // OTTIENI GIFT CARD TRAMITE ID SOLO SE APPARTIENE ALL’UTENTE
    // -----------------------------------------------------
    public async Task<DtoGiftCard?> OttieniTramiteIdAsync(string id, string utenteId)
    {
        // Recupera la gift card
        GiftCard? giftCard = await _contesto.GiftCards.FindAsync(id);

        if (giftCard == null)
            return null;

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

    // -----------------------------------------------------
    // CREA UNA NUOVA GIFT CARD
    // -----------------------------------------------------
    public async Task<DtoGiftCard> CreazioneAsync(DtoCreazioneGiftCard dto)
    {
        GiftCard giftCard = new GiftCard();
        giftCard.Nome = dto.Nome;
        giftCard.Durata = dto.Durata;
        giftCard.Prezzo = dto.Prezzo;
        giftCard.NumeroMovie = dto.NumeroMovie;

        _contesto.GiftCards.Add(giftCard);
        await _contesto.SaveChangesAsync();

        return new DtoGiftCard
        {
            Id = giftCard.Id,
            Nome = giftCard.Nome,
            Durata = giftCard.Durata,
            Prezzo = giftCard.Prezzo,
            NumeroMovie = giftCard.NumeroMovie
        };
    }

    // -----------------------------------------------------
    // MODIFICA GIFT CARD ESISTENTE
    // -----------------------------------------------------
    public async Task<DtoGiftCard?> ModificaAsync(string id, DtoCreazioneGiftCard dto)
    {
        GiftCard? giftCard = await _contesto.GiftCards.FindAsync(id);

        if (giftCard == null)
            return null;

        giftCard.Nome = dto.Nome;
        giftCard.Durata = dto.Durata;
        giftCard.Prezzo = dto.Prezzo;
        giftCard.NumeroMovie = dto.NumeroMovie;

        await _contesto.SaveChangesAsync();

        return new DtoGiftCard
        {
            Id = giftCard.Id,
            Nome = giftCard.Nome,
            Durata = giftCard.Durata,
            Prezzo = giftCard.Prezzo,
            NumeroMovie = giftCard.NumeroMovie
        };
    }

    // -----------------------------------------------------
    // ELIMINA GIFT CARD
    // -----------------------------------------------------
    public async Task<bool> EliminazioneAsync(string id)
    {
        GiftCard? giftCard = await _contesto.GiftCards.FindAsync(id);

        if (giftCard == null)
            return false;

        _contesto.GiftCards.Remove(giftCard);
        await _contesto.SaveChangesAsync();

        return true;
    }
}
```

## AdminService.cs // SOLO AGGIUNTA

```c#
public async Task<DtoGiftCard?> OttieniGiftCardTramiteIdPerAdminAsync(string id)
{
    // Recupera la GiftCard tramite chiave primaria (ID).
    // FindAsync è il metodo più veloce quando si cerca per PK.
    GiftCard? giftCard = await _contesto.GiftCards.FindAsync(id);

    // Se la GiftCard non esiste, restituisce null.
    if (giftCard == null)
    {
        return null;
    }

    // Mapping manuale dell'entità GiftCard verso il DTO.
    // Questo DTO contiene solo le informazioni principali della GiftCard.
    DtoGiftCard dto = new DtoGiftCard();
    dto.Id = giftCard.Id;
    dto.Nome = giftCard.Nome;
    dto.Durata = giftCard.Durata;
    dto.Prezzo = giftCard.Prezzo;
    dto.NumeroMovie = giftCard.NumeroMovie;

    return dto;
}

public async Task<List<DtoUtente>> OttieniUtentiTramiteGiftCardAsync(string giftCardId)
{
    // Recupera tutti gli utenti dal database.
    // NOTA: questo approccio carica tutti gli utenti in memoria.
    List<Utente> utenti = await _contesto.Utenti.ToListAsync();

    // Recupera tutte le GiftCard dal database.
    // Anche questo approccio carica tutte le gift card in memoria.
    List<GiftCard> giftCards = await _contesto.GiftCards.ToListAsync();

    GiftCard? giftCardTrovata = null;

    // Ricerca manuale della GiftCard tramite ciclo.
    // Si interrompe appena viene trovata quella con l'ID richiesto.
    for (int i = 0; i < giftCards.Count; i++)
    {
        GiftCard giftCardCorrente = giftCards[i];

        if (giftCardCorrente.Id == giftCardId)
        {
            giftCardTrovata = giftCardCorrente;
            break;
        }
    }

    // Se la GiftCard non esiste, restituisce null.
    if (giftCardTrovata == null)
    {
        return null;
    }

    List<DtoUtente> risultato = new List<DtoUtente>();

    // Scorre tutti gli utenti per verificare chi possiede la GiftCard trovata.
    for (int i = 0; i < utenti.Count; i++)
    {
        Utente utenteCorrente = utenti[i];

        // Confronto diretto tra entità GiftCard.
        // Questo funziona solo se EF Core ha tracciato entrambe le entità
        // e se rappresentano lo stesso riferimento in memoria.
        if (utenteCorrente.GiftCard == giftCardTrovata)
        {
            // Mapping dell'utente verso il DTO.
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
```

# Controllers

## GiftCardController.cs

# Helpers

## CalcoliHelper.cs

# Data

## ContestoDb // SOLO AGGIUNTA

```c#
public DbSet<GiftCard> GiftCards { get; set; }
// ✔ Rappresenta la tabella "GiftCard" nel database
// ✔ Permette di eseguire query, inserimenti, modifiche ed eliminazioni sulle GiftCard
// ✔ EF Core genererà automaticamente la tabella se non esiste (Code First)
```

# Program.cs // SOLO AGGIUNTA
```c#
builder.Services.AddScoped<ProiezioneService>();
// ✔ Registra ProiezioneService nel Dependency Injection container
// ✔ Lifetime: Scoped → una nuova istanza per ogni richiesta HTTP
// ✔ Ideale per servizi che usano DbContext (anch’esso scoped)
// ✔ Permette ai controller di ricevere ProiezioneService tramite costruttore
```