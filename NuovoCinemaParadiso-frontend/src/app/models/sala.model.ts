export interface SalaCreazione {
    nome: string;
    capienza: number;
    tipologiaSalaId: string;
}

export interface Sala {
    id: string | null;
    nome: string;
    capienza: number;
    tipologiaSalaId: string;
    nomeTipologia: string;
}