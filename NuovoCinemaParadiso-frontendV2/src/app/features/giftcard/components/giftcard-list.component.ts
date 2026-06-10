import { Component, inject, signal, OnInit } from '@angular/core';
import { RouterModule } from '@angular/router';
import { GestioneService } from '../../../services/gestione.service';
import { GiftCardService } from '../../../services/giftcard.service';
import { GiftCard } from '../../../models/gestione.model';
import { AuthService } from '../../../services/auth.service';
import { CreaCodiceComponent } from "./crea-codice.component";

@Component({
  selector: 'giftcard-list',
  standalone: true,
  imports: [RouterModule, CreaCodiceComponent], // AGGIUNTO RouterModule
  templateUrl: './giftcard-list.component.html',
})
export class GiftCardListComponent implements OnInit {
  private readonly gestioneService = inject(GestioneService);
  private readonly giftCardService = inject(GiftCardService);
  public readonly authService = inject(AuthService);

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
        error: (error) => this.gestisciErrore(error),
      });
    } else {
      const utenteId = this.authService.utenteCorrente()?.id;
      if (!utenteId) {
        this.errorMessage.set("Errore: Impossibile identificare l'utente.");
        this.isLoading.set(false);
        return;
      }
      this.gestioneService.ottieniMieGiftCards().subscribe({
        next: (data) => this.gestisciSuccesso(data),
        error: (error) => this.gestisciErrore(error),
      });
    }
  }

  eliminaGiftCard(id: string | null | undefined): void {
    if (!id) return;
    if (!this.authService.possiedeQualsiasiRuolo(['Gestore', 'Operatore'])) {
      alert('Non hai i permessi per eliminare.');
      return;
    }

    if (confirm('Sei sicuro di voler eliminare questa Gift Card?')) {
      this.isLoading.set(true);
      this.giftCardService.elimina(id).subscribe({
        next: () => this.caricaGiftCard(),
        error: (e) => {
          console.error("Errore eliminazione:", e);
          this.errorMessage.set('Impossibile eliminare la Gift Card.');
          this.isLoading.set(false);
        },
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
    this.errorMessage.set('Errore nel caricamento delle giftcard.');
  }

  trackById(_: number, item: any): string | null {
    return item.id;
  }
}