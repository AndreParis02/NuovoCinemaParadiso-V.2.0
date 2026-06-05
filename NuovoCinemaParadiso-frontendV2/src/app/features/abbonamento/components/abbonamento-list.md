## abbonamento-list

### abbonamento-list.component.ts 

<details>
    <summary>
    V1.0
    </summary>

Utente: Andrea Paris
Data: 03/06/2026
Descrizione creazione file .ts di abbonamento-list

```ts
import { Component, inject, signal } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';

import { AuthService } from '../../../services/auth.service';
import { AbbonamentoService } from '../../../services/abbonamento.service';


import { Abbonamento } from '../../../models/abbonamento.model';
import { AbbonamentoFormComponent } from "./abbonamento-form.component";


@Component({
  selector: 'abbonamento-list',
  standalone: true,
  templateUrl: './abbonamento-list.component.html',
  imports: [AbbonamentoFormComponent]
})

export class AbbonamentoListComponent {

  private readonly authService = inject(AuthService);
  private readonly abbonamentoService = inject(AbbonamentoService);


  readonly abbonamenti = signal<Abbonamento[]>([]);
  readonly abbonamentoScelto = signal<Abbonamento | null>(null);
  readonly staCaricando = signal(false);
  readonly staInviando = signal(false);
  readonly messaggioErrore = signal('');
  readonly messaggioSuccesso = signal('');


  constructor() {
    this.caricaAbbonamenti();
  }
  visualizzabileDa(): boolean {
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

  elimina(item: Abbonamento): void {
    if (!this.visualizzabileDa()) {
      return;
    }
    
    if (!confirm(`Sei sicuro di voler eliminare l'abbonamento "${item.nome}"?`)) {
      return;
    }

    this.messaggioErrore.set('');
    this.messaggioSuccesso.set('');

    this.abbonamentoService.elimina(item.id).subscribe({
      next: () => {
        if (this.abbonamentoScelto()?.id === item.id) {
          this.abbonamentoScelto.set(null);
        }
        this.caricaAbbonamenti();
      },
      error: (error: unknown) => {

        this.messaggioErrore.set(this.estraiMessaggioErrore(error, 'eliminazione non riuscita.'));
      }
    });
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

```

</details>

### abbonamento-list.component.html

<details>
    <summary>
    V1.0
    </summary>

Utente: Andrea Paris
Data: 03/06/2026
Descrizione creazione file .html di abbonamento-list

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
            <h2>Lista Abbonamenti</h2>

            @if (staCaricando()) {
            <p class="muted">Caricamento in corso...</p>
            } @else if (abbonamenti().length === 0) {
            <p class="muted">Nessun abbonamento presente.</p>
            } @else {
            <div class="list">
                @for (item of abbonamenti(); track tracciaPerId($index.toString(), item)) {
                <div class="list-item">
                    <div>
                        <strong>{{ item.nome }}</strong>
                        <p>{{ item.prezzo }} €</p>
                        <p>{{ item.durata }}</p>
                        <p>{{ item.sconto }} €</p>
                        <div class="muted">ID: {{ item.id }}</div>
                        @if(visualizzabileDa()) {
                        <div class="btn-row" style="margin-top: 1rem;">
                            <button class="btn btn-secondary" type="button" (click)="abbonamentoScelto.set(item)">Modifica</button>
                            <button class="btn btn-secondary" type="button" (click)="elimina(item)" [disabled]="staInviando()">Elimina</button>
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
            <abbonamento-form [abbonamentoSelezionato]="abbonamentoScelto()" (modificaCompletata)="caricaAbbonamenti()"></abbonamento-form>
        </article>
        }
    </div>
```

</details>

## abbonamento-form

### abbonamento-form.component.ts 

<details>
    <summary>
    V1.0
    </summary>

Utente: Andrea Paris
Data: 04/06/2026
Descrizione creazione file .ts di abbonamento-form

```ts
import { Component, inject, signal, input, effect } from '@angular/core';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { HttpErrorResponse } from '@angular/common/http';

import { AuthService } from '../../../services/auth.service';
import { AbbonamentoService } from '../../../services/abbonamento.service';

import { Abbonamento } from '../../../models/abbonamento.model';



@Component({
    selector: 'abbonamento-form',
    standalone: true,
    imports: [ReactiveFormsModule],
    templateUrl: './abbonamento-form.component.html'
})

export class AbbonamentoFormComponent {

    private readonly formBuilder = inject(FormBuilder);
    private readonly authService = inject(AuthService);
    private readonly abbonamentoService = inject(AbbonamentoService);


    readonly abbonamentoSelezionato = input<Abbonamento | null>(null);
    readonly staCaricando = signal(false);
    readonly staInviando = signal(false);
    readonly messaggioErrore = signal('');
    readonly messaggioSuccesso = signal('');
    readonly modificaId = signal<string>('');


    readonly form = this.formBuilder.nonNullable.group({
        nome: ['', [Validators.required, Validators.maxLength(100)]],
        durata: [1, [Validators.required, Validators.min(1)]],
        prezzo: [1, [Validators.required, Validators.min(1)]],
        sconto: [1, [Validators.required, Validators.min(1), Validators.max(100)]]
    });

    constructor() {

        effect(() => {
            const abbonamento = this.abbonamentoSelezionato();
            if (abbonamento) {
                this.inizioModifica(abbonamento);
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

            ? this.abbonamentoService.modifica(this.modificaId(), this.form.getRawValue())
            : this.abbonamentoService.crea(this.form.getRawValue());


        request$.subscribe({
            next: () => {
                this.staInviando.set(false);
                this.messaggioSuccesso.set(this.modificaId() ? 'Abbonamento aggiornato.' : 'Abbonamento creato.');
                this.ripristinaForm();
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

    ripristinaForm(): void {
        this.modificaId.set('');
        this.form.reset({ nome: '', durata: 0, prezzo: 0, sconto: 0});
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
```

</details>

### abbonamento-form.component.html

<details>
    <summary>
    V1.0
    </summary>

Utente: Andrea Paris
Data: 04/06/2026
Descrizione creazione file .html di abbonamento-form

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
                <h2>{{ modificaId() ? 'Modifica abbonamento' : 'Aggiungi abbonamento' }}</h2>

                <form class="form-grid" [formGroup]="form" (ngSubmit)="invia()">
                    <div>
                        <label for="nome">Nome</label>
                        <input id="nome" type="text" formControlName="nome" [disabled]="!modificabileDa()">
                        <label for="durata">Durata</label>
                        <input id="durata" type="number" formControlName="durata"
                            [disabled]="!modificabileDa()">
                        <label for="prezzo">Prezzo (€)</label>
                        <input id="prezzo" type="number" formControlName="prezzo"
                            [disabled]="!modificabileDa()">
                        <label for="sconto">Sconto (€)</label>
                        <input id="sconto" type="number" formControlName="sconto"
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
