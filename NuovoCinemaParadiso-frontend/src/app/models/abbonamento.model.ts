export interface AbbonamentoResponse {
    id: string | null;
    nome: string;
    durata: number;
    prezzo: number;
    sconto: number;
}

export interface AbbonamentoRequest {
    nome: string;
    durata: number;
    prezzo: number;
    sconto: number;
}