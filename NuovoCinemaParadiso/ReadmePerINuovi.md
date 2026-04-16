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

|user|file|commento| risposta
|---|---|---|---|
|Greg| | | |
|Marco| AcquistiController | perché si crea un DTO DtoCreazioneLogAzioni? | per avere un input di dati come negli altri dto, forse giusto il dto di output potrebbe essere omesso
|Marco| AcquistiController | perché il costruttore di DtoCreazioneLogAzioni non inizializza il timestamp? | perchè lo fa il service ma penso si possa gestire nell'entità o nel dto
|Marco| AcquistiController | string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier); cosa succede se utenteId==null? | teoricamente vuol dire che non sei loggato, quindi l'errore è nella risposta del curl ( non so dirti di preciso cosa esce, probabilmente non va direttamente il comando)
|Marco| AuthController | RicercaProfiloLoggato() non logga il fallimento dell'azione. Accade anche in altri metodi | non ha senso loggare il fallimento di alcune azioni che hanno riuscita solo se sei collegato
|Marco| RuoloUtenteService | Perché ModificaRuoloUtente() non ritorna semplicemente un true o false? | perchè è gestito da una task che accetta una stringa e deve tornare il ruolo
|Marco| MoviesController | Perché OttieniPerGenere() logga un fallimento se trova zero film? | (qua sono anche io d'accordo che il controllo deve essere gestito diversamente)
|Marco| Program.cs | perché builder.Services.AddScoped aggiunge JwtHelper e non CalcoliHelper? | una dimenticanza
|Marco| DTO | noi abbiamo suddiviso i DTO in Request e Response | ok noi in input output
|Marco| UtentiController | AbbonatiAsync() perché non preleva direttamente l'abbonamento tramite abbonamentoId? | in che senso?
















