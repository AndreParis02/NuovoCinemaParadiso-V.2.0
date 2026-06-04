import { Component, inject, signal } from '@angular/core';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { HttpErrorResponse } from '@angular/common/http';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'login-page',
  standalone: true,
  imports: [ReactiveFormsModule, RouterLink],
  templateUrl: './login.page.html'
})

export class LoginPage {
  private readonly formBuilder = inject(FormBuilder);
  private readonly authService = inject(AuthService);
  private readonly router = inject(Router);

  readonly staCaricando = signal(false);
  readonly messaggioErrore = signal('');

  readonly form = this.formBuilder.nonNullable.group({
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required]]
  });

  invia(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.staCaricando.set(true);
    this.messaggioErrore.set('');
    
    // Chiamata al metodo login del servizio AuthService.
    // getRawValue() recupera i dati dal form (email e password) ignorando lo stato 'disabled'.
    this.authService.login(this.form.getRawValue()).subscribe({
  
    // Callback eseguita in caso di successo (il backend ha risposto positivamente)
    next: () => {
      // Nasconde l'indicatore di caricamento
      this.staCaricando.set(false);
      
      // Reindirizza l'utente alla pagina 'dashboard'
      // 'void' è usato per ignorare la Promise restituita dal router
      void this.router.navigate(['/dashboard']);
    },

      // Callback eseguita in caso di errore (es. credenziali errate o server offline)
      error: (error: unknown) => {
        // Nasconde l'indicatore di caricamento anche in caso di fallimento
        this.staCaricando.set(false);
      
      // Imposta il messaggio di errore estraendolo dalla risposta del server
      // Se non trova un messaggio specifico, usa il fallback "Login non riuscito."
      this.messaggioErrore.set(this.estraiMessaggioErrore(error, 'Login non riuscito.'));
      }
    });
  }
/**
 * Metodo privato per estrarre in modo sicuro il messaggio di errore 
 * da una risposta HTTP.
 */
  private estraiMessaggioErrore(error: unknown, fallback: string): string {
    // Verifica se l'errore è un oggetto di tipo HttpErrorResponse (standard di Angular)
    if (error instanceof HttpErrorResponse) {
      // Prova a leggere il messaggio inviato dal backend (es. error.error.message)
      // Se non esiste (null/undefined), restituisce il messaggio di fallback
      return error.error?.message ?? fallback;
    }

    // Se l'errore non è di tipo HTTP, restituisce il fallback predefinito
    return fallback;
  }
}