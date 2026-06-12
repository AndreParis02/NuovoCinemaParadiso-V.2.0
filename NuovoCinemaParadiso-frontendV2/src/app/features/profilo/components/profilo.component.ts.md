# Documentazione di `profilo.component.ts`

- Autore: Alessadnro Gabriele Gregorio
- File: `profilo.component.ts`
- Cartella: `NuovoCinemaParadiso-frontendV2/src/app/features/profilo/components`

## Scopo
Componente standalone Angular che mostra e modifica i dati del profilo utente. Gestisce il caricamento iniziale, la validazione del form e la cancellazione dell'account, oltre a sincronizzare il refresh del profilo attraverso `ProfiloService`.

## Modifiche chiave rilevate
Rispetto a versioni precedenti, questo componente ora utilizza i metodi `ProfiloCallbackInit` e `ProfiloCallbackDestroy` di `ProfiloService` per registrare e rimuovere un callback di refresh, invece della precedente gestione tramite observable.

## Codice con commenti esplicativi
```ts
import { Component, inject, signal, OnDestroy, OnInit } from '@angular/core';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { HttpErrorResponse } from '@angular/common/http';
import { DatePipe } from '@angular/common';
import { UtenteService } from '../../../services/utente.service';
import { AuthService } from '../../../services/auth.service';
import { Utente } from '../../../models/utente.model';
import { ProfiloService } from '../../../services/profilo.service';

@Component({
  selector: 'profilo',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    DatePipe
  ],
  templateUrl: './profilo.component.html',
})
export class ProfiloComponent implements OnInit, OnDestroy {
  // Iniezione dei servizi necessari: form builder, servizio utente, autenticazione e gestione profilo.
  private readonly formBuilder = inject(FormBuilder);
  private readonly utenteService = inject(UtenteService);
  private readonly authService = inject(AuthService);
  private readonly profiloService = inject(ProfiloService);

  readonly utente = signal<Utente | null>(null);          // Stato del profilo utente.
  readonly staCaricando = signal(false);                 // Indica il caricamento iniziale e gli aggiornamenti.
  readonly staInviando = signal(false);                  // Indica il salvataggio o l'eliminazione del profilo.
  readonly messaggioErrore = signal('');                // Messaggio di errore per l'interfaccia.
  readonly messaggioSuccesso = signal('');              // Messaggio di successo per l'interfaccia.

  // Form reattivo con validazione dei campi.
  readonly form = this.formBuilder.nonNullable.group({
    nomeCompleto: ['', [Validators.required, Validators.maxLength(100)]],
    eta: [0, [Validators.required, Validators.min(14), Validators.max(100)]]
  });

  ngOnInit(): void {
    // Registra un callback nel servizio di profilo per ricevere richieste di refresh da altri componenti.
    this.profiloService.ProfiloCallbackInit(() => this.caricaUtente());

    // Carica i dati dell'utente al primo avvio del componente.
    this.caricaUtente();
  }
  
  ngOnDestroy(): void {
    // Rimuove il callback quando il componente viene distrutto per evitare riferimenti residuali.
    this.profiloService.ProfiloCallbackDestroy();
  }

  modificabileDa(): boolean {
    // Controlla il ruolo dell'utente per abilitare la modifica del profilo.
    return this.authService.possiedeQualsiasiRuolo(['Operatore']);
  }

  caricaUtente(): void {
    this.staCaricando.set(true);
    this.messaggioErrore.set('');

    this.utenteService.profilo().subscribe({
      next: (item) => {
        this.utente.set(item);

        // Popola il form con i valori ricevuti dal backend.
        this.form.patchValue({
          nomeCompleto: item.nomeCompleto,
          eta: item.eta
        });

        this.staCaricando.set(false);
      },
      error: (error) => {
        this.staCaricando.set(false);
        this.messaggioErrore.set(
          this.estraiMessaggioErrore(error, 'Accedi per vedere il profilo')
        );
      }
    });
  }

  invia(): void {
    if (this.form.invalid) {
      // Se il form è invalido, mostra gli errori e interrompe l'invio.
      this.form.markAllAsTouched();
      return;
    }

    this.staInviando.set(true);
    this.messaggioErrore.set('');
    this.messaggioSuccesso.set('');

    this.utenteService.modifica(this.form.getRawValue()).subscribe({
      next: () => {
        this.staInviando.set(false);
        this.messaggioSuccesso.set('Profilo aggiornato');
        this.caricaUtente();
      },
      error: (error) => {
        this.staInviando.set(false);
        this.messaggioErrore.set(
          this.estraiMessaggioErrore(error, 'Modifica non riuscita.')
        );
      }
    });
  }

  elimina(): void {
    const confirmed = confirm('Vuoi davvero eliminare il tuo account?');
    if (!confirmed) return;

    this.messaggioErrore.set('');
    this.messaggioSuccesso.set('');

    this.utenteService.eliminaProfilo().subscribe({
      next: () => {
        this.authService.logout(); // Rimuove il token di autenticazione locale.
        this.messaggioSuccesso.set('Account eliminato');
        window.location.href = '/login'; // Reindirizza l'utente alla pagina di login.
      },
      error: (error) => {
        this.messaggioErrore.set(
          this.estraiMessaggioErrore(error, 'Eliminazione non riuscita.')
        );
      }
    });
  }

  private estraiMessaggioErrore(error: unknown, fallback: string): string {
    if (error instanceof HttpErrorResponse) {
      return error.error?.message ?? fallback;
    }
    return fallback;
  }
}
```

## Note aggiuntive
- La registrazione del callback tramite `ProfiloCallbackInit` è la modifica principale di questa versione.
- `ProfiloCallbackDestroy` evita che il callback rimanga attivo dopo la distruzione del componente.
- La validazione del form è gestita interamente lato client con `Validators`.
