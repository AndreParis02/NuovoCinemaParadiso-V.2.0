import { Component, inject, signal, input, effect } from '@angular/core';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { HttpErrorResponse } from '@angular/common/http';

import { AuthService } from '../../../services/auth.service';
import { TurnoService } from '../../../services/turno.service';

import { Turno } from '../../../models/turno.model';



@Component({
    selector: 'turno-form',
    standalone: true,
    imports: [ReactiveFormsModule],
    templateUrl: './turno-form.component.html'
})

export class TurnoFormComponent {

    private readonly formBuilder = inject(FormBuilder);
    private readonly authService = inject(AuthService);
    private readonly turnoService = inject(TurnoService);


    readonly turnoSelezionato = input<Turno | null>(null);
    readonly staCaricando = signal(false);
    readonly staInviando = signal(false);
    readonly messaggioErrore = signal('');
    readonly messaggioSuccesso = signal('');
    readonly modificaId = signal<string>('');


    readonly form = this.formBuilder.nonNullable.group({
        nome: ['', [Validators.required, Validators.maxLength(100)]],
        oraInizio: ['', [Validators.required]],
        oraFine: ['', [Validators.required]]
    });

    constructor() {

        effect(() => {
            const turno = this.turnoSelezionato();
            if (turno) {
                this.inizioModifica(turno);
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

            ? this.turnoService.modifica(this.modificaId(), this.form.getRawValue())
            : this.turnoService.crea(this.form.getRawValue());


        request$.subscribe({
            next: () => {
                this.staInviando.set(false);
                this.messaggioSuccesso.set(this.modificaId() ? 'Turno aggiornato.' : 'Turno creato.');
                this.ripristinaForm();
            },
            error: (error: unknown) => {
                this.staInviando.set(false);
                this.messaggioErrore.set(this.estraiMessaggioErrore(error, 'Operazione non riuscita.'));
            }
        });
    }

    inizioModifica(item: Turno): void {

        if (!this.modificabileDa()) {
            return;
        }
        this.modificaId.set(item.id);

        this.form.patchValue({ nome: item.nome, oraInizio: item.oraInizio, oraFine: item.oraFine });
        this.messaggioErrore.set('');
        this.messaggioSuccesso.set('');
    }
    

    ripristinaForm(): void {
        this.modificaId.set('');
        this.form.reset({ nome: '', oraInizio: '', oraFine: ''});
    }
    tracciaPerId(_: string, item: Turno): string {
        return item.id;
    }

    private estraiMessaggioErrore(error: unknown, fallback: string): string {

        if (error instanceof HttpErrorResponse) {
            return error.error?.message ?? fallback;
        }
        return fallback;
    }
}
