import { Component, OnInit } from '@angular/core';
import { TipologiaSala } from '../../models/tipologia-sala.model'; // Esempio di path
import { TipologiaSalaService } from '../../services/tipologia-sala.service';

@Component({
  selector: 'app-tipologia-sala-list',
  templateUrl: './tipologia-sala-list.component.html',
})
export class TipologiaSalaListComponent implements OnInit {
  
  tipologiaSala: TipologiaSala[] = [];

  // L'iniezione del servizio nel costruttore
  constructor(private tipologiaSalaService: TipologiaSalaService) { }

  ngOnInit(): void {
    this.tipologiaSalaService.ottieniTutto().subscribe(data => {
      this.tipologiaSala = data;
    });
  }
}