export interface Acquisto {
    id: string | null;
    proiezioneId: string;
    utenteId: string;
    prezzoFinale: number;
    orarioCreazione: string;
    numeroBiglietti: number;
    metodoPagamento: string;
}
export interface AcquistoCreazione {
    proiezioneId: string;
    numeroBiglietti: number;
    metodoPagamento: string;
}