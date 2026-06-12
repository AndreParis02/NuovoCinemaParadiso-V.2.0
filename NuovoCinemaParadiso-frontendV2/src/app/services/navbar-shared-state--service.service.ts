import { Injectable, signal, inject, effect } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { AuthService } from './auth.service';
import { Utente } from '../models/utente.model';
import { firstValueFrom } from 'rxjs'; 
@Injectable({
  providedIn: 'root'
})
export class NavbarSharedStateService {
  private http = inject(HttpClient);
  private authService = inject(AuthService);
  private readonly baseUrl = `${environment.apiBaseUrl}/Auth`;

  readonly utenteLoggato = signal<Utente | null>(null);

  constructor() {
 
    effect(async () => {
      const sessione = this.authService.utenteCorrente();

      if (sessione) {
        try {
          await this.caricaFlussoProfiloESaldo();
        } catch (err) {
          console.error("Errore nel canale asincrono della navbar:", err);
        }
      } else {
        this.utenteLoggato.set(null);
      }
    });
  }

  private async caricaFlussoProfiloESaldo(): Promise<void> {
    const profilo = await firstValueFrom(this.http.get<Utente>(`${this.baseUrl}/profilo`));
    
    const resSaldo = await firstValueFrom(this.http.get<{ saldo: number }>(`${this.baseUrl}/saldo`));
    
    profilo.saldo = resSaldo.saldo;
    this.utenteLoggato.set(profilo);
  }

  async forzaAggiornamentoProfilo(): Promise<void> {
    if (!this.authService.isAutenticato()) return;

    const profiloAggiornato = await firstValueFrom(this.http.get<Utente>(`${this.baseUrl}/profilo`));

    this.utenteLoggato.update(attuale =>
      attuale ? { ...attuale, ...profiloAggiornato } : null
    );
  }

  async forzaAggiornamentoSaldo(): Promise<void> {
    if (!this.utenteLoggato()) return;

    const res = await firstValueFrom(this.http.get<{ saldo: number }>(`${this.baseUrl}/saldo`));
    
    this.utenteLoggato.update(attuale => 
      attuale ? { ...attuale, saldo: res.saldo } : null
    );
  }
}