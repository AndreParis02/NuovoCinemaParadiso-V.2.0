import { Component, inject, signal } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';

import { AuthService } from '../../../services/auth.service';
import { SalaService } from '../../../services/sala.service';


import { Sala } from '../../../models/sala.model';


@Component({
  selector: 'sala-list',
  standalone: true,
  templateUrl: './sala-list.component.html'
})

export class SalaListComponent {

  private readonly authService = inject(AuthService);
  private readonly salaService = inject(SalaService);


  readonly sale = signal<Sala[]>([]);
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
