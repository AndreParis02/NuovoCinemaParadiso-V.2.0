import { Component, input} from '@angular/core';

import { Abbonamento } from '../../../models/abbonamento.model';

@Component({
    selector: 'abbonamento-detail',
    standalone: true,
    templateUrl: './abbonamento-detail.component.html'
})

export class AbbonamentoDetailComponent {

    readonly abbonamentoSelezionato = input<Abbonamento | null>(null);

    constructor() {
        
    }

}
