# Angular


## Analisi iniziale:
Prima di tutto si definiscono queste cose:

- Elenco Tutti endpoint;
- quali dati che entrano e escono;
- quali endpoint che richiedono il login; 
- quali endpoint richiedono ruoli ben precisi;

- leggere tutti i dto e creare le interfacce typescript;

|Assegnato| user | task |
|---|---|---|
|x|Andrea B.|LogAzioni, Login, ModificaRuoloUtente|
|x|Andrea P.|Abbonamento, Acquisto, GiftCard|
|x|Fabio|AuthResponse, GenereMovie, Movie| 
|x|Greg|Proiezione, Registrazione, Sala|
|x|Lorenzo|TipologiaSala, Turno, Utente| 


## TipologiaSala
|TipoDato|Model|Dto In|Dto Out|
|---|---|---|---|
|string|Id| |Id|
|string|Nome|Nome|Nome|
|decimal|MaggiorazionePrezzo|MaggiorazionePrezzo|MaggiorazionePrezzo|
|List<Sala>|Sale|||

## Turno
|TipoDato|Model|Dto In|Dto Out|
|---|---|---|---|
|string|Id||Id|
|string|Nome|Nome|Nome|
|decimal|MaggiorazionePrezzo|MaggiorazionePrezzo|MaggiorazionePrezzo|
|List<Sala>|Sale|||

## Utente
|TipoDato|Model|Dto In|Dto Out|
|---|---|---|---|
|string|Id||Id|
|string|NomeCompleto|NomeCompleto|NomeCompleto|
|string|Email||Email|
|int|Eta|Eta|Eta|
|DateTimeOffset|DataInizioAbbonamento||DataInizioAbbonamento|
|DateTimeOffset|DataInizioGiftCard||DataInizioGiftCard|
|bool|SeAbbonato||SeAbbonato|
|bool|PossiedeGiftCard||PossiedeGiftCard|
|string|AbbonamentoId||AbbonamentoId|
|Abbonamento|Abbonamento|||
|string|||TipoAbbonamento|
|string|GiftCardId||GiftCardId|
|GiftCard|GiftCard|||
|string|||TipoGiftCard|
|List<Acquisto>|Acquisti|||

## Authresponse
|TipoDato|Model (Utente)|Dto In(DtoLogin)|Dto Out|
|---|---|---|---|
|string|||Id|
|string|NomeCompleto||NomeCompleto|
|string|||Token|
|int|Eta||Eta|
|string|Email|Email|Email|
|string||Password||
|string|||Ruolo|
|DateTimeOffset|||DataInizioAbbonamento|
|DateTimeOffset|||DataInizioGiftCard|
|bool|||SeAbbonato|
|bool|||PossiedeGiftCard|

## GenereMovie
|TipoDato|Model|Dto In|Dto Out|
|---|---|---|---|
|string|Id||Id|
|string|Genere|Genere|Genere|
|List<Movie>|Movies|||

## Movie
|TipoDato|Model|Dto In|Dto Out|
|---|---|---|---|
|string|Id||Id|
|string|Titolo|Titolo|Titolo|
|string|Descrizione|Descrizione|Descrizione|
|int|DurataMinuti|DurataMinuti|DurataMinuti|
|decimal|PrezzoMovie|PrezzoMovie|PrezzoMovie|
|string|GenereId|GenereId|GenereId|
|string|||Genere|
|GenereMovie|Genere|||
|List<Acquisto>|Acquisti|||

## Abbonamento
TipoDato|Model|Dto In|Dto Out|
|---|---|---|---|
|string|Id| |Id|
|string|Nome|Nome|Nome|
|int|Durata|Durata|Durata|
|decimal|Prezzo|Prezzo|Prezzo|
|int|Sconto|Sconto|Sconto|
|List<Utente>|Utenti| | |

## Acquisto
TipoDato|Model|Dto In|Dto Out|
|---|---|---|---|
|string|Id| |Id|
|string|ProiezioneId|ProiezioneId|ProiezioneId|
|Proiezione|Proiezione| | |
|string|UtenteId||UtenteId|
|Utente|Utente| | |
|decimal|PrezzoFinale||PrezzoFinale|
|DateTimeOffset|OrarioCreazione||OrarioCreazione|
|int|NumeroBiglietti|NumeroBiglietti|NumeroBiglietti|
|string|MetodoPagamento|MetodoPagamento|MetodoPagamento|

## LogAzioni
|LogAzioni|DtoLogAzioni|DtoCreazioneLogAzioni|
|---|---|---|
|string|Id|Id|Id|
|string|IdUtente|IdUtente|IdUtente|
|string|NomeAzione|NomeAzione|NomeAzione|
|bool|Effettuato|Effettuato|Effettuato|
|string|Messaggio|Messaggio|Messaggio|
|DateTimeOffset|TimeStamp|TimeStamp|TimeStamp|

