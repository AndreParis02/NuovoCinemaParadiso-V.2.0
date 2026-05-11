export interface Turno {
    id: string | null;
    oraInizio: string;
    oraFine: string;
    nome: string;
}
export interface TurnoCreazione {
    oraInizio: string;
    oraFine: string;
    nome: string;
}