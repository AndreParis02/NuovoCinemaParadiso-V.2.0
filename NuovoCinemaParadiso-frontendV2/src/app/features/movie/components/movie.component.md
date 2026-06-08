## movie-list

### movie-list.component.ts 

<details>
    <summary>
    V1.1
    </summary>

Utente: Andrea Paris
Data: 06/06/2026
Descrizione: fix errore modifica in tempo reale file .ts di movie-list

```ts
import { Component, inject, signal } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';

import { AuthService } from '../../../services/auth.service';
import { MovieService } from '../../../services/movie.service';

import { Movie } from '../../../models/movie.model';
import { MovieFormComponent } from "./movie-form.component";

@Component({
  selector: 'movie-list',
  standalone: true,
  templateUrl: './movie-list.component.html',
  imports: [MovieFormComponent]
})
export class MovieListComponent {

  private readonly authService = inject(AuthService);
  private readonly movieService = inject(MovieService);

  readonly movies = signal<Movie[]>([]);
  readonly filmScelto = signal<Movie | null>(null);
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

  elimina(item: Movie): void {
    if (!this.visualizzabileDa()) {
      return;
    }
    
    if (!confirm(`Sei sicuro di voler eliminare il film "${item.titolo}"?`)) {
      return;
    }

    this.messaggioErrore.set('');
    this.messaggioSuccesso.set('');
    this.staInviando.set(true);

    this.movieService.elimina(item.id).subscribe({
      next: () => {
        this.staInviando.set(false);
        this.messaggioSuccesso.set('Film eliminato con successo.');

        if (this.filmScelto()?.id === item.id) {
          this.filmScelto.set(null);
        }

        // OTTIMIZZAZIONE SEGNALE: Rimuove l'elemento direttamente dallo stato locale
        this.movies.update(listaAttuale => listaAttuale.filter(m => m.id !== item.id));
      },
      error: (error: unknown) => {
        this.staInviando.set(false);
        this.messaggioErrore.set(this.estraiMessaggioErrore(error, 'eliminazione non riuscita.'));
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

### movie-list.component.html

<details>
    <summary>
    V1.1
    </summary>

Utente: Andrea Paris
Data: 06/06/2026
Descrizione: fix errore modifica in tempo reale file .html di movie-list

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
                        @if(visualizzabileDa()) {
                        <div class="btn-row" style="margin-top: 1rem;">
                            <button class="btn btn-secondary" type="button" (click)="filmScelto.set(item)">Modifica</button>
                            <button class="btn btn-secondary" type="button" (click)="elimina(item)" [disabled]="staInviando()">Elimina</button>
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
            <movie-form [movieSelezionato]="filmScelto()" (modificaCompletata)="caricaMovies()"></movie-form>
        </article>
        }
    </div> 
</section>
```

</details>

## movie-form

### movie-form.component.ts 

<details>
    <summary>
    V1.1
    </summary>

Utente: Andrea Paris
Data: 06/06/2026
Descrizione: fix errore modifica in tempo reale file .ts di movie-form

```ts
import { Component, inject, signal, input, effect, output } from '@angular/core';
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

    // Outputs tramite la nuova API output() di Angular (Signals friendly)
    readonly modificaCompletata = output<void>();

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

        effect(() => {
            const movie = this.movieSelezionato();
            if (movie) {
                this.inizioModifica(movie);
            } else {
                this.ripristinaForm();
            }
        });
    }

    modificabileDa(): boolean {
        return this.authService.possiedeQualsiasiRuolo(['Operatore']);
    }

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
                
                // Comunica al padre di ricaricare la lista
                this.modificaCompletata.emit();
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

        this.form.patchValue({ 
            titolo: item.titolo, 
            descrizione: item.descrizione, 
            durataMinuti: item.durataMinuti, 
            prezzoMovie: item.prezzoMovie, 
            genereId: item.genereId 
        });
        
        this.messaggioErrore.set('');
        this.messaggioSuccesso.set('');
    }

    ripristinaForm(): void {
        this.modificaId.set('');
        this.form.reset({ titolo: '', descrizione: '', durataMinuti: 1, prezzoMovie: 0.01, genereId: '' });
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

</details>

### movie-form.component.html

<details>
    <summary>
    V1.1
    </summary>

Utente: Andrea Paris
Data: 06/06/2026
Descrizione: fix errore modifica in tempo reale file .html di movie-form

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
                    <input id="descrizione" type="text" formControlName="descrizione" [disabled]="!modificabileDa()">
                    
                    <label for="durataMinuti">Durata (minuti)</label>
                    <input id="durataMinuti" type="number" formControlName="durataMinuti" [disabled]="!modificabileDa()">
                    
                    <label for="prezzoMovie">Prezzo (€)</label>
                    <input id="prezzoMovie" type="number" formControlName="prezzoMovie" [disabled]="!modificabileDa()">
                    
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

