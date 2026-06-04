export interface SessioneUtente {
    id: string;
    nomeCompleto: string;
    token: string;
    eta: number;
    email: string;
    ruolo: string;
    dataInizioAbbonamento: string;
    seAbbonato: boolean;
}
