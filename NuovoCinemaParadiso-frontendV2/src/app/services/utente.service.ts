import { environment } from '../../environments/environment';
import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Utente, UtenteCreazione } from '../models/utente.model';

@Injectable({
    providedIn: 'root'
})
export class UtenteService {

    private readonly http = inject(HttpClient);
    private readonly baseUrl = `${environment.apiBaseUrl}/Auth`;

    profilo(): Observable<Utente> {
        return this.http.get<Utente>(`${this.baseUrl}/profilo`);
    }

    modifica(payload: UtenteCreazione): Observable<any> {
        return this.http.put<any>(`${this.baseUrl}/modifica`, payload);
    }

    eliminaProfilo(): Observable<any> {
        return this.http.delete<any>(`${this.baseUrl}/elimina`);
    }
}