import { Sala } from './sala.model';

export interface TipologiaSala {
    id: string;
    nome: string;
    maggiorazionePrezzo: number;
    sale: Sala[];
}