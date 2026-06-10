import { Component, inject, signal, output } from '@angular/core';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { HttpErrorResponse } from '@angular/common/http';
import { GiftCardService } from '../../../services/giftcard.service';

@Component({
  selector: 'riscatta-codice-form',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './riscatta-codice.component.html',
})
export class RiscattaCodiceComponent {
  private readonly formBuilder = inject(FormBuilder);
  private readonly giftCardService = inject(GiftCardService);

  readonly staInviando = signal(false);
  readonly messaggioErrore = signal('');
  readonly messaggioSuccesso = signal('');

  readonly riscattoCompletato = output<void>();

  readonly form = this.formBuilder.nonNullable.group({
    codiceRiscatto: ['', [Validators.required, Validators.maxLength(50)]]
  });

  invia(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.staInviando.set(true);
    this.messaggioErrore.set('');
    this.messaggioSuccesso.set('');

    const codice = this.form.getRawValue().codiceRiscatto;

    this.giftCardService.riscatta(codice).subscribe({
      next: () => {
        this.staInviando.set(false);
        this.messaggioSuccesso.set('Gift Card riscattata! I crediti sono stati aggiunti ai tuoi crediti.');

        this.riscattoCompletato.emit();
        this.form.reset({ codiceRiscatto: '' });
      },
      error: (error: unknown) => {
        this.staInviando.set(false);
        this.messaggioErrore.set(this.estraiMessaggioErrore(error, 'Codice non valido o già utilizzato.'));
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