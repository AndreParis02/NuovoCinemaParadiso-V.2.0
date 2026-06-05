
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
- abbonamento-form.component.ts [operatore] Andrea Paris (fatto)

## biglietto
### components
- biglietto-list.component.ts [utente,operatore,gestore] Andrea Bruno (fatto)
- biglietto-form.component.ts [operatore] (Marco)

## dashboard
[priorità]
### layout 
- dashboard.layout.ts (Francesco: inserire nel layout di dashboard il component di profilo, biglietti, giftcard, abbonamento (e se non abbonato devono apparire gli abbonamenti disponibili))

(PER IL GESTORE: deve avere SOLO log, vedi giftcard, implementare cambio ruolo al posto dell'operatore)


## shared/navbar
[priorità] (Fabio: modificare aggiungendo sale per l'operatore, rimuovendo profilo per tutti, crediti)

## profilo
### components
- profilo.component.ts [utente,operatore,gestore] Francesco

## genere-movie

### components

- genere-movie-list.component.ts [tutti] Greg

Utente: Greg

Data: 04/06/2026

```ts
//controllare gli import. nomi classi e file sono differenti per "-"
import { Component, inject, signal } from '@angular/core';
//import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { AuthService } from '../../../services/auth.service';
import { GenereMovieService } from '../../../services/genere-movie.service';
import { GenereMovie } from '../../../models/genere-movie.model';

@Component({
  selector: 'genere-movie-list',
  imports: [],
  templateUrl: './genere-movie-list.component.html',
})

export class GenereMovieComponentList {

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
          this.extractErrorMessage(error, 'Impossibile caricare i generi dei film')
        );
      }
    });
  }

  trackById(_: string, item: GenereMovie): string | null {
    return item.id
  }

  private extractErrorMessage(error: unknown, fallback: string): string {
    if (error instanceof Error) {
      return error?.message ?? fallback;
    }
    return fallback
  }
}
```

Utente: Greg

Data: 04/06/2026

```html
<section>
    <h1 class="page-title">Generi di Film</h1>
    <p class="page-subtitle"> il backend restituisce la lista generi di film</p>

     @if(errorMessage()){
        <div class="alert alert-warning">{{ errorMessage()}}</div>
     }

     @if(successMessage()){
        <div class="alert alert-success">{{ successMessage() }}</div>
     }

     <div class="grid grid-2">
        <article class="card">
            <h2>Lista generi di film</h2>

            @if(isLoading()){
                <p class="muted"> Caricamento in corso...</p>
            } @else if(generiMovies().length === 0){
                <p class="muted"> Nessun genere di film presente</p>
            } @else{
                <div class="list">
                    @for(item of generiMovies(); track item.id) {
                        <div class="list-item">
                            <div>
                                <strong>{{ item.genere }}</strong>
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


## giftcard
### components
- riscatta-codice.component.ts (Simeone)
- crea-codice.component.ts (Simeone)
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
- register.component.ts
<details>
<summary>versione1.0</summary>

Lorenzo Laviosa
03/06/2026

creazione componente per la registrazione
## register.component.ts
```ts
import { Component, inject, signal } from '@angular/core';
import { FormBuilder, Validators, ReactiveFormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../../services/auth.service';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    CommonModule,
    RouterLink,
  ],
  templateUrl: './register.component.html',
})
export class RegisterComponent {

  private readonly fb = inject(FormBuilder);
  private readonly authService = inject(AuthService);
  private readonly router = inject(Router);

  // segnali per stato UI e gestione errori
  readonly isLoading = signal(false);
  readonly errorMessage = signal('');
  readonly submitted = signal(false);

  // form tipizzato; eta non-nullable per evitare errori TS
  readonly registerForm = this.fb.nonNullable.group({
    nomeCompleto: this.fb.nonNullable.control('', Validators.required),
    eta: this.fb.nonNullable.control(18, Validators.required),
    email: this.fb.nonNullable.control('', [Validators.required, Validators.email]),
    password: this.fb.nonNullable.control('', [Validators.required, Validators.minLength(6)]),
  });

  submit(): void {
    this.submitted.set(true); // abilita la visualizzazione degli errori

    if (this.registerForm.invalid) {
      return; // evita chiamate al backend con dati non validi
    }

    this.isLoading.set(true);
    this.errorMessage.set('');

    const payload = this.registerForm.getRawValue(); // valori già sicuri e tipizzati

    this.authService.registrazione(payload).subscribe({
      next: () => {
        this.isLoading.set(false);
        this.router.navigate(['/login']); // redirect dopo registrazione
      },
      error: (err: unknown) => {
        this.isLoading.set(false);
        this.errorMessage.set(this.extractError(err)); // messaggio leggibile
      }
    });
  }

  // converte errori backend in testo leggibile
  private extractError(error: unknown): string {
    if (error instanceof HttpErrorResponse) {
      return error.error?.message ?? 'Errore durante la registrazione.';
    }
    return 'Errore imprevisto.';
  }
}
```
- register.component.html
<details>
<summary>versione1.0</summary>

Lorenzo Laviosa
03/06/2026

## register.component.html
```html
<section class="register-section">

  <h1 class="page-title">Registrazione</h1>
  <p class="page-subtitle">Crea un nuovo account per accedere all'applicazione.</p>

  <!-- errore proveniente dal backend -->
  @if (errorMessage()) {
    <div class="alert alert-danger soft-alert">
      {{ errorMessage() }}
    </div>
  }

  <!-- form reattivo con validazione -->
  <form [formGroup]="registerForm" (ngSubmit)="submit()" class="card register-card">

    <div class="grid grid-2">
      <div class="form-group">
        <label>Nome completo</label>
        <input type="text" formControlName="nomeCompleto" placeholder="Mario Rossi">

        <!-- errore mostrato solo dopo submit -->
        @if (submitted() && registerForm.controls.nomeCompleto.invalid) {
          <p class="error-text">Il nome è obbligatorio.</p>
        }
      </div>

      <div class="form-group">
        <label>Età</label>
        <input type="number" formControlName="eta" placeholder="18">

        @if (submitted() && registerForm.controls.eta.invalid) {
          <p class="error-text">Inserisci un'età valida.</p>
        }
      </div>
    </div>

    <div class="grid grid-2">
      <div class="form-group">
        <label>Email</label>
        <input type="email" formControlName="email" placeholder="email@example.com">

        @if (submitted() && registerForm.controls.email.invalid) {
          <p class="error-text">Inserisci un'email valida.</p>
        }
      </div>

      <div class="form-group">
        <label>Password</label>
        <input type="password" formControlName="password" placeholder="••••••••">

        @if (submitted() && registerForm.controls.password.invalid) {
          <p class="error-text">La password deve contenere almeno 6 caratteri.</p>
        }
      </div>
    </div>

    <!-- pulsanti principali -->
    <div class="btn-row">
      <button class="btn btn-primary" type="button" (click)="submit()">
        Registrati
      </button>

      <a [routerLink]="['/login']" class="btn btn-secondary">
        Torna al login
      </a>
    </div>

  </form>

</section>
```


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

<details>
<summary>versione1.1</summary>

Francesco Lorenzi
03/06/2026

ho dovuto modificare la lista per poter integrare il form in essa, la lista è parente del form, che è quindi suo figlio. 

## movie-list.component.ts

```ts

