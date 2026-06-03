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

  private readonly authService = inject(AuthService);
  private readonly operatoreService = inject(OperatoreService);
  private readonly utenteService = inject(UtenteService);

  readonly utenti = signal<Utente[]>([]);
  readonly staCaricando = signal(false);
  readonly staInviando = signal(false);
  readonly messaggioErrore = signal('');
  readonly messaggioSuccesso = signal('');

  constructor() {
    this.caricaUtenti();
  }

  visualizzabileDa(): boolean {
    return this.authService.possiedeQualsiasiRuolo(['Operatore']);
  }

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

  tracciaPerId(_: string, item: Utente): string {
      return item.id;
  }

  elimina(id: string): void {
    if (!confirm("Sei sicuro di voler eliminare questo utente?")) return;

    this.staInviando.set(true);
    this.messaggioErrore.set('');
    this.messaggioSuccesso.set('');

    this.utenteService.eliminaProfilo().subscribe({
      next: () => {
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

  modifica(id: string) {
  }

  private estraiMessaggioErrore(error: unknown, fallback: string): string {
    if (error instanceof HttpErrorResponse) {
      return error.error?.message ?? fallback;
    }
    return fallback;
  }
}