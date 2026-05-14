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

  crea(payload: SalaCreazione): Observable<Sala> {
    return this.http.post<Sala>(this.baseUrl, payload);
  }

  modifica(id: string, payload: SalaCreazione): Observable<Sala> {
    return this.http.put<Sala>(`${this.baseUrl}/${id}`, payload);
  }

  elimina(id: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }
}