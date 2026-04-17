# Task
- Leggere il readme v.1 e v.2 e readme dei comandi curl.
- Annotarsi se ci sono passaggi poco chiari e se può essere migliorato in qualche modo.
- Aggiornare questo readme ogni volta che viene annotato e testato qualcosa.

## Testing.
- Alpha testing:

Services:

Prima fase 17/04/2026


|Assegnato| user | task | feedback |
|---|---|---|---|
| |Greg|abbonamento| Creazione, Modifica. |
| |Marco|turno| Creazione| Modifica|
| |Francesco|Sala | creazione| modifica |
| |Simeone| | |


Seconda fase

|Assegnato| user | task | feedback |
|---|---|---|---|
| |Greg|Acquisto|Creazione, Modifica|
| |Marco|DtoLogAzioni-LogAzioniService (il service deve essere una classe statica con le conseguenti modifiche)||
| |Francesco|Sala | creazione| modifica |
| |Simeone|Acquisto|Creazione, Modifica|


terza fase

|Assegnato| user | task | feedback |
|---|---|---|---|
| |Greg|      |         |
| |Marco|DtoLogAzioni-LogAzioniService|        |
| |Simeone|          |        |






## Fix, risoluzione di problemi.


## Improvements (implementazioni)

- Francesco: Dto di proiezione.


## Annotazioni varie.

|user|file|commento| risposta
|---|---|---|---|
|Greg| AbbonamentoController.cs | BUG: Tutti sono autorizzati alla creazione dell'abbonamento|  |
|Greg| AbbonamentoController.cs | BUG: GET ID e GET ADMIN/ID sono identici non essendoci nessun controllo |  |
|Marco|  |  | 
|Francesco|  | 

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
















