export interface Abbonamento {
    id: string;
    nome: string;
    durata: number;
    prezzo: number;
    sconto: number;
}

export interface AbbonamentoCreazione {
    nome: string;
    durata: number;
    prezzo: number;
    sconto: number;
} 