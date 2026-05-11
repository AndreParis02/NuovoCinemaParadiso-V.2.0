export interface Sala {
    id: string | null;
    nome: string;
    capienza: number;
    tipologiaSalaId: string;
    nomeTipologia: string;
}

export interface SalaCreazione {
    nome: string;
    capienza: number;
    tipologiaSalaId: string;
}
