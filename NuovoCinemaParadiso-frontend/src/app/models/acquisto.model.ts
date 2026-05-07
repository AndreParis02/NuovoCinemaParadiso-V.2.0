import { Utente } from './utente.model';
import { Proiezione } from './proiezione.model';

export interface Acquisto {
    id: string;
    proiezioneId: string;
    proiezione: Proiezione | null;
    utenteId: string;
    utente: Utente | null;
    numeroBiglietti: number;
    orarioCreazione: string;
    prezzoFinale: number;
    metodoPagamento: string;
}