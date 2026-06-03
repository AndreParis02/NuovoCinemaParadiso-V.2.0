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
- abbonamento-list.component.ts [tutti] - Andrea Paris (fatto)
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

<details><summary>Versione 1.0</summary>

- Utente: Simeone
- Data: 03/06/2026
```ts
import { Component, inject, signal, OnInit } from '@angular/core';
import { DatePipe, CurrencyPipe } from '@angular/common';
import { GestioneService } from '../../../services/gestione.service';
import { GiftCard } from '../../../models/gestione.model';
import { AuthService } from '../../../services/auth.service';

@Component({
  selector: 'giftcard-list',
  standalone: true,
  imports: [DatePipe, CurrencyPipe], 
  templateUrl: './giftcard-list.component.html',
})
export class GiftCardListComponent implements OnInit {

  private readonly gestioneService = inject(GestioneService);
  private readonly authService = inject(AuthService);

  readonly giftCards = signal<GiftCard[]>([]);

  readonly isLoading = signal(false);
  readonly errorMessage = signal('');

  ngOnInit(): void {
    this.caricaGiftCard();
  }

  caricaGiftCard(): void {
    this.isLoading.set(true);
    this.errorMessage.set('');

    if (this.authService.isGestore()) {
      
      this.gestioneService.ottieniGiftCards().subscribe({
        next: (data) => this.gestisciSuccesso(data),
        error: (error) => this.gestisciErrore(error)
      });

    } else {

      const utenteId = this.authService.utenteCorrente()?.id;

      if (!utenteId) {
        this.errorMessage.set('Errore: Impossibile identificare l\'utente.');
        this.isLoading.set(false);
        return;
      }

      this.gestioneService.ottieniMieGiftCards().subscribe({
        next: (data) => this.gestisciSuccesso(data),
        error: (error) => this.gestisciErrore(error)
      });

    }
  }


  private gestisciSuccesso(data: GiftCard[]): void {
    this.giftCards.set(data);
    this.isLoading.set(false); 
  }

  private gestisciErrore(error: unknown): void {
    console.error('ERRORE GiftCard:', error);
    this.isLoading.set(false);
    this.errorMessage.set('Si è verificato un errore nel caricamento dei dati delle giftcard.');
  }

  trackById(_: number, item: any): string | null {
    return item.id;
  }
}
```

</details>

- giftcard-list.component.html [gestore,utente] Simeone

<details><summary>Versione 1.0</summary>

- Utente: Simeone
- Data: 03/06/2026
```html
<section class="page">
    <h1 class="page-title">Gestione Gift Card</h1>
    <p class="page-subtitle">Elenco di tutte le Gift Card attive nel sistema.</p>

    @if(errorMessage()){
        <div class="alert alert-warning">{{ errorMessage() }}</div>
    }

    @if(isLoading()){
        <p class="muted">Recupero Gift Card dal server in corso...</p>
    } @else {
        
        <article class="card">
            <h2 style="margin-top: 0; margin-bottom: 1.5rem; font-size: 1.2rem;">
                Totale Card Emesse: <span style="color: var(--primary);">{{ giftCards().length }}</span>
            </h2>

            <div class="list">
                @for(gc of giftCards(); track trackById($index, gc)){
                    <div class="list-item">
                        
                        <div class="user-info">
                            <strong>{{ gc.nome }}</strong> 
                            <br>
                            <span class="muted" style="font-size: 0.85rem;">Codice: {{ gc.codiceRiscatto }}</span>
                        </div>

                        <div>
                            <span class="badge">{{ gc.valore | currency:'EUR' }}</span>
                        </div>

                    </div>
                } @empty {
                    <div class="list-item">
                        <span class="muted">Nessuna Gift Card emessa al momento.</span>
                    </div>
                }
            </div>
        </article>

    }
</section>
```

</details>


## auth 
### page
### components
- login.component.ts
<details>
<summary>versione1.0</summary>

Lorenzo Laviosa
03/06/2026

creazione componente per il login

## login.component.ts
```ts
import { Component, inject, signal } from '@angular/core';
import { FormBuilder, Validators, ReactiveFormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../../services/auth.service';
import { HttpErrorResponse } from '@angular/common/http';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    CommonModule,
    RouterLink, // necessario per usare [routerLink] nell'HTML
  ],
  templateUrl: './login.component.html',
})
export class LoginComponent {

  // servizi principali
  private readonly fb = inject(FormBuilder);
  private readonly authService = inject(AuthService);
  private readonly router = inject(Router);

  // segnali per stato UI
  readonly isLoading = signal(false);
  readonly errorMessage = signal('');
  readonly submitted = signal(false);

  // form reattivo tipizzato
  readonly loginForm = this.fb.nonNullable.group({
    email: this.fb.nonNullable.control('', {
      validators: [Validators.required, Validators.email]
    }),
    password: this.fb.nonNullable.control('', {
      validators: [Validators.required]
    })
  });

  submit(): void {
    this.submitted.set(true); // abilita la visualizzazione degli errori

    if (this.loginForm.invalid) {
      return; // evita chiamate inutili al backend
    }

    this.isLoading.set(true);
    this.errorMessage.set('');

    const payload = this.loginForm.getRawValue(); // valori già tipizzati

    // chiamata al backend
    this.authService.login(payload).subscribe({
      next: () => {
        this.isLoading.set(false);
        this.router.navigate(['/']); // redirect dopo login
      },
      error: (err: unknown) => {
        this.isLoading.set(false);
        this.errorMessage.set(this.extractError(err)); // messaggio leggibile
      }
    });
  }

  // estrae un messaggio di errore leggibile dal backend
  private extractError(error: unknown): string {
    if (error instanceof HttpErrorResponse) {
      return error.error?.message ?? 'Credenziali non valide.';
    }
    return 'Errore durante il login.';
  }
}
```

