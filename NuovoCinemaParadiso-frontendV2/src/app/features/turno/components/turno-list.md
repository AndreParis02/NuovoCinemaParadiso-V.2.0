## turno-list

### turno-list.component.ts 

<details>
    <summary>
    V1.0
    </summary>

Utente: Andrea Paris
Data: 03/06/2026
Descrizione creazione file .ts di turno-list

```ts
import { Component, inject, signal } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';

import { AuthService } from '../../../services/auth.service';
import { TurnoService } from '../../../services/turno.service';


import { Turno } from '../../../models/turno.model';
import { TurnoFormComponent } from "./turno-form.component";


@Component({
  selector: 'turno-list',
  standalone: true,
  templateUrl: './turno-list.component.html',
  imports: [TurnoFormComponent]
})

export class TurnoListComponent {

  private readonly authService = inject(AuthService);
  private readonly turnoService = inject(TurnoService);


  readonly turni = signal<Turno[]>([]);
  readonly turnoScelto = signal<Turno | null>(null);
  readonly staCaricando = signal(false);
  readonly staInviando = signal(false);
  readonly messaggioErrore = signal('');
  readonly messaggioSuccesso = signal('');


  constructor() {
    this.caricaTurni();
  }
  visualizzabileDa(): boolean {
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

  elimina(item: Turno): void {
    if (!this.visualizzabileDa()) {
      return;
    }
    
    if (!confirm(`Sei sicuro di voler eliminare il turno "${item.nome}"?`)) {
      return;
    }

    this.messaggioErrore.set('');
    this.messaggioSuccesso.set('');

    this.turnoService.elimina(item.id).subscribe({
      next: () => {
        if (this.turnoScelto()?.id === item.id) {
          this.turnoScelto.set(null);
        }
        this.caricaTurni();
      },
      error: (error: unknown) => {

        this.messaggioErrore.set(this.estraiMessaggioErrore(error, 'eliminazione non riuscita.'));
      }
    });
  }

  tracciaPerId(_: string, item: Turno): string {
    return item.id;
  }

  private estraiMessaggioErrore(error: unknown, fallback: string): string {

    if (error instanceof HttpErrorResponse) {
      return error.error?.message ?? fallback;
    }
    return fallback;
  }
}

```

</details>

### turno-list.component.html

<details>
    <summary>
    V1.0
    </summary>

Utente: Andrea Paris
Data: 03/06/2026
Descrizione creazione file .html di turno-list

```html
<section>
    @if (messaggioErrore()) {
    <div class="alert alert-warning">{{ messaggioErrore() }}</div>
    }
    @if (messaggioSuccesso()) {
    <div class="alert alert-success">{{ messaggioSuccesso() }}</div>
    }

    <div class="grid grid-2">
        <article class="card">
            <h2>Lista Turni</h2>

            @if (staCaricando()) {
            <p class="muted">Caricamento in corso...</p>
            } @else if (turni().length === 0) {
            <p class="muted">Nessun turno presente.</p>
            } @else {
            <div class="list">
                @for (item of turni(); track tracciaPerId($index.toString(), item)) {
                <div class="list-item">
                    <div>
                        <strong>{{ item.nome }}</strong>
                        <p>Ora Inizio: {{ item.oraInizio.substring(0, 5) }}</p>
                        <p>Ora Fine: {{ item.oraFine.substring(0, 5) }}</p>
                        @if(visualizzabileDa()) {
                        <div class="btn-row" style="margin-top: 1rem;">
                            <button class="btn btn-secondary" type="button" (click)="turnoScelto.set(item)">Modifica</button>
                                <button class="btn btn-secondary" type="button" (click)="elimina(item)">Elimina</button>

                        </div>
                        }
                    </div>
                </div>
                }
            </div>
            }
        </article>
        @if (visualizzabileDa()) {
        <article class="card">
            <turno-form [turnoSelezionato]="turnoScelto()" (modificaCompletata)="caricaTurni()"></turno-form>
        </article>
        }
    </div>
```

</details>

## turno-form

### turno-form.component.ts 

