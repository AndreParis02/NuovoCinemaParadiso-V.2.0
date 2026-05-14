import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { Acquisto, AcquistoCreazione } from '../models/acquisto.model';

@Injectable({
  providedIn: 'root',
})
export class AcquistoService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiBaseUrl}/acquisto`;

  ottieniTutto(utenteId: string): Observable<Acquisto[]> {
    return this.http.get<Acquisto[]>(`${this.baseUrl}/utente/${utenteId}`);
  }

  ottieniTramiteId(id: string, utenteId: string): Observable<Acquisto> {
    return this.http.get<Acquisto>(`${this.baseUrl}/${id}/utente/${utenteId}`);
  }

  crea(payload: AcquistoCreazione, utenteId: string): Observable<Acquisto> {
    return this.http.post<Acquisto>(`${this.baseUrl}/utente/${utenteId}`, payload);
  }

  modifica(id: string, payload: AcquistoCreazione): Observable<Acquisto> {
    return this.http.put<Acquisto>(`${this.baseUrl}/${id}`, payload);
  }

  elimina(id: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }
}
