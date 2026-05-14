import { Component, inject, signal } from '@angular/core';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { HttpErrorResponse } from '@angular/common/http';
import { AuthService } from '../../services/auth.service';
import { AdminService } from '../../services/admin.service';
import { Utente } from '../../models/utente.model';

@Component({
    selector: 'gestore-lista-utenti-page',
    standalone: true,
    imports: [ReactiveFormsModule],
    templateUrl: './gestore-lista-utenti.page.html'
})
export class GestoreListaUtentiPage {
    private readonly formBuilder = inject(FormBuilder);
    private readonly authService = inject(AuthService);
    private readonly gestoreService = inject(AdminService);


    readonly staInviando = signal(false);
    readonly staCaricando = signal(false);
    readonly messaggioSuccesso = signal('');
    readonly messaggioErrore = signal('');
    readonly ruoli = ['Gestore', 'Operatore', 'Utente'];
    readonly utenti = signal<Utente[]>([]);
    readonly modificaId = signal<string>('');

    readonly form = this.formBuilder.nonNullable.group({
        nomeCompleto: ['', [Validators.required], [Validators.maxLength(100)]],
        eta: [0, [Validators.required]]
    });

    constructor() {
        this.caricaUtenti();
    }
    modificabileDa(): boolean {
        return this.authService.hasAnyRole(['Gestore', 'Operatore']);
    }
    caricaUtenti(): void {

        this.messaggioErrore.set('');
        this.staCaricando.set(true);

        this.gestoreService.OttieniUtenti().subscribe({
            next: (items) => {
                console.log('Dati grezzi dal server:', items);
                this.utenti.set(items);
                this.staCaricando.set(false);
            },
            error: (error: unknown) => {
                this.staCaricando.set(false);
                this.messaggioErrore.set(this.estraiMessaggioErrore(error, 'utenti non trovati'));
            }
        });
    }
    inizioModifica(item: Utente): void {
        // Verifica i permessi prima di permettere l'inizio della modifica
        if (!this.modificabileDa()) {
            return;
        }
        this.modificaId.set(item.id);
        // dizionario che rappresenta la mappa del modulo
        this.form.patchValue({ nomeCompleto: item.nomeCompleto, eta: item.eta });
        this.messaggioErrore.set('');
        this.messaggioSuccesso.set('');
    }

    elimina(item: Utente): void {
        if (!this.modificabileDa()) {
            return;
        }
        const confirmed = confirm(`Eliminare l'utente \" ${item.nomeCompleto}\"?`);
        if (!confirmed) {
            return;
        }

        this.messaggioErrore.set('');
        this.messaggioSuccesso.set('');

        this.gestoreService.eliminaUtente(item.id).subscribe({
            next: () => {
                if (this.modificaId() === item.id) {
                    this.ripristinaForm();
                }
                this.caricaUtenti();
            },
            error: (error: unknown) => {

                this.messaggioErrore.set(this.estraiMessaggioErrore(error, 'eliminazione non riuscita.'));
            }
        });
    }
    ripristinaForm(): void {
        this.modificaId.set('');
        this.form.reset({ nomeCompleto: '', eta: 0 });
    }
    // il _ indica che il primo parametro non viene utilizzato,
    //  è una convenzione per indicare che è presente ma non serve
    tracciaPerId(_: string, item: Utente): string {
        return item.id;
    }
    private estraiMessaggioErrore(error: unknown, fallback: string): string {

        if (error instanceof HttpErrorResponse) {
            return error.error?.message ?? fallback;
        }
        return fallback;
    }
}