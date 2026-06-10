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



## PAGES

### utente.page.ts V1.0 

<details>
<summary>Versione 1.0</summary>
Andrea Bruno 28-05-2026
Creazione della pagina dell'area riservata dell'utente

```c#
import { Component, inject, signal, OnInit } from '@angular/core';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { HttpErrorResponse } from '@angular/common/http';
import { DatePipe } from '@angular/common';
import { UtenteService } from '../../services/utente.service';
import { AuthService } from '../../services/auth.service';
import { Utente } from '../../models/utente.model';

@Component({
  selector: 'utente-page',
  standalone: true, // Componente standalone (senza modulo)
  imports: [
    ReactiveFormsModule, // Necessario per i form reattivi
    DatePipe             // Per formattare date nel template
  ],
  templateUrl: './utente.page.html',
})
export class UtentePage implements OnInit {

  // Iniezione dei servizi tramite inject() (stile Angular moderno)
  private readonly formBuilder = inject(FormBuilder);
  private readonly utenteService = inject(UtenteService);
  private readonly authService = inject(AuthService);

  // Signals: stato reattivo del componente
  readonly utente = signal<Utente | null>(null); // Dati dell’utente
  readonly staCaricando = signal(false);         // Stato caricamento profilo
  readonly staInviando = signal(false);          // Stato invio form
  readonly messaggioErrore = signal('');         // Messaggi di errore
  readonly messaggioSuccesso = signal('');       // Messaggi di successo

  // Form reattivo con validazioni
  readonly form = this.formBuilder.nonNullable.group({
    nomeCompleto: ['', [Validators.required, Validators.maxLength(100)]],
    eta: [0, [Validators.required, Validators.min(14), Validators.max(100)]]
  });

  // Angular chiama questo metodo quando il componente viene inizializzato
  ngOnInit(): void {
    this.caricaUtente(); // Carica i dati del profilo all’avvio
  }
  
  // Controlla se l’utente ha un ruolo che permette modifiche limitate
  modificabileDa(): boolean {
    return this.authService.possiedeQualsiasiRuolo(['Operatore']);
  }

  // Recupera i dati del profilo dal backend
  caricaUtente(): void {
    this.staCaricando.set(true);
    this.messaggioErrore.set('');

    this.utenteService.profilo().subscribe({
      next: (item) => {
        this.utente.set(item); // Salva i dati nel signal

        // Precompila il form con i dati ricevuti
        this.form.patchValue({
          nomeCompleto: item.nomeCompleto,
          eta: item.eta
        });

        this.staCaricando.set(false);
      },
      error: (error) => {
        this.staCaricando.set(false);
        this.messaggioErrore.set(
          this.estraiMessaggioErrore(error, 'Accedi per vedere il profilo')
        );
      }
    });
  }

  // Invia al backend le modifiche del profilo
  invia(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched(); // Mostra errori di validazione
      return;
    }

    this.staInviando.set(true);
    this.messaggioErrore.set('');
    this.messaggioSuccesso.set('');

    this.utenteService.modifica(this.form.getRawValue()).subscribe({
      next: () => {
        this.staInviando.set(false);
        this.messaggioSuccesso.set('Profilo aggiornato');

        // Ricarica i dati aggiornati
        this.caricaUtente();
      },
      error: (error) => {
        this.staInviando.set(false);
        this.messaggioErrore.set(
          this.estraiMessaggioErrore(error, 'Modifica non riuscita.')
        );
      }
    });
  }

  // Elimina definitivamente l’account dell’utente
  elimina(): void {
    const confirmed = confirm('Vuoi davvero eliminare il tuo account?');
    if (!confirmed) return;

    this.messaggioErrore.set('');
    this.messaggioSuccesso.set('');

    this.utenteService.eliminaProfilo().subscribe({
      next: () => {
        this.authService.logout(); // Rimuove token e sessione
        this.messaggioSuccesso.set('Account eliminato');

        // Redirect manuale alla pagina di login
        window.location.href = '/login';
      },
      error: (error) => {
        this.messaggioErrore.set(
          this.estraiMessaggioErrore(error, 'Eliminazione non riuscita.')
        );
      }
    });
  }

  // Estrae un messaggio leggibile dagli errori HTTP
  private estraiMessaggioErrore(error: unknown, fallback: string): string {
    if (error instanceof HttpErrorResponse) {
      return error.error?.message ?? fallback;
    }
    return fallback;
  }
}
```

### utente.page.html

<details>
<summary>Versione 1.0</summary>
Andrea Bruno 28-05-2026
Creazione pagina html dell'area riservata dell'utente
Se l'utente non è abbonato non viene visualizzata la card e 
ovviamento vale anche per operatore e gestore  

```c#
<!--
  Commento: Questo blocco è il template HTML per la lista delle tipologie di sala.
  Usa una sintassi simile a quella dei template (condizioni con @if, ciclo @for).
  Le funzioni invocate: errorMessage(), successMessage(), isLoading(), tipologiaSala().
  Ogni elemento 'item' ha campi: id, nome, maggiorazionePrezzo.
-->

<section>

    <!-- HEADER: mostrato solo se l'utente è stato caricato -->
    @if (utente()) {
        <!-- Titolo principale con il nome dell'utente -->
        <h1 class="page-title">Ciao, {{ utente()?.nomeCompleto }}</h1>

        <!-- Sottotitolo descrittivo -->
        <p class="page-subtitle">Gestisci le informazioni del tuo account.</p>
    }

    <!-- MESSAGGI DI ERRORE E SUCCESSO -->
    @if (messaggioErrore()) {
        <div class="alert alert-warning">{{ messaggioErrore() }}</div>
    }

    @if (messaggioSuccesso()) {
        <div class="alert alert-success">{{ messaggioSuccesso() }}</div>
    }

    <!-- STATO DI CARICAMENTO -->
    @if (staCaricando()) {
        <p class="muted">Caricamento profilo...</p>

    <!-- NESSUN UTENTE TROVATO -->
    } @else if (!utente()) {
        <p class="muted">Nessun profilo trovato.</p>

    <!-- CONTENUTO PRINCIPALE -->
    } @else {

    <!-- GRID CON DUE CARD AFFIANCATE -->
    <div class="grid grid-2" style="margin-top: 1.5rem;">

        <!-- CARD SINISTRA: INFORMAZIONI UTENTE -->
        <article class="card">
            <h2>Informazioni Utente</h2>

            <div class="list">

                <!-- Email -->
                <div class="list-item">
                    <strong>Email</strong>
                    <p>{{ utente()?.email }}</p>
                </div>

                <!-- Età -->
                <div class="list-item">
                    <strong>Età</strong>
                    <p>{{ utente()?.eta }}</p>
                </div>

                <!-- Saldo: mostrato solo ai clienti (non operatori) -->
                @if (!modificabileDa()) {
                    <div class="list-item">
                        <strong>Saldo</strong>
                        <p>{{ utente()?.saldo }} €</p>
                    </div>
                }
            </div>
        </article>

        <!-- CARD DESTRA: MODIFICA + ELIMINAZIONE -->
        <article class="card">
            <h2>Modifica Profilo</h2>

            <!-- Form reattivo Angular -->
            <form class="form-grid" [formGroup]="form" (ngSubmit)="invia()">

                <!-- Campi del form -->
                <div>
                    <label for="nomeCompleto">Nome completo</label>
                    <input id="nomeCompleto" type="text" formControlName="nomeCompleto">

                    <label for="eta">Età</label>
                    <input id="eta" type="number" formControlName="eta">
                </div>

                <!-- Pulsanti: aggiorna + elimina -->
                <div class="btn-row">

                    <!-- Pulsante AGGIORNA -->
                    <button class="btn btn-primary" type="submit" [disabled]="staInviando()">
                        {{ staInviando() ? 'Salvataggio...' : 'Aggiorna' }}
                    </button>

                    <!-- Pulsante ELIMINA ACCOUNT (rosso pieno) -->
                    <button class="btn btn-danger-solid" type="button" (click)="elimina()">
                        Elimina account
                    </button>
                </div>

            </form>
        </article>

    </div>

    <!-- CARD ABBONAMENTO (solo per clienti e solo se abbonati) -->
    @if (!modificabileDa()) {
        @if (utente()?.seAbbonato) {

            <article class="card" style="margin-top: 2rem;">
                <h2>Abbonamento</h2>

                <!-- Riga orizzontale con i dati dell'abbonamento -->
                <div class="abbonamento-row">

                    <!-- Stato -->
                    <div>
                        <strong>Stato:</strong>
                        {{ utente()?.seAbbonato ? 'Attivo' : 'Non attivo' }}
                    </div>

                    <!-- Data inizio abbonamento -->
                    <div>
                        <strong>Inizio:</strong>
                        {{ utente()?.dataInizioAbbonamento | date }}
                    </div>

                    <!-- Tipo abbonamento -->
                    <div>
                        <strong>Tipo:</strong>
                        {{ utente()?.tipoAbbonamento }}
                    </div>

                </div>
            </article>

        }
    }

    } <!-- Fine else principale -->

</section>
```

### register.page.ts 

<details>
<summary>Versione 1.0</summary>
Utente: Andrea Paris
Data: 25/06/2026

```ts
import { Component, inject, signal } from '@angular/core';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { HttpErrorResponse } from '@angular/common/http';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'register-page',
  standalone: true,
  imports: [ReactiveFormsModule, RouterLink],
  templateUrl: './register.page.html'
})
export class RegisterPage {

  private readonly fb = inject(FormBuilder);
  private readonly authService = inject(AuthService);
  private readonly router = inject(Router);

  readonly isLoading = signal(false);
  readonly successMessage = signal('');
  readonly errorMessage = signal('');

  readonly form = this.fb.group({
    email: this.fb.nonNullable.control('', [
      Validators.required,
      Validators.email
    ]),

    password: this.fb.nonNullable.control('', [
      Validators.required,
      Validators.minLength(6)
    ]),

    nomeCompleto: this.fb.nonNullable.control('', [
      Validators.required,
      Validators.maxLength(100)
    ]),

    eta: this.fb.nonNullable.control(0, [
      Validators.required,
      Validators.min(1)
    ])
  });

  submit(): void {

    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.isLoading.set(true);
    this.successMessage.set('');
    this.errorMessage.set('');

    const payload = this.form.getRawValue();

    this.authService.registrazione(payload).subscribe({

      next: (response) => {
        this.isLoading.set(false);
        this.successMessage.set(response.message);

        setTimeout(() => {
          void this.router.navigate(['/login']);
        }, 900);
      },

      error: (error: unknown) => {
        this.isLoading.set(false);
        this.errorMessage.set(this.exctractErrorMessage(error));
      }
    });
  }

  private exctractErrorMessage(error: unknown): string {

    if (error instanceof HttpErrorResponse) {

      if (Array.isArray(error.error) && error.error.length > 0) {
        return error.error
          .map((item: { description?: string }) =>
            item.description ?? 'Errore')
          .join(' | ');
      }

      return error.error?.message ?? 'Registrazione non riuscita';
    }

    return 'Registrazione non riuscita';
  }
}
```
</details>

### register.page.html 

<details>
<summary>Versione 1.0</summary>
Utente: Andrea Paris
Data: 25/06/2026

```html
<section>
    <h1 class="page-title">Registrazione</h1>

    <p class="page-subtitle">
        Il backend assegna automaticamente il ruolo
        <strong>User</strong>
        ai nuovi account
    </p>

    <div class="card">

        @if(successMessage()){
        <div class="alert alert-success">
            {{ successMessage() }}
        </div>
        }

        @if(errorMessage()){
        <div class="alert alert-warning">
            {{ errorMessage() }}
        </div>
        }

        <form class="form-grid" [formGroup]="form" (ngSubmit)="submit()">

            <div class="grid grid-2">

                <div>
                    <label for="nomeCompleto">
                        Nome completo
                    </label>

                    <input id="nomeCompleto" type="text" formControlName="nomeCompleto" />
                </div>

                <div>
                    <label for="eta">
                        Età
                    </label>

                    <input id="eta" type="number" formControlName="eta" />
                </div>

            </div>

            <div class="grid grid-2">

                <div>
                    <label for="email">
                        Email
                    </label>

                    <input id="email" type="email" formControlName="email" />
                </div>

                <div>
                    <label for="password">
                        Password
                    </label>

                    <input id="password" type="password" formControlName="password" />
                </div>

            </div>

            <div class="btn-row">

                <button class="btn btn-primary" type="submit" [disabled]="isLoading()">

                    {{ isLoading() ? "Registrazione..." : "Registrati" }}

                </button>

                <a class="btn btn-secondary" routerLink="/login">

                    Torna al login

                </a>

            </div>

        </form>

    </div>
</section>
```
</details>

### operatore.page.ts
<details>
<summary> v.1.0 </summary>

Utente: Andrea Paris
Data: 28/05/2026
Descrizone: Creazione Pagina Operatore con solo il cambio ruolo (per ora)

```ts
import { Component, inject, signal } from '@angular/core';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { HttpErrorResponse } from '@angular/common/http';
import { GestioneUtenteService } from '../../services/gestione-utente.service';

@Component({
  selector: 'operatore-page',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './operatore.page.html'
})
export class OperatorePage {

  private readonly fb = inject(FormBuilder);
  private readonly gestioneUtenteService = inject(GestioneUtenteService);

  readonly isSubmitting = signal(false);
  readonly successMessage = signal('');
  readonly errorMessage = signal('');

  readonly roles = ['Gestore', 'Operatore', 'Utente'];

  readonly cambiaRuoloForm = this.fb.nonNullable.group({
    email: ['', [Validators.required, Validators.email]],
    nuovoRuolo: ['Utente', [Validators.required]]
  });

  cambiaRuolo(): void {

    if (this.cambiaRuoloForm.invalid) {
      this.cambiaRuoloForm.markAllAsTouched();
      return;
    }

    this.isSubmitting.set(true);
    this.successMessage.set('');
    this.errorMessage.set('');

    this.gestioneUtenteService
      .modificaRuolo(this.cambiaRuoloForm.getRawValue())
      .subscribe({

        next: (response) => {

          this.isSubmitting.set(false);

          this.successMessage.set(
            `${response.messaggio} Nuovo ruolo: ${response.ruolo}`
          );
        },

        error: (error: unknown) => {

          this.isSubmitting.set(false);

          this.errorMessage.set(
            this.estraiMessaggioErrore(error)
          );
        }
      });
  }

  private estraiMessaggioErrore(error: unknown): string {

    if (error instanceof HttpErrorResponse) {

      return error.error?.messaggio
        ?? 'Cambio ruolo non riuscito';
    }

    return 'Cambio ruolo non riuscito';
  }

}
```
</details>

### operatore.page.html 

<details>
<summary>v.1.0</summary>

Utente: Andrea Paris
Data: 28/05/2026
Descrizone: Creazione Pagina Operatore con solo il cambio ruolo (per ora)

```ts
<section>

    <h1 class="page-title">
        Cambio ruolo utente
    </h1>

    <p class="page-subtitle">
        Pagina visibile solo al ruolo
        <strong>Operatore</strong>
    </p>

    <div class="grid grid-2">

        <!-- CARD FORM -->

        <article class="card">

            @if (successMessage()) {

                <div class="alert alert-success">
                    {{ successMessage() }}
                </div>
            }

            @if (errorMessage()) {

                <div class="alert alert-warning">
                    {{ errorMessage() }}
                </div>
            }

            <form
                class="form-grid"
                [formGroup]="cambiaRuoloForm"
                (ngSubmit)="cambiaRuolo()"
            >

                <!-- EMAIL -->

                <div>

                    <label for="email">
                        Email utente
                    </label>

                    <input
                        id="email"
                        type="email"
                        formControlName="email"
                    />

                    @if (cambiaRuoloForm.controls.email.touched && cambiaRuoloForm.controls.email.invalid) {

                        <div class="field-error">

                            @if (cambiaRuoloForm.controls.email.errors?.['required']) {

                                <small>
                                    L'email è obbligatoria
                                </small>
                            }

                            @if (cambiaRuoloForm.controls.email.errors?.['email']) {

                                <small>
                                    Inserisci un'email valida
                                </small>
                            }

                        </div>
                    }

                </div>

                <!-- RUOLO -->

                <div>

                    <label for="nuovoRuolo">
                        Nuovo ruolo
                    </label>

                    <select
                        id="nuovoRuolo"
                        formControlName="nuovoRuolo"
                    >

                        @for (role of roles; track role) {

                            <option [value]="role">
                                {{ role }}
                            </option>
                        }

                    </select>

                </div>

                <!-- BOTTONE -->

                <div class="btn-row">

                    <button class="btn btn-primary" type="submit"[disabled]="isSubmitting()">

                        {{ isSubmitting() ? 'Aggiornamento...' : 'Aggiorna ruolo' }}

                    </button>

                </div>

            </form>

        </article>

        <!-- CARD INFO -->

        <article class="card">

            <h2>
                Ruoli disponibili
            </h2>

            <div class="list">

                @for (role of roles; track role) {

                    <div class="list-item">

                        <span>
                            Ruolo
                        </span>

                        <strong>
                            {{ role }}
                        </strong>

                    </div>
                }

            </div>

        </article>

    </div>

</section>
```
</details>


### abbonamento.page.ts
<details>

<summary>versione1.0</summary>

