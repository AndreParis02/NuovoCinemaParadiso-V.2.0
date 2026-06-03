import { Component, OnInit, inject } from '@angular/core';
import { Biglietto } from '../../models/biglietto.model'; 
import { BigliettoService } from '../../services/biglietto.service';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'biglietto-list',
  standalone: true,
  templateUrl: './biglietto-list.component.html',
})
export class BigliettoListComponent implements OnInit {
  
  biglietto: Biglietto[] = [];
  messaggio: string | null = null;

  private authService = inject(AuthService);

  constructor(private bigliettoService: BigliettoService) { }

  ngOnInit(): void {
    const utente = this.authService.utenteCorrente();
    if(!utente?.id){
        this.messaggio = "Utente non loggato" ;
        return;
    }
    const utenteId: string = utente?.id;

    this.bigliettoService.ottieniTutto().subscribe({
      next: data => this.biglietto = data,
      error: errore => this.messaggio = "Errore durante il caricamento dei biglietti."
    });
  }
}