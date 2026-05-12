import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { Proiezione, ProiezioneCreazione } from '../models/proiezione.model';


@Injectable({ providedIn: 'root' })
export class ProiezioneService {
    private readonly http = inject(HttpClient);
    private readonly baseUrl = `${environment.apiBaseUrl}/proiezione`;

    ottieniTutto(): Observable<Proiezione[]> {
        return this.http.get<Proiezione[]>(this.baseUrl);
    }

    ottieniTuttoStorico(): Observable<Proiezione[]> {
        return this.http.get<Proiezione[]>(`${this.baseUrl}/storico`);
    }

    ottieniTramiteId(id: string): Observable<Proiezione> {
        return this.http.get<Proiezione>(`${this.baseUrl}/${id}`);
    }

    ottieniTramiteMovieId(id:string): Observable <Proiezione> {
        return this.http.get<Proiezione>(`${this.baseUrl}/movie/${id}`);
    }

    ottieniTramiteSalaId(id:string): Observable <Proiezione> {
        return this.http.get<Proiezione>(`${this.baseUrl}/sala/${id}`);
    }

    ottieniTramiteTurnoId(id:string): Observable <Proiezione> {
        return this.http.get<Proiezione>(`${this.baseUrl}/turno/${id}`);
    }

    crea(payload: ProiezioneCreazione): Observable<Proiezione> {
        return this.http.post<Proiezione>(this.baseUrl, payload);
    }

    modifica(id: string, payload: ProiezioneCreazione): Observable<Proiezione> {
        return this.http.put<Proiezione>(`${this.baseUrl}/${id}`, payload);
    }

    elimina(id: string): Observable<void> {
        return this.http.delete<void>(`${this.baseUrl}/${id}`);
    }
}