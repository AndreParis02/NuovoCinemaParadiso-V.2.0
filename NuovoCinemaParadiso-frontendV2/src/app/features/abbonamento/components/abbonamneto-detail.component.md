# abbonamento-detail

<detail>
<summary>versione1.0</summary>

Francesco Lorenzi 05/06/2026

molto semplicemente passiamo un oggetto abbonamento esternamente

## abbonamento-detail.ts
```ts
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

```
## abbonamento-detail.html

utilizzando questo oggetto abbonamento, diamo le sue info, non ho messo la durata perchè verraà utilizzata dalla dashboard e non c'è modo di calcolare la scadenza in modo intuitivo 

```html
<article>
    @if (abbonamentoSelezionato()) {
    <h2>Dettagli Abbonamento</h2>
    <div>
        <p><strong>Nome:</strong> {{ abbonamentoSelezionato()!.nome }}</p>
        <p><strong>sconto:</strong> {{ abbonamentoSelezionato()!.sconto }} €</p>
        <p><strong>Prezzo:</strong> {{ abbonamentoSelezionato()!.prezzo }} €</p>  
    </div>
    }
    @if (!abbonamentoSelezionato()){
        <div>
            <p>non sei abbonato a nessun abbonamento.</p>
        </div>
    }
</article>
```
</detail>