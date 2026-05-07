import { Sala } from './sala.model';
import { Movie } from './movie.model';
import { Turno } from './turno.model';
import { Acquisto } from './acquisto.model';




export interface Proiezione {
    id: string;
    dataProiezione: Date;
    movieId: string;
    movie: Movie | null;
    salaId: string;
    sala: Sala | null;
    turnoId: string;
    turno: Turno | null;
    acquisti: Acquisto[];
}