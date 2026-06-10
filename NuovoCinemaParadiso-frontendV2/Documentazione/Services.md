## SERVICES

## abbonamento.service.ts
<details><summary> abbonamento.service V1.0 </summary>

```ts
/**
 * Service Angular dedicato alla gestione delle operazioni CRUD
 * relative all'entità abboanamento.
 *
 * Il service comunica con il backend tramite HttpClient
 * utilizzando gli endpoint REST esposti dall'API.
 */
import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { Abbonamento, AbbonamentoCreazione } from '../models/abbonamento.model';

@Injectable({ providedIn: 'root' })
export class AbbonamentoService {
  /**
   * Istanza del client HTTP utilizzata per effettuare
   * richieste verso il backend.
   */
  private readonly http = inject(HttpClient);
  /**
   * URL base dell'endpoint REST per la gestione
   *  di abbonamento.
   */
  private readonly baseUrl = `${environment.apiBaseUrl}/abbonamento`;
  /**
   * Recupera tutti gli abboanamenti presenti nel sistema.
   *
   * @returns Observable contenente un array di abboanamenti.
   */
  ottieniTutto(): Observable<Abbonamento[]> {
    return this.http.get<Abbonamento[]>(this.baseUrl);
  }
  /**
   * Recupera uno specifico abboanamento tramite identificativo GUID.
   *
   * @param id Identificativo univoco dell' abboanamento.
   * @returns Observable contenente l' abboanamento di risposta.
   */
  ottieniTramiteId(id: string): Observable<Abbonamento> {
    return this.http.get<Abbonamento>(`${this.baseUrl}/${id}`);
  }
  /**
   * crea un nuovo abboanamento utilizzando un payload.
   *
   * @param payload modello di richiesta per creazione dell' abboanamento.
   * @returns Observable contenente l' abboanamento di risposta.
   */
  crea(payload: AbbonamentoCreazione): Observable<Abbonamento> {
    return this.http.post<Abbonamento>(this.baseUrl, payload);
  }
  /**
   * Modifica uno specifico abboanamento tramite identificativo GUID con le informazioni all'interno di un payload.
   *
   * @param id Identificativo univoco dell abboanamento.
   * @param payload modello di richiesta per la modifica dell' abboanamento.
   * @returns Observable contenente l' abboanamento di risposta.
   */
  modifica(id: string, payload: AbbonamentoCreazione): Observable<Abbonamento> {
    return this.http.put<Abbonamento>(`${this.baseUrl}/${id}`, payload);
  }
  /**
   * Elimina uno specifico abbonamento tramite identificativo GUID.
   *
   * @param id Identificativo univoco dell' abbonamento.
   */
  elimina(id: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }
}
```
</details>

## auth.service.ts
<details><summary> auth.service V1.0 </summary>

```ts
import { environment } from '../../environments/environment';
import { inject, Injectable, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { tap } from 'rxjs/operators';
import { Observable } from 'rxjs';
import { Auth } from '../models/auth.model';
import { Login } from '../models/login.model';
import { Registrazione } from '../models/registrazione.model';
import { SessioneUtente } from '../models/sessione-utente.model';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private readonly http = inject(HttpClient);
  private readonly router = inject(Router);
  private readonly storagekey = 'nuovo_cinema_paradiso_auth';

  readonly utenteCorrente = signal<SessioneUtente | null>(this.readStoredUser());

  login(payload: Login): Observable<Auth> {
    return this.http
      .post<Auth>(`${environment.apiBaseUrl}/Auth/login`, payload)
      .pipe(tap((response) => this.setSession(response)));
  }

  registrazione(payload: Registrazione): Observable<{ message: string }> {
    return this.http.post<{ message: string }>(
      `${environment.apiBaseUrl}/Auth/registrazione`,
      payload,
    );
  }

  logout(): void {
    localStorage.removeItem(this.storagekey);
    this.utenteCorrente.set(null);
    void this.router.navigate(['/login']);
  }

  isAutenticato(): boolean {
    return this.utenteCorrente() !== null;
  }

  possiedeQualsiasiRuolo(ruoli: string[]): boolean {
    const ruolo = this.utenteCorrente()?.ruolo ?? '';
    return ruoli.includes(ruolo);
  }

  ruoloCorrispondente(ruolo: string): boolean {
    return this.utenteCorrente()?.ruolo === ruolo;
  }

  ottieniToken(): string | null {
    return this.utenteCorrente()?.token ?? null;
  }

  private setSession(risposta: Auth): void {
    const utenteInSessione: SessioneUtente = {
      token: risposta.token,
      id: risposta.id,
      email: risposta.email,
      nomeCompleto: risposta.nomeCompleto,
      ruolo: risposta.ruolo,
    };

    localStorage.setItem(this.storagekey, JSON.stringify(utenteInSessione));
    this.utenteCorrente.set(utenteInSessione);
  }

  private readStoredUser(): SessioneUtente | null {
    const raw = localStorage.getItem(this.storagekey);

    if (!raw) {
      return null;
    }

    try {
      return JSON.parse(raw) as SessioneUtente;
    } catch {
      localStorage.removeItem(this.storagekey);
      return null;
    }
  }
}
```
</details>

### Aggiornamento Codice

Utente: Fabio Tammaro
Data: 28/05/2026
Descrizione: Aggiunto il metodo isGestore per discriminare le azioni che tale ruolo permette.
<details><summary> auth-service.ts V1.1</summary>

``` typescript
import { environment } from '../../environments/environment';
import { inject, Injectable, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { tap } from 'rxjs/operators';
import { Observable } from 'rxjs';
import { Login } from '../models/login.model';
import { Registrazione } from '../models/registrazione.model';
import { SessioneUtente } from '../models/sessione-utente.model';
import { RuoliUtente } from '../ruoliUtente';

@Injectable({
  providedIn: 'root', 
})
export class AuthService {

  // inject() è il nuovo approccio Angular moderno alla dependency injection
  private readonly http = inject(HttpClient);

  // Router serve per effettuare redirect programmatici
  private readonly router = inject(Router);

  // chiave usata nel localStorage
  private readonly storagekey = 'nuovo_cinema_paradiso_auth';

  // url base del controller backend
  private readonly baseUrl = `${environment.apiBaseUrl}/Auth`;

  /*
    signal() crea uno stato reattivo Angular.

    utenteCorrente() contiene:
    - null se nessuno è loggato
    - SessioneUtente se autenticato

    readStoredUser() prova a recuperare l'utente salvato nel localStorage
    quando l'app viene ricaricata.
  */
  readonly utenteCorrente = signal<SessioneUtente | null>(this.readStoredUser());

  /*
    login():
    - invia email/password al backend
    - riceve il DTO SessioneUtente
    - salva la sessione tramite setSession()
  */
  login(payload: Login): Observable<SessioneUtente> {

    return this.http
      .post<SessioneUtente>(`${this.baseUrl}/login`, payload)

      /*
        tap() esegue effetti collaterali SENZA modificare la response.
        Qui viene usato per salvare la sessione dopo il login.
      */
      .pipe(tap((response) => this.setSession(response)));
  }

  registrazione(payload: Registrazione): Observable<{ message: string }> {

    return this.http.post<{ message: string }>(
      `${this.baseUrl}/registrazione`,
      payload
    );
  }

  /*
    logout():
    - rimuove sessione dal localStorage
    - resetta il signal
    - redirect al login
  */
  logout(): void {

    localStorage.removeItem(this.storagekey);

    // aggiorna immediatamente tutta la UI reattiva
    this.utenteCorrente.set(null);

    void this.router.navigate(['/login']);
  }

  
  isAutenticato(): boolean {
    return this.utenteCorrente() !== null;
  }

  isGestore(): boolean {
    return this.utenteCorrente()?.ruolo === 'Gestore';
  }

  /*
    verifica se il ruolo dell'utente è incluso
    nell'array di ruoli consentiti
  */
  possiedeQualsiasiRuolo(ruoli: RuoliUtente[]): boolean {

    const ruolo = this.ottieniRuoloUtente();

    return !!ruolo && ruoli.includes(ruolo);
  }

  /*
    verifica un ruolo specifico
  */
  ruoloCorrispondente(ruolo: string): boolean {

    return this.utenteCorrente()?.ruolo === ruolo;
  }

  ottieniToken(): string | null {

    return this.utenteCorrente()?.token ?? null;
  }

  ottieniRuoloUtente(): RuoliUtente | null {

    return this.utenteCorrente()?.ruolo ?? null;
  }

  /*
    setSession():
    salva i dati utente dopo login.
  */
  private setSession(risposta: SessioneUtente): void {

    const utenteInSessione: SessioneUtente = {

      id: risposta.id,
      nomeCompleto: risposta.nomeCompleto,
      token: risposta.token,
      eta: risposta.eta,
      email: risposta.email,
      ruolo: risposta.ruolo,
      dataInizioAbbonamento: risposta.dataInizioAbbonamento,
      dataInizioGiftCard: risposta.dataInizioGiftCard,
      seAbbonato: risposta.seAbbonato,
      possiedeGiftCard: risposta.possiedeGiftCard
    }

    /*
      salvataggio persistente browser

      JSON.stringify() converte oggetto -> stringa JSON
    */
    localStorage.setItem(
      this.storagekey,
      JSON.stringify(utenteInSessione)
    );

    /*
      aggiorna signal reattivo Angular

      tutti i componenti che usano utenteCorrente()
      si aggiornano automaticamente.
    */
    this.utenteCorrente.set(utenteInSessione);
  }

  /*
    readStoredUser():
    recupera utente dal localStorage all'avvio app.
  */
  private readStoredUser(): SessioneUtente | null {

    const raw = localStorage.getItem(this.storagekey);

    if (!raw) {
      return null;
    }

    try {

      /*
        JSON.parse():
        converte stringa JSON -> oggetto JS
      */
      return JSON.parse(raw) as SessioneUtente;

    } catch {

      /*
        se JSON è corrotto:
        - rimuove dati invalidi
        - evita crash applicazione
      */
      localStorage.removeItem(this.storagekey);

      return null;
    }
  }
}
```

