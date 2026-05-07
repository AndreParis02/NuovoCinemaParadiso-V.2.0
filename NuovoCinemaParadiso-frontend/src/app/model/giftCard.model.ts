import { Utente } from './utente.model';
export interface GiftCard {
    id: string;
    nome: string;
    durata: number;
    prezzo: number;
    numeroMovie: number;
    utenti: Utente[];
}