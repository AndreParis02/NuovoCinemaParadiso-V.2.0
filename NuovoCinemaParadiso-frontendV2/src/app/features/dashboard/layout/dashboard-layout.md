# dashboard.layout
<details>
<summary>Versione1.0</summary>

Francesco Lorenzi 05/06/2026

pagina dashboard di layout

## dashboard.layout.ts
```ts
import { Component, inject, signal } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
// servizi
import { AuthService } from '../../../services/auth.service';
//componenti 
import { ProfiloComponent } from '../../profilo/components/profilo.component';
import { BigliettoListComponent } from '../../biglietto/biglietto-list.component';
import { GiftCardListComponent } from '../../giftcard/components/giftcard-list.component';
import { AbbonamentoListComponent } from '../../abbonamento/components/abbonamento-list.component';
//import { AbbonamentoDetailComponent } from '../../abbonamento/components/abbonamento-detail.component';
import { LogListComponent } from '../../log/components/log-list.component';
import { UtenteListComponent } from '../../cambio-ruolo/components/utente-list.component';

@Component({
  selector: 'dashboard-layout',
  standalone: true,
  templateUrl: './dashboard.layout.html',
  imports: [
    ProfiloComponent,
    BigliettoListComponent,
    GiftCardListComponent,
    AbbonamentoListComponent,
    //AbbonamentoDetailComponent, aggiungere il dettaglio dell'abbonamento appena possibile
    LogListComponent,
    UtenteListComponent
]
})
export class DashboardLayoutComponent {

  private readonly authService = inject(AuthService);

  readonly staCaricando = signal(false);
  readonly staInviando = signal(false);
  readonly messaggioErrore = signal('');
  readonly messaggioSuccesso = signal('');

  constructor() {
  }
    //controlli dei ruoli
  isGestore(): boolean {
    return this.authService.possiedeQualsiasiRuolo(['Gestore']);
  }
  isOperatore(): boolean {
    return this.authService.possiedeQualsiasiRuolo(['Operatore']);
  }
  isUtente(): boolean {
    return this.authService.possiedeQualsiasiRuolo(['Utente']);
  }

  private estraiMessaggioErrore(error: unknown, fallback: string): string {
    if (error instanceof HttpErrorResponse) {
      return error.error?.message ?? fallback;
    }
    return fallback;
  }
}
```
## dashboad.layout.html

 questo primo layout molto semplice non è ancora in grado di mostrare l'abbonamneto dell'utente, bisogna prima creare il componente dettaglio per fare ciò.

```html

<profilo></profilo><!-- ho dovutro creare un componente "profilo" riciclando il codice per avere la info di profilo -->
@if (isUtente()) { <!-- giustamente ho inserito visibili solo all'utente la lista dei suoi biglietti e delle sue giftcard  -->
    <div class="divider"></div>
    <h2>Acquisti</h2>
    <biglietto-list></biglietto-list>
    <giftcard-list></giftcard-list>
    }
    <!-- ricordarsi di aggiungere il dettaglio dell'abbonamento appena possibile -->
     <!-- bisognerà poi mettere un controllo per vedere se l'utente è abbonato e di conseguenza mostrare solo il suo abbonamento e no la lista-->
    <h1 class="page-title">Abbonamenti disponibili</h1>
    <abbonamento-list></abbonamento-list>
    
<!-- in questa area di gestione ho inserito tutti componenti che dovrebbero essere visti solo dal gestore (i log, la lista utenti, la lista giftcar)  --> 
@if (isGestore()) {
    <log-list></log-list>
    <utenti-list></utenti-list>
    <giftcard-list></giftcard-list> <!--   --> 
}

```

</details>