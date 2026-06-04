import { Component, inject, signal } from '@angular/core';
import { AuthService } from '../../../services/auth.service';
import { TipologiaSala } from '../../../models/tipologia-sala.model'; 
import { TipologiaSalaService } from '../../../services/tipologia-sala.service';

@Component({
  selector: 'tipologia-sala-list',
  templateUrl: './tipologia-sala-list.component.html',
})

export class TipologiaSalaPage {

  private readonly authService = inject(AuthService);
  private readonly tipologiaSalaService = inject(TipologiaSalaService);

  readonly tipologiaSala = signal<TipologiaSala[]>([]);
  readonly isLoading = signal(false);
  readonly errorMessage = signal('');
  readonly successMessage = signal('');


  constructor() {
    this.loadTipologiaSala();
  }

  loadTipologiaSala(): void {
    this.isLoading.set(true);
    this.errorMessage.set('');

    this.tipologiaSalaService.ottieniTutto().subscribe({
      next: (items) => {
        this.tipologiaSala.set(items);
        this.isLoading.set(false);
      },
      error: (error: unknown) => {
        this.isLoading.set(false);
        this.errorMessage.set(
          this.extractErrorMessage(error, 'Impossibile caricare la tipologia sala')
        );
      }
    });
  }

  trackById(_: string, item: TipologiaSala): string | null {
    return item.id
  }

  private extractErrorMessage(error: unknown, fallback: string): string {
    if (error instanceof Error) {
      return error?.message ?? fallback;
    }
    return fallback
  }
}
