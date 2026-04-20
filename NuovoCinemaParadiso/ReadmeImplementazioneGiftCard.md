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

## AuthService.cs // SOLO AGGIUNTA

```c#
public async Task<DtoAuthResponse?> LoginAsync(DtoLogin dto)
{
    // Recupera l’utente tramite email.
    // Se non esiste, il login fallisce immediatamente.
    Utente? utente = await _gestioneUtenti.FindByEmailAsync(dto.Email);
    
    if (utente == null)
    {
        return null;
    }

    // ---------------------------------------------------------
    // CONTROLLO SCADENZA ABBONAMENTO
    // ---------------------------------------------------------
    // Se l’utente risulta abbonato, calcoliamo la scadenza
    // utilizzando la data di inizio e la durata dell’abbonamento.
    if (utente.SeAbbonato == true)
    {
        // Calcola la data di scadenza dell’abbonamento.
        DateTimeOffset? scadenzaAbbonamento =
            Calcoli.CalcolaScadenza(utente.DataInizioAbbonamento, utente.Abbonamento.Durata);

        // Calcola quanti giorni mancano alla scadenza.
        int giorniMancanti =
            Calcoli.GiorniAllaScadenza(utente.DataInizioAbbonamento, utente.Abbonamento.Durata);

        // Se mancano 0 giorni, significa che l’abbonamento è scaduto.
        if (giorniMancanti == 0)
        {
            utente.SeAbbonato = false;
        }
    }

    // ---------------------------------------------------------
    // CONTROLLO SCADENZA GIFT CARD
    // ---------------------------------------------------------
    // Se l’utente possiede una gift card, calcoliamo la scadenza
    // utilizzando la data di attivazione e la durata della gift card.
    if (utente.PossiedeGiftCard == true)
    {
        // Calcola la data di scadenza della gift card.
        DateTimeOffset? scadenzaGiftCard =
            Calcoli.CalcolaScadenza(utente.DataInizioGiftCard, utente.GiftCard.Durata);

        // Calcola quanti giorni mancano alla scadenza.
        int giorniMancanti =
            Calcoli.GiorniAllaScadenza(utente.DataInizioGiftCard, utente.GiftCard.Durata);

        // Se mancano 0 giorni, la gift card è scaduta.
        if (giorniMancanti == 0)
        {
            utente.PossiedeGiftCard = false;
        }
    }
    
    // ---------------------------------------------------------
    // VERIFICA PASSWORD
    // ---------------------------------------------------------
    // Controlla che la password inserita sia corretta.
    // Se fallisce, il login non procede.
    SignInResult result =
        await _gestioneAccesso.CheckPasswordSignInAsync(utente, dto.Password, false);

    if (!result.Succeeded)
    {
        return null;
    }

    // ---------------------------------------------------------
    // RECUPERO RUOLI UTENTE
    // ---------------------------------------------------------
    IList<string> ruoli = await _gestioneUtenti.GetRolesAsync(utente);

    // ---------------------------------------------------------
    // GENERAZIONE TOKEN JWT
    // ---------------------------------------------------------
    string token = _jwtHelper.GenerateToken(utente, ruoli);

    // ---------------------------------------------------------
    // COSTRUZIONE RISPOSTA DI LOGIN
    // ---------------------------------------------------------
    DtoAuthResponse response = new DtoAuthResponse();
    response.Token = token;
    response.Id = utente.Id;
    response.NomeCompleto = utente.NomeCompleto;
    response.Email = utente.Email ?? string.Empty;

    // Se l’utente ha almeno un ruolo, restituiamo il primo.
    response.Ruolo = ruoli.Count > 0 ? ruoli[0] : "";

    return response;
}
```

## GiftCardService.cs

# Controllers

## GiftCardController.cs

# Helpers

## CalcoliHelper.cs // SOLO AGGIUNTA

```c#
/// <summary>
/// Calcola il prezzo finale di un acquisto in base a:
/// - prezzo base del film
/// - maggiorazione della tipologia sala
/// - numero di biglietti acquistati
/// - stato dell’utente (abbonato o possessore di gift card)
/// 
/// La logica gestisce tre casi:
/// 1. Utente NON abbonato e senza gift card → paga tutto
/// 2. Utente con gift card → scala i film disponibili
/// 3. Utente abbonato → paga tutto (l’abbonamento non dà sconti sui biglietti)
/// </summary>
public static Decimal CalcolaPrezzoFinale(decimal prezzoMovie, decimal maggiorazione, int numeroBiglietti, Utente utente)
{
    // ---------------------------------------------------------
    // CASO 1: Utente NON abbonato e NON possessore di gift card
    // ---------------------------------------------------------
    // Se l’utente non è abbonato, si applica il prezzo pieno.
    if (utente.SeAbbonato == false)
    {
        Decimal prezzoFinale = (prezzoMovie + maggiorazione) * numeroBiglietti;
        return prezzoFinale;
    }

    // ---------------------------------------------------------
    // CASO 2: Utente possessore di gift card
    // ---------------------------------------------------------
    // Se l’utente ha una gift card attiva, si scala il numero di film disponibili.
    else if (utente.PossiedeGiftCard == true)
    {
        // Se la gift card copre TUTTI i biglietti richiesti
        if (utente.GiftCard.NumeroMovie > numeroBiglietti)
        {
            // Tutti i biglietti sono coperti → prezzo 0
            Decimal prezzoFinale = 0;

            // Scala i film rimanenti dalla gift card
            utente.GiftCard.NumeroMovie = utente.GiftCard.NumeroMovie - numeroBiglietti;

            return prezzoFinale;
        }

        // Se la gift card copre ESATTAMENTE il numero di biglietti richiesti
        else if (utente.GiftCard.NumeroMovie == numeroBiglietti)
        {
            // Tutti i biglietti sono coperti → prezzo 0
            Decimal prezzoFinale = 0;

            // La gift card viene completamente consumata
            utente.GiftCard.NumeroMovie = utente.GiftCard.NumeroMovie - numeroBiglietti;

            // L’utente non possiede più una gift card attiva
            utente.PossiedeGiftCard = false;

            return prezzoFinale;
        }

        // Se la gift card copre SOLO una parte dei biglietti richiesti
        else
        {
            // Calcola quanti biglietti NON sono coperti dalla gift card
            int bigliettiRimanenti = numeroBiglietti - utente.GiftCard.NumeroMovie;

            // Prezzo da pagare solo per i biglietti non coperti
            Decimal prezzoFinale = (prezzoMovie + maggiorazione) * bigliettiRimanenti;

            // La gift card viene esaurita
            utente.PossiedeGiftCard = false;

            return prezzoFinale;
        }
    }

    // ---------------------------------------------------------
    // CASO 3: Utente abbonato (ma senza gift card)
    // ---------------------------------------------------------
    // L’abbonamento NON dà sconti sui biglietti → paga tutto.
    else
    {
        Decimal prezzoFinale = (prezzoMovie + maggiorazione) * numeroBiglietti;
        return prezzoFinale;
    }
}
```

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