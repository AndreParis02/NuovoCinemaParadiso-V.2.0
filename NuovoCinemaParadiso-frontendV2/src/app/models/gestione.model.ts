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

export interface ContoCinema {
  id: string;
  iban: string;
  titolareConto: string;
  saldo: number;
}

export interface Biglietto {
  id: string | null;
  utenteId: string;
  prezzoFinale: number;
  orarioCreazione: string;
  proiezioneId: string;
  numeroBiglietti: number;
  nomeSala: string;
  titoloMovie: string;
  nomeTipologiaSala: string;
  oraInizio: string; 
  dataProiezione: string; 
}

export interface GiftCard {
  id: string | null;
  nome: string;
  valore: number;
  codiceRiscatto: string;
}