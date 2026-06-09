import { Component, inject, OnInit, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { HttpErrorResponse } from '@angular/common/http';
import { GiftCardService } from '../../../services/giftcard.service';
import { AuthService } from '../../../services/auth.service';

@Component({
  selector: 'app-giftcard-form',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './giftcard-form.component.html'
})
export class GiftCardFormComponent implements OnInit {
  private readonly formBuilder = inject(FormBuilder);
  private readonly giftCardService = inject(GiftCardService);
  private readonly authService = inject(AuthService);
  private readonly route = inject(ActivatedRoute); 
  private readonly router = inject(Router); 

  readonly staCaricando = signal(true);
  readonly staInviando = signal(false);
  readonly messaggioErrore = signal('');
  readonly idModifica = signal<string | null>(null);

  readonly form = this.formBuilder.nonNullable.group({
    nome: ['', [Validators.required, Validators.maxLength(50)]],
    valore: [10, [Validators.required, Validators.min(1), Validators.max(1000)]],
    codiceRiscatto: ['', [Validators.required, Validators.maxLength(50)]]
  });

  ngOnInit(): void {
    if (!this.authService.possiedeQualsiasiRuolo(['Gestore', 'Operatore'])) {
      this.router.navigate(['/']); 
      return;
    }

    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.idModifica.set(id);
      this.caricaDati(id);
    } else {
      this.staCaricando.set(false);
    }
  }

  caricaDati(id: string): void {
    this.giftCardService.ottieniTramiteId(id).subscribe({
      next: (gc) => {
        this.form.patchValue({
          nome: gc.nome,
          valore: gc.valore,
          codiceRiscatto: gc.codiceRiscatto
        });
        this.staCaricando.set(false);
      },
      error: () => {
        this.messaggioErrore.set('Gift Card non trovata o già eliminata.');
        this.staCaricando.set(false);
      }
    });
  }

  invia(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const id = this.idModifica();
    if (!id) return;

    this.staInviando.set(true);
    this.messaggioErrore.set('');

    this.giftCardService.modifica(id, this.form.getRawValue()).subscribe({
      next: () => {
        this.router.navigate(['/giftcard-list']); 
      },
      error: (error: unknown) => {
        this.staInviando.set(false);
        this.messaggioErrore.set(this.estraiMessaggioErrore(error, 'Errore durante il salvataggio.'));
      }
    });
  }

  annulla(): void {
    this.router.navigate(['/giftcard-list']); 
  }

  private estraiMessaggioErrore(error: unknown, fallback: string): string {
    if (error instanceof HttpErrorResponse) {
      return error.error?.message || error.error?.errore || fallback;
    }
    return fallback;
  }
}