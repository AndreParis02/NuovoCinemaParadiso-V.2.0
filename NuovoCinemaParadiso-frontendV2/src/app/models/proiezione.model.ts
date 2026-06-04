export interface Proiezione {
    id: string;
    dataProiezione: Date;
    movieId: string;
    titoloMovie: string;
    salaId: string;
    nomeSala: string;
    turnoId: string;
    nomeTurno: string;
}

export interface ProiezioneCreazione {
    movieId: string;
    salaId: string;
    turnoId: string;
    dataProiezione: string;
}