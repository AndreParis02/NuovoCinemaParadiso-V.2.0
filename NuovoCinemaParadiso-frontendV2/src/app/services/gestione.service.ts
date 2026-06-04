import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { LogAzioni, ContoCinema, Biglietto, GiftCard } from '../models/gestione.model'; 

@Injectable({ providedIn: 'root' })
export class GestioneService { 

  private readonly http = inject(HttpClient);
  private readonly gestoreUrl = `${environment.apiBaseUrl}/Gestore`;
  private readonly giftCardUrl = `${environment.apiBaseUrl}/GiftCard`;


  ottieniLog(): Observable<LogAzioni[]> {
    return this.http.get<LogAzioni[]>(`${this.gestoreUrl}/logs`);
  }

  ottieniConto(): Observable<ContoCinema> {
    return this.http.get<ContoCinema>(`${this.gestoreUrl}/conto`);
  }

  ottieniBiglietti(): Observable<Biglietto[]> {
    return this.http.get<Biglietto[]>(`${this.gestoreUrl}/biglietti`);
  }

  ottieniGiftCards(): Observable<GiftCard[]> {
    return this.http.get<GiftCard[]>(this.giftCardUrl);
  }

  ottieniMieGiftCards(): Observable<GiftCard[]> {
    return this.http.get<GiftCard[]>(`${this.giftCardUrl}/mie`);
  }
}
