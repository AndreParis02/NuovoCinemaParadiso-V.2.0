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

  readonly user = this.authService.utenteCorrente;
  readonly isAdmin = computed(() => this.authService.hasRole('Admin'));
  readonly isAuthenticated = computed(() => this.authService.seAutenticato ());

  logout(): void {
    this.authService.logout();
  }
}