import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { TipologiaSala, TipologiaSalaCreazione } from '../models/tipologia-sala.model';


@Injectable({
  providedIn: 'root',
})
export class TipologiaSalaService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiBaseUrl}/tipologiaSala`;

  ottieniTutto(): Observable<TipologiaSala[]>
  {
    return this.http.get<TipologiaSala[]>(this.baseUrl);
  }

  ottieniTramiteId(id:string): Observable<TipologiaSala>
  {
    return this.http.get<TipologiaSala>(`${this.baseUrl}/${id}`);
  }

  crea(payload:TipologiaSalaCreazione): Observable<TipologiaSala>
  {
    return this.http.post<TipologiaSala>(this.baseUrl, payload);
  }

  modifica(id:string, payload: TipologiaSalaCreazione): Observable<TipologiaSala>
  {
    return this.http.put<TipologiaSala>(`${this.baseUrl}/${id}`, payload);
  }

  elimina(id:string): Observable<void>
  {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }
}