## Login
|TipoDato|Utente|DtoLogin|DtoAuthResponse|
|---|---|---|---|
|string||Id|
|string|NomeCompleto||NomeCompleto|
|string|||Token|
|int|Eta||Eta|
|string||Email|Email|
|string||Password||
|string|||Ruolo|
|DateTimeOffset|DataInizioAbbonamento||DataInizioAbbonamento|
|DateTimeOffset|DataInizioGiftCard||DataInizioGiftCard|
|bool|SeAbbonato||SeAbbonato|
|bool|PossiedeGiftCard||PossiedeGiftCard|
|List<Acquisto>|Acquisti|||
|string|AbbonamentoId|||
|Abbonamento|Abbonamento|||
|string|GiftCardId|||
|GiftCard|GiftCard|||

## ModificaRuoloUtente
|TipoDato|Utente|DtoModificaRuoloUtente|DtoModificaRuoloUtente|
|---|---|---|---|
|string||Email|Email|
|string||NuovoRuolo|Ruolo|
|string|||messaggio|

## GiftCard
TipoDato|Model|Dto In|Dto Out|
|---|---|---|---|
|string|Id| |Id|
|string|Nome|Nome|Nome|
|int|Durata|Durata|Durata|
|decimal|Prezzo|Prezzo|Prezzo|
|int|NumeroMovie|NumeroMovie|NumeroMovie|
|List<Utente>|Utenti| | |

## Proiezione

|TipoDato|Model|Dto In|Dto Out|
|---|---|---|---|
|string|Id||Id|
|string|DataProiezione|DataProiezione|DataProiezione|
|ForeignKey|MovieId|MovieId|MovieId|
|ForeignKey|SalaId|SalaId|SalaId|
|ForeignKey|TurnoId|TurnoId|TurnoId|
|List|Acquisti|||

## Registrazione
|TipoDato|Model|Dto In|Dto Out|
|---|---|---|---|
|string||Email||
|string||Password||
|string||NomeCompleto||
|int||Eta||

## Sala
|TipoDato|Model|Dto In|Dto Out|
|---|---|---|---|
|string|Id||Id|
|string|Nome|Nome|Nome|
|int|Capeinza|Capeinza|Capeinza|
|List|Acquisti|||
|String|TipologiaSalaId|TipologiaSalaId|TipologiaSalaId|

# ENDPOINT

|Controller|Tipo di chiamata|Endpoint| Login Richiesto | Ruolo richiesto|
|---|---|---|---|---|

## AuthController

|Tipo di chiamata|Endpoint| Login Richiesto | Ruolo richiesto|
|---|---|---|---|
|POST|Auth/registrazione| No| Nessuno|
|GET|Auth/profilo| Richiesto | Qualsiasi|
|PUT|Auth/modifica| Richiesto[Authorize] | Gestore o operatore|
|DELETE|Auth/elimina | Richiesto | Qualsiasi|


## GenereMovie

|Tipo di chiamata|Endpoint| Login Richiesto | Ruolo richiesto|
|---|---|---|---|
|GET|/GenereMovie| Richiesto| Qualsiasi |
|GET|/GenereMovie/{id}| Richiesto | Qualsiasi|
|POST|/GenereMovie| Richiesto[Authorize] | Gestore o operatore|
|PUT|/GenereMovie/{id} | Richiesto[Authorize] | Gestore o Operatore |
|DELETE|/GenereMovie/{id} | Richiesto[Authorize] | Gestore o Operatore |

## Movie

|Tipo di chiamata|Endpoint| Login Richiesto | Ruolo richiesto|
|---|---|---|---|
|GET|/Movie| Si| |
|GET|/Movie/{Id}| Si | Utente|
|GET|/Movie/genereMovie/{genereId}| Si | Utente|
|POST|/Movie| Si | Gestore o operatore|
|PUT|/Movie/{id} | Si | Gestore o Operatore |
|DELETE|/Movie/{id} | Si | Gestore o Operatore |

## Acquisto

|Tipo di chiamata|Endpoint| Login Richiesto | Ruolo richiesto|
|---|---|---|---|
|GET|/Acquisto| Richiesto|Qualsiasi|
|GET|/Acquisto/{id}| Richiesto |Qualsiasi|
|POST|/Acquisto| Richiesto |Qualsiasi|
|PUT|/Acquisto/{id} | Richiesto[Authorize] | Gestore o Operatore |
|DELETE|/Acquisto/{id} | Richiesto[Authorize] | Gestore o Operatore |

## GiftCard

|Tipo di chiamata|Endpoint| Login Richiesto | Ruolo richiesto|
|---|---|---|---|
|GET|/GiftCard| Richiesto|Qualsiasi|
|GET|/GiftCard/{id}| Richiesto |Qualsiasi|
|POST|/GiftCard| Richiesto[Authorize] |Gestore o Operatore|
|PUT|/GiftCard/{id} | Richiesto[Authorize] | Gestore o Operatore |
|DELETE|/GiftCard/{id} | Richiesto[Authorize] | Gestore o Operatore |

## TipologiaSalaController.cs

