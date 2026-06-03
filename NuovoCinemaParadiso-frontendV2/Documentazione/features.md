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

## abbonamento
### page
- abbonamento.page.ts
### components
- abbonamento-list.component.ts [tutti] - Andrea Paris
- abbonamento-detail.component.ts [utente](abbonato)
- abbonamento-form.component.ts [operatore]

## biglietto
### components
- biglietto-list.component.ts [utente,operatore,gestore] Andrea Bruno
- biglietto-form.component.ts [operatore]

## dashboard
### layout 
- dashboard.layout.ts
 

## profilo
### components
- profilo.component.ts [utente,operatore,gestore]


## genere-movie
### components
- genere-movie-list.component.ts [tutti] Greg

## giftcard
### components
- riscatta-codice.component.ts
- crea-codice.component.ts
- giftcard-list.component.ts [gestore,utente] Simeone

## auth 
### page
### components
- login.component.ts
- register.component.ts

## movie
### components
- movie-list.component.ts [operatore] Francesco
- movie-form.component.ts [operatore]

## proiezione
### page
### components
- proiezione-list.component.ts [tutti] Fabio
- proiezione-form.component.ts [operatore]
- proiezione-detail.component.ts [tutti]

## sala
### components
- sala-list.component.ts [operatore] Francesco
- sala-form.component.ts [operatore]

## tipologia-sala
### components
- tipologia-sala-list.component.ts [operatore] Greg
- tipologia-sala-form.component.ts [operatore]

## turno
### components
- turno-list.component.ts [operatore] Andrea paris
- turno-form.component.ts [operatore]

## cambio-ruolo
### components
- cambio-ruolo-form.component.ts [gestore]
- utenti-list.component.ts [gestore] Andrea Bruno

## log
### components
- log-list.component.ts [gestore] Simeone

## conto-cinema
### components
- conto-cinema-detail.component.ts [gestore]


