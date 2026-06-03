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

## auth 
### page
### components
- login.component.ts
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
    const confirmed = confirm(`Eliminare il film \" ${item.titolo}\"?`);
    if (!confirmed) {
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

## conto-cinema
### components
- conto-cinema-detail.component.ts [gestore]


