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
- biglietto-list.component.ts [utente,operatore,gestore] Andrea Bruno (fatto)
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
- utenti-list.component.ts [gestore] Andrea Bruno (fatto)

## log
### components
- log-list.component.ts [gestore] Simeone

## conto-cinema
### components
- conto-cinema-detail.component.ts [gestore]

## biglietto
### biglietto-list.component.ts

<details>
<summary>## biglietto-list.component.ts V1.0</summary>

Andrea Bruno 03-06-2026
Creazione del file

```ts
import { Component, inject, signal } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';

import { Biglietto } from '../../models/biglietto.model';
import { BigliettoService } from '../../services/biglietto.service';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'biglietto-list',
  standalone: true,
  templateUrl: './biglietto-list.component.html',
})
export class BigliettoListComponent {

  // Servizi necessari
  private readonly authService = inject(AuthService);
  private readonly bigliettoService = inject(BigliettoService);

  // Stato reattivo del componente
  readonly biglietti = signal<Biglietto[]>([]);
  readonly staCaricando = signal(false);
  readonly staInviando = signal(false);
  readonly messaggioErrore = signal('');
  readonly messaggioSuccesso = signal('');

  // ID dell'utente loggato
  readonly utenteId;

  constructor() {
    // Recupera l'utente corrente
    const utente = this.authService.utenteCorrente();

    // Se non loggato, mostra errore
    if (!utente?.id) {
      this.messaggioErrore.set("Utente non loggato");
      return;
    }

    this.utenteId = utente.id;

    // Carica i biglietti all'avvio
    this.caricaBiglietti();
  }

  // Recupera i biglietti dell'utente
  caricaBiglietti(): void {
    if (!this.utenteId) return;

    this.staCaricando.set(true);
    this.messaggioErrore.set('');

    this.bigliettoService.ottieniTutto().subscribe({
      next: (items) => {
        this.biglietti.set(items);
        this.staCaricando.set(false);
      },
      error: (error: unknown) => {
        this.staCaricando.set(false);
        this.messaggioErrore.set(
          this.estraiMessaggioErrore(error, 
            'Errore durante il caricamento dei biglietti')
        );
      }
    });
  }

  // Funzione di tracking per *ngFor
  tracciaPerId(_: string, item: Biglietto): string {
    return item.id;
  }

  // Elimina un biglietto
  elimina(id: string): void {
    if (!confirm("Sei sicuro di voler eliminare questo biglietto?")) return;

    this.staInviando.set(true);
    this.messaggioErrore.set('');
    this.messaggioSuccesso.set('');

    this.bigliettoService.elimina(id).subscribe({
      next: () => {
        // Rimuove il biglietto dalla lista locale
        this.biglietti.update(lista => lista.filter(b => b.id !== id));
        this.staInviando.set(false);
        this.messaggioSuccesso.set("Biglietto eliminato con successo");
      },
      error: (error: unknown) => {
        this.staInviando.set(false);
        this.messaggioErrore.set(
          this.estraiMessaggioErrore(error, "Errore durante l'eliminazione del biglietto")
        );
      }
    });
  }

  // Placeholder per futura modifica
  modifica(id: string) {
  }

  // Estrae un messaggio leggibile dall'errore HTTP
  private estraiMessaggioErrore(error: unknown, fallback: string): string {
    if (error instanceof HttpErrorResponse) {
      return error.error?.message ?? fallback;
    }
    return fallback;
  }
}
```

### biglietto-list.component.html

<details>
<summary>biglietto-list.component.html V1.0</summary>

Andrea Bruno 03-06-2026
Creazione del file minimale esteticamente da modificare 

```html
<section>

    <!-- Messaggio di errore -->
    @if (messaggioErrore()) {
        <div class="alert alert-warning">{{ messaggioErrore() }}</div>
    }

    <!-- Messaggio di successo -->
    @if (messaggioSuccesso()) {
        <div class="alert alert-success">{{ messaggioSuccesso() }}</div>
    }

    <div class="grid grid-2">

        <article class="card">
            <h2>I tuoi biglietti</h2>

            <!-- Stato di caricamento -->
            @if (staCaricando()) {
                <p class="muted">Caricamento in corso...</p>

            <!-- Nessun biglietto -->
            } @else if (biglietti().length === 0) {
                <p class="muted">Non hai ancora acquistato nessun biglietto.</p>

            <!-- Lista biglietti -->
            } @else {

                <div class="list">

                    <!-- Ciclo dei biglietti -->
                    @for (b of biglietti(); track b.id){

                        <div class="list-item">
                            <div>

                                <!-- Intestazione biglietto -->
                                <strong>Biglietto #{{ b.id }}</strong>

                                <!-- Informazioni principali -->
                                <p>Film: {{ b.titoloMovie }}</p>
                                <p>Sala: {{ b.nomeSala }}</p>
                                <p>Tipologia sala: {{ b.nomeTipologiaSala }}</p>
                                <p>Numero biglietti: {{ b.numeroBiglietti }}</p>
                                <p>Data: {{ b.dataProiezione }}</p>
                                <p>Ora inizio: {{ b.oraInizio }}</p>
                                <p>Prezzo totale: {{ b.prezzoFinale }} €</p>

                                <!-- ID tecnico -->
                                <div class="muted">ID: {{ b.id }}</div>

                                <!-- Pulsanti azione -->
                                <div class="btn-row">
                                    <button class="btn btn-primary" (click)="modifica(b.id)">
                                        Modifica
                                    </button>

                                    <button class="btn btn-danger"
                                            (click)="elimina(b.id)"
                                            [disabled]="staInviando()">
                                        Elimina
                                    </button>
                                </div>

                            </div>
                        </div>

                    }
                </div>

            }
        </article>
    </div>
</section>
```

