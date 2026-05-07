export interface DtoLogAzioni {
    id: string | null;
    idUtente: string | null;
    nomeAzione: string;
    effettuato: boolean;
    messaggio: string;
    timeStamp: string;
}