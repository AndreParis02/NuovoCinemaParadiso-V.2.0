# Documentazione di `biglietto-list.component.ts`

- Autore: Alessadnro Gabriele Gregorio
- File: `biglietto-list.component.ts`
- Cartella: `NuovoCinemaParadiso-frontendV2/src/app/features/biglietto`

## Scopo
Componente standalone Angular responsabile della visualizzazione, gestione e cancellazione dei biglietti per l'utente attualmente autenticato.

## Descrizione dettagliata
Questo componente carica la lista dei biglietti dall'API tramite `BigliettoService`, mostra un messaggio di caricamento mentre attende la risposta, e gestisce gli stati di errore o successo. Quando un biglietto viene eliminato, aggiorna anche il saldo globale e richiede il refresh del profilo utente.

## Codice con commenti esplicativi
```ts
import { Component, inject, signal, output } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
import { ProfiloService } from '../../services/profilo.service'
import { Biglietto } from '../../models/biglietto.model';
import { BigliettoService } from '../../services/biglietto.service';
import { AuthService } from '../../services/auth.service';
import { NavbarSharedStateService } from '../../services/navbar-shared-state--service.service';

@Component({
  selector: 'biglietto-list',
  standalone: true,
  templateUrl: './biglietto-list.component.html'
})
export class BigliettoListComponent {

  // Inietta i servizi necessari per l'autenticazione, gestione biglietto,
  // stato condiviso della navbar e aggiornamento del profilo.
  private readonly authService = inject(AuthService);
  private readonly bigliettoService = inject(BigliettoService);
  private readonly navbarSharedStateService = inject(NavbarSharedStateService);
  private readonly profiloService = inject(ProfiloService);

  // Event output placeholder, non utilizzato direttamente nel codice attuale.
  readonly modificaCompletata = output<void>();

  // Segnali reattivi per gestire la UI.
  readonly biglietti = signal<Biglietto[]>([]);   // Lista dei biglietti caricati
  readonly staCaricando = signal(false);          // Loader per le richieste
  readonly staInviando = signal(false);           // Indica eventuali invii/eliminazioni
  readonly messaggioErrore = signal('');         // Messaggi di errore da mostrare
  readonly messaggioSuccesso = signal('');       // Messaggi di successo da mostrare

  readonly utenteId;                             // ID dell'utente autenticato

  constructor() {
    const utente = this.authService.utenteCorrente();

    if (!utente?.id) {
      // Se l'utente non è loggato, mostro un errore e interrompo l'inizializzazione.
      this.messaggioErrore.set("Utente non loggato");
      return;
    }

    this.utenteId = utente.id;
    // Avvia il caricamento dei biglietti appena il componente viene creato.
    this.caricaBiglietti();
  }

  caricaBiglietti(): void {
    if (!this.utenteId) return;

    this.staCaricando.set(true);
    this.messaggioErrore.set('');

    this.bigliettoService.ottieniTutto().subscribe({
      next: (items) => {
        // Popola la lista locale dei biglietti e disattiva il loader.
        this.biglietti.set(items);
        this.staCaricando.set(false);
      },
      error: (error: unknown) => {
        // In caso di errore, mostra il messaggio appropriato.
        this.staCaricando.set(false);
        this.messaggioErrore.set(
          this.estraiMessaggioErrore(error,
            'Errore durante il caricamento dei biglietti')
        );
      }
    });
  }

  tracciaPerId(_: string, item: Biglietto): string {
    // TrackBy usato da Angular per ottimizzare il rendering delle liste.
    return item.id;
  }

  elimina(id: string): void {
    // Conferma dell'utente prima di continuare.
    if (!confirm("Sei sicuro di voler eliminare questo biglietto?")) return;

    this.staInviando.set(true);
    this.messaggioErrore.set('');
    this.messaggioSuccesso.set('');

    this.bigliettoService.elimina(id).subscribe({
      next: () => {
        // Rimuove immediatamente il biglietto dalla lista locale dopo l'eliminazione.
        this.biglietti.update(lista => lista.filter(b => b.id !== id));
        this.staInviando.set(false);

        // Aggiorna lo stato globale della navbar e chiede il refresh del profilo.
        this.navbarSharedStateService.forzaAggiornamentoSaldo();
        this.profiloService.richiediAggiornamentoProfilo();

        this.messaggioSuccesso.set("Biglietto eliminato con successo");
      },
      error: (error: unknown) => {
        console.log("Errore di eliminazione");
        this.staInviando.set(false);
        this.messaggioErrore.set(
          this.estraiMessaggioErrore(error, "Errore durante l'eliminazione del biglietto")
        );
      }
    });
  }

  modifica(id: string) {
    // Spazio riservato per la futura implementazione della modifica del biglietto.
    // Al momento non contiene logica.
  }

  private estraiMessaggioErrore(error: unknown, fallback: string): string {
    if (error instanceof HttpErrorResponse) {
      // Estrae il messaggio specifico fornito dal backend, se disponibile.
      return error.error?.message ?? fallback;
    }
    return fallback;
  }
}
```

## Note aggiuntive
- Il componente non gestisce ancora la modifica del biglietto: il metodo `modifica(id: string)` è lasciato vuoto.
- La cancellazione del biglietto esegue anche un aggiornamento del saldo e del profilo, per mantenere l'interfaccia coerente.
- L'uso di segnali (`signal`) permette di aggiornare la UI in modo reattivo senza un Angular form complesso.
