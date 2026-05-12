import { Component, OnInit } from '@angular/core';
import { TipologiaSala } from '../../models/tipologia-sala.model'; 
import { TipologiaSalaService } from '../../services/tipologia-sala.service';

@Component({
  selector: 'app-tipologia-sala-detail',
  templateUrl: './tipologia-sala-detail.component.html',
})
export class TipologiaSalaDetailComponent implements OnInit {
  
  tipologiaSala: TipologiaSala | null = null;
  id: string = ''; // impostare questo ID in qualche modo, ad esempio tramite route parametrica

  constructor(private tipologiaSalaService: TipologiaSalaService, id: string) {
    this.id = id;
  }

  ngOnInit(): void {
    this.tipologiaSalaService.ottieniTramiteId(this.id).subscribe(data => {
      this.tipologiaSala = data;
    });
  }
}