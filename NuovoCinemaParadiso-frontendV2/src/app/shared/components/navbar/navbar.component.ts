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