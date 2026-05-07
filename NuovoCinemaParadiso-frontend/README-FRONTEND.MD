# NUOVO CINEMA PARADISO

# IMPORTANTISSIMO

UNA VOLTA CHE VIENE ASSEGNATA UNA TASK, AL COMPLETAMENTO DELLA STESSA BISOGNA AGGIORNARE QUESTA DOCUMENTAZIONE.
SEGUIRE LO SCHELETRO PRINCIPALE.
## Setup installazioni Node.js e Angular

Attraverso la powershell, come amministratore, digitare il seguente comando per installare Node.JS(la versione deve essere allineate alla documentazione Angular
che ne attesta la compatibilità):
```bash
choco install nodejs
```

nel terminale su Visual studio Code, per angular, digitare:
```bash
npm install -g @angular/cli
```

## Creazione del programma

Per creare la nostra applicazione frontend, digitare il seguente comando:

```bash
ng new nomeapplicazione --routing --style=scss --standalone --skip-tests
```

--routing
→ aggiunge automaticamente il sistema di routing Angular.

Ti crea i file per gestire le pagine/navigazione dell’app:

- app.routes.ts
- configurazione del router

Serve per fare cose tipo:

- /login
- /dashboard
- /utenti

--style=scss
→ imposta SCSS come preprocessore CSS invece del CSS normale.

Vantaggi:

- variabili ($primary-color)
- nesting
- mixin
- file più organizzati


--standalone
- usa la nuova architettura Angular standalone invece dei classici NgModule.

Quindi:

- niente app.module.ts
- i componenti si importano direttamente
- progetto più moderno e leggero

È lo standard consigliato nelle versioni recenti di Angular.

--skip-tests
→ evita la creazione automatica dei file di test.

Senza questo flag Angular genera:

*.spec.ts

Con questo flag, il progetto più pulito e hai meno file iniziali.


## IMPORTANTE

Una volta effettuato il pull dal developer, ad inizio progetto, essendo la cartella node_modules inserita nel gitignore, bisognerà digitare il seguente comando
```bash
npm install
```
per avere la cartella in locale in modo tale che il progetto possa essere, successivamente, avviato.


## Creazione dei componenti

# models

Contiene solo interfacce TypeScript.

Serve per descrivere la forma dei dati.

- Trasportare manualmente tutti i modelli del backend (esempio: login.model.ts)


## services

Contiene la logica HTTP.

Qui fai:
- chiamate ai backend
- logica riusabile collegata ai dati

Esempi:

- auth.service.ts
ecc ecc

## pages

Contiene le schermate vere:

 - login
 - register
 - dashboard
 - ecc ecc

 ## core

 Contiene infrastruttura applicativa globale:

 - guard
 - interceptor

 ## shared

 Contiene componenti riusabili.

Nel nostro caso la navbar.




## PAGES

## MODELS

## SERVICES

## GUARDS

## INTERCEPTORS

## SHARED

## ENVIRONMENT