Francesco Lorenzi 26/05/2026
```ts
import { Component, inject, signal } from '@angular/core';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { HttpErrorResponse } from '@angular/common/http';
//servizi
import { AuthService } from '../../services/auth.service';
import { AbbonamentoService } from '../../services/abbonamento.service';
//modelli
import { Abbonamento } from '../../models/abbonamento.model';


@Component({
  selector: 'abbonamento-page',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './abbonamento.page.html'
})

export class AbbonamentoPage {
  //servizi
  private readonly formBuilder = inject(FormBuilder);
  private readonly authService = inject(AuthService);
  private readonly abbonamentoService = inject(AbbonamentoService);
  //modelli
  readonly abbonamenti = signal<Abbonamento[]>([]);

  readonly staCaricando = signal(false);
  readonly staInviando = signal(false);
  readonly messaggioErrore = signal('');
  readonly messaggioSuccesso = signal('');
  readonly modificaId = signal<string>('');

  //form
  readonly form = this.formBuilder.nonNullable.group({
    nome: ['', [Validators.required, Validators.maxLength(100)]],
    durata: [1, [Validators.required, Validators.min(1)]],
    prezzo: [1, [Validators.required, Validators.min(1), Validators.max(999999999)]],
    sconto: [0, [Validators.required, Validators.min(0), Validators.max(100)]]
  });
  //costruttore, carica la lista degli abbonamenti che posso essere modificati o eliminati
  constructor() {
    this.caricaAbbonamenti();
  }
  modificabileDa(): boolean {
    return this.authService.possiedeQualsiasiRuolo(['Operatore']);
  }
  //carica la lista degli abbonamenti che posso essere modificati o eliminati
  caricaAbbonamenti(): void {

    this.staCaricando.set(true);
    this.messaggioErrore.set('');
    this.abbonamentoService.ottieniTutto().subscribe({

      next: (items) => {
        this.abbonamenti.set(items);
        this.staCaricando.set(false);

      },
      error: (error: unknown) => {
        this.staCaricando.set(false);
        this.messaggioErrore.set(this.estraiMessaggioErrore(error, 'abbonamenti non trovati'));
      }
    });
  }
  //funzione dei bottoni, modifica o crea
  invia(): void {

    if (this.form.invalid || !this.modificabileDa()) {
      this.form.markAllAsTouched();
      return;
    }

    this.staInviando.set(true);
    this.messaggioErrore.set('');
    this.messaggioSuccesso.set('');


    const request$ = this.modificaId()

      ? this.abbonamentoService.modifica(this.modificaId(), this.form.getRawValue())
      : this.abbonamentoService.crea(this.form.getRawValue());


    request$.subscribe({
      next: () => {
        this.staInviando.set(false);
        this.messaggioSuccesso.set(this.modificaId() ? 'Abbonamento aggiornato.' : 'Abbonamento creato.');
        this.ripristinaForm();
        this.caricaAbbonamenti();
      },
      error: (error: unknown) => {
        this.staInviando.set(false);
        this.messaggioErrore.set(this.estraiMessaggioErrore(error, 'Operazione non riuscita.'));
      }
    });
  }
  //funzione di modifica
  inizioModifica(item: Abbonamento): void {

    if (!this.modificabileDa()) {
      return;
    }
    this.modificaId.set(item.id);

    this.form.patchValue({ nome: item.nome, durata: item.durata, prezzo: item.prezzo, sconto: item.sconto });
    this.messaggioErrore.set('');
    this.messaggioSuccesso.set('');
  }
  //funzione di elimiazione
  elimina(item: Abbonamento): void {
    if (!this.modificabileDa()) {
      return;
    }
    const confirmed = confirm(`Eliminare l'abbonamento \" ${item.nome}\"?`);
    if (!confirmed) {
      return;
    }

    this.messaggioErrore.set('');
    this.messaggioSuccesso.set('');

    this.abbonamentoService.elimina(item.id).subscribe({
      next: () => {
        if (this.modificaId() === item.id) {
          this.ripristinaForm();
        }
        this.caricaAbbonamenti();
      },
      error: (error: unknown) => {

        this.messaggioErrore.set(this.estraiMessaggioErrore(error, 'eliminazione non riuscita.'));
      }
    });
  }
  //ripristino del form
  ripristinaForm(): void {
    this.modificaId.set('');
    this.form.reset({ nome: '', durata: 1, prezzo: 0.01, sconto: 0 });
  }
  //tracci gli abbonamenti er id
  tracciaPerId(_: string, item: Abbonamento): string {
    return item.id;
  }

  private estraiMessaggioErrore(error: unknown, fallback: string): string {

    if (error instanceof HttpErrorResponse) {
      return error.error?.message ?? fallback;
    }
    return fallback;
  }
}
```
</details>

### abbonamento.page.html
<details>
<summary>versione1.0</summary>

Francesco Lorenzi 26/05/2026

```html
<section>
    <h1 class="page-title">Abbonamenti</h1>
    <p class="page-subtitle">Gestisci gli abbonamenti disponibili nel cinema.</p>

    @if (messaggioErrore()) {
    <div class="alert alert-warning">{{ messaggioErrore() }}</div>
    }

    @if (messaggioSuccesso()) {
    <div class="alert alert-success">{{ messaggioSuccesso() }}</div>
    }

    <div class="grid grid-2">
        <article class="card">
            <h2>Lista Abbonamenti</h2>

            @if (staCaricando()) {
            <p class="muted">Caricamento in corso...</p>
            } @else if (abbonamenti().length === 0) {
            <p class="muted">Nessun abbonamento presente.</p>
            } @else {
            <div class="list">
                @for (item of abbonamenti(); track tracciaPerId($index.toString(), item)) {
                <div class="list-item">
                    <div>
                        <strong>{{ item.nome }}</strong>
                        <p>{{ item.durata }} mesi</p>
                        <p>Prezzo: {{ item.prezzo }} €</p>
                        <p>Sconto per biglietto: {{ item.sconto }} €</p>
                        <div class="muted">ID: {{ item.id }}</div>
                    </div>

                    @if (modificabileDa()) {
                    <div class="btn-row">
                        <button class="btn btn-secondary" type="button" (click)="inizioModifica(item)">Modifica</button>
                        <button class="btn btn-danger" type="button" (click)="elimina(item)">Elimina</button>
                    </div>
                    }
                </div>
                }
            </div>
            }
        </article>
        <article class="card">
            <h2>{{ modificaId() ? 'Modifica abbonamento' : 'Aggiungi abbonamento' }}</h2>

            @if (!modificabileDa()) {
            <div>
                Il tuo ruolo non ti permette di modificare o eliminare abbonamenti.
            </div>
            }
            <form class="form-grid" [formGroup]="form" (ngSubmit)="invia()">
                <div>
                    <label for="nome">Nome</label>
                    <input id="nome" type="text" formControlName="nome" [disabled]="!modificabileDa()">
                    <label for="durata">Durata (mesi)</label>
                    <input id="durata" type="number" formControlName="durata" [disabled]="!modificabileDa()">
                    <label for="prezzo">Prezzo (€)</label>
                    <input id="prezzo" type="number" formControlName="prezzo" [disabled]="!modificabileDa()" step="1">
                    <label for="sconto">Sconto per biglietto (€)</label>
                    <input id="sconto" type="number" formControlName="sconto" [disabled]="!modificabileDa()" step="0">
                </div>

                <div class="btn-row">
                    <button class="btn btn-primary" type="submit" [disabled]="staInviando() || !modificabileDa()">
                        {{ staInviando() ? 'Salvataggio...' : (modificaId() ? 'Aggiorna' : 'Crea') }}
                    </button>
                    @if (modificaId()) {
                    <button class="btn btn-secondary" type="button" (click)="ripristinaForm()">Annulla</button>
                    }
                </div>
            </form>
        </article>
    </div>
</section>

```
</details>

### genere-movie.page.html

<details><summary> genere-movie.page V1.0</summary>

```html
<section>
  <h1 class="page-title">Generi di Film</h1>
  <p class="page-subtitle">il backend restituisce la lista generi di film</p>

  @if(errorMessage()){
  <div class="alert alert-warning">{{ errorMessage()}}</div>
  } @if(successMessage()){
  <div class="alert alert-success">{{ successMessage() }}</div>
  }

  <div class="grid grid-2">
    <article class="card">
      <h2>Lista generi di film</h2>

      @if(isLoading()){
      <p class="muted">Caricamento in corso...</p>
      } @else if(generiMovies().length === 0){
      <p class="muted">Nessun genere di film presente</p>
      } @else{
      <div class="list">
        @for(item of generiMovies(); track item.id) {
        <div class="list-item">
          <div>
            <strong>{{ item.genere }}</strong>
          </div>
        </div>
        }
      </div>
      }
    </article>
  </div>
</section>
</details>
```

### genere-movie-list.component.ts V 1.0
Utente: Greg
Data: 03/06/2026
Descrizione: Creato il componente list genere movie

```ts

// Import dei simboli principali di Angular e del supporto per i segnali reattivi.
import { Component, inject, signal } from '@angular/core';
//import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { AuthService } from '../../../services/auth.service';
import { GenereMovieService } from '../../../services/genere-movie.service';
import { GenereMovie } from '../../../models/genere-movie.model';

@Component({
  selector: 'app-genere-movie',
  imports: [],
  templateUrl: './genere-movie.list.component.html',
})

export class GenereMoviePage {

  // Iniezione dei servizi necessari per autenticazione e gestione dei generi film.
  private readonly authService = inject(AuthService);
  private readonly genereMovieService = inject(GenereMovieService);

  // Stato reattivo del componente: lista generi, caricamento e messaggi.
  readonly generiMovies = signal<GenereMovie[]>([]);
  readonly isLoading = signal(false);
  readonly errorMessage = signal('');
  readonly successMessage = signal('');

  constructor() {
    // Al caricamento del componente, viene eseguita la richiesta dei generi.
    this.loadGeneriMovies();
  }

  // Recupera la lista dei generi dei film dal backend.
  loadGeneriMovies(): void {
    this.isLoading.set(true); // Mostra lo stato di caricamento.
    this.errorMessage.set(''); // Resetta eventuali messaggi di errore precedenti.

    // Chiamata al servizio che restituisce un Observable contenente i generi.
    this.genereMovieService.ottieniTutto().subscribe({
      next: (items) => {
        // Aggiorna la lista quando i dati sono disponibili.
        this.generiMovies.set(items);
        this.isLoading.set(false);
      },
      error: (error: unknown) => {
        // Gestione dell'errore e aggiornamento dello stato.
        this.isLoading.set(false);
        this.errorMessage.set(
          this.extractErrorMessage(error, 'Impossibile caricare i generi dei film')
        );
      }
    });
  }

  // Funzione di trackBy usata per ottimizzare il rendering delle liste in Angular.
  trackById(_: string, item: GenereMovie): string | null {
    return item.id;
  }

  // Estrae un messaggio di errore leggibile dall'oggetto ricevuto.
  private extractErrorMessage(error: unknown, fallback: string): string {
    if (error instanceof Error) {
      return error?.message ?? fallback;
    }
    return fallback;
  }
}

```

### genere-movie-list.component.html V 1.0

Utente: Greg
Data: 03/06/2026
Descrizione: Creato file html per mostrare la list genere movie

```html
<!-- Contenitore principale della sezione -->
<section>
    <!-- Titolo principale della pagina -->
    <h1 class="page-title">Generi di Film</h1>
    <!-- Sottotitolo descrittivo -->
    <p class="page-subtitle"> il backend restituisce la lista generi di film</p>

     <!-- Mostra messaggio di errore se presente nel segnale errorMessage -->
     @if(errorMessage()){
        <div class="alert alert-warning">{{ errorMessage()}}</div>
     }

     <!-- Mostra messaggio di successo se presente nel segnale successMessage -->
     @if(successMessage()){
        <div class="alert alert-success">{{ successMessage() }}</div>
     }

     <!-- Contenitore griglia a 2 colonne -->
     <div class="grid grid-2">
        <!-- Card principale per la lista generi -->
        <article class="card">
            <h2>Lista generi di film</h2>

            <!-- Verifica lo stato di caricamento -->
            @if(isLoading()){
                <!-- Mostra messaggio di caricamento mentre i dati vengono recuperati -->
                <p class="muted"> Caricamento in corso...</p>
            } @else if(generiMovies().length === 0){
                <!-- Mostra messaggio quando non ci sono generi disponibili -->
                <p class="muted"> Nessun genere di film presente</p>
            } @else{
                <!-- Contenitore della lista generi -->
                <div class="list">
                    <!-- Loop di iterazione sulla lista generiMovies usando trackBy per l'ID -->
                    @for(item of generiMovies(); track item.id) {
                        <!-- Elemento singolo della lista -->
                        <div class="list-item">
                            <div>
                                <!-- Nome del genere in grassetto -->
                                <strong>{{ item.genere }}</strong>
                                <!-- ID del genere in testo attenuato -->
                                <div class="muted">ID: {{ item.id }}</div>
                            </div>
                        </div>
}
                </div>
            }
        </article>
     </div>
</section>

```

### log-azioni.page.html
<details>
<summary>Versione 1.0</summary>

```html
<section>
    <h1 class="page-title">Log di Sistema</h1>
    <p class="page-subtitle">Visualizzazione delle operazioni registrate nel Nuovo Cinema Paradiso.</p>

     @if(errorMessage()){
        <div class="alert alert-warning">{{ errorMessage() }}</div>
     }

     <div>
        <article class="card">
            <h2>Cronologia Operazioni</h2>

            @if(isLoading()){
                <p class="muted">Caricamento log in corso...</p>
            } @else if(logs().length === 0){
                <p class="muted">Nessun log presente nel sistema.</p>
            } @else {
                <div class="list">
                    @for(item of logs(); track trackById($index, item)){
                        <div class="list-item">
                            <div>
                                <p><strong>{{ item.nomeAzione }}</strong>
                                
                                {{ item.messaggio }} (Esito: {{ item.effettuato ? 'Successo' : 'Fallito' }})</p>
                                
                                <div class="muted"> 
                                    Utente ID: {{ item.idUtente || 'Sistema' }} | 
                                    Data: {{ item.timeStamp | date:'dd/MM/yyyy HH:mm:ss' }}
                                </div>
                            </div>
                        </div>
                    }
                </div>
            }
        </article>
     </div>
</section>
```
</details>

### log-azioni.page.ts
<details>
<summary>Versione 1.0</summary>

```ts
import { Component, inject, signal, OnInit } from '@angular/core';
import { DatePipe } from '@angular/common';
import { LogAzioniService } from '../../services/log-azioni.service';
import { LogAzioni } from '../../models/logAzioni.model';

@Component({
  selector: 'app-log-azioni',
  standalone: true,
  imports: [DatePipe], 
  templateUrl: './log-azioni.page.html',
  styleUrl: './log-azioni.page.css',
})
export class LogAzioniPage implements OnInit {

  private readonly logAzioniService = inject(LogAzioniService);

  readonly logs = signal<LogAzioni[]>([]);
  readonly isLoading = signal(false);
  readonly errorMessage = signal('');

  ngOnInit(): void {
    this.loadLogs();
  }

  loadLogs(): void {
    this.isLoading.set(true);
    this.errorMessage.set('');

    this.logAzioniService.ottieniTutti().subscribe({
      next: (items) => {
        this.logs.set(items);
        this.isLoading.set(false);
      },
      error: (error: unknown) => {
        this.isLoading.set(false);
        this.errorMessage.set(
          this.extractErrorMessage(error, 'Impossibile caricare i log')
        );
      }
    });
  }

  trackById(_: number, item: LogAzioni): string | null {
    return item.id;
  }

  private extractErrorMessage(error: unknown, fallback: string): string {
    if (error instanceof Error) {
      return error?.message ?? fallback;
    }
    return fallback;
  }
}
```
</details>

### tipologia-sala-list.component.ts

```ts
/**
 * Componente Angular della lista di modelli tipologia-sala
 * questo componente angular sfrutta il servizio relativo per creare una lista
 * con ottieniTutto(), questo ottiene tutti le iterazione del db di tipologia-sala
 */
import { Component, OnInit } from '@angular/core';
import { TipologiaSala } from '../../models/tipologia-sala.model'; //path del modello
import { TipologiaSalaService } from '../../services/tipologia-sala.service'; //path del servizio

@Component({
  selector: 'app-tipologia-sala-list' /*è il nome html dello Slug con cui usi 
                                        un componente angular dentro un template
                                        ,nel nostro caso possiamo usare quel componente cosi:
                                        <app-tipologia-sala-list> </app-tipologia-sala-list>
                                        in pratica, quando angular trova questo tag html,
                                        mostra questo componente utilizzando il suo template,
                                        la rotta angular diventerebbe questa: 
                                        {
                                        path: 'tipologia-sala-list', 
                                        component: TipologiaSalaListComponent
                                        }*/,
  templateUrl: './tipologia-sala-list.component.html', //URL del template
})
export class TipologiaSalaListComponent implements OnInit {
  tipologiaSala: TipologiaSala[] = []; //lista

  // L'iniezione del servizio nel costruttore
  constructor(private tipologiaSalaService: TipologiaSalaService) {}

  //funzione che chiama il servizio per creare una lista
  ngOnInit(): void {
    this.tipologiaSalaService.ottieniTutto().subscribe((data) => {
      this.tipologiaSala = data;
    });
  }
}
```

### tipologia-sala-list.component.ts V 1.1

- Utente: Greg
- Data: 03/06/2026
- Descrizione: Aggiunto componente lista

```ts
/**
 * Componente Angular della lista di modelli tipologia-sala
 * questo componente angular sfrutta il servizio relativo per creare una lista
 * con ottieniTutto(), questo ottiene tutti le iterazione del db di tipologia-sala
 */
import { Component, inject, signal } from '@angular/core';
import { AuthService } from '../../../services/auth.service';
import { TipologiaSala } from '../../../models/tipologia-sala.model'; 
import { TipologiaSalaService } from '../../../services/tipologia-sala.service';

@Component({
  selector: 'tipologia-sala-list',
  /*è il nome html dello Slug con cui usi 
                                        un componente angular dentro un template
                                        ,nel nostro caso possiamo usare quel componente cosi:
                                        <app-tipologia-sala-list> </app-tipologia-sala-list>
                                        in pratica, quando angular trova questo tag html,
                                        mostra questo componente utilizzando il suo template,
                                        la rotta angular diventerebbe questa: 
                                        {
                                        path: 'tipologia-sala-list', 
                                        component: TipologiaSalaListComponent
                                        }*/,
  templateUrl: './tipologia-sala-list.component.html', //URL del template
})

export class TipologiaSalaPage{

  // Iniezione del servizio di autenticazione
  private readonly authService = inject(AuthService);
  // Iniezione del servizio per gestire le tipologie di sala
  private readonly tipologiaSalaService = inject(TipologiaSalaService);

  // Signal per memorizzare l'array delle tipologie di sala
  readonly tipologiaSala = signal<TipologiaSala[]>([]);
  // Signal per tracciare lo stato di caricamento
  readonly isLoading = signal(false);
  // Signal per memorizzare i messaggi di errore
  readonly errorMessage = signal('');
  // Signal per memorizzare i messaggi di successo
  readonly successMessage = signal('');


  // Costruttore che carica le tipologie di sala al momento dell'inizializzazione
  constructor() {
    this.loadTipologiaSala();
  }

  // Metodo che carica tutte le tipologie di sala dal servizio
  loadTipologiaSala(): void {
    // Imposta lo stato di caricamento a true
    this.isLoading.set(true);
    // Pulisce il messaggio di errore precedente
    this.errorMessage.set('');

    // Effettua la chiamata HTTP per ottenere tutte le tipologie
    this.tipologiaSalaService.ottieniTutto().subscribe({
      // Nel caso di successo
      next: (items) => {
        // Aggiorna il signal con gli elementi ricevuti
        this.tipologiaSala.set(items);
        // Disattiva lo stato di caricamento
        this.isLoading.set(false);
      },
      // Nel caso di errore
      error: (error: unknown) => {
        // Disattiva lo stato di caricamento
        this.isLoading.set(false);
        // Imposta il messaggio di errore estratto dall'eccezione
        this.errorMessage.set(
          this.extractErrorMessage(error, 'Impossibile caricare la tipologia sala')
        );
      }
    });
  }

