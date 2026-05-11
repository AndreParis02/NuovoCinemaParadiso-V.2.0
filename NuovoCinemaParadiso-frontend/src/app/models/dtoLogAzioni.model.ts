export interface LogAzioni {
    id: string | null;
    idUtente: string | null;
    nomeAzione: string;
    effettuato: boolean;
    messaggio: string;
    timeStamp: string;
}

export interface LogAzioniCreazione {
    id: string | null;
    idUtente: string;
    nomeAzione: string;
    effettuato: boolean;
    messaggio: string;
    timeStamp: string;
}