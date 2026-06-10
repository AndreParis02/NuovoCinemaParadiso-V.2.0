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