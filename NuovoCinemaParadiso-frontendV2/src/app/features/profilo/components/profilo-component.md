# profilo.component
<details>
<summary>versione1.0</summary>

Francesco Lorenzi 05/06/2026

componente copiato da utente.page.ts, non ho cambiato nulla a parte il perrcorso degli importi dei servizi

## profilo.component.ts
```ts
import { Component, inject, signal, OnInit } from '@angular/core';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { HttpErrorResponse } from '@angular/common/http';
import { DatePipe } from '@angular/common';
// servizi, sono dovuto scendere di una cartella in più
import { UtenteService } from '../../../services/utente.service';
import { AuthService } from '../../../services/auth.service';
import { Utente } from '../../../models/utente.model';

@Component({
  selector: 'profilo',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    DatePipe
  ],
  templateUrl: './profilo.component.html',
})
export class ProfiloComponent implements OnInit {
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

```
## profilo.component.html

anche il componente html è copiato, qua non ho modificato niente

```html
<section>
    @if (utente()) {
    <h1 class="page-title">Ciao, {{ utente()?.nomeCompleto }}</h1>
    <p class="page-subtitle">Gestisci le informazioni del tuo account.</p>
    }

    @if (messaggioErrore()) {
    <div class="alert alert-warning">{{ messaggioErrore() }}</div>
    }

    @if (messaggioSuccesso()) {
    <div class="alert alert-success">{{ messaggioSuccesso() }}</div>
    }

    @if (staCaricando()) {
    <p class="muted">Caricamento profilo...</p>
    } @else if (!utente()) {
    <p class="muted">Nessun profilo trovato.</p>
    } @else {

    <div class="grid grid-2" style="margin-top: 1.5rem;">

        <article class="card">
            <h2>Informazioni Utente</h2>

            <div class="list">

                <div class="list-item">
                    <strong>Email</strong>
                    <p>{{ utente()?.email }}</p>
                </div>

                <div class="list-item">
                    <strong>Età</strong>
                    <p>{{ utente()?.eta }}</p>
                </div>

                @if (!modificabileDa()) {
                <div class="list-item">
                    <strong>Saldo</strong>
                    <p>{{ utente()?.saldo }} €</p>
                </div>
                }

            </div>
        </article>

        <article class="card">
            <h2>Modifica Profilo</h2>

            <form class="form-grid" [formGroup]="form" (ngSubmit)="invia()">

                <div>
                    <label for="nomeCompleto">Nome completo</label>
                    <input id="nomeCompleto" type="text" formControlName="nomeCompleto">

                    <label for="eta">Età</label>
                    <input id="eta" type="number" formControlName="eta">
                </div>

                <div class="btn-row">
                    <button class="btn btn-primary" type="submit" [disabled]="staInviando()">
                        {{ staInviando() ? 'Salvataggio...' : 'Aggiorna' }}
                    </button>

                    <button class="btn btn-danger-solid" type="button" (click)="elimina()">
                        Elimina account
                    </button>
                </div>

            </form>
        </article>

    </div>

    @if (!modificabileDa()) {
    @if (utente()?.seAbbonato)
    {
    <article class="card" style="margin-top: 2rem;">
        <h2>Abbonamento</h2>

        <div class="abbonamento-row">
            <div>
                <strong>Stato:</strong>
                {{ utente()?.seAbbonato ? 'Attivo' : 'Non attivo' }}
            </div>

            @if (utente()?.seAbbonato) {
            <div>
                <strong>Inizio:</strong>
                {{ utente()?.dataInizioAbbonamento | date }}
            </div>

            <div>
                <strong>Tipo:</strong>
                {{ utente()?.tipoAbbonamento }}
            </div>
            }
        </div>
    </article>
    }
    }
    }
</section>
```
</details>