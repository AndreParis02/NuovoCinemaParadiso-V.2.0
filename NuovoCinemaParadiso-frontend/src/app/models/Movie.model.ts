export interface Movie {
    id: string | null;
    titolo: string;
    descrizione: string;
    durataMinuti: number;
    prezzoMovie: number;
    genereId: string;
    genere: string;
}

export interface MovieCreazione {
    titolo: string;
    descrizione: string;
    durataMinuti: number;
    prezzoMovie: number;
    genereId: string;
}
