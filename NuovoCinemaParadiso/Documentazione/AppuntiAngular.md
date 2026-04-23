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
| |Greg|Proiezione, Registrazione, Sala|
|x|Lorenzo|TipologiaSala, Turno, Utente| 


## TipologiaSala
|TipoDato|Model|Dto In|Dto Out|
|---|---|---|---|
|string Id|string Nome|string Id
|string Nome|decimal MaggiorazionePrezzo|string Nome
|decimal MaggiorazionePrezzo||decimal MaggiorazionePrezzo
|List Sale||

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
|GET|Auth/profilo| Si | Tutti|
|PUT|Auth/modifica| Si | Gestore o operatore|
|DELETE|Auth/elimina | Si | Tutti|


## GenereMovie

|Tipo di chiamata|Endpoint| Login Richiesto | Ruolo richiesto|
|---|---|---|---|
|GET|/GenereMovie| Si| |
|GET|/GenereMovie/{id}| Si | Utente|
|POST|/GenereMovie| Si | Gestore o operatore|
|PUT|/GenereMovie/{id} | Si | Gestore o Operatore |
|DELETE|/GenereMovie/{id} | Si | Gestore o Operatore |

## Movie

|Tipo di chiamata|Endpoint| Login Richiesto | Ruolo richiesto|
|---|---|---|---|
|GET|/Movie| Si| |
|GET|/Movie/{Id}| Si | Utente|
|GET|/Movie/genereMovie/{genereId}| Si | Utente|
|POST|/Movie| Si | Gestore o operatore|
|PUT|/Movie/{id} | Si | Gestore o Operatore |
|DELETE|/Movie/{id} | Si | Gestore o Operatore |

## Abbonamento

|TipoChiamata|Endpoint|LoginRichiesto|RuoloRichiesto|
|---|---|---|---|
|GET|/Abbonamento|SI|Utente|
|GET|/Abbonamento/Id|SI|Utente|
|POST|/Abbonamento|SI|GestoreOrOperatore|
|PUT|/Abbonamento/Id|SI|GestoreOrOperatore|
|DELETE|/Abbonamento/Id|SI|GestoreOrOperatore|

## GestoreUtenti

|TipoChiamata|Endpoint|LoginRichiesto|RuoloRichiesto|
|---|---|---|---|
|PUT|/GestoreUtenti/cambia-ruolo|SI|Gestore|

## Admin

|TipoChiamata|Endpoint|LoginRichiesto|RuoloRichiesto|
|---|---|---|---|
|GET|/Admin/listaUtenti|SI|GestoreOrOperatore|
|GET|/Admin/ricercaProfilo/Id|SI|GestoreOrOperatore|
|DELETE|/Admin/eliminaUtente/Id|SI|GestoreOrOperatore|
|GET|/Admin/acquisto|SI|GestoreOrOperatore|
|GET|/Admin/acquisto/Id|SI|GestoreOrOperatore|
|GET|/Admin/utenti/abbonamento/Id_abbonamento|SI|GestoreOrOperatore|
|GET|/Admin/utenti/giftCard/Id_giftCard|SI|GestoreOrOperatore|
|GET|/Admin/log|SI|Gestore|