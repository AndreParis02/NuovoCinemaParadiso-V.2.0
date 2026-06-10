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



