Controllare se funzionano tutte le eccezioni testando le chiamate tramite postman e controllando se c'è un messaggio per ogni "errore" tipo inserimento di una stringa invece che int, modifica della sala con il nome di una sala già esistente ecc...
Nel caso non fossero presenti aggiungerle:


## Greg:

Abbonamento; (fatto)

Giftcard; (fatto)

Admin; (da fare) 

Auth; (da fare)


## Andrea P:

GenereMovie; (da fare)

GestoreUtenti; (da fare)

Utente; (da fare)


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



Acquisto; (Fabio??)





