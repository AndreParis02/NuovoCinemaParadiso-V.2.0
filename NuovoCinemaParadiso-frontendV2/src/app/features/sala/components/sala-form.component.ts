import { Component, inject, signal, input, effect, output } from '@angular/core';
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

    readonly modificaCompletata = output<void>();
    
    readonly salaSelezionata = input<Sala | null>(null);
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
                
                this.modificaCompletata.emit();
                this.ripristinaForm();
            },
            error: (error: unknown) => {
                this.staInviando.set(false);
                this.messaggioErrore.set(this.estraiMessaggioErrore(error, 'Operazione non riuscita.'));
            }
        });
    }

    inizioModifica(item: Sala): void {

        if (!this.modificabileDa()) {
            return;
        }
        this.modificaId.set(item.id);

        this.form.patchValue({ nome: item.nome, capienza: item.capienza, tipologiaSalaId: item.tipologiaSalaId });
        this.messaggioErrore.set('');
        this.messaggioSuccesso.set('');
    }

    ripristinaForm(): void {
        this.modificaId.set('');
        this.form.reset({ nome: '', capienza: 0, tipologiaSalaId: 'scegli una tipologia' });
    }
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
