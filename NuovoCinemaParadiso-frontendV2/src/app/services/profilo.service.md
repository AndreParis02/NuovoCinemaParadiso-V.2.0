# Documentazione di `profilo.service.ts`

- Autore: Alessadnro Gabriele Gregorio
- File: `profilo.service.ts`
- Cartella: `NuovoCinemaParadiso-frontendV2/src/app/services`

## Scopo
Servizio Angular che mette a disposizione un meccanismo semplice per notificare i componenti che il profilo utente deve essere aggiornato. Nella versione attuale la notifica avviene tramite callback registrabile.

## Modifiche rilevate
Il servizio è stato aggiornato da un approccio basato su observable/RxJS a un approccio basato su callback. Ora espone metodi per registrare e distruggere un callback di refresh del profilo.

## Codice con commenti esplicativi
```ts
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ProfiloService {
  // Callback che viene invocata quando un componente richiede l'aggiornamento del profilo.
  private refreshCallback: (() => void) | null = null;

  ProfiloCallbackInit(callback: () => void): void {
    // Registra il callback fornito dal componente. Questo callback verrà chiamato
    // quando si richiede esplicitamente l'aggiornamento del profilo.
    this.refreshCallback = callback;
  }

  ProfiloCallbackDestroy(): void {
    // Rimuove il callback quando il componente non è più attivo,
    // evitando riferimenti persistenti che potrebbero causare memory leak.
    this.refreshCallback = null;
  }

  richiediAggiornamentoProfilo(): void {
    // Invia il segnale di aggiornamento del profilo chiamando il callback registrato.
    this.refreshCallback?.();
  }
}
```

## Approfondimenti
- `ProfiloCallbackInit` è responsabile della registrazione del callback quando il componente viene inizializzato.
- `ProfiloCallbackDestroy` è fondamentale per interrompere il riferimento al callback quando il componente viene distrutto.
- `richiediAggiornamentoProfilo` chiama il callback solo se è presente, in modo da non generare errori quando nessun componente è registrato.
- Questo approccio mantiene la logica leggera e diretta e serve per sincronizzare il comportamento del profilo tra componenti senza usare observable complessi.
