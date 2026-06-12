# NUOVO CINEMA PARADISO

# LUNEDI: IMPOSTARE LE CONVENZIONI PER LA STESURA DEL CODICE.

## Convenzioni

- Mantenere la lingua italiana.
- Utilizzare il camel case per le variabili e il pascal case per le funzioni.
- I nomi dei branch devono riportare la tipologia del file da modificare e il nome (Entità-Azione).
- Da adesso in poi verranno accettate solo le pull requests con il readme relativo alle modifiche fatte.

## Readme
- Il readme manterrà la stessa impostazione del readme back-end con:
Utente:
Data:
Descrizione:

E in più sarà necessario utilizzare il folding in questo modo, onde evitare di avere problemi nella risoluzione dei
conflitti nel merge.
ESEMPIO:
<DETAILS><SUMMARY>VERSIONE 1.0</SUMMARY> CODICE </DETAILS>

# STABILIRE L'ORDINE DELLE COSE DA FARE!

- Creare la cartella core, contenente guards, interceptors, services.
- Creare la cartella shared e decidere quali componenti sono condivisi fra le pagine. Se ci fossero
  più pagine che usano uno stesso modello, il modello condiviso si mette in shared.
- Creare la cartella features che contiene le entità, a seconda dell'entità:
  - TipologiaSala ( list, detail, form )
  - Ruolo ( list, detail, form )
  - Turno ( list, detail, form )
  - GenereMovie ( list, detail, form )
  - Sala ( list, detail, form )
  - Movie ( list, detail, form )
  - Utente ( list, detail, form )
  - Abbonamento ( list, detail, form )
  - GiftCard ( list, detail, form )
  - Biglietto ( list, detail, form )
  - Proiezione ( list, detail, form )
  - LogAzioni ( list, detail )
  - Gestore ( form )

## Liste

### entita-list.page

Solo le liste presenti in una pagina

### entita-list.component

Tutte le liste che sono presenti in più pagine

- Lista tipologie Sala
  - tipologia sala
  - sala

- Lista Ruoli
  - ruolo utente

- Lista Turni
  - turno
  - proiezione

- Lista Generi Movie
  - genere movie
  - movie

- Lista Sale
  - sala
  - proiezione

- Lista Movies
  - movie
  - proiezione

- Lista utenti
  - Abbonamento
  - Gestore
  - GiftCard

- Lista Abbonamenti
  - abbonamento

- Lista GiftCards
  - giftcard

- Lista Biglietti
  - biglietto

- Lista Proiezioni
  - proiezione
  - biglietto

- Lista Log Azioni
  - gestore

## Detail

### entita-detail.page

- TipologiaSala
- Ruolo
- Turno
- GenereMovie
- Sala
- Movie
- Utente
- Abbonamento
- GiftCard
- Biglietto
- Proiezione
- LogAzioni

### entita-form.page

- TipologiaSala ( impostare in dataseeder ) priorità bassa
- Turno
- GenereMovie ( impostare in dataseeder ) priorità bassa
- Sala
- Movie
- Utente
- Abbonamento
- Biglietto
- Proiezione
- LogAzioni

  Ognuna di queste entità deve essere una cartella dentro features, ed ognuna deve contenere page, component, service e models.
  - Creare app.routes.ts dove verranno definite tutte le rotte.

### operatore

La pagina operatore sarà una dashboard contenente tutte le operazioni(component) delle azioni che il operatore può eseguire.
Ad ogni azione corrisponderà quindi una page(ESEMPIO: cambio di ruolo avrà il suo pulsante nella dashboard che richiamerà pagina di modifica del ruolo al suo interno).

operatore
|---- operatore-dashboard.page.ts
|---- operatore-dashboard.page.html
|---- operatore-cambio-ruolo.component.ts
|---- operatore-cambio-ruolo.component.html
|---- operatore-utenti-lista.component.ts (conterrà la elimina e la modifica)
|---- operatore-utenti-lista.component.html
|---- operatore-utente-detail.component.ts(conterrà la elimina e la modifica)
|---- operatore-utente-detail.component.html
|---- operatore-utenti-abbonati-lista.component.ts
|---- operatore-utenti-abbonati-lista.component.html
|---- operatore-biglietti-lista.component.ts
|---- operatore-biglietti-lista.component.html
|---- operatore-log-audit-lista.component.ts
|---- operatore-log-audit-lista.component.html