import { Component, inject, signal } from '@angular/core';
//ho cancellato l'importo, perche il form è un componente esterno
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
  readonly filmScelto = signal<Movie | null>(null);//aggiunto il film scelto, così da poterlo inserire nel form per fare delle modifiche su di esso
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
  //aggiunta la funzione di eliminazione, selezioniamo già un film per la modifica, lo selezioneremo allo stesso modo per l'eliminazione
  elimina(item: Movie): void {
    if (!this.visualizzabileDa()) {
      return;
    }
    // popup di conferma
    if (!confirm(`Sei sicuro di voler eliminare il film "${item.titolo}"?`)) {
      return;
    }

    this.messaggioErrore.set('');
    this.messaggioSuccesso.set('');

    this.movieService.elimina(item.id).subscribe({
      next: () => {
        if (this.filmScelto()?.id === item.id) {
          this.filmScelto.set(null);
        }
        this.caricaMovies();
      },
      error: (error: unknown) => {

        this.messaggioErrore.set(this.estraiMessaggioErrore(error, 'eliminazione non riuscita.'));
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
                    <div>
                        <strong>{{ item.titolo }}</strong>
                        <p>{{ item.prezzoMovie }} €</p>
                        <div class="muted">ID: {{ item.id }}</div>
                        <!-- agiiunti i bottoni di eliminazione e modifica-->
                         @if(visualizzabileDa()) {
                        <div class="btn-row" style="margin-top: 1rem;">
                            <!-- questo bottone setta il film scelto con l'elemento della lista -->
                            <button (click)="filmScelto.set(item)">Modifica</button>
                            <!-- questo bootone elimina l'elemento della lista --> 
                            <button (click)="elimina(item)" [disabled]="staInviando()">Elimina</button>
                        </div>
                        }
                    </div>    
                </div>
                }
            </div>
            }
        </article>
        <!-- qua carica il componente form, suo figlio-->
         @if (visualizzabileDa()) {
        <article class="card">
          <!-- tutti i dati ignettati nel form (input, effect)
           quando si sceglie il film, il form si completa con i 
           dati di questo per la modifica -->
           
            <movie-form [movieSelezionato]="filmScelto()" (modificaCompletata)="caricaMovies()"></movie-form>
        </article>
        }
```

</details>

- movie-form.component.ts [operatore] Francesco
<details>
<summary>versione1.0</summary>

Francesco Lorenzi 03/06/2026

creazione del componente form del movie

## movie-form.component.ts
```ts
import { Component, inject, signal, input, effect } from '@angular/core';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { HttpErrorResponse } from '@angular/common/http';

import { AuthService } from '../../../services/auth.service';
import { MovieService } from '../../../services/movie.service';
import { GenereMovieService } from '../../../services/genere-movie.service';

import { Movie } from '../../../models/movie.model';
import { GenereMovie } from '../../../models/genere-movie.model';


@Component({
    selector: 'movie-form',
    standalone: true,
    imports: [ReactiveFormsModule],
    templateUrl: './movie-form.component.html'
})

export class MovieFormComponent {

    private readonly formBuilder = inject(FormBuilder);
    private readonly authService = inject(AuthService);
    private readonly movieService = inject(MovieService);
    private readonly genereService = inject(GenereMovieService);


    readonly generi = signal<GenereMovie[]>([]);
    readonly movieSelezionato = input<Movie | null>(null);
    readonly staCaricando = signal(false);
    readonly staInviando = signal(false);
    readonly messaggioErrore = signal('');
    readonly messaggioSuccesso = signal('');
    readonly modificaId = signal<string>('');


    readonly form = this.formBuilder.nonNullable.group({
        titolo: ['', [Validators.required, Validators.maxLength(100)]],
        descrizione: ['', [Validators.required, Validators.maxLength(200)]],
        durataMinuti: [1, [Validators.required, Validators.min(1)]],
        prezzoMovie: [0.01, [Validators.required, Validators.min(0.01), Validators.max(999999999)]],
        genereId: ['', [Validators.required]]
    });

    constructor() {
        this.caricaGeneri();
      // quando chiamiamo il form dal componente lista, viene condiviso con il form il film selezionato, e di conseguenza il form si riempirà dinamicamente.  
        effect(() => {
            const movie = this.movieSelezionato();
            if (movie) {
                this.inizioModifica(movie);
            } else {
                this.ripristinaForm();
            }
        });
    }
    // booleano per controllare i privilegi
    modificabileDa(): boolean {
        return this.authService.possiedeQualsiasiRuolo(['Operatore']);
    }
    //c carichiamo i generi così da poterli selezionare da un menù a tendina
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
            }
        });
    }
    // bottone di invio o di modifica
    invia(): void {

        if (this.form.invalid || !this.modificabileDa()) {
            this.form.markAllAsTouched();
            return;
        }

        this.staInviando.set(true);
        this.messaggioErrore.set('');
        this.messaggioSuccesso.set('');


        const request$ = this.modificaId()

            ? this.movieService.modifica(this.modificaId(), this.form.getRawValue())
            : this.movieService.crea(this.form.getRawValue());


        request$.subscribe({
            next: () => {
                this.staInviando.set(false);
                this.messaggioSuccesso.set(this.modificaId() ? 'Film aggiornato.' : 'Film creato.');
                this.ripristinaForm();
            },
            error: (error: unknown) => {
                this.staInviando.set(false);
                this.messaggioErrore.set(this.estraiMessaggioErrore(error, 'Operazione non riuscita.'));
            }
        });
    }

    inizioModifica(item: Movie): void {

        if (!this.modificabileDa()) {
            return;
        }
        this.modificaId.set(item.id);

        this.form.patchValue({ titolo: item.titolo, descrizione: item.descrizione, durataMinuti: item.durataMinuti, prezzoMovie: item.prezzoMovie, genereId: item.genereId });
        this.messaggioErrore.set('');
        this.messaggioSuccesso.set('');
    }

    ripristinaForm(): void {
        this.modificaId.set('');
        this.form.reset({ titolo: '', descrizione: '', durataMinuti: 1, prezzoMovie: 0, genereId: '' });
    }
    tracciaPerId(_: string, item: Movie | GenereMovie): string {
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

## movie-form.component.html

Francesco Lorenzi 03/06/2026
 semplice html 
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
                <h2>{{ modificaId() ? 'Modifica film' : 'Aggiungi film' }}</h2>

                <form class="form-grid" [formGroup]="form" (ngSubmit)="invia()">
                    <div>
                        <label for="titolo">Titolo</label>
                        <input id="titolo" type="text" formControlName="titolo" [disabled]="!modificabileDa()">
                        <label for="descrizione">Descrizione</label>
                        <input id="descrizione" type="text" formControlName="descrizione"
                            [disabled]="!modificabileDa()">
                        <label for="durataMinuti">Durata (minuti)</label>
                        <input id="durataMinuti" type="number" formControlName="durataMinuti"
                            [disabled]="!modificabileDa()">
                        <label for="prezzoMovie">Prezzo (€)</label>
                        <input id="prezzoMovie" type="number" formControlName="prezzoMovie"
                            [disabled]="!modificabileDa()">
                            <!-- per scegliere il genere ho questo menù a tendina -->
                        <label for="genereId">Genere</label>
                        <select id="genereId" formControlName="genereId" [disabled]="!modificabileDa()">
                            <option value="">Seleziona un genere</option>
                            @for (genere of generi(); track tracciaPerId($index.toString(), genere)) {
                            <option [value]="genere.id">{{ genere.genere }}</option>
                            }
                        </select>
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

Utente: Fabio Tammaro
Data: 04/06/2026
Descrizione: Aggiunti h3 per avere più informazioni sul film da modificare

<details><summary> movie-form.component V1.1</summary>

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
                <h2>{{ modificaId() ? 'Modifica film' : 'Aggiungi film' }}</h2>
                <h3>{{movieSelezionato()?.titolo}} - {{movieSelezionato()?.genere}} - {{movieSelezionato()?.durataMinuti }}min</h3>
                 
                <form class="form-grid" [formGroup]="form" (ngSubmit)="invia()">
                    <div>
                        <label for="titolo">Titolo</label>
                        <input id="titolo" type="text" formControlName="titolo" [disabled]="!modificabileDa()">
                        <label for="descrizione">Descrizione</label>
                        <input id="descrizione" type="text" formControlName="descrizione"
                            [disabled]="!modificabileDa()">
                        <label for="durataMinuti">Durata (minuti)</label>
                        <input id="durataMinuti" type="number" formControlName="durataMinuti"
                            [disabled]="!modificabileDa()">
                        <label for="prezzoMovie">Prezzo (€)</label>
                        <input id="prezzoMovie" type="number" formControlName="prezzoMovie"
                            [disabled]="!modificabileDa()">
                        <label for="genereId">Genere</label>
                        <select id="genereId" formControlName="genereId" [disabled]="!modificabileDa()">
                            <option value="">Seleziona un genere</option>
                            @for (genere of generi(); track tracciaPerId($index.toString(), genere)) {
                            <option [value]="genere.id">{{ genere.genere }}</option>
                            }
                        </select>
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

## proiezione
### page
### components
- proiezione-list.component.ts [tutti] Fabio

Utente: Fabio Tammaro
Data: 03/06/2026
Descrizione: Creato il component list per la proiezione e modificato il biglietto service proiezione service, oltre ai modelli di proiezione.
<details>
<summary>proiezione-list.component.ts Versione 1.0</summary>

```typescript
  import { Component, computed, inject, signal } from '@angular/core';
  import { HttpErrorResponse } from '@angular/common/http';
  import { ProiezioneService } from '../../../services/proiezione.service';
  import { AuthService } from '../../../services/auth.service';
  import { Proiezione } from '../../../models/proiezione.model';
  import { BigliettoService } from '../../../services/biglietto.service';
  import { RouterLink } from '@angular/router';
  import { CommonModule } from '@angular/common';
  //import FormsModule per avere la possibilità di scegliere quanti biglietti acquistare.
  import { FormsModule } from '@angular/forms';

  @Component({
    selector: 'proiezione-list',
    standalone: true,
    imports: [RouterLink, CommonModule, FormsModule],
    templateUrl: './proiezione-list.html',
  })
  export class ProiezioneList {

    private readonly proiezioneService = inject(ProiezioneService);
    private readonly authService = inject(AuthService);
    private readonly bigliettoService = inject(BigliettoService);

    // =========================
    // STATE
    // =========================
    readonly listaProiezioni = signal<Proiezione[]>([]);
    readonly staCaricando = signal(false);
    readonly staInviando = signal(false);
    readonly messaggioErrore = signal('');
    readonly messaggioSuccesso = signal('');
    readonly modificaId = signal<string | null>(null);

    readonly isOperatore = computed(() => this.authService.isOperatore());
    readonly isGestore = computed(() => this.authService.isGestore());

    // quantità per proiezione
    quantitaSelezionata: Record<string, number> = {};

    // loading per singola proiezione
    readonly loadingMap = signal<Record<string, boolean>>({});

    constructor() {
      this.ottieniTutto();
    }

    // =========================
    // LOAD LISTA
    // =========================
    ottieniTutto(): void {
      this.staCaricando.set(true);
      this.messaggioErrore.set('');

      this.proiezioneService.ottieniTutto().subscribe({
        next: (data) => {
          this.listaProiezioni.set(data);

          // inizializza quantità a 1 per ogni proiezione
          const init: Record<string, number> = {};
          data.forEach(p => {
            init[p.id] = 1;
          });
          this.quantitaSelezionata = init;
          this.staCaricando.set(false);
        },
        error: (err) => {
          this.staCaricando.set(false);
          this.messaggioErrore.set(
            this.estraiMessaggioErrore(err, 'Errore caricamento proiezioni')
          );
        }
      });
    }

    // =========================
    // ACQUISTO BIGLIETTI
    // =========================
    acquista(proiezioneId: string) {

        const numeroBiglietti = this.quantitaSelezionata[proiezioneId] ?? 1;

      this.loadingMap.update(m => ({
        ...m,
        [proiezioneId]: true
      }));

      this.messaggioErrore.set('');
      this.messaggioSuccesso.set('');

      this.bigliettoService.crea({
        proiezioneId,
        numeroBiglietti
      }).subscribe({
        next: () => {
          this.loadingMap.update(m => ({
            ...m,
            [proiezioneId]: false
          }));

          this.messaggioSuccesso.set('Biglietti acquistati con successo!');
        },
        error: (err) => {
          this.loadingMap.update(m => ({
            ...m,
            [proiezioneId]: false
          }));

          this.messaggioErrore.set(
            this.estraiMessaggioErrore(err, 'Errore creazione biglietto')
          );
        }
      });
    }

    elimina(item: Proiezione): void {
    if (!this.isOperatore()) {
      return;
    }

    const confirmed = confirm(`Eliminare la proiezione\"${item.id}\"?`)

    if (!confirmed) {
      return;
    }

    this.messaggioErrore.set('');
    this.messaggioSuccesso.set('');

    this.proiezioneService.elimina(item.id).subscribe({
      next: () => {
        this.messaggioSuccesso.set('Proiezione eliminata');

        this.ottieniTutto();
      },
      error: (error: unknown) => {
        this.messaggioErrore.set(this.estraiMessaggioErrore(error, 'Eliminazione non riuscita'));

      }
    });
  }

    // =========================
    // ERROR HANDLER
    // =========================
    private estraiMessaggioErrore(error: unknown, fallback: string): string {
      if (error instanceof HttpErrorResponse) {
        return error.error?.message ?? error.error?.messaggio ?? fallback;
      }

      return fallback;
    }
  }
