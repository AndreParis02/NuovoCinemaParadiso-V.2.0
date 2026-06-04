export interface Biglietto {
    id: string | null;
    proiezioneId: string;
    utenteId: string;
    prezzoFinale: number;
    orarioCreazione: string;
    numeroBiglietti: number;
    metodoPagamento: string;
}
export interface BigliettoCreazione {
    proiezioneId: string;
    numeroBiglietti: number;
}