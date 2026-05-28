import { environment } from '../../environments/environment';
import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import {UtenteModificaRuoloRichiesta, UtenteModificaRuoloRisposta } from '../models/utente.model';


@Injectable({
    providedIn: 'root'
})
export class GestioneUtenteService {
    private readonly http = inject(HttpClient);
    private readonly baseUrl = `${environment.apiBaseUrl}/RuoloUtenti/cambia-ruolo`;


    modificaRuolo(payload: UtenteModificaRuoloRichiesta): Observable<UtenteModificaRuoloRisposta> {
        return this.http.put<UtenteModificaRuoloRisposta>(`${this.baseUrl}`, payload);
    }
}
