import { Component, inject, signal } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';

import { AuthService } from '../../../services/auth.service';

import { ProfiloComponent } from '../../profilo/components/profilo.component';
import { BigliettoListComponent } from '../../biglietto/biglietto-list.component';
import { GiftCardListComponent } from '../../giftcard/components/giftcard-list.component';
import { AbbonamentoListComponent } from '../../abbonamento/components/abbonamento-list.component';
//import { AbbonamentoDetailComponent } from '../../abbonamento/components/abbonamento-detail.component';
import { LogListComponent } from '../../log/components/log-list.component';
import { UtenteListComponent } from '../../cambio-ruolo/components/utente-list.component';
import { CreaCodiceComponent } from '../../giftcard/components/crea-codice.component';
import { RiscattaCodiceComponent } from '../../giftcard/components/riscatta-codice.component';

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
    UtenteListComponent,
    CreaCodiceComponent,
    RiscattaCodiceComponent
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