### Aggiornamento Codice

Utente: Marco Strazzeri
Data: 28/05/2026
Descrizione: Fixata la funzione ottieniRuoloUtente, prima dava problemi con la lettura del ruolo

<details><summary> auth-service.ts V1.2</summary>

```ts

import { environment } from '../../environments/environment';
import { inject, Injectable,signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { tap } from 'rxjs/operators';
import { Observable } from 'rxjs';
import { Login } from '../models/login.model';
import { Registrazione } from '../models/registrazione.model';
import { SessioneUtente } from '../models/sessione-utente.model';
import { RuoliUtente } from '../ruoliUtente';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private readonly http = inject(HttpClient);
  private readonly router = inject(Router);
  private readonly storagekey = 'nuovo_cinema_paradiso_auth';
  private readonly baseUrl = `${environment.apiBaseUrl}/Auth`;

  readonly utenteCorrente = signal<SessioneUtente | null>(this.readStoredUser());

  login(payload: Login): Observable<SessioneUtente> {

    return this.http
      .post<SessioneUtente>(`${this.baseUrl}/login`, payload)
      .pipe(tap((response) => this.setSession(response)));
  }

  registrazione(payload: Registrazione): Observable<{ message: string }> {
    return this.http.post<{ message: string }>(`${this.baseUrl}/registrazione`, payload);

  }

  logout(): void {
    localStorage.removeItem(this.storagekey);
    this.utenteCorrente.set(null);
    void this.router.navigate(['/login']);
  }

  isAutenticato(): boolean {
    return this.utenteCorrente() !== null;
  }

 isGestore(): boolean {
  return this.utenteCorrente()?.ruolo === 'Gestore';
 }
  possiedeQualsiasiRuolo(ruoli: RuoliUtente[]): boolean {
    const ruolo = this.ottieniRuoloUtente();
    return !!ruolo && ruoli.includes(ruolo);
  }

  ruoloCorrispondente(ruolo: string): boolean {
    return this.utenteCorrente()?.ruolo === ruolo;

  }

  ottieniToken(): string | null {
    return this.utenteCorrente()?.token ?? null;
  }
  
/*
  ottieniRuoloUtente(): RuoliUtente | null{
    const ruolo = localStorage.getItem('ruolo');
    console.log('Ruolo ottenuto dal localStorage:', ruolo); // Debug log
    if(ruolo === 'Operatore' || ruolo === 'Gestore' || ruolo === 'Utente')
    {
      return ruolo;
    }
    return null;
  }  */



  ottieniRuoloUtente(): RuoliUtente | null {
  const raw = localStorage.getItem(this.storagekey);  //legge i dati dal localstorage

    if (!raw) {
      return null;
    }

    try {
      const utente = JSON.parse(raw) as SessioneUtente; //lettura del json

      if (
        utente.ruolo === 'Operatore' ||
        utente.ruolo === 'Gestore' ||
        utente.ruolo === 'Utente'
      ) {
        return utente.ruolo;  //ritorno del ruolo se valido
      }

      return null;
    } catch {
      return null;
  }
}

  private setSession(risposta: SessioneUtente): void {
    const utenteInSessione: SessioneUtente = {
      id: risposta.id,
      nomeCompleto: risposta.nomeCompleto,
      token: risposta.token,
      eta: risposta.eta,
      email: risposta.email,
      ruolo: risposta.ruolo,
      dataInizioAbbonamento: risposta.dataInizioAbbonamento,
      dataInizioGiftCard: risposta.dataInizioGiftCard,
      seAbbonato: risposta.seAbbonato,
      possiedeGiftCard: risposta.possiedeGiftCard
    }

    localStorage.setItem(this.storagekey, JSON.stringify(utenteInSessione));
    console.log('Utente salvato in localStorage:', utenteInSessione.ruolo); // Debug log
    this.utenteCorrente.set(utenteInSessione);
  }

  private readStoredUser(): SessioneUtente | null {
    const raw = localStorage.getItem(this.storagekey);

    if (!raw) {
      return null;
    }
    
    try{
      return JSON.parse(raw) as SessioneUtente;
      
    } catch{
      localStorage.removeItem(this.storagekey);
      return null
    }
  }
}

```

### Aggiornamento Codice

<details>
<summary>Versione 1.3</summary>
Fabio 9-06-2026
Descrizione: Import NavbarSharedStateService

```ts
import { environment } from '../../environments/environment';
import { inject, Injectable, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { tap } from 'rxjs/operators';
import { Observable } from 'rxjs';
import { Login } from '../models/login.model';
import { Registrazione } from '../models/registrazione.model';
import { SessioneUtente } from '../models/sessione-utente.model';
import { RuoliUtente } from '../ruoliUtente';

@Injectable({
    providedIn: 'root',
})
export class AuthService {

    private readonly http = inject(HttpClient);
    private readonly router = inject(Router);
    private readonly storageKey = 'nuovo_cinema_paradiso_auth';
    private readonly baseUrl = `${environment.apiBaseUrl}/Auth`;



    // Stato utente sincronizzato con localStorage
    readonly utenteCorrente = signal<SessioneUtente | null>(this.caricaUtenteDaStorage());

    // ---------------------------
    // LOGIN
    // ---------------------------
  login(payload: Login): Observable<SessioneUtente> {
    return this.http.post<SessioneUtente>(`${this.baseUrl}/login`, payload).pipe(
      tap(response => {
        this.salvaSessione(response);
        
        // ALIMENTIAMO IL CANALE: l'effetto nel NavbarSharedStateService 
        // intercetterà questo cambio e caricherà profilo e saldo!
        this.utenteCorrente.set(response);
      })
    );
  }

    // ---------------------------
    // REGISTRAZIONE
    // ---------------------------
    registrazione(payload: Registrazione): Observable<{ message: string }> {
        return this.http.post<{ message: string }>(
            `${this.baseUrl}/registrazione`,
            payload
        );
    }

    // ---------------------------
    // LOGOUT
    // ---------------------------
    logout(): void {
        localStorage.removeItem(this.storageKey);
        this.utenteCorrente.set(null);
        void this.router.navigate(['/login']);
    }

    // ---------------------------
    // STATO UTENTE
    // ---------------------------
    isAutenticato(): boolean {
        return this.utenteCorrente() !== null;
    }

    isOperatore(): boolean {
        return this.utenteCorrente()?.ruolo === 'Operatore'
    }

    isGestore(): boolean {
        return this.utenteCorrente()?.ruolo === 'Gestore';
    }
    possiedeQualsiasiRuolo(ruoli: RuoliUtente[]): boolean {
        const ruolo = this.ottieniRuoloUtente();
        return !!ruolo && ruoli.includes(ruolo);
    }

    ruoloCorrispondente(ruolo: string): boolean {
        return this.utenteCorrente()?.ruolo === ruolo;
    }

    ottieniToken(): string | null {
        return this.utenteCorrente()?.token ?? null;
    }

    ottieniRuoloUtente(): RuoliUtente | null {
        const raw = localStorage.getItem(this.storageKey);

        if (!raw) {
            return null;
        }

        try {
            const utente = JSON.parse(raw) as SessioneUtente;

            if (
                utente.ruolo === 'Operatore' ||
                utente.ruolo === 'Gestore' ||
                utente.ruolo === 'Utente'
            ) {
                return utente.ruolo;
            }

            return null;
        } catch {
            return null;
        }
    }

    private setSession(risposta: SessioneUtente): void {
        const utenteInSessione: SessioneUtente = {
            id: risposta.id,
            nomeCompleto: risposta.nomeCompleto,
            token: risposta.token,
            eta: risposta.eta,
            email: risposta.email,
            ruolo: risposta.ruolo,
            dataInizioAbbonamento: risposta.dataInizioAbbonamento,
            seAbbonato: risposta.seAbbonato,
        }

        localStorage.setItem(this.storageKey, JSON.stringify(utenteInSessione));
        console.log('Utente salvato in localStorage:', utenteInSessione.ruolo);
        this.utenteCorrente.set(utenteInSessione)
    }

    // ---------------------------
    // PRIVATE
    // ---------------------------
    private salvaSessione(risposta: SessioneUtente): void {
        localStorage.setItem(this.storageKey, JSON.stringify(risposta));
        this.utenteCorrente.set(risposta);
    }

    private caricaUtenteDaStorage(): SessioneUtente | null {
        const raw = localStorage.getItem(this.storageKey);
        if (!raw) return null;

        try {
            return JSON.parse(raw) as SessioneUtente;
        } catch {
            localStorage.removeItem(this.storageKey);
            return null;
        }
    }
}
```

