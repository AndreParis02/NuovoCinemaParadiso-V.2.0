import { Component, inject, signal } from '@angular/core';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { HttpErrorResponse } from '@angular/common/http';

import { MovieService } from '../../services/movie.service';
import { ProiezioneService } from '../../services/proiezione.service';

import { Movie } from '../../models/movie.model';
import { GenereMovie } from '../../models/genere-movie.model';
import { Proiezione } from '../../models/proiezione.model'; 


@Component({
  selector: 'lista-movie',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './lista-movie.component.html'
})

export class ListaMovieComponent {

  private readonly movieService = inject(MovieService);
  private readonly proiezioneService = inject(ProiezioneService);


  readonly movies = signal<Movie[]>([]);
  readonly proiezioni = signal<Proiezione[]>([]);
  readonly staCaricando = signal(false);
  readonly staInviando = signal(false);
  readonly messaggioErrore = signal('');
  readonly messaggioSuccesso = signal('');

  constructor() {
    this.caricaMovies();
  }
  caricaMovies(): void {

    this.staCaricando.set(true);
    this.messaggioErrore.set('');
    this.proiezioneService.ottieniTutto().subscribe({
      next: (items) => {
        this.proiezioni.set([]);
        for (let i = 0; i < items.length; i++) {
            if(new Date(items[i].dataProiezione) >= new Date()) {
                this.proiezioni.update(proiezioni => [...proiezioni, items[i]]);
            }
        }
        this.movies.set([]);
        for (let i = 0; i < this.proiezioni().length; i++) {
            this.movieService.ottieniTramiteId(this.proiezioni()[i].movieId).subscribe({
                next: (movie) => {
                    if(!this.movies().some(m => m.id === movie.id)) { 
                        this.movies.update(movies => [...movies, movie]);
                    }
                },
                error: (error: unknown) => {
                    this.messaggioErrore.set(this.estraiMessaggioErrore(error, 'film non trovati'));
                }
            });
        }
        this.staCaricando.set(false);
      },
      error: (error: unknown) => {
        this.staCaricando.set(false);
        this.messaggioErrore.set(this.estraiMessaggioErrore(error, 'proiezioni non trovate'));
      }
    });
  }

  tracciaPerId(_: string, item: Movie ): string {
    return item.id;
  }

  private estraiMessaggioErrore(error: unknown, fallback: string): string {

    if (error instanceof HttpErrorResponse) {
      return error.error?.message ?? fallback;
    }
    return fallback;
  }
}
