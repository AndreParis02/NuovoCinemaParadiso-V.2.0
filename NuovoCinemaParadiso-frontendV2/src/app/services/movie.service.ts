import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { Movie, MovieCreazione } from '../models/movie.model';


@Injectable({
  providedIn: 'root',
})
export class MovieService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiBaseUrl}/movie`;

  ottieniTutto(): Observable<Movie[]>
  {
    return this.http.get<Movie[]>(this.baseUrl);
  }

  ottieniTramiteId(id:string): Observable<Movie>
  {
    return this.http.get<Movie>(`${this.baseUrl}/${id}`);
  }

  ottieniTramiteGenere(id:string): Observable<Movie[]>
  {
    return this.http.get<Movie[]>(`${this.baseUrl}/genere/${id}`);
  }

  crea(payload:MovieCreazione): Observable<Movie>
  {
    return this.http.post<Movie>(this.baseUrl, payload);
  }

  modifica(id:string, payload: MovieCreazione): Observable<Movie>
  {
    return this.http.put<Movie>(`${this.baseUrl}/${id}`, payload);
  }

  elimina(id:string): Observable<void>
  {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }
}