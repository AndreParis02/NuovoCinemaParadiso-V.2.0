import { Component, inject } from '@angular/core';
import { RouterLink } from '@angular/router';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-dashboard-page',
  standalone: true,
  imports: [RouterLink],
  templateUrl: './dashboard.page.html'
})
export class DashboardPage {
  private readonly authService = inject(AuthService);

  readonly user = this.authService.utenteCorrente;

  puoEditare(): boolean {
    return this.authService.possiedeQualsiasiRuolo(['Operatore']);
  }

    isOperatore(): boolean {
    return this.authService.ruoloCorrispondente('Operatore');
  }
}

