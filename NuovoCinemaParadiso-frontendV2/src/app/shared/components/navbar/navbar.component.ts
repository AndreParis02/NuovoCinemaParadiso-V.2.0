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
        effect(() => {
            if (this.isAutenticato()) {
                this.caricaUtente();
            } else {
                this.utenteCorrente.set(null);
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