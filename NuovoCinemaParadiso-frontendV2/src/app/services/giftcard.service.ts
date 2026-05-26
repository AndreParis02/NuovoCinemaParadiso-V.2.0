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

    ottieniTutto(): Observable<GiftCard[]> {
        return this.http.get<GiftCard[]>(this.baseUrl);
    }

    ottieniTramiteId(id: number): Observable<GiftCard> {
        return this.http.get<GiftCard>(`${this.baseUrl}/${id}`);
    }

    crea(payload: GiftCardCreazione): Observable<GiftCard> {
        return this.http.post<GiftCard>(this.baseUrl, payload);
    }

    modifica(id: number, payload: GiftCardCreazione): Observable<GiftCard> {
        return this.http.put<GiftCard>(`${this.baseUrl}/${id}`, payload);
    }

    elimina(id: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
}
}