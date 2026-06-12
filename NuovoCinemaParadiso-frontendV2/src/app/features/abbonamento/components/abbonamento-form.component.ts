import { Component, inject, signal, input, effect, output } from '@angular/core';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { HttpErrorResponse } from '@angular/common/http';

import { AuthService } from '../../../services/auth.service';
import { AbbonamentoService } from '../../../services/abbonamento.service';

import { Abbonamento } from '../../../models/abbonamento.model';
import { ProfiloService } from '../../../services/profilo.service';



@Component({
    selector: 'abbonamento-form',
    standalone: true,
    imports: [ReactiveFormsModule],
    templateUrl: './abbonamento-form.component.html'
})

export class AbbonamentoFormComponent {

    private readonly formBuilder = inject(FormBuilder);
    private readonly authService = inject(AuthService);
    private readonly abbonamentoService = inject(AbbonamentoService);
    private readonly profiloService = inject(ProfiloService);

    readonly modificaCompletata = output<void>();

    readonly abbonamentoSelezionato = input<Abbonamento | null>(null);
    readonly staCaricando = signal(false);
    readonly staInviando = signal(false);
    readonly messaggioErrore = signal('');
    readonly messaggioSuccesso = signal('');
    readonly modificaId = signal<string>('');


    readonly form = this.formBuilder.nonNullable.group({
        nome: ['', [Validators.required, Validators.maxLength(100)]],
        durata: [1, [Validators.required, Validators.min(1)]],
        prezzo: [1, [Validators.required, Validators.min(1)]],
        sconto: [1, [Validators.required, Validators.min(1), Validators.max(100)]]
    });

    constructor() {

        effect(() => {
            const abbonamento = this.abbonamentoSelezionato();
            if (abbonamento) {
                this.inizioModifica(abbonamento);
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

            ? this.abbonamentoService.modifica(this.modificaId(), this.form.getRawValue())
            : this.abbonamentoService.crea(this.form.getRawValue());


        request$.subscribe({
            next: () => {
                this.staInviando.set(false);
                this.messaggioSuccesso.set(this.modificaId() ? 'Abbonamento aggiornato.' : 'Abbonamento creato.');
                this.profiloService.richiediAggiornamentoProfilo();
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

    inizioModifica(item: Abbonamento): void {

        if (!this.modificabileDa()) {
            return;
        }
        this.modificaId.set(item.id);

        this.form.patchValue({ nome: item.nome, durata: item.durata, prezzo: item.prezzo, sconto: item.sconto });
        this.messaggioErrore.set('');
        this.messaggioSuccesso.set('');
    }

    ripristinaForm(): void {
        this.modificaId.set('');
        this.form.reset({ nome: '', durata: 0, prezzo: 0, sconto: 0});
    }
    tracciaPerId(_: string, item: Abbonamento): string {
        return item.id;
    }

    private estraiMessaggioErrore(error: unknown, fallback: string): string {

        if (error instanceof HttpErrorResponse) {
            return error.error?.message ?? fallback;
        }
        return fallback;
    }
}