## Azioni per ruolo.

### Operatore:

- gestione degli utenti(operazioni crud).
- gestione degli abbonamenti (operazioni crud).
- gestione dei movies (operazioni crud).
- gestione delle sale (operazioni crud).
- gestione delle tipologie di sala (operazioni crud).
- gestione dei turni (operazioni crud).
- gestione delle proiezioni(operazioni crud).

I generi dei movies sono hard-coded, preimpostati nel data seeder.
La giftcard è hard-coded. Avrà un codice di riscatto alla sua creazione che potrà essere usato per aggiungere il valore di quella giftcard al saldo di chi riscatta il codice.

### GESTORE:

- leggere la lista dei biglietti.
- leggere la lista degli audit.
- leggere il saldo del conto del cinema.

### UTENTE

- leggere la lista dei propri biglietti.
- visualizzare tutte le informazioni anagrafiche del suo profilo, saldo compreso.
- creare un codice di riscatto da poter usare per caricare il proprio conto.

# DASHBOARD

## DASHBOARD GUEST

- homepage del sito dove ci saranno:
- pulsanti di registrazione e login.
- pulsante di visualizzazione delle proiezioni.
- pulsante di visualizzazione dei film(solo per operatore)

## DASHBORD DA LOGGATO

- homepage simile a quella guest ma con pulsanti di:
- pulsante di logout.
- visualizzazione del proprio profilo:
  - ogni ruolo avrà la sua pagina profilo diversa in base alle azioni che può fare.
- pulsante di visualizzazione delle proiezioni.

# PAGINE

## Pagina proiezioni

- lista delle proiezioni con pulsanti di:
- acquisto.
- modifica(operatore).
- eliminazione(operatore).

## Pagina movies

- lista dei film con pulsanti di:

- creazione(operatore).
- modifica(operatore).
- eliminazione(operatore).

## Pagina sale:

- lista delle sale con pulsanti di:

- creazione(operatore).
- modifica(operatore).
- eliminazione(operatore).

## Passaggi

- Creare i modelli angular che devono rispecchiare i DTO del backend e radunare richiesta e risposta
  all'interno dello stesso file ts.

( ESEMPIO CON DTO BACKEND E MODELLO ANGULAR IN TYPESCRIPT)

```csharp
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
  - tipologiaSala-list.page.ts (lista)
  - tipologiaSala-detail.component.ts (elemento singolo)
  - tipologiaSala-form.component.ts (pagina coi comandi crud)

  però, i componenti condivisi tipo la navbar, si mette `.component`

```ts
export class TipologiaSalaService {
  // url dove si trova il service
  private apiUrl = 'la rotta dell api backend';
  // costruttore con i metodi OttieniTutto e CreaTipologiaSala
  constructor(private http: HttpClient) {}

  OttieniTutto(): observable<tipologieSala[]> {
    return this.http.get<tipologieSala[]>(this.apiUrl);
  }

