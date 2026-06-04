# Features

## Dashboard
è una pagina dove tutti sono autorizzati ad entrare, in questa pagina si ha una visibilità differente in base all'autorizzazione:

- Guest: 
    - proiezioni
    - abbonamenti
- Utente: 
    - proiezioni
    - abbonamenti
    - profilo
    - abbonamento a cui l'utente è abbonato
- Operatore: 
- Gestore:

## abbonamento
### page
- abbonamento.page.ts
### components
- abbonamento-list.component.ts [tutti] - Andrea Paris
- abbonamento-detail.component.ts [utente](abbonato)
- abbonamento-form.component.ts [operatore]

## biglietto
### components
- biglietto-list.component.ts [utente,operatore,gestore] Andrea Bruno
- biglietto-form.component.ts [operatore]

## dashboard
### layout 
- dashboard.layout.ts
 

## profilo
### components
- profilo.component.ts [utente,operatore,gestore]


## genere-movie
### components
- genere-movie-list.component.ts [tutti] Greg

## giftcard
### components
- riscatta-codice.component.ts
- crea-codice.component.ts
- giftcard-list.component.ts [gestore,utente] Simeone

## auth 
### page
### components
- login.component.ts
- register.component.ts

## movie
### components
- movie-list.component.ts [operatore] Francesco
- movie-form.component.ts [operatore]

## proiezione
### page
### components
- proiezione-list.component.ts [tutti] Fabio

Utente: Fabio Tammaro
Data: 03/06/2026
Descrizione: Creato il component list per la proiezione e modificato il biglietto service proiezione service, oltre ai modelli di proiezione.
<details>
<summary>proiezione-list.component.ts Versione 1.0</summary>

```typescript
  import { Component, computed, inject, signal } from '@angular/core';
  import { HttpErrorResponse } from '@angular/common/http';
  import { ProiezioneService } from '../../../services/proiezione.service';
  import { AuthService } from '../../../services/auth.service';
  import { Proiezione } from '../../../models/proiezione.model';
  import { BigliettoService } from '../../../services/biglietto.service';
  import { RouterLink } from '@angular/router';
  import { CommonModule } from '@angular/common';
  //import FormsModule per avere la possibilità di scegliere quanti biglietti acquistare.
  import { FormsModule } from '@angular/forms';

  @Component({
    selector: 'proiezione-list',
    standalone: true,
    imports: [RouterLink, CommonModule, FormsModule],
    templateUrl: './proiezione-list.html',
  })
  export class ProiezioneList {

    private readonly proiezioneService = inject(ProiezioneService);
    private readonly authService = inject(AuthService);
    private readonly bigliettoService = inject(BigliettoService);

    // =========================
    // STATE
    // =========================
    readonly listaProiezioni = signal<Proiezione[]>([]);
    readonly staCaricando = signal(false);
    readonly staInviando = signal(false);
    readonly messaggioErrore = signal('');
    readonly messaggioSuccesso = signal('');
    readonly modificaId = signal<string | null>(null);

    readonly isOperatore = computed(() => this.authService.isOperatore());
    readonly isGestore = computed(() => this.authService.isGestore());

    // quantità per proiezione
    quantitaSelezionata: Record<string, number> = {};

    // loading per singola proiezione
    readonly loadingMap = signal<Record<string, boolean>>({});

    constructor() {
      this.ottieniTutto();
    }

    // =========================
    // LOAD LISTA
    // =========================
    ottieniTutto(): void {
      this.staCaricando.set(true);
      this.messaggioErrore.set('');

      this.proiezioneService.ottieniTutto().subscribe({
        next: (data) => {
          this.listaProiezioni.set(data);

          // inizializza quantità a 1 per ogni proiezione
          const init: Record<string, number> = {};
          data.forEach(p => {
            init[p.id] = 1;
          });
          this.quantitaSelezionata = init;
          this.staCaricando.set(false);
        },
        error: (err) => {
          this.staCaricando.set(false);
          this.messaggioErrore.set(
            this.estraiMessaggioErrore(err, 'Errore caricamento proiezioni')
          );
        }
      });
    }

    // =========================
    // ACQUISTO BIGLIETTI
    // =========================
    acquista(proiezioneId: string) {

        const numeroBiglietti = this.quantitaSelezionata[proiezioneId] ?? 1;

      this.loadingMap.update(m => ({
        ...m,
        [proiezioneId]: true
      }));

      this.messaggioErrore.set('');
      this.messaggioSuccesso.set('');

      this.bigliettoService.crea({
        proiezioneId,
        numeroBiglietti
      }).subscribe({
        next: () => {
          this.loadingMap.update(m => ({
            ...m,
            [proiezioneId]: false
          }));

          this.messaggioSuccesso.set('Biglietti acquistati con successo!');
        },
        error: (err) => {
          this.loadingMap.update(m => ({
            ...m,
            [proiezioneId]: false
          }));

          this.messaggioErrore.set(
            this.estraiMessaggioErrore(err, 'Errore creazione biglietto')
          );
        }
      });
    }

    elimina(item: Proiezione): void {
    if (!this.isOperatore()) {
      return;
    }

    const confirmed = confirm(`Eliminare la proiezione\"${item.id}\"?`)

    if (!confirmed) {
      return;
    }

    this.messaggioErrore.set('');
    this.messaggioSuccesso.set('');

    this.proiezioneService.elimina(item.id).subscribe({
      next: () => {
        this.messaggioSuccesso.set('Proiezione eliminata');

        this.ottieniTutto();
      },
      error: (error: unknown) => {
        this.messaggioErrore.set(this.estraiMessaggioErrore(error, 'Eliminazione non riuscita'));

      }
    });
  }

    // =========================
    // ERROR HANDLER
    // =========================
    private estraiMessaggioErrore(error: unknown, fallback: string): string {
      if (error instanceof HttpErrorResponse) {
        return error.error?.message ?? error.error?.messaggio ?? fallback;
      }

      return fallback;
    }
  }
