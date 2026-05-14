
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

  login(payload: Login): Observable<Auth> {

    return this.http
      .post<Auth>(`${this.baseUrl}/login`, payload)
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

  seAutenticato(): boolean {
    return this.utenteCorrente() !== null;
  }

  hasAnyRole(ruoli: string[]): boolean {
    const ruolo = this.utenteCorrente()?.ruolo ?? "";
    return ruoli.includes(ruolo);
  }

  hasRole(ruolo: string): boolean {
    return this.utenteCorrente()?.ruolo === ruolo;

  }

  ottieniToken(): string | null {
    console.log('TOKEN:', this.utenteCorrente()?.token);
    return this.utenteCorrente()?.token ?? null;
  }

  private setSession(risposta: Auth): void {
    const utenteInSessione: SessioneUtente = {
      token: risposta.token,
      id: risposta.id,
      email: risposta.email,
      nomeCompleto: risposta.nomeCompleto,
      ruolo: risposta.ruolo
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