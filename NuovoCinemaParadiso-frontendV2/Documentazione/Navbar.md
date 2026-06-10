# shared/navbar
[priorità] (Fabio: modificare aggiungendo sale per l'operatore, rimuovendo profilo per tutti, crediti) 

### navbar.component.ts

<details>
Utente: Fabio Tammaro
Data: 06/06/2026
Descrizione: modificata la navbar in base alla decisioni prese per l'interfaccia della web app. Aggiunto l'import di effect per avere la lettura del cambiamento di signal<'Utente'>

<summary> V1.0 </summary>

```ts
import { Component, computed, inject, signal, effect } from '@angular/core';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { AuthService } from './../../../services/auth.service';
import { UtenteService } from '../../../services/utente.service';
import { Utente } from '../../../models/utente.model';

@Component({
    selector: 'app-navbar',
    standalone: true,
    imports: [RouterLink, RouterLinkActive],
    templateUrl: './navbar.component.html',
    styleUrl: './navbar.component.css'
})
export class NavbarComponent {

    private readonly authService = inject(AuthService);
    private readonly utenteService = inject(UtenteService);

    readonly utenteCorrente = signal<Utente | null>(null);
    readonly utenteInSessione = computed(() => this.authService.utenteCorrente());

    readonly isAutenticato = computed(() => this.authService.isAutenticato());
    readonly isGestore = computed(() => this.authService.isGestore());
    readonly isOperatore = computed(() => this.authService.isOperatore());

    constructor() {
        // L'effect reagisce AUTOMATICAMENTE ogni volta che 'isAutenticato' cambia valore
        effect(() => {
            if (this.isAutenticato()) {
                this.caricaUtente(); // Se è loggato, carica i dati aggiornati
            } else {
                this.utenteCorrente.set(null); // Se fa logout, azzera i dati
            }
        });
    }

    caricaUtente(): void {
        this.utenteService.profilo().subscribe({
            next: (item) => {
                this.utenteCorrente.set(item);
            }
        });
    }

    logout(): void {
        this.authService.logout();
    }
}

```
</details>

### Aggiornamento Codice

<details>
Utente: Fabio Tammaro
Data: 08/06/2026

<summary> V1.1 </summary>

```ts
import { Component, computed, inject } from '@angular/core';
import { RouterLink } from '@angular/router';
import { AuthService } from './../../../services/auth.service';
import { NavbarSharedStateService } from '../../../services/navbar-shared-state--service.service';

@Component({
    selector: 'app-navbar',
    standalone: true,
    imports: [RouterLink],
    templateUrl: './navbar.component.html',
    styleUrl: './navbar.component.css'
})
export class NavbarComponent {
    private readonly authService = inject(AuthService);
    private readonly navbarSharedStateService = inject(NavbarSharedStateService);

    // Mappiamo i segnali dell'AuthService direttamente per l'HTML
    readonly isAutenticato = this.authService.isAutenticato;
    readonly isGestore = this.authService.isGestore;
    readonly isOperatore = this.authService.isOperatore;
    
    // Leggiamo passivamente l'utente e calcoliamo il saldo in tempo reale
    readonly utenteCorrente = this.navbarSharedStateService.utenteLoggato;
    readonly utenteInSessione = this.authService.utenteCorrente;
    readonly saldoCondiviso = computed(() => this.utenteCorrente()?.saldo ?? 0);

    logout(): void {
        this.authService.logout(); 
        // Non serve chiamare pulisciUtente()! 
        // L'effect nel servizio noterà che sessioneAttiva è diventata null e pulirà tutto da solo.
    }
}
```

</details>

### Aggiornamento Codice

<details>
Utente: Fabio Tammaro
Data: 10/06/2026

<summary> V1.2 </summary>

```ts
import { Component, computed, inject } from '@angular/core';
import { RouterLink } from '@angular/router';
import { AuthService } from './../../../services/auth.service';
import { NavbarSharedStateService } from '../../../services/navbar-shared-state--service.service';

@Component({
    selector: 'app-navbar',
    standalone: true,
    imports: [RouterLink],
    templateUrl: './navbar.component.html',
    styleUrl: './navbar.component.css'
})
export class NavbarComponent {
    private readonly authService = inject(AuthService);
    private readonly navbarSharedStateService = inject(NavbarSharedStateService);

    readonly isAutenticato = computed(()=> this.authService.isAutenticato());
    readonly isGestore = computed(()=> this.authService.isGestore());
    readonly isOperatore = computed(()=> this.authService.isOperatore());
    readonly isUtente = computed(()=> this.authService.isUtente());
    
    readonly utenteCorrente = this.navbarSharedStateService.utenteLoggato;
    readonly utenteInSessione = this.authService.utenteCorrente;
    readonly saldoCondiviso = computed(() => this.utenteCorrente()?.saldo ?? 0);

    logout(): void {
        this.authService.logout(); 
      
    }
}
```

### navbar.component.html 
<details>
Utente: Fabio Tammaro
Data: 06/06/2026

<summary> V1.0 </summary>

```html
<header class="navbar-shell">
    <div class="container navbar">
        <nav class="links">
            <a routerLink="/dashboard" class="brand">Nuovo cinema Paradiso</a>
            <div class="btn-row">
                <a class="btn btn-secondary" routerLink="/proiezioni">Proiezioni</a>
            </div>
            <div class="btn-row">
                <a class="btn btn-secondary" routerLink="/abbonamenti">Abbonamenti</a>
            </div>
            @if(isOperatore()){
            <div class="btn-row">
                <a class="btn btn-secondary" routerLink="/movies">Film</a>
            </div>
            <div class="btn-row">
                <a class="btn btn-secondary" routerLink="/sale">Sale</a>
            </div>
            }
            @if(!isAutenticato()){
            <div class="btn-row">
                <a class="btn btn-secondary" routerLink="/register">Registrati</a>
            </div>
            }

            @if(!isGestore() && !isOperatore() && isAutenticato()){
            <div>
                <strong>{{utenteCorrente()?.nomeCompleto}}</strong>
                <div class="muted small">Saldo: {{utenteCorrente()?.saldo}}</div>
                <div class="muted small">{{utenteCorrente()?.email}}</div>
            </div>
            }

            @if(isGestore() || isOperatore()){
            <div>
                <strong>{{utenteCorrente()?.nomeCompleto}}</strong>
                <div class="muted small">{{utenteInSessione()?.ruolo}}</div>
            </div>
            }

            @if(isAutenticato()){
            <div class="user-box">
                <button class="btn btn-secondary" type="button" (click)="logout()">Logout</button>
            </div>
            }
        </nav>
    </div>
</header>

```
</details>

### Aggiornamento Codice

<details>
Utente: Fabio Tammaro
Data: 08/06/2026

<summary> V1.1 </summary>

```html
<header class="navbar-shell">
    <div class="container navbar">
        <nav class="links">
            <a routerLink="/dashboard" class="brand">Nuovo cinema Paradiso</a>
            <div class="btn-row">
                <a class="btn btn-secondary" routerLink="/proiezioni">Proiezioni</a>
            </div>
            <div class="btn-row">
                <a class="btn btn-secondary" routerLink="/abbonamenti">Abbonamenti</a>
            </div>
            @if(isOperatore()){
            <div class="btn-row">
                <a class="btn btn-secondary" routerLink="/movies">Film</a>
            </div>
            <div class="btn-row">
                <a class="btn btn-secondary" routerLink="/sale">Sale</a>
            </div>
            }
            @if(!isAutenticato()){
            <div class="btn-row">
                <a class="btn btn-secondary" routerLink="/register">Registrati</a>
            </div>
            }

            @if(!isGestore() && !isOperatore() && isAutenticato()){
            <div>
                <strong>{{utenteCorrente()?.nomeCompleto}} </strong>
                <span class="badge">{{utenteCorrente()?.saldo }}</span>
            </div>
            }

            @if(isGestore() || isOperatore()){
            <div>
                <strong>{{utenteCorrente()?.nomeCompleto}}</strong>
                <div class="muted small">{{utenteInSessione()?.ruolo}}</div>
            </div>
            }

            @if(isAutenticato()){
            <div class="user-box">
                <button class="btn btn-secondary" type="button" (click)="logout()">Logout</button>
            </div>
            }
        </nav>
    </div>
</header>
```

### Aggiornamento Codice

<details>
Utente: Fabio Tammaro
Data: 10/06/2026

<summary> V1.2 </summary>

```html
<header class="navbar-shell">
    <div class="container navbar">
        <nav class="links">
            <a routerLink="/dashboard" class="brand">Nuovo cinema Paradiso</a>
            <div class="btn-row">
                <a class="btn btn-secondary" routerLink="/proiezioni">Proiezioni</a>
            </div>
            <div class="btn-row">
                <a class="btn btn-secondary" routerLink="/abbonamenti">Abbonamenti</a>
            </div>
            @if(isOperatore()){
            <div class="btn-row">
                <a class="btn btn-secondary" routerLink="/movies">Film</a>
            </div>
            <div class="btn-row">
                <a class="btn btn-secondary" routerLink="/sale">Sale</a>
            </div>
            }

            @if(!isAutenticato()){
            <div class="btn-row">
                <a class="btn btn-secondary" routerLink="/register">Registrati</a>
            </div>
            }
            @if(isUtente()){
            <div>
                <strong>{{utenteCorrente()?.nomeCompleto}} </strong>
                <span class="badge">{{utenteCorrente()?.saldo }}</span>
            </div>
            }
            @else if(isGestore() || isOperatore()){
            <div>
                <strong>{{utenteCorrente()?.nomeCompleto}}</strong>
                <div class="muted small">{{utenteInSessione()?.ruolo}}</div>
            </div>
            }
            @if(isAutenticato()){
            <div class="user-box">
                <button class="btn btn-secondary" type="button" (click)="logout()">Logout</button>
            </div>
            }
        </nav>
    </div>
</header>
```

</details>

### navbar.component.css

<details>
<summary>Versione 1.0</summary>

```css
/* Contenitore principale della navbar */
.navbar-shell {
  position: sticky;
  top: 0;
  z-index: 10;
  background: rgba(15, 23, 42, 0.92);
  backdrop-filter: blur(10px);
  border-bottom: 1px solid var(--border);
}

.navbar {
  min-height: 72px;
  display: flex;
  align-items: center;
  justify-content: space-between;
  gap: 1rem;
}

.brand {
  font-size: 1.1rem;
  font-weight: 800;
  color: var(--text);
}

.links {
  display: flex;
  flex-wrap: wrap;
  gap: 1rem;
}

.links a {
  color: var(--muted);
  font-weight: 600;
}

.links a.active,
.links a:hover {
  color: var(--primary);
}

.user-box {
  display: flex;
  align-items: center;
  gap: 1rem;
}

.small {
  font-size: 0.875rem;
  color: var(--muted);
}
/* Responsività tablet/mobile*/
@media (max-width: 900px) {
  .navbar {
    flex-direction: column;
    align-items: flex-start;
    padding: 0.75rem 0;
  }

  .user-box {
    width: 100%;
    justify-content: space-between;
  }
}
```
</details>