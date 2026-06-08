import { Routes } from '@angular/router';
import { authGuard } from './core/guards/auth.guard';
import { guestGuard } from './core/guards/guest.guard';
import { roleGuard } from './core/guards/role.guard';
import { GestionePage } from './pages/gestione/gestione.page';
import { BigliettoListComponent } from './features/biglietto/biglietto-list.component';
import { UtenteListComponent } from './features/cambio-ruolo/component/utente-list.component';

export const routes: Routes = [
  {
    path: '',
    pathMatch: 'full',
    redirectTo: 'dashboard',
  },
  /*
  {
    path: 'login',
    canActivate: [guestGuard],
    loadComponent: () => import('./pages/login/login.page').then((m) => m.LoginPage),
  },
  */
 
  {
    path: 'login',
    canActivate: [guestGuard],
    loadComponent: () => import('./features/auth/component/login.component').then((m) => m.LoginComponent),
  },
  
  {
    path: 'register',
    canActivate: [guestGuard],
    loadComponent: () => import('./features/auth/component/register.component').then((m) => m.RegisterComponent),
  },
  {
    path: 'giftcard-list',
    canActivate: [authGuard, roleGuard], 
    loadComponent: () => import('./features/giftcard/components/giftcard-list.component').then(m => m.GiftCardListComponent)
  },
  {
    path: 'gestione/giftcard/modifica/:id',
    canActivate: [authGuard, roleGuard], 
    loadComponent: () => import('./features/giftcard/components/giftcard-form.component').then(m => m.GiftCardFormComponent)
  },
  
  {
    path: 'giftcard-crea-codice',
    canActivate: [authGuard, roleGuard], 
    loadComponent: () => import('./features/giftcard/components/crea-codice.component').then(m => m.CreaCodiceComponent)
  },
  {
    path: 'log-list',
    canActivate: [authGuard, roleGuard], 
    //data: { roles: ['Gestore'] }, <- non più necessario dopo il isGestore del log-list.component.ts
    loadComponent: () => import('./features/log/components/log-list.component').then(m => m.LogListComponent)
  },
  {
    path: 'profilo',
    canActivate: [authGuard],
    loadComponent: () => import('./pages/utente/utente.page').then((m) => m.UtentePage),
  },
  {
    path: 'dashboard',
    canActivate: [authGuard],
    data: {
      roles: ['Operatore'],
    },
    loadComponent: () => import('./features/dashboard/layout/dashboard.layout').then((m) => m.DashboardLayoutComponent),
  },

  // Aggiunto per test del componente biglietto-list
  {
    path: 'lista-biglietti',
    component: BigliettoListComponent,
    canActivate: [authGuard],
  },
  {
    path: 'lista-utenti',
    component: UtenteListComponent,
    canActivate: [authGuard],
  },
  {
    path: 'gestione',
    component: GestionePage, // <-- Usiamo component invece di loadComponent
    canActivate: [roleGuard], // <-- Solo roleGuard
    data: { roles: ['Gestore'] }, // <-- Ruolo richiesto
  },
  {
    path: 'abbonamento',
    canActivate: [authGuard],
    loadComponent: () => import('./features/abbonamento/components/abbonamento-list.component').then((m) => m.AbbonamentoListComponent)
  },

  {
    path: 'operatore/cambio-ruolo',
    canActivate: [authGuard, roleGuard],
    data: { roles: ['operatore'] },
    loadComponent: () => import('./pages/operatore/operatore.page').then((m) => m.OperatorePage)
  },
  {
    path: 'listaUtenti',
    canActivate: [authGuard],
    loadComponent: () =>
      import('./pages/operatore/operatore-lista-utenti.page').then(
        (m) => m.OperatoreListaUtentiPage,
      ),
  },
  {
    path: 'proiezioni',
    loadComponent: () => import('./features/proiezione/components/proiezione-list.component').then((m) => m.ProiezioneList)
  },
  {
    path: 'giftcard',
    canActivate: [authGuard],
    loadComponent: () => import('./pages/giftcard/giftcard.page').then((m) => m.GiftcardPage)
  },
  {
    path: 'genere-movie',
    canActivate: [authGuard],
    loadComponent: () => import('./features/genere-movie/components/genere-movie-list.component').then((m) => m.GenereMovieListComponent)
  },
  {
    path: 'tipologia-sala',
    canActivate: [authGuard],
    loadComponent: () => import('./features/tipologia-sala/components/tipologia-sala-list.component').then((m) => m.TipologiaSalaListComponent)
  },
  {
    path: 'movies',
    canActivate: [authGuard],
    loadComponent: () => import('./features/movie/components/movie-list.component').then((m) => m.MovieListComponent)
  },
  {
    path: 'sale',
    canActivate: [authGuard],
    loadComponent: () => import('./features/sala/components/sala-list.component').then((m) => m.SalaListComponent),
  },
  {
    path: 'abbonamenti',
    loadComponent: () => import('./features/abbonamento/components/abbonamento-list.component').then((m) => m.AbbonamentoListComponent),
  },
  {
    path: 'turni',
    canActivate: [authGuard],
    loadComponent: () => import('./features/turno/components/turno-list.component').then((m) => m.TurnoListComponent),
  },
  {
  path: 'cambio-ruolo',
  loadComponent: () => import('./features/cambio-ruolo/component/cambio-ruolo-form.component')
      .then(m => m.CambioRuoloFormComponent)
}

];
