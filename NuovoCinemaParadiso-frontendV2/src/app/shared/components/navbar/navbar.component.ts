import { Component, computed, inject } from '@angular/core';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { AuthService } from './../../../services/auth.service';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [RouterLink, RouterLinkActive],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.css'
})
export class NavbarComponent {
  private readonly authService = inject(AuthService);

  readonly utente        = this.authService.utenteCorrente;
  readonly isGestore     = computed(() => this.authService.ruoloCorrispondente('Gestore'));
  readonly isOperatore     = computed(() => this.authService.ruoloCorrispondente('Operatore'));
  readonly isAutenticato = computed(() => this.authService.isAutenticato ());

  logout(): void {
    this.authService.logout();
  }
}