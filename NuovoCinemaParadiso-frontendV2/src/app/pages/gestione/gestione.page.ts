import { Component, inject, signal, OnInit } from '@angular/core';
import { DatePipe, CurrencyPipe } from '@angular/common';
import { GestioneService } from '../../services/gestione.service';
import { LogAzioni, ContoCinema, Biglietto, GiftCard } from '../../models/gestione.model';

@Component({
  selector: 'app-gestione',
  standalone: true,
  imports: [DatePipe, CurrencyPipe], 
  templateUrl: './gestione.page.html',
  styleUrl: './gestione.page.css',
})
export class GestionePage implements OnInit {

  private readonly gestioneService = inject(GestioneService);

  readonly logs = signal<LogAzioni[]>([]);
  readonly biglietti = signal<Biglietto[]>([]);
  readonly giftCards = signal<GiftCard[]>([]);
  readonly conto = signal<ContoCinema | null>(null);

  readonly isLoading = signal(false);
  readonly errorMessage = signal('');

  ngOnInit(): void {
    this.caricaTutto();
  }

  caricaTutto(): void {
    this.isLoading.set(true);
    this.errorMessage.set('');

    this.gestioneService.ottieniConto().subscribe({
      next: (data) => this.conto.set(data),
      error: (e) => console.error('Errore Conto', e)
    });

    
    this.gestioneService.ottieniGiftCards().subscribe({
      next: (data) => this.giftCards.set(data),
      error: (e) => console.error('Errore GiftCard', e)
    });

    
    this.gestioneService.ottieniBiglietti().subscribe({
      next: (data) => this.biglietti.set(data),
      error: (e) => console.error('Errore Biglietti', e)
    });

   
    this.gestioneService.ottieniLog().subscribe({
      next: (data) => {
        this.logs.set(data);
        this.isLoading.set(false); 
      },
      error: (error: unknown) => {
        console.error('ERRORE LOG AZIONI:', error);
        
        this.isLoading.set(false);
        this.errorMessage.set('Si è verificato un errore nel caricamento dei dati.');
      }
    });
  }

  trackById(_: number, item: any): string | null {
    return item.id;
  }
}