import { environment } from '../../environments/environment';
import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import {UtenteModificaRuolo } from '../models/utente.model';


@Injectable({
    providedIn: 'root'
})
export class GestioneUtenteService {
    private readonly http = inject(HttpClient);
    private readonly baseUrl = `${environment.apiBaseUrl}/gestone-utente`;


    modificaRuolo(id: string, payload: UtenteModificaRuolo): Observable<string> {
        return this.http.put<string>(`${this.baseUrl}/${id}`, payload);
    }
}
