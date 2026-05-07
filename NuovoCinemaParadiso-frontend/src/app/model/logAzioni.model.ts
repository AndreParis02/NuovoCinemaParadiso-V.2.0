export interface LogAzioni {
    id: string;
    idUtente: string | null;
    nomeAzione: string;
    effettuato: boolean;
    messaggio: string;
    timeStamp: string;
}