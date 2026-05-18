# Task
- Leggere il readme v.1 e v.2 e readme dei comandi curl.
- Annotarsi se ci sono passaggi poco chiari e se può essere migliorato in qualche modo.
- Aggiornare questo readme ogni volta che viene annotato e testato qualcosa.

## Testing.
- Alpha testing:

Services:

Prima fase 17/04/2026 finita


|Assegnato| user | task | feedback |
|---|---|---|---|
| |Greg|abbonamento| Creazione, Modifica. |
| |Marco|turno| Creazione| Modifica|
| |Francesco|Sala | creazione| modifica |
| |Simeone| | |


Seconda fase( Greg e Francesco hanno completato le tasks)

|Assegnato| user | task | feedback |
|---|---|---|---|
| |Greg|Biglietto|Creazione, Modifica|
| |Marco|DtoLogAzioni-LogAzioniService (il service deve essere una classe statica con le conseguenti modifiche)||
| |Francesco|Sala | creazione| modifica |
| |Simeone|Biglietto|Creazione, Modifica|


terza fase

|Assegnato| user | task | feedback |
|---|---|---|---|
| |Greg| Proiezione|Tutti gli ottieni(testare i curl)|
| |Marco||        |
| |Simeone|          |        |
| |Francesco|Proiezione |Creazione/Modifica/Elimina(testare i curl)|


quarta fase 20/04/2026

|Assegnato| user | task | feedback |
|---|---|---|---|
| |Greg| Log - Testare il curl di lettura del log e aggiungerlo nei comandi curl, aggiornare il readme 2.0 con le modifiche del logservice|
| |Marco||        |
| |Simeone|          |        |
| |Francesco|GiftCardController - creare gli endpoint - aggiornare il readme 2.0||






## Fix, risoluzione di problemi.


## Improvements (implementazioni)

- Francesco: Dto di proiezione.


## Annotazioni varie.

|user|file|commento| risposta
|---|---|---|---|
|Greg| AbbonamentoController.cs | BUG: Tutti sono autorizzati alla creazione dell'abbonamento|  |
|Greg| AbbonamentoController.cs | BUG: GET ID e GET GESTORE/ID sono identici non essendoci nessun controllo |  |
|Marco|  |  | 
|Francesco| ApplicationDbContext.cs  | PROBLEMA LOGICO: alla eliminazione di una proiezione, gli acquista collegati ad essa vengono eliminati a cascata 

|Greg| | |  |
|Marco|  |  | 


### MARCO
Test Creazione Turno
Creazione di TurnoController crea turni con nome duplicato sul db e poi restituisce un errore di turno già esistente.
Sembra che il turno venga creato senza nessun controllo, e il controllo sul nome che dovrebbe essere effettuato avviene tramite un Contain e non con un'uguaglianza di stringhe.

```bash
curl -s -X POST "http://localhost:5226/api/Turno" \
-H "Content-Type: application/json" \
-H "Authorization: Bearer $TOKEN" \
-d '{
    "oraInizio": "15:30:00",
    "oraFine": "18:00:00",
    "nome": "Pomeriggio5"
}' | jq
```

Restituisce turno già esistente e crea la fascia pomeriggio5 sul db.
Sul DB vedo 3 fasce orarie che hanno orainizio e orafine tipo 00:00:00.0000010. E' un errore del dataseeder?

Test Modifica turno
Il test sembra dare esito positivo, i valori sul db vengono modificati correttamente
