## login.component.html

Lorenzo Laviosa
03/06/2026

```html
<section class="login-section">

  <h1 class="page-title">Accedi</h1>
  <p class="page-subtitle">Inserisci le tue credenziali per accedere all'applicazione.</p>

  <!-- Errore proveniente dal backend -->
  @if (errorMessage()) {
    <div class="alert alert-danger soft-alert">
      {{ errorMessage() }}
    </div>
  }

  <div class="grid grid-2 login-grid">

    <!-- Form di login con Reactive Forms -->
    <form [formGroup]="loginForm" (ngSubmit)="submit()" class="card login-card">

      <div class="form-group">
        <label>Email</label>
        <input type="email" formControlName="email" placeholder="esempio@email.com">

        <!-- Errori mostrati solo dopo submit -->
        @if (submitted() && loginForm.controls.email.invalid) {
          <p class="error-text">Inserisci un'email valida.</p>
        }
      </div>

      <div class="form-group">
        <label>Password</label>
        <input type="password" formControlName="password" placeholder="••••••••">

        @if (submitted() && loginForm.controls.password.invalid) {
          <p class="error-text">La password è obbligatoria.</p>
        }
      </div>

      <!-- Bottoni principali -->
      <div class="button-row">
        <button type="submit" class="btn-primary" [disabled]="isLoading()">
          {{ isLoading() ? 'Accesso in corso...' : 'Login' }}
        </button>

        <a [routerLink]="['/register']" class="btn-secondary">
          Registrati
        </a>
      </div>

    </form>

    <!-- Card informativa con utenti demo -->
    <div class="card login-card">
      <h2>Utenti demo</h2>

      <div class="demo-list">
        <div class="demo-item">
          <strong>Gestore</strong>
          <p>gestore@gmail.com / 123456</p>
        </div>

        <div class="demo-item">
          <strong>Operatore</strong>
          <p>operatore@gmail.com / 123456</p>
        </div>

        <div class="demo-item">
          <strong>Utente</strong>
          <p>utente1@gmail.com / 123456</p>
        </div>
      </div>

    </div>

  </div>

</section>
```

- register.component.ts

## movie
### components
- movie-list.component.ts [operatore] Francesco
<details>
<summary>versione1.0</summary>

Francesco Lorenzi
03/06/2026

creazione del componente lista movie

## movie-list.component.ts

```ts

import { Component, inject, signal } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { HttpErrorResponse } from '@angular/common/http';
//servizi
import { AuthService } from '../../../services/auth.service';
import { MovieService } from '../../../services/movie.service';

//modelli
import { Movie } from '../../../models/movie.model';

@Component({
  selector: 'movie-list',
  standalone: true,
  templateUrl: './movie-list.component.html'
})

export class MovieListComponent {
// servizi
  private readonly authService = inject(AuthService);
  private readonly movieService = inject(MovieService);

// componenti
  readonly movies = signal<Movie[]>([]);
  readonly staCaricando = signal(false);
  readonly staInviando = signal(false);
  readonly messaggioErrore = signal('');
  readonly messaggioSuccesso = signal('');
  constructor() {
    this.caricaMovies();
  }
  visualizzabileDa(): boolean {
    return this.authService.possiedeQualsiasiRuolo(['Operatore']);
  }
  // carica tii i movie in una lista
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
      }
    });
  }
// traccia la lista dei movie per i loro ID
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

## movie-list.component.html

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
                    <div><!-- ho lasciato la descrizione fuori dai campi dalla lista perché irrilevante -->
                        <strong>{{ item.titolo }}</strong>
                        <p>{{ item.prezzoMovie }} €</p>
                        <div class="muted">ID: {{ item.id }}</div>
                    </div>    
                </div>
                }
            </div>
            }
        </article>
```

</details>

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
<details>
<summary>versione1.0</summary>

Francesco Lorenzi 03/06/2026

creazione del componente lista sala