```
</details>

Utente: Fabio Tammaro
Data: 04/06/2026
Descrizione: Aggiunta "proiezioneScelta"
<details>
<summary>proiezione-list.component.ts Versione 1.1</summary>

```typescript
  import { Component, computed, inject, signal } from '@angular/core';
  import { HttpErrorResponse } from '@angular/common/http';
  import { ProiezioneService } from '../../../services/proiezione.service';
  import { AuthService } from '../../../services/auth.service';
  import { Proiezione } from '../../../models/proiezione.model';
  import { BigliettoService } from '../../../services/biglietto.service';
  import { RouterLink } from '@angular/router';
  import { CommonModule } from '@angular/common';
  import { FormsModule } from '@angular/forms';
  // import del proiezione form per averlo nell'html del list component.
  import { ProiezioneFormComponent } from "./proiezione-form.component";


  @Component({
    selector: 'proiezione-list',
    standalone: true,
    imports: [RouterLink, CommonModule, FormsModule],
    templateUrl: './proiezione-list.html',
  })
  export class ProiezioneList {

    private readonly proiezioneService = inject(ProiezioneService);
    private readonly authService = inject(AuthService);
    private readonly bigliettoService = inject(BigliettoService);

    readonly listaProiezioni = signal<Proiezione[]>([]);
    //aggiunta proprietà per la proiezione selezionata
    readonly proiezioneScelta = signal<Proiezione | null>(null);
    readonly staCaricando = signal(false);
    readonly staInviando = signal(false);
    readonly messaggioErrore = signal('');
    readonly messaggioSuccesso = signal('');
    readonly modificaId = signal<string | null>(null);

    readonly isOperatore = computed(() => this.authService.isOperatore());
    readonly isGestore = computed(() => this.authService.isGestore());

    quantitaSelezionata: Record<string, number> = {};

    readonly loadingMap = signal<Record<string, boolean>>({});

    constructor() {
      this.ottieniTutto();
    }

    ottieniTutto(): void {
      this.staCaricando.set(true);
      this.messaggioErrore.set('');

      this.proiezioneService.ottieniTutto().subscribe({
        next: (data) => {
          this.listaProiezioni.set(data);

          const init: Record<string, number> = {};
          data.forEach(p => {
            init[p.id] = 1;
          });
          this.quantitaSelezionata = init;
          this.staCaricando.set(false);
        },
        error: (err) => {
          this.staCaricando.set(false);
          this.messaggioErrore.set(
            this.estraiMessaggioErrore(err, 'Errore caricamento proiezioni')
          );
        }
      });
    }

    acquista(proiezioneId: string) {

        const numeroBiglietti = this.quantitaSelezionata[proiezioneId] ?? 1;

      this.loadingMap.update(m => ({
        ...m,
        [proiezioneId]: true
      }));

      this.messaggioErrore.set('');
      this.messaggioSuccesso.set('');

      this.bigliettoService.crea({
        proiezioneId,
        numeroBiglietti
      }).subscribe({
        next: () => {
          this.loadingMap.update(m => ({
            ...m,
            [proiezioneId]: false
          }));

          this.messaggioSuccesso.set('Biglietti acquistati con successo!');
        },
        error: (err) => {
          this.loadingMap.update(m => ({
            ...m,
            [proiezioneId]: false
          }));

          this.messaggioErrore.set(
            this.estraiMessaggioErrore(err, 'Errore creazione biglietto')
          );
        }
      });
    }

    elimina(item: Proiezione): void {
    if (!this.isOperatore()) {
      return;
    }

    const confirmed = confirm(`Eliminare la proiezione\"${item.id}\"?`)

    if (!confirmed) {
      return;
    }

    this.messaggioErrore.set('');
    this.messaggioSuccesso.set('');

    this.proiezioneService.elimina(item.id).subscribe({
      next: () => {
        this.messaggioSuccesso.set('Proiezione eliminata');

        this.ottieniTutto();
      },
      error: (error: unknown) => {
        this.messaggioErrore.set(this.estraiMessaggioErrore(error, 'Eliminazione non riuscita'));

      }
    });
  }

    private estraiMessaggioErrore(error: unknown, fallback: string): string {
      if (error instanceof HttpErrorResponse) {
        return error.error?.message ?? error.error?.messaggio ?? fallback;
      }

      return fallback;
    }
  }
