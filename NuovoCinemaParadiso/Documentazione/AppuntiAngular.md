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
