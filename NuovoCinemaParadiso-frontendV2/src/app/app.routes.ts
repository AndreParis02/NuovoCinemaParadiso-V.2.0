import { Routes } from '@angular/router';
import { authGuard } from './core/guards/auth.guard';
import { guestGuard } from './core/guards/guest.guard';
import { roleGuard } from './core/guards/role.guard';
import { GestionePage } from './pages/gestione/gestione.page';
import { BigliettoListComponent } from './features/biglietto/components/biglietto-list.component';

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
    loadComponent: () => import('./pages/register/register.page').then((m) => m.RegisterPage),
  },
  {
    path: 'giftcard-list',
    canActivate: [authGuard, roleGuard], 
    loadComponent: () => import('./features/giftcard/components/giftcard-list.component').then(m => m.GiftCardListComponent)
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
    loadComponent: () => import('./pages/dashboard/dashboard.page').then((m) => m.DashboardPage),
  },

  // Aggiunto per test del componente biglietto-list
  {
    path: 'lista-biglietti',
    component: BigliettoListComponent,
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
    loadComponent: () => import('./pages/abbonamento/abbonamento.page').then((m) => m.AbbonamentoPage)
  },

  {
    path: 'operatore/cambio-ruolo',
    canActivate: [authGuard, roleGuard],
    data: { roles: ['operatore'] },
    loadComponent: () => import('./pages/operatore/operatore.page').then((m) => m.OperatorePage)
  },
  {
    path: 'abbonamenti',
    canActivate: [authGuard],
    loadComponent: () => import('./pages/abbonamento/abbonamento.page').then((m) => m.AbbonamentoPage)
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
    canActivate: [authGuard, roleGuard],
    loadComponent: () => import('./pages/proiezione/proiezione.page').then((m) => m.ProiezionePage)
  },
  {
    path: 'giftcard',
    canActivate: [authGuard],
    loadComponent: () => import('./pages/giftcard/giftcard.page').then((m) => m.GiftcardPage)
  },
  {
    path: 'genere-movie',
    canActivate: [authGuard],
    loadComponent: () => import('./features/genere-movie/components/genere-movie-list.component').then((m) => m.GenereMoviePage)
  },
  {
    path: 'tipologia-sala',
    canActivate: [authGuard],
    loadComponent: () => import('./features/tipologia-sala/components/tipologia-sala-list.component').then((m) => m.TipologiaSalaPage)
  },
  {
    path: 'sala',
    canActivate: [authGuard],
    loadComponent: () => import('./pages/sala/sala.page').then((m) => m.SalaPage)
  },
  {
    path: 'movies',
    canActivate: [authGuard],
    loadComponent: () => import('./pages/movie/movie.page').then((m) => m.MoviePage),
  },
  {
    path: 'turno',
    canActivate: [authGuard],
    loadComponent: () => import('./pages/turno/turno.page').then((m) => m.TurnoPage),
  },
];
