import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { GiftCard, GiftCardCreazione } from '../models/giftCard.model';

@Injectable({
    providedIn: 'root',
})

export class GiftCardService {
    private readonly http = inject(HttpClient);
    private readonly baseUrl = `${environment.apiBaseUrl}/giftcard`;
    private readonly utenteUrl = `${environment.apiBaseUrl}/Utente/giftCard`;

    ottieniTutto(): Observable<GiftCard[]> {
        return this.http.get<GiftCard[]>(this.baseUrl);
    }

    ottieniTramiteId(id: String): Observable<GiftCard> {
        return this.http.get<GiftCard>(`${this.baseUrl}/${id}`); //prima c'era scritto id number, ERRORE
    }

    crea(payload: GiftCardCreazione): Observable<GiftCard> {
        return this.http.post<GiftCard>(this.baseUrl, payload);
    }

    modifica(id: string, payload: GiftCardCreazione): Observable<GiftCard> {
        return this.http.put<GiftCard>(`${this.baseUrl}/${id}`, payload);
    }

    elimina(id: string): Observable<void> {
        return this.http.delete<void>(`${this.baseUrl}/${id}`);
    }

    ricarica(importo: number): Observable<{messaggio: string}> {
        return this.http.put<{messaggio: string}>(`${this.utenteUrl}/ricarica`, { importo: importo });
    }

    riscatta(codiceRiscatto: string): Observable<any> {
        return this.http.post<any>(`${this.utenteUrl}/riscatta`, { codiceRiscatto: codiceRiscatto });
    }
}