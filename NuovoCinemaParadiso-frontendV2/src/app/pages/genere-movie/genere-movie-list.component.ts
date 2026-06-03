//controllare gli import. nomi classi e file sono differenti per "-"
import { Component, inject, signal } from '@angular/core';
//import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { AuthService } from '../../services/auth.service';
import { GenereMovieService } from '../../services/genere-movie.service';
import { GenereMovie } from '../../models/genere-movie.model';

@Component({
  selector: 'genere-movie-list',
  imports: [],
  templateUrl: './genere-movie-list.component.html',
})

export class GenereMoviePage {

  private readonly authService = inject(AuthService);
  private readonly genereMovieService = inject(GenereMovieService);

  readonly generiMovies = signal<GenereMovie[]>([]);
  readonly isLoading = signal(false);
  readonly errorMessage = signal('');
  readonly successMessage = signal('');


  constructor() {
    this.loadGeneriMovies();
  }

  //carica la lista dei generi dal backend
  loadGeneriMovies(): void {
    this.isLoading.set(true);
    this.errorMessage.set('');

    //chiamata al servizio ottienitutto per ottenere la lista dei generi dei film
    this.genereMovieService.ottieniTutto().subscribe({
      next: (items) => {
        this.generiMovies.set(items);
        this.isLoading.set(false);
      },
      error: (error: unknown) => {
        this.isLoading.set(false);
        this.errorMessage.set(
          this.extractErrorMessage(error, 'Impossibile caricare i generi dei film')
        );
      }
    });
  }

  trackById(_: string, item: GenereMovie): string | null {
    return item.id
  }

  private extractErrorMessage(error: unknown, fallback: string): string {
    if (error instanceof Error) {
      return error?.message ?? fallback;
    }
    return fallback
  }
}
