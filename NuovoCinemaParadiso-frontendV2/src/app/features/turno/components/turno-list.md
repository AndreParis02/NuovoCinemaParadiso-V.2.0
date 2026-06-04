## turno-list

### turno-list.component.ts 

<details>
    <summary>
    V1.0
    </summary>

Utente: Andrea Paris
Data: 03/06/2026
Descrizione creazione file .ts di turno-list

```ts
import { Component, inject, signal } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';

import { TurnoService } from '../../../services/turno.service';
import { Turno } from '../../../models/turno.model';

@Component({
    selector: 'turno-list',
    standalone: true,
    templateUrl: './turno-list.component.html'
})
export class TurnoListComponent {

    private readonly turnoService = inject(TurnoService);



    readonly turni = signal<Turno[]>([]);
    readonly staCaricando = signal(false);
    readonly messaggioErrore = signal('');
    readonly messaggioSuccesso = signal('');

    constructor() {
        this.caricaTurni();
    }

    caricaTurni(): void {

        this.staCaricando.set(true);
        this.messaggioErrore.set('');

        this.turnoService.ottieniTutto().subscribe({
            next: (items) => {
                this.turni.set(items);
                this.staCaricando.set(false);
            },
            error: (error: unknown) => {
                this.staCaricando.set(false);
                this.messaggioErrore.set(
                    this.estraiMessaggioErrore(error, 'Turni non trovati')
                );
            }
        });
    }

    tracciaPerId(_: number, item: Turno): string {
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

### turno-list.component.html

<details>
    <summary>
    V1.0
    </summary>

Utente: Andrea Paris
Data: 03/06/2026
Descrizione creazione file .html di turno-list

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
            <h2>Lista turni</h2>

            @if(staCaricando()){
            <p class="muted"> Caricamento in corso...</p>
            } @else if(turni().length === 0){
            <p class="muted"> Nessun turno presente</p>
            } @else{
            <div class="list">
                <div class="list-header">
                    <div>Turno</div>
                    <div>Ora inizio</div>
                    <div>Ora fine</div>
                </div>
                @for (item of turni(); track item.id) {
                <div class="list-item">
                    <div>{{ item.nome }}</div>
                    <div>{{ item.oraInizio }}</div>
                    <div>{{ item.oraFine }}</div>
                </div>
                }
            </div>
            }
        </article>
    </div>
</section>
```

</details>
