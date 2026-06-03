import { Component, OnInit, inject } from '@angular/core';
import { OperatoreService } from '../../services/operatore.service';
import { Utente } from '../../models/utente.model';

@Component({
  selector: 'utenti-list',
  standalone: true,
  templateUrl: './utenti-list.component.html'
})
export class UtentiListComponent implements OnInit {

  utenti: Utente[] = [];
  messaggioErrore: string | null = null;

  private operatoreService = inject(OperatoreService);

  ngOnInit(): void {
    this.messaggioErrore = null;

    this.operatoreService.OttieniUtenti().subscribe({
      next: (items) => {
        this.utenti = items;
      },
      error: () => {
        this.messaggioErrore = 'Errore nel caricamento utenti';
      }
    });
  }
}