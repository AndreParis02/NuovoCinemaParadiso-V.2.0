import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { Biglietto, BigliettoCreazione } from '../models/biglietto.model';

@Injectable({
  providedIn: 'root',
})
export class BigliettoService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiBaseUrl}/biglietto`;

  ottieniTutto(utenteId: string): Observable<Biglietto[]> {
    return this.http.get<Biglietto[]>(`${this.baseUrl}/utente/${utenteId}`);
  }

  ottieniTramiteId(id: string, utenteId: string): Observable<Biglietto> {
    return this.http.get<Biglietto>(`${this.baseUrl}/${id}/utente/${utenteId}`);
  }

  crea(payload: BigliettoCreazione): Observable<any> {
    return this.http.post<any>(this.baseUrl, payload);  
  }

  modifica(id: string, payload: BigliettoCreazione): Observable<Biglietto> {
    return this.http.put<Biglietto>(`${this.baseUrl}/${id}`, payload);
  }

  elimina(id: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }
}