```
</details>

<details>
<summary>proiezione-list.component.html Versione 1.0</summary>

```html
<section>
    <h1 class="page-title">Proiezioni</h1>
    @if(!isOperatore()){
    <p class="page-subtitle">
        Visualizza le proiezioni del cinema e acquista i biglietti.
    </p>
    }
    @else{
    <p class="page-subtitle">
        Visualizza e gestisci le proiezioni del cinema.
    </p>
    }

    @if (messaggioErrore()) {
    <div class="alert alert-warning">
        {{ messaggioErrore() }}
    </div>
    }

    @if (messaggioSuccesso()) {
    <div class="alert alert-success">
        {{ messaggioSuccesso() }}
    </div>
    }

    <div class="grid grid-2">

        <article class="card">
            <h2>Lista Proiezioni</h2>

            @if (staCaricando()) {
            <p class="muted">Caricamento in corso...</p>

            } @else if (listaProiezioni().length === 0) {

            <p class="muted">Nessuna proiezione disponibile.</p>

            } @else {

            <div class="list">

                @for (item of listaProiezioni(); track item.id) {

                <div class="list-item">

                    <div>
                        <strong>{{ item.titoloMovie }}</strong>

                        <p>{{ item.dataProiezione | date:'dd/MM/yyyy' }}</p>
                        <p>Sala: {{ item.nomeSala }}</p>
                        <p>Turno: {{ item.nomeTurno }}</p>
                    </div>
                    @if(!isGestore() && !isOperatore()){

                    <div class="btn-row">

                        <input type="number" min="1" class="form-control" [(ngModel)]="quantitaSelezionata[item.id]" />

                        <button class="btn btn-secondary" type="button" (click)="acquista(item.id)">
                            Acquista
                        </button>

                    </div>
                    }
                    @if(isOperatore()){
                                <div class="btn-row">
                                    <button class="btn btn-secondary" type="button"> Crea</button>
                                    <button class="btn btn-secondary" type="button"> Modifica</button>
                                    <button class="btn btn-secondary" type="button" (click)="elimina(item)"> Elimina</button>
                                </div>
                            }
                </div>

                }

            </div>

            }

        </article>

    </div>

</section>
```
</details>

- proiezione-form.component.ts [operatore]
- proiezione-detail.component.ts [tutti]

## sala
### components
- sala-list.component.ts [operatore] Francesco
- sala-form.component.ts [operatore]

## tipologia-sala
### components
- tipologia-sala-list.component.ts [operatore] Greg
- tipologia-sala-form.component.ts [operatore]

## turno
### components
- turno-list.component.ts [operatore] Andrea paris
- turno-form.component.ts [operatore]

## cambio-ruolo
### components
- cambio-ruolo-form.component.ts [gestore]
- utenti-list.component.ts [gestore] Andrea Bruno

## log
### components
- log-list.component.ts [gestore] Simeone

## conto-cinema
### components
- conto-cinema-detail.component.ts [gestore]