<details>
    <summary>
    V1.0
    </summary>

Utente: Andrea Paris
Data: 05/06/2026
Descrizione creazione file .ts di turno-form

```ts
import { Component, inject, signal, input, effect,output } from '@angular/core';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { HttpErrorResponse } from '@angular/common/http';

import { AuthService } from '../../../services/auth.service';
import { TurnoService } from '../../../services/turno.service';

import { Turno } from '../../../models/turno.model';



@Component({
    selector: 'turno-form',
    standalone: true,
    imports: [ReactiveFormsModule],
    templateUrl: './turno-form.component.html'
})

export class TurnoFormComponent {

    private readonly formBuilder = inject(FormBuilder);
    private readonly authService = inject(AuthService);
    private readonly turnoService = inject(TurnoService);

    readonly modificaCompletata = output<void>();

    readonly turnoSelezionato = input<Turno | null>(null);
    readonly staCaricando = signal(false);
    readonly staInviando = signal(false);
    readonly messaggioErrore = signal('');
    readonly messaggioSuccesso = signal('');
    readonly modificaId = signal<string>('');


    readonly form = this.formBuilder.nonNullable.group({
        nome: ['', [Validators.required, Validators.maxLength(100)]],
        oraInizio: ['', [Validators.required]],
        oraFine: ['', [Validators.required]]
    });

    constructor() {

        effect(() => {
            const turno = this.turnoSelezionato();
            if (turno) {
                this.inizioModifica(turno);
            } else {
                this.ripristinaForm();
            }
        });
    }
    modificabileDa(): boolean {
        return this.authService.possiedeQualsiasiRuolo(['Operatore']);
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
                
                this.modificaCompletata.emit();
                this.ripristinaForm();
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

        this.form.patchValue({ nome: item.nome, oraInizio: item.oraInizio, oraFine: item.oraFine });
        this.messaggioErrore.set('');
        this.messaggioSuccesso.set('');
    }
    

    ripristinaForm(): void {
        this.modificaId.set('');
        this.form.reset({ nome: '', oraInizio: '', oraFine: ''});
    }
    tracciaPerId(_: string, item: Turno): string {
        return item.id;
    }

    private estraiMessaggioErrore(error: unknown, fallback: string): string {

        if (error instanceof HttpErrorResponse) {
            return error.error?.message ?? fallback;
        }
        return fallback;
    }
}
```

</details>

### turno-form.component.html

<details>
    <summary>
    V1.0
    </summary>

Utente: Andrea Paris
Data: 05/06/2026
Descrizione creazione file .html di turno-form

```html

<section>
        @if (messaggioErrore()) {
        <div class="alert alert-warning">{{ messaggioErrore() }}</div>
        }

        @if (messaggioSuccesso()) {
        <div class="alert alert-success">{{ messaggioSuccesso() }}</div>
        }

        <div class="grid grid-2">
            <article class="card">
                <h2>{{ modificaId() ? 'Modifica turno' : 'Aggiungi turno' }}</h2>

                <form class="form-grid" [formGroup]="form" (ngSubmit)="invia()">
                    <div>
                        <label for="nome">Nome</label>
                        <input id="nome" type="text" formControlName="nome" [disabled]="!modificabileDa()">
                        <label for="oraInizio">Ora Inizio</label>
                        <input id="oraInizio" type="time" step="60" formControlName="oraInizio"
                            [disabled]="!modificabileDa()">
                        <label for="oraFine">Ora Fine</label>
                        <input id="oraFine" type="time" step="60" formControlName="oraFine"
                            [disabled]="!modificabileDa()">
                    </div>

                    <div class="btn-row">
                        <button class="btn btn-primary" type="submit" [disabled]="staInviando() || !modificabileDa()">
                            {{ staInviando() ? 'Salvataggio...' : (modificaId() ? 'Aggiorna' : 'Crea') }}
                        </button>
                        @if (modificaId()) {
                        <button class="btn btn-secondary" type="button" (click)="ripristinaForm()">Annulla</button>
                        }
                    </div>
                </form>
            </article>
        </div> 
</section>
```

</details>

