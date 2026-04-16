entità proiezione con:

IdProiezione;
SalaId;
FilmId;
TurnoId; (rimuoverlo da Sala, vecchio FasciaOraria)
DataProiezione;

---

Sala:
TipologiaSalaId;
FasciaOrariaId;

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
Sala Nuova:
TipologiaSalaId;
