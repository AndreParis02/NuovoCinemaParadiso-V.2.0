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


||string|Id||Id|
||string|Nome|Nome|Nome|
||TimeOnly|OraInizio|OraInizio|OraInizio|
||TimeOnly|OraFine|OraFine|OraFine|
||List<Sala>|Sale|||

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

## GiftCard
TipoDato|Model|Dto In|Dto Out|
|---|---|---|---|
|string|Id| |Id|
|string|Nome|Nome|Nome|
|int|Durata|Durata|Durata|
|decimal|Prezzo|Prezzo|Prezzo|
|int|NumeroMovie|NumeroMovie|NumeroMovie|
|List<Utente>|Utenti| | |
