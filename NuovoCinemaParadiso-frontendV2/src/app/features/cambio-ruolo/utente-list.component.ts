import { Component, OnInit, inject } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';

import { OperatoreService } from '../../services/operatore.service';
import { Utente } from '../../models/utente.model';

@Component({
  selector: 'utenti-list',
  standalone: true,
  templateUrl: './utente-list.component.html'
})
export class UtenteListComponent implements OnInit {

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