import { Component, inject, signal } from '@angular/core';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { HttpErrorResponse } from '@angular/common/http';
import { GestioneUtenteService } from '../../services/gestione-utente.service';

@Component({
  selector: 'operatore-page',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './operatore.page.html'
})
export class OperatorePage {

  private readonly fb = inject(FormBuilder);
  private readonly gestioneUtenteService = inject(GestioneUtenteService);

  readonly isSubmitting = signal(false);
  readonly successMessage = signal('');
  readonly errorMessage = signal('');

  readonly roles = ['Gestore', 'Operatore', 'Utente'];

  readonly cambiaRuoloForm = this.fb.nonNullable.group({
    email: ['', [Validators.required, Validators.email]],
    nuovoRuolo: ['Utente', [Validators.required]]
  });

  cambiaRuolo(): void {

    if (this.cambiaRuoloForm.invalid) {
      this.cambiaRuoloForm.markAllAsTouched();
      return;
    }

    this.isSubmitting.set(true);
    this.successMessage.set('');
    this.errorMessage.set('');

    this.gestioneUtenteService
      .modificaRuolo(this.cambiaRuoloForm.getRawValue())
      .subscribe({

        next: (response) => {

          this.isSubmitting.set(false);

          this.successMessage.set(
            `${response.messaggio} Nuovo ruolo: ${response.ruolo}`
          );
        },

        error: (error: unknown) => {

          this.isSubmitting.set(false);

          this.errorMessage.set(
            this.estraiMessaggioErrore(error)
          );
        }
      });
  }

  private estraiMessaggioErrore(error: unknown): string {

    if (error instanceof HttpErrorResponse) {

      return error.error?.messaggio
        ?? 'Cambio ruolo non riuscito';
    }

    return 'Cambio ruolo non riuscito';
  }

}