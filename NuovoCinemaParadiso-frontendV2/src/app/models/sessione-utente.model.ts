export interface SessioneUtente {
    id: string;
    nomeCompleto: string;
    token: string;
    eta: number;
    email: string;
    ruolo: string;
    abbonamento: string | null;
    dataInizioAbbonamento: string;
    seAbbonato: boolean;
}
