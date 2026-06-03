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
    readonly movies = signal<Movie[]>([]);
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
        this.caricaMovies();

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
                this.ripristinaForm();
            },
            error: (error: unknown) => {
                this.staInviando.set(false);
                this.messaggioErrore.set(this.estraiMessaggioErrore(error, 'Operazione non riuscita.'));
            }
        });
    }

    trovaMoviePerId(id: string): Movie | undefined {
        return this.movies().find(movie => movie.id === id);
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
