export interface Biglietto {
    id: string | null;
    proiezioneId: string;
    utenteId: string;
    prezzoFinale: number;
    orarioCreazione: string;
    numeroBiglietti: number;
    nomeSala: string;
    titoloMovie : string;
    nomeTipologiaSala: string;
    dataProiezione: string;
    oraInizio: string;
}
export interface BigliettoCreazione {
    proiezioneId: string;
    numeroBiglietti: number;
}