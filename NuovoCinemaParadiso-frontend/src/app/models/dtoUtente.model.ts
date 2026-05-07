export interface DtoUtente {
    id: string;
    nomeCompleto: string;
    dataInizioAbbonamento: string;
    dataInizioGiftCard: string;
    seAbbonato: boolean;
    possiedeGiftCard: boolean;
    email: string;
    eta: number;
    abbonamentoId: string;
    giftCardId: string;
    tipoAbbonamento: string;
    tipoGiftCard: string;
}