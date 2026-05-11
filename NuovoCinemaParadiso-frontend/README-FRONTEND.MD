# NUOVO CINEMA PARADISO

# LUNEDI: IMPOSTARE LE CONVENZIONI PER LA STESURA DEL CODICE.

## Convenzioni

- Mantenere la lingua italiana.
- Utilizzare il camel case per le variabili e il pascal case per le funzioni.
- I nomi dei branch devono riportare la tipologia del file da modificare e il nome (Entità-Azione).
- Da adesso in poi verranno accettate solo le pull requests con il readme relativo alle modifiche fatte.


# STABILIRE L'ORDINE DELLE COSE DA FARE.

- Creare la cartella core, contenente guards, interceptors, services.
- Creare la cartella shared e decidere quali componenti sono condivisi fra le pagine. Se ci fossero
  più pagine che usano uno stesso modello, il modello condiviso si mette in shared.
- Creare la cartella features che contiene le entità:
  - TipologiaSala
  - Ruolo
  - Turno
  - GenereMovie
  - Sala
  - Movie
  - Utente
  - Abbonamento
  - GiftCard
  - Acquisto
  - Proiezione

  Ognuna di queste entità deve essere una cartella dentro features, ed ognuna deve contenere page, component, service e models.

  - Creare app.routes.ts dove verranno definite tutte le rotte.

 ## Passaggi

 - Creare i modelli angular che devono rispecchiare i DTO del backend e radunare richiesta e risposta
   all'interno dello stesso file ts.
 
 ( ESEMPIO CON DTO BACKEND E MODELLO ANGULAR IN TYPESCRIPT)

 ``` csharp
 using System.ComponentModel.DataAnnotations;

 namespace NuovoCinemaParadiso.Dtos;

 public class DtoSala
 {
    public string? Id {get; set;}

    [StringLength(100)]
    public string Nome {get; set;} = string.Empty;
    public int Capienza {get; set;}
    public string TipologiaSalaId {get; set;} = string.Empty;
    public string NomeTipologia {get; set;} = string.Empty;
 }
 ```

 ```ts
 export interface SalaCreazione {
    nome: string;
    capienza: number;
    tipologiaSalaId: string;
}

export interface Sala {
    id: string | null;
    nome: string;
    capienza: number;
    tipologiaSalaId: string;
    nomeTipologia: string;
}
 ```

 - Creare i services angular che chiamano il backend, il componente angular non si occupa delle chiamate
   http, quest'ultime vanno inserite nei services.
 - Creare le pagine angular seguendo le entità del backend
   Ci sarà: 
   - tipologiaSala-list.component.ts  (lista)
   - tipologiaSala-detail.component.ts (elemento singolo)
   - tipologiaSala-form.component.ts (pagina coi comandi crud)

```ts
 export class TipologiaSalaService {
    // url dove si trova il service
    private apiUrl = 'la rotta dell api backend'
    // costruttore con i metodi OttieniTutto e CreaTipologiaSala
    constructor(private http:HttpClient){}

    OttieniTutto(): observable<tipologieSala[]>{
     return this.http.get<tipologieSala[]>(this.apiUrl);
    }

    CreaTipologiaSala(tipologiaSala: CreaTipologiaSala): observable<TipologiaSala>{
     return this.http.post<tipologieSala[]>(this.apiUrl, tipologiaSala);

    } 
}

 ```
### Procedura

- Prima si fa la lista:

  ```ts
  tipologiaSala: TipologieSala [] = [];

  ngOnInit():void { //qui ci sarà il richiamo al service scritto in precedenza
   this.tipologiaSalaService.OttieniTutto() }
  ``` 
 - => poi il template di quella lista (@ng for)

  ```html
  <div *ngfor= "let elencoSale" >
   <h3>{{ tipologiaSala.nome}}</h3>
    <p> {{tipologiaSala.maggiorazionePrezzo}} </p>
  </div>
  ```
  => aggiungere il routing (la parte in
  fondo dove c'è l'export)

  Routes rappresenta un dizionario con chiave = percorso e valore = nome del component

  ```ts
  export const routes: Routes = [
    {path: 'tipologiaSala', component: TipologiaSalaListComponent},
    {path: 'tipologiaSala/new', component: TipologiaSalaFormComponent},
    {path: 'tipologiaSala/:id', component: TipologiaSalaDetailComponent}
  ]
  ```
 Quando la lista funziona aggiungiamo il minimo delle cose indispensabili che servono:
 - form di creazione, form di modifica, form di eliminazione, validazione, messaggi di errore.
 Opzionalmente, in una seconda fase, si possono aggiungere ottimizzazioni tipo loading e pagine tipo la conferma
 di una azione.

# Ordine suggerito di esecuzione

1° = lista dei dati.
2° = dettaglio del singolo elemento.
3° = creazione.
4° = modifica.
5° = eliminazione.
6° = validazioni.
7° = implementazioni, miglioramenti UI.

Solo se tutto il CRUD è funzionante aggiungiamo l'autenticazione:
- Login
- Register
- JWT
- AuthService di Angular
- Interceptor
- Guard
- Ruoli


Elenco dei Tasks:

- Creare progetto angular rispettando l'archetipo che abbiamo deciso.
- Creare i modelli.
- Creare i service angular.
- Creare i componenti e le pagine.
- Collegare il routing.
- Fare i CRUD base.
- Gestire errori e validazioni.
- Aggiungere l'autenticazione.
- Rifinire la UI.
- Tutti i test.

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

# sala.model.ts
```ts
export interface SalaCreazione {
    nome: string;
    capienza: number;
    tipologiaSalaId: string;
}

export interface Sala {
    id: string | null;
    nome: string;
    capienza: number;
    tipologiaSalaId: string;
    nomeTipologia: string;
}
```
# proiezione.model.ts
```ts
export interface Proiezione{
    id: string;
    dataProiezione: Date;
    movieId: string;
    salaId: string;
    turnoId: string;
}

export interface ProiezioneCreazione {
    movieId: string;
    salaId: string;
    turnoId: string;
    dataProiezione: Date;
}
```
## SERVICES

## GUARDS

## INTERCEPTORS

## SHARED

## ENVIRONMENT