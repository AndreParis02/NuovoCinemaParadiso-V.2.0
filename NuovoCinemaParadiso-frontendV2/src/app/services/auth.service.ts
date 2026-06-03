import { environment } from '../../environments/environment';
import { inject, Injectable, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { tap } from 'rxjs/operators';
import { Observable } from 'rxjs';
import { Login } from '../models/login.model';
import { Registrazione } from '../models/registrazione.model';
import { SessioneUtente } from '../models/sessione-utente.model';
import { RuoliUtente } from '../ruoliUtente';

@Injectable({
  providedIn: 'root',
})
export class AuthService {

  private readonly http = inject(HttpClient);
  private readonly router = inject(Router);
  private readonly storageKey = 'nuovo_cinema_paradiso_auth';
  private readonly baseUrl = `${environment.apiBaseUrl}/Auth`;

  // Stato utente sincronizzato con localStorage
  readonly utenteCorrente = signal<SessioneUtente | null>(this.caricaUtenteDaStorage());

  // ---------------------------
  // LOGIN
  // ---------------------------
  login(payload: Login): Observable<SessioneUtente> {
    return this.http
      .post<SessioneUtente>(`${this.baseUrl}/login`, payload)
      .pipe(
        tap(response => this.salvaSessione(response))
      );
  }

  // ---------------------------
  // REGISTRAZIONE
  // ---------------------------
  registrazione(payload: Registrazione): Observable<{ message: string }> {
    return this.http.post<{ message: string }>(
      `${this.baseUrl}/registrazione`,
      payload
    );
  }

  // ---------------------------
  // LOGOUT
  // ---------------------------
  logout(): void {
    localStorage.removeItem(this.storageKey);
    this.utenteCorrente.set(null);
    void this.router.navigate(['/login']);
  }

  // ---------------------------
  // STATO UTENTE
  // ---------------------------
  isAutenticato(): boolean {
    return this.utenteCorrente() !== null;
  }

  isGestore(): boolean {
    return this.utenteCorrente()?.ruolo === 'Gestore';
  }

  ruoloCorrispondente(ruolo: string): boolean {
    return this.utenteCorrente()?.ruolo === ruolo;
  }

  possiedeQualsiasiRuolo(ruoli: RuoliUtente[]): boolean {
    const ruolo = this.ottieniRuoloUtente();
    return !!ruolo && ruoli.includes(ruolo);
  }

  ottieniToken(): string | null {
    return this.utenteCorrente()?.token ?? null;
  }

  ottieniRuoloUtente(): RuoliUtente | null {
    const utente = this.utenteCorrente();
    if (!utente) return null;

    if (utente.ruolo === 'Operatore' || utente.ruolo === 'Gestore' || utente.ruolo === 'Utente') {
      return utente.ruolo;
    }

    return null;
  }

  // ---------------------------
  // PRIVATE
  // ---------------------------
  private salvaSessione(risposta: SessioneUtente): void {
    localStorage.setItem(this.storageKey, JSON.stringify(risposta));
    this.utenteCorrente.set(risposta);
  }

  private caricaUtenteDaStorage(): SessioneUtente | null {
    const raw = localStorage.getItem(this.storageKey);
    if (!raw) return null;

    try {
      return JSON.parse(raw) as SessioneUtente;
    } catch {
      localStorage.removeItem(this.storageKey);
      return null;
    }
  }
}