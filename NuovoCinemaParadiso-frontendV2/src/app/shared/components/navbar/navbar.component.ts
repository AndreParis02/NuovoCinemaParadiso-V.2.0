import { Component, computed, inject } from '@angular/core';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { AuthService } from './../../../services/auth.service';
import { UtenteService } from '../../../services/utente.service';

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

  readonly utenteCorrente = this.utenteService.profilo;

  readonly isAutenticato = computed(() => this.authService.isAutenticato());
  readonly isGestore = computed(() => this.authService.isGestore());
  readonly isOperatore = computed(() => this.authService.isOperatore());


  
  logout(): void {
    this.authService.logout();
  }
}