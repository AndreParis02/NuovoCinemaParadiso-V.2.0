export interface DtoMovie {
    id: string | null;
    titolo: string;
    descrizione: string;
    durataMinuti: number;
    prezzoMovie: number;
    genereId: string;
    genere: string;
}