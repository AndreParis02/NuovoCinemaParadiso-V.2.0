export interface Utente {
    id: string;
    nomeCompleto: string;
    dataInizioAbbonamento: string;
    seAbbonato: boolean;
    email: string;
    eta: number;
    abbonamentoId: string;
    tipoAbbonamento: string;
    saldo: number;
}

export interface UtenteCreazione {
    nomeCompleto: string;
    eta: number;
}

export interface UtenteModificaRuolo {
    email: string;
    nuovoRuolo: string;
}