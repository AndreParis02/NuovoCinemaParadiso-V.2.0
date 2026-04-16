# Task
- Leggere il readme v.1 e v.2 e readme dei comandi curl.
- Annotarsi se ci sono passaggi poco chiari e se può essere migliorato in qualche modo.
- Aggiornare questo readme ogni volta che viene annotato e testato qualcosa.

## Testing.
- Alpha testing:

Services:

Prima fase


|Assegnato| user | task | feedback |
|---|---|---|---|
| |Greg|abbonamento| Creazione, Modifica. |
| |Marco|turno| Creazione, Modifica. |
| |Simeone|generemovie| Creazione, Modifica. |


Seconda fase

|Assegnato| user | task | feedback |
|---|---|---|---|
| |Greg|Sala|Creazione, Modifica |
| |Marco|Movie| Creazione, Modifica|
| |Simeone|Acquisto|Creazione, Modifica, calcolo |






## Fix, risoluzione di problemi.


## Improvements (implementazioni)

- Francesco: Dto di proiezione.


## Annotazioni varie.

|user|file|commento
|---|---|---|
|Greg| | |
|Marco| AcquistiController | perché si crea un DTO DtoCreazioneLogAzioni? |
|Marco| AcquistiController | perché il costruttore di DtoCreazioneLogAzioni non inizializza il timestamp? |
|Marco| AcquistiController | string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier); cosa succede se utenteId==null? |
|Marco| AuthController | RicercaProfiloLoggato() non logga il fallimento dell'azione. Accade anche in altri metodi |
|Marco| RuoloUtenteService | Perché ModificaRuoloUtente() non ritorna semplicemente un true o false? |
|Marco| MoviesController | Perché OttieniPerGenere() logga un fallimento se trova zero film? |
|Marco| Program.cs | perché builder.Services.AddScoped aggiunge JwtHelper e non CalcoliHelper? |
|Marco| DTO | noi abbiamo suddiviso i DTO in Request e Response |
|Marco| UtentiController | AbbonatiAsync() perché non preleva direttamente l'abbonamento tramite abbonamentoId? |















OttieniTuttiGliAcquistiUtente

|Simeone| | |

