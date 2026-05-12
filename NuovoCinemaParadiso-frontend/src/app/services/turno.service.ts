import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { Turno, TurnoCreazione  } from '../models/turno.model';


@Injectable({ providedIn: 'root' })
export class TurnoService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiBaseUrl}/turno`;

  ottieniTutto(): Observable<Turno[]> {
    return this.http.get<Turno[]>(this.baseUrl);
  }

  ottieniTramiteId(id: string): Observable<Turno> {
    return this.http.get<Turno>(`${this.baseUrl}/${id}`);
  }

  crea(payload: TurnoCreazione): Observable<Turno> {
    return this.http.post<Turno>(this.baseUrl, payload);
  }

  modifica(id: string, payload: TurnoCreazione): Observable<Turno> {
    return this.http.put<Turno>(`${this.baseUrl}/${id}`, payload);
  }

  elimina(id: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }
}