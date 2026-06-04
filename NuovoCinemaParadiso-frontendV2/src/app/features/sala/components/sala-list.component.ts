import { Component, inject, signal } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';

import { AuthService } from '../../../services/auth.service';
import { SalaService } from '../../../services/sala.service';

import { Sala } from '../../../models/sala.model';
import { SalaFormComponent } from './sala-form.component';


@Component({
  selector: 'sala-list',
  standalone: true,
  templateUrl: './sala-list.component.html',
  imports: [SalaFormComponent]
})

export class SalaListComponent {

  private readonly authService = inject(AuthService);
  private readonly salaService = inject(SalaService);


  readonly sale = signal<Sala[]>([]);
  readonly salaScelta = signal<Sala | null>(null);
  readonly staCaricando = signal(false);
  readonly staInviando = signal(false);
  readonly messaggioErrore = signal('');
  readonly messaggioSuccesso = signal('');


  constructor() {
    this.caricaSale();
  }
  visualizzabileDa(): boolean {
    return this.authService.possiedeQualsiasiRuolo(['Operatore']);
  }
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

  elimina(item: Sala): void {
    if (!this.visualizzabileDa()) {
      return;
    }

    if (!confirm(`Sei sicuro di voler eliminare la sala "${item.nome}"?`)) {
      return;
    }

    this.staInviando.set(true);
    this.salaService.elimina(item.id).subscribe({
      next: () => {
        this.staInviando.set(false);
        this.messaggioSuccesso.set('Sala eliminata con successo');
        this.caricaSale();
      },
      error: (error: unknown) => {
        this.staInviando.set(false);
        this.messaggioErrore.set(this.estraiMessaggioErrore(error, 'Errore durante l\'eliminazione della sala'));
      }
    });
  }

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
