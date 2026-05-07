import { Acquisto } from './acquisto.model';
import { Abbonamento } from './abbonamento.model';
import { GiftCard } from './giftCard.model';


export interface Utente {
    nomeCompleto: string;
    eta: number;
    seAbbonato: boolean;
    possiedeGiftCard: boolean;
    dataInizioAbbonamento: string;
    dataInizioGiftCard: string;
    acquisti: Acquisto[];
    abbonamentoId: string | null;
    abbonamento: Abbonamento | null;
    giftCardId: string | null;
    giftCard: GiftCard | null;
}