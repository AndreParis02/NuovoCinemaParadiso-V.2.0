export interface Auth {
   id: string;
   nomeCompleto: string;
   token: string;
   eta: number;
   email: string;
   ruolo: string;
   dataInizioAbbonamento: string;
   dataInizioGiftCard: string;
   seAbbonato: boolean;
   possiedeGiftCard: boolean;
}

export interface Login {
   email: string;
   password: string;
}

export interface Registrazione {
   email: string;
   password: string;
   nomeCompleto: string;
   eta: number;
}