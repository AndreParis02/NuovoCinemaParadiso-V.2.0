export interface SalaRequest {
    nome: string;
    capienza: number;
    tipologiaSalaId: string;
}

export interface SalaResponse {
    id: string | null;
    nome: string;
    capienza: number;
    tipologiaSalaId: string;
    nomeTipologia: string;
}