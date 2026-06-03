import { Component, inject, signal } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';

import { TurnoService } from '../../../services/turno.service';
import { Turno } from '../../../models/turno.model';

@Component({
    selector: 'turno-list',
    standalone: true,
    templateUrl: './turno-list.component.html'
})
export class TurnoListComponent {

    private readonly turnoService = inject(TurnoService);



    readonly turni = signal<Turno[]>([]);
    readonly staCaricando = signal(false);
    readonly messaggioErrore = signal('');
    readonly messaggioSuccesso = signal('');

    constructor() {
        this.caricaTurni();
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
                this.messaggioErrore.set(
                    this.estraiMessaggioErrore(error, 'Turni non trovati')
                );
            }
        });
    }

    tracciaPerId(_: number, item: Turno): string {
        return item.id;
    }

    private estraiMessaggioErrore(
        error: unknown,
        fallback: string
    ): string {

        if (error instanceof HttpErrorResponse) {
            return error.error?.message ?? fallback;
        }

        return fallback;
    }
}