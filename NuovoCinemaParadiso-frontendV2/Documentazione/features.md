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
