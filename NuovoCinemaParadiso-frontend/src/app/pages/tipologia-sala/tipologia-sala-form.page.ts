import { Component, OnInit } from '@angular/core';
import { TipologiaSala, TipologiaSalaCreazione } from '../../models/tipologia-sala.model';
import { TipologiaSalaService } from '../../services/tipologia-sala.service';
@Component({
    selector: 'app-tipologia-sala-form',
    templateUrl: './tipologia-sala-form.page.html',
})
export class TipologiaSalaFormPage {
    tipologiaSalaCreazione: TipologiaSalaCreazione = {
        nome: '',
        maggiorazionePrezzo: 0
    };

    constructor(private tipologiaSalaService: TipologiaSalaService) {
       
    }


    Submit(): void {
        this.tipologiaSalaService.crea(this.tipologiaSala).subscribe(data => {
      this.tipologiaSala = data;
    });
  }
}