|Tipo chiamata|Endpoint|Login richiesto|Ruolo richiesto|
|---|---|---|---|
|GET|TipologiaSala|Richiesto|Qualsiasi|
|GET|TipologiaSala/id|Richiesto|Qualsiasi|
|POST|TipologiaSala|Richiesto[Authorize]|GestoreoOperatore|
|PUT|TipologiaSala/id|Richiesto[Authorize]|GestoreoOperatore|
|DELETE|TipologiaSala/id|Richiesto[Authorize]|GestoreoOperatore|

## TurnoController.cs
|Tipo chiamata|Endpoint|Login richiesto|Ruolo richiesto|
|---|---|---|---|
|GET|TipologiaSala|Richiesto|Qualsiasi|
|GET|TipologiaSala/id|Richiesto|Qualsiasi|
|POST|TipologiaSala|Richiesto[Authorize]|GestoreoOperatore|
|PUT|TipologiaSala/id|Richiesto[Authorize]|GestoreoOperatore|
|DELETE|TipologiaSala/id|Richiesto[Authorize]|GestoreoOperatore|

## Sala

|Tipo di chiamata|Endpoint| Login Richiesto | Ruolo richiesto|
|---|---|---|---|
|GET|/Sala|Richiesto|Qualsiasi|
|GET|/Sala/TipologiaSala/{tipologiaId}|Richiesto| |
|GET|/Sala/{Id}|Richiesto|Qualsiasi|
|POST|/Sala/|Richiesto[Authorize]| Gestore o Operatore |
|PUT|/Sala/{Id}|Richiesto[Authorize]| Gestore o Operatore |
|DELETE|/Sala/{Id}|Richiesto[Authorize]| Gestore o Operatore |

## Proiezione

|Tipo di chiamata|Endpoint| Login Richiesto | Ruolo richiesto|
|---|---|---|---|
|GET|/Proiezione|Richiesto|Qualsiasi|
|GET|/Proiezione/{id}|Richiesto|Qualsiasi|
|GET|/Proiezione/turno/{TurnoId}|Richiesto|Qualsiasi|
|GET|/Proiezione/sala/{salaId}|Richiesto|Qualsiasi|
|GET|/Proiezione/movie/{movieId}|Richiesto|Qualsiasi|
|POST|/Proiezione|Richiesto[Authorize]| Gestore o Operatore |
|PUT|/Proiezione/{id}|Richiesto[Authorize]| Gestore o Operatore |
|DELETE|/Proiezione/{id}|Richiesto[Authorize]| Gestore o Operatore |

## Abbonamento

|TipoChiamata|Endpoint|LoginRichiesto|RuoloRichiesto|
|---|---|---|---|
|GET|/Abbonamento|Richiesto|Utente|
|GET|/Abbonamento/Id|Richiesto|Utente|
|POST|/Abbonamento|Richiesto[Authorize]|GestoreOrOperatore|
|PUT|/Abbonamento/Id|Richiesto[Authorize]|GestoreOrOperatore|
|DELETE|/Abbonamento/Id|Richiesto[Authorize]|GestoreOrOperatore|

## GestoreUtenti

|TipoChiamata|Endpoint|LoginRichiesto|RuoloRichiesto|
|---|---|---|---|
|PUT|/GestoreUtenti/cambia-ruolo|Richiesto[Authorize]|Gestore|

## Admin

|TipoChiamata|Endpoint|LoginRichiesto|RuoloRichiesto|
|---|---|---|---|
|GET|/Admin/listaUtenti|Richiesto[Authorize]|GestoreOrOperatore|
|GET|/Admin/ricercaProfilo/Id|Richiesto[Authorize]|GestoreOrOperatore|
|DELETE|/Admin/eliminaUtente/Id|Richiesto[Authorize]|GestoreOrOperatore|
|GET|/Admin/acquisto|Richiesto[Authorize]|GestoreOrOperatore|
|GET|/Admin/acquisto/Id|Richiesto[Authorize]|GestoreOrOperatore|
|GET|/Admin/utenti/abbonamento/Id_abbonamento|Richiesto[Authorize]|GestoreOrOperatore|
|GET|/Admin/utenti/giftCard/Id_giftCard|Richiesto[Authorize]|GestoreOrOperatore|
|GET|/Admin/log|Richiesto[Authorize]|Gestore|
|GET|/Movie| Richiesto|Qualsiasi|
|GET|/Movie/{Id}| Richiesto | Qualsiasi|
|GET|/Movie/genereMovie/{genereId}| Richiesto | Qualsiasi|
|POST|/Movie| Richiesto[Authorize] | Gestore o operatore|
|PUT|/Movie/{id} | Richiesto[Authorize] | Gestore o Operatore |
|DELETE|/Movie/{id} | Richiesto[Authorize] | Gestore o Operatore |



Creare una tabella per le operazioni crud per entità

Levare l'autorizzazione per gli ottieni tutti (per renderli visibili anehe senza registrazione) e integrare l'implementazione dell'età

le Modifiche su che dati lavorano e di cosa ho bisogno