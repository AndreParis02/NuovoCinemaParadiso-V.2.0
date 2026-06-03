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

  private readonly authService = inject(AuthService);
  private readonly bigliettoService = inject(BigliettoService);

  readonly biglietti = signal<Biglietto[]>([]);
  readonly staCaricando = signal(false);
  readonly staInviando = signal(false);
  readonly messaggioErrore = signal('');
  readonly messaggioSuccesso = signal('');

  readonly utenteId;
  constructor() {
    const utente = this.authService.utenteCorrente();

    if (!utente?.id) {
      this.messaggioErrore.set("Utente non loggato");
      return;
    }

    this.utenteId = utente.id;
    this.caricaBiglietti();
  }

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

  tracciaPerId(_: string, item: Biglietto): string {
    return item.id;
  }

  elimina(id: string): void {
    if (!confirm("Sei sicuro di voler eliminare questo biglietto?")) return;

    this.staInviando.set(true);
    this.messaggioErrore.set('');
    this.messaggioSuccesso.set('');

    this.bigliettoService.elimina(id).subscribe({
      next: () => {
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

  modifica(id: string) {
  }

  private estraiMessaggioErrore(error: unknown, fallback: string): string {
    if (error instanceof HttpErrorResponse) {
      return error.error?.message ?? fallback;
    }
    return fallback;
  }
}