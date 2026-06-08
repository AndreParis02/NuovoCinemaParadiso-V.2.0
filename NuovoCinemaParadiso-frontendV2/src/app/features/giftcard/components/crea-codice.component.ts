import { Component, inject, signal, output } from '@angular/core';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { HttpErrorResponse } from '@angular/common/http';
import { GiftCardService } from '../../../services/giftcard.service';

@Component({
  selector: 'crea-codice-form',
  standalone: true,
  imports: [ReactiveFormsModule
  ],
  templateUrl: './crea-codice.component.html',
})
export class CreaCodiceComponent {
  private readonly formBuilder = inject(FormBuilder);
  private readonly giftCardService = inject(GiftCardService);

  readonly staInviando = signal(false);
  readonly messaggioErrore = signal('');
  readonly messaggioSuccesso = signal('');

  readonly ricaricaCompletata = output<void>();

  readonly form = this.formBuilder.nonNullable.group({
    valore: [10, [Validators.required, Validators.min(1), Validators.max(1000)]]
  });

  invia(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.staInviando.set(true);
    this.messaggioErrore.set('');
    this.messaggioSuccesso.set('');

    const valore = this.form.getRawValue().valore;

    this.giftCardService.ricarica(valore).subscribe({
      next: () => {
        this.staInviando.set(false);
        this.messaggioSuccesso.set('Gift Card acquistata e codice generato!');

        this.ricaricaCompletata.emit();
        this.form.reset({ valore: 10 });


      },
      error: (error: unknown) => {
        this.staInviando.set(false);
        this.messaggioErrore.set(this.estraiMessaggioErrore(error, 'Operazione non riuscita. Controlla il tuo saldo.'));
      },
    });
  }

  private estraiMessaggioErrore(error: unknown, fallback: string): string {
    if (error instanceof HttpErrorResponse) {
      return error.error?.message || error.error?.errore || fallback;
    }
    return fallback;
  }
}