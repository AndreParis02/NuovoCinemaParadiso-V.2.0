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

export interface UtenteModificaRuoloRichiesta {
    email: string;
    nuovoRuolo: string;
}

export interface UtenteModificaRuoloRisposta {
    messaggio: string;
    email: string;
    ruolo: string;
}