  // Metodo trackBy per ottimizzare il rendering di *ngFor
  trackById(_: string, item: TipologiaSala): string | null {
    return item.id
  }

  // Metodo privato che estrae il messaggio di errore da un'eccezione
  private extractErrorMessage(error: unknown, fallback: string): string {
    // Controlla se l'errore è un'istanza di Error
    if (error instanceof Error) {
      // Ritorna il messaggio dell'errore o il fallback se non disponibile
      return error?.message ?? fallback;
    }
    // Ritorna il messaggio di fallback se l'errore non è un Error
    return fallback
  }
}
```

### tipologia-sala-list.component.html

componente html template della lista di tipologia-sala

```html
<div class="container">
  <!-- contenitore della lista -->
  <ul>
    <!-- Cicla su ogni 'tipologia' presente nell'array tipologiaSala -->
    <li *ngFor="let tipologia of tipologiaSala">
      {{ tipologia.id }} - {{ tipologia.nome }} - {{tipologia.maggiorazionePrezzo}}€
    </li>
  </ul>
</div>
```

### tipologia-sala-detail.componenr.ts

```ts

/**
 * Componente Angular del modello tipologia-sala in dettaglio
 * questo componente angular sfrutta il servizio relativo per mostrare l'iterazione in dettaglio
 * con ottieniTramiteId(), questo ottiene la iterazione del db di tipologia-sala con tale id in dettaglio
 */
import { Component, OnInit } from '@angular/core';
import { TipologiaSala } from '../../models/tipologia-sala.model'; //path del modello
import { TipologiaSalaService } from '../../services/tipologia-sala.service'; //path del servizio

@Component({
  selector: 'app-tipologia-sala-detail',
  templateUrl: './tipologia-sala-detail.component.html', //URL del template
})
export class TipologiaSalaDetailComponent implements OnInit {
  tipologiaSala: TipologiaSala | null = null;
  id: string = ''; // impostare questo ID in qualche modo, ad esempio tramite route parametrica

  // L'iniezione del servizio e del campo id nel costruttore
  constructor(
    private tipologiaSalaService: TipologiaSalaService,
    id: string,
  ) {
    this.id = id;
  }
  //funzione del servizio per prenderee una tipologia-sala in dettaglio
  ngOnInit(): void {
    this.tipologiaSalaService.ottieniTramiteId(this.id).subscribe((data) => {
      this.tipologiaSala = data;
    });
  }
}
```

### tipologia-sala-list.component.html V 1.1

- Utente: Greg
- Data: 03/06/2026
- Descrizione: Aggiunto componente lista

```html
<section>
    <h1 class="page-title">Tipologia Sala</h1>
    <p class="page-subtitle"> il backend restituisce le varie tipologia delle sale</p>

      <!-- Mostra un messaggio di errore se presente -->
      @if(errorMessage()){
        <div class="alert alert-warning">{{ errorMessage()}}</div>
      }

      <!-- Mostra un messaggio di successo se presente -->
      @if(successMessage()){
        <div class="alert alert-success">{{ successMessage() }}</div>
      }

     <div class="grid grid-2">
        <article class="card">
            <h2>Lista delle sale del cinema</h2>

            <!-- Stato di caricamento / nessun dato / lista popolata -->
            @if(isLoading()){
              <p class="muted"> Caricamento in corso...</p>
            } @else if(tipologiaSala().length === 0){
              <p class="muted"> Nessuna tipologia sala presente</p>
            } @else{
                <div class="list">
                    <!-- Ciclo sui risultati: per ogni tipologia mostra nome, maggiorazione e id -->
                    @for(item of tipologiaSala(); track item.id) {
                      <div class="list-item">
                        <div>
                          <strong>{{ item.nome }}</strong>
                          <p> {{item.maggiorazionePrezzo}} €</p>
                          <div class="muted"> {{ item.id }}</div>
                        </div>
                      </div>
                    }
                </div>
            }
        </article>
     </div>
</section>

```

### tipologia-sala-detail.component.html

non vi è encora un css

```html
<!-- componente in dettaglio di una tiplogia-sala,
  ha un titolo e un paragrafo per ogni campo del modello in un div-->
<div>
  <h2>Dettagli Tipologia Sala</h2>
  <div><span>ID: </span><span>{{ tipologiaSala?.id }}</span></div>
  <div><span>Nome: </span><span>{{ tipologiaSala?.nome }}</span></div>
  <div>
    <span>Maggiorazione Prezzo: </span><span>{{ tipologiaSala?.maggiorazionePrezzo }}€</span>
  </div>
</div>
```


### sala-page.ts
Alessandro Gregorio

26/05/2026

Componente CRUD per la gestione delle sale: carica la lista dal backend, permette a utenti con ruolo Operatore di creare, modificare ed eliminare sale tramite un form reattivo
<details>

<summary> sala.page.ts (V1.0)</summary>

**versione1.0**



```ts
import { Component, inject, signal } from '@angular/core';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { HttpErrorResponse } from '@angular/common/http';
import { AuthService } from '../../services/auth.service';
import { SalaService } from '../../services/sala.service';
import { Observable } from 'rxjs';
import { Sala } from '../../models/sala.model';

@Component({
  // Tag HTML con cui questo componente viene dichiarato nel router o in altri template
  selector: 'sala-page',
  // Componente standalone: non appartiene a nessun NgModule, si autogestisce
  standalone: true,
  // ReactiveFormsModule abilita le direttive formGroup, formControlName ecc. nel template
  imports: [ReactiveFormsModule],
  templateUrl: './sala.page.html'
})
export class SalaPage {

  // ===========================
  // DIPENDENZE (Dependency Injection)
  // ===========================

  // FormBuilder: factory per creare FormGroup e FormControl in modo dichiarativo
  private readonly formBuilder = inject(FormBuilder);
  // AuthService: usato per verificare il ruolo dell'utente corrente
  private readonly authService = inject(AuthService);
  // SalaService: espone le chiamate HTTP per le operazioni CRUD sulle sale
  private readonly salaService = inject(SalaService);

  // ===========================
  // STATO REATTIVO (Signals)
  // Ogni signal, quando aggiornato con .set(), notifica Angular di rirenderizzare
  // le parti del template che lo leggono, senza bisogno di Change Detection manuale
  // ===========================

  // Lista delle sale caricate dal backend
  readonly Sala = signal<Sala[]>([]);
  // true mentre è in corso la chiamata GET iniziale
  readonly staCaricando = signal(false);
  // true mentre è in corso una chiamata POST/PUT
  readonly staInviando = signal(false);
  // Messaggio di errore da mostrare all'utente (stringa vuota = nessun errore)
  readonly messaggioErrore = signal('');
  // Messaggio di successo da mostrare dopo un'operazione andata a buon fine
  readonly messaggioSuccesso = signal('');
  // ID della sala in fase di modifica; stringa vuota = siamo in modalità "crea"
  readonly modificaId = signal<string>('');

  // ===========================
  // FORM REATTIVO
  // nonNullable.group garantisce che reset() ripristini i valori iniziali
  // e non null/undefined
  // ===========================
  readonly form = this.formBuilder.nonNullable.group({
    // Nome obbligatorio, massimo 100 caratteri
    nome: ['', [Validators.required, Validators.maxLength(100)]],
    // Capienza obbligatoria, valore minimo 0
    capienza: [0, [Validators.required, Validators.min(0)]],
    // ID della tipologia sala associata, obbligatorio
    tipologiaSalaId: ['', [Validators.required]]
  });

  // ===========================
  // COSTRUTTORE
  // ===========================
  constructor() {
    // Carica subito la lista delle sale all'avvio del componente
    this.caricaSala();

    // Se l'utente non ha i permessi, disabilita tutti i campi del form
    // in modo da impedire qualsiasi interazione, anche aggirando i controlli UI
    if (!this.modificabileDa()) {
      this.form.disable();
    }
  }

  // ===========================
  // CONTROLLO PERMESSI
  // Ritorna true solo se l'utente ha il ruolo "Operatore"
  // Usato sia nel template (visibilità pulsanti) sia nella logica (guardie nei metodi)
  // ===========================
  modificabileDa(): boolean {
    return this.authService.possiedeQualsiasiRuolo(['Operatore']);
  }

  // ===========================
  // CARICAMENTO LISTA SALE
  // ===========================
  caricaSala(): void {
    this.staCaricando.set(true);
    this.messaggioErrore.set('');

    // ottieniTutto() restituisce un Observable<Sala[]>
    // subscribe() si "mette in ascolto" della risposta asincrona del server
    this.salaService.ottieniTutto().subscribe({
      // next: chiamato quando il server risponde con successo
      next: (items) => {
        this.Sala.set(items);         // Aggiorna la lista con i dati ricevuti
        this.staCaricando.set(false); // Nasconde lo spinner di caricamento
      },
      // error: chiamato se la richiesta HTTP fallisce
      error: (error: unknown) => {
        this.staCaricando.set(false);
        this.messaggioErrore.set(this.estraiMessaggioErrore(error, 'Sale non trovate'));
      }
    });
  }

  // ===========================
  // INVIO FORM (Crea o Modifica)
  // ===========================
  invia(): void {
    // Doppia guardia: form non valido o utente senza permessi → abort
    if (this.form.invalid || !this.modificabileDa()) {
      // markAllAsTouched forza la visualizzazione degli errori di validazione nel template

      this.form.markAllAsTouched();
      return;
    }

    this.staInviando.set(true);
    this.messaggioErrore.set('');
    this.messaggioSuccesso.set('');
    
    const request$: Observable<string> = this.modificaId()
  ? this.SalaService.modifica(this.modificaId(), this.form.getRawValue())
  : this.SalaService.crea(this.form.getRawValue());

    request$.subscribe({
      next: () => {
        this.staInviando.set(false);
        // Imposta il messaggio di successo in base all'operazione effettuata
        this.messaggioSuccesso.set(this.modificaId() ? 'Sala aggiornata.' : 'Sala creata.');
        this.ripristinaForm();  
        this.caricaSala(); // Rica  rica la lista per mostrare i dati aggiornati
      },
      error: (error: unknown) => {
        console.log(error)
        this.staInviando.set(false);
        this.messaggioErrore.set(this.estraiMessaggioErrore(error, 'Operazione non riuscita.'));
      }
    });
  }

  inizioModifica(item: Sala): void {
    // Verifica i permessi prima di permettere l'inizio della modifica
    if (!this.modificabileDa()) {
      return;
    }
    this.modificaId.set(item.id!);
    // dizionario che rappresenta la mappa del modulo
    this.form.patchValue({ nome: item.nome, capienza: item.capienza,tipologiaSalaId: item.tipologiaSalaId});
    this.messaggioErrore.set('');
    this.messaggioSuccesso.set('');
  }
  
  elimina(item: Sala) : void {
    if(!this.modificabileDa()) {
      return;
    }
    const confirmed = confirm(`Eliminare la sala \" ${item.nome}\"?`);
    if(!confirmed){
      return;
    }

    this.messaggioErrore.set('');
    this.messaggioSuccesso.set('');

    this.SalaService.elimina(item.id!).subscribe({
      next : () => {
        if (this.modificaId() === item.id) {
          this.ripristinaForm();
        }
        this.caricaSala();
      },
      error: (error: unknown) => {
    
      this.messaggioErrore.set(this.estraiMessaggioErrore(error, 'eliminazione non riuscita.'));
      }
    });  
  }

  ripristinaForm() : void {
    this.modificaId.set('');
    this.form.reset({ nome: '', capienza: 0 ,tipologiaSalaId : ''});
  }
  // il _ indica che il primo parametro non viene utilizzato,
  //  è una convenzione per indicare che è presente ma non serve
  tracciaPerId(_: string, item: Sala) : string {
    return item.id!;
  }

  private estraiMessaggioErrore(error: unknown, fallback: string): string {
    
    if (error instanceof HttpErrorResponse) {
      return error.error?.message ?? fallback;
    }
    return fallback;
  }
}
```
</details>
Alessandro Gregorio

25/05/2026

Template della pagina Sale: mostra la lista delle sale e un form reattivo per crearle o modificarle, con messaggi di feedback e controlli visibili solo agli utenti autorizzati.
<details>

<summary> sala.page.html (V1.0)</summary>



```html
<!-- ===========================
     SEZIONE PRINCIPALE: SALE
     Gestione CRUD delle sale (lista + form)
     =========================== -->
<section>

  <!-- Titolo e sottotitolo della pagina -->
  <h1 class="page-title">Sale</h1>
  <p class="page-subtitle">Gestisci le tue sale</p>

  <!-- Alert di errore: visibile solo se il segnale messaggioErrore() ha un valore -->
  @if (messaggioErrore()) {
    <div class="alert alert-warning">{{ messaggioErrore() }}</div>
  }

  <!-- Alert di successo: visibile solo se il segnale messaggioSuccesso() ha un valore -->
  @if (messaggioSuccesso()) {
    <div class="alert alert-success">{{ messaggioSuccesso() }}</div>
  }

  <!-- Layout a due colonne: lista sale | form aggiunta/modifica -->
  <div class="grid grid-2">

    <!-- ===========================
         CARD SINISTRA: Lista Sale
         =========================== -->
    <article class="card">
      <h2>Lista Sale</h2>

      <!-- Stato di caricamento: mostrato mentre i dati vengono recuperati dal server -->
      @if (staCaricando()) {
        <p class="muted">Caricamento in corso...</p>

      <!-- Stato vuoto: nessuna sala presente nel sistema -->
      } @else if (Sala().length === 0) {
        <p class="muted">Nessuna Sala presente.</p>

      <!-- Lista sale: iterazione con @for sul segnale Sala() -->
      } @else {
        <div class="list">

          <!-- Ogni item rappresenta una singola sala; track per item.id per ottimizzare il rendering -->
          @for (item of Sala(); track item.id) {
            <div class="list-item">

              <!-- Dettagli della sala -->
              <div>
                <strong>Nome: {{ item.nome }}</strong>
                <p>Capienza: {{ item.capienza }}</p>
                <p>Tipologia: {{ item.nomeTipologia }}</p>
                <p>TipologiaID: {{ item.tipologiaSalaId }}</p>
                <div class="muted">ID: {{ item.id }}</div>
              </div>

              <!-- Pulsanti azione: visibili solo se l'utente ha i permessi (modificabileDa()) -->
              @if (modificabileDa()) {
                <div class="btn-row">
                  <!-- Popola il form con i dati della sala selezionata per la modifica -->
                  <button class="btn btn-secondary" type="button" (click)="inizioModifica(item)">Modifica</button>
                  <!-- Elimina la sala dopo conferma -->
                  <button class="btn btn-danger" type="button" (click)="elimina(item)">Elimina</button>
                </div>
              }

            </div>
          }

        </div>
      }
    </article>

    <!-- ===========================
         CARD DESTRA: Form Crea / Modifica
         Il titolo cambia dinamicamente in base a modificaId()
         =========================== -->
    <article class="card">
      <h2>{{ modificaId() ? 'Modifica Sala' : 'Aggiungi Sala' }}</h2>

      <!-- Messaggio informativo per utenti senza permessi di modifica -->
      @if (!modificabileDa()) {
        <div>
          Il tuo ruolo non ti permette di modificare o eliminare le Sale.
        </div>
      }

      <!-- Form reattivo Angular; invia i dati tramite invia() all'ngSubmit -->
      <form class="form-grid" [formGroup]="form" (ngSubmit)="invia()">
        <div>

          <!-- Campo: nome della sala -->
          <label for="nome">Nome Sala</label>
          <input id="nome" type="text" formControlName="nome" [disabled]="!modificabileDa()">

          <!-- Campo: capienza massima della sala -->
          <label for="capienza">capienza</label>
          <input id="capienza" type="number" formControlName="capienza" [disabled]="!modificabileDa()">

          <!-- Campo: ID della tipologia associata alla sala -->
          <label for="tipologiaSalaId">tipologiaSalaId</label>
          <input id="tipologiaSalaId" type="text" formControlName="tipologiaSalaId" [disabled]="!modificabileDa()">

        </div>

        <!-- Pulsanti form -->
        <div class="btn-row">

          <!--
            Submit: disabilitato durante l'invio o senza permessi.
            Il testo cambia dinamicamente:
              - "Salvataggio..." durante l'invio
              - "Aggiorna" in modalità modifica
              - "Crea" in modalità creazione
          -->
          <button class="btn btn-primary" type="submit" [disabled]="staInviando() || !modificabileDa()">
            {{ staInviando() ? 'Salvataggio...' : (modificaId() ? 'Aggiorna' : 'Crea') }}
          </button>

          <!-- Annulla modifica: visibile solo in modalità modifica, ripristina il form allo stato iniziale -->
          @if (modificaId()) {
            <button class="btn btn-secondary" type="button" (click)="ripristinaForm()">Annulla</button>
          }

        </div>
      </form>
    </article>

  </div>
</section>

```

</details>

### movie.page.ts
Francesco Lorenzi
Data:25/05/2026

<details>

<summary> movie.page.ts (V1.0)</summary>

```typescript
//importo servizi
import { AuthService } from '../../services/auth.service';
import { MovieService } from '../../services/movie.service';
import { GenereMovieService } from '../../services/genere-movie.service';
//importo modelli
import { Movie } from '../../models/movie.model';
import { GenereMovie } from '../../models/genere-movie.model';

@Component({
  selector: 'movie-page',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './movie.page.html',
})
export class MoviePage {
  //servizi
  private readonly formBuilder = inject(FormBuilder);
  private readonly authService = inject(AuthService);
  private readonly movieService = inject(MovieService);
  private readonly genereService = inject(GenereMovieService);
  //modelli
  readonly movies = signal<Movie[]>([]);
  readonly generi = signal<GenereMovie[]>([]);

  readonly staCaricando = signal(false);
  readonly staInviando = signal(false);
  readonly messaggioErrore = signal('');
  readonly messaggioSuccesso = signal('');
  readonly modificaId = signal<string>('');

  //form per la creazione di un modello Movie
  readonly form = this.formBuilder.nonNullable.group({
    titolo: ['', [Validators.required, Validators.maxLength(100)]],
    descrizione: ['', [Validators.required, Validators.maxLength(200)]],
    durataMinuti: [1, [Validators.required, Validators.min(1)]],
    prezzoMovie: [0.01, [Validators.required, Validators.min(0.01), Validators.max(999999999)]],
    genereId: ['', [Validators.required]],
  });
  //il costruttore carica anche i generi
  //cosicché questi possano essere selezionati nel form
  constructor() {
    this.caricaMovies();
    this.caricaGeneri();
  }
  modificabileDa(): boolean {
    return this.authService.possiedeQualsiasiRuolo(['Operatore']);
  }
  //metodo per caricare i movie
  caricaMovies(): void {
    this.staCaricando.set(true);
    this.messaggioErrore.set('');
    this.movieService.ottieniTutto().subscribe({
      next: (items) => {
        this.movies.set(items);
        this.staCaricando.set(false);
      },
      error: (error: unknown) => {
        this.staCaricando.set(false);
        this.messaggioErrore.set(this.estraiMessaggioErrore(error, 'film non trovati'));
      },
    });
  }
  //metodo per caricare i generi
  caricaGeneri(): void {
    this.staCaricando.set(true);
    this.messaggioErrore.set('');
    this.genereService.ottieniTutto().subscribe({
      next: (items) => {
        this.generi.set(items);
        this.staCaricando.set(false);
      },
      error: (error: unknown) => {
        this.staCaricando.set(false);
        this.messaggioErrore.set(this.estraiMessaggioErrore(error, 'generi non trovati'));
      },
    });
  }

