import { Component, inject, signal, OnInit } from '@angular/core';
import { DatePipe, CurrencyPipe } from '@angular/common';
import { GestioneService } from '../../../services/gestione.service';
import { LogAzioni } from '../../../models/gestione.model';

@Component({
  selector: 'log-list',
  standalone: true,
  imports: [DatePipe, CurrencyPipe],
  templateUrl: './log-list.component.html',
  
})
export class LogListComponent implements OnInit {
  private readonly gestioneService = inject(GestioneService);

  readonly logs = signal<LogAzioni[]>([]);

  readonly isLoading = signal(false);
  readonly errorMessage = signal('');

  ngOnInit(): void {
    this.caricaLogs();
  }

  caricaLogs(): void {
    this.isLoading.set(true);
    this.errorMessage.set('');

    this.gestioneService.ottieniLog().subscribe({
      next: (data) => {
        this.logs.set(data);
        this.isLoading.set(false);
      },
      error: (error: unknown) => {
        console.error('ERRORE LOG AZIONI:', error);

        this.isLoading.set(false);
        this.errorMessage.set('Si è verificato un errore nel caricamento dei dati.');
      },
    });
  }

  trackById(_: number, item: any): string | null {
    return item.id;
  }
}
