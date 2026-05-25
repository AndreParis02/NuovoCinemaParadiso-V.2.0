import { Routes } from '@angular/router';
import { authGuard } from './core/guards/auth.guard';
import { guestGuard } from './core/guards/guest.guard';
import { roleGuard } from './core/guards/role.guard';

export const routes: Routes = [
    {
        path: '',
        pathMatch: 'full',
        redirectTo: 'dashboard'
    },
    {
        path: 'login',
        canActivate: [guestGuard],
        loadComponent: () => import('./pages/login/login.page').then((m) => m.LoginPage) // load component carica le pagine in lazyloagin nel modello standalone di angular
    },
    
    {
        path: 'register',
        canActivate: [guestGuard],
        loadComponent: () => import('./pages/register/register.page').then((m) => m.RegisterPage)
    },
    {
        path: 'movie',
        canActivate: [authGuard],
        loadComponent: () => import('./pages/movie/movie.page').then((m) => m.MoviePage)
    },
    {
        path: 'dashboard',
        canActivate: [authGuard],
        loadComponent: () => import('./pages/dashboard/dashboard.page').then((m) => m.DashboardPage)
    },
    /*
    {
        path: 'abbonamento/:id',
        canActivate: [authGuard],
        loadComponent: () => import('./pages/abbonamento/abbonamento-detail.page').then((m) => m.AbbonamentoDetailPage)
    },
    {
        path: 'abbonamento-list',
        canActivate: [authGuard],
        loadComponent: () => import('./pages/abbonamento/abbonamento-list.page').then((m) => m.AbbonamentoListPage)
    },
    
    {
        path: 'gestore/listaUtenti',
        canActivate: [authGuard],
        loadComponent: () => import('./pages/gestore/gestore-lista-utenti.page').then((m) => m.GestoreListaUtentiPage)
    },
    
    {
        path: 'gestore/biglietto',
        canActivate: [authGuard],
        loadComponent: () => import('./pages/users-list/users-list.page').then((m) => m.BigliettoListPage)
    },
    {
        path: 'gestore/biglietto:id',
        canActivate: [authGuard],
        loadComponent: () => import('./pages/users-list/users-list.page').then((m) => m.BigliettoDetailPage)
    },
    */
    {
        path: 'gestore/change-role',
        canActivate: [authGuard, roleGuard],
        data: { roles: ['gestore'] },
        loadComponent: () => import('./pages/gestore-change-role/gestore-change-role.page').then((m) => m.GestoreChangeRolePage)
    },
    /*
    {
        path: 'biglietto',
        canActivate: [authGuard],
        loadComponent: () => import('./pages/biglietto/biglietto.page').then((m) => m.BigliettoPage)
    },
    {
        path: 'giftcard',
        canActivate: [authGuard],
        loadComponent: () => import('./pages/giftcard/giftcard.page').then((m) => m.GiftcardDetailPage)
    },
    {
        path: 'giftcard',
        canActivate: [authGuard],
        loadComponent: () => import('./pages/giftcard/giftcard.page').then((m) => m.GiftcardListPage)
    },
    {
        path: 'genere-movie',
        canActivate: [authGuard],
        loadComponent: () => import('./pages/genere-movie/genere-movie.page').then((m) => m.GenereMovieDetailPage)
    },
    {
        path: 'genere-movie',
        canActivate: [authGuard],
        loadComponent: () => import('./pages/genere-movie/genere-movie.page').then((m) => m.GenereMovieListPage)
    },
    */
    {
        path: 'tipologia-sala',
        canActivate: [authGuard],
        loadComponent: () => import('../../../NuovoCinemaParadiso-frontendV2/src/app/pages/tipologia-sala/tipologia-sala.page').then((m) => m.TipologiaSalaPage)
    },
        {
        path: 'genere-movie',
        canActivate: [authGuard],
        loadComponent: () => import('../../../NuovoCinemaParadiso-frontendV2/src/app/pages/genere-movie/genere-movie.page').then((m) => m.GenereMoviePage)
    },
    /*
    {
        path: 'tipologia-sala',
        //canActivate: [authGuard],
        loadComponent: () => import('./pages/tipologia-sala/tipologia-sala-list.component').then((m) => m.TipologiaSalaListComponent)
    },
]

{
    path: 'sala',
    canActivate: [authGuard],
    loadComponent: () => import('./pages/sala/sala.page').then((m) => m.SalaDetailPage)
},
{
    path: 'sala',
    canActivate: [authGuard],
    loadComponent: () => import('./pages/sala/sala.page').then((m) => m.SalaListPage)
},
{
    path: 'proiezione',
    canActivate: [authGuard],
    loadComponent: () => import('./pages/proiezione/proiezione.page').then((m) => m.ProiezioneDetailPage)
},
{
    path: 'proiezione',
    canActivate: [authGuard],
    loadComponent: () => import('./pages/proiezione/proiezione.page').then((m) => m.ProiezioneListPage)
},
{
    path: 'turno',
    canActivate: [authGuard],
    loadComponent: () => import('./pages/turno/turno.page').then((m) => m.TurnoDetailPage)
},
{
    path: 'turno',
    canActivate: [authGuard],
    loadComponent: () => import('./pages/turno/turno.page').then((m) => m.TurnoListPage)
},
{
    path: '**',
    redirectTo: 'dashboard'
},

*/
];