  invia(): void {
    // Controlla se il form è invalido
    // o se l'utente non ha i permessi per modificare
    if (this.form.invalid || !this.modificabileDa()) {
      this.form.markAllAsTouched();
      return;
    }

    this.staInviando.set(true);
    this.messaggioErrore.set('');
    this.messaggioSuccesso.set('');

    // Scelta dinamica dell'operazione:
    // - Se modificaId ha un valore → PUT (aggiornamento sala esistente)
    // - Se modificaId è vuoto   → POST (creazione nuova sala)
    const request$: Observable<string> = this.modificaId()
      ? this.salaService.modifica(this.modificaId(), this.form.getRawValue())
      : this.salaService.crea(this.form.getRawValue());

    request$.subscribe({
      next: () => {
        this.staInviando.set(false);
        // Messaggio contestuale in base all'operazione appena completata
        this.messaggioSuccesso.set(this.modificaId() ? 'Film aggiornato.' : 'Film creato.');
        this.ripristinaForm(); // Torna in modalità "crea" e svuota i campi
        this.caricaMovies();    // Ricarica la lista per mostrare i dati aggiornati
      },
      error: (error: unknown) => {
        this.staInviando.set(false);
        this.messaggioErrore.set(this.estraiMessaggioErrore(error, 'Operazione non riuscita.'));
      }
    });
  }

  inizioModifica(item: Movie): void {
    // Guardia permessi: esce subito se l'utente non può modificare
    if (!this.modificabileDa()) return;

    this.modificaId.set(item.id!);

    // patchValue aggiorna solo i campi specificati, lasciando invariati gli altri
    this.form.patchValue({ titolo: item.titolo, descrizione: item.descrizione, durataMinuti: item.durataMinuti, prezzoMovie: item.prezzoMovie, genereId: item.genereId });

    // Pulisce eventuali messaggi residui da operazioni precedenti
    this.messaggioErrore.set('');
    this.messaggioSuccesso.set('');
  }

  // ===========================
  // ELIMINAZIONE FILM
  // ===========================
  elimina(item: Movie): void {
    // Guardia permessi
    if (!this.modificabileDa()) return;

    // Richiede conferma esplicita all'utente prima di procedere
    const confirmed = confirm(`Eliminare il film "${item.nome}"?`);
    if (!confirmed) return;

    this.messaggioErrore.set('');
    this.messaggioSuccesso.set('');

    this.movieService.elimina(item.id!).subscribe({
      next: () => {
        // Se il film eliminato era quella in modifica, resetta il form
        // per evitare di lavorare su un'entità che non esiste più
        if (this.modificaId() === item.id) {
          this.ripristinaForm();
        }
        this.caricaMovies(); // Aggiorna la lista rimuovendo il film eliminato.
      },
      error: (error: unknown) => {
        this.messaggioErrore.set(this.estraiMessaggioErrore(error, 'Eliminazione non riuscita.'));
      }
    });
  }

  // ===========================
  // RIPRISTINO FORM
  // Torna alla modalità "crea" svuotando modificaId e resettando i campi
  // ===========================
  ripristinaForm(): void {
    this.modificaId.set('');
    this.form.reset({ titolo: '', descrizione: '', durataMinuti: 0, prezzoMovie: 0, genereId: '' });

  }

  // ===========================
  // FUNZIONE DI TRACKING per @for
  // Restituisce l'ID univoco di ogni sala così Angular può identificare
  // quali elementi del DOM aggiornare senza ricreare l'intera lista
  // Il parametro _ (indice) è ignorato per convenzione
  // ===========================
  tracciaPerId(_: string, item: Sala): string {
    return item.id!;
  }

  // ===========================
  // UTILITY: ESTRAZIONE MESSAGGIO DI ERRORE
  // Controlla se l'errore è una risposta HTTP e tenta di estrarne il messaggio
  // Se non disponibile, usa il messaggio di fallback fornito dal chiamante
  // ===========================
  private estraiMessaggioErrore(error: unknown, fallback: string): string {
    if (error instanceof HttpErrorResponse) {
      // L'operatore ?. evita crash se error.error è null/undefined
      // ?? usa il fallback se message non esiste
      return error.error?.message ?? fallback;
    }
    return fallback;
  }
}
```

</details>



## MODELS

### abbonamento.model.ts

<details>
<summary>abbonamento.model.ts V1.0</summary>

Francesco Lorenzi 26/05/2026
```ts

export interface Abbonamento {
    id: string; //la chiave primaria non può essere null
    nome: string;
    durata: number;
    prezzo: number;
    sconto: number;
}

export interface AbbonamentoCreazione {
    nome: string;
    durata: number;
    prezzo: number;
    sconto: number;
} 
```
</details>


### auth.model.ts
<details>
<summary> auth.model.ts V1.0</summary>

```ts
// modello per la creazione della sessione dell'utente
export interface Auth {
  token: string;
  id: string;
  email: string;
  nomeCompleto: string;
  ruolo: string;
}
```
</details>

### gestione.model.ts

<details><summary> gestione.model.ts V1 </summary>
- Utente: Simeone
- Data: 28/05/2026
- Descrizione: creazione modello per pagina di gestione dedita al ruolo gestore

```ts
export interface LogAzioni {
  id: string | null;
  idUtente: string | null;
  nomeAzione: string;
  effettuato: boolean;
  messaggio: string;
  timeStamp: string;
}

export interface LogAzioniCreazione {
    id: string | null;
    idUtente: string;
    nomeAzione: string;
    effettuato: boolean;
    messaggio: string;
    timeStamp: string;
}

export interface ContoCinema {
  id: string;
  iban: string;
  titolareConto: string;
  saldo: number;
}

export interface Biglietto {
  id: string | null;
  utenteId: string;
  prezzoFinale: number;
  orarioCreazione: string;
  proiezioneId: string;
  numeroBiglietti: number;
  nomeSala: string;
  titoloMovie: string;
  nomeTipologiaSala: string;
  oraInizio: string; 
  dataProiezione: string; 
}

export interface GiftCard {
  id: string | null;
  nome: string;
  valore: number;
  codiceRiscatto: string;
}
```
</details>

### biglietto.model.ts

<details><summary> biglietto.model.ts V1.0 </summary>

```ts
// modello di ritorno che rappresenta un Biglietto
export interface Biglietto {
  id: string | null;
  proiezioneId: string;
  utenteId: string;
  prezzoFinale: number;
  orarioCreazione: string;
  numeroBiglietti: number;
  metodoPagamento: string;
}
// modello di inserimento necessario per la creazione o la modifica di un modello Biglietto
export interface BigliettoCreazione {
  proiezioneId: string;
  numeroBiglietti: number;
  metodoPagamento: string;
}
```
</details>

### genere-movie.model.ts

<details><summary> genere-movie.model V1.0</summary>

```ts
// modello di ritorno che rappresenta un Genere
export interface GenereMovie {
  id: string;
  genere: string;
}
// modello di inserimento necessario per la creazione o la modifica di un modello Genere
export interface GenereMovieCreazione {
  genere: string;
}
```
</details>

Utente: Marco Strazzeri
Data: 25/05/2026
Descrizione: Rimosso GenereMovieCreazione perché non creiamo generi

<details><summary> genere-movie.model V1.1</summary>

```ts
// modello di ritorno che rappresenta un Genere
export interface GenereMovie {
  id: string | null;
  genere: string;
}

```
</details>

### giftcard.model.ts

<details><summary> giftcard.model V1.0 </summary>

```ts
// modello di ritorno che rappresenta una GiftCard

export interface GiftCard {
  id: string | null;
  nome: string;
  durata: number;
  prezzo: number;
  numeroMovie: number;
}
// modello di inserimento necessario per la creazione o la modifica di un modello GiftCard
export interface GiftCardCreazione {
  nome: string;
  durata: number;
  prezzo: number;
  numeroMovie: number;
}
```
</details >

### giftCard.model.ts

Andrea Bruno 28-05-2026
Aggiunta modello CodiceRiscatto

<details><summary> giftcard.model V1.1 </summary>

```c#
export interface GiftCard {
    id: string | null;
    nome: string;
    valore: number;
    codiceRiscatto: string;
}

export interface GiftCardCreazione {
    nome: string;
    valore: number;
    codiceRiscatto: string;
}

export interface RicaricaGiftCard {
    importo: number;
}

export interface CodiceRiscatto {    // <-- AGGIUNTA
    CodiceRiscatto: string;
}
```

### logAzioni.model.ts

<details><summary> logAzioni.model V1.0 </summary>

```ts
// modello di ritorno che rappresenta una Log di un'azione di un utente
export interface LogAzioni {
  id: string | null;
  idUtente: string | null;
  nomeAzione: string;
  effettuato: boolean;
  messaggio: string;
  timeStamp: string;
}
// modello di inserimento necessario per la creazione dei Log delle azioni degli utenti
export interface LogAzioniCreazione {
  id: string | null;
  idUtente: string;
  nomeAzione: string;
  effettuato: boolean;
  messaggio: string;
  timeStamp: string;
}
```
</details>

### login.model.ts
<details><summary> login.model V1.0 </summary>

```ts
//modello necessario per confermare i dati d'accesso dell'utente e ricevere il token
export interface Login {
  email: string;
  password: string;
}
```
</details>

### movie.model.ts
<details><summary> movie.model V1.0 </summary>

```ts
// modello di ritorno che rappresenta un Movie (film)
export interface Movie {
  id: string | null;
  titolo: string;
  descrizione: string;
  durataMinuti: number;
  prezzoMovie: number;
  genereId: string;
  genere: string;
}
// modello di inserimento necessario per la creazione o la modifica di un Movie (film)
export interface MovieCreazione {
  titolo: string;
  descrizione: string;
  durataMinuti: number;
  prezzoMovie: number;
  genereId: string;
}
```
</details >


### movie.model.ts

Francesco Lorenzi
25/05/2026
ho dovuto modificare il campo id perchè giustamente questo non dovrebbe essere nullabile

<details><summary>movie.model V1.1 </summary>

```ts
export interface Movie {
  id: string; //la chiave primaria non può essere null
  titolo: string;
  descrizione: string;
  durataMinuti: number;
  prezzoMovie: number;
  genereId: string;
  genere: string;
}

export interface MovieCreazione {
  titolo: string;
  descrizione: string;
  durataMinuti: number;
  prezzoMovie: number;
  genereId: string;
}
```
</details>

### proiezione.model.ts
<details>
<summary> proiezione.model V1.0 </summary>

```ts
// modello di ritorno che rappresenta una Proiezione
export interface Proiezione {
  id: string;
  dataProiezione: Date;
  movieId: string;
  salaId: string;
  turnoId: string;
}
// modello di inserimento necessario per la creazione o la modifica di una Proiezione
export interface ProiezioneCreazione {
  movieId: string;
  salaId: string;
  turnoId: string;
  dataProiezione: Date;
}
```
</details>

Utente: Fabio Tammaro
Data: 03/06/2026
Descrizione: aggiunte proprietà necessarie alla visualizzazione nella lista delle proiezioni.
<details><summary> proiezione.model V1.1 </summary>

```ts
export interface Proiezione {
    id: string;
    dataProiezione: Date;
    movieId: string;
    titoloMovie: string;
    salaId: string;
    nomeSala: string;
    turnoId: string;
    nomeTurno: string;
}

export interface ProiezioneCreazione {
    movieId: string;
    salaId: string;
    turnoId: string;
    dataProiezione: string;
}
```
</details>

### registrazione.model.ts V1.0
<details><summary> registrazione.model V1.0 </summary>

```ts
// modello di inserimento necessario per la creazione di un nuovo Utente
export interface Registrazione {
  email: string;
  password: string;
  nomeCompleto: string;
  eta: number;
}
```
</details>

### sala.model.ts
<details>
<summary> sala.model V1.0 </summary>

```ts
// modello di ritorno che rappresenta una Sala
export interface SalaCreazione {
  nome: string;
  capienza: number;
  tipologiaSalaId: string;
}
// modello di inserimento necessario per la creazione o la modifica di una Sala
export interface Sala {
  id: string | null;
  nome: string;
  capienza: number;
  tipologiaSalaId: string;
  nomeTipologia: string;
}
```
</details>
<details><summary> sala.model V1.1 </summary>

```ts

export interface SalaCreazione {
  nome: string;
  capienza: number;
  tipologiaSalaId: string;
}

export interface Sala {
  id: string; //la chiave primaria non può essere nullabile
  nome: string;
  capienza: number;
  tipologiaSalaId: string;
  nomeTipologia: string;
}
```
</details>

### sessione-utente.model.ts
<details>
<summary> sessione-utente.model V1.0 </summary>

```ts
// modello per la gestione della sessione dell'utente
export interface SessioneUtente {
  id: string;
  nomeCompleto: string;
  token: string;
  eta: number;
  email: string;
  ruolo: string;
  dataInizioAbbonamento: string;
  dataInizioGiftCard: string;
  seAbbonato: boolean;
  possiedeGiftCard: boolean;
}
```
</details>

### tipologia-sala.model.ts
<details>
<summary> tipologia-sala.model V1.0 </summary>

```ts
// modello di ritorno che rappresenta una Tipologia di una Sala
export interface TipologiaSala {
  id: string;
  nome: string;
  maggiorazionePrezzo: number;
}

// modello di inserimento necessario per la creazione o la modifica di una Tipologia Sala
export interface TipologiaSalaCreazione {
  nome: string;
  maggiorazionePrezzo: number;
}
```
</details>

### turno.model.ts
<details>
<summary> turno.model V1.0 </summary>

```ts
// modello di ritorno che rappresenta un Turno
export interface Turno {
  id: string | null;
  oraInizio: string;
  oraFine: string;
  nome: string;
}
// modello di inserimento necessario per la creazione o la modifica di un Turno
export interface TurnoCreazione {
  oraInizio: string;
  oraFine: string;
  nome: string;
}
```
</details>

**version1.1**

<details><summary> turno.model V1.0 </summary>

```ts

export interface Turno {
    id: string;// la chiave primario non può essere nullabile
    oraInizio: string;
    oraFine: string;
    nome: string;
}
export interface TurnoCreazione {
    oraInizio: string;
    oraFine: string;
    nome: string;
}
```
</details>

### utente.model.ts
<details>
<summary> utente.model V1.1 </summary>

```ts
// Modello di ritrono che rappresenta un Utente
export interface Utente {
  id: string;
  nomeCompleto: string;
  dataInizioAbbonamento: string;
  dataInizioGiftCard: string;
  seAbbonato: boolean;
  possiedeGiftCard: boolean;
  email: string;
  eta: number;
  abbonamentoId: string;
  giftCardId: string;
  tipoAbbonamento: string;
  tipoGiftCard: string;
}

// modello necessario per la modifica di un Utente
export interface UtenteCreazione {
  nomeCompleto: string;
  eta: number;
}

// modello necessario per la modifica del ruolo di un Utente
export interface UtenteModificaRuolo {
  email: string;
  nuovoRuolo: string;
}
```
</details>

<details><summary> utente.model V1.0 </summary>

```ts
export interface Utente {
    id: string;
    nomeCompleto: string;
    dataInizioAbbonamento: string;
    seAbbonato: boolean;
    email: string;
    eta: number;
    abbonamentoId: string;
    tipoAbbonamento: string;
    Saldo: number;
}

export interface UtenteCreazione {
    nomeCompleto: string;
    eta: number;
}

export interface UtenteModificaRuoloRichiesta {
    email: string;
    nuovoRuolo: string;
}
// aggiunta UtenteModificaRuoloRisposta
export interface UtenteModificaRuoloRisposta {
    messaggio: string;
    email: string;
    ruolo: string;
}
```
</details>

## SERVICES
### operatore.service.ts
<details>
<summary> operatore.service.ts V1.0 </summary>

Andrea Bruno 03-06-2026
Modifica del metodo OttieniUtenti()

```ts
import { environment } from '../../environments/environment';
import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Biglietto } from '../models/biglietto.model';
import { Utente } from '../models/utente.model';

@Injectable({
    providedIn: 'root'
})
export class OperatoreService {

    // HttpClient iniettato per fare richieste HTTP
    private readonly http = inject(HttpClient);

    // URL base dell'API per gli endpoint dell'operatore
    private readonly baseUrl = `${environment.apiBaseUrl}/operatore`;

    // Ottiene la lista completa degli utenti
    OttieniUtenti(): Observable<Utente[]> {
        return this.http.get<Utente[]>(`${this.baseUrl}/listaUtenti`);
    }

    // Ottiene un singolo utente tramite ID
    OttieniUtentePerId(id: string): Observable<Utente> {
        return this.http.get<Utente>(`${this.baseUrl}/utenti/${id}`);
    }

    // Elimina un utente tramite ID
    eliminaUtente(id: string): Observable<void> {
        return this.http.delete<void>(`${this.baseUrl}/utenti/${id}`);
    }

    // Ottiene gli utenti che possiedono un certo abbonamento
    ottieniUtentiTramiteAbbonamento(abbonamentoId: string): Observable<Utente[]> {
        return this.http.get<Utente[]>(
            `${this.baseUrl}/utenti/abbonamento/${abbonamentoId}`
        );
    }

    // Ottiene gli utenti che possiedono una certa gift card
    ottieniUtentiTramiteGiftCard(giftCardId: string): Observable<Utente[]> {
        return this.http.get<Utente[]>(
            `${this.baseUrl}/utenti/giftcard/${giftCardId}`
        );
    }

    // Ottiene tutti i biglietti
    ottieniBiglietti(): Observable<Biglietto[]> {
        return this.http.get<Biglietto[]>(`${this.baseUrl}/biglietti`);
    }

    // Ottiene un singolo biglietto tramite ID
    ottieniBigliettoPerId(id: string): Observable<Biglietto> {
        return this.http.get<Biglietto>(`${this.baseUrl}/biglietti/${id}`);
    }
}
```

### sala.service.ts
<details>
<summary> sala.service.ts V1.0 </summary>

```ts

import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http'; // importo dell'http Client
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment'; //importo dell'enviroment
import { Sala, SalaCreazione  } from '../models/sala.model';//importo dei modelli Sala


