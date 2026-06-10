
### proiezione.page.ts

<details>
 
Andrea Bruno 10-06-2026
- Creazione della pagina ts di proiezione 

</details><summary> Versione 1.0 </summary>

```ts
import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ProiezioneList } from './components/proiezione-list.component';

@Component({
  selector: 'proiezione-page',
  standalone: true,

  // Import dei moduli necessari per il template della pagina.
  // CommonModule → abilita *ngIf, *ngFor, pipe comuni, ecc.
  // ProiezioneList → componente principale che gestisce lista, acquisto e form.
  imports: [CommonModule, ProiezioneList],

  // Template della pagina (contenitore).  
  // La logica è delegata ai componenti figli.
  templateUrl: './proiezione.page.html'
})
export class ProiezionePage {

  // Questa pagina non contiene logica: funge solo da "contenitore" per routing e layout.
  // Tutta la logica di caricamento, modifica, acquisto e gestione è nel componente ProiezioneList.
  
}
```

### proiezione.page.html

<details>
 
Andrea Bruno 10-06-2026
- Creazione della pagina html di proiezione

</details><summary> Versione 1.0 </summary>   

```html
<section class="container">

    <!--
      Il componente principale che gestisce:
      - caricamento delle proiezioni
      - acquisto biglietti
      - eliminazione (se operatore)
      - form di creazione/modifica (inserito internamente)
      
      La page non contiene logica: delega tutto a ProiezioneList che a sua volta contiene il form
    -->
    <proiezione-list></proiezione-list>
</section>
```