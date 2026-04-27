# ERRORI DA SISTEMARE


## PROBLEMI MIGRATION 

Sono andrea b scrivo per non dimenticarmi, ho testato il programma dopo aver inserito il nuovo dataSeeder e ho nuotato due errori.

- Sala espone ancora la tabella TurnoId (Andrea P)
- Acquisto espone ancora le tabelle MovieId e SalaId (Simeone)

Ho controllato tutto il codie ed è tutto apposto poi però ho controllato le Migration, e sono tutte impostate con tabelle e Foreign Key sbagliate e sovrascritte.
Il problema è che anche togliendo le migrazioni, vengono ricreate uguali e da quanto ho capito vengano create in base al bin e obj, e per questo anche eliminate rimangono uguali.
Ho provato a cancellare le cartelle bin e obj per poi runnare dinuovo il progetto ma niente, le mie competenze finiscono qui lunedì controlliamo tutti insieme.

## CURL

- GiftCard/LeggiGiftCardTramiteIdGiftCard (Fabio)

- Utente/GiftCard (Da l'errore sbagliato se hai già l'abbonamento) (Greg)

- Utente/Abbonati (Da l'errore sbagliato se hai già l'abbonamento) (Greg)

Testarli per vedere tutte le risposte errate; (Marco)

- Admin/LeggiProfiloTramiteIdUtente (Da errore in runtime se non trova l'utente) (funziona)

- Admin/LeggiAbbonamentoPerIdAbbonamento (funziona)

- Admin/LeggiUtentiPerIdAbbonamento (funziona)

- Admin/LeggiGiftCardTramiteIdGiftCard

- Admin/LeggiUtentiPerIdGiftCard

- collegare i bool dell'utente a gift card e abbonamento per il quale è collegato

- CONTROLLARE FUNZIONI POST SE E' SBAGLIATO L'ID DA ERRORE IN RUNTIME (Francesco)

## Generale

- Nella migrations builder viene creato turnoID ma non esiste nel modello
