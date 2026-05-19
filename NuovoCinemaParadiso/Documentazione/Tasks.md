Controllare se funzionano tutte le eccezioni testando le chiamate tramite postman e controllando se c'è un messaggio per ogni "errore" tipo inserimento di una stringa invece che int, modifica della sala con il nome di una sala già esistente ecc...
Nel caso non fossero presenti aggiungerle:


## Greg:

Abbonamento; (fatto)

Giftcard; (fatto)

Gestore; (fatto) 

Auth; (da fare)


## Andrea P:

GenereMovie; (Fatto)

GestoreUtenti; (Fatto)

Utente; (Fatto)


## Francesco: 

Movie;  (da fare)

TipologiaSala; (da fare)


## Lorenzo: 

Proiezione; (da fare)

Sala; (da fare)


## Simeone: 

Turno:
Se provavo ad eliminare un turno che aveva una proiezione associata tramite ForeignKey, giustamente, non mi permetteva di fare il curl DELETE, ma invece di darmi un errore lineare crashava il tutto e dava 500 Internal Server Error. Adesso, avendo modificato in TurnoService.ts e TurnoController.cs, da un messaggio di errore più chiaro e lineare senza far crashare l'intera webapi rstituendo un 400 Bad Request.

Piccolo plus: In TurnoService.cs ho aggiunto dei controlli logici in CreazioneAsync e ModificaAsync per far sì che non ci siano turni omonimi.



Biglietto; (fatto)





# FRONTEND TASKS

## Models
- Francesco: Abbonamento, biglietto, generemovie

- Greg: giftcard, logazioni, movie

- Fabio : proiezione, sala (FATTO)

- Lorenzo: tipologiasala, turno, utente



# TASKS 20/05/2026

- CONTROLLARE IL README (GIFTCARD, GENEREMOVIE, CONTOCINEMA)

- CONTROLLARE LA RISCOSSIONE DELLA GIFTCARD SU POSTMAN

- CONTROLLARE I DTO ORA CHE NON è PRESENTE LA GIFT CARD

- CONTROLLARE I DTO CHE COMPRENDONO IL SALDO DELL'UTENTE SIA NEI FILE CHE NEL README.

- CONTROLLARE RICARICA E RISCOSSIONE CODICE ( MAGARI CAMBIARE IL NOME DA GIFTCARD A CODICERISCATTO? O UN NOME IDONEO)