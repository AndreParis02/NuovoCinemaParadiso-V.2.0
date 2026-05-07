import { Proiezione } from './proiezione.model';
import { TipologiaSala } from './tipologiaSala.model';


export interface Sala {
    id: string;
    nome: string;
    capienza: number;
    proiezioni: Proiezione[];
    tipologiaSalaId: string;
    tipologiaSala: TipologiaSala | null;
}