</details>

### Aggiornamento Codice

<details>
<summary>Versione 1.4</summary>
Fabio 10-06-2026
Descrizione: aggiunto isUtente

```ts
import { environment } from '../../environments/environment';
import { inject, Injectable, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { tap } from 'rxjs/operators';
import { Observable } from 'rxjs';
import { Login } from '../models/login.model';
import { Registrazione } from '../models/registrazione.model';
import { SessioneUtente } from '../models/sessione-utente.model';
import { RuoliUtente } from '../ruoliUtente';

@Injectable({
    providedIn: 'root',
})
export class AuthService {

    private readonly http = inject(HttpClient);
    private readonly router = inject(Router);
    private readonly storageKey = 'nuovo_cinema_paradiso_auth';
    private readonly baseUrl = `${environment.apiBaseUrl}/Auth`;

    readonly utenteCorrente = signal<SessioneUtente | null>(this.caricaUtenteDaStorage());

  login(payload: Login): Observable<SessioneUtente> {

    return this.http.post<SessioneUtente>(`${this.baseUrl}/login`, payload).pipe(
      tap(response => {
        this.salvaSessione(response);
        this.utenteCorrente.set(response);
      })
    );
  }

    registrazione(payload: Registrazione): Observable<{ message: string }> {
        return this.http.post<{ message: string }>(
            `${this.baseUrl}/registrazione`,
            payload
        );
    }

    logout(): void {
        localStorage.removeItem(this.storageKey);
        this.utenteCorrente.set(null);
        void this.router.navigate(['/login']);
    }

    isAutenticato(): boolean {
        return this.utenteCorrente() !== null;
    }

    isOperatore(): boolean {
        return this.utenteCorrente()?.ruolo === 'Operatore'
    }

    isGestore(): boolean {
        return this.utenteCorrente()?.ruolo === 'Gestore';
    }
    
    isUtente(): boolean{
     return this.utenteCorrente()?.ruolo === 'Utente';
    }
    
    possiedeQualsiasiRuolo(ruoli: RuoliUtente[]): boolean {
        const ruolo = this.ottieniRuoloUtente();
        return !!ruolo && ruoli.includes(ruolo);
    }

    ruoloCorrispondente(ruolo: string): boolean {
        return this.utenteCorrente()?.ruolo === ruolo;
    }

    ottieniToken(): string | null {
        return this.utenteCorrente()?.token ?? null;
    }

    ottieniRuoloUtente(): RuoliUtente | null {
        const raw = localStorage.getItem(this.storageKey);

        if (!raw) {
            return null;
        }

        try {
            const utente = JSON.parse(raw) as SessioneUtente;

            if (
                utente.ruolo === 'Operatore' ||
                utente.ruolo === 'Gestore' ||
                utente.ruolo === 'Utente'
            ) {
                return utente.ruolo;
            }

            return null;
        } catch {
            return null;
        }
    }

    private salvaSessione(risposta: SessioneUtente): void {
        localStorage.setItem(this.storageKey, JSON.stringify(risposta));
        this.utenteCorrente.set(risposta);
    }

    private caricaUtenteDaStorage(): SessioneUtente | null {
        const raw = localStorage.getItem(this.storageKey);
        if (!raw) return null;

        try {
            return JSON.parse(raw) as SessioneUtente;
        } catch {
            localStorage.removeItem(this.storageKey);
            return null;
        }
    }
}
```


## biglietto.service
<details><summary> biglietto.service V1.0 </summary>

```ts
import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { Biglietto, BigliettoCreazione } from '../models/biglietto.model';

@Injectable({
  providedIn: 'root',
})
export class BigliettoService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiBaseUrl}/biglietto`;

  ottieniTutto(utenteId: string): Observable<Biglietto[]> {
    return this.http.get<Biglietto[]>(`${this.baseUrl}/utente/${utenteId}`);
  }

  ottieniTramiteId(id: string, utenteId: string): Observable<Biglietto> {
    return this.http.get<Biglietto>(`${this.baseUrl}/${id}/utente/${utenteId}`);
  }
  
  // la creazione, corrispondente all'acquisto deve corrispondere al backend che ha come risposta una stringa di conferma o di insuccesso.
  crea(payload: BigliettoCreazione): Observable<any> {
    return this.http.post<any>(this.baseUrl, payload);  
  }

  modifica(id: string, payload: BigliettoCreazione): Observable<Biglietto> {
    return this.http.put<Biglietto>(`${this.baseUrl}/${id}`, payload);
  }

  elimina(id: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }
}

```
</details>

### Aggiornamento Codice

<details><summary> biglietto.service V1.1 </summary>

Francesco Lorenzi 10/06/2026

ho dovuto modificare la richiesta di creazione, che accettava solo un file json come risposta osservabile, tuttavia il backend restituiva una stringa messaggio dando così un errore anche se l'azione avveniva correttamente
```ts
import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { Biglietto, BigliettoCreazione } from '../models/biglietto.model';

@Injectable({
  providedIn: 'root',
})
export class BigliettoService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiBaseUrl}/biglietto`;

  ottieniTutto(utenteId: string): Observable<Biglietto[]> {
    return this.http.get<Biglietto[]>(`${this.baseUrl}/utente/${utenteId}`);
  }

  ottieniTramiteId(id: string, utenteId: string): Observable<Biglietto> {
    return this.http.get<Biglietto>(`${this.baseUrl}/${id}/utente/${utenteId}`);
  }
  //ora l'osservabile è una stringa e non un "any" che accettava qualsiasi tipo di JSON
  crea(payload: BigliettoCreazione): Observable<string> {
  return this.http.post(this.baseUrl, payload, { responseType: 'text' });
  }

  modifica(id: string, payload: BigliettoCreazione): Observable<Biglietto> {
    return this.http.put<Biglietto>(`${this.baseUrl}/${id}`, payload);
  }

  elimina(id: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }
}

```
</details>
 
## genere-movie.service.ts
<details><summary> genere-movie.service V1.0 </summary>

```ts
import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { GenereMovie, GenereMovieCreazione } from '../models/genere-movie.model';
/**
 * Service Angular dedicato alla gestione delle operazioni CRUD
 * relative all'entità generemovie.
 *
 * Il service comunica con il backend tramite HttpClient
 * utilizzando gli endpoint REST esposti dall'API.
 */
@Injectable({
  providedIn: 'root',
})
export class GenereMovieService {
  /**
   * Istanza del client HTTP utilizzata per effettuare
   * richieste verso il backend.
   */
  private readonly http = inject(HttpClient);
  /**
   * URL base dell'endpoint REST per la gestione
   * delle tipologie di sala.
   */
  private readonly baseUrl = `${environment.apiBaseUrl}/generemovie`;

  /**
   * Recupera tutti i generimovie presenti nel sistema.
   *
   * @returns Observable contenente un array di generimovie.
   */
  ottieniTutto(): Observable<GenereMovie[]> {
    return this.http.get<GenereMovie[]>(this.baseUrl);
  }

  /**
   * Recupera uno specifico generemovie tramite identificativo GUID.
   *
   * @param id Identificativo univoco del generemovie.
   * @returns Observable contenente il generemovie di risposta.
   */
  ottieniTramiteId(id: number): Observable<GenereMovie> {
    return this.http.get<GenereMovie>(`${this.baseUrl}/${id}`);
  }

  /**
   * crea un nuovo generemovie utilizzando un payload.
   *
   * @param payload modello di richiesta per creazione del generemovie.
   * @returns Observable contenente il generemovie di risposta.
   */
  crea(payload: GenereMovieCreazione): Observable<GenereMovie> {
    return this.http.post<GenereMovie>(this.baseUrl, payload);
  }

  /**
   * Modifica uno specifico generemovie tramite identificativo GUID con le informazioni all'interno di un payload.
   *
   * @param id Identificativo univoco del generemovie.
   * @param payload modello di richiesta per la modifica del generemovie.
   * @returns Observable contenente il generemovie di risposta.
   */
  modifica(id: number, payload: GenereMovieCreazione): Observable<GenereMovie> {
    return this.http.put<GenereMovie>(`${this.baseUrl}/${id}`, payload);
  }

  /**
   * Elimina uno specifico generemovie tramite identificativo GUID.
   *
   * @param id Identificativo univoco del generemovie.
   */
  elimina(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }
}
```
</details>

