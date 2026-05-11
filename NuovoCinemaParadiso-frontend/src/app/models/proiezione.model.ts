export interface Proiezione {
    id: string;
    dataProiezione: Date;
    movieId: string;
    salaId: string;
    turnoId: string;
}

export interface ProiezioneCreazione {
    movieId: string;
    salaId: string;
    turnoId: string;
    dataProiezione: Date;
}
