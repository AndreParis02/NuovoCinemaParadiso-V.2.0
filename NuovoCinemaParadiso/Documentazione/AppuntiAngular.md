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

