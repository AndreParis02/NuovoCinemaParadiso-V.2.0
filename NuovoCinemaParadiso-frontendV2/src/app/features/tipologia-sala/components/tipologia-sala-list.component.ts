import { Component, inject, signal } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';

import { AuthService } from '../../../services/auth.service';
import { TipologiaSalaService } from '../../../services/tipologia-sala.service';
import { TipologiaSala } from '../../../models/tipologia-sala.model';
import { TipologiaSalaFormComponent } from "./tipologia-sala-form.component";

@Component({
  selector: 'tipologia-sala-list',
  standalone: true,
  templateUrl: './tipologia-sala-list.component.html',
  imports: [TipologiaSalaFormComponent]
})
export class TipologiaSalaList {
  private readonly authService = inject(AuthService);
  private readonly tipologiaSalaService = inject(TipologiaSalaService);

  readonly tipologie = signal<TipologiaSala[]>([]);
  readonly tipologiaScelta = signal<TipologiaSala | null>(null);
  readonly staCaricando = signal(false);
  readonly staInviando = signal(false);
  readonly messaggioErrore = signal('');
  readonly messaggioSuccesso = signal('');

  constructor() {
    this.caricaTipologie();
  }

  visualizzabileDa(): boolean {
    return this.authService.possiedeQualsiasiRuolo(['Operatore']);
  }

  caricaTipologie(): void {
    this.staCaricando.set(true);
    this.messaggioErrore.set('');
    this.tipologiaSalaService.ottieniTutto().subscribe({
      next: (items) => {
        this.tipologie.set(items);
        this.staCaricando.set(false);
      },
      error: (error: unknown) => {
        this.staCaricando.set(false);
        this.messaggioErrore.set(this.estraiMessaggioErrore(error, 'tipologie sala non trovate'));
      }
    });
  }

  elimina(item: TipologiaSala): void {
    if (!this.visualizzabileDa()) {
      return;
    }
    const confirmed = confirm(`Eliminare la tipologia sala \"${item.nome}\"?`);
    if (!confirmed) {
      return;
    }

    this.messaggioErrore.set('');
    this.messaggioSuccesso.set('');

    this.tipologiaSalaService.elimina(item.id, item).subscribe({
      next: () => {
        if (this.tipologiaScelta()?.id === item.id) {
          this.tipologiaScelta.set(null);
        } 
        this.caricaTipologie();
      },
      error: (error: unknown) => {
        this.messaggioErrore.set(this.estraiMessaggioErrore(error, 'eliminazione non riuscita.'));
      }
    });
  }

  tracciaPerId(_: string, item: TipologiaSala): string {
    return item.id;
  }

  private estraiMessaggioErrore(error: unknown, fallback: string): string {
    if (error instanceof HttpErrorResponse) {
      return error.error?.message ?? fallback;
    }
    return fallback;
  }
}