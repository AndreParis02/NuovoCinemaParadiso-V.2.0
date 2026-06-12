import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ProfiloService {
  private refreshCallback: (() => void) | null = null;

  ProfiloCallbackInit(callback: () => void): void {
    this.refreshCallback = callback;
  }

  ProfiloCallbackDestroy(): void {
    this.refreshCallback = null;
  }

  richiediAggiornamentoProfilo(): void {
    this.refreshCallback?.();
  }
}
