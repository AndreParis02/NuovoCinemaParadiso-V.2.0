import { Component, inject, signal } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
import { AuthService } from '../../../services/auth.service';
import { AbbonamentoService } from '../../../services/abbonamento.service';
import { UtenteService } from '../../../services/utente.service';
import { NavbarSharedStateService } from '../../../services/navbar-shared-state--service.service';
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
  private readonly utenteService = inject(UtenteService);
 private readonly navbarSharedStateService = inject(NavbarSharedStateService);

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

  puoAbbonarsi(): boolean {
      return this.authService.possiedeQualsiasiRuolo(['Utente']);
  }

  puoVisualizzareListaAbbonamenti(): boolean {
      return this.puoAbbonarsi() && !this.authService.utenteCorrente()?.seAbbonato;
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

   abbonati(id: string): void {
        if (id == null)  {
            this.messaggioErrore.set('Nessun abbonamento selezionato');
            return;
        }
        this.utenteService.abbonati(id).subscribe({
            next: () => {
                this.abbonamentoService.ottieniTramiteId(id).subscribe({
                    next: (abbonamento) => {
                        this.abbonamentoScelto.set(abbonamento);
                        this.abbonamentoService.aggiornaStatoAbbonamento(true, abbonamento.nome, new Date().toISOString());
                    },
                    error: (error) => {
                        this.messaggioErrore.set(this.estraiMessaggioErrore(error, 'Abbonamento sottoscritto ma errore nel recupero dettagli'));
                    }
                });
                this.navbarSharedStateService.forzaAggiornamentoSaldo(); 
                this.authService.aggiornaStatoAbbonamento(true);
                this.messaggioSuccesso.set('Abbonamento effettuato con successo');
            },
            error: (error) => {
                this.messaggioErrore.set(this.estraiMessaggioErrore(error, 'Errore durante l\'abbonamento'));
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
