import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { LogAzioni, LogAzioniCreazione } from '../models/logAzioni.model';

@Injectable({ providedIn: 'root' })
export class LogAzioniService {

  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiBaseUrl}/gestore/logs`;


  ottieniTutti(): Observable<LogAzioni[]> {
    return this.http.get<LogAzioni[]>(this.baseUrl);
  }
}
