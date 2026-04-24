# ERRORI DA SISTEMARE

## CURL

- GiftCard/LeggiGiftCardTramiteIdGiftCard

- Abbonamento/LeggiAbbonamentoTramiteIdAbbonamento

- Utente/GiftCard (Da l'errore sbagliato se hai già l'abbonamento)

- Utente/Abbonati (Da l'errore sbagliato se hai già l'abbonamento)

- Admin/LeggiProfiloTramiteIdUtente (Da errore in runtime se non trova l'utente)

- Admin/LeggiAbbonamentoPerIdAbbonamento

- Admin/LeggiUtentiPerIdAbbonamento

- Admin/LeggiGiftCardTramiteIdGiftCard

- Admin/LeggiUtentiPerIdGiftCard

- CONTROLLARE FUNZIONI POST SE E' SBAGLIATO L'ID DA ERRORE IN RUNTIME

## Generale

- Nella migrations builder viene creato turnoID ma non esiste nel modello
