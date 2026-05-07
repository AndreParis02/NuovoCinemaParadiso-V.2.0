import { Proiezione } from './proiezione.model';
import { GenereMovie } from './genereMovie.model';


export interface Movie {
    id: string;
    titolo: string;
    descrizione: string;
    durataMinuti: number;
    prezzoMovie: number;
    proiezioni: Proiezione[];
    genereId: string;
    genere: GenereMovie | null;
}