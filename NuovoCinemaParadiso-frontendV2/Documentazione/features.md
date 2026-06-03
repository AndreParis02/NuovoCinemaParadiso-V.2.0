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
- abbonamento-list.component.ts [tutti]
- abbonamento-detail.component.ts [utente](abbonato)
- abbonamento-form.component.ts [operatore]

## biglietto
### components
- biglietto-list.component.ts [utente,operatore,gestore]
- biglietto-form.component.ts [operatore]

## dashboard
### layout 
- dashboard.layout.ts
 

## profilo
### components
- profilo.component.ts [utente,operatore,gestore]


## genere-movie
### components
- genere-movie-list.component.ts [tutti]

## codice riscatto(giftcard)
### components
- riscatta-codice.component.ts
- crea-codice.component.ts
- giftcared-lista.component.ts [gestore,utente]

## auth 
### page
### components
- login.component.ts
- register.component.ts

## movie
### components
- movie-lista.component.ts [operatore]
- movie-form.component.ts [operatore]

## proiezione
### page
### components
- proiezione-lista.component.ts [tutti]
- proiezione-form.component.ts [operatore]
- proiezione-detail.component.ts [tutti]

## sala
### components
- sala-lista.component.ts [operatore]
- sala-form.component.ts [operatore]

## tipologia-sala
### components
- tipologia-sala-lista.component.ts [operatore]
- tipologia-sala-form.component.ts [operatore]

## turno
### components
- turno-lista.component.ts [operatore]
- turno-form.component.ts [operatore]

## cambio-ruolo
### components
- cambio-ruolo-form.component.ts [gestore]
- utenti-lista.component.ts [gestore]

## log
### components
- log-lista.component.ts [gestore]

## conto-cinema
### components
- conto-cinema-detail.component.ts [gestore]


