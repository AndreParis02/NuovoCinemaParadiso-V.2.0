import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { Abbonamento, AbbonamentoCreazione  } from '../models/abbonamento.model';


@Injectable({ providedIn: 'root' })
export class AbbonamentoService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiBaseUrl}/abbonamento`;

  ottieniTutto(): Observable<Abbonamento[]> {
    return this.http.get<Abbonamento[]>(this.baseUrl);
  }

  ottieniTramiteId(id: string): Observable<Abbonamento> {
    return this.http.get<Abbonamento>(`${this.baseUrl}/${id}`);
  }

  crea(payload: AbbonamentoCreazione): Observable<Abbonamento> {
    return this.http.post<Abbonamento>(this.baseUrl, payload);
  }

  modifica(id: string, payload: AbbonamentoCreazione): Observable<Abbonamento> {
    return this.http.put<Abbonamento>(`${this.baseUrl}/${id}`, payload);
  }

  elimina(id: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }
}