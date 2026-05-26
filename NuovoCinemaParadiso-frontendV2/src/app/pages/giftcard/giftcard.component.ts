import { Component, inject, signal } from '@angular/core';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { UtenteService } from '../../services/utente.service';
import { GiftCard } from '../../models/giftCard.model';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
    selector: 'giftcard-page',
    standalone: true,
    imports: [ReactiveFormsModule],
    templateUrl: './giftcard.page.html'
})
export class GiftCardPage {

    private readonly fb = inject(FormBuilder);
    private readonly utenteService = inject(UtenteService);

    readonly giftCards = signal<GiftCard[]>([]);
    readonly staCaricando = signal(false);
    readonly staInviando = signal(false);
    readonly messaggioErrore = signal('');
    readonly messaggioSuccesso = signal('');

    readonly form = this.fb.nonNullable.group({
        codiceRiscatto: ['', Validators.required]
    });

    constructor() {
        this.caricaGiftCards();
    }

    caricaGiftCards(): void {
        this.staCaricando.set(true);

        this.utenteService.ottieniGiftCardUtente().subscribe({
            next: (cards) => {
                this.giftCards.set(cards);
                this.staCaricando.set(false);
            },
            error: () => {
                this.staCaricando.set(false);
                this.giftCards.set([]);
            }
        });
    }

    riscatta(): void {
        if (this.form.invalid) return;

        this.staInviando.set(true);
        this.messaggioErrore.set('');
        this.messaggioSuccesso.set('');

        const codice = this.form.value.codiceRiscatto!;

        this.utenteService.riscattaGiftCard(codice).subscribe({
            next: () => {
                this.staInviando.set(false);
                this.messaggioSuccesso.set('Gift card riscattata con successo.');
                this.form.reset();
                this.caricaGiftCards();
            },
            error: (err: unknown) => {
                this.staInviando.set(false);
                this.messaggioErrore.set(this.estraiErrore(err, 'Codice non valido.'));
            }
        });
    }

    tracciaPerId(_: number, item: GiftCard): string {
        return item.id ?? '';
    }

    private estraiErrore(error: unknown, fallback: string): string {
        if (error instanceof HttpErrorResponse) {
            return error.error?.message ?? fallback;
        }
        return fallback;
    }
}