  CreaTipologiaSala(tipologiaSala: CreaTipologiaSala): observable<TipologiaSala> {
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
<div *ngfor="let elencoSale">
  <h3>{{ tipologiaSala.nome}}</h3>
  <p>{{tipologiaSala.maggiorazionePrezzo}}</p>
</div>
```

=> aggiungere il routing (la parte in
fondo dove c'è l'export)

Routes rappresenta un dizionario con chiave = percorso e valore = nome del component

```ts
export const routes: Routes = [
  { path: 'tipologiaSala', component: TipologiaSalaListComponent },
  { path: 'tipologiaSala/new', component: TipologiaSalaFormComponent },
  { path: 'tipologiaSala/:id', component: TipologiaSalaDetailComponent },
];
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

\*.spec.ts

Con questo flag, il progetto più pulito e hai meno file iniziali.

## IMPORTANTE

Una volta effettuato il pull dal developer, ad inizio progetto, essendo la cartella node_modules inserita nel gitignore, bisognerà digitare il seguente comando

```bash
npm install
```

per avere la cartella in locale in modo tale che il progetto possa essere, successivamente, avviato.

# app.routes
<details>
<summary>app.routes</summary>

- Si può limitare una rotta ad un operatore usando la routeguard(diverso dal *@ng if che indica che un blocco
  di codice venga saltato in una determinata condizione).
- Per un autorizzazione semplice si usa CanActivate() oppure CanMatch() se vogliamo impostare il caricamento
  condizionale della route (nella rubrica di esempio, abbiamo usato CanActivate() perchè l'admin aveva solo
  una rotta da proteggere che era il change-role).

In questo caso dobbiamo lavorare su AuthService, volendo possiamo comunque prendere il ruolo dal local_storage
dove il ruolo viene autorizzato dal backend al login.


```typescript
ottieniRuoloUtente(): RuoliUtente | null{
    const ruolo = localStorage.getItem('ruolo');
    if(ruolo === 'Operatore' || ruolo === 'Gestore' || ruolo === 'Utente')
    {
      return ruolo;
    }
    return null;
  }  
```

Abbiamo bisogno comunque di un tipo di dato RuoliUtente che è una costante, definita in un nuovo file
ruoliUtente.ts con l'export type.

```typescript
export type RuoliUtente = 'Gestore' | 'Operatore' | 'Utente';
```
Dopo aver creato il file lo importiamo su auth.service.ts in modo che nel return di ottieniRuoloUtente venga condizionato dall'utente attivo.

```typescript
import { RuoliUtente } from '../ruoliUtente';
```
Dopo di che nel service possiamo usare la costante RuoliUtente nel metodo ottieniRuoloUtente e nel metodo:

```typescript
possiedeQualsiasiRuolo(ruoli: RuoliUtente[]): boolean {
    const ruolo = this.ottieniRuoloUtente();
    return !!ruolo && ruoli.includes(ruolo);
  }
```

Quindi nel role.guard.ts avremo l'import di RuoliUtente e definiremo la costante dei ruoli autorizzati.

```typescript
export const roleGuard: CanActivateFn = (route: ActivatedRouteSnapshot) => {
  const authService = inject(AuthService);
  const router = inject(Router);
  const ruoliAutorizzati = (route.data['role'] as RuoliUtente[] | undefined) ?? [];

  if (ruoliAutorizzati.length === 0 || authService.possiedeQualsiasiRuolo(ruoliAutorizzati)) {
      return true;
  }
  return router.createUrlTree(['/nonAutorizzato']);

};
```

Dobbiamo creare la pagina nonAutorizzato.


Adesso possiamo usare i ruoli autorizzati nelle routes(app.routes.ts).
In app.routes.ts facciamo un nuovo path al di sotto del quale tutte le pagine sono protette.
Quindi il codice da così:
```typescript

   {
    path: 'operatore/change-role',
    canActivate: [authGuard, roleGuard],
    data: { roles: ['gestore'] },
    loadComponent: () => import('./pages/gestore-change-role/gestore-change-role.page').then((m) => m.GestoreChangeRolePage)
    },
```
diventa così:

```typescript

   {
    path: 'operatore',
    component:OperatoreComponent,
    canActivate: [roleGuard],
    data: { roles: ['Operatore'] },
    },
```
OperatoreComponent è un esempio, noi avremo nella route il componente reale dentro la cartella page.Così:

```typescript

   {
    path: 'operatore',
    component:OperatorePage,
    canActivate: [roleGuard],
    data: { roles: ['Operatore'] },
    },
```

Il path operatore proteggerà altre rotte nel nostro caso e quindi dovremo creare un area operatore che avrà al suo interno
dei children contenenti le rotte protette.
Esempio
```typescript

  
  {
    path: 'operatore',
    canActivate: [authGuard, roleGuard],
    data: {
      roles: ['Operatore']
    },

    children: [

      {
        path: 'dashboard',
        loadComponent: () =>
          import('./pages/operatore-dashboard/operatore-dashboard.page')
            .then(m => m.DashboardPage)
      },

      {
        path: 'movie',
        loadComponent: () =>
          import('./pages/movie/movie.page')
            .then(m => m.OperatoreMoviePage)
      },

      {
        path: 'profilo',
        loadComponent: () =>
          import('./pages/profilo/profilo.page')
            .then(m => m.OperatoreProfiloPage)
      }

    ]
  }
```

Angular usa i routeGuard per controllare l'accesso alla route prima dell'attivazione della pagina e CanActivate controlla
se una route può essere aperta e CanMatch è utile per le rotte lazyloaded(già caricate in memoria).
Dato che abbiamo deciso di usare Angular in modalità standalone, possiamo far diventare la sezione il contenitore delle
pagine riferite ad un ruolo specifico ed app.component.ts diventa il contenitore principale.

In questo modo la logica dei ruoli rimane nei guard.

IN PRATICA NON DOBBIAMO CREARE RUOLOCOMPONENT, DOBBIAMO SOLO PROTEGGERE LE PAGINE CHE ABBIAMO GIà IN PAGES USANDO:

```typescript
canActivate:[roleGuard],
data: {
  roles: ['Ruolo']
  },
```

## Creazione pagina nonAutorizzato

Non è obbligatoria, nel senso che potremmo rimandare al login:

```typescript
return router.createUrlTree(['/login']);
```
E quindi il guard rimane così come è ora, indirizzando verso il login.

Opzione 2: redirect nella dashboard.
```typescript
return router.createUrlTree(['/dashboard']);
```
La più sensata se l'utente è loggato ma non ha i permessi.

Opzione 3: pagina nonAutorizzato.

La più informativa per l'utente perchè possiamo mostrare un messaggio tipo "Non hai i permessi per visualizzare questa
pagina".

```typescript
path: 'nonAutorizzato',
loadComponent:() =>
import('../pages/non-autorizzato/non-autorizzato.page').then(m => m.NonAutorizzatoPage ) 
data: {
  roles: ['Ruolo']
  },
```
Ricordarsi di mettere la rotta in app.routes.ts.
Nel role-guard.ts possiamo usare /'nonAutorizzato' quando l'utente è loggato ma non ha il ruolo.

Questo è il modo più versatile perchè permette di avere un comportamento finale:

- Utente non loggato => reindirizzato al login.
- Utente loggato ma ruolo sbagliato => reindirizzato alla pagina nonAutorizzato.
- Utente loggato correto => entra nella pagina.

</details>
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

## GUARDS

## INTERCEPTORS

## ENVIRONMENT

## FEATURES

### Dashboard
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

### abbonamento (Fatto)
### page
- abbonamento.page.ts
### components
- abbonamento-list.component.ts [tutti] - Andrea Paris (fatto)
- abbonamento-detail.component.ts [utente](abbonato) Francesco
- abbonamento-form.component.ts [operatore] Andrea Paris (fatto)

### auth (Fatto)
### page
### components
- login.component.ts
- register.component.ts
- register.component.html

### biglietto (Fatto)
### components
- biglietto-list.component.ts [utente,operatore,gestore] Andrea Bruno (fatto) 
- biglietto-form.component.ts [operatore] (Marco)

### cambio-ruolo (Fatto)
### components
- cambio-ruolo-form.component.ts [gestore] (Lorenzo)
- cambio-ruolo-form.component.html

- utenti-list.component.ts [gestore] Andrea Bruno (fatto)
- utenti-list.component.html 

### conto-cinema (Non fatto)
### components
- conto-cinema-detail.component.ts [gestore]

### dashboard (Fatto)
[priorità]
### layout  
Francesco (fatto)

- dashboard.layout.ts (Francesco: inserire nel layout di dashboard il component di profilo, biglietti, giftcard, abbonamento (e se non abbonato devono apparire gli abbonamenti disponibili))

(PER IL GESTORE: deve avere SOLO log, vedi giftcard, implementare cambio ruolo al posto dell'operatore)

### genere-movie (Fatto)
### components

- genere-movie-list.component.ts [tutti] Greg (fatto)
- genere-movie-list.component.html

### giftcard (Fatto)
### components
- riscatta-codice.component.ts (Simeone)

- crea-codice.component.ts (Simeone) Fatto!
- crea-codice.component.html (Simeone) Fatto!

- giftcard-form.component.ts (Simeone) Fatto!
- giftcard-form.component.html (Simeone) Fatto!

- giftcard-list.component.ts [gestore,utente] Simeone (Fatto!)
- giftcard-list.component.html [gestore,utente] Simeone (Fatto!)

### log (Fatto)
### components

- log-list.component.ts [gestore] Simeone (fatto!)
- log-list.component.html [gestore] Simeone (Fatto!)

### movie (Fatto)
### components

- movie-list.component.ts [operatore] Francesco (fatto)
- movie-list.component.html
- movie-form.component.ts
- movie-form.component.html

### profilo (Fatto)

### components
- profilo.component.ts [utente,operatore,gestore] 

### proiezione (Fatto)

### page
### components
- proiezione-list.component.ts [tutti] Fabio (fatto)
- proiezione-list.component.html 

- proiezione-form.component.ts [operatore]
- proiezione-form.component.html

- proiezione-detail.component.ts [tutti]
- proiezione-detail.component.html

### sala (Fatto)

### components
- sala-list.component.ts [operatore] Francesco (fatto)
- sala-list.component.html

- sala-form.component.ts [operatore] Francesco
- sala-form.component.html

### tipologia-sala (Fatto)
### components

- tipologia-sala-list.component.ts [operatore] Greg
- tipologia-sala-list.component.html [operatore] Greg

- tipologia-sala-form.component.ts [operatore] Greg
- tipologia-sala-form.component.html [operatore] Greg

### turno (Fatto)
### components

- turno-list.component.ts [operatore] Andrea paris (fatto)
- turno-form.component.ts [operatore] Andrea paris (fatto)


### shared/navbar (Fatto)
[priorità] (Fabio: modificare aggiungendo sale per l'operatore, rimuovendo profilo per tutti, crediti) (fatto)


# 29/05/2026
## Obiettivo: refactoring
Organizzare al meglio la struttura del frontend in modo che ci sia meno codice duplicato possibile e che ogni modulo possa essere facilmente riutilizzabile in altre webapi, come se fosse una libreria.  

Ogni area dev'essere divisa in componenti ma non per forza ogni ruolo deve avere una cartella separata, la regola migliore e che permette meno duplicazione di codice è la divisione per features, quindi:
- features -> funzionalità
- pages -> schermate (per l'utente)
- components -> pezzi riutilizzabili che andiamo a richiamare nelle pages (blocchetti per lo sviluppatore)
- roles -> sono i permessi dei ruoli sulle routes

La page, in quanto collettore di più components, avrà in:
```ts
@Component({
  imports:[RouterOutlet, RouterLink]
  template:`<nav>
              <a routerLink="proiezioni">Proiezioni</a>
            </nav>`
            <router-outlet />
})
```

un esempio di struttura delle folder nell'applicazione potrebbe essere:
features/
  abbonamenti/
    pages/
      abbonamenti.page.ts
    components/
      abbonamenti-list.component.ts

E nelle route decidi:
```ts
data:{
  roles: ['Operatore', 'Gestore']
}
```

Usiamo la convenzione pages quando un component è collegato ad una route

e potrebbe essere per esempio:
- area-operatore.ts 
- area-gestore.ts


invece, usiamo components per pezzetti riutilizzabili all'interno della pagina area operatore, esempio:
- operatore-card.ts
- operatore-form.ts
- operatore-table.ts
- operatore-list.ts

La settimana prossima implementiamo anche dei contenitori di funzioni che noi percepiamo utili in un contesto di ruolo ma non saranno cose che hanno un corrispettivo tra i file dell'applicazione, sarà semplicemente una vera e propria route (es: /Operatore) che collezionerà le varie features relative ad un ruolo specifico, però per farlo dobbiamo implementare nella core/guard/ le funzionalità riguardanti il CanActivateChild.