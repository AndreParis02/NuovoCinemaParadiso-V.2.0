import { Component, inject, signal } from '@angular/core';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { HttpErrorResponse } from '@angular/common/http';

import { AuthService } from '../../services/auth.service';
import { TurnoService } from '../../services/turno.service';

import { Turno } from '../../models/turno.model';



@Component({
  selector: 'turno-page',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './turno.page.html'
})

export class TurnoPage {

  private readonly formBuilder = inject(FormBuilder);
  private readonly authService = inject(AuthService);
  private readonly turnoService = inject(TurnoService);
  



  readonly turni = signal<Turno[]>([]);
  readonly staCaricando = signal(false);
  readonly staInviando = signal(false);
  readonly messaggioErrore = signal('');
  readonly messaggioSuccesso = signal('');
  readonly modificaId = signal<string>('');


  readonly form = this.formBuilder.nonNullable.group({
    oraInizio: ['', [Validators.required]],
    oraFine: ['', [Validators.required]],
    nome: ['', [Validators.required, Validators.maxLength(100)]]
  });

  constructor() {
    this.caricaTurni();
  }
  modificabileDa(): boolean {
    return this.authService.possiedeQualsiasiRuolo(['Operatore']);
  }
  caricaTurni(): void {

    this.staCaricando.set(true);
    this.messaggioErrore.set('');
    this.turnoService.ottieniTutto().subscribe({

      next: (items) => {
        this.turni.set(items);
        this.staCaricando.set(false);

      },
      error: (error: unknown) => {
        this.staCaricando.set(false);
        this.messaggioErrore.set(this.estraiMessaggioErrore(error, 'turni non trovati'));
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

      ? this.turnoService.modifica(this.modificaId(), this.form.getRawValue())
      : this.turnoService.crea(this.form.getRawValue());


    request$.subscribe({
      next: () => {
        this.staInviando.set(false);
        this.messaggioSuccesso.set(this.modificaId() ? 'Turno aggiornato.' : 'Turno creato.');
        this.ripristinaForm();
        this.caricaTurni();
      },
      error: (error: unknown) => {
        this.staInviando.set(false);
        this.messaggioErrore.set(this.estraiMessaggioErrore(error, 'Operazione non riuscita.'));
      }
    });
  }

  inizioModifica(item: Turno): void {

    if (!this.modificabileDa()) {
      return;
    }
    this.modificaId.set(item.id);

    this.form.patchValue({ oraInizio: item.oraInizio, oraFine: item.oraFine, nome: item.nome });
    this.messaggioErrore.set('');
    this.messaggioSuccesso.set('');
  }

  elimina(item: Turno): void {
    if (!this.modificabileDa()) {
      return;
    }
    const confirmed = confirm(`Eliminare il turno \" ${item.nome}\"?`);
    if (!confirmed) {
      return;
    }

    this.messaggioErrore.set('');
    this.messaggioSuccesso.set('');

    this.turnoService.elimina(item.id).subscribe({
      next: () => {
        if (this.modificaId() === item.id) {
          this.ripristinaForm();
        }
        this.caricaTurni();
      },
      error: (error: unknown) => {

        this.messaggioErrore.set(this.estraiMessaggioErrore(error, 'eliminazione non riuscita.'));
      }
    });
  }

  ripristinaForm(): void {
    this.modificaId.set('');
    this.form.reset({ oraInizio: '', oraFine: '', nome: '' });
  }
  tracciaPerId(_: string, item: Turno ): string {
    return item.id;
  }

  private estraiMessaggioErrore(error: unknown, fallback: string): string {

    if (error instanceof HttpErrorResponse) {
      return error.error?.message ?? fallback;
    }
    return fallback;
  }
}
