export interface ProiezioneResponse {
    id: string;
    dataProiezione: Date;
    movieId: string;
    salaId: string;
    turnoId: string;
}

export interface ProiezioneRequest {
    movieId: string;
    salaId: string;
    turnoId: string;
    dataProiezione: Date;
}