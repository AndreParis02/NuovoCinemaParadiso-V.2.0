import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { Sala, SalaCreazione  } from '../models/sala.model';


@Injectable({ providedIn: 'root' })
export class SalaService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiBaseUrl}/sala`;

  ottieniTutto(): Observable<Sala[]> {
    return this.http.get<Sala[]>(this.baseUrl);
  }
  ottieniPerTipologia(tipologiaId: string): Observable<Sala[]> {
    return this.http.get<Sala[]>(`${this.baseUrl}/tipologia/${tipologiaId}`);
  }

  ottieniTramiteId(id: string): Observable<Sala> {
    return this.http.get<Sala>(`${this.baseUrl}/${id}`);
  }

  crea(payload: SalaCreazione): Observable<string> {
    return this.http.post(`${this.baseUrl}`, payload, { responseType: 'text' });
  }
  
  modifica(id: string, payload: SalaCreazione): Observable<string> {
    return this.http.put(`${this.baseUrl}/${id}`, payload, { responseType: 'text' });
  }

  elimina(id: string): Observable<void> {
    return this.http.put<void>(`${this.baseUrl}/elimina/${id}`, null);
  }
}