## cambio-ruolo
### utenti-list.component.ts

<details>
<summary>utenti-list.component.ts V1.0</summary>

Andrea Bruno 03-06-2026
Creazione del file

```ts
import { Component, inject, signal } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';

import { OperatoreService } from '../../services/operatore.service';
import { UtenteService } from '../../services/utente.service';
import { AuthService } from '../../services/auth.service';
import { Utente } from '../../models/utente.model';

@Component({
  selector: 'utenti-list',
  standalone: true,
  templateUrl: './utente-list.component.html'
})
export class UtenteListComponent {

  // Servizi necessari
  private readonly authService = inject(AuthService);
  private readonly operatoreService = inject(OperatoreService);
  private readonly utenteService = inject(UtenteService);

  // Stato reattivo del componente
  readonly utenti = signal<Utente[]>([]);
  readonly staCaricando = signal(false);
  readonly staInviando = signal(false);
  readonly messaggioErrore = signal('');
  readonly messaggioSuccesso = signal('');

  constructor() {
    // Carica gli utenti all'avvio
    this.caricaUtenti();
  }

  // Controlla se l'utente corrente può vedere questa pagina
  visualizzabileDa(): boolean {
    return this.authService.possiedeQualsiasiRuolo(['Operatore']);
  }

  // Recupera la lista degli utenti
  caricaUtenti(): void {
    this.staCaricando.set(true);
    this.messaggioErrore.set('');

    this.operatoreService.OttieniUtenti().subscribe({
      next: (items) => {
        this.utenti.set(items);
        this.staCaricando.set(false);
      },
      error: (error: unknown) => {
        this.staCaricando.set(false);
        this.messaggioErrore.set(
          this.estraiMessaggioErrore(error, 
            'Errore durante il caricamento degli utenti')
        );
      }
    });
  }

  // Tracking per *ngFor
  tracciaPerId(_: string, item: Utente): string {
    return item.id;
  }

  // Elimina un utente
  elimina(id: string): void {
    if (!confirm("Sei sicuro di voler eliminare questo utente?")) return;

    this.staInviando.set(true);
    this.messaggioErrore.set('');
    this.messaggioSuccesso.set('');

    // ⚠️ Nota: eliminaProfilo() elimina l'utente loggato, non quello passato come id
    this.utenteService.eliminaProfilo().subscribe({
      next: () => {
        // Rimuove l'utente dalla lista locale
        this.utenti.update(lista => lista.filter(b => b.id !== id));
        this.staInviando.set(false);
        this.messaggioSuccesso.set("Utente eliminato con successo");
      },
      error: (error: unknown) => {
        this.staInviando.set(false);
        this.messaggioErrore.set(
          this.estraiMessaggioErrore(error, 
            "Errore durante l'eliminazione dell'utente")
        );
      }
    });
  }

  // Placeholder per futura modifica
  modifica(id: string) {
  }

  // Estrae un messaggio leggibile dall'errore HTTP
  private estraiMessaggioErrore(error: unknown, fallback: string): string {
    if (error instanceof HttpErrorResponse) {
      return error.error?.message ?? fallback;
    }
    return fallback;
  }
}
```

### utenti-list.component.html

<details>
<summary>utenti-list.component.html V1.0</summary>

Andrea Bruno 03-06-2026
Creazione del file minimale esteticamente da modificare

```html
<section>

    <!-- Messaggio di errore -->
    @if (messaggioErrore()) {
        <div class="alert alert-warning">{{ messaggioErrore() }}</div>
    }

    <!-- Messaggio di successo -->
    @if (messaggioSuccesso()) {
        <div class="alert alert-success">{{ messaggioSuccesso() }}</div>
    }

    <div class="grid grid-2">

        <article class="card">
            <h2>Lista utenti</h2>

            <!-- Stato di caricamento -->
            @if (staCaricando()) {
                <p class="muted">Caricamento in corso...</p>

            <!-- Nessun utente -->
            } @else if (utenti().length === 0) {
                <p class="muted">Non ci sono utenti.</p>

            <!-- Lista utenti -->
            } @else {

                <div class="list">

                    <!-- Ciclo degli utenti -->
                    @for (u of utenti(); track u.id) {

                        <div class="list-item">
                            <div>

                                <!-- Intestazione -->
                                <strong>Utente #{{ u.id }}</strong>

                                <!-- Informazioni principali -->
                                <p>Nome completo: {{ u.nomeCompleto }}</p>
                                <p>Email: {{ u.email }}</p>
                                <p>Età: {{ u.eta }}</p>
                                <p>Saldo: {{ u.saldo }}</p>

                                <!-- Dati abbonamento se presenti -->
                                @if(u.seAbbonato) {
                                    <p>Tipo di abbonamento: {{ u.tipoAbbonamento }}</p>
                                    <p>Data inizio abbonamento: {{ u.dataInizioAbbonamento }}</p>
                                }

                                <!-- ID tecnico -->
                                <div class="muted">ID: {{ u.id }}</div>

                                <!-- Pulsanti azione -->
                                <div class="btn-row">
                                    <button class="btn btn-primary" (click)="modifica(u.id)">
                                        Modifica
                                    </button>

                                    <button class="btn btn-danger"
                                            (click)="elimina(u.id)"
                                            [disabled]="staInviando()">
                                        Elimina
                                    </button>
                                </div>

                            </div>
                        </div>

                    }
                </div>

            }
        </article>
    </div>
</section>
```