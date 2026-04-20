## Implementazione Proiezione.

Implementare una funzionalità che permette al gestore di creare, modificare o eliminare una proiezione.
Essa comprende il film, la sala, il turno e la data di trasmissione.

- Modello e riferimento nel ContestoDb.
- Dtos.
- Services e riferimento nel Program.cs.
- Controller.
- Calcolo nell'helper.

entità proiezione con:

IdProiezione;
SalaId;
FilmId;
TurnoId; (rimuoverlo da Sala, vecchio FasciaOraria)
DataProiezione;

---

Sala:
TipologiaSalaId;

Film:
GenereId;

Utente:
AbbonamentoId;

---

Acquisto:

SalaId;
FilmId;
utenteId;

---

Creare ProiezioniController e ProiezioneService;

Cambiare da fasceOrarie a Turni;

Acquisto eliminare SalaId e FilmId e sostituirlo con ProiezioneId

Acquisto Nuovo:

ProiezioneId;
utenteId;

# Task

- Lorenzo: DtoProiezione da sistemare, DtoCreazioneProiezione e funzione ottieniperidasync
- Fabio: Controller di proiezione.