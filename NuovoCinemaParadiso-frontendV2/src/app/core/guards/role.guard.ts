import { inject } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { RuoliUtente } from '../../ruoliUtente';

export const roleGuard: CanActivateFn = (route: ActivatedRouteSnapshot) => {
  const authService = inject(AuthService);
  const router = inject(Router);
  const ruoliAutorizzati = (route.data['role'] as RuoliUtente[] | undefined) ?? [];

  if (ruoliAutorizzati.length === 0 || authService.possiedeQualsiasiRuolo(ruoliAutorizzati)) {
      return true;
  }
  return router.createUrlTree(['/nonAutorizzato']);

};