### Aggiornamento Codice

Utente: Marco Strazzeri
Data: 25/05/2026
Descrizione: Modificato per gestire il solo caricamento della lista dei generi

<details><summary> genere-movie.service.ts V1.1</summary>

```ts
//controllare gli import. nomi classi e file sono differenti per "-"
import { Component, inject, signal } from '@angular/core';
//import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { AuthService } from '../../services/auth.service';
import { GenereMovieService } from '../../services/genere-movie.service';
import { GenereMovie } from '../../models/genere-movie.model';

@Component({
  selector: 'app-genere-movie',
  imports: [],
  templateUrl: './genere-movie.page.html',
  styleUrl: './genere-movie.page.css',
})
export class GenereMoviePage {
  private readonly authService = inject(AuthService);
  private readonly genereMovieService = inject(GenereMovieService);

  readonly generiMovies = signal<GenereMovie[]>([]);
  readonly isLoading = signal(false);
  readonly errorMessage = signal('');
  readonly successMessage = signal('');

  constructor() {
    this.loadGeneriMovies();
  }

  //carica la lista dei generi dal backend
  loadGeneriMovies(): void {
    this.isLoading.set(true);
    this.errorMessage.set('');

    //chiamata al servizio ottienitutto per ottenere la lista dei generi dei film
    this.genereMovieService.ottieniTutto().subscribe({
      next: (items) => {
        this.generiMovies.set(items);
        this.isLoading.set(false);
      },
      error: (error: unknown) => {
        this.isLoading.set(false);
        this.errorMessage.set(
          this.extractErrorMessage(error, 'Impossibile caricare i generi dei film'),
        );
      },
    });
  }

  trackById(_: string, item: GenereMovie): string | null {
    return item.id;
  }

  private extractErrorMessage(error: unknown, fallback: string): string {
    if (error instanceof Error) {
      return error?.message ?? fallback;
    }
    return fallback;
  }
}
```
</details>

## gestione-utente.service.ts

<details><summary> gestione-utente.service V1.0 </summary>

```ts
import { environment } from '../../environments/environment';
import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { UtenteModificaRuolo } from '../models/utente.model';

@Injectable({
  providedIn: 'root',
})
export class GestioneUtenteService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiBaseUrl}/gestione-utente`;

  modificaRuolo(id: string, payload: UtenteModificaRuolo): Observable<string> {
    return this.http.put<string>(`${this.baseUrl}/${id}`, payload);
  }
}
```
</details>

### Aggiornamento Codice

<details>
<details><summary> V1.1 </summary>
Utente: Andrea Paris
Data: 28/05/2026
Descrizione: apportati cambiamenti al metodo modificaRuolo

```ts
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

```
</details>

## gestione.service.ts
<details><summary>gestioneservice.ts V1.0 </summary>

- Utente: Simeone
- Data: 28/05/2026
- Descrizione: service per le funzionalità della pagina gestione per il ruolo gestore con i collegamenti ponti al backend

```ts
import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { LogAzioni, ContoCinema, Biglietto, GiftCard } from '../models/gestione.model'; // Adatta il percorso

@Injectable({ providedIn: 'root' })
export class GestioneService { 

  private readonly http = inject(HttpClient);
  private readonly gestoreUrl = `${environment.apiBaseUrl}/Gestore`;// Il gestoreUrl punta alla radice del controller Gestor
  private readonly giftCardUrl = `${environment.apiBaseUrl}/GiftCard`;


  ottieniLog(): Observable<LogAzioni[]> {
    return this.http.get<LogAzioni[]>(`${this.gestoreUrl}/logs`);
  }
 // da qua sotto TUTTE MODIFICHE ED AGGIUNTE
  ottieniConto(): Observable<ContoCinema> {
    return this.http.get<ContoCinema>(`${this.gestoreUrl}/conto`);
  }

  ottieniBiglietti(): Observable<Biglietto[]> {
    return this.http.get<Biglietto[]>(`${this.gestoreUrl}/biglietti`);
  }

  ottieniGiftCards(): Observable<GiftCard[]> {
    return this.http.get<GiftCard[]>(this.giftCardUrl); //è un get nudo e crudio 
  }
}

```

## gestore.service.cs

<details><summary> gestore.service V1.0 </summary>

```ts
import { environment } from '../../environments/environment';
import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Biglietto } from '../models/biglietto.model';
import { Utente, UtenteModificaRuolo } from '../models/utente.model';
import { LogAzioni } from '../models/logAzioni.model';

@Injectable({
  providedIn: 'root',
})
export class GestoreService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiBaseUrl}/gestore`;

  // GET tutti utenti
  OttieniUtenti(): Observable<Utente[]> {
    return this.http.get<Utente[]>(`${this.baseUrl}/utenti`);
  }

  // GET utente per id
  OttieniUtentePerId(id: string): Observable<Utente> {
    return this.http.get<Utente>(`${this.baseUrl}/utenti/${id}`);
  }

  // DELETE utente
  eliminaUtente(id: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/utenti/${id}`);
  }

  // GET utenti per abbonamento
  ottieniUtentiTramiteAbbonamento(abbonamentoId: string): Observable<Utente[]> {
    return this.http.get<Utente[]>(`${this.baseUrl}/utenti/abbonamento/${abbonamentoId}`);
  }

  // GET utenti per giftcard
  ottieniUtentiTramiteGiftCard(giftCardId: string): Observable<Utente[]> {
    return this.http.get<Utente[]>(`${this.baseUrl}/utenti/giftcard/${giftCardId}`);
  }

  // GET tutti biglietti
  ottieniBiglietti(): Observable<Biglietto[]> {
    return this.http.get<Biglietto[]>(`${this.baseUrl}/biglietti`);
  }

  // GET biglietto per id
  ottieniBigliettoPerId(id: string): Observable<Biglietto> {
    return this.http.get<Biglietto>(`${this.baseUrl}/biglietti/${id}`);
  }

  ottieniLogAzioni(): Observable<LogAzioni[]> {
    return this.http.get<LogAzioni[]>(`${this.baseUrl}/log`);
  }
}
```
</details>

## giftcard.service.ts

<details><summary> giftcard.service V1.0 </summary>

```ts
import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { GiftCard, GiftCardCreazione } from '../models/giftCard.model';
/**
 * Service Angular dedicato alla gestione delle operazioni CRUD
 * relative all'entità giftcard.
 *
 * Il service comunica con il backend tramite HttpClient
 * utilizzando gli endpoint REST esposti dall'API.
 */
@Injectable({
  providedIn: 'root',
})
export class GiftCardService {
  /**
   * Istanza del client HTTP utilizzata per effettuare
   * richieste verso il backend.
   */
  private readonly http = inject(HttpClient);
  /**
   * URL base dell'endpoint REST per la gestione
   * delle tipologie di sala.
   */
  private readonly baseUrl = `${environment.apiBaseUrl}/giftcard`;

  /**
   * Recupera tutte le giftcard presenti nel sistema.
   *
   * @returns Observable contenente un array di giftcard.
   */
  ottieniTutto(): Observable<GiftCard[]> {
    return this.http.get<GiftCard[]>(this.baseUrl);
  }

  /**
   * Recupera una specifica giftcard tramite identificativo GUID.
   *
   * @param id Identificativo univoco della giftcard.
   * @returns Observable contenente la giftcard di risposta.
   */
  ottieniTramiteId(id: number): Observable<GiftCard> {
    return this.http.get<GiftCard>(`${this.baseUrl}/${id}`);
  }

  /**
   * crea un nuova giftcard utilizzando un payload.
   *
   * @param payload modello di richiesta per creazione della giftcard.
   * @returns Observable contenente la giftcard di risposta.
   */
  crea(payload: GiftCardCreazione): Observable<GiftCard> {
    return this.http.post<GiftCard>(this.baseUrl, payload);
  }