@Injectable({ providedIn: 'root' })
export class SalaService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiBaseUrl}/sala`; /*utilizza l'enviroment per creare
                                                                 un url di comunicazione verso il
                                                                  controller del backend*/

  ottieniTutto(): Observable<Sala[]> {
    //chiamta http per ottenere tutte le sale
    return this.http.get<Sala[]>(this.baseUrl);
  }
  ottieniPerTipologia(tipologiaId: string): Observable<Sala[]> {
    // chiamata http per ottenre tutte le sale relative ad una tipologia
    return this.http.get<Sala[]>(`${this.baseUrl}/tipologia/${tipologiaId}`);
    //per creare la chiamata http, bisogna combinare il baseUrl con la firma http del metodo del controller
  }

  ottieniTramiteId(id: string): Observable<Sala> {
    // chiamamta http per ottenere una sala tramite il suo id
    return this.http.get<Sala>(`${this.baseUrl}/${id}`);
  }

  crea(payload: SalaCreazione): Observable<Sala> {
    // chiamata http per creare una nuova sala
    return this.http.post<Sala>(this.baseUrl, payload);
  }

  modifica(id: string, payload: SalaCreazione): Observable<Sala> {
    // chiamata http per la modifica di una sala
    return this.http.put<Sala>(`${this.baseUrl}/${id}`, payload);
  }

  elimina(id: string): Observable<void> {
    // chiamata http per l'eliminazione di una sala
```

</details>

<details>

<summary>sala.service.ts V1.2</summary>

Alessandro Gregorio

26/05/2026

Cambiato endpoint per eliminare una sala

```ts

import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { Sala, SalaCreazione } from '../models/sala.model';

// @Injectable({ providedIn: 'root' }) registra il servizio nel dependency injector
// globale dell'applicazione: esiste una sola istanza condivisa da tutti i componenti
// che lo iniettano (pattern Singleton)
@Injectable({ providedIn: 'root' })
export class SalaService {

  // HttpClient: servizio Angular per effettuare chiamate HTTP
  private readonly http = inject(HttpClient);

  // URL base per tutti gli endpoint di questo servizio,
  // costruito concatenando l'indirizzo API definito nell'environment
  // (cambia automaticamente tra sviluppo e produzione) con il path "/sala"
  private readonly baseUrl = `${environment.apiBaseUrl}/sala`;

  // ===========================
  // GET /sala
  // Recupera la lista completa di tutte le sale
  // ===========================
  ottieniTutto(): Observable<Sala[]> {
    return this.http.get<Sala[]>(this.baseUrl);
  }

  // ===========================
  // GET /sala/tipologia/:tipologiaId
  // Recupera solo le sale che appartengono a una specifica tipologia
  // utile per filtrare la lista in base al tipo di sala
  // ===========================
  ottieniPerTipologia(tipologiaId: string): Observable<Sala[]> {
    return this.http.get<Sala[]>(`${this.baseUrl}/tipologia/${tipologiaId}`);
  }

  // ===========================
  // GET /sala/:id
  // Recupera una singola sala tramite il suo ID univoco
  // ===========================
  ottieniTramiteId(id: string): Observable<Sala> {
    return this.http.get<Sala>(`${this.baseUrl}/${id}`);
  }

  // ===========================
  // POST /sala
  // Crea una nuova sala inviando i dati nel body della richiesta
  // responseType: 'text' indica che il backend risponde con una stringa
  // (es. l'ID della risorsa appena creata) invece di un oggetto JSON
  // ===========================
  crea(payload: SalaCreazione): Observable<string> {
    return this.http.post(`${this.baseUrl}`, payload, { responseType: 'text' });
  }

  // ===========================
  // PUT /sala/:id
  // Aggiorna una sala esistente identificata dall'ID
  // Il payload contiene i nuovi valori da sovrascrivere
  // responseType: 'text' come per crea(), il backend risponde con una stringa
  // ===========================
  modifica(id: string, payload: SalaCreazione): Observable<string> {
    return this.http.put(`${this.baseUrl}/${id}`, payload, { responseType: 'text' });
  }

  // ===========================
  // PUT /sala/elimina/:id
  // Esegue un soft delete della sala: usa PUT invece di DELETE
  // perché il backend marca il record come eliminato senza rimuoverlo fisicamente dal DB
  // Il body è null perché non servono dati aggiuntivi oltre all'ID nell'URL
  // ===========================
  elimina(id: string): Observable<void> {
    return this.http.put<void>(`${this.baseUrl}/elimina/${id}`, null);
  }
}
```

</details>


### gestione.service.ts
<details><summary>gestioneservice.ts V1.0 </summary>

- Utente: Simeone
- Data: 28/05/2026
- Descrizione: service per le funzionalità della pagina gestione per il ruolo gestore con i collegamenti ponti al backend

```ts
import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { LogAzioni, ContoCinema, Biglietto, GiftCard } from '../models/gestione.model'; // Adatta il percorso

@Injectable({ providedIn: 'root' })
export class GestioneService { 

  private readonly http = inject(HttpClient);
  private readonly gestoreUrl = `${environment.apiBaseUrl}/Gestore`;// Il gestoreUrl punta alla radice del controller Gestor
  private readonly giftCardUrl = `${environment.apiBaseUrl}/GiftCard`;


  ottieniLog(): Observable<LogAzioni[]> {
    return this.http.get<LogAzioni[]>(`${this.gestoreUrl}/logs`);
  }
 // da qua sotto TUTTE MODIFICHE ED AGGIUNTE
  ottieniConto(): Observable<ContoCinema> {
    return this.http.get<ContoCinema>(`${this.gestoreUrl}/conto`);
  }

  ottieniBiglietti(): Observable<Biglietto[]> {
    return this.http.get<Biglietto[]>(`${this.gestoreUrl}/biglietti`);
  }

  ottieniGiftCards(): Observable<GiftCard[]> {
    return this.http.get<GiftCard[]>(this.giftCardUrl); //è un get nudo e crudio 
  }
}

```

### tipologiaSalaService.ts
<details><summary>gestioneservice.ts V1.1 </summary>

- Utente: Simeone
- Data: 28/05/2026
- Descrizione: ora le giftcard sono ottenibili anche dall'utente, ma ovviamente solo quelle create da lui
```ts
import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { LogAzioni, ContoCinema, Biglietto, GiftCard } from '../models/gestione.model'; 

@Injectable({ providedIn: 'root' })
export class GestioneService { 

  private readonly http = inject(HttpClient);
  private readonly gestoreUrl = `${environment.apiBaseUrl}/Gestore`;
  private readonly giftCardUrl = `${environment.apiBaseUrl}/GiftCard`;


  ottieniLog(): Observable<LogAzioni[]> {
    return this.http.get<LogAzioni[]>(`${this.gestoreUrl}/logs`);
  }

  ottieniConto(): Observable<ContoCinema> {
    return this.http.get<ContoCinema>(`${this.gestoreUrl}/conto`);
  }

  ottieniBiglietti(): Observable<Biglietto[]> {
    return this.http.get<Biglietto[]>(`${this.gestoreUrl}/biglietti`);
  }

  ottieniGiftCards(): Observable<GiftCard[]> {
    return this.http.get<GiftCard[]>(this.giftCardUrl);
  }

  ottieniMieGiftCards(): Observable<GiftCard[]> {
    return this.http.get<GiftCard[]>(`${this.giftCardUrl}/mie`);
  }
}

```

</details>

### TipologiaSalaService.ts V1.0

<details><summary> tipologia.sala.service.ts V1.0 </summary>

```ts
import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { TipologiaSala, TipologiaSalaCreazione } from '../models/TipologiaSala.model';

/**
 * Service Angular dedicato alla gestione delle operazioni CRUD
 * relative all'entità TipologiaSala.
 *
 * Il service comunica con il backend tramite HttpClient
 * utilizzando gli endpoint REST esposti dall'API.
 */
@Injectable({
  providedIn: 'root',
})
export class TipologiaSalaService {
  /**
   * Istanza del client HTTP utilizzata per effettuare
   * richieste verso il backend.
   */
  private readonly http = inject(HttpClient);

  /**
   * URL base dell'endpoint REST per la gestione
   * delle tipologie di sala.
   */
  private readonly baseUrl = `${environment.apiBaseUrl}/tipologiaSala`;

  /**
   * Recupera tutte le tipologie di sala presenti nel sistema.
   *
   * @returns Observable contenente un array di TipologiaSala.
   */
  ottieniTutto(): Observable<TipologiaSala[]> {
    return this.http.get<TipologiaSala[]>(this.baseUrl);
  }

  /**
   * Recupera una specifica tipologia di sala tramite identificativo GUID.
   *
   * @param id Identificativo univoco della tipologia di sala.
   * @returns Observable contenente la TipologiaSala richiesta.
   */
  ottieniTramiteId(id: string): Observable<TipologiaSala> {
    return this.http.get<TipologiaSala>(`${this.baseUrl}/${id}`);
  }

  /**
   * Crea una nuova tipologia di sala nel sistema.
   *
   * @param payload Oggetto contenente i dati necessari alla creazione.
   * @returns Observable contenente la TipologiaSala creata.
   */
  crea(payload: TipologiaSalaCreazione): Observable<TipologiaSala> {
    return this.http.post<TipologiaSala>(this.baseUrl, payload);
  }

  /**
   * Modifica una tipologia di sala esistente.
   *
   * @param id Identificativo GUID della tipologia da modificare.
   * @param payload Nuovi dati della tipologia di sala.
   * @returns Observable contenente la TipologiaSala aggiornata.
   */
  modifica(id: string, payload: TipologiaSalaCreazione): Observable<TipologiaSala> {
    return this.http.put<TipologiaSala>(`${this.baseUrl}/${id}`, payload);
  }

  /**
   * Elimina una tipologia di sala tramite identificativo GUID.
   *
   * @param id Identificativo univoco della tipologia di sala.
   * @returns Observable<void> restituito al completamento dell'operazione.
   */
  elimina(id: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }
}

```
</details>

### TipologiaSalaService.ts V1.1

<details> 

  Utente: Alessandro Gregorio

  Data: 04/06/2026

  Dettagli: DELETE -> PUT. La delete è diventata una put per poter modificare il parametro isDeleted per far considerare la tipologia sala eliminata ma senza effettivamente eliminarla ed eliminare le sue correlazioni

<summary> tipologia.sala.service.ts V1.0 </summary>

```ts

import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { TipologiaSala, TipologiaSalaCreazione } from '../models/tipologia-sala.model';
import pa from '@angular/common/locales/pa';
/**
 * Service Angular dedicato alla gestione delle operazioni CRUD
 * relative all'entità TipologiaSala.
 *
 * Il service comunica con il backend tramite HttpClient
 * utilizzando gli endpoint REST esposti dall'API.
 */

@Injectable({
  providedIn: 'root',
})
export class TipologiaSalaService {
  /**
   * Istanza del client HTTP utilizzata per effettuare
   * richieste verso il backend.
   */
  private readonly http = inject(HttpClient);
    /**
   * URL base dell'endpoint REST per la gestione
   * delle tipologie di sala.
   */
  private readonly baseUrl = `${environment.apiBaseUrl}/tipologiaSala`;

 /**
   * Recupera tutte le tipologie di sala presenti nel sistema.
   *
   * @returns Observable contenente un array di TipologiaSala.
   */
  ottieniTutto(): Observable<TipologiaSala[]>
  {
    return this.http.get<TipologiaSala[]>(this.baseUrl);
  }
    /**
   * Recupera una specifica tipologia di sala tramite identificativo GUID.
   *
   * @param id Identificativo univoco della tipologia di sala.
   * @returns Observable contenente la TipologiaSala richiesta.
   */
  ottieniTramiteId(id:string): Observable<TipologiaSala>
  {
    return this.http.get<TipologiaSala>(`${this.baseUrl}/${id}`);
  }

 /**
   * Crea una nuova tipologia di sala nel sistema.
   *
   * @param payload Oggetto contenente i dati necessari alla creazione.
   * @returns Observable contenente la TipologiaSala creata.
   */
  crea(payload:TipologiaSalaCreazione): Observable<TipologiaSala>
  {
    return this.http.post<TipologiaSala>(this.baseUrl, payload);
  }
    /**
   * Modifica una tipologia di sala esistente.
   *
   * @param id Identificativo GUID della tipologia da modificare.
   * @param payload Nuovi dati della tipologia di sala.
   * @returns Observable contenente la TipologiaSala aggiornata.
   */

  modifica(id:string, payload: TipologiaSalaCreazione): Observable<TipologiaSala>
  {
    return this.http.put<TipologiaSala>(`${this.baseUrl}/${id}`, payload);
  }

  /**
   * Elimina una tipologia di sala tramite identificativo GUID.
   * Non lo elimina effettivamente dal database ma commuta il parametro isDeleted
   *
   * @param id Identificativo univoco della tipologia di sala.
   * @returns Observable<void> restituito al completamento dell'operazione.
   */
  elimina(id:string, payload: TipologiaSalaCreazione): Observable<TipologiaSala>
  {
    return this.http.put<TipologiaSala>(`${this.baseUrl}/elimina/${id}`, payload);
  }
}

```

</details>

### movieService
<details><summary> movie.service V1.0 </summary>

```ts
import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { Movie, MovieCreazione } from '../models/Movie.model';

@Injectable({
  providedIn: 'root',
})
export class MovieService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiBaseUrl}/movie`;

  ottieniTutto(): Observable<Movie[]> {
    return this.http.get<Movie[]>(this.baseUrl);
  }

  ottieniTramiteId(id: string): Observable<Movie> {
    return this.http.get<Movie>(`${this.baseUrl}/${id}`);
  }

  ottieniTramiteGenere(id: string): Observable<Movie[]> {
    return this.http.get<Movie[]>(`${this.baseUrl}/genere/${id}`);
  }

  crea(payload: MovieCreazione): Observable<Movie> {
    return this.http.post<Movie>(this.baseUrl, payload);
  }

  modifica(id: string, payload: MovieCreazione): Observable<Movie> {
    return this.http.put<Movie>(`${this.baseUrl}/${id}`, payload);
  }

  elimina(id: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }
}
```
</details>

### turno.service.ts
<details><summary> turno.service V1.0 </summary>

```ts
import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { Turno, TurnoCreazione } from '../models/turno.model';

/**
 * Service Angular dedicato alla gestione delle operazioni CRUD
 * relative all'entità Turno.
 *
 * Il service comunica con il backend tramite HttpClient
 * utilizzando gli endpoint REST esposti dall'API.
 */
@Injectable({ providedIn: 'root' })
export class TurnoService {
  /**
   * Istanza del client HTTP utilizzata per effettuare
   * richieste verso il backend.
   */
  private readonly http = inject(HttpClient);
  /**
   * URL base dell'endpoint REST per la gestione
   *  di turno.
   */
  private readonly baseUrl = `${environment.apiBaseUrl}/turno`;

  /**
   * Recupera tutti i turni presenti nel sistema.
   *
   * @returns Observable contenente un array di Turni.
   */
  ottieniTutto(): Observable<Turno[]> {
    return this.http.get<Turno[]>(this.baseUrl);
  }
  /**
   * Recupera uno specifico turno tramite identificativo GUID.
   *
   * @param id Identificativo univoco del turno.
   * @returns Observable contenente il turno di risposta.
   */
  ottieniTramiteId(id: string): Observable<Turno> {
    return this.http.get<Turno>(`${this.baseUrl}/${id}`);
  }
  /**
   * crea un nuovo turno utilizzando un payload.
   *
   * @param payload modello di richiesta per creazione del turno.
   * @returns Observable contenente il turno di risposta.
   */
  crea(payload: TurnoCreazione): Observable<Turno> {
    return this.http.post<Turno>(this.baseUrl, payload);
  }
  /**
   * Modifica uno specifico turno tramite identificativo GUID con le informazioni all'interno di un payload.
   *
   * @param id Identificativo univoco del turno.
   * @param payload modello di richiesta per la modifica del turno.
   * @returns Observable contenente il turno di risposta.
   */
  modifica(id: string, payload: TurnoCreazione): Observable<Turno> {
    return this.http.put<Turno>(`${this.baseUrl}/${id}`, payload);
  }

  /**
   * Elimina uno specifico turno tramite identificativo GUID.
   *
   * @param id Identificativo univoco del turno.
   */
  elimina(id: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }
}
```
</details>

### abbonamento.service.ts
<details><summary> abbonamento.service V1.0 </summary>

```ts
/**
 * Service Angular dedicato alla gestione delle operazioni CRUD
 * relative all'entità abboanamento.
 *
 * Il service comunica con il backend tramite HttpClient
 * utilizzando gli endpoint REST esposti dall'API.
 */
import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { Abbonamento, AbbonamentoCreazione } from '../models/abbonamento.model';

@Injectable({ providedIn: 'root' })
export class AbbonamentoService {
  /**
   * Istanza del client HTTP utilizzata per effettuare
   * richieste verso il backend.
   */
  private readonly http = inject(HttpClient);
  /**
   * URL base dell'endpoint REST per la gestione
   *  di abbonamento.
   */
  private readonly baseUrl = `${environment.apiBaseUrl}/abbonamento`;
  /**
   * Recupera tutti gli abboanamenti presenti nel sistema.
   *
   * @returns Observable contenente un array di abboanamenti.
   */
  ottieniTutto(): Observable<Abbonamento[]> {
    return this.http.get<Abbonamento[]>(this.baseUrl);
  }
  /**
   * Recupera uno specifico abboanamento tramite identificativo GUID.
   *
   * @param id Identificativo univoco dell' abboanamento.
   * @returns Observable contenente l' abboanamento di risposta.
   */
  ottieniTramiteId(id: string): Observable<Abbonamento> {
    return this.http.get<Abbonamento>(`${this.baseUrl}/${id}`);
  }
  /**
   * crea un nuovo abboanamento utilizzando un payload.
   *
   * @param payload modello di richiesta per creazione dell' abboanamento.
   * @returns Observable contenente l' abboanamento di risposta.
   */
  crea(payload: AbbonamentoCreazione): Observable<Abbonamento> {
    return this.http.post<Abbonamento>(this.baseUrl, payload);
  }
  /**
   * Modifica uno specifico abboanamento tramite identificativo GUID con le informazioni all'interno di un payload.
   *
   * @param id Identificativo univoco dell abboanamento.
   * @param payload modello di richiesta per la modifica dell' abboanamento.
   * @returns Observable contenente l' abboanamento di risposta.
   */
  modifica(id: string, payload: AbbonamentoCreazione): Observable<Abbonamento> {
    return this.http.put<Abbonamento>(`${this.baseUrl}/${id}`, payload);
  }
  /**
   * Elimina uno specifico abbonamento tramite identificativo GUID.
   *
   * @param id Identificativo univoco dell' abbonamento.
   */
  elimina(id: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }
}
```
</details>

### giftcard.service.ts
<details><summary> giftcard.service V1.0 </summary>

```ts
import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { GiftCard, GiftCardCreazione } from '../models/giftCard.model';
/**
 * Service Angular dedicato alla gestione delle operazioni CRUD
 * relative all'entità giftcard.
 *
 * Il service comunica con il backend tramite HttpClient
 * utilizzando gli endpoint REST esposti dall'API.
 */
