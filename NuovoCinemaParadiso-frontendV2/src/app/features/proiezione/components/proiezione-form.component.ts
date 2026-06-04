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
