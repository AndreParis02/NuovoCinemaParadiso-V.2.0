import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { GenereMovie } from '../models/genere-movie.model';

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

}