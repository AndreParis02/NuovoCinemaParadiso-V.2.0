import { Component, inject, signal } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';

import { AuthService } from '../../../services/auth.service';
import { AbbonamentoService } from '../../../services/abbonamento.service';
import { UtenteService } from '../../../services/utente.service';

import { ProfiloComponent } from '../../profilo/components/profilo.component';
import { BigliettoListComponent } from '../../biglietto/biglietto-list.component';
import { GiftCardListComponent } from '../../giftcard/components/giftcard-list.component';
import { AbbonamentoListComponent } from '../../abbonamento/components/abbonamento-list.component';
import { LogListComponent } from '../../log/components/log-list.component';
import { UtenteListComponent } from '../../cambio-ruolo/components/utente-list.component';
import { CreaCodiceComponent } from '../../giftcard/components/crea-codice.component';
import { ProiezioneList } from '../../proiezione/components/proiezione-list.component';
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
    ProiezioneList,
    LogListComponent,
    UtenteListComponent,
    CreaCodiceComponent,
    RiscattaCodiceComponent
]
})
export class DashboardLayoutComponent {

  private readonly authService = inject(AuthService);
  private readonly abbonamentoService = inject(AbbonamentoService);
  private readonly utenteService = inject(UtenteService);

  readonly sessioneUtente = this.authService.utenteCorrente;
  readonly staCaricando = signal(false);
  readonly staInviando = signal(false);
  readonly messaggioErrore = signal('');
  readonly messaggioSuccesso = signal('');

  constructor() {
  }

  ngOnInit(): void {
    this.utenteService.profilo().subscribe({
      next: (utenteServer) => {
        // Sincronizza lo stato basandosi sul backend (se ha un abbonamentoId, è abbonato)
        const haAbbonamento = !!utenteServer.abbonamentoId;
        this.abbonamentoService.aggiornaStatoAbbonamento(haAbbonamento, utenteServer.tipoAbbonamento ?? null, utenteServer.dataInizioAbbonamento);
      },
      error: (err) => this.messaggioErrore.set(this.estraiMessaggioErrore(err, 'Errore nel caricamento profilo'))
    });
  }

  isGestore(): boolean {
    return this.authService.isGestore();
  }
  isOperatore(): boolean {
    return this.authService.isOperatore();
  }
  isUtente(): boolean {
    return this.authService.isUtente();
  }
  seAbbonato(): boolean {
    return this.sessioneUtente()?.seAbbonato ?? false;
  }

  private estraiMessaggioErrore(error: unknown, fallback: string): string {
    if (error instanceof HttpErrorResponse) {
      return error.error?.message ?? fallback;
    }
    return fallback;
  }
}