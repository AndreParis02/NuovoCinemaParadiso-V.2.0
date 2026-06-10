import { Component, inject, signal } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';

import { AuthService } from '../../../services/auth.service';
import { TurnoService } from '../../../services/turno.service';


import { Turno } from '../../../models/turno.model';
import { TurnoFormComponent } from "./turno-form.component";


@Component({
  selector: 'turno-list',
  standalone: true,
  templateUrl: './turno-list.component.html',
  imports: [TurnoFormComponent]
})

export class TurnoListComponent {

  private readonly authService = inject(AuthService);
  private readonly turnoService = inject(TurnoService);


  readonly turni = signal<Turno[]>([]);
  readonly turnoScelto = signal<Turno | null>(null);
  readonly staCaricando = signal(false);
  readonly staInviando = signal(false);
  readonly messaggioErrore = signal('');
  readonly messaggioSuccesso = signal('');


  constructor() {
    this.caricaTurni();
  }
  visualizzabileDa(): boolean {
    return this.authService.possiedeQualsiasiRuolo(['Operatore']);
  }
  caricaTurni(): void {

    this.staCaricando.set(true);
    this.messaggioErrore.set('');
    this.turnoService.ottieniTutto().subscribe({

      next: (items) => {
        this.turni.set(items);
        this.staCaricando.set(false);

      },
      error: (error: unknown) => {
        this.staCaricando.set(false);
        this.messaggioErrore.set(this.estraiMessaggioErrore(error, 'turni non trovati'));
      }
    });
  }

  elimina(item: Turno): void {
    if (!this.visualizzabileDa()) {
      return;
    }
    
    if (!confirm(`Sei sicuro di voler eliminare il turno "${item.nome}"?`)) {
      return;
    }

    this.messaggioErrore.set('');
    this.messaggioSuccesso.set('');

    this.turnoService.elimina(item.id).subscribe({
      next: () => {
        if (this.turnoScelto()?.id === item.id) {
          this.turnoScelto.set(null);
        }
        this.caricaTurni();
      },
      error: (error: unknown) => {

        this.messaggioErrore.set(this.estraiMessaggioErrore(error, 'eliminazione non riuscita.'));
      }
    });
  }

  tracciaPerId(_: string, item: Turno): string {
    return item.id;
  }

  private estraiMessaggioErrore(error: unknown, fallback: string): string {

    if (error instanceof HttpErrorResponse) {
      return error.error?.message ?? fallback;
    }
    return fallback;
  }
}
