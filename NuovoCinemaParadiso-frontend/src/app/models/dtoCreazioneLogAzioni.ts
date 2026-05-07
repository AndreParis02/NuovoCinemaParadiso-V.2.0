export interface DtoCreazioneLogAzioni {
    id: string | null;
    idUtente: string;
    nomeAzione: string;
    effettuato: boolean;
    messaggio: string;
    timeStamp: string;
}