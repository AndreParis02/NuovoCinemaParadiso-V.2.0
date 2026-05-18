
import { environment } from '../../environments/environment';
import { inject, Injectable,signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { tap } from 'rxjs/operators';
import { Observable } from 'rxjs';
import { Auth } from '../models/auth.model';
import { Login } from '../models/login.model';
import { Registrazione } from '../models/registrazione.model';
import { SessioneUtente } from '../models/sessione-utente.model';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private readonly http = inject(HttpClient);
  private readonly router = inject(Router);
  private readonly storagekey = 'nuovo_cinema_paradiso_auth';
  private readonly baseUrl = `${environment.apiBaseUrl}/Auth`;

  readonly utenteCorrente = signal<SessioneUtente | null>(this.readStoredUser());

  login(payload: Login): Observable<SessioneUtente> {

    return this.http
      .post<SessioneUtente>(`${this.baseUrl}/login`, payload)
      .pipe(tap((response) => this.setSession(response)));
  }

  registrazione(payload: Registrazione): Observable<{ message: string }> {
    return this.http.post<{ message: string }>(`${this.baseUrl}/registrazione`, payload);

  }

  logout(): void {
    localStorage.removeItem(this.storagekey);
    this.utenteCorrente.set(null);
    void this.router.navigate(['/login']);
  }

  isAutenticato(): boolean {
    return this.utenteCorrente() !== null;
  }

  possiedeQualsiasiRuolo(ruoli: string[]): boolean {
    const ruolo = this.utenteCorrente()?.ruolo ?? "";
    return ruoli.includes(ruolo);
  }

  ruoloCorrispondente(ruolo: string): boolean {
    return this.utenteCorrente()?.ruolo === ruolo;

  }

  ottieniToken(): string | null {
    return this.utenteCorrente()?.token ?? null;
  }

  private setSession(risposta: SessioneUtente): void {
    const utenteInSessione: SessioneUtente = {
      id: risposta.id,
      nomeCompleto: risposta.nomeCompleto,
      token: risposta.token,
      eta: risposta.eta,
      email: risposta.email,
      ruolo: risposta.ruolo,
      dataInizioAbbonamento: risposta.dataInizioAbbonamento,
      dataInizioGiftCard: risposta.dataInizioGiftCard,
      seAbbonato: risposta.seAbbonato,
      possiedeGiftCard: risposta.possiedeGiftCard
    }

    localStorage.setItem(this.storagekey, JSON.stringify(utenteInSessione));
    this.utenteCorrente.set(utenteInSessione);
  }

  private readStoredUser(): SessioneUtente | null {
    const raw = localStorage.getItem(this.storagekey);

    if (!raw) {
      return null;
    }
    
    try{
      return JSON.parse(raw) as SessioneUtente;
      
    } catch{
      localStorage.removeItem(this.storagekey);
      return null
    }
  }
}