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

## Utente.cs // AGGIUNTA

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

## DtoUtente.cs // INTERO

```c#
namespace NuovoCinemaParadiso.Dtos;

/// <summary>
/// DTO utilizzato per restituire le informazioni principali di un utente.
/// Contiene sia dati anagrafici sia informazioni su abbonamenti e Gift Card.
/// </summary>
public class DtoUtente
{
    /// <summary>
    /// Identificativo univoco dell'utente.
    /// </summary>
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Nome e cognome dell'utente.
    /// </summary>
    public string NomeCompleto { get; set; } = string.Empty;

    /// <summary>
    /// Data di inizio dell'abbonamento (se presente).
    /// </summary>
    public DateTimeOffset DataInizio { get; set; }

    /// <summary>
    /// Data di attivazione della Gift Card (se presente).
    /// </summary>
    public DateTimeOffset DataInizioGiftCard { get; set; }

    /// <summary>
    /// Indica se l'utente possiede un abbonamento attivo.
    /// </summary>
    public bool SeAbbonato { get; set; }

    /// <summary>
    /// Indica se l'utente possiede una Gift Card attiva.
    /// </summary>
    public bool PossiedeGiftCard { get; set; } = false;

    /// <summary>
    /// Email dell'utente.
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Età dell'utente.
    /// </summary>
    public int Eta { get; set; }

    /// <summary>
    /// Identificativo dell'abbonamento associato all'utente.
    /// Può essere string.Empty se l'utente non è abbonato.
    /// </summary>
    public string AbbonamentoId { get; set; } = string.Empty;

    /// <summary>
    /// Identificativo della Gift Card associata all'utente.
    /// Può essere string.Empty se l'utente non possiede una Gift Card.
    /// </summary>
    public string GiftCardId { get; set; } = string.Empty;

    /// <summary>
    /// Nome o tipologia dell'abbonamento (es. "Mensile", "Annuale").
    /// </summary>
    public string TipoAbbonamento { get; set; } = string.Empty;
    
    /// <summary>
    /// Nome o tipologia della GiftCard (es. "10 film", "20 film").
    /// </summary>
    public string TipoGiftCard { get; set; } = string.Empty;
}
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

## GiftCardService.cs

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