@Injectable({
  providedIn: 'root',
})
export class GiftCardService {
  /**
   * Istanza del client HTTP utilizzata per effettuare
   * richieste verso il backend.
   */
  private readonly http = inject(HttpClient);
  /**
   * URL base dell'endpoint REST per la gestione
   * delle tipologie di sala.
   */
  private readonly baseUrl = `${environment.apiBaseUrl}/giftcard`;

  /**
   * Recupera tutte le giftcard presenti nel sistema.
   *
   * @returns Observable contenente un array di giftcard.
   */
  ottieniTutto(): Observable<GiftCard[]> {
    return this.http.get<GiftCard[]>(this.baseUrl);
  }

  /**
   * Recupera una specifica giftcard tramite identificativo GUID.
   *
   * @param id Identificativo univoco della giftcard.
   * @returns Observable contenente la giftcard di risposta.
   */
  ottieniTramiteId(id: number): Observable<GiftCard> {
    return this.http.get<GiftCard>(`${this.baseUrl}/${id}`);
  }

  /**
   * crea un nuova giftcard utilizzando un payload.
   *
   * @param payload modello di richiesta per creazione della giftcard.
   * @returns Observable contenente la giftcard di risposta.
   */
  crea(payload: GiftCardCreazione): Observable<GiftCard> {
    return this.http.post<GiftCard>(this.baseUrl, payload);
  }

  /**
   * Modifica una specifica giftcard tramite identificativo GUID con le informazioni all'interno di un payload.
   *
   * @param id Identificativo univoco della giftcard.
   * @param payload modello di richiesta per la modifica della giftcard.
   * @returns Observable contenente la giftcard di risposta.
   */
  modifica(id: number, payload: GiftCardCreazione): Observable<GiftCard> {
    return this.http.put<GiftCard>(`${this.baseUrl}/${id}`, payload);
  }
  /**
   * Elimina una specifica giftcard tramite identificativo GUID.
   *
   * @param id Identificativo univoco della giftcard.
   */
  elimina(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }
}
```
</details>


<details><summary>Versione 1.1</summary>

- Utente: Simeone
- Data: 05/06/2026
```ts
import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { GiftCard, GiftCardCreazione } from '../models/giftCard.model';

@Injectable({
    providedIn: 'root',
})

export class GiftCardService {
    private readonly http = inject(HttpClient);
    private readonly baseUrl = `${environment.apiBaseUrl}/giftcard`;
    private readonly utenteUrl = `${environment.apiBaseUrl}/Utente/giftCard`;
    // verifica attentamente se il codice qua sopra è nelle giuste rotte backend

    ottieniTutto(): Observable<GiftCard[]> {
        return this.http.get<GiftCard[]>(this.baseUrl);
    }

    ottieniTramiteId(id: String): Observable<GiftCard> {
        return this.http.get<GiftCard>(`${this.baseUrl}/${id}`); //prima c'era scritto id number, ERRORE
    }

    crea(payload: GiftCardCreazione): Observable<GiftCard> {
        return this.http.post<GiftCard>(this.baseUrl, payload);
    }

    modifica(id: string, payload: GiftCardCreazione): Observable<GiftCard> {
        return this.http.put<GiftCard>(`${this.baseUrl}/${id}`, payload);
    }

    elimina(id: string): Observable<void> {
        return this.http.delete<void>(`${this.baseUrl}/${id}`);
    }

    //AGGIUNTEVMIE

    // Ricarica si aspetta un DtoRicaricaGiftCard dal backend, che ha solo l'Importo
    ricarica(importo: number): Observable<{messaggio: string}> {
        return this.http.put<{messaggio: string}>(`${this.utenteUrl}/ricarica`, { importo: importo });
    }

    // Riscatto si aspetta un DtoCodiceRiscatto
    riscatta(codiceRiscatto: string): Observable<any> {
        return this.http.post<any>(`${this.utenteUrl}/riscatta`, { codiceRiscatto: codiceRiscatto });
    }
}
```

</details>
 

### genere-movie.service.ts
<details><summary> genere-movie.service V1.0 </summary>

```ts
import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { GenereMovie, GenereMovieCreazione } from '../models/genere-movie.model';
/**
 * Service Angular dedicato alla gestione delle operazioni CRUD
 * relative all'entità generemovie.
 *
 * Il service comunica con il backend tramite HttpClient
 * utilizzando gli endpoint REST esposti dall'API.
 */
@Injectable({
  providedIn: 'root',
})
export class GenereMovieService {
  /**
   * Istanza del client HTTP utilizzata per effettuare
   * richieste verso il backend.
   */
  private readonly http = inject(HttpClient);
  /**
   * URL base dell'endpoint REST per la gestione
   * delle tipologie di sala.
   */
  private readonly baseUrl = `${environment.apiBaseUrl}/generemovie`;

  /**
   * Recupera tutti i generimovie presenti nel sistema.
   *
   * @returns Observable contenente un array di generimovie.
   */
  ottieniTutto(): Observable<GenereMovie[]> {
    return this.http.get<GenereMovie[]>(this.baseUrl);
  }

  /**
   * Recupera uno specifico generemovie tramite identificativo GUID.
   *
   * @param id Identificativo univoco del generemovie.
   * @returns Observable contenente il generemovie di risposta.
   */
  ottieniTramiteId(id: number): Observable<GenereMovie> {
    return this.http.get<GenereMovie>(`${this.baseUrl}/${id}`);
  }

  /**
   * crea un nuovo generemovie utilizzando un payload.
   *
   * @param payload modello di richiesta per creazione del generemovie.
   * @returns Observable contenente il generemovie di risposta.
   */
  crea(payload: GenereMovieCreazione): Observable<GenereMovie> {
    return this.http.post<GenereMovie>(this.baseUrl, payload);
  }

  /**
   * Modifica uno specifico generemovie tramite identificativo GUID con le informazioni all'interno di un payload.
   *
   * @param id Identificativo univoco del generemovie.
   * @param payload modello di richiesta per la modifica del generemovie.
   * @returns Observable contenente il generemovie di risposta.
   */
  modifica(id: number, payload: GenereMovieCreazione): Observable<GenereMovie> {
    return this.http.put<GenereMovie>(`${this.baseUrl}/${id}`, payload);
  }

  /**
   * Elimina uno specifico generemovie tramite identificativo GUID.
   *
   * @param id Identificativo univoco del generemovie.
   */
  elimina(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }
}
```
</details>

Utente: Marco Strazzeri
Data: 25/05/2026
Descrizione: Modificato per gestire il solo caricamento della lista dei generi

<details><summary> genere-movie.service.ts V1.1</summary>

```ts
//controllare gli import. nomi classi e file sono differenti per "-"
import { Component, inject, signal } from '@angular/core';
//import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { AuthService } from '../../services/auth.service';
import { GenereMovieService } from '../../services/genere-movie.service';
import { GenereMovie } from '../../models/genere-movie.model';

@Component({
  selector: 'app-genere-movie',
  imports: [],
  templateUrl: './genere-movie.page.html',
  styleUrl: './genere-movie.page.css',
})
export class GenereMoviePage {
  private readonly authService = inject(AuthService);
  private readonly genereMovieService = inject(GenereMovieService);

  readonly generiMovies = signal<GenereMovie[]>([]);
  readonly isLoading = signal(false);
  readonly errorMessage = signal('');
  readonly successMessage = signal('');

  constructor() {
    this.loadGeneriMovies();
  }

  //carica la lista dei generi dal backend
  loadGeneriMovies(): void {
    this.isLoading.set(true);
    this.errorMessage.set('');

    //chiamata al servizio ottienitutto per ottenere la lista dei generi dei film
    this.genereMovieService.ottieniTutto().subscribe({
      next: (items) => {
        this.generiMovies.set(items);
        this.isLoading.set(false);
      },
      error: (error: unknown) => {
        this.isLoading.set(false);
        this.errorMessage.set(
          this.extractErrorMessage(error, 'Impossibile caricare i generi dei film'),
        );
      },
    });
  }

  trackById(_: string, item: GenereMovie): string | null {
    return item.id;
  }

  private extractErrorMessage(error: unknown, fallback: string): string {
    if (error instanceof Error) {
      return error?.message ?? fallback;
    }
    return fallback;
  }
}
```
</details>

### auth.service.ts
<details><summary> auth.service V1.0 </summary>

```ts
import { environment } from '../../environments/environment';
import { inject, Injectable, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { tap } from 'rxjs/operators';
import { Observable } from 'rxjs';
import { Auth } from '../models/auth.model';
import { Login } from '../models/login.model';
import { Registrazione } from '../models/registrazione.model';
import { SessioneUtente } from '../models/sessione-utente.model';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private readonly http = inject(HttpClient);
  private readonly router = inject(Router);
  private readonly storagekey = 'nuovo_cinema_paradiso_auth';

  readonly utenteCorrente = signal<SessioneUtente | null>(this.readStoredUser());

  login(payload: Login): Observable<Auth> {
    return this.http
      .post<Auth>(`${environment.apiBaseUrl}/Auth/login`, payload)
      .pipe(tap((response) => this.setSession(response)));
  }

  registrazione(payload: Registrazione): Observable<{ message: string }> {
    return this.http.post<{ message: string }>(
      `${environment.apiBaseUrl}/Auth/registrazione`,
      payload,
    );
  }

  logout(): void {
    localStorage.removeItem(this.storagekey);
    this.utenteCorrente.set(null);
    void this.router.navigate(['/login']);
  }

  isAutenticato(): boolean {
    return this.utenteCorrente() !== null;
  }

  possiedeQualsiasiRuolo(ruoli: string[]): boolean {
    const ruolo = this.utenteCorrente()?.ruolo ?? '';
    return ruoli.includes(ruolo);
  }

  ruoloCorrispondente(ruolo: string): boolean {
    return this.utenteCorrente()?.ruolo === ruolo;
  }

  ottieniToken(): string | null {
    return this.utenteCorrente()?.token ?? null;
  }

  private setSession(risposta: Auth): void {
    const utenteInSessione: SessioneUtente = {
      token: risposta.token,
      id: risposta.id,
      email: risposta.email,
      nomeCompleto: risposta.nomeCompleto,
      ruolo: risposta.ruolo,
    };

    localStorage.setItem(this.storagekey, JSON.stringify(utenteInSessione));
    this.utenteCorrente.set(utenteInSessione);
  }

  private readStoredUser(): SessioneUtente | null {
    const raw = localStorage.getItem(this.storagekey);

    if (!raw) {
      return null;
    }

    try {
      return JSON.parse(raw) as SessioneUtente;
    } catch {
      localStorage.removeItem(this.storagekey);
      return null;
    }
  }
}
```
</details>

### log-azioni.service.ts
<details><summary> log-azioni.service V1.0 </summary>

```ts
import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { LogAzione, LogAzioneCreazione } from '../models/log-azioni.model';

/**
 * Service Angular dedicato alla gestione dei log delle azioni.
 *
 * Questo service ha lo scopo di replicare il comportamento del
 * LogAzioniService backend, mantenendo coerenza architetturale:
 * - metodi espliciti
 * - nessuna logica nascosta
 * - nessuna trasformazione automatica
 * - nessun interceptor o pipe complesso
 *
 * Il service si limita a:
 * 1. inviare richieste HTTP al backend
 * 2. restituire i dati così come arrivano
 * 3. mantenere un'interfaccia chiara e leggibile
 */
@Injectable({ providedIn: 'root' })
export class LogAzioniService {
  /**
   * Istanza di HttpClient ottenuta tramite dependency injection.
   * Viene utilizzata per comunicare con il backend.
   */
  private readonly http = inject(HttpClient);

  /**
   * URL base dell'endpoint REST dedicato ai log.
   *
   * Nota: il backend espone presumibilmente un controller come:
   * [HttpPost("/logazioni")] e [HttpGet("/logazioni")]
   */
  private readonly baseUrl = `${environment.apiBaseUrl}/logazioni`;

  /**
   * Salva un nuovo log nel backend.
   *
   * Questo metodo corrisponde al metodo backend:
   *   SalvataggioLogAzioneAsync(string? idUtente, string azione, bool effettuato)
   *
   * Il payload Angular deve contenere:
   * - idUtente: string | null
   * - azione: string
   * - effettuato: boolean
   *
   * Il backend genera automaticamente:
   * - messaggio ("operazione eseguita" / "operazione fallita")
   * - timestamp
   * - id del log
   *
   * @param payload modello contenente i dati necessari alla creazione del log
   * @returns Observable<void> perché il backend non restituisce un DTO
   */
  salva(payload: LogAzioneCreazione): Observable<void> {
    return this.http.post<void>(this.baseUrl, payload);
  }

  /**
   * Recupera tutti i log presenti nel sistema.
   *
   * Questo metodo corrisponde al backend:
   *   LetturaLogAzioneAsync()
   *
   * Il backend restituisce una lista di DtoLogAzioni,
   * che in Angular viene mappata nell'interfaccia LogAzione.
   *
   * @returns Observable contenente un array di log
   */
  ottieniTutti(): Observable<LogAzione[]> {
    return this.http.get<LogAzione[]>(this.baseUrl);
  }
}
```
</details>

### log-azioni.service.ts

- Utente: Simeone
- Data: 26/05/2026
- Ho eseguito la task di crearci la pagina dedicata
<details><summary> log-azioni.service V1.1 </summary>

```ts
import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { LogAzioni, LogAzioniCreazione } from '../models/logAzioni.model'; //ho modificato il nome perché il modello non corrispondeva

@Injectable({ providedIn: 'root' })
export class LogAzioniService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiBaseUrl}/gestore/logs`; //ho cambiato l'indirizzo prendendo spunto dagli altri url che hanno funzioni esclusivi che sono solitamente preceduti da {nome ruolo}7 e la funzione, quindi essendo i log di sistema una funzione disponibile solo per gestore ho utilizzato questo url

  ottieniTutti(): Observable<LogAzioni[]> {
    return this.http.get<LogAzioni[]>(this.baseUrl);
  }
}
```
</details>

### proiezione.service.ts

<details>
<summary>Versione 1.0</summary>

```ts
import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { Proiezione, ProiezioneCreazione } from '../models/proiezione.model';
/**
 * Service Angular dedicato alla gestione delle operazioni CRUD
 * relative all'entità proiezione.
 *
 * Il service comunica con il backend tramite HttpClient
 * utilizzando gli endpoint REST esposti dall'API.
 */
@Injectable({ providedIn: 'root' })
export class ProiezioneService {
  /**
   * URL base dell'endpoint REST per la gestione
   * delle proiezioni.
   */
  private readonly http = inject(HttpClient);
  /**
   * URL base dell'endpoint REST per la gestione
   *  della proiezione.
   */
  private readonly baseUrl = `${environment.apiBaseUrl}/proiezione`;

  /**
   * Recupera tutte le proiezioni visibili presenti nel sistema.
   *
   * @returns Observable contenente un array di proiezioni.
   */
  ottieniTutto(): Observable<Proiezione[]> {
    return this.http.get<Proiezione[]>(this.baseUrl);
  }

  /**
   * Recupera tutte le proiezioni presenti nel sistema anche quelle nascoste.
   *
   * @returns Observable contenente un array di proiezioni.
   */
  ottieniTuttoStorico(): Observable<Proiezione[]> {
    return this.http.get<Proiezione[]>(`${this.baseUrl}/storico`);
  }

  /**
   * Recupera una specifica proiezione tramite identificativo GUID.
   *
   * @param id Identificativo univoco della proiezione.
   * @returns Observable contenente la proiezione di risposta.
   */
  ottieniTramiteId(id: string): Observable<Proiezione> {
    return this.http.get<Proiezione>(`${this.baseUrl}/${id}`);
  }

  /**
   * Recupera una specifica proiezione tramite identificativo del movie(film) collegato GUID.
   *
   * @param id Identificativo univoco del movie.
   * @returns Observable contenente la proiezione di risposta.
   */
  ottieniTramiteMovieId(id: string): Observable<Proiezione> {
    return this.http.get<Proiezione>(`${this.baseUrl}/movie/${id}`);
  }

  /**
   * Recupera una specifica proiezione tramite identificativo della sala collegata GUID.
   *
   * @param id Identificativo univoco della sala.
   * @returns Observable contenente la proiezione di risposta.
   */
  ottieniTramiteSalaId(id: string): Observable<Proiezione> {
    return this.http.get<Proiezione>(`${this.baseUrl}/sala/${id}`);
  }

  /**
   * Recupera una specifica proiezione tramite identificativo del turno collegato GUID.
   *
   * @param id Identificativo univoco del turno.
   * @returns Observable contenente la proiezione di risposta.
   */
  ottieniTramiteTurnoId(id: string): Observable<Proiezione> {
    return this.http.get<Proiezione>(`${this.baseUrl}/turno/${id}`);
  }

  /**
   * crea una nuova proiezione utilizzando un payload.
   *
   * @param payload modello di richiesta per creazione della proiezione.
   * @returns Observable contenente la proiezione di risposta.
   */
  crea(payload: ProiezioneCreazione): Observable<Proiezione> {
    return this.http.post<Proiezione>(this.baseUrl, payload);
  }

  /**
   * Modifica una specifica proiezione tramite identificativo GUID con le informazioni all'interno di un payload.
   *
   * @param id Identificativo univoco della proiezione.
   * @param payload modello di richiesta per la modifica della proiezione.
   * @returns Observable contenente la proiezione di risposta.
   */
  modifica(id: string, payload: ProiezioneCreazione): Observable<Proiezione> {
    return this.http.put<Proiezione>(`${this.baseUrl}/${id}`, payload);
  }

  /**
   * Elimina una specifica proiezione tramite identificativo GUID.
   *
   * @param id Identificativo univoco della proiezione.
   */
  elimina(id: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }
}
```
</details>


Utente: Fabio Tammaro
Data: 03/06/2026
Descrizione: modificata la eliminazione in "put".
<details>
<summary>Versione 1.1</summary>

