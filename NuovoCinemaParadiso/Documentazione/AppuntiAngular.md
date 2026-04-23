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