  /**
   * Modifica una specifica giftcard tramite identificativo GUID con le informazioni all'interno di un payload.
   *
   * @param id Identificativo univoco della giftcard.
   * @param payload modello di richiesta per la modifica della giftcard.
   * @returns Observable contenente la giftcard di risposta.
   */
  modifica(id: number, payload: GiftCardCreazione): Observable<GiftCard> {
    return this.http.put<GiftCard>(`${this.baseUrl}/${id}`, payload);
  }
  /**
   * Elimina una specifica giftcard tramite identificativo GUID.
   *
   * @param id Identificativo univoco della giftcard.
   */
  elimina(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }
}
```
</details>

### Aggiornamento Codice

<details><summary>Versione 1.1</summary>

- Utente: Simeone
- Data: 05/06/2026
```ts
import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { GiftCard, GiftCardCreazione } from '../models/giftCard.model';

@Injectable({
    providedIn: 'root',
})

export class GiftCardService {
    private readonly http = inject(HttpClient);
    private readonly baseUrl = `${environment.apiBaseUrl}/giftcard`;
    private readonly utenteUrl = `${environment.apiBaseUrl}/Utente/giftCard`;
    // verifica attentamente se il codice qua sopra è nelle giuste rotte backend

    ottieniTutto(): Observable<GiftCard[]> {
        return this.http.get<GiftCard[]>(this.baseUrl);
    }

    ottieniTramiteId(id: String): Observable<GiftCard> {
        return this.http.get<GiftCard>(`${this.baseUrl}/${id}`); //prima c'era scritto id number, ERRORE
    }

    crea(payload: GiftCardCreazione): Observable<GiftCard> {
        return this.http.post<GiftCard>(this.baseUrl, payload);
    }

    modifica(id: string, payload: GiftCardCreazione): Observable<GiftCard> {
        return this.http.put<GiftCard>(`${this.baseUrl}/${id}`, payload);
    }

    elimina(id: string): Observable<void> {
        return this.http.delete<void>(`${this.baseUrl}/${id}`);
    }

    //AGGIUNTEVMIE

    // Ricarica si aspetta un DtoRicaricaGiftCard dal backend, che ha solo l'Importo
    ricarica(importo: number): Observable<{messaggio: string}> {
        return this.http.put<{messaggio: string}>(`${this.utenteUrl}/ricarica`, { importo: importo });
    }

    // Riscatto si aspetta un DtoCodiceRiscatto
    riscatta(codiceRiscatto: string): Observable<any> {
        return this.http.post<any>(`${this.utenteUrl}/riscatta`, { codiceRiscatto: codiceRiscatto });
    }
}
```

</details>
 
## log-azioni.service.ts (Inglobato in gestore)
<details><summary> log-azioni.service V1.0 </summary>

```ts
import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { LogAzione, LogAzioneCreazione } from '../models/log-azioni.model';

/**
 * Service Angular dedicato alla gestione dei log delle azioni.
 *
 * Questo service ha lo scopo di replicare il comportamento del
 * LogAzioniService backend, mantenendo coerenza architetturale:
 * - metodi espliciti
 * - nessuna logica nascosta
 * - nessuna trasformazione automatica
 * - nessun interceptor o pipe complesso
 *
 * Il service si limita a:
 * 1. inviare richieste HTTP al backend
 * 2. restituire i dati così come arrivano
 * 3. mantenere un'interfaccia chiara e leggibile
 */
@Injectable({ providedIn: 'root' })
export class LogAzioniService {
  /**
   * Istanza di HttpClient ottenuta tramite dependency injection.
   * Viene utilizzata per comunicare con il backend.
   */
  private readonly http = inject(HttpClient);

  /**
   * URL base dell'endpoint REST dedicato ai log.
   *
   * Nota: il backend espone presumibilmente un controller come:
   * [HttpPost("/logazioni")] e [HttpGet("/logazioni")]
   */
  private readonly baseUrl = `${environment.apiBaseUrl}/logazioni`;

  /**
   * Salva un nuovo log nel backend.
   *
   * Questo metodo corrisponde al metodo backend:
   *   SalvataggioLogAzioneAsync(string? idUtente, string azione, bool effettuato)
   *
   * Il payload Angular deve contenere:
   * - idUtente: string | null
   * - azione: string
   * - effettuato: boolean
   *
   * Il backend genera automaticamente:
   * - messaggio ("operazione eseguita" / "operazione fallita")
   * - timestamp
   * - id del log
   *
   * @param payload modello contenente i dati necessari alla creazione del log
   * @returns Observable<void> perché il backend non restituisce un DTO
   */
  salva(payload: LogAzioneCreazione): Observable<void> {
    return this.http.post<void>(this.baseUrl, payload);
  }

  /**
   * Recupera tutti i log presenti nel sistema.
   *
   * Questo metodo corrisponde al backend:
   *   LetturaLogAzioneAsync()
   *
   * Il backend restituisce una lista di DtoLogAzioni,
   * che in Angular viene mappata nell'interfaccia LogAzione.
   *
   * @returns Observable contenente un array di log
   */
  ottieniTutti(): Observable<LogAzione[]> {
    return this.http.get<LogAzione[]>(this.baseUrl);
  }
}
```
</details>

### Aggiornamento Codice

- Utente: Simeone
- Data: 26/05/2026
- Ho eseguito la task di crearci la pagina dedicata
<details><summary> log-azioni.service V1.1 </summary>

```ts
import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { LogAzioni, LogAzioniCreazione } from '../models/logAzioni.model'; //ho modificato il nome perché il modello non corrispondeva

@Injectable({ providedIn: 'root' })
export class LogAzioniService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiBaseUrl}/gestore/logs`; //ho cambiato l'indirizzo prendendo spunto dagli altri url che hanno funzioni esclusivi che sono solitamente preceduti da {nome ruolo}7 e la funzione, quindi essendo i log di sistema una funzione disponibile solo per gestore ho utilizzato questo url

  ottieniTutti(): Observable<LogAzioni[]> {
    return this.http.get<LogAzioni[]>(this.baseUrl);
  }
}
```
</details>

## movieService

<details><summary> movie.service V1.0 </summary>

```ts
import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { Movie, MovieCreazione } from '../models/Movie.model';

@Injectable({
  providedIn: 'root',
})
export class MovieService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiBaseUrl}/movie`;

  ottieniTutto(): Observable<Movie[]> {
    return this.http.get<Movie[]>(this.baseUrl);
  }

  ottieniTramiteId(id: string): Observable<Movie> {
    return this.http.get<Movie>(`${this.baseUrl}/${id}`);
  }

  ottieniTramiteGenere(id: string): Observable<Movie[]> {
    return this.http.get<Movie[]>(`${this.baseUrl}/genere/${id}`);
  }

  crea(payload: MovieCreazione): Observable<Movie> {
    return this.http.post<Movie>(this.baseUrl, payload);
  }

  modifica(id: string, payload: MovieCreazione): Observable<Movie> {
    return this.http.put<Movie>(`${this.baseUrl}/${id}`, payload);
  }

  elimina(id: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }
}
```
</details>

## navbar-shared-state--service.service.ts
<details>
<summary>Versione 1.0</summary>
Fabio 9-06-2026
Descrizione: creazione di questo service per far si che il saldo dell'utente si aggiorni in tempo reale quando facciamo oeprazioni tipo comprare gift card, abbonamento, proiezioni ecc...

```ts
import { Injectable, signal, computed, inject, effect } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { AuthService } from './auth.service';
import { Utente } from '../models/utente.model';
import { firstValueFrom } from 'rxjs'; // Usato SOLO per trasformare la chiamata in Promise (async/await)

@Injectable({
  providedIn: 'root'
})
export class NavbarSharedStateService {
  private http = inject(HttpClient);
  private authService = inject(AuthService);
  private readonly baseUrl = `${environment.apiBaseUrl}/Auth`;

  // Sorgente di verità finale per la Navbar
  readonly utenteLoggato = signal<Utente | null>(null);

  constructor() {
    // CANALE DI COMUNICAZIONE PERENNE (Senza RxJS)
    // Questo effect monitora il Signal "sessioneAttiva" di AuthService.
    // Gira a ogni login, logout o refresh.
    effect(async () => {
      const sessione = this.authService.utenteCorrente();

      if (sessione) {
        try {
          // Avviamo il flusso asincrono lineare (Canale Attivo)
          await this.caricaFlussoProfiloESaldo();
        } catch (err) {
          console.error("Errore nel canale asincrono della navbar:", err);
        }
      } else {
        // Se la sessione diventa null (Logout), pialliamo i dati immediatamente
        this.utenteLoggato.set(null);
      }
    });
  }

