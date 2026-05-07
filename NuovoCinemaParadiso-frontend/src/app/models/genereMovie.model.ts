import { Movie } from './movie.model';
export interface GenereMovie {
    id: string;
    genere: string;
    movies: Movie[];
}