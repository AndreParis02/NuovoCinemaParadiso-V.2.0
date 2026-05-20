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

    public string MetodoPagamento { get; set; } = "standard"; // Metodo di pagamento scelto
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