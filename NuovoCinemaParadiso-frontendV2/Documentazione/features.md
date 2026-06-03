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
- proiezione-form.component.ts [operatore]
- proiezione-detail.component.ts [tutti]

## sala
### components
- sala-list.component.ts [operatore] Francesco
<details>
<summary>versione1.0</summary>

Francesco Lorenzi 03/06/2026

creazione del componente lista sala

## sala-list.component.ts
```ts
import { Component, inject, signal } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
// servizi
import { AuthService } from '../../../services/auth.service';
import { SalaService } from '../../../services/sala.service';

// modelli
import { Sala } from '../../../models/sala.model';


@Component({
  selector: 'sala-list',
  standalone: true,
  templateUrl: './sala-list.component.html'
})

export class SalaListComponent {
// servizi
  private readonly authService = inject(AuthService);
  private readonly salaService = inject(SalaService);
// modelli
  readonly sale = signal<Sala[]>([]);

  readonly staCaricando = signal(false);
  readonly staInviando = signal(false);
  readonly messaggioErrore = signal('');
  readonly messaggioSuccesso = signal('');


  constructor() {
    this.caricaSale();
  }
  // la lista delle sale è visualizzabile solo dall'operatore
  visualizzabileDa(): boolean {
    return this.authService.possiedeQualsiasiRuolo(['Operatore']);
  }
  // carica le sale sulla lista
  caricaSale(): void {

    this.staCaricando.set(true);
    this.messaggioErrore.set('');
    this.salaService.ottieniTutto().subscribe({

      next: (items) => {
        this.sale.set(items);
        this.staCaricando.set(false);

      },
      error: (error: unknown) => {
        this.staCaricando.set(false);
        this.messaggioErrore.set(this.estraiMessaggioErrore(error, 'sale non trovate'));
      }
    });
  }
// traccia la lista per ID
  tracciaPerId(_: string, item: Sala): string {
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
## sala-list.component.html
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
            <h2>Lista Sale</h2>

            @if (staCaricando()) {
            <p class="muted">Caricamento in corso...</p>
            } @else if (sale().length === 0) {
            <p class="muted">Nessuna sala presente.</p>
            } @else {
            <div class="list">
                @for (item of sale(); track tracciaPerId($index.toString(), item)) {
                <div class="list-item">
                    <div>
                        <strong>{{ item.nome }}</strong>
                        <p>Tipologia: {{ item.nomeTipologia }}</p>
                        <p>Capienza: {{ item.capienza }}</p>
                        <div class="muted">ID: {{ item.id }}</div>
                    </div>    
                </div>
                }
            </div>
            }
        </article>
```
</details>

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


