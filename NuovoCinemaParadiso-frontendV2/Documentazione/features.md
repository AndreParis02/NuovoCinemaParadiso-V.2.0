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




# biglietto
## biglietto-list.component.ts V1.0

Andrea Bruno 03-06-2026
Creazione del file (funzionante ma problemi con il refresh della pagina)

```ts
import { Component, OnInit, inject } from '@angular/core';
import { DatePipe } from '@angular/common';
import { Biglietto } from '../../models/biglietto.model';
import { BigliettoService } from '../../services/biglietto.service';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'biglietto-list',
  standalone: true,
  imports: [DatePipe],
  templateUrl: './biglietto-list.component.html',
})
export class BigliettoListComponent implements OnInit {

  biglietto: Biglietto[] = [];          // lista dei biglietti caricati
  messaggio: string | null = null;      // messaggio di errore o info

  private authService = inject(AuthService); // servizio per ottenere l’utente loggato

  constructor(private bigliettoService: BigliettoService) {} // servizio API biglietti

  ngOnInit(): void {

    const utente = this.authService.utenteCorrente(); // recupero utente loggato

    if (!utente?.id) {                 // se non è loggato → messaggio
      this.messaggio = "Utente non loggato";
      return;
    }

    const utenteId = utente.id;        // id dell’utente loggato

    this.bigliettoService.ottieniTutto(utenteId).subscribe({
      next: data => this.biglietto = data,                 // biglietti caricati
      error: () => this.messaggio = "Errore durante il caricamento dei biglietti."
    });
  }
}
```

## biglietto-list.component.html V1.0

Andrea Bruno 03-06-2026
Creazione del file minimale esteticamente da modificare 

```html
<section>

    <!-- Titolo pagina -->
    <h1 class="page-title">I tuoi biglietti</h1>
    <p class="page-subtitle">Visualizza gli acquisti effettuati.</p>

    <!-- Messaggio di errore o info (es. utente non loggato) -->
    @if (messaggio) {
        <div class="alert alert-warning">{{ messaggio }}</div>
    }

    <!-- Caso: nessun biglietto presente -->
    @if (!messaggio && biglietto.length === 0) {
        <p class="muted">Non hai ancora acquistato nessun biglietto.</p>
    }

    <!-- Caso: ci sono biglietti -->
    @if (biglietto.length > 0) {

        <!-- Griglia delle card -->
        <div class="grid grid-2" style="margin-top: 1.5rem;">

            <!-- Ciclo dei biglietti -->
            @for (b of biglietto; track b.id) {

                <article class="card">
                    <h2>Biglietto #{{ b.id }}</h2>

                    <!-- Lista dettagli biglietto -->
                    <div class="list">

                        <div class="list-item">
                            <strong>Film</strong>
                            <p>{{ b.titoloMovie }}</p>
                        </div>

                        <div class="list-item">
                            <strong>Sala</strong>
                            <p>{{ b.nomeSala }}</p>
                        </div>

                        <div class="list-item">
                            <strong>Tipologia sala</strong>
                            <p>{{ b.nomeTipologiaSala }}</p>
                        </div>

                        <div class="list-item">
                            <strong>Numero biglietti</strong>
                            <p>{{ b.numeroBiglietti }}</p>
                        </div>

                        <div class="list-item">
                            <strong>Data di proiezione</strong>
                            <p>{{ b.dataProiezione }}</p>
                        </div>

                        <div class="list-item">
                            <strong>Orario di inizio film</strong>
                            <p>{{ b.oraInizio }}</p>
                        </div>

                        <div class="list-item">
                            <strong>Prezzo totale</strong>
                            <p>{{ b.prezzoFinale }} €</p>
                        </div>

                    </div>

                </article>

            }

        </div>

    }

</section>
```

# cambio-ruolo
## utenti-list.component.ts V1.0

Andrea Bruno 03-06-2026
Creazione del file (funzionante ma problemi con il refresh della pagina)

```ts
import { Component, OnInit, inject } from '@angular/core';
import { OperatoreService } from '../../services/operatore.service';
import { Utente } from '../../models/utente.model';

@Component({
  selector: 'utenti-list',                     // nome del componente usato nel template
  standalone: true,                            // componente standalone
  templateUrl: './utenti-list.component.html'  // file HTML associato
})
export class UtentiListComponent implements OnInit {

  utenti: Utente[] = [];                       // lista utenti caricati dal backend
  messaggioErrore: string | null = null;       // messaggio di errore da mostrare

  private operatoreService = inject(OperatoreService); // servizio per ottenere gli utenti

  ngOnInit(): void {

    this.messaggioErrore = null;               // reset messaggio errore

    // chiamata API per ottenere tutti gli utenti
    this.operatoreService.OttieniUtenti().subscribe({
      next: (items) => {
        this.utenti = items;                   // utenti caricati correttamente
      },
      error: () => {
        this.messaggioErrore = 'Errore nel caricamento utenti'; // errore API
      }
    });
  }
}
```

## utenti-list.component.ts V1.0

Andrea Bruno 03-06-2026
Creazione del file minimale esteticamente da modificare

```html
<section>

    <!-- Titolo della pagina -->
    <h1 class="page-title">Lista utenti</h1>
    <p class="page-subtitle">Visualizza tutti gli utenti registrati.</p>

    <!-- Messaggio di errore (es. problemi nel caricamento) -->
    @if (messaggioErrore) {
        <div class="alert alert-warning">{{ messaggioErrore }}</div>
    }

    <!-- Caso: nessun utente trovato -->
    @if (!messaggioErrore && utenti.length === 0) {
        <p class="muted">Nessun utente trovato.</p>
    }

    <!-- Caso: ci sono utenti -->
    @if (utenti.length > 0) {

        <!-- Griglia delle card utenti -->
        <div class="grid grid-2" style="margin-top: 1.5rem;">

            <!-- Ciclo degli utenti -->
            @for (u of utenti; track u.id) {

                <article class="card">
                    <h2>Utente #{{ u.id }}</h2>

                    <!-- Lista dei dettagli dell'utente -->
                    <div class="list">

                        <div class="list-item">
                            <strong>Nome completo</strong>
                            <p>{{ u.nomeCompleto }}</p>
                        </div>

                        <div class="list-item">
                            <strong>Email</strong>
                            <p>{{ u.email }}</p>
                        </div>

                        <div class="list-item">
                            <strong>Età</strong>
                            <p>{{ u.eta }}</p>
                        </div>

                        <div class="list-item">
                            <strong>Saldo</strong>
                            <p>{{ u.saldo }}</p>
                        </div>

                        <!-- Se l’utente è abbonato, mostra i dettagli dell’abbonamento -->
                        @if (u.seAbbonato) {

                            <div class="list-item">
                                <strong>Tipo di abbonamento</strong>
                                <p>{{ u.tipoAbbonamento }}</p>
                            </div>

                            <div class="list-item">
                                <strong>Data inizio abbonamento</strong>
                                <p>{{ u.dataInizioAbbonamento }}</p>
                            </div>

                        }

                    </div>

                </article>

            }

        </div>

    }

</section>
```