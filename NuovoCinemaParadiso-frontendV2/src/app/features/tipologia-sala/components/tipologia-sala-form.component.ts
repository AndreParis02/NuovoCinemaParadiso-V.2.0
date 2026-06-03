import { Component, inject, signal, input, output, effect } from '@angular/core';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { HttpErrorResponse } from '@angular/common/http';

import { AuthService } from '../../../services/auth.service';
import { TipologiaSalaService } from '../../../services/tipologia-sala.service';
import { TipologiaSala } from '../../../models/tipologia-sala.model';

@Component({
    selector: 'tipologia-sala-form',
    standalone: true,
    imports: [ReactiveFormsModule],
    templateUrl: './tipologia-sala-form.component.html'
})
export class TipologiaSalaFormComponent {
    private readonly formBuilder = inject(FormBuilder);
    private readonly authService = inject(AuthService);
    private readonly tipologiaSalaService = inject(TipologiaSalaService);

    readonly tipologiaSelezionata = input<TipologiaSala | null>(null, { alias: 'movieSelezionato' });
    readonly modificaCompletata = output<void>();

    readonly staCaricando = signal(false);
    readonly staInviando = signal(false);
    readonly messaggioErrore = signal('');
    readonly messaggioSuccesso = signal('');
    readonly modificaId = signal<string>('');

    readonly form = this.formBuilder.nonNullable.group({
        nome: ['', [Validators.required, Validators.maxLength(100)]],
        maggiorazionePrezzo: [1, [Validators.required, Validators.min(1)]],
    });

    constructor() {
        effect(() => {
            const tipologia = this.tipologiaSelezionata();
            if (tipologia) {
                this.inizioModifica(tipologia);
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
            ? this.tipologiaSalaService.modifica(this.modificaId(), this.form.getRawValue())
            : this.tipologiaSalaService.crea(this.form.getRawValue());

        request$.subscribe({
            next: () => {
                this.staInviando.set(false);
                this.messaggioSuccesso.set(this.modificaId() ? 'Tipologia sala aggiornata.' : 'Tipologia sala creata.');
                this.modificaCompletata.emit();
                this.ripristinaForm();
            },
            error: (error: unknown) => {
                this.staInviando.set(false);
                this.messaggioErrore.set(this.estraiMessaggioErrore(error, 'Operazione non riuscita.'));
            }
        });
    }

    inizioModifica(item: TipologiaSala): void {
        if (!this.modificabileDa()) {
            return;
        }
        this.modificaId.set(item.id);
        this.form.patchValue({ nome: item.nome, maggiorazionePrezzo: item.maggiorazionePrezzo });
        this.messaggioErrore.set('');
        this.messaggioSuccesso.set('');
    }

    ripristinaForm(): void {
        this.modificaId.set('');
        this.form.reset({ nome: '', maggiorazionePrezzo: 1 });
    }

    tracciaPerId(_: string, item: TipologiaSala): string {
        return item.id;
    }

    private estraiMessaggioErrore(error: unknown, fallback: string): string {
        if (error instanceof HttpErrorResponse) {
            return error.error?.message ?? fallback;
        }
        return fallback;
    }
}