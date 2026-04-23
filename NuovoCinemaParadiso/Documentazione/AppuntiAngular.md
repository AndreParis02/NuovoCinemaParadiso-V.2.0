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