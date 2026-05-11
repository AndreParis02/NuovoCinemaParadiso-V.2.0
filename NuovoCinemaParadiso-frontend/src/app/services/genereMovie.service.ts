import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { GenereMovie, GenereMovieCreazione } from '../models/GenereMovie.model';

@Injectable({
  providedIn: 'root',
})

export class GenereMovieService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiBaseUrl}/generemovie`;

  ottieniTutto(): Observable<GenereMovie[]>
  {
    return this.http.get<GenereMovie[]>(this.baseUrl);
  }

  ottieniTramiteId(id:number): Observable<GenereMovie>
  {
    return this.http.get<GenereMovie>(`${this.baseUrl}/${id}`);
  }

  crea(payload:GenereMovieCreazione): Observable<GenereMovie>
  {
    return this.http.post<GenereMovie>(this.baseUrl, payload);
  }

  aggiorna(id:number, payload: GenereMovieCreazione): Observable<GenereMovie>
  {
    return this.http.put<GenereMovie>(`${this.baseUrl}/${id}`, payload);
  }

  elimina(id:number): Observable<void>
  {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }
}