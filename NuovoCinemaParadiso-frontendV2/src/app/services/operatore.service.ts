import { environment } from '../../environments/environment';
import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Biglietto } from '../models/biglietto.model';
import { Utente } from '../models/utente.model';

@Injectable({
    providedIn: 'root'
})

export class OperatoreService {

    private readonly http = inject(HttpClient);
    private readonly baseUrl = `${environment.apiBaseUrl}/operatore`;

    OttieniUtenti(): Observable<Utente[]> {
        return this.http.get<Utente[]>(`${this.baseUrl}/listaUtenti`);
    }

    OttieniUtentePerId(id: string): Observable<Utente> {
        return this.http.get<Utente>(`${this.baseUrl}/utenti/${id}`);
    }

    eliminaUtente(id: string): Observable<void> {
        return this.http.delete<void>(`${this.baseUrl}/utenti/${id}`);
    }

    ottieniUtentiTramiteAbbonamento(abbonamentoId: string): Observable<Utente[]> {
        return this.http.get<Utente[]>(
            `${this.baseUrl}/utenti/abbonamento/${abbonamentoId}`
        );
    }

    ottieniUtentiTramiteGiftCard(giftCardId: string): Observable<Utente[]> {
        return this.http.get<Utente[]>(
            `${this.baseUrl}/utenti/giftcard/${giftCardId}`
        );
    }

    ottieniBiglietti(): Observable<Biglietto[]> {
        return this.http.get<Biglietto[]>(`${this.baseUrl}/biglietti`);
    }

    ottieniBigliettoPerId(id: string): Observable<Biglietto> {
        return this.http.get<Biglietto>(`${this.baseUrl}/biglietti/${id}`);
    }
}