  // Sfruttiamo async/await per eliminare la complessità delle pipe RxJS
  private async caricaFlussoProfiloESaldo(): Promise<void> {
    // 1. Scarichiamo il profilo trasformando l'Observable in Promise
    const profilo = await firstValueFrom(this.http.get<Utente>(`${this.baseUrl}/profilo`));
    
    // 2. Scarichiamo il saldo relativo a questo profilo
    const resSaldo = await firstValueFrom(this.http.get<{ saldo: number }>(`${this.baseUrl}/saldo`));
    
    // 3. Uniamo i dati e aggiorniamo il Signal globale
    profilo.saldo = resSaldo.saldo;
    this.utenteLoggato.set(profilo);
  }

  // Azione continua: se l'utente compra un biglietto nel sito, 
  // chiami questo metodo e la navbar si aggiorna da sola in background
  async forzaAggiornamentoSaldo(): Promise<void> {
    if (!this.utenteLoggato()) return;

    const res = await firstValueFrom(this.http.get<{ saldo: number }>(`${this.baseUrl}/saldo`));
    
    this.utenteLoggato.update(attuale => 
      attuale ? { ...attuale, saldo: res.saldo } : null
    );
  }
}
```
</details>

### Aggiornamento Codice

<details>
<summary>Versione 1.1</summary>
Fabio 10-06-2026
Descrizione: aggiunto import { firstValueFrom } from 'rxjs';

```ts
import { Injectable, signal, inject, effect } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { AuthService } from './auth.service';
import { Utente } from '../models/utente.model';
import { firstValueFrom } from 'rxjs'; 
@Injectable({
  providedIn: 'root'
})
export class NavbarSharedStateService {
  private http = inject(HttpClient);
  private authService = inject(AuthService);
  private readonly baseUrl = `${environment.apiBaseUrl}/Auth`;

  readonly utenteLoggato = signal<Utente | null>(null);

  constructor() {
 
    effect(async () => {
      const sessione = this.authService.utenteCorrente();

      if (sessione) {
        try {
          await this.caricaFlussoProfiloESaldo();
        } catch (err) {
          console.error("Errore nel canale asincrono della navbar:", err);
        }
      } else {
        this.utenteLoggato.set(null);
      }
    });
  }

  private async caricaFlussoProfiloESaldo(): Promise<void> {
    const profilo = await firstValueFrom(this.http.get<Utente>(`${this.baseUrl}/profilo`));
    
    const resSaldo = await firstValueFrom(this.http.get<{ saldo: number }>(`${this.baseUrl}/saldo`));
    
    profilo.saldo = resSaldo.saldo;
    this.utenteLoggato.set(profilo);
  }

  async forzaAggiornamentoSaldo(): Promise<void> {
    if (!this.utenteLoggato()) return;

    const res = await firstValueFrom(this.http.get<{ saldo: number }>(`${this.baseUrl}/saldo`));
    
    this.utenteLoggato.update(attuale => 
      attuale ? { ...attuale, saldo: res.saldo } : null
    );
  }
}
```
</details>



## operatore.service.ts
<details>
<summary> operatore.service.ts V1.0 </summary>

Andrea Bruno 03-06-2026
Modifica del metodo OttieniUtenti()

```ts
import { environment } from '../../environments/environment';
import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Biglietto } from '../models/biglietto.model';
import { Utente } from '../models/utente.model';

@Injectable({
    providedIn: 'root'
})
export class OperatoreService {

    // HttpClient iniettato per fare richieste HTTP
    private readonly http = inject(HttpClient);

    // URL base dell'API per gli endpoint dell'operatore
    private readonly baseUrl = `${environment.apiBaseUrl}/operatore`;

    // Ottiene la lista completa degli utenti
    OttieniUtenti(): Observable<Utente[]> {
        return this.http.get<Utente[]>(`${this.baseUrl}/listaUtenti`);
    }

    // Ottiene un singolo utente tramite ID
    OttieniUtentePerId(id: string): Observable<Utente> {
        return this.http.get<Utente>(`${this.baseUrl}/utenti/${id}`);
    }

    // Elimina un utente tramite ID
    eliminaUtente(id: string): Observable<void> {
        return this.http.delete<void>(`${this.baseUrl}/utenti/${id}`);
    }

    // Ottiene gli utenti che possiedono un certo abbonamento
    ottieniUtentiTramiteAbbonamento(abbonamentoId: string): Observable<Utente[]> {
        return this.http.get<Utente[]>(
            `${this.baseUrl}/utenti/abbonamento/${abbonamentoId}`
        );
    }

    // Ottiene gli utenti che possiedono una certa gift card
    ottieniUtentiTramiteGiftCard(giftCardId: string): Observable<Utente[]> {
        return this.http.get<Utente[]>(
            `${this.baseUrl}/utenti/giftcard/${giftCardId}`
        );
    }

    // Ottiene tutti i biglietti
    ottieniBiglietti(): Observable<Biglietto[]> {
        return this.http.get<Biglietto[]>(`${this.baseUrl}/biglietti`);
    }

    // Ottiene un singolo biglietto tramite ID
    ottieniBigliettoPerId(id: string): Observable<Biglietto> {
        return this.http.get<Biglietto>(`${this.baseUrl}/biglietti/${id}`);
    }
}
```

## proiezione.service.ts

<details>
<summary>Versione 1.0</summary>

```ts
import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { Proiezione, ProiezioneCreazione } from '../models/proiezione.model';
/**
 * Service Angular dedicato alla gestione delle operazioni CRUD
 * relative all'entità proiezione.
 *
 * Il service comunica con il backend tramite HttpClient
 * utilizzando gli endpoint REST esposti dall'API.
 */
@Injectable({ providedIn: 'root' })
export class ProiezioneService {
  /**
   * URL base dell'endpoint REST per la gestione
   * delle proiezioni.
   */
  private readonly http = inject(HttpClient);
  /**
   * URL base dell'endpoint REST per la gestione
   *  della proiezione.
   */
  private readonly baseUrl = `${environment.apiBaseUrl}/proiezione`;

  /**
   * Recupera tutte le proiezioni visibili presenti nel sistema.
   *
   * @returns Observable contenente un array di proiezioni.
   */
  ottieniTutto(): Observable<Proiezione[]> {
    return this.http.get<Proiezione[]>(this.baseUrl);
  }

  /**
   * Recupera tutte le proiezioni presenti nel sistema anche quelle nascoste.
   *
   * @returns Observable contenente un array di proiezioni.
   */
  ottieniTuttoStorico(): Observable<Proiezione[]> {
    return this.http.get<Proiezione[]>(`${this.baseUrl}/storico`);
  }

  /**
   * Recupera una specifica proiezione tramite identificativo GUID.
   *
   * @param id Identificativo univoco della proiezione.
   * @returns Observable contenente la proiezione di risposta.
   */
  ottieniTramiteId(id: string): Observable<Proiezione> {
    return this.http.get<Proiezione>(`${this.baseUrl}/${id}`);
  }

  /**
   * Recupera una specifica proiezione tramite identificativo del movie(film) collegato GUID.
   *
   * @param id Identificativo univoco del movie.
   * @returns Observable contenente la proiezione di risposta.
   */
  ottieniTramiteMovieId(id: string): Observable<Proiezione> {
    return this.http.get<Proiezione>(`${this.baseUrl}/movie/${id}`);
  }

  /**
   * Recupera una specifica proiezione tramite identificativo della sala collegata GUID.
   *
   * @param id Identificativo univoco della sala.
   * @returns Observable contenente la proiezione di risposta.
   */
  ottieniTramiteSalaId(id: string): Observable<Proiezione> {
    return this.http.get<Proiezione>(`${this.baseUrl}/sala/${id}`);
  }

  /**
   * Recupera una specifica proiezione tramite identificativo del turno collegato GUID.
   *
   * @param id Identificativo univoco del turno.
   * @returns Observable contenente la proiezione di risposta.
   */
  ottieniTramiteTurnoId(id: string): Observable<Proiezione> {
    return this.http.get<Proiezione>(`${this.baseUrl}/turno/${id}`);
  }

  /**
   * crea una nuova proiezione utilizzando un payload.
   *
   * @param payload modello di richiesta per creazione della proiezione.
   * @returns Observable contenente la proiezione di risposta.
   */
  crea(payload: ProiezioneCreazione): Observable<Proiezione> {
    return this.http.post<Proiezione>(this.baseUrl, payload);
  }

  /**
   * Modifica una specifica proiezione tramite identificativo GUID con le informazioni all'interno di un payload.
   *
   * @param id Identificativo univoco della proiezione.
   * @param payload modello di richiesta per la modifica della proiezione.
   * @returns Observable contenente la proiezione di risposta.
   */
  modifica(id: string, payload: ProiezioneCreazione): Observable<Proiezione> {
    return this.http.put<Proiezione>(`${this.baseUrl}/${id}`, payload);
  }

