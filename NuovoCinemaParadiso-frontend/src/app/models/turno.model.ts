import { Proiezione } from './proiezione.model';

export interface Turno {
    id: string;
    oraInizio: string;
    oraFine: string;
    proiezioni: Proiezione[];
    nome: string;
}