import { environment } from '../../environments/environment';
import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Biglietto } from '../models/biglietto.model';
import { Utente, UtenteModificaRuolo } from '../models/utente.model';
import { LogAzioni } from '../models/logAzioni.model';

@Injectable({
    providedIn: 'root'
})
export class GestoreService {

    private readonly http = inject(HttpClient);
    private readonly baseUrl = `${environment.apiBaseUrl}/gestore`;

    ottieniLogAzioni(): Observable<LogAzioni[]> {
        return this.http.get<LogAzioni[]>(`${this.baseUrl}/log`)
    }
}