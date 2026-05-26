import { environment } from '../../environments/environment';
import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Utente, UtenteCreazione } from '../models/utente.model';
import { GiftCard } from '../models/giftCard.model';

@Injectable({
    providedIn: 'root'
})
export class UtenteService {

    private readonly http = inject(HttpClient);
    private readonly baseUrl = `${environment.apiBaseUrl}/utente`;


    profilo(id: string): Observable<Utente> {
        return this.http.get<Utente>(`${this.baseUrl}/${id}`);
    }

    modifica(payload: UtenteCreazione): Observable<Utente> {
        return this.http.put<Utente>(this.baseUrl, payload);
    }

    elimina(id: string): Observable<void> {
        return this.http.delete<void>(`${this.baseUrl}/${id}`);
    }

    abbonati(abbonamentoId: string, utenteId: string): Observable<Utente> {

        return this.http.post<Utente>(`${this.baseUrl}/abbonati`,{abbonamentoId,utenteId});
    }

    giftcard(giftcardId: string, utenteId: string): Observable<Utente> {

        return this.http.post<Utente>(`${this.baseUrl}/giftcard`,{giftcardId,utenteId});
    }

    ottieniGiftCardUtente(): Observable<GiftCard[]> {
        return this.http.get<GiftCard[]>(`${this.baseUrl}/giftcard/mie`);
    }

    riscattaGiftCard(codiceRiscatto: string): Observable<any> {
    return this.http.post<Utente>(`${this.baseUrl}/giftcard/riscatta`, { codiceRiscatto });
    }


}