# Features

## Dashboard
è una pagina dove tutti sono autorizzati ad entrare, in questa pagina si ha una visibilità differente in base all'autorizzazione:

- Guest: 
    - proiezioni
    - abbonamenti
- Utente: 
    - proiezioni
    - abbonamenti
    - profilo
    - abbonamento a cui l'utente è abbonato
- Operatore: 
- Gestore:

## abbonamento (Fatto)
### page
- abbonamento.page.ts
### components
- abbonamento-list.component.ts [tutti] - Andrea Paris (fatto)
- abbonamento-detail.component.ts [utente](abbonato) Francesco
- abbonamento-form.component.ts [operatore] Andrea Paris (fatto)

## auth (Fatto)
### page
### components
- login.component.ts
- register.component.ts
- register.component.html

## biglietto (Fatto)
### components
- biglietto-list.component.ts [utente,operatore,gestore] Andrea Bruno (fatto) 
- biglietto-form.component.ts [operatore] (Marco)

## cambio-ruolo (Fatto)
### components
- cambio-ruolo-form.component.ts [gestore] (Lorenzo)
- cambio-ruolo-form.component.html

- utenti-list.component.ts [gestore] Andrea Bruno (fatto)
- utenti-list.component.html 

## conto-cinema (Non fatto)
### components
- conto-cinema-detail.component.ts [gestore]

## dashboard (Fatto)
[priorità]
### layout  
Francesco (fatto)

- dashboard.layout.ts (Francesco: inserire nel layout di dashboard il component di profilo, biglietti, giftcard, abbonamento (e se non abbonato devono apparire gli abbonamenti disponibili))

(PER IL GESTORE: deve avere SOLO log, vedi giftcard, implementare cambio ruolo al posto dell'operatore)

## genere-movie (Fatto)
### components

- genere-movie-list.component.ts [tutti] Greg (fatto)
- genere-movie-list.component.html

## giftcard (Fatto)
### components
- riscatta-codice.component.ts (Simeone)

- crea-codice.component.ts (Simeone) Fatto!
- crea-codice.component.html (Simeone) Fatto!

- giftcard-form.component.ts (Simeone) Fatto!
- giftcard-form.component.html (Simeone) Fatto!

- giftcard-list.component.ts [gestore,utente] Simeone (Fatto!)
- giftcard-list.component.html [gestore,utente] Simeone (Fatto!)

## log (Fatto)
### components

- log-list.component.ts [gestore] Simeone (fatto!)
- log-list.component.html [gestore] Simeone (Fatto!)

## movie (Fatto)
### components

- movie-list.component.ts [operatore] Francesco (fatto)
- movie-list.component.html
- movie-form.component.ts
- movie-form.component.html

## profilo (Fatto)

### components
- profilo.component.ts [utente,operatore,gestore] 

## proiezione (Fatto)

### page
### components
- proiezione-list.component.ts [tutti] Fabio (fatto)
- proiezione-list.component.html 

- proiezione-form.component.ts [operatore]
- proiezione-form.component.html

- proiezione-detail.component.ts [tutti]
- proiezione-detail.component.html

## sala (Fatto)

### components
- sala-list.component.ts [operatore] Francesco (fatto)
- sala-list.component.html

- sala-form.component.ts [operatore] Francesco
- sala-form.component.html

## tipologia-sala (Fatto)
### components

- tipologia-sala-list.component.ts [operatore] Greg
- tipologia-sala-list.component.html [operatore] Greg

- tipologia-sala-form.component.ts [operatore] Greg
- tipologia-sala-form.component.html [operatore] Greg

## turno (Fatto)
### components

- turno-list.component.ts [operatore] Andrea paris (fatto)
- turno-form.component.ts [operatore] Andrea paris (fatto)


## shared/navbar (Fatto)
[priorità] (Fabio: modificare aggiungendo sale per l'operatore, rimuovendo profilo per tutti, crediti) (fatto)