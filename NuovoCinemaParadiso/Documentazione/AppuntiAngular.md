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
| |Andrea B.|LogAzioni, Login, ModificaRuoloUtente|
|x|Andrea P.|Abbonamento, Acquisto, GiftCard|
| |Fabio|AuthResponse, GenereMovie, Movie| 
| |Greg|Proiezione, Registrazione, Sala|
| |Lorenzo|TipologiaSala, Turno, Utente| 


## TipologiaSala
|Model|Dto In|Dto Out|
|---|---|---|
|string Id|string Nome|string Id
|string Nome|decimal MaggiorazionePrezzo|string Nome
|decimal MaggiorazionePrezzo||decimal MaggiorazionePrezzo
|List Sale||

## Turno
|Model|Dto In|Dto Out|
|---|---|---|
|string Id|Required TimeOnly OraInizio|string Id
|Required TimeOnly OraInizio|Required TimeOnly OraFine|TimeOnly OraInizio
|Required TimeOnly OraFine|string Nome|TimeOnly OraFine
|string Nome||string Nome
|List Sale||


## Utente
|Model|Dto In|Dto Out|
|---|---|---|
|string NomeCompleto|Required string NomeCompleto|string Id
|int Eta|Required Eta|string NomeCompleto
|bool SeAbbonato||DateTimeOffset DataInizioAbbonamento
|bool PossiedeGiftCard||DateTimeOffset DataInizioGiftCard
|DateTimeOffset DataInizioAbbonamento||bool SeAbbonato
|DateTimeOffset DataInizioGiftCard||bool PossiedeGiftCard
|List Acquisti||string Email
|Abbonamento? Abbonamento||int Eta
|string GiftCard||string AbbonamentoId
|GiftCard? GiftCard||string GiftCard
|||string TipoAbbonamento
|||string TipoGiftCard
## Authresponse
|Model|Dto In|Dto Out|
|---|---|---|
||(DTOLOGIN) string Email, string Password|string Id, string NomeCompleto, string Token, int Eta, string Email , string Ruolo , DateTimeOffset DataInizioAbbonamento, DateTimeOffset DataInizioGiftCard, bool SeAbbonato,bool PossiedeGiftCard|


## GenereMovie
|Model|Dto In|Dto Out|
|---|---|---|
|Guid String, string Genere, List Movies|string Genere|string Id, string Genere|

## Movie
|Model|Dto In|Dto Out|
|---|---|---|
|Guid String, string Titolo, int DurataMinuti, String Descrizione, decimal PrezzoMovie, string GenereId, string Genere[foreign key], List Acquisti|string Titolo, string Descrizione, int DurataMinuti, decimal PrezzoMovie, string GenereId, string Genere |string Id, string Titolo, string Descrizione, int DurataMinuti, decimal PrezzoMovie, string GenereId, string Genere|

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

## GiftCard
TipoDato|Model|Dto In|Dto Out|
|---|---|---|---|
|string|Id| |Id|
|string|Nome|Nome|Nome|
|int|Durata|Durata|Durata|
|decimal|Prezzo|Prezzo|Prezzo|
|int|NumeroMovie|NumeroMovie|NumeroMovie|
|List<Utente>|Utenti| | |
# Greg
|Model|Dto In|Dto Out|
|---|---|---|
|Proiezione.cs| DtoCreazioneProiezione.cs|Dtoproiezione.cs|
||DtoRegostrazione.cs||
|Sala.cs|DtoCreazioneSala.cs|DtoSala.cs|
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