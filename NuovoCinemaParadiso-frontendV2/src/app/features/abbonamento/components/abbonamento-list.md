## abbonamento-list

### abbonamento-list.component.ts 

<details>
    <summary>
    V1.0
    </summary>

Utente: Andrea Paris
Data: 03/06/2026
Descrizione creazione file .ts di abbonamento-list

```ts
import { Component, inject, signal } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';

import { AbbonamentoService } from '../../../services/abbonamento.service';
import { Abbonamento } from '../../../models/abbonamento.model';

@Component({
    selector: 'app-abbonamento-list',
    standalone: true,
    templateUrl: './abbonamento-list.component.html',
    styleUrls: ['./abbonamento-list.component.css']
})
export class AbbonamentoListComponent {

    private readonly abbonamentoService = inject(AbbonamentoService);



    readonly abbonamenti = signal<Abbonamento[]>([]);
    readonly staCaricando = signal(false);
    readonly messaggioErrore = signal('');
    readonly messaggioSuccesso = signal('');

    constructor() {
        this.caricaAbbonamenti();
    }

    caricaAbbonamenti(): void {

        this.staCaricando.set(true);
        this.messaggioErrore.set('');

        this.abbonamentoService.ottieniTutto().subscribe({
            next: (items) => {
                this.abbonamenti.set(items);
                this.staCaricando.set(false);
            },
            error: (error: unknown) => {
                this.staCaricando.set(false);
                this.messaggioErrore.set(
                    this.estraiMessaggioErrore(error, 'Abbonamenti non trovati')
                );
            }
        });
    }

    tracciaPerId(_: number, item: Abbonamento): string {
        return item.id;
    }

    private estraiMessaggioErrore(
        error: unknown,
        fallback: string
    ): string {

        if (error instanceof HttpErrorResponse) {
            return error.error?.message ?? fallback;
        }

        return fallback;
    }
}
```

</details>

### abbonamento-list.component.html

<details>
    <summary>
    V1.0
    </summary>

Utente: Andrea Paris
Data: 03/06/2026
Descrizione creazione file .html di abbonamento-list

```html
<section>
    @if(messaggioErrore()){
    <div class="alert alert-warning">{{ messaggioErrore()}}</div>
    }

    @if(messaggioSuccesso()){
    <div class="alert alert-success">{{ messaggioSuccesso() }}</div>
    }

    <div class="grid grid-2">
        <article class="card">
            <h2>Lista abbonamenti</h2>

            @if(staCaricando()){
            <p class="muted"> Caricamento in corso...</p>
            } @else if(abbonamenti().length === 0){
            <p class="muted"> Nessun abbonamento presente</p>
            } @else{
            <div class="list">
                <div class="list-header">
                    <div>Abbonamento</div>
                    <div>Prezzo</div>
                    <div>Sconto</div>
                    <div>Durata</div>
                </div>
                @for (item of abbonamenti(); track item.id) {
                <div class="list-item">
                    <div>{{ item.nome }}</div>
                    <div>{{ item.prezzo }} €</div>
                    <div>{{ item.sconto }}</div>
                    <div>{{ item.durata }} mesi</div>
                </div>
                }
            </div>
            }
        </article>
    </div>
</section>
```

</details>

### abbonamento-list.component.css

<details>
    <summary>
    V1.0
    </summary>

Utente: Andrea Paris
Data: 03/06/2026
Descrizione creazione file .css di abbonamento-list

```css
.list-header,
.list-item {
    display: grid;
    grid-template-columns: 2fr 1fr 1fr 1fr;
    align-items: center;
}

.list-header {
    font-weight: 600;
    padding: 0.75rem 1rem;
    margin-bottom: 0.5rem;
}

.list-item {
    padding: 1rem;
    border: 1px solid var(--border-color, #1e3a5f);
    border-radius: 12px;
    margin-bottom: 0.75rem;
}
```

</details>