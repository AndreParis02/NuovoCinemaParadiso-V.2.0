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
| |Andrea P.|Abbonamento, Acquisto, GiftCard|
| |Fabio|AuthResponse, GenereMovie, Movie| 
| |Greg|Proiezione, Registrazione, Sala|
| |Lorenzo|TipologiaSala, Turno, Utente| 



|Model|Dto In|Dto Out|
|---|---|---|

## LogAzioni

|LogAzioni|DtoLogAzioni|DtoCreazioneLogAzioni|
|---|---|---|
|string Id|string Id|string Id|
|string IdUtente|string IdUtente|string IdUtente|
|string NomeAzione|string NomeAzione|string NomeAzione|
|bool Effettuato|bool Effettuato|bool Effettuato|
|string Messaggio|string Messaggio|string Messaggio|
|DateTimeOffset TimeStamp|DateTimeOffset TimeStamp|DateTimeOffset TimeStamp|

## Login

|Utente|DtoLogin|DtoAuthResponse|
|---|---|---|
|---|---|string Id|
|string NomeCompleto|---|string NomeCompleto|
|---|---|string Token|
|int Eta|---|int Eta|
|---|string Email|string Email|
|---|string Password|---|
|---|---|string Ruolo|
|DateTimeOffset DataInizioAbbonamento|---|DateTimeOffset DataInizioAbbonamento|
|DateTimeOffset DataInizioGiftCard|---|DateTimeOffset DataInizioGiftCard|
|bool SeAbbonato|---|bool SeAbbonato|
|bool PossiedeGiftCard|---|bool PossiedeGiftCard|
|List<Acquisto> Acquisti|---|---|
|string AbbonamentoId|---|---|
|Abbonamento Abbonamento|---|---|
|string GiftCardId|---|---|
|GiftCard GiftCard|---|---|

## ModificaRuoloUtente

|Utente|DtoModificaRuoloUtente|DtoModificaRuoloUtente|
|---|---|---|
|---|string Email|string Email|
|---|string NuovoRuolo|string Ruolo|
|---|---|string messaggio|