```ts
import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { Proiezione, ProiezioneCreazione } from '../models/proiezione.model';

@Injectable({ providedIn: 'root' })
export class ProiezioneService {
  
  private readonly http = inject(HttpClient);
 
  private readonly baseUrl = `${environment.apiBaseUrl}/proiezione`;

  ottieniTutto(): Observable<Proiezione[]> {
    return this.http.get<Proiezione[]>(this.baseUrl);
  }

  ottieniTuttoStorico(): Observable<Proiezione[]> {
    return this.http.get<Proiezione[]>(`${this.baseUrl}/storico`);
  }

  ottieniTramiteId(id: string): Observable<Proiezione> {
    return this.http.get<Proiezione>(`${this.baseUrl}/${id}`);
  }

  ottieniTramiteMovieId(id: string): Observable<Proiezione> {
    return this.http.get<Proiezione>(`${this.baseUrl}/movie/${id}`);
  }

  
  ottieniTramiteSalaId(id: string): Observable<Proiezione> {
    return this.http.get<Proiezione>(`${this.baseUrl}/sala/${id}`);
  }

  ottieniTramiteTurnoId(id: string): Observable<Proiezione> {
    return this.http.get<Proiezione>(`${this.baseUrl}/turno/${id}`);
  }

 
  crea(payload: ProiezioneCreazione): Observable<Proiezione> {
    return this.http.post<Proiezione>(this.baseUrl, payload);
  }


  modifica(id: string, payload: ProiezioneCreazione): Observable<Proiezione> {
    return this.http.put<Proiezione>(`${this.baseUrl}/${id}`, payload);
  }

// è una put perchè non può eliminare la proiezione direttamente.
  elimina(id: string) {
        return this.http.put<{ messaggio: string }>(`${this.baseUrl}/elimina/${id}`,
            {}
        );
}
```
</details>

### biglietto.service
<details><summary> biglietto.service V1.0 </summary>

```ts
import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { Biglietto, BigliettoCreazione } from '../models/biglietto.model';

@Injectable({
  providedIn: 'root',
})
export class BigliettoService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiBaseUrl}/biglietto`;

  ottieniTutto(utenteId: string): Observable<Biglietto[]> {
    return this.http.get<Biglietto[]>(`${this.baseUrl}/utente/${utenteId}`);
  }

  ottieniTramiteId(id: string, utenteId: string): Observable<Biglietto> {
    return this.http.get<Biglietto>(`${this.baseUrl}/${id}/utente/${utenteId}`);
  }
  
  // la creazione, corrispondente all'acquisto deve corrispondere al backend che ha come risposta una stringa di conferma o di insuccesso.
  crea(payload: BigliettoCreazione): Observable<any> {
    return this.http.post<any>(this.baseUrl, payload);  
  }

  modifica(id: string, payload: BigliettoCreazione): Observable<Biglietto> {
    return this.http.put<Biglietto>(`${this.baseUrl}/${id}`, payload);
  }

  elimina(id: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }
}

```
</details>

<details><summary> biglietto.service V1.1 </summary>

Francesco Lorenzi 10/06/2026

ho dovuto modificare la richiesta di creazione, che accettava solo un file json come risposta osservabile, tuttavia il backend restituiva una stringa messaggio dando così un errore anche se l'azione avveniva correttamente
```ts
import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { Biglietto, BigliettoCreazione } from '../models/biglietto.model';

@Injectable({
  providedIn: 'root',
})
export class BigliettoService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiBaseUrl}/biglietto`;

  ottieniTutto(utenteId: string): Observable<Biglietto[]> {
    return this.http.get<Biglietto[]>(`${this.baseUrl}/utente/${utenteId}`);
  }

  ottieniTramiteId(id: string, utenteId: string): Observable<Biglietto> {
    return this.http.get<Biglietto>(`${this.baseUrl}/${id}/utente/${utenteId}`);
  }
  //ora l'osservabile è una stringa e non un "any" che accettava qualsiasi tipo di JSON
  crea(payload: BigliettoCreazione): Observable<string> {
  return this.http.post(this.baseUrl, payload, { responseType: 'text' });
  }

  modifica(id: string, payload: BigliettoCreazione): Observable<Biglietto> {
    return this.http.put<Biglietto>(`${this.baseUrl}/${id}`, payload);
  }

  elimina(id: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }
}

```
</details>

### GestoreService
<details><summary> gestore.service V1.0 </summary>

```ts
import { environment } from '../../environments/environment';
import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Biglietto } from '../models/biglietto.model';
import { Utente, UtenteModificaRuolo } from '../models/utente.model';
import { LogAzioni } from '../models/logAzioni.model';

@Injectable({
  providedIn: 'root',
})
export class GestoreService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiBaseUrl}/gestore`;

  // GET tutti utenti
  OttieniUtenti(): Observable<Utente[]> {
    return this.http.get<Utente[]>(`${this.baseUrl}/utenti`);
  }

  // GET utente per id
  OttieniUtentePerId(id: string): Observable<Utente> {
    return this.http.get<Utente>(`${this.baseUrl}/utenti/${id}`);
  }

  // DELETE utente
  eliminaUtente(id: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/utenti/${id}`);
  }

  // GET utenti per abbonamento
  ottieniUtentiTramiteAbbonamento(abbonamentoId: string): Observable<Utente[]> {
    return this.http.get<Utente[]>(`${this.baseUrl}/utenti/abbonamento/${abbonamentoId}`);
  }

  // GET utenti per giftcard
  ottieniUtentiTramiteGiftCard(giftCardId: string): Observable<Utente[]> {
    return this.http.get<Utente[]>(`${this.baseUrl}/utenti/giftcard/${giftCardId}`);
  }

  // GET tutti biglietti
  ottieniBiglietti(): Observable<Biglietto[]> {
    return this.http.get<Biglietto[]>(`${this.baseUrl}/biglietti`);
  }

  // GET biglietto per id
  ottieniBigliettoPerId(id: string): Observable<Biglietto> {
    return this.http.get<Biglietto>(`${this.baseUrl}/biglietti/${id}`);
  }

  ottieniLogAzioni(): Observable<LogAzioni[]> {
    return this.http.get<LogAzioni[]>(`${this.baseUrl}/log`);
  }
}
```
</details>

### gestione-utente.service.ts
<details><summary> gestione-utente.service V1.0 </summary>

```ts
import { environment } from '../../environments/environment';
import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { UtenteModificaRuolo } from '../models/utente.model';

@Injectable({
  providedIn: 'root',
})
export class GestioneUtenteService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiBaseUrl}/gestione-utente`;

  modificaRuolo(id: string, payload: UtenteModificaRuolo): Observable<string> {
    return this.http.put<string>(`${this.baseUrl}/${id}`, payload);
  }
}
```
</details>

### gestione-utente.service.ts
<details>
<details><summary> V1.1 </summary>
Utente: Andrea Paris
Data: 28/05/2026
Descrizione: apportati cambiamenti al metodo modificaRuolo

```ts
import { environment } from '../../environments/environment';
import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import {UtenteModificaRuoloRichiesta, UtenteModificaRuoloRisposta } from '../models/utente.model';


@Injectable({
    providedIn: 'root'
})
export class GestioneUtenteService {
    private readonly http = inject(HttpClient);
    private readonly baseUrl = `${environment.apiBaseUrl}/RuoloUtenti/cambia-ruolo`;


    modificaRuolo(payload: UtenteModificaRuoloRichiesta): Observable<UtenteModificaRuoloRisposta> {
        return this.http.put<UtenteModificaRuoloRisposta>(`${this.baseUrl}`, payload);
    }
}

```
</details>


### auth.service.ts

Utente: Fabio Tammaro
Data: 28/05/2026
Descrizione: Aggiunto il metodo isGestore per discriminare le azioni che tale ruolo permette.
<details><summary> auth-service.ts V1.0</summary>

``` typescript
import { environment } from '../../environments/environment';
import { inject, Injectable, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { tap } from 'rxjs/operators';
import { Observable } from 'rxjs';
import { Login } from '../models/login.model';
import { Registrazione } from '../models/registrazione.model';
import { SessioneUtente } from '../models/sessione-utente.model';
import { RuoliUtente } from '../ruoliUtente';

@Injectable({
  providedIn: 'root', 
})
export class AuthService {

  // inject() è il nuovo approccio Angular moderno alla dependency injection
  private readonly http = inject(HttpClient);

  // Router serve per effettuare redirect programmatici
  private readonly router = inject(Router);

  // chiave usata nel localStorage
  private readonly storagekey = 'nuovo_cinema_paradiso_auth';

  // url base del controller backend
  private readonly baseUrl = `${environment.apiBaseUrl}/Auth`;

  /*
    signal() crea uno stato reattivo Angular.

    utenteCorrente() contiene:
    - null se nessuno è loggato
    - SessioneUtente se autenticato

    readStoredUser() prova a recuperare l'utente salvato nel localStorage
    quando l'app viene ricaricata.
  */
  readonly utenteCorrente = signal<SessioneUtente | null>(this.readStoredUser());

  /*
    login():
    - invia email/password al backend
    - riceve il DTO SessioneUtente
    - salva la sessione tramite setSession()
  */
  login(payload: Login): Observable<SessioneUtente> {

    return this.http
      .post<SessioneUtente>(`${this.baseUrl}/login`, payload)

      /*
        tap() esegue effetti collaterali SENZA modificare la response.
        Qui viene usato per salvare la sessione dopo il login.
      */
      .pipe(tap((response) => this.setSession(response)));
  }

  registrazione(payload: Registrazione): Observable<{ message: string }> {

    return this.http.post<{ message: string }>(
      `${this.baseUrl}/registrazione`,
      payload
    );
  }

  /*
    logout():
    - rimuove sessione dal localStorage
    - resetta il signal
    - redirect al login
  */
  logout(): void {

    localStorage.removeItem(this.storagekey);

    // aggiorna immediatamente tutta la UI reattiva
    this.utenteCorrente.set(null);

    void this.router.navigate(['/login']);
  }

  
  isAutenticato(): boolean {
    return this.utenteCorrente() !== null;
  }

  isGestore(): boolean {
    return this.utenteCorrente()?.ruolo === 'Gestore';
  }

  /*
    verifica se il ruolo dell'utente è incluso
    nell'array di ruoli consentiti
  */
  possiedeQualsiasiRuolo(ruoli: RuoliUtente[]): boolean {

    const ruolo = this.ottieniRuoloUtente();

    return !!ruolo && ruoli.includes(ruolo);
  }

  /*
    verifica un ruolo specifico
  */
  ruoloCorrispondente(ruolo: string): boolean {

    return this.utenteCorrente()?.ruolo === ruolo;
  }

  ottieniToken(): string | null {

    return this.utenteCorrente()?.token ?? null;
  }

  ottieniRuoloUtente(): RuoliUtente | null {

    return this.utenteCorrente()?.ruolo ?? null;
  }

  /*
    setSession():
    salva i dati utente dopo login.
  */
  private setSession(risposta: SessioneUtente): void {

    const utenteInSessione: SessioneUtente = {

      id: risposta.id,
      nomeCompleto: risposta.nomeCompleto,
      token: risposta.token,
      eta: risposta.eta,
      email: risposta.email,
      ruolo: risposta.ruolo,
      dataInizioAbbonamento: risposta.dataInizioAbbonamento,
      dataInizioGiftCard: risposta.dataInizioGiftCard,
      seAbbonato: risposta.seAbbonato,
      possiedeGiftCard: risposta.possiedeGiftCard
    }

    /*
      salvataggio persistente browser

      JSON.stringify() converte oggetto -> stringa JSON
    */
    localStorage.setItem(
      this.storagekey,
      JSON.stringify(utenteInSessione)
    );

    /*
      aggiorna signal reattivo Angular

      tutti i componenti che usano utenteCorrente()
      si aggiornano automaticamente.
    */
    this.utenteCorrente.set(utenteInSessione);
  }

  /*
    readStoredUser():
    recupera utente dal localStorage all'avvio app.
  */
  private readStoredUser(): SessioneUtente | null {

    const raw = localStorage.getItem(this.storagekey);

    if (!raw) {
      return null;
    }

    try {

      /*
        JSON.parse():
        converte stringa JSON -> oggetto JS
      */
      return JSON.parse(raw) as SessioneUtente;

    } catch {

      /*
        se JSON è corrotto:
        - rimuove dati invalidi
        - evita crash applicazione
      */
      localStorage.removeItem(this.storagekey);

      return null;
    }
  }
}
```

### auth.service.ts

Utente: Marco Strazzeri
Data: 28/05/2026
Descrizione: Fixata la funzione ottieniRuoloUtente, prima dava problemi con la lettura del ruolo

<details><summary> auth-service.ts V1.1</summary>

```ts

import { environment } from '../../environments/environment';
import { inject, Injectable,signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { tap } from 'rxjs/operators';
import { Observable } from 'rxjs';
import { Login } from '../models/login.model';
import { Registrazione } from '../models/registrazione.model';
import { SessioneUtente } from '../models/sessione-utente.model';
import { RuoliUtente } from '../ruoliUtente';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private readonly http = inject(HttpClient);
  private readonly router = inject(Router);
  private readonly storagekey = 'nuovo_cinema_paradiso_auth';
  private readonly baseUrl = `${environment.apiBaseUrl}/Auth`;

  readonly utenteCorrente = signal<SessioneUtente | null>(this.readStoredUser());

  login(payload: Login): Observable<SessioneUtente> {

    return this.http
      .post<SessioneUtente>(`${this.baseUrl}/login`, payload)
      .pipe(tap((response) => this.setSession(response)));
  }

  registrazione(payload: Registrazione): Observable<{ message: string }> {
    return this.http.post<{ message: string }>(`${this.baseUrl}/registrazione`, payload);

  }

  logout(): void {
    localStorage.removeItem(this.storagekey);
    this.utenteCorrente.set(null);
    void this.router.navigate(['/login']);
  }

  isAutenticato(): boolean {
    return this.utenteCorrente() !== null;
  }

 isGestore(): boolean {
  return this.utenteCorrente()?.ruolo === 'Gestore';
 }
  possiedeQualsiasiRuolo(ruoli: RuoliUtente[]): boolean {
    const ruolo = this.ottieniRuoloUtente();
    return !!ruolo && ruoli.includes(ruolo);
  }

  ruoloCorrispondente(ruolo: string): boolean {
    return this.utenteCorrente()?.ruolo === ruolo;

  }

  ottieniToken(): string | null {
    return this.utenteCorrente()?.token ?? null;
  }
  
/*
  ottieniRuoloUtente(): RuoliUtente | null{
    const ruolo = localStorage.getItem('ruolo');
    console.log('Ruolo ottenuto dal localStorage:', ruolo); // Debug log
    if(ruolo === 'Operatore' || ruolo === 'Gestore' || ruolo === 'Utente')
    {
      return ruolo;
    }
    return null;
  }  */



  ottieniRuoloUtente(): RuoliUtente | null {
  const raw = localStorage.getItem(this.storagekey);  //legge i dati dal localstorage

    if (!raw) {
      return null;
    }

    try {
      const utente = JSON.parse(raw) as SessioneUtente; //lettura del json

      if (
        utente.ruolo === 'Operatore' ||
        utente.ruolo === 'Gestore' ||
        utente.ruolo === 'Utente'
      ) {
        return utente.ruolo;  //ritorno del ruolo se valido
      }

      return null;
    } catch {
      return null;
  }
}

  private setSession(risposta: SessioneUtente): void {
    const utenteInSessione: SessioneUtente = {
      id: risposta.id,
      nomeCompleto: risposta.nomeCompleto,
      token: risposta.token,
      eta: risposta.eta,
      email: risposta.email,
      ruolo: risposta.ruolo,
      dataInizioAbbonamento: risposta.dataInizioAbbonamento,
      dataInizioGiftCard: risposta.dataInizioGiftCard,
      seAbbonato: risposta.seAbbonato,
      possiedeGiftCard: risposta.possiedeGiftCard
    }

    localStorage.setItem(this.storagekey, JSON.stringify(utenteInSessione));
    console.log('Utente salvato in localStorage:', utenteInSessione.ruolo); // Debug log
    this.utenteCorrente.set(utenteInSessione);
  }

  private readStoredUser(): SessioneUtente | null {
    const raw = localStorage.getItem(this.storagekey);

    if (!raw) {
      return null;
    }
    
    try{
      return JSON.parse(raw) as SessioneUtente;
      
    } catch{
      localStorage.removeItem(this.storagekey);
      return null
    }
  }
}

```

### auth.service.ts

<details>
<summary>Versione 1.2</summary>
Fabio 9-06-2026
Descrizione: Import NavbarSharedStateService

```ts
import { environment } from '../../environments/environment';
import { inject, Injectable, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { tap } from 'rxjs/operators';
import { Observable } from 'rxjs';
import { Login } from '../models/login.model';
import { Registrazione } from '../models/registrazione.model';
import { SessioneUtente } from '../models/sessione-utente.model';
import { RuoliUtente } from '../ruoliUtente';

@Injectable({
    providedIn: 'root',
})
export class AuthService {

    private readonly http = inject(HttpClient);
    private readonly router = inject(Router);
    private readonly storageKey = 'nuovo_cinema_paradiso_auth';
    private readonly baseUrl = `${environment.apiBaseUrl}/Auth`;



    // Stato utente sincronizzato con localStorage
    readonly utenteCorrente = signal<SessioneUtente | null>(this.caricaUtenteDaStorage());

    // ---------------------------
    // LOGIN
    // ---------------------------
  login(payload: Login): Observable<SessioneUtente> {
    return this.http.post<SessioneUtente>(`${this.baseUrl}/login`, payload).pipe(
      tap(response => {
        this.salvaSessione(response);
        
        // ALIMENTIAMO IL CANALE: l'effetto nel NavbarSharedStateService 
        // intercetterà questo cambio e caricherà profilo e saldo!
        this.utenteCorrente.set(response);
      })
    );
  }

    // ---------------------------
    // REGISTRAZIONE
    // ---------------------------
    registrazione(payload: Registrazione): Observable<{ message: string }> {
        return this.http.post<{ message: string }>(
            `${this.baseUrl}/registrazione`,
            payload
        );
    }

    // ---------------------------
    // LOGOUT
    // ---------------------------
    logout(): void {
        localStorage.removeItem(this.storageKey);
        this.utenteCorrente.set(null);
        void this.router.navigate(['/login']);
    }

    // ---------------------------
    // STATO UTENTE
    // ---------------------------
    isAutenticato(): boolean {
        return this.utenteCorrente() !== null;
    }

    isOperatore(): boolean {
        return this.utenteCorrente()?.ruolo === 'Operatore'
    }

    isGestore(): boolean {
        return this.utenteCorrente()?.ruolo === 'Gestore';
    }
    possiedeQualsiasiRuolo(ruoli: RuoliUtente[]): boolean {
        const ruolo = this.ottieniRuoloUtente();
        return !!ruolo && ruoli.includes(ruolo);
    }

    ruoloCorrispondente(ruolo: string): boolean {
        return this.utenteCorrente()?.ruolo === ruolo;
    }

    ottieniToken(): string | null {
        return this.utenteCorrente()?.token ?? null;
    }

    ottieniRuoloUtente(): RuoliUtente | null {
        const raw = localStorage.getItem(this.storageKey);

        if (!raw) {
            return null;
        }

        try {
            const utente = JSON.parse(raw) as SessioneUtente;

            if (
                utente.ruolo === 'Operatore' ||
                utente.ruolo === 'Gestore' ||
                utente.ruolo === 'Utente'
            ) {
                return utente.ruolo;
            }

            return null;
        } catch {
            return null;
        }
    }

    private setSession(risposta: SessioneUtente): void {
        const utenteInSessione: SessioneUtente = {
            id: risposta.id,
            nomeCompleto: risposta.nomeCompleto,
            token: risposta.token,
            eta: risposta.eta,
            email: risposta.email,
            ruolo: risposta.ruolo,
            dataInizioAbbonamento: risposta.dataInizioAbbonamento,
            seAbbonato: risposta.seAbbonato,
        }

        localStorage.setItem(this.storageKey, JSON.stringify(utenteInSessione));
        console.log('Utente salvato in localStorage:', utenteInSessione.ruolo);
        this.utenteCorrente.set(utenteInSessione)
    }

    // ---------------------------
    // PRIVATE
    // ---------------------------
    private salvaSessione(risposta: SessioneUtente): void {
        localStorage.setItem(this.storageKey, JSON.stringify(risposta));
        this.utenteCorrente.set(risposta);
    }

    private caricaUtenteDaStorage(): SessioneUtente | null {
        const raw = localStorage.getItem(this.storageKey);
        if (!raw) return null;

        try {
            return JSON.parse(raw) as SessioneUtente;
        } catch {
            localStorage.removeItem(this.storageKey);
            return null;
        }
    }
}
```

### navbar-shared-state--service.service.ts
<details>
<summary>Versione 1.0</summary>
Fabio 9-06-2026
Descrizione: creazione di questo service per far si che il saldo dell'utente si aggiorni in tempo reale quando facciamo oeprazioni tipo comprare gift card, abbonamento, proiezioni ecc...

```ts
import { Injectable, signal, computed, inject, effect } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { AuthService } from './auth.service';
import { Utente } from '../models/utente.model';
import { firstValueFrom } from 'rxjs'; // Usato SOLO per trasformare la chiamata in Promise (async/await)

@Injectable({
  providedIn: 'root'
})
export class NavbarSharedStateService {
  private http = inject(HttpClient);
  private authService = inject(AuthService);
  private readonly baseUrl = `${environment.apiBaseUrl}/Auth`;

  // Sorgente di verità finale per la Navbar
  readonly utenteLoggato = signal<Utente | null>(null);

  constructor() {
    // CANALE DI COMUNICAZIONE PERENNE (Senza RxJS)
    // Questo effect monitora il Signal "sessioneAttiva" di AuthService.
    // Gira a ogni login, logout o refresh.
    effect(async () => {
      const sessione = this.authService.utenteCorrente();

      if (sessione) {
        try {
          // Avviamo il flusso asincrono lineare (Canale Attivo)
          await this.caricaFlussoProfiloESaldo();
        } catch (err) {
          console.error("Errore nel canale asincrono della navbar:", err);
        }
      } else {
        // Se la sessione diventa null (Logout), pialliamo i dati immediatamente
        this.utenteLoggato.set(null);
      }
    });
  }

  // Sfruttiamo async/await per eliminare la complessità delle pipe RxJS
  private async caricaFlussoProfiloESaldo(): Promise<void> {
    // 1. Scarichiamo il profilo trasformando l'Observable in Promise
    const profilo = await firstValueFrom(this.http.get<Utente>(`${this.baseUrl}/profilo`));
    
    // 2. Scarichiamo il saldo relativo a questo profilo
    const resSaldo = await firstValueFrom(this.http.get<{ saldo: number }>(`${this.baseUrl}/saldo`));
    
    // 3. Uniamo i dati e aggiorniamo il Signal globale
    profilo.saldo = resSaldo.saldo;
    this.utenteLoggato.set(profilo);
  }

  // Azione continua: se l'utente compra un biglietto nel sito, 
  // chiami questo metodo e la navbar si aggiorna da sola in background
  async forzaAggiornamentoSaldo(): Promise<void> {
    if (!this.utenteLoggato()) return;

    const res = await firstValueFrom(this.http.get<{ saldo: number }>(`${this.baseUrl}/saldo`));
    
    this.utenteLoggato.update(attuale => 
      attuale ? { ...attuale, saldo: res.saldo } : null
    );
  }
}
```
</details>

### utente.service.ts

<details>
<summary>Versione 1.0</summary>
Andrea Bruno 28-05-2026
Cambio di rotta da Utente a Auth 
Modifiche nel richiamo dei servizi 

```c#
import { environment } from '../../environments/environment';
import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Utente, UtenteCreazione } from '../models/utente.model';

@Injectable({
    providedIn: 'root'
})
export class UtenteService {

    // HttpClient viene iniettato tramite inject() (metodo moderno Angular)
    private readonly http = inject(HttpClient);

    // Base URL dell'API: punta al controller Auth del backend
    // /Auth è corretto perché il controller è AuthController
    private readonly baseUrl = `${environment.apiBaseUrl}/Auth`;

    // Recupera il profilo dell'utente loggato
    // Ritorna un Observable<Utente> che il componente sottoscrive
    profilo(): Observable<Utente> {
        return this.http.get<Utente>(`${this.baseUrl}/profilo`);
    }

    // Modifica i dati dell'utente (nome + età)
    // Usa PUT perché aggiorna risorse esistenti
    modifica(payload: UtenteCreazione): Observable<any> {
        return this.http.put<any>(`${this.baseUrl}/modifica`, payload);
    }

    // Elimina definitivamente l'account dell'utente
    // Il backend gestisce la rimozione e la logout viene fatta nel componente
    eliminaProfilo(): Observable<any> {
        return this.http.delete<any>(`${this.baseUrl}/elimina`);
    }
}
```
</details>
<details>
<summary>Versione 1.1</summary>
Francesco lorenzi 05-06-2026
aggiunta il richiamo della funzione per abbonarsi 

```c#
import { environment } from '../../environments/environment';
import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Utente, UtenteCreazione } from '../models/utente.model';

