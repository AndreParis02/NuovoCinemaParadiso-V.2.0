export interface AcquistoResponse {
    id: string | null;
    proiezioneId: string;
    utenteId: string;
    prezzoFinale: number;
    orarioCreazione: string;
    numeroBiglietti: number;
    metodoPagamento: string;
}
export interface AcquistoRequest {
    proiezioneId: string;
    numeroBiglietti: number;
    metodoPagamento: string;
}