export interface Utente {
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

export interface UtenteCreazione {
    nomeCompleto: string;
    eta: number;
}

export interface UtenteModificaRuolo {
    email: string;
    nuovoRuolo: string;
}
