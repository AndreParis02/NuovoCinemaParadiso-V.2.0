import { Injectable, signal, computed, inject, effect } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { AuthService } from './auth.service';
import { Utente } from '../models/utente.model';
import { firstValueFrom } from 'rxjs'; // Usato SOLO per trasformare la chiamata in Promise (async/await)

@Injectable({
  providedIn: 'root'
})
export class NavbarSharedStateService {
  private http = inject(HttpClient);
  private authService = inject(AuthService);
  private readonly baseUrl = `${environment.apiBaseUrl}/Auth`;

  // Sorgente di verità finale per la Navbar
  readonly utenteLoggato = signal<Utente | null>(null);

  constructor() {
    // CANALE DI COMUNICAZIONE PERENNE (Senza RxJS)
    // Questo effect monitora il Signal "sessioneAttiva" di AuthService.
    // Gira a ogni login, logout o refresh.
    effect(async () => {
      const sessione = this.authService.utenteCorrente();

      if (sessione) {
        try {
          // Avviamo il flusso asincrono lineare (Canale Attivo)
          await this.caricaFlussoProfiloESaldo();
        } catch (err) {
          console.error("Errore nel canale asincrono della navbar:", err);
        }
      } else {
        // Se la sessione diventa null (Logout), pialliamo i dati immediatamente
        this.utenteLoggato.set(null);
      }
    });
  }

  // Sfruttiamo async/await per eliminare la complessità delle pipe RxJS
  private async caricaFlussoProfiloESaldo(): Promise<void> {
    // 1. Scarichiamo il profilo trasformando l'Observable in Promise
    const profilo = await firstValueFrom(this.http.get<Utente>(`${this.baseUrl}/profilo`));
    
    // 2. Scarichiamo il saldo relativo a questo profilo
    const resSaldo = await firstValueFrom(this.http.get<{ saldo: number }>(`${this.baseUrl}/saldo`));
    
    // 3. Uniamo i dati e aggiorniamo il Signal globale
    profilo.saldo = resSaldo.saldo;
    this.utenteLoggato.set(profilo);
  }

  // Azione continua: se l'utente compra un biglietto nel sito, 
  // chiami questo metodo e la navbar si aggiorna da sola in background
  async forzaAggiornamentoSaldo(): Promise<void> {
    if (!this.utenteLoggato()) return;

    const res = await firstValueFrom(this.http.get<{ saldo: number }>(`${this.baseUrl}/saldo`));
    
    this.utenteLoggato.update(attuale => 
      attuale ? { ...attuale, saldo: res.saldo } : null
    );
  }
}