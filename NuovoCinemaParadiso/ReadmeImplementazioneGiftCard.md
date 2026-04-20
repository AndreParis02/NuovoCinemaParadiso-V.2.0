# Implementazione gift card

- Un utente può acquistare una gift card che comprende un numero di biglietti, ogni volta che acquisterà un biglietto questo verrà scalato dal totale dei biglietti della gift card.
- Modello
- Dtos
- Services
- Controller
- Calcolo nell'helper.

# AGGIORNARE IL README OGNI VOLTA CHE VIENE IMPLEMENTATO QUALCOSA.

# Models

## GiftCard.cs // INTERO // INCLUSO

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

## Utente.cs // SOLO AGGIUNTA // INCLUSO

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

## DtoUtente.cs // SOLO AGGIUNTA // INCLUSO

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

## DtoGiftCard.cs // INTERO // INCLUSO

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

## DtoCreazioneGiftCard.cs // INTERO // INCLUSO

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

## UtenteService.cs // SOLO AGGIUNTA // INCLUSO

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

## AuthService.cs // SOLO AGGIUNTA // INCLUSO

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

## GiftCardService.cs // INTERO // INCLUSO

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

# Controllers

## GiftCardController.cs // INTERO // INCLUSO

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
        await Log(utenteId, "Ottieni tutte le GiftCard", true);

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
            await Log(utenteId, "Ottieni GiftCard tramite id", false);
            return NotFound($"GiftCard con id {id} non trovata");
        }

        await Log(utenteId, "Ottieni GiftCard tramite id", true);
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
        await Log(utenteId, "Creazione GiftCard", true);

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
            await Log(utenteId, "Modifica GiftCard", false);
            return NotFound(new { messaggio = "GiftCard non trovata." });
        }

        await Log(utenteId, "Modifica GiftCard", true);
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
            await Log(utenteId, "Eliminazione GiftCard", false);
            return NotFound(new { messaggio = "GiftCard non trovata." });
        }

        await Log(utenteId, "Eliminazione GiftCard", true);
        return NoContent();
    }

    // ---------------------------------------------------------
    // METODO PRIVATO PER SALVARE LOG DELLE AZIONI
    // ---------------------------------------------------------
    public async Task Log(string utenteId, string azione, bool risultato)
    {
        // Messaggio da salvare nel log
        string messaggio = risultato ? "Operazione eseguita" : "Operazione fallita";

        // Salvataggio log tramite servizio dedicato
        await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
        {
            IdUtente = utenteId,
            NomeAzione = azione,
            Effettuato = risultato,
            Messaggio = messaggio
        });
    }
}
```

# Helpers

## CalcoliHelper.cs // SOLO AGGIUNTA // INCLUSO

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

## ContestoDb // SOLO AGGIUNTA // INCLUSO

```c#
public DbSet<GiftCard> GiftCards { get; set; }
// ✔ Rappresenta la tabella "GiftCard" nel database
// ✔ Permette di eseguire query, inserimenti, modifiche ed eliminazioni sulle GiftCard
// ✔ EF Core genererà automaticamente la tabella se non esiste (Code First)
```

# Program.cs // SOLO AGGIUNTA // INCLUSO
```c#
builder.Services.AddScoped<ProiezioneService>();
// ✔ Registra ProiezioneService nel Dependency Injection container
// ✔ Lifetime: Scoped → una nuova istanza per ogni richiesta HTTP
// ✔ Ideale per servizi che usano DbContext (anch’esso scoped)
// ✔ Permette ai controller di ricevere ProiezioneService tramite costruttore
```