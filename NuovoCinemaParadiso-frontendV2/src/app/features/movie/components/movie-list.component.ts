import { Component, inject, signal } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';

import { AuthService } from '../../../services/auth.service';
import { MovieService } from '../../../services/movie.service';


import { Movie } from '../../../models/movie.model';
import { MovieFormComponent } from "./movie-form.component";


@Component({
  selector: 'movie-list',
  standalone: true,
  templateUrl: './movie-list.component.html',
  imports: [MovieFormComponent]
})

export class MovieListComponent {

  private readonly authService = inject(AuthService);
  private readonly movieService = inject(MovieService);


  readonly movies = signal<Movie[]>([]);
  readonly filmScelto = signal<Movie | null>(null);
  readonly staCaricando = signal(false);
  readonly staInviando = signal(false);
  readonly messaggioErrore = signal('');
  readonly messaggioSuccesso = signal('');


  constructor() {
    this.caricaMovies();
  }
  visualizzabileDa(): boolean {
    return this.authService.possiedeQualsiasiRuolo(['Operatore']);
  }
  caricaMovies(): void {

    this.staCaricando.set(true);
    this.messaggioErrore.set('');
    this.movieService.ottieniTutto().subscribe({

      next: (items) => {
        this.movies.set(items);
        this.staCaricando.set(false);

      },
      error: (error: unknown) => {
        this.staCaricando.set(false);
        this.messaggioErrore.set(this.estraiMessaggioErrore(error, 'film non trovati'));
      }
    });
  }

  elimina(item: Movie): void {
    if (!this.visualizzabileDa()) {
      return;
    }
    const confirmed = confirm(`Eliminare il film \" ${item.titolo}\"?`);
    if (!confirmed) {
      return;
    }

    this.messaggioErrore.set('');
    this.messaggioSuccesso.set('');

    this.movieService.elimina(item.id).subscribe({
      next: () => {
        if (this.filmScelto()?.id === item.id) {
          this.filmScelto.set(null);
        }
        this.caricaMovies();
      },
      error: (error: unknown) => {

        this.messaggioErrore.set(this.estraiMessaggioErrore(error, 'eliminazione non riuscita.'));
      }
    });
  }

  tracciaPerId(_: string, item: Movie): string {
    return item.id;
  }

  private estraiMessaggioErrore(error: unknown, fallback: string): string {

    if (error instanceof HttpErrorResponse) {
      return error.error?.message ?? fallback;
    }
    return fallback;
  }
}
