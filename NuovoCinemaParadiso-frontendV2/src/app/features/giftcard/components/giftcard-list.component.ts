import { Component, inject, signal, OnInit } from '@angular/core';
import { DatePipe, CurrencyPipe } from '@angular/common';
import { GestioneService } from '../../../services/gestione.service';
import { GiftCard } from '../../../models/gestione.model';
import { AuthService } from '../../../services/auth.service';

@Component({
  selector: 'giftcard-list',
  standalone: true,
  imports: [DatePipe, CurrencyPipe], 
  templateUrl: './giftcard-list.component.html',
})
export class GiftCardListComponent implements OnInit {

  private readonly gestioneService = inject(GestioneService);
  private readonly authService = inject(AuthService);

  readonly giftCards = signal<GiftCard[]>([]);

  readonly isLoading = signal(false);
  readonly errorMessage = signal('');

  ngOnInit(): void {
    this.caricaGiftCard();
  }

  caricaGiftCard(): void {
    this.isLoading.set(true);
    this.errorMessage.set('');

    if (this.authService.isGestore()) {
      
      this.gestioneService.ottieniGiftCards().subscribe({
        next: (data) => this.gestisciSuccesso(data),
        error: (error) => this.gestisciErrore(error)
      });

    } else {

      const utenteId = this.authService.utenteCorrente()?.id;

      if (!utenteId) {
        this.errorMessage.set('Errore: Impossibile identificare l\'utente.');
        this.isLoading.set(false);
        return;
      }

      this.gestioneService.ottieniMieGiftCards().subscribe({
        next: (data) => this.gestisciSuccesso(data),
        error: (error) => this.gestisciErrore(error)
      });

    }
  }


  private gestisciSuccesso(data: GiftCard[]): void {
    this.giftCards.set(data);
    this.isLoading.set(false); 
  }

  private gestisciErrore(error: unknown): void {
    console.error('ERRORE GiftCard:', error);
    this.isLoading.set(false);
    this.errorMessage.set('Si è verificato un errore nel caricamento dei dati delle giftcard.');
  }

  trackById(_: number, item: any): string | null {
    return item.id;
  }
}