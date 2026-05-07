import { Utente } from './utente.model';

export interface Abbonamento {
    id: string;
    nome: string;
    durata: number;
    prezzo: number;
    sconto: number;
    utenti: Utente[];
}