import { Component, inject, signal } from '@angular/core';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { GiftCardService } from '../../services/giftcard.service';
import { UtenteService } from '../../services/utente.service';
import { GiftCard } from '../../models/giftCard.model';
import { HttpErrorResponse } from '@angular/common/http';
import { AuthService } from '../../services/auth.service';
import { FormsModule } from '@angular/forms';

@Component({
    selector: 'giftcard-page',
    standalone: true,
    imports: [ReactiveFormsModule,FormsModule],
    templateUrl: './giftcard.page.html'
})
export class GiftCardPage {

    private readonly fb = inject(FormBuilder);
    private readonly giftCardService = inject(GiftCardService);
    private readonly utenteService = inject(UtenteService);
    private readonly authService = inject(AuthService);

    readonly giftCards = signal<GiftCard[]>([]);
    readonly staCaricando = signal(false);
    readonly staInviando = signal(false);
    readonly messaggioErrore = signal('');
    readonly messaggioSuccesso = signal('');

    readonly ruolo = this.authService.utenteCorrente()?.ruolo ?? '';

    valoreGiftCard = 0;

    readonly form = this.fb.nonNullable.group({
        codiceRiscatto: ['', Validators.required]
    });

    constructor() {
        this.caricaGiftCards();
    }

    caricaGiftCards(): void {
        this.staCaricando.set(true);

        this.giftCardService.ottieniTutto().subscribe({
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

    crea(): void {
        if (this.valoreGiftCard <= 0) {
            this.messaggioErrore.set('Inserisci un valore valido.');
            return;
        }

        this.staInviando.set(true);
        this.messaggioErrore.set('');
        this.messaggioSuccesso.set('');

        const payload = {
            nome: "Gift Card",
            valore: this.valoreGiftCard,
            codiceRiscatto: crypto.randomUUID() // genera un codice univoco
        };

        this.giftCardService.crea(payload).subscribe({
            next: () => {
                this.staInviando.set(false);
                this.messaggioSuccesso.set('Gift card creata con successo.');
                this.valoreGiftCard = 0;
                this.caricaGiftCards();
            },
            error: () => {
                this.staInviando.set(false);
                this.messaggioErrore.set('Errore nella creazione della gift card.');
            }
        });
    }

    elimina(id: string): void {
        this.giftCardService.elimina(id).subscribe({
            next: () => {
                this.messaggioSuccesso.set('Gift card eliminata.');
                this.caricaGiftCards();
            },
            error: () => {
                this.messaggioErrore.set('Errore durante l\'eliminazione.');
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
