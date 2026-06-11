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