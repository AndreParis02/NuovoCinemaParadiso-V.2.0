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
import { UtenteService } from './utente.service';
import { NavbarSharedStateService } from './navbar-shared-state--service.service';

@Injectable({
    providedIn: 'root',
})
export class AuthService {

    private readonly http = inject(HttpClient);
    private readonly router = inject(Router);
    private readonly utenteService = inject(UtenteService);
    private readonly storageKey = 'nuovo_cinema_paradiso_auth';
    private readonly baseUrl = `${environment.apiBaseUrl}/Auth`;
    private readonly navbarSharedStateService = inject(NavbarSharedStateService);



    // Stato utente sincronizzato con localStorage
    readonly utenteCorrente = signal<SessioneUtente | null>(this.caricaUtenteDaStorage());

    // ---------------------------
    // LOGIN
    // ---------------------------
    login(payload: any) {
    this.http.post<SessioneUtente>('.../login', payload).subscribe(response => {
      this.salvaSessione(response);
      
      // ALIMENTIAMO IL CANALE: impostando il signal, scateniamo tutte le reazioni a catena
      this.utenteCorrente.set(response);
    });
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

    isOperatore(): boolean {
        return this.utenteCorrente()?.ruolo === 'Operatore'
    }

    isGestore(): boolean {
        return this.utenteCorrente()?.ruolo === 'Gestore';
    }
    possiedeQualsiasiRuolo(ruoli: RuoliUtente[]): boolean {
        const ruolo = this.ottieniRuoloUtente();
        return !!ruolo && ruoli.includes(ruolo);
    }

    ruoloCorrispondente(ruolo: string): boolean {
        return this.utenteCorrente()?.ruolo === ruolo;
    }

    ottieniToken(): string | null {
        return this.utenteCorrente()?.token ?? null;
    }

    ottieniRuoloUtente(): RuoliUtente | null {
        const raw = localStorage.getItem(this.storageKey);

        if (!raw) {
            return null;
        }

        try {
            const utente = JSON.parse(raw) as SessioneUtente;

            if (
                utente.ruolo === 'Operatore' ||
                utente.ruolo === 'Gestore' ||
                utente.ruolo === 'Utente'
            ) {
                return utente.ruolo;
            }

            return null;
        } catch {
            return null;
        }
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
            seAbbonato: risposta.seAbbonato,
        }

        localStorage.setItem(this.storageKey, JSON.stringify(utenteInSessione));
        console.log('Utente salvato in localStorage:', utenteInSessione.ruolo);
        this.utenteCorrente.set(utenteInSessione)
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