@Injectable({
    providedIn: 'root'
})
export class UtenteService {

    // HttpClient viene iniettato tramite inject() (metodo moderno Angular)
    private readonly http = inject(HttpClient);

    // Base URL dell'API: punta al controller Auth del backend
    // /Auth è corretto perché il controller è AuthController
    private readonly baseUrl = `${environment.apiBaseUrl}/Auth`;

    // Recupera il profilo dell'utente loggato
    // Ritorna un Observable<Utente> che il componente sottoscrive
    profilo(): Observable<Utente> {
        return this.http.get<Utente>(`${this.baseUrl}/profilo`);
    }

    // Modifica i dati dell'utente (nome + età)
    // Usa PUT perché aggiorna risorse esistenti
    modifica(payload: UtenteCreazione): Observable<any> {
        return this.http.put<any>(`${this.baseUrl}/modifica`, payload);
    }

    // Elimina definitivamente l'account dell'utente
    // Il backend gestisce la rimozione e la logout viene fatta nel componente
    eliminaProfilo(): Observable<any> {
        return this.http.delete<any>(`${this.baseUrl}/elimina`);
    }
    //Abbona l'utente della sessione con un determinato abbonamento
    abbonati( abbonamentoId: string): Observable<any> {
        return this.http.post<any>(`${environment.apiBaseUrl}/Utente/abbonati`, { IdAbbonamento: abbonamentoId });
    }
}
```
</details>

## GUARDS

## INTERCEPTORS

## SHARED

### COMPONENTS

**lista-movie.component.ts**
<details>

<summary>Versione 1.0</summary>

Francesco Lorenzi 28/05/2026

componente TypeScript per creare una lista di film, questa lista di film comprende solo quelli dove ci sono delle proiezioni attive con essi, giustamente se ho delle proiezioni con lo stesso film, non voglio che esso sia ripetuto
```ts
import { Component, inject, signal } from '@angular/core';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { HttpErrorResponse } from '@angular/common/http';
//servizi, non abbiamo bisogno di autenticazione, la lista dei film deve essere visibile da chiunque
import { MovieService } from '../../services/movie.service';
import { ProiezioneService } from '../../services/proiezione.service';
//modelli
import { Movie } from '../../models/movie.model';
import { GenereMovie } from '../../models/genere-movie.model';
import { Proiezione } from '../../models/proiezione.model'; 


@Component({
  selector: 'lista-movie',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './lista-movie.component.html'
})

export class ListaMovieComponent {
  //servizi
  private readonly movieService = inject(MovieService);
  private readonly proiezioneService = inject(ProiezioneService);
  //modelli
  readonly movies = signal<Movie[]>([]);
  readonly proiezioni = signal<Proiezione[]>([]);
  readonly staCaricando = signal(false);
  readonly staInviando = signal(false);
  readonly messaggioErrore = signal('');
  readonly messaggioSuccesso = signal('');

  constructor() {
    this.caricaMovies();
  }
  //carica tutti i movie presenti nelle proiezioni attive
  caricaMovies(): void {

    this.staCaricando.set(true);
    this.messaggioErrore.set('');
    this.proiezioneService.ottieniTutto().subscribe({//ottengo tutte le proiezioni attive
      next: (items) => {
        this.proiezioni.set([]);
        for (let i = 0; i < items.length; i++) {
            if(new Date(items[i].dataProiezione) >= new Date()) {
                this.proiezioni.update(proiezioni => [...proiezioni, items[i]]);//le inserisco nell'array(lista)
            }
        }
        this.movies.set([]);
        //per ogni singola proiezione, prendo il suo film e lo inserisco in una array senza avere doppioni
        for (let i = 0; i < this.proiezioni().length; i++) {
            this.movieService.ottieniTramiteId(this.proiezioni()[i].movieId).subscribe({
                next: (movie) => {
                  //questo controllo serve per evitare di inserire più volte lo stesso film se è presente in più proiezioni
                    if(!this.movies().some(m => m.id === movie.id)) { 
                        this.movies.update(movies => [...movies, movie]);
                    }
                },
                error: (error: unknown) => {
                    this.messaggioErrore.set(this.estraiMessaggioErrore(error, 'film non trovati'));
                }
            });
        }
        this.staCaricando.set(false);
      },
      error: (error: unknown) => {
        this.staCaricando.set(false);
        this.messaggioErrore.set(this.estraiMessaggioErrore(error, 'proiezioni non trovate'));
      }
    });
  }

  tracciaPerId(_: string, item: Movie): string {
    return item.id;
  }

  private estraiMessaggioErrore(error: unknown, fallback: string): string {

    if (error instanceof HttpErrorResponse) {
      return error.error?.message ?? fallback;
    }
    return fallback;
  }
}

```
</details>

**lista-movie.component.html**
<details>

<summary>Versione 1.0</summary>

Francesco Lorenzi 28/05/2026

semplice componente html della lista dei film
```html
<section>

    @if (messaggioErrore()) {
    <div class="alert alert-warning">{{ messaggioErrore() }}</div>
    }

    @if (messaggioSuccesso()) {
    <div class="alert alert-success">{{ messaggioSuccesso() }}</div>
    }

    <div class="grid grid-2">
        <article class="card">
            <h2>Lista Film</h2>

            @if (staCaricando()) {
            <p class="muted">Caricamento in corso...</p>
            } @else if (movies().length === 0) {
            <p class="muted">Nessun film presente.</p>
            } @else {
            <div class="list">
                @for (item of movies(); track tracciaPerId($index.toString(), item)) {
                <div class="list-item">
                    <div>
                        <strong>{{ item.titolo }}</strong>
                        <p>{{ item.prezzoMovie }} €</p>
                        <p>{{item.durataMinuti}} min</p>
                        <p>{{item.genere }}</p>
                        <p>{{item.descrizione}}</p>
                    </div>
                </div>
                }
            </div>
            }
        </article>
    </div>
</section>
```
</details>

### Navbar V1.0
Utente: Fabio Tammaro
Data: 26/05/2026
Descrizione: Modificata la navbar nel suo file ts e nel file hmtl in base al ruolo e all'autenticazione. (il nome del branch differisce: Modifiche-dashboard)

### navbar.component.ts

<details>
<summary>Versione 1.0</summary>

```typescript
import { Component, computed, inject } from '@angular/core';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { AuthService } from './../../../services/auth.service';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [RouterLink, RouterLinkActive],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.css'
})
export class NavbarComponent {
  private readonly authService = inject(AuthService);

  readonly utente          = this.authService.utenteCorrente;
  // quando authService cambia il valore delle variabili sottostanti viene ricalcolato
  // e si aggiorna di conseguenza.
  readonly isAutenticato   = computed(() => this.authService.isAutenticato ());
  readonly isUtente        = computed(()=> this.authService.ruoloCorrispondente('Utente'));

  logout(): void {
    this.authService.logout();
  }
}
```
</details>



### navbar.component.html 

<details>
<summary>Versione 1.0</summary>

```html

<header class="navbar-shell">
  <div class="container navbar">
    @if(!isAutenticato()){
    <a routerLink="/dashboard" class="brand">Nuovo cinema Paradiso</a>
    <div class="btn-row">
      <a class="btn btn-secondary" routerLink="/film">Film</a>
    </div>
    <div class="btn-row">
      <a class="btn btn-secondary" routerLink="/proiezioni">Proiezioni</a>
    </div>

    <nav class="links">
      <div class="btn-row">
        <a class="btn btn-secondary" routerLink="/login">Login</a>
      </div>
      <div class="btn-row">
        <a class="btn btn-secondary" routerLink="/register">Registrati</a>
      </div>
    </nav>
    }

    @if (isAutenticato()) {
    <nav class="links">
      <a routerLink="/proiezioni" class="brand">Nuovo cinema Paradiso</a>
      <div class="btn-row">
        <a class="btn btn-secondary" routerLink="/film">Film</a>
      </div>
      <div class="btn-row">
        <a class="btn btn-secondary" routerLink="/proiezioni">Proiezioni</a>
      </div>
      <div class="btn-row">
        <a class="btn btn-secondary" routerLink="/abbonamenti">Abbonamenti</a>
      </div>
       @if(isGestore()){
      <div class="btn-row">
        <a class="btn btn-secondary" routerLink="/gestione">Gestione</a>
      </div>
       }
      <div class="btn-row">
        <a class="btn btn-secondary" routerLink="/Profilo">Profilo</a>
      </div>
      <div>
        <strong>{{ utente()?.ruolo }}</strong>
        <div class="muted small">{{ utente()?.email }}</div>
      </div>
      <div class="user-box">
        <button class="btn btn-secondary" type="button" (click)="logout()">Logout</button>
      </div>
    </nav>

  }
  </div>
</header>
```
</details>

### Navbar V2.0
Utente: Fabio Tammaro
Data: 28/05/2026
Descrizione: Modificata la navbar nel suo file ts e nel file hmtl in base al ruolo e all'autenticazione, aggiunto il pulsante gestione.

### navbar.component.ts

<details>
<summary>Versione 1.0</summary>

```typescript
import { Component, computed, inject } from '@angular/core';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { AuthService } from './../../../services/auth.service';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [RouterLink, RouterLinkActive],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.css'
})
export class NavbarComponent {
  private readonly authService = inject(AuthService);

  readonly utente          = this.authService.utenteCorrente;
  // quando authService cambia il valore delle variabili sottostanti viene ricalcolato
  // e si aggiorna di conseguenza.
  readonly isAutenticato   = computed(() => this.authService.isAutenticato ());
  readonly isUtente        = computed(()=> this.authService.ruoloCorrispondente('Utente'));
  readonly isGestore       = computed(() => this.authService.isGestore ());


  logout(): void {
    this.authService.logout();
  }
}
```
</details>



### navbar.component.html 

<details>
<summary>Versione 1.0</summary>

```html

<!-- Header ereditato dal css -->
<header class="navbar-shell">
  <div class="container navbar">
    @if(!isAutenticato()){
    <a routerLink="/dashboard" class="brand">Nuovo cinema Paradiso</a>
    <div class="btn-row">
        <a class="btn btn-secondary" routerLink="/film">Film</a>
      </div>
      <div class="btn-row">
    <div class="btn-row">
      <a class="btn btn-secondary" routerLink="/film">Proiezioni</a>
    </div>

    <nav class="links">
      <div class="btn-row">
        <a class="btn btn-secondary" routerLink="/login">Login</a>
      </div>
      <div class="btn-row">
        <a class="btn btn-secondary" routerLink="/register">Registrati</a>
      </div>
    </nav>
    }

    @if (isAutenticato()) {
    <nav class="links">
      <a routerLink="/proiezioni" class="brand">Nuovo cinema Paradiso</a>
      <div class="btn-row">
        <a class="btn btn-secondary" routerLink="/film">Film</a>
      </div>
      <div class="btn-row">
        <a class="btn btn-secondary" routerLink="/proiezioni">Proiezioni</a>
      </div>
      <div class="btn-row">
        <a class="btn btn-secondary" routerLink="/abbonamenti">Abbonamenti</a>
      </div>
       @if(isGestore()){
      <div class="btn-row">
        <a class="btn btn-secondary" routerLink="/gestore/logs">Gestione</a>
      </div>
       }
      <div class="btn-row">
        <a class="btn btn-secondary" routerLink="/Profilo">Profilo</a>
      </div>
      <div>
        <!-- Strong rappresenta un contenuto di forte importanza semantica per il browser che lo mostrerà in grassetto.-->
        <strong>{{ utente()?.ruolo }}</strong>
        <div class="muted small">{{ utente()?.email }}</div>
      </div>
      <div class="user-box">
        <button class="btn btn-secondary" type="button" (click)="logout()">Logout</button>
      </div>
    </nav>

  }
  </div>
</header>
```
</details>

### navbar.component.css

<details>
<summary>Versione 1.0</summary>

```css
/* Contenitore principale della navbar */
.navbar-shell {
  position: sticky;
  top: 0;
  z-index: 10;
  background: rgba(15, 23, 42, 0.92);
  backdrop-filter: blur(10px);
  border-bottom: 1px solid var(--border);
}

.navbar {
  min-height: 72px;
  display: flex;
  align-items: center;
  justify-content: space-between;
  gap: 1rem;
}

.brand {
  font-size: 1.1rem;
  font-weight: 800;
  color: var(--text);
}

.links {
  display: flex;
  flex-wrap: wrap;
  gap: 1rem;
}

.links a {
  color: var(--muted);
  font-weight: 600;
}

.links a.active,
.links a:hover {
  color: var(--primary);
}

.user-box {
  display: flex;
  align-items: center;
  gap: 1rem;
}

.small {
  font-size: 0.875rem;
  color: var(--muted);
}
/* Responsività tablet/mobile*/
@media (max-width: 900px) {
  .navbar {
    flex-direction: column;
    align-items: flex-start;
    padding: 0.75rem 0;
  }

  .user-box {
    width: 100%;
    justify-content: space-between;
  }
}
```
</details>
## ENVIRONMENT


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