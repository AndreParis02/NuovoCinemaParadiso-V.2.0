import { Component, inject, signal, OnInit } from '@angular/core';
import { DatePipe } from '@angular/common';
import { LogAzioniService } from '../../services/log-azioni.service';
import { LogAzioni } from '../../models/logAzioni.model';

@Component({
  selector: 'app-log-azioni',
  standalone: true,
  imports: [DatePipe], 
  templateUrl: './log-azioni.page.html',
  styleUrl: './log-azioni.page.css',
})
export class LogAzioniPage implements OnInit {

  private readonly logAzioniService = inject(LogAzioniService);

  readonly logs = signal<LogAzioni[]>([]);
  readonly isLoading = signal(false);
  readonly errorMessage = signal('');

  ngOnInit(): void {
    this.loadLogs();
  }

  loadLogs(): void {
    this.isLoading.set(true);
    this.errorMessage.set('');

    this.logAzioniService.ottieniTutti().subscribe({
      next: (items) => {
        this.logs.set(items);
        this.isLoading.set(false);
      },
      error: (error: unknown) => {
        this.isLoading.set(false);
        this.errorMessage.set(
          this.extractErrorMessage(error, 'Impossibile caricare i log')
        );
      }
    });
  }

  trackById(_: number, item: LogAzioni): string | null {
    return item.id;
  }

  private extractErrorMessage(error: unknown, fallback: string): string {
    if (error instanceof Error) {
      return error?.message ?? fallback;
    }
    return fallback;
  }
}