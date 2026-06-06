
## Navbar

### navbar.component.ts

Utente: Fabio Tammaro
Data: 06/06/2026
Descrizione: modificata la navbar in base alla decisioni prese per l'interfaccia della web app. Aggiunto l'import di effect per avere la lettura del cambiamento di signal<'Utente'>


<details><summary> navbar.component.ts V1.0 </summary>

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

<details><summary> navbar.component.html V1.0</summary>

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