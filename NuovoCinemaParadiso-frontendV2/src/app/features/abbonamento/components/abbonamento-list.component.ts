import { Component, inject, signal } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';

import { AbbonamentoService } from '../../../services/abbonamento.service';
import { Abbonamento } from '../../../models/abbonamento.model';

@Component({
    selector: 'abbonamento-list',
    standalone: true,
    templateUrl: './abbonamento-list.component.html',
    styleUrls: ['./abbonamento-list.component.css']
})
export class AbbonamentoListComponent {

    private readonly abbonamentoService = inject(AbbonamentoService);



    readonly abbonamenti = signal<Abbonamento[]>([]);
    readonly staCaricando = signal(false);
    readonly messaggioErrore = signal('');
    readonly messaggioSuccesso = signal('');

    constructor() {
        this.caricaAbbonamenti();
    }

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
                this.messaggioErrore.set(
                    this.estraiMessaggioErrore(error, 'Abbonamenti non trovati')
                );
            }
        });
    }

    tracciaPerId(_: number, item: Abbonamento): string {
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