## sala-list.component.ts
```ts
import { Component, inject, signal } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
// servizi
import { AuthService } from '../../../services/auth.service';
import { SalaService } from '../../../services/sala.service';

// modelli
import { Sala } from '../../../models/sala.model';


@Component({
  selector: 'sala-list',
  standalone: true,
  templateUrl: './sala-list.component.html'
})

export class SalaListComponent {
// servizi
  private readonly authService = inject(AuthService);
  private readonly salaService = inject(SalaService);
// modelli
  readonly sale = signal<Sala[]>([]);

  readonly staCaricando = signal(false);
  readonly staInviando = signal(false);
  readonly messaggioErrore = signal('');
  readonly messaggioSuccesso = signal('');


  constructor() {
    this.caricaSale();
  }
  // la lista delle sale è visualizzabile solo dall'operatore
  visualizzabileDa(): boolean {
    return this.authService.possiedeQualsiasiRuolo(['Operatore']);
  }
  // carica le sale sulla lista
  caricaSale(): void {

    this.staCaricando.set(true);
    this.messaggioErrore.set('');
    this.salaService.ottieniTutto().subscribe({

      next: (items) => {
        this.sale.set(items);
        this.staCaricando.set(false);

      },
      error: (error: unknown) => {
        this.staCaricando.set(false);
        this.messaggioErrore.set(this.estraiMessaggioErrore(error, 'sale non trovate'));
      }
    });
  }
// traccia la lista per ID
  tracciaPerId(_: string, item: Sala): string {
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
## sala-list.component.html
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
            <h2>Lista Sale</h2>

            @if (staCaricando()) {
            <p class="muted">Caricamento in corso...</p>
            } @else if (sale().length === 0) {
            <p class="muted">Nessuna sala presente.</p>
            } @else {
            <div class="list">
                @for (item of sale(); track tracciaPerId($index.toString(), item)) {
                <div class="list-item">
                    <div>
                        <strong>{{ item.nome }}</strong>
                        <p>Tipologia: {{ item.nomeTipologia }}</p>
                        <p>Capienza: {{ item.capienza }}</p>
                        <div class="muted">ID: {{ item.id }}</div>
                    </div>    
                </div>
                }
            </div>
            }
        </article>
```
</details>

- sala-form.component.ts [operatore]

## tipologia-sala
### components
- tipologia-sala-list.component.ts [operatore] Greg
- tipologia-sala-form.component.ts [operatore]

## turno
### components
- turno-list.component.ts [operatore] Andrea paris (fatto)
- turno-form.component.ts [operatore]

## cambio-ruolo
### components
- cambio-ruolo-form.component.ts [gestore]
- utenti-list.component.ts [gestore] Andrea Bruno

## log
### components
- log-list.component.ts [gestore] Simeone

<details><summary>Versione 1.0</summary>

- Utente: Simeone
- Data: 03/06/2026
```ts
import { Component, inject, signal, OnInit } from '@angular/core';
import { DatePipe, CurrencyPipe } from '@angular/common';
import { GestioneService } from '../../../services/gestione.service';
import { LogAzioni } from '../../../models/gestione.model';

@Component({
  selector: 'log-list',
  standalone: true,
  imports: [DatePipe, CurrencyPipe],
  templateUrl: './log-list.component.html',
  
})
export class LogListComponent implements OnInit {
  private readonly gestioneService = inject(GestioneService);

  readonly logs = signal<LogAzioni[]>([]);

  readonly isLoading = signal(false);
  readonly errorMessage = signal('');

  ngOnInit(): void {
    this.caricaLogs();
  }

  caricaLogs(): void {
    this.isLoading.set(true);
    this.errorMessage.set('');

    this.gestioneService.ottieniLog().subscribe({
      next: (data) => {
        this.logs.set(data);
        this.isLoading.set(false);
      },
      error: (error: unknown) => {
        console.error('ERRORE LOG AZIONI:', error);

        this.isLoading.set(false);
        this.errorMessage.set('Si è verificato un errore nel caricamento dei dati.');
      },
    });
  }

  trackById(_: number, item: any): string | null {
    return item.id;
  }
}

```

</details>

- log-list.component.html [gestore] Simeone


<details><summary>Versione 1.0</summary>

- Utente: Simeone
- Data: 03/06/2026
```html
<section>
    <h1 class="page-title">Cronologia Operazioni di Sistema</h1>
    <p class="page-subtitle">Registro delle azioni effettuate all'interno del Nuovo Cinema Paradiso.</p>

    @if(errorMessage()){
        <div class="alert alert-warning">{{ errorMessage() }}</div>
    }

    @if(isLoading()){
        <p class="muted">Recupero log dal server in corso...</p>
    } @else {
        
        <article class="card">
            <div class="list">
                @for(item of logs(); track trackById($index, item)){
                    <div class="list-item">
                        <div>
                            <p>
                                <strong>{{ item.nomeAzione }}</strong>
                                {{ item.messaggio }} (Esito: {{ item.effettuato ? 'Successo' : 'Fallito' }})
                            </p>
                            <div class="muted"> 
                                Utente ID: {{ item.idUtente || 'Sistema' }} | 
                                Data: {{ item.timeStamp | date:'dd/MM/yyyy HH:mm:ss' }}
                            </div>
                        </div>
                    </div>
                } @empty {
                    <div class="list-item">
                        <p class="muted">Nessuna operazione registrata nel sistema al momento.</p>
                    </div>
                }
            </div>
        </article>

    }
</section>
```

</details>


## conto-cinema
### components
- conto-cinema-detail.component.ts [gestore]


