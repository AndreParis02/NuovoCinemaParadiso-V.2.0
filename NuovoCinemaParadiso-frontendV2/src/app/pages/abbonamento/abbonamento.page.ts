import { Component, inject, signal } from '@angular/core';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { HttpErrorResponse } from '@angular/common/http';

import { AuthService } from '../../services/auth.service';
import { AbbonamentoService } from '../../services/abbonamento.service';

import { Abbonamento } from '../../models/abbonamento.model';


@Component({
  selector: 'abbonamento-page',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './abbonamento.page.html'
})

export class AbbonamentoPage {

  private readonly formBuilder = inject(FormBuilder);
  private readonly authService = inject(AuthService);
  private readonly abbonamentoService = inject(AbbonamentoService);

  readonly abbonamenti = signal<Abbonamento[]>([]);
  readonly staCaricando = signal(false);
  readonly staInviando = signal(false);
  readonly messaggioErrore = signal('');
  readonly messaggioSuccesso = signal('');
  readonly modificaId = signal<string>('');


  readonly form = this.formBuilder.nonNullable.group({
    nome: ['', [Validators.required, Validators.maxLength(100)]],
    durata: [1, [Validators.required, Validators.min(1)]],
    prezzo: [1, [Validators.required, Validators.min(1), Validators.max(999999999)]],
    sconto: [0, [Validators.required, Validators.min(0), Validators.max(100)]]
  });

  constructor() {
    this.caricaAbbonamenti();
  }
  modificabileDa(): boolean {
    return this.authService.possiedeQualsiasiRuolo(['Operatore']);
  }
  caricaAbbonamenti(): void {

    this.staCaricando.set(true);
    this.messaggioErrore.set('');
    this.abbonamentoService.ottieniTutto().subscribe({

      next: (items) => {
        this.abbonamenti.set(items);
        this.staCaricando.set(false);

      },
      error: (error: unknown) => {
        this.staCaricando.set(false);
        this.messaggioErrore.set(this.estraiMessaggioErrore(error, 'abbonamenti non trovati'));
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

      ? this.abbonamentoService.modifica(this.modificaId(), this.form.getRawValue())
      : this.abbonamentoService.crea(this.form.getRawValue());


    request$.subscribe({
      next: () => {
        this.staInviando.set(false);
        this.messaggioSuccesso.set(this.modificaId() ? 'Abbonamento aggiornato.' : 'Abbonamento creato.');
        this.ripristinaForm();
        this.caricaAbbonamenti();
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

  elimina(item: Abbonamento): void {
    if (!this.modificabileDa()) {
      return;
    }
    const confirmed = confirm(`Eliminare l'abbonamento \" ${item.nome}\"?`);
    if (!confirmed) {
      return;
    }

    this.messaggioErrore.set('');
    this.messaggioSuccesso.set('');

    this.abbonamentoService.elimina(item.id).subscribe({
      next: () => {
        if (this.modificaId() === item.id) {
          this.ripristinaForm();
        }
        this.caricaAbbonamenti();
      },
      error: (error: unknown) => {

        this.messaggioErrore.set(this.estraiMessaggioErrore(error, 'eliminazione non riuscita.'));
      }
    });
  }

  ripristinaForm(): void {
    this.modificaId.set('');
    this.form.reset({ nome: '', durata: 1, prezzo: 1, sconto: 0 });
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
