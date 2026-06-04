import { Component, inject, signal, OnInit } from '@angular/core';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { HttpErrorResponse } from '@angular/common/http';
import { DatePipe } from '@angular/common';
import { UtenteService } from '../../services/utente.service';
import { AuthService } from '../../services/auth.service';
import { Utente } from '../../models/utente.model';

@Component({
  selector: 'utente-page',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    DatePipe
  ],
  templateUrl: './utente.page.html',
})
export class UtentePage implements OnInit {
  private readonly formBuilder = inject(FormBuilder);
  private readonly utenteService = inject(UtenteService);
  private readonly authService = inject(AuthService);

  readonly utente = signal<Utente | null>(null);
  readonly staCaricando = signal(false);
  readonly staInviando = signal(false);
  readonly messaggioErrore = signal('');
  readonly messaggioSuccesso = signal('');

  readonly form = this.formBuilder.nonNullable.group({
    nomeCompleto: ['', [Validators.required, Validators.maxLength(100)]],
    eta: [0, [Validators.required, Validators.min(14), Validators.max(100)]]
  });

  ngOnInit(): void {
    this.caricaUtente();
  }
  
  modificabileDa(): boolean {
    return this.authService.possiedeQualsiasiRuolo(['Operatore']);
  }

  caricaUtente(): void {
    this.staCaricando.set(true);
    this.messaggioErrore.set('');

    this.utenteService.profilo().subscribe({
      next: (item) => {
        this.utente.set(item);

        this.form.patchValue({
          nomeCompleto: item.nomeCompleto,
          eta: item.eta
        });

        this.staCaricando.set(false);
      },
      error: (error) => {
        this.staCaricando.set(false);
        this.messaggioErrore.set(
          this.estraiMessaggioErrore(error, 'Accedi per vedere il profilo')
        );
      }
    });
  }

  invia(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.staInviando.set(true);
    this.messaggioErrore.set('');
    this.messaggioSuccesso.set('');

    this.utenteService.modifica(this.form.getRawValue()).subscribe({
      next: () => {
        this.staInviando.set(false);
        this.messaggioSuccesso.set('Profilo aggiornato');
        this.caricaUtente();
      },
      error: (error) => {
        this.staInviando.set(false);
        this.messaggioErrore.set(
          this.estraiMessaggioErrore(error, 'Modifica non riuscita.')
        );
      }
    });
  }

  elimina(): void {
    const confirmed = confirm('Vuoi davvero eliminare il tuo account?');
    if (!confirmed) return;

    this.messaggioErrore.set('');
    this.messaggioSuccesso.set('');

    this.utenteService.eliminaProfilo().subscribe({
      next: () => {
        this.authService.logout(); // rimuove token
        this.messaggioSuccesso.set('Account eliminato');
        window.location.href = '/login'; // redirect
      },
      error: (error) => {
        this.messaggioErrore.set(
          this.estraiMessaggioErrore(error, 'Eliminazione non riuscita.')
        );
      }
    });
  }

  private estraiMessaggioErrore(error: unknown, fallback: string): string {

    if (error instanceof HttpErrorResponse) {
      return error.error?.message ?? fallback;
    }
    return fallback;
  }
}
