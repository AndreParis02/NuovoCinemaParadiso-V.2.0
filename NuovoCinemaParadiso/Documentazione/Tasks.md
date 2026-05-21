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

- ANDREA PARIS:CONTROLLARE IL README (GIFTCARD, GENEREMOVIE, CONTOCINEMA) IN CORSO

- FABIO: - CONTROLLARE LA RISCOSSIONE DELLA GIFTCARD SU POSTMAN - FATTO
         - MODIFICA DEL SALDO INIZIALE DI UN UTENTE CHE SI REGISTRA. FATTO

- ANDREA BRUNO: CONTROLLARE I DTO ORA CHE NON è PRESENTE LA GIFT CARD - FATTO

- ANDREA BRUNO: CONTROLLARE I DTO CHE COMPRENDONO IL SALDO DELL'UTENTE SIA NEI FILE CHE NEL README. - FATTO

- CONTROLLARE RICARICA E RISCOSSIONE CODICE ( MAGARI CAMBIARE IL NOME DA GIFTCARD A CODICERISCATTO? O UN NOME IDONEO)

- Simeone: VERIFICARE CHE L'ABBONAMENTO SCALI I SOLDI DAL SALDO (Fatto)
- FRANCESCO: CONTROLLARE LE OPERAZIONI DI ACQUISTO BIGLIETTO, TRASFERIMENTO DENARO SU CONTO CINEMA. - IN CORSO.

- SIMEONE: CONTROLLARE CHE SIA SOLO IL GESTORE A LEGGERE GLI AUDIT(LOG). - FATTO

- LORENZO: - CONTROLLARE CHE SIA SOLO IL GESTORE A LEGGERE LA LISTA DEI BIGLIETTI. - FATTO
           

- MARCO: MODIFICA DEL CALCOLAPREZZOFINALE - IN CORSO



# TASKS 21/05/2026

MODIFICARE CONTROLLER E SERVICE CHE NON HANNO LA NECESSITà DI AVERE DTO IN USCITA CHE PORTINO DATI NON NECESSARI AL FRONTEND.
DI CONSEGUENZA ALCUNI AZIONI RIPORTERANNO SOLO UN TRUE O UN FALSE CON UN MESSAGGIO DI RIUSCITA O FALLIMENTO.

## SERVICES

- ANDREA PARIS:- TipologiaSalaService
               - ContoCinemaService (Fatto)

- FABIO: - GenereMovieService (NULLA DA MODIFICARE).
         - GestoreService - LogAzioni(fatto)  - OttieniBiglietti(IN CORSO).

- ANDREA BRUNO: - GiftCardService (Fatto)
                - LogazioniService (Fatto)

- GREG: - MovieService (fatto)
        - OperatoreService (fatto)

- Simeone: - ProiezioneService
           - RuoloUtenteService

- FRANCESCO: - SalaService
             - BigliettoService

- LORENZO: - AbbonamentoService (fatto)
           - UtenteService

- MARCO: - TurnoService
         - AuthService


## CONTROLLERS

- ANDREA PARIS:- BigliettoController
               - ContoCinemaController

- FABIO: - GenereMovieController
         - GestoreController

- ANDREA BRUNO: - GiftCardController (Fatto)
                - LogazioniController (Fatto)

- GREG: - MovieController
        - OperatoreController

- Simeone: - ProiezioneController
           - RuoloUtenteController

- FRANCESCO: - SalaController
             - TipologiaSalaController

- LORENZO: - AbbonamentoController (fatto)
           - UtenteController

- MARCO: - TurnoController
         - AuthController