```
</details>

<details>
<summary>proiezione-list.component.html Versione 1.1</summary>

```html
<section>
    <h1 class="page-title">Proiezioni</h1>
    @if(!isOperatore()){
    <p class="page-subtitle">
        Visualizza le proiezioni del cinema e acquista i biglietti.
    </p>
    }
    @else{
    <p class="page-subtitle">
        Visualizza e gestisci le proiezioni del cinema.
    </p>
    }

    @if (messaggioErrore()) {
    <div class="alert alert-warning">
        {{ messaggioErrore() }}
    </div>
    }

    @if (messaggioSuccesso()) {
    <div class="alert alert-success">
        {{ messaggioSuccesso() }}
    </div>
    }

    <div class="grid grid-2">

        <article class="card">
            <h2>Lista Proiezioni</h2>

            @if (staCaricando()) {
            <p class="muted">Caricamento in corso...</p>

            } @else if (listaProiezioni().length === 0) {

            <p class="muted">Nessuna proiezione disponibile.</p>

            } @else {

            <div class="list">

                @for (item of listaProiezioni(); track item.id) {

                <div class="list-item">

                    <div>
                        <strong>{{ item.titoloMovie }}</strong>

                        <p>{{ item.dataProiezione | date:'dd/MM/yyyy' }}</p>
                        <p>Sala: {{ item.nomeSala }}</p>
                        <p>Turno: {{ item.nomeTurno }}</p>
                    </div>
                    @if(!isGestore() && !isOperatore()){

                    <div class="btn-row">

                        <input type="number" min="1" class="form-control" [(ngModel)]="quantitaSelezionata[item.id]" />

                        <button class="btn btn-secondary" type="button" (click)="acquista(item.id)">
                            Acquista
                        </button>

                    </div>
                    }
                    @if(isOperatore()){
                                <div class="btn-row">
                                    <button class="btn btn-secondary" type="button" (click)="elimina(item)"> Elimina</button>
                                </div>
                            }
                </div>

                }

            </div>

            }

        </article>
        @if (isOperatore()) {
        <article class="card">
            <proiezione-form [proiezioneSelezionata]="proiezioneScelta()" (modificaCompletata)="ottieniTutto()"></proiezione-form>
        </article>
        }
    </div>

</section>
```
</details>


- proiezione-form.component.ts [operatore]

Utente: Fabio Tammaro
Data: 04/06/2026
Descrizione: aggiunta l'importazione di proiezione-form che ha validità solo per la creazione
<details>
<summary>proiezione-form.component.ts Versione 1.1</summary>

```ts

// import di input ed effect per lavorare con il modulo di modifica/creazione.
import { Component, inject, signal, input, effect } from '@angular/core';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { HttpErrorResponse } from '@angular/common/http';
import { AuthService } from '../../../services/auth.service';
import { ProiezioneService } from '../../../services/proiezione.service';
import { MovieService } from '../../../services/movie.service';
import { SalaService } from '../../../services/sala.service';
import { TurnoService } from '../../../services/turno.service';
import { Movie } from '../../../models/movie.model';
import { Sala } from '../../../models/sala.model';
import { Turno } from '../../../models/turno.model';
import { Proiezione } from '../../../models/proiezione.model';



@Component({
    selector: 'proiezione-form',
    standalone: true,
    imports: [ReactiveFormsModule],
    templateUrl: './proiezione-form.component.html'
})

export class ProiezioneFormComponent {

    private readonly formBuilder = inject(FormBuilder);
    private readonly authService = inject(AuthService);
    private readonly movieService = inject(MovieService);
    private readonly salaService = inject(SalaService);
    private readonly turnoService = inject(TurnoService);
    private readonly proiezioneService = inject(ProiezioneService);


    readonly movies = signal<Movie[]>([]);
    readonly sale = signal<Sala[]>([]);
    readonly turni = signal<Turno[]>([]);
    readonly proiezioneSelezionata = input<Proiezione | null>(null);
    readonly staCaricando = signal(false);
    readonly staInviando = signal(false);
    readonly messaggioErrore = signal('');
    readonly messaggioSuccesso = signal('');
    readonly modificaId = signal<string>('');

    //costruzione del form
    readonly form = this.formBuilder.nonNullable.group({
        dataProiezione: ['', [Validators.required]],
        movieId: ['', [Validators.required]],
        salaId: ['', [Validators.required]],
        turnoId: ['', [Validators.required]],
    });

