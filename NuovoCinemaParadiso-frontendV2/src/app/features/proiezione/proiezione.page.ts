import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ProiezioneList } from './components/proiezione-list.component';

@Component({
  selector: 'proiezione-page',
  standalone: true,
  imports: [CommonModule, ProiezioneList],
  templateUrl: './proiezione.page.html'
})
export class ProiezionePage {}