  /**
   * Elimina una specifica proiezione tramite identificativo GUID.
   *
   * @param id Identificativo univoco della proiezione.
   */
  elimina(id: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }
}
```
</details>

### Aggiornamento Codice

Utente: Fabio Tammaro
Data: 03/06/2026
Descrizione: modificata la eliminazione in "put".
<details>
<summary>Versione 1.1</summary>

```ts
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

  ottieniTramiteMovieId(id: string): Observable<Proiezione> {
    return this.http.get<Proiezione>(`${this.baseUrl}/movie/${id}`);
  }

  
  ottieniTramiteSalaId(id: string): Observable<Proiezione> {
    return this.http.get<Proiezione>(`${this.baseUrl}/sala/${id}`);
  }

  ottieniTramiteTurnoId(id: string): Observable<Proiezione> {
    return this.http.get<Proiezione>(`${this.baseUrl}/turno/${id}`);
  }

 
  crea(payload: ProiezioneCreazione): Observable<Proiezione> {
    return this.http.post<Proiezione>(this.baseUrl, payload);
  }


  modifica(id: string, payload: ProiezioneCreazione): Observable<Proiezione> {
    return this.http.put<Proiezione>(`${this.baseUrl}/${id}`, payload);
  }

// è una put perchè non può eliminare la proiezione direttamente.
  elimina(id: string) {
        return this.http.put<{ messaggio: string }>(`${this.baseUrl}/elimina/${id}`,
            {}
        );
}
```
</details>

## sala.service.ts
<details>
<summary> sala.service.ts V1.0 </summary>

```ts

import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http'; // importo dell'http Client
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment'; //importo dell'enviroment
import { Sala, SalaCreazione  } from '../models/sala.model';//importo dei modelli Sala


@Injectable({ providedIn: 'root' })
export class SalaService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiBaseUrl}/sala`; /*utilizza l'enviroment per creare
                                                                 un url di comunicazione verso il
                                                                  controller del backend*/

  ottieniTutto(): Observable<Sala[]> {
    //chiamta http per ottenere tutte le sale
    return this.http.get<Sala[]>(this.baseUrl);
  }
  ottieniPerTipologia(tipologiaId: string): Observable<Sala[]> {
    // chiamata http per ottenre tutte le sale relative ad una tipologia
    return this.http.get<Sala[]>(`${this.baseUrl}/tipologia/${tipologiaId}`);
    //per creare la chiamata http, bisogna combinare il baseUrl con la firma http del metodo del controller
  }

  ottieniTramiteId(id: string): Observable<Sala> {
    // chiamamta http per ottenere una sala tramite il suo id
    return this.http.get<Sala>(`${this.baseUrl}/${id}`);
  }

  crea(payload: SalaCreazione): Observable<Sala> {
    // chiamata http per creare una nuova sala
    return this.http.post<Sala>(this.baseUrl, payload);
  }

  modifica(id: string, payload: SalaCreazione): Observable<Sala> {
    // chiamata http per la modifica di una sala
    return this.http.put<Sala>(`${this.baseUrl}/${id}`, payload);
  }

  elimina(id: string): Observable<void> {
    // chiamata http per l'eliminazione di una sala
```

</details>

### Aggiornamento Codice

<details>

<summary>sala.service.ts V1.1</summary>

Alessandro Gregorio

26/05/2026

Cambiato endpoint per eliminare una sala

```ts

import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { Sala, SalaCreazione } from '../models/sala.model';

// @Injectable({ providedIn: 'root' }) registra il servizio nel dependency injector
// globale dell'applicazione: esiste una sola istanza condivisa da tutti i componenti
// che lo iniettano (pattern Singleton)
@Injectable({ providedIn: 'root' })
export class SalaService {

  // HttpClient: servizio Angular per effettuare chiamate HTTP
  private readonly http = inject(HttpClient);

  // URL base per tutti gli endpoint di questo servizio,
  // costruito concatenando l'indirizzo API definito nell'environment
  // (cambia automaticamente tra sviluppo e produzione) con il path "/sala"
  private readonly baseUrl = `${environment.apiBaseUrl}/sala`;

  // ===========================
  // GET /sala
  // Recupera la lista completa di tutte le sale
  // ===========================
  ottieniTutto(): Observable<Sala[]> {
    return this.http.get<Sala[]>(this.baseUrl);
  }

  // ===========================
  // GET /sala/tipologia/:tipologiaId
  // Recupera solo le sale che appartengono a una specifica tipologia
  // utile per filtrare la lista in base al tipo di sala
  // ===========================
  ottieniPerTipologia(tipologiaId: string): Observable<Sala[]> {
    return this.http.get<Sala[]>(`${this.baseUrl}/tipologia/${tipologiaId}`);
  }

  // ===========================
  // GET /sala/:id
  // Recupera una singola sala tramite il suo ID univoco
  // ===========================
  ottieniTramiteId(id: string): Observable<Sala> {
    return this.http.get<Sala>(`${this.baseUrl}/${id}`);
  }

  // ===========================
  // POST /sala
  // Crea una nuova sala inviando i dati nel body della richiesta
  // responseType: 'text' indica che il backend risponde con una stringa
  // (es. l'ID della risorsa appena creata) invece di un oggetto JSON
  // ===========================
  crea(payload: SalaCreazione): Observable<string> {
    return this.http.post(`${this.baseUrl}`, payload, { responseType: 'text' });
  }

  // ===========================
  // PUT /sala/:id
  // Aggiorna una sala esistente identificata dall'ID
  // Il payload contiene i nuovi valori da sovrascrivere
  // responseType: 'text' come per crea(), il backend risponde con una stringa
  // ===========================
  modifica(id: string, payload: SalaCreazione): Observable<string> {
    return this.http.put(`${this.baseUrl}/${id}`, payload, { responseType: 'text' });
  }

  // ===========================
  // PUT /sala/elimina/:id
  // Esegue un soft delete della sala: usa PUT invece di DELETE
  // perché il backend marca il record come eliminato senza rimuoverlo fisicamente dal DB
  // Il body è null perché non servono dati aggiuntivi oltre all'ID nell'URL
  // ===========================
  elimina(id: string): Observable<void> {
    return this.http.put<void>(`${this.baseUrl}/elimina/${id}`, null);
  }
}
```

</details>



###TipologiaSalaService.ts V1.0

<details><summary> tipologia.sala.service.ts V1.0 </summary>

```ts
import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { TipologiaSala, TipologiaSalaCreazione } from '../models/TipologiaSala.model';

/**
 * Service Angular dedicato alla gestione delle operazioni CRUD
 * relative all'entità TipologiaSala.
 *
 * Il service comunica con il backend tramite HttpClient
 * utilizzando gli endpoint REST esposti dall'API.
 */
@Injectable({
  providedIn: 'root',
})
export class TipologiaSalaService {
  /**
   * Istanza del client HTTP utilizzata per effettuare
   * richieste verso il backend.
   */
  private readonly http = inject(HttpClient);

  /**
   * URL base dell'endpoint REST per la gestione
   * delle tipologie di sala.
   */
  private readonly baseUrl = `${environment.apiBaseUrl}/tipologiaSala`;

  /**
   * Recupera tutte le tipologie di sala presenti nel sistema.
   *
   * @returns Observable contenente un array di TipologiaSala.
   */
  ottieniTutto(): Observable<TipologiaSala[]> {
    return this.http.get<TipologiaSala[]>(this.baseUrl);
  }

  /**
   * Recupera una specifica tipologia di sala tramite identificativo GUID.
   *
   * @param id Identificativo univoco della tipologia di sala.
   * @returns Observable contenente la TipologiaSala richiesta.
   */
  ottieniTramiteId(id: string): Observable<TipologiaSala> {
    return this.http.get<TipologiaSala>(`${this.baseUrl}/${id}`);
  }

  /**
   * Crea una nuova tipologia di sala nel sistema.
   *
   * @param payload Oggetto contenente i dati necessari alla creazione.
   * @returns Observable contenente la TipologiaSala creata.
   */
  crea(payload: TipologiaSalaCreazione): Observable<TipologiaSala> {
    return this.http.post<TipologiaSala>(this.baseUrl, payload);
  }

  /**
   * Modifica una tipologia di sala esistente.
   *
   * @param id Identificativo GUID della tipologia da modificare.
   * @param payload Nuovi dati della tipologia di sala.
   * @returns Observable contenente la TipologiaSala aggiornata.
   */
  modifica(id: string, payload: TipologiaSalaCreazione): Observable<TipologiaSala> {
    return this.http.put<TipologiaSala>(`${this.baseUrl}/${id}`, payload);
  }

  /**
   * Elimina una tipologia di sala tramite identificativo GUID.
   *
   * @param id Identificativo univoco della tipologia di sala.
   * @returns Observable<void> restituito al completamento dell'operazione.
   */
  elimina(id: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }
}

```
</details>

