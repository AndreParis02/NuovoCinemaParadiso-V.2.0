export interface GiftCard {
    id: string | null;
    nome: string;
    valore: number;
    codiceRiscatto: string;
}

export interface GiftCardCreazione {
    nome: string;
    valore: number;
    codiceRiscatto: string;
}

export interface RicaricaGiftCard {
    importo: number;
}

export interface CodiceRiscatto {
    CodiceRiscatto: string;
}