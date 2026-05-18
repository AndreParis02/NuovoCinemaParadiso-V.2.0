import { environment } from '../../environments/environment';
import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Biglietto } from '../models/biglietto.model';
import { Utente, UtenteModificaRuolo } from '../models/utente.model';
import { LogAzioni } from '../models/logAzioni.model';

@Injectable({
    providedIn: 'root'
})
export class OperatoreService {

    private readonly http = inject(HttpClient);
    private readonly baseUrl = `${environment.apiBaseUrl}/operatore`;


    // GET tutti utenti
    OttieniUtenti(): Observable<Utente[]> {
        return this.http.get<Utente[]>(`${this.baseUrl}/utenti`);
    }

    // GET utente per id
    OttieniUtentePerId(id: string): Observable<Utente> {
        return this.http.get<Utente>(`${this.baseUrl}/utenti/${id}`);
    }

    // DELETE utente
    eliminaUtente(id: string): Observable<void> {
        return this.http.delete<void>(`${this.baseUrl}/utenti/${id}`);
    }

    // GET utenti per abbonamento
    ottieniUtentiTramiteAbbonamento(abbonamentoId: string): Observable<Utente[]> {
        return this.http.get<Utente[]>(
            `${this.baseUrl}/utenti/abbonamento/${abbonamentoId}`
        );
    }

    // GET utenti per giftcard
    ottieniUtentiTramiteGiftCard(giftCardId: string): Observable<Utente[]> {
        return this.http.get<Utente[]>(
            `${this.baseUrl}/utenti/giftcard/${giftCardId}`
        );
    }

    // GET tutti biglietti
    ottieniBiglietti(): Observable<Biglietto[]> {
        return this.http.get<Biglietto[]>(`${this.baseUrl}/biglietti`);
    }

    // GET biglietto per id
    ottieniBigliettoPerId(id: string): Observable<Biglietto> {
        return this.http.get<Biglietto>(`${this.baseUrl}/biglietti/${id}`);
    }
}