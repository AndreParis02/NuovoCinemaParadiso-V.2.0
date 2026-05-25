import { Component, inject, signal } from '@angular/core';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { HttpErrorResponse } from '@angular/common/http';
import { AuthService } from '../../services/auth.service';
import { SalaService } from '../../services/sala.service';
// path del modello Sala, da adattare in base alla struttura del progetto
import { Sala } from '../../models/sala.model';


@Component({
  //corrisponde al tag HTML che rappresenta questa pagina
  selector: 'sala-page',
  // indica che abbiamo deciso di creare una app standalone, senza modules 
  standalone: true,
  // importiamo ReactiveFormsModule per poter utilizzare i form reattivi nella nostra pagina
  // i form reattivi possono adattarsi a diversi comportamenti
  imports: [ReactiveFormsModule],
  // url del template HTML associato a questa pagina
  templateUrl: './sala.page.html'
})
// export prepara la classe per essere utilizzata in altri file
export class SalaPage {
  // il form builder è un servizio che ci permette di creare form reattivi 
  // in modo semplice e strutturato
  private readonly formBuilder = inject(FormBuilder);
  private readonly authService = inject(AuthService);
  private readonly SalaService = inject(SalaService);

  //devono essere redonly perché non devono essere modificati dopo l'inizializzazione
  // signal è una funzionalità di Angular che permette di creare variabili reattive
  // , ovvero variabili che quando cambiano aggiornano automaticamente la UI

  readonly Sala = signal<Sala[]>([]);
  readonly staCaricando = signal(false);
  readonly staInviando = signal(false);
  readonly messaggioErrore = signal('');
  readonly messaggioSuccesso = signal('');
  readonly modificaId = signal<string >('');

  // group definisce il gruppo di campi del form ai quali vogliamo applicare una validazione
  readonly form = this.formBuilder.nonNullable.group({
    nome: ['', [Validators.required, Validators.maxLength(100)]],
    capienza: [0, [Validators.required, Validators.min(0)]],
    tipologiaSalaId: ['', [Validators.required]]
  });

  
  constructor() {
    this.caricaSala();

    if (!this.modificabileDa()) {
      this.form.disable();
  }
  }
  modificabileDa(): boolean {
      return this.authService.possiedeQualsiasiRuolo(['Operatore']);
  }
 // metodo per caricare le tipologie di sala dal backend
  caricaSala() : void {
    // set è il metodo che permette di aggiornare il valore di una signal
    this.staCaricando.set(true);

    this.messaggioErrore.set('');

    //subscribe è il medodo che permette di interagire con un Observable,
    // cioè una struttura dati che rappresenta un flusso di dati asincrono,
    //  come ad esempio la risposta di una chiamata HTTP
    this.SalaService.ottieniTutto().subscribe({
      //prende i DTO che arrivano dal backend e li assegna alla signal tipologieSala
      // , in questo modo la UI si aggiorna automaticamente
      next: (items) => {
        this.Sala.set(items);
        this.staCaricando.set(false);

      },
      
      error : (error: unknown) => {
        this.staCaricando.set(false);
        this.messaggioErrore.set(this.estraiMessaggioErrore(error, 'Sale non trovate'));
      }
    });
  }

  invia(): void {
    // Controlla se il form è invalido o se l'utente non ha i permessi per modificare
    if (this.form.invalid || !this.modificabileDa()) {
      this.form.markAllAsTouched();
      return;
    }

    this.staInviando.set(true);
    this.messaggioErrore.set('');
    this.messaggioSuccesso.set('');
    const request$ = this.modificaId()
      ? this.SalaService.modifica(this.modificaId(), this.form.getRawValue())
      : this.SalaService.crea(this.form.getRawValue());

    request$.subscribe({
      next: () => {
        this.staInviando.set(false);
        // Imposta il messaggio di successo in base all'operazione effettuata
        this.messaggioSuccesso.set(this.modificaId() ? 'Sala aggiornata.' : 'Sala creata.');
        this.ripristinaForm();  
        this.caricaSala(); // Rica  rica la lista per mostrare i dati aggiornati
      },
      error: (error: unknown) => {
        console.log(error)
        this.staInviando.set(false);
        this.messaggioErrore.set(this.estraiMessaggioErrore(error, 'Operazione non riuscita.'));
      }
    });
  }

  inizioModifica(item: Sala): void {
    // Verifica i permessi prima di permettere l'inizio della modifica
    if (!this.modificabileDa()) {
      return;
    }
    this.modificaId.set(item.id!);
    // dizionario che rappresenta la mappa del modulo
    this.form.patchValue({ nome: item.nome, capienza: item.capienza,tipologiaSalaId: item.tipologiaSalaId});
    this.messaggioErrore.set('');
    this.messaggioSuccesso.set('');
  }
  
  elimina(item: Sala) : void {
    if(!this.modificabileDa()) {
      return;
    }
    const confirmed = confirm(`Eliminare la sala \" ${item.nome}\"?`);
    if(!confirmed){
      return;
    }

    this.messaggioErrore.set('');
    this.messaggioSuccesso.set('');

    this.SalaService.elimina(item.id!).subscribe({
      next : () => {
        if (this.modificaId() === item.id) {
          this.ripristinaForm();
        }
        this.caricaSala();
      },
      error: (error: unknown) => {
    
      this.messaggioErrore.set(this.estraiMessaggioErrore(error, 'eliminazione non riuscita.'));
      }
    });  
  }

  ripristinaForm() : void {
    this.modificaId.set('');
    this.form.reset({ nome: '', capienza: 0 ,tipologiaSalaId : ''});
  }
  // il _ indica che il primo parametro non viene utilizzato,
  //  è una convenzione per indicare che è presente ma non serve
  tracciaPerId(_: string, item: Sala) : string {
    return item.id!;
  }

  private estraiMessaggioErrore(error: unknown, fallback: string): string {
    
    if (error instanceof HttpErrorResponse) {
      return error.error?.message ?? fallback;
    }
    return fallback;
  }
}