    constructor() {
        this.caricaMovies();
        this.caricaSale();
        this.caricaTurni();

        effect(() => {
            const proiezione = this.proiezioneSelezionata();
            if (proiezione) {
                this.inizioModifica(proiezione);
            } else {
                this.ripristinaForm();
            }
        });
    }
    modificabileDa(): boolean {
        return this.authService.possiedeQualsiasiRuolo(['Operatore']);
    }

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
                this.messaggioErrore.set(this.estraiMessaggioErrore(error, 'Film non trovati'));
            }
        });
    }

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
                this.messaggioErrore.set(this.estraiMessaggioErrore(error, 'Sale non trovate'));
            }
        });
    }

    caricaTurni(): void {

        this.staCaricando.set(true);
        this.messaggioErrore.set('');
        this.turnoService.ottieniTutto().subscribe({
            next: (items) => {
                this.turni.set(items);
                this.staCaricando.set(false);
            },
            error: (error: unknown) => {
                this.staCaricando.set(false);
                this.messaggioErrore.set(this.estraiMessaggioErrore(error, 'Turni non trovati'));
            }
        });
    }

    invia(): void {

        if (this.form.invalid || !this.modificabileDa()) {
            this.form.markAllAsTouched();
            return;
        }

        this.staInviando.set(true);
        this.messaggioErrore.set('');
        this.messaggioSuccesso.set('');


        const request$ = this.modificaId()

            ? this.proiezioneService.modifica(this.modificaId(), this.form.getRawValue())
            : this.proiezioneService.crea(this.form.getRawValue());


        request$.subscribe({
            next: () => {
                this.staInviando.set(false);
                this.messaggioSuccesso.set(this.modificaId() ? 'Proiezione aggiornata.' : 'Proiezione creata.');
                this.ripristinaForm();
            },
            error: (error: unknown) => {
                this.staInviando.set(false);
                this.messaggioErrore.set(this.estraiMessaggioErrore(error, 'Operazione non riuscita.'));
            }
        });
    }

    inizioModifica(item: Proiezione): void {

        if (!this.modificabileDa()) {
            return;
        }
        this.modificaId.set(item.id);

        this.form.patchValue({ dataProiezione: item.dataProiezione, movieId: item.titoloMovie, salaId: item.nomeSala, turnoId:item.nomeTurno });
        this.messaggioErrore.set('');
        this.messaggioSuccesso.set('');
    }

    ripristinaForm(): void {
        this.modificaId.set('');
        this.form.reset({ dataProiezione: '', movieId: '', salaId: '', turnoId: '' });
    }

    // implementazione per il recupero delle entità richiamate in proiezione.
    tracciaPerId(_: string, item: Proiezione | Movie | Sala | Turno): string {
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

Utente: Fabio Tammaro
Data: 04/06/2026
Descrizione: aggiunta l'importazione di proiezione-form.html che ha validità solo per la creazione

<details>
<summary>proiezione-form.component.html Versione 1.1</summary>

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
                <h2>{{ modificaId() ? 'Modifica proiezione' : 'Aggiungi proiezione' }}</h2>
                <h3>{{proiezioneSelezionata()?.titoloMovie}} - {{proiezioneSelezionata()?.dataProiezione}} - {{proiezioneSelezionata()?.nomeTurno }}</h3>
                <form class="form-grid" [formGroup]="form" (ngSubmit)="invia()">
                    <div>
                        <label for="dataProiezione">Data Proiezione</label>
                        <input id="dataProiezione" type="date" formControlName="dataProiezione" [disabled]="!modificabileDa()">
                        <label for="salaId">Sala</label>
                        <select id="genereId" formControlName="genereId" [disabled]="!modificabileDa()">
                            <option value="">Seleziona una sala</option>
                            @for (sala of sale(); track tracciaPerId($index.toString(), sala)) {
                            <option [value]="sala.id">{{ sala.nome }}</option>
                            }
                        </select>
                        <label for="movieId">Film</label>
                        <select id="movieId" formControlName="movieId" [disabled]="!modificabileDa()">
                            <option value="">Seleziona un film</option>
                            @for (movie of movies(); track tracciaPerId($index.toString(), movie)) {
                            <option [value]="movie.id">{{ movie.titolo }}</option>
                            }
                        </select>
                        <label for="turnoId">Turno</label>
                        <select id="turnoId" formControlName="turnoId" [disabled]="!modificabileDa()">
                            <option value="">Seleziona un turno</option>
                            @for (turno of turni(); track tracciaPerId($index.toString(), turno)) {
                            <option [value]="turno.id">{{ turno.nome }}</option>
                            }
                        </select>
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


- proiezione-detail.component.ts [tutti]

<details>
<summary>versione1.1</summary>

Francesco Lorenzi 03/06/2026

ho dovuto modificare la lista per poter integrare il form in essa, la lista è parente del form, che è quindi suo figlio. 

## sala-list.component.ts
```ts
import { Component, inject, signal } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
// servizi
import { AuthService } from '../../../services/auth.service';
import { SalaService } from '../../../services/sala.service';

// modelli
import { Sala } from '../../../models/sala.model';
// importo del componente form
import { SalaFormComponent } from './sala-form.component';


@Component({
  selector: 'sala-list',
  standalone: true,
  templateUrl: './sala-list.component.html',
  imports: [SalaFormComponent]//importo del componente form
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
  // aggiunta funzione di eliminazione 
  elimina(item: Sala): void {
    if (!this.visualizzabileDa()) {
      return;
    }
    // manda un popup di conferma dell'azione
    if (!confirm(`Sei sicuro di voler eliminare la sala "${item.nome}"?`)) {
      return;
    }

    this.staInviando.set(true);
    this.salaService.elimina(item.id).subscribe({
      next: () => {
        this.staInviando.set(false);
        this.messaggioSuccesso.set('Sala eliminata con successo');
        this.caricaSale();
      },
      error: (error: unknown) => {
        this.staInviando.set(false);
        this.messaggioErrore.set(this.estraiMessaggioErrore(error, 'Errore durante l\'eliminazione della sala'));
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
                        <!-- aggiunta dei bottoni di elimina e di modifica visibili solo dall'operatore -->
                        @if(visualizzabileDa()) {
                        <div class="btn-row" style="margin-top: 1rem;">
                            <button (click)="salaScelta.set(item)">Modifica</button>
                            <button (click)="elimina(item)" [disabled]="staInviando()">Elimina</button>
                        </div>
                    }
                    </div>    
                </div>
                }
            </div>
            }
        </article>
        <!-- aggiunta del form visibile solo dall'operatore -->
         @if (visualizzabileDa()){
        <article class="card">
            <sala-form [salaSelezionata]="salaScelta()" (modificaCompletata)="caricaSale()"></sala-form>
        </article>
        }
```
</details>

- sala-form.component.ts [operatore] Francesco
<details>
<summary>versione1.0</summary>
Francesco Lorenzi 03/06/2026

## sala-form.component.ts
```ts
//alla chiamata possimao ignettare imput ed effect 
import { Component, inject, signal, input, effect } from '@angular/core';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { HttpErrorResponse } from '@angular/common/http';

import { AuthService } from '../../../services/auth.service';
import { SalaService } from '../../../services/sala.service';
import { TipologiaSalaService } from '../../../services/tipologia-sala.service';


import { Sala } from '../../../models/sala.model';
import { TipologiaSala } from '../../../models/tipologia-sala.model';


@Component({
    selector: 'sala-form',
    standalone: true,
    imports: [ReactiveFormsModule],
    templateUrl: './sala-form.component.html'
})

export class SalaFormComponent {

    private readonly formBuilder = inject(FormBuilder);
    private readonly authService = inject(AuthService);
    private readonly salaService = inject(SalaService);
    private readonly tipologiaSalaService = inject(TipologiaSalaService);

    readonly salaSelezionata = input<Sala | null>(null);// la sala selezionata che ci passiamo dalla lista padre
    readonly tipologie = signal<TipologiaSala[]>([]);
    readonly staCaricando = signal(false);
    readonly staInviando = signal(false);
    readonly messaggioErrore = signal('');
    readonly messaggioSuccesso = signal('');
    readonly modificaId = signal<string>('');


    readonly form = this.formBuilder.nonNullable.group({
        nome: ['', [Validators.required, Validators.maxLength(100)]],
        capienza: [1, [Validators.required, Validators.min(1)]],
        tipologiaSalaId: ['', [Validators.required]]
    });

    constructor() {
        this.caricaTipologie();
        // la funzione effect prende l'input, che è la sala che si selezionata, e riempe il form dinamicamente
        effect(() => {
            const sala = this.salaSelezionata();
            if (sala) {
                this.inizioModifica(sala);
            } else {
                this.ripristinaForm();
            }
        });
    }
    modificabileDa(): boolean {
        return this.authService.possiedeQualsiasiRuolo(['Operatore']);
    }
    // carico tutte le tipologie così da poterle scegliere nel menù a tendina
    caricaTipologie(): void {

        this.staCaricando.set(true);
        this.messaggioErrore.set('');
        this.tipologiaSalaService.ottieniTutto().subscribe({
            next: (tipologie) => {
                this.tipologie.set(tipologie);
                this.staCaricando.set(false);
            },
            error: (error) => {
                this.staCaricando.set(false);
                this.messaggioErrore.set(this.estraiMessaggioErrore(error, 'Errore nel caricamento delle tipologie.'));
            }
        });
    }
    // funzione del bottone di modifica o di creazione 
    invia(): void {

        if (this.form.invalid || !this.modificabileDa()) {
            this.form.markAllAsTouched();
            return;
        }

        this.staInviando.set(true);
        this.messaggioErrore.set('');
        this.messaggioSuccesso.set('');


        const request$ = this.modificaId()

            ? this.salaService.modifica(this.modificaId(), this.form.getRawValue())
            : this.salaService.crea(this.form.getRawValue());


        request$.subscribe({
            next: () => {
                this.staInviando.set(false);
                this.messaggioSuccesso.set(this.modificaId() ? 'Sala aggiornata.' : 'Sala creata.');
                this.ripristinaForm();
            },
            error: (error: unknown) => {
                this.staInviando.set(false);
                this.messaggioErrore.set(this.estraiMessaggioErrore(error, 'Operazione non riuscita.'));
            }
        });
    }
    // prende la sala selezionata e riempe il form 
    inizioModifica(item: Sala): void {

        if (!this.modificabileDa()) {
            return;
        }
        this.modificaId.set(item.id);

        this.form.patchValue({ nome: item.nome, capienza: item.capienza, tipologiaSalaId: item.tipologiaSalaId });
        this.messaggioErrore.set('');
        this.messaggioSuccesso.set('');
    }
    // svuota il form
    ripristinaForm(): void {
        this.modificaId.set('');
        this.form.reset({ nome: '', capienza: 0, tipologiaSalaId: 'scegli una tipologia' });
    }
    // traccia per id per la lista del menù a tendina
    tracciaPerId(_: string, item: Sala | TipologiaSala): string {
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
## sala-form.component.html
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
                <h2>{{ modificaId() ? 'Modifica sala' : 'Aggiungi sala' }}</h2>

                <form class="form-grid" [formGroup]="form" (ngSubmit)="invia()">
                    <div>
                        <label for="nome">Nome</label>
                        <input id="nome" type="text" formControlName="nome" [disabled]="!modificabileDa()">
                        <label for="capienza">Capienza</label>
                        <input id="capienza" type="number" formControlName="capienza" [disabled]="!modificabileDa()">
                        <label for="tipologiaSalaId">Tipologia</label>
                        <select id="tipologiaSalaId" formControlName="tipologiaSalaId" [disabled]="!modificabileDa()">
                            <option value="">Seleziona una tipologia</option>
                            @for (tipologia of tipologie(); track tracciaPerId($index.toString(), tipologia)) {
                            <option [value]="tipologia.id">{{ tipologia.nome }}</option>
                            }
                        </select>
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

## tipologia-sala

### components

- tipologia-sala-list.component.ts [operatore] Greg

Utente: Greg

Data: 04/06/2026

```ts
import { Component, inject, signal } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';

import { AuthService } from '../../../services/auth.service';
import { TipologiaSalaService } from '../../../services/tipologia-sala.service';
import { TipologiaSala } from '../../../models/tipologia-sala.model';
import { TipologiaSalaFormComponent } from "./tipologia-sala-form.component";

@Component({
  selector: 'tipologia-sala-list',
  standalone: true,
  templateUrl: './tipologia-sala-list.component.html',
  imports: [TipologiaSalaFormComponent]
})
export class TipologiaSalaList {
  private readonly authService = inject(AuthService);
  private readonly tipologiaSalaService = inject(TipologiaSalaService);

  readonly tipologie = signal<TipologiaSala[]>([]);
  readonly tipologiaScelta = signal<TipologiaSala | null>(null);
  readonly staCaricando = signal(false);
  readonly staInviando = signal(false);
  readonly messaggioErrore = signal('');
  readonly messaggioSuccesso = signal('');

  constructor() {
    this.caricaTipologie();
  }

  visualizzabileDa(): boolean {
    return this.authService.possiedeQualsiasiRuolo(['Operatore']);
  }

  caricaTipologie(): void {
    this.staCaricando.set(true);
    this.messaggioErrore.set('');
    this.tipologiaSalaService.ottieniTutto().subscribe({
      next: (items) => {
        this.tipologie.set(items);
        this.staCaricando.set(false);
      },
      error: (error: unknown) => {
        this.staCaricando.set(false);
        this.messaggioErrore.set(this.estraiMessaggioErrore(error, 'tipologie sala non trovate'));
      }
    });
  }

  elimina(item: TipologiaSala): void {
    if (!this.visualizzabileDa()) {
      return;
    }
    const confirmed = confirm(`Eliminare la tipologia sala \"${item.nome}\"?`);
    if (!confirmed) {
      return;
    }

    this.messaggioErrore.set('');
    this.messaggioSuccesso.set('');

    this.tipologiaSalaService.elimina(item.id, item).subscribe({
      next: () => {
        if (this.tipologiaScelta()?.id === item.id) {
          this.tipologiaScelta.set(null);
        } 
        this.caricaTipologie();
      },
      error: (error: unknown) => {
        this.messaggioErrore.set(this.estraiMessaggioErrore(error, 'eliminazione non riuscita.'));
      }
    });
  }

  tracciaPerId(_: string, item: TipologiaSala): string {
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

Utente: Greg

Data: 04/06/2026

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
            <h2>Lista Tipologie Sala</h2>

            @if (staCaricando()) {
            <p class="muted">Caricamento in corso...</p>
            } @else if (tipologie().length === 0) {
            <p class="muted">Nessuna tipologia presente.</p>
            } @else {
            <div class="list">
                @for (item of tipologie(); track tracciaPerId($index.toString(), item)) {
                <div class="list-item">
                    <div>
                        <strong>{{ item.nome }}</strong>
                        <p>Maggiorazione: {{ item.maggiorazionePrezzo }} €</p>
                        <div class="muted">ID: {{ item.id }}</div>
                        @if(visualizzabileDa()) {
                        <div class="btn-row" style="margin-top: 1rem;">
                            <button (click)="tipologiaScelta.set(item)">Modifica</button>
                            <button (click)="elimina(item)" [disabled]="staInviando()">Elimina</button>
                        </div>
                        }
                    </div>
                </div>
                }
            </div>
            }
        </article>

        @if (visualizzabileDa()) {
        <article class="card">
            <tipologia-sala-form [movieSelezionato]="tipologiaScelta()" (modificaCompletata)="caricaTipologie()"></tipologia-sala-form>
        </article>
        }
    </div>
</section>
```

- tipologia-sala-form.component.ts [operatore] Greg

Utente: Greg

Data: 04/06/2026

```ts
import { Component, inject, signal, input, output, effect } from '@angular/core';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { HttpErrorResponse } from '@angular/common/http';

import { AuthService } from '../../../services/auth.service';
import { TipologiaSalaService } from '../../../services/tipologia-sala.service';
import { TipologiaSala } from '../../../models/tipologia-sala.model';

@Component({
    selector: 'tipologia-sala-form',
    standalone: true,
    imports: [ReactiveFormsModule],
    templateUrl: './tipologia-sala-form.component.html'
})
export class TipologiaSalaFormComponent {
    private readonly formBuilder = inject(FormBuilder);
    private readonly authService = inject(AuthService);
    private readonly tipologiaSalaService = inject(TipologiaSalaService);

    readonly tipologiaSelezionata = input<TipologiaSala | null>(null, { alias: 'movieSelezionato' });
    readonly modificaCompletata = output<void>();

    readonly staCaricando = signal(false);
    readonly staInviando = signal(false);
    readonly messaggioErrore = signal('');
    readonly messaggioSuccesso = signal('');
    readonly modificaId = signal<string>('');

    readonly form = this.formBuilder.nonNullable.group({
        nome: ['', [Validators.required, Validators.maxLength(100)]],
        maggiorazionePrezzo: [1, [Validators.required, Validators.min(1)]],
    });

    constructor() {
        effect(() => {
            const tipologia = this.tipologiaSelezionata();
            if (tipologia) {
                this.inizioModifica(tipologia);
            } else {
                this.ripristinaForm();
            }
        });
    }

    modificabileDa(): boolean {
        return this.authService.possiedeQualsiasiRuolo(['Operatore']);
    }

    invia(): void {
        if (this.form.invalid || !this.modificabileDa()) {
            this.form.markAllAsTouched();
            return;
        }

        this.staInviando.set(true);
        this.messaggioErrore.set('');
        this.messaggioSuccesso.set('');

        const request$ = this.modificaId()
            ? this.tipologiaSalaService.modifica(this.modificaId(), this.form.getRawValue())
            : this.tipologiaSalaService.crea(this.form.getRawValue());

        request$.subscribe({
            next: () => {
                this.staInviando.set(false);
                this.messaggioSuccesso.set(this.modificaId() ? 'Tipologia sala aggiornata.' : 'Tipologia sala creata.');
                this.modificaCompletata.emit();
                this.ripristinaForm();
            },
            error: (error: unknown) => {
                this.staInviando.set(false);
                this.messaggioErrore.set(this.estraiMessaggioErrore(error, 'Operazione non riuscita.'));
            }
        });
    }

    inizioModifica(item: TipologiaSala): void {
        if (!this.modificabileDa()) {
            return;
        }
        this.modificaId.set(item.id);
        this.form.patchValue({ nome: item.nome, maggiorazionePrezzo: item.maggiorazionePrezzo });
        this.messaggioErrore.set('');
        this.messaggioSuccesso.set('');
    }

    ripristinaForm(): void {
        this.modificaId.set('');
        this.form.reset({ nome: '', maggiorazionePrezzo: 1 });
    }

    tracciaPerId(_: string, item: TipologiaSala): string {
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

Utente: Greg

Data: 04/06/2026

```html
<h2>{{ modificaId() ? 'Modifica tipologia' : 'Aggiungi tipologia' }}</h2>

<form class="form-grid" [formGroup]="form" (ngSubmit)="invia()">
    <div>
        <label for="nome">Nome</label>
        <input id="nome" type="text" formControlName="nome" [disabled]="!modificabileDa()">
        
        <label for="maggiorazionePrezzo">Maggiorazione Prezzo (€)</label>
        <input id="maggiorazionePrezzo" type="number" formControlName="maggiorazionePrezzo" [disabled]="!modificabileDa()">
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
```

## turno
### components
- turno-list.component.ts [operatore] Andrea paris (fatto)
- turno-form.component.ts [operatore] Andrea paris (fatto)

## cambio-ruolo
### components
- cambio-ruolo-form.component.ts [gestore] (Lorenzo)
<details>
<summary>versione1.0</summary>

- Utente: Lorenzo Laviosa
- Data: 5/06/2026
## cambio-ruolo-form.component.ts
```ts
// Component Angular standalone per la pagina di cambio ruolo
import { Component, inject, signal } from '@angular/core';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { HttpErrorResponse } from '@angular/common/http';
import { GestioneUtenteService } from '../../../services/gestione-utente.service';

@Component({
  selector: 'app-cambio-ruolo-form',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './cambio-ruolo-form.component.html'
})
export class CambioRuoloFormComponent {

  // Iniettiamo FormBuilder per creare il form
  private readonly fb = inject(FormBuilder);

  // Iniettiamo il servizio che chiama il backend
  private readonly gestioneUtenteService = inject(GestioneUtenteService);

  // Stati reattivi per UI (loading, messaggi)
  readonly isSubmitting = signal(false);
  readonly successMessage = signal('');
  readonly errorMessage = signal('');

  // Lista ruoli mostrata nel select
  readonly roles = ['Gestore', 'Operatore', 'Utente'];

  // Form reattivo con validazioni
  readonly cambiaRuoloForm = this.fb.nonNullable.group({
    email: ['', [Validators.required, Validators.email]],   // email obbligatoria e valida
    nuovoRuolo: ['Utente', [Validators.required]]           // ruolo obbligatorio
  });

  // Metodo chiamato al submit del form
  cambiaRuolo(): void {

    // Se il form è invalido, mostriamo gli errori
    if (this.cambiaRuoloForm.invalid) {
      this.cambiaRuoloForm.markAllAsTouched();
      return;
    }

    // Reset messaggi e attiviamo loading
    this.isSubmitting.set(true);
    this.successMessage.set('');
    this.errorMessage.set('');

    // Chiamata al backend
    this.gestioneUtenteService
      .modificaRuolo(this.cambiaRuoloForm.getRawValue())
      .subscribe({

        // Risposta OK
        next: (response) => {
          this.isSubmitting.set(false);
          this.successMessage.set(
            `${response.messaggio} Nuovo ruolo: ${response.ruolo}`
          );
        },

        // Errore backend o rete
        error: (error: unknown) => {
          this.isSubmitting.set(false);
          this.errorMessage.set(this.extractErrorMessage(error));
        }
      });
  }

  // Estrae un messaggio leggibile dall'errore HTTP
  private extractErrorMessage(error: unknown): string {

    if (error instanceof HttpErrorResponse) {
      return error.error?.messaggio ?? 'Cambio ruolo non riuscito';
    }

    return 'Cambio ruolo non riuscito';
  }
}
```
- cambio-ruolo-form.component.html
<details>
<summary>versione1.0</summary>

- Utente: Lorenzo Laviosa
- Data: 5/06/2026
## cambio-ruolo-form.component.html

```html
<section>

    <!-- TITOLO PAGINA -->
    <h1 class="page-title">
        Cambio ruolo utente
    </h1>

    <!-- SOTTOTITOLO -->
    <p class="page-subtitle">
        Pagina visibile solo al ruolo
        <strong>Operatore</strong>
    </p>

    <div class="grid grid-2">

        <!-- CARD FORM PRINCIPALE -->
        <article class="card">

            <!-- Messaggio di successo -->
            @if (successMessage()) {
                <div class="alert alert-success">
                    {{ successMessage() }}
                </div>
            }

            <!-- Messaggio di errore -->
            @if (errorMessage()) {
                <div class="alert alert-warning">
                    {{ errorMessage() }}
                </div>
            }

            <!-- FORM CAMBIO RUOLO -->
            <form
                class="form-grid"
                [formGroup]="cambiaRuoloForm"
                (ngSubmit)="cambiaRuolo()"
            >

                <!-- CAMPO EMAIL -->
                <div>

                    <label for="email">Email utente</label>

                    <input
                        id="email"
                        type="email"
                        formControlName="email"
                    />

                    <!-- Errori di validazione email -->
                    @if (cambiaRuoloForm.controls.email.touched && cambiaRuoloForm.controls.email.invalid) {

                        <div class="field-error">

                            @if (cambiaRuoloForm.controls.email.errors?.['required']) {
                                <small>L'email è obbligatoria</small>
                            }

                            @if (cambiaRuoloForm.controls.email.errors?.['email']) {
                                <small>Inserisci un'email valida</small>
                            }

                        </div>
                    }

                </div>

                <!-- SELECT RUOLO -->
                <div>

                    <label for="nuovoRuolo">Nuovo ruolo</label>

                    <select
                        id="nuovoRuolo"
                        formControlName="nuovoRuolo"
                    >
                        <!-- Ciclo Angular @for: mostra i ruoli -->
                        @for (role of roles; track role) {
                            <option [value]="role">
                                {{ role }}
                            </option>
                        }
                    </select>

                </div>

                <!-- BOTTONE SUBMIT -->
                <div class="btn-row">

                    <button
                        class="btn btn-primary"
                        type="submit"
                        [disabled]="isSubmitting()"
                    >
                        {{ isSubmitting() ? 'Aggiornamento...' : 'Aggiorna ruolo' }}
                    </button>

                </div>

            </form>

        </article>

        <!-- CARD LATERALE: LISTA RUOLI -->
        <article class="card">

            <h2>Ruoli disponibili</h2>

            <div class="list">

                <!-- Lista ruoli mostrata a destra -->
                @for (role of roles; track role) {

                    <div class="list-item">
                        <span>Ruolo</span>
                        <strong>{{ role }}</strong>
                    </div>

                }

            </div>

        </article>

    </div>

</section>
```


- utenti-list.component.ts [gestore] Andrea Bruno (fatto)

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

## biglietto
### biglietto-list.component.ts

<details>
<summary>## biglietto-list.component.ts V1.0</summary>

Andrea Bruno 03-06-2026
Creazione del file

```ts
import { Component, inject, signal } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';

import { Biglietto } from '../../models/biglietto.model';
import { BigliettoService } from '../../services/biglietto.service';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'biglietto-list',
  standalone: true,
  templateUrl: './biglietto-list.component.html',
})
export class BigliettoListComponent {

  // Servizi necessari
  private readonly authService = inject(AuthService);
  private readonly bigliettoService = inject(BigliettoService);

  // Stato reattivo del componente
  readonly biglietti = signal<Biglietto[]>([]);
  readonly staCaricando = signal(false);
  readonly staInviando = signal(false);
  readonly messaggioErrore = signal('');
  readonly messaggioSuccesso = signal('');

  // ID dell'utente loggato
  readonly utenteId;

  constructor() {
    // Recupera l'utente corrente
    const utente = this.authService.utenteCorrente();

    // Se non loggato, mostra errore
    if (!utente?.id) {
      this.messaggioErrore.set("Utente non loggato");
      return;
    }

    this.utenteId = utente.id;

    // Carica i biglietti all'avvio
    this.caricaBiglietti();
  }

  // Recupera i biglietti dell'utente
  caricaBiglietti(): void {
    if (!this.utenteId) return;

    this.staCaricando.set(true);
    this.messaggioErrore.set('');

    this.bigliettoService.ottieniTutto().subscribe({
      next: (items) => {
        this.biglietti.set(items);
        this.staCaricando.set(false);
      },
      error: (error: unknown) => {
        this.staCaricando.set(false);
        this.messaggioErrore.set(
          this.estraiMessaggioErrore(error, 
            'Errore durante il caricamento dei biglietti')
        );
      }
    });
  }

  // Funzione di tracking per *ngFor
  tracciaPerId(_: string, item: Biglietto): string {
    return item.id;
  }

  // Elimina un biglietto
  elimina(id: string): void {
    if (!confirm("Sei sicuro di voler eliminare questo biglietto?")) return;

    this.staInviando.set(true);
    this.messaggioErrore.set('');
    this.messaggioSuccesso.set('');

    this.bigliettoService.elimina(id).subscribe({
      next: () => {
        // Rimuove il biglietto dalla lista locale
        this.biglietti.update(lista => lista.filter(b => b.id !== id));
        this.staInviando.set(false);
        this.messaggioSuccesso.set("Biglietto eliminato con successo");
      },
      error: (error: unknown) => {
        this.staInviando.set(false);
        this.messaggioErrore.set(
          this.estraiMessaggioErrore(error, "Errore durante l'eliminazione del biglietto")
        );
      }
    });
  }

  // Placeholder per futura modifica
  modifica(id: string) {
  }

  // Estrae un messaggio leggibile dall'errore HTTP
  private estraiMessaggioErrore(error: unknown, fallback: string): string {
    if (error instanceof HttpErrorResponse) {
      return error.error?.message ?? fallback;
    }
    return fallback;
  }
}
```

### biglietto-list.component.html

<details>
<summary>biglietto-list.component.html V1.0</summary>

Andrea Bruno 03-06-2026
Creazione del file minimale esteticamente da modificare 

```html
<section>

    <!-- Messaggio di errore -->
    @if (messaggioErrore()) {
        <div class="alert alert-warning">{{ messaggioErrore() }}</div>
    }

    <!-- Messaggio di successo -->
    @if (messaggioSuccesso()) {
        <div class="alert alert-success">{{ messaggioSuccesso() }}</div>
    }

    <div class="grid grid-2">

        <article class="card">
            <h2>I tuoi biglietti</h2>

            <!-- Stato di caricamento -->
            @if (staCaricando()) {
                <p class="muted">Caricamento in corso...</p>

            <!-- Nessun biglietto -->
            } @else if (biglietti().length === 0) {
                <p class="muted">Non hai ancora acquistato nessun biglietto.</p>

            <!-- Lista biglietti -->
            } @else {

                <div class="list">

                    <!-- Ciclo dei biglietti -->
                    @for (b of biglietti(); track b.id){

                        <div class="list-item">
                            <div>

                                <!-- Intestazione biglietto -->
                                <strong>Biglietto #{{ b.id }}</strong>

                                <!-- Informazioni principali -->
                                <p>Film: {{ b.titoloMovie }}</p>
                                <p>Sala: {{ b.nomeSala }}</p>
                                <p>Tipologia sala: {{ b.nomeTipologiaSala }}</p>
                                <p>Numero biglietti: {{ b.numeroBiglietti }}</p>
                                <p>Data: {{ b.dataProiezione }}</p>
                                <p>Ora inizio: {{ b.oraInizio }}</p>
                                <p>Prezzo totale: {{ b.prezzoFinale }} €</p>

                                <!-- ID tecnico -->
                                <div class="muted">ID: {{ b.id }}</div>

                                <!-- Pulsanti azione -->
                                <div class="btn-row">
                                    <button class="btn btn-primary" (click)="modifica(b.id)">
                                        Modifica
                                    </button>

                                    <button class="btn btn-danger"
                                            (click)="elimina(b.id)"
                                            [disabled]="staInviando()">
                                        Elimina
                                    </button>
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

## cambio-ruolo
### utenti-list.component.ts

<details>
<summary>utenti-list.component.ts V1.0</summary>

Andrea Bruno 03-06-2026
Creazione del file

```ts
import { Component, inject, signal } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';

import { OperatoreService } from '../../services/operatore.service';
import { UtenteService } from '../../services/utente.service';
import { AuthService } from '../../services/auth.service';
import { Utente } from '../../models/utente.model';

@Component({
  selector: 'utenti-list',
  standalone: true,
  templateUrl: './utente-list.component.html'
})
export class UtenteListComponent {

  // Servizi necessari
  private readonly authService = inject(AuthService);
  private readonly operatoreService = inject(OperatoreService);
  private readonly utenteService = inject(UtenteService);

  // Stato reattivo del componente
  readonly utenti = signal<Utente[]>([]);
  readonly staCaricando = signal(false);
  readonly staInviando = signal(false);
  readonly messaggioErrore = signal('');
  readonly messaggioSuccesso = signal('');

  constructor() {
    // Carica gli utenti all'avvio
    this.caricaUtenti();
  }

  // Controlla se l'utente corrente può vedere questa pagina
  visualizzabileDa(): boolean {
    return this.authService.possiedeQualsiasiRuolo(['Operatore']);
  }

  // Recupera la lista degli utenti
  caricaUtenti(): void {
    this.staCaricando.set(true);
    this.messaggioErrore.set('');

    this.operatoreService.OttieniUtenti().subscribe({
      next: (items) => {
        this.utenti.set(items);
        this.staCaricando.set(false);
      },
      error: (error: unknown) => {
        this.staCaricando.set(false);
        this.messaggioErrore.set(
          this.estraiMessaggioErrore(error, 
            'Errore durante il caricamento degli utenti')
        );
      }
    });
  }

  // Tracking per *ngFor
  tracciaPerId(_: string, item: Utente): string {
    return item.id;
  }

  // Elimina un utente
  elimina(id: string): void {
    if (!confirm("Sei sicuro di voler eliminare questo utente?")) return;

    this.staInviando.set(true);
    this.messaggioErrore.set('');
    this.messaggioSuccesso.set('');

    // ⚠️ Nota: eliminaProfilo() elimina l'utente loggato, non quello passato come id
    this.utenteService.eliminaProfilo().subscribe({
      next: () => {
        // Rimuove l'utente dalla lista locale
        this.utenti.update(lista => lista.filter(b => b.id !== id));
        this.staInviando.set(false);
        this.messaggioSuccesso.set("Utente eliminato con successo");
      },
      error: (error: unknown) => {
        this.staInviando.set(false);
        this.messaggioErrore.set(
          this.estraiMessaggioErrore(error, 
            "Errore durante l'eliminazione dell'utente")
        );
      }
    });
  }

  // Placeholder per futura modifica
  modifica(id: string) {
  }

  // Estrae un messaggio leggibile dall'errore HTTP
  private estraiMessaggioErrore(error: unknown, fallback: string): string {
    if (error instanceof HttpErrorResponse) {
      return error.error?.message ?? fallback;
    }
    return fallback;
  }
}
```

### utenti-list.component.html

<details>
<summary>utenti-list.component.html V1.0</summary>

Andrea Bruno 03-06-2026
Creazione del file minimale esteticamente da modificare

```html
<section>

    <!-- Messaggio di errore -->
    @if (messaggioErrore()) {
        <div class="alert alert-warning">{{ messaggioErrore() }}</div>
    }

    <!-- Messaggio di successo -->
    @if (messaggioSuccesso()) {
        <div class="alert alert-success">{{ messaggioSuccesso() }}</div>
    }

    <div class="grid grid-2">

        <article class="card">
            <h2>Lista utenti</h2>

            <!-- Stato di caricamento -->
            @if (staCaricando()) {
                <p class="muted">Caricamento in corso...</p>

            <!-- Nessun utente -->
            } @else if (utenti().length === 0) {
                <p class="muted">Non ci sono utenti.</p>

            <!-- Lista utenti -->
            } @else {

                <div class="list">

                    <!-- Ciclo degli utenti -->
                    @for (u of utenti(); track u.id) {

                        <div class="list-item">
                            <div>

                                <!-- Intestazione -->
                                <strong>Utente #{{ u.id }}</strong>

                                <!-- Informazioni principali -->
                                <p>Nome completo: {{ u.nomeCompleto }}</p>
                                <p>Email: {{ u.email }}</p>
                                <p>Età: {{ u.eta }}</p>
                                <p>Saldo: {{ u.saldo }}</p>

                                <!-- Dati abbonamento se presenti -->
                                @if(u.seAbbonato) {
                                    <p>Tipo di abbonamento: {{ u.tipoAbbonamento }}</p>
                                    <p>Data inizio abbonamento: {{ u.dataInizioAbbonamento }}</p>
                                }

                                <!-- ID tecnico -->
                                <div class="muted">ID: {{ u.id }}</div>

                                <!-- Pulsanti azione -->
                                <div class="btn-row">
                                    <button class="btn btn-primary" (click)="modifica(u.id)">
                                        Modifica
                                    </button>

                                    <button class="btn btn-danger"
                                            (click)="elimina(u.id)"
                                            [disabled]="staInviando()">
                                        Elimina
                                    </button>
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