## tipologia-sala.service.ts

<details> 

  Utente: Alessandro Gregorio

  Data: 04/06/2026

  Dettagli: DELETE -> PUT. La delete è diventata una put per poter modificare il parametro isDeleted per far considerare la tipologia sala eliminata ma senza effettivamente eliminarla ed eliminare le sue correlazioni

<summary> tipologia-sala.service.ts V1.0 </summary>

```ts

import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { TipologiaSala, TipologiaSalaCreazione } from '../models/tipologia-sala.model';
import pa from '@angular/common/locales/pa';
/**
 * Service Angular dedicato alla gestione delle operazioni CRUD
 * relative all'entità TipologiaSala.
 *
 * Il service comunica con il backend tramite HttpClient
 * utilizzando gli endpoint REST esposti dall'API.
 */

@Injectable({
  providedIn: 'root',
})
export class TipologiaSalaService {
  /**
   * Istanza del client HTTP utilizzata per effettuare
   * richieste verso il backend.
   */
  private readonly http = inject(HttpClient);
    /**
   * URL base dell'endpoint REST per la gestione
   * delle tipologie di sala.
   */
  private readonly baseUrl = `${environment.apiBaseUrl}/tipologiaSala`;

 /**
   * Recupera tutte le tipologie di sala presenti nel sistema.
   *
   * @returns Observable contenente un array di TipologiaSala.
   */
  ottieniTutto(): Observable<TipologiaSala[]>
  {
    return this.http.get<TipologiaSala[]>(this.baseUrl);
  }
    /**
   * Recupera una specifica tipologia di sala tramite identificativo GUID.
   *
   * @param id Identificativo univoco della tipologia di sala.
   * @returns Observable contenente la TipologiaSala richiesta.
   */
  ottieniTramiteId(id:string): Observable<TipologiaSala>
  {
    return this.http.get<TipologiaSala>(`${this.baseUrl}/${id}`);
  }

 /**
   * Crea una nuova tipologia di sala nel sistema.
   *
   * @param payload Oggetto contenente i dati necessari alla creazione.
   * @returns Observable contenente la TipologiaSala creata.
   */
  crea(payload:TipologiaSalaCreazione): Observable<TipologiaSala>
  {
    return this.http.post<TipologiaSala>(this.baseUrl, payload);
  }
    /**
   * Modifica una tipologia di sala esistente.
   *
   * @param id Identificativo GUID della tipologia da modificare.
   * @param payload Nuovi dati della tipologia di sala.
   * @returns Observable contenente la TipologiaSala aggiornata.
   */

  modifica(id:string, payload: TipologiaSalaCreazione): Observable<TipologiaSala>
  {
    return this.http.put<TipologiaSala>(`${this.baseUrl}/${id}`, payload);
  }

  /**
   * Elimina una tipologia di sala tramite identificativo GUID.
   * Non lo elimina effettivamente dal database ma commuta il parametro isDeleted
   *
   * @param id Identificativo univoco della tipologia di sala.
   * @returns Observable<void> restituito al completamento dell'operazione.
   */
  elimina(id:string, payload: TipologiaSalaCreazione): Observable<TipologiaSala>
  {
    return this.http.put<TipologiaSala>(`${this.baseUrl}/elimina/${id}`, payload);
  }
}

```

</details>


## turno.service.ts
<details><summary> turno.service V1.0 </summary>

```ts
import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { Turno, TurnoCreazione } from '../models/turno.model';

/**
 * Service Angular dedicato alla gestione delle operazioni CRUD
 * relative all'entità Turno.
 *
 * Il service comunica con il backend tramite HttpClient
 * utilizzando gli endpoint REST esposti dall'API.
 */
@Injectable({ providedIn: 'root' })
export class TurnoService {
  /**
   * Istanza del client HTTP utilizzata per effettuare
   * richieste verso il backend.
   */
  private readonly http = inject(HttpClient);
  /**
   * URL base dell'endpoint REST per la gestione
   *  di turno.
   */
  private readonly baseUrl = `${environment.apiBaseUrl}/turno`;

  /**
   * Recupera tutti i turni presenti nel sistema.
   *
   * @returns Observable contenente un array di Turni.
   */
  ottieniTutto(): Observable<Turno[]> {
    return this.http.get<Turno[]>(this.baseUrl);
  }
  /**
   * Recupera uno specifico turno tramite identificativo GUID.
   *
   * @param id Identificativo univoco del turno.
   * @returns Observable contenente il turno di risposta.
   */
  ottieniTramiteId(id: string): Observable<Turno> {
    return this.http.get<Turno>(`${this.baseUrl}/${id}`);
  }
  /**
   * crea un nuovo turno utilizzando un payload.
   *
   * @param payload modello di richiesta per creazione del turno.
   * @returns Observable contenente il turno di risposta.
   */
  crea(payload: TurnoCreazione): Observable<Turno> {
    return this.http.post<Turno>(this.baseUrl, payload);
  }
  /**
   * Modifica uno specifico turno tramite identificativo GUID con le informazioni all'interno di un payload.
   *
   * @param id Identificativo univoco del turno.
   * @param payload modello di richiesta per la modifica del turno.
   * @returns Observable contenente il turno di risposta.
   */
  modifica(id: string, payload: TurnoCreazione): Observable<Turno> {
    return this.http.put<Turno>(`${this.baseUrl}/${id}`, payload);
  }

  /**
   * Elimina uno specifico turno tramite identificativo GUID.
   *
   * @param id Identificativo univoco del turno.
   */
  elimina(id: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }
}
```
</details>

## utente.service.ts

<details>
<summary>Versione 1.0</summary>
Andrea Bruno 28-05-2026
Cambio di rotta da Utente a Auth 
Modifiche nel richiamo dei servizi 

```c#
import { environment } from '../../environments/environment';
import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Utente, UtenteCreazione } from '../models/utente.model';

@Injectable({
    providedIn: 'root'
})
export class UtenteService {

    // HttpClient viene iniettato tramite inject() (metodo moderno Angular)
    private readonly http = inject(HttpClient);

    // Base URL dell'API: punta al controller Auth del backend
    // /Auth è corretto perché il controller è AuthController
    private readonly baseUrl = `${environment.apiBaseUrl}/Auth`;

    // Recupera il profilo dell'utente loggato
    // Ritorna un Observable<Utente> che il componente sottoscrive
    profilo(): Observable<Utente> {
        return this.http.get<Utente>(`${this.baseUrl}/profilo`);
    }

    // Modifica i dati dell'utente (nome + età)
    // Usa PUT perché aggiorna risorse esistenti
    modifica(payload: UtenteCreazione): Observable<any> {
        return this.http.put<any>(`${this.baseUrl}/modifica`, payload);
    }

    // Elimina definitivamente l'account dell'utente
    // Il backend gestisce la rimozione e la logout viene fatta nel componente
    eliminaProfilo(): Observable<any> {
        return this.http.delete<any>(`${this.baseUrl}/elimina`);
    }
}
```
</details>
<details>
<summary>Versione 1.1</summary>
Francesco lorenzi 05-06-2026
aggiunta il richiamo della funzione per abbonarsi 

```c#
import { environment } from '../../environments/environment';
import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Utente, UtenteCreazione } from '../models/utente.model';

@Injectable({
    providedIn: 'root'
})
export class UtenteService {

    // HttpClient viene iniettato tramite inject() (metodo moderno Angular)
    private readonly http = inject(HttpClient);

    // Base URL dell'API: punta al controller Auth del backend
    // /Auth è corretto perché il controller è AuthController
    private readonly baseUrl = `${environment.apiBaseUrl}/Auth`;

    // Recupera il profilo dell'utente loggato
    // Ritorna un Observable<Utente> che il componente sottoscrive
    profilo(): Observable<Utente> {
        return this.http.get<Utente>(`${this.baseUrl}/profilo`);
    }

    // Modifica i dati dell'utente (nome + età)
    // Usa PUT perché aggiorna risorse esistenti
    modifica(payload: UtenteCreazione): Observable<any> {
        return this.http.put<any>(`${this.baseUrl}/modifica`, payload);
    }

    // Elimina definitivamente l'account dell'utente
    // Il backend gestisce la rimozione e la logout viene fatta nel componente
    eliminaProfilo(): Observable<any> {
        return this.http.delete<any>(`${this.baseUrl}/elimina`);
    }
    //Abbona l'utente della sessione con un determinato abbonamento
    abbonati( abbonamentoId: string): Observable<any> {
        return this.http.post<any>(`${environment.apiBaseUrl}/Utente/abbonati`, { IdAbbonamento: abbonamentoId });
    }
}
```
</details>
