import { Component, inject, signal } from '@angular/core';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { HttpErrorResponse } from '@angular/common/http';
import { AuthService } from '../../services/auth.service';
import { TipologiaSalaService } from '../../services/tipologia-sala.service';
// path del modello TipologiaSala, da adattare in base alla struttura del progetto
import { TipologiaSala } from '../../models/tipologia-sala.model';


@Component({
  //corrisponde al tag HTML che rappresenta questa pagina
  selector: 'tipologia-sala-page',
  // indica che abbiamo deciso di creare una app standalone, senza modules 
  standalone: true,
  // importiamo ReactiveFormsModule per poter utilizzare i form reattivi nella nostra pagina
  // i form reattivi possono adattarsi a diversi comportamenti
  imports: [ReactiveFormsModule],
  // url del template HTML associato a questa pagina
  templateUrl: './tipologia-sala.page.html'
})
// export prepara la classe per essere utilizzata in altri file
export class TipologiaSalaPage {
  // il form builder è un servizio che ci permette di creare form reattivi 
  // in modo semplice e strutturato
  private readonly formBuilder = inject(FormBuilder);
  private readonly authService = inject(AuthService);
  private readonly tipologiaSalaService = inject(TipologiaSalaService);

  //devono essere redonly perché non devono essere modificati dopo l'inizializzazione
  // signal è una funzionalità di Angular che permette di creare variabili reattive
  // , ovvero variabili che quando cambiano aggiornano automaticamente la UI

  readonly tipologieSala = signal<TipologiaSala[]>([]);
  readonly staCaricando = signal(false);
  readonly staInviando = signal(false);
  readonly messaggioErrore = signal('');
  readonly messaggioSuccesso = signal('');
  readonly modificaId = signal<string >('');

  // group definisce il gruppo di campi del form ai quali vogliamo applicare una validazione
  readonly form = this.formBuilder.nonNullable.group({
    nome: ['', [Validators.required, Validators.maxLength(100)]],
    maggiorazionePrezzo: [0, [Validators.required, Validators.min(0)]]
  });

   
  constructor() {
    this.caricaTipologieSala();
  }
  modificabileDa(): boolean {
      return this.authService.possiedeQualsiasiRuolo(['Gestore','Operatore']);
  }
 // metodo per caricare le tipologie di sala dal backend
  caricaTipologieSala() : void {
    // set è il metodo che permette di aggiornare il valore di una signal
    this.staCaricando.set(true);

    this.messaggioErrore.set('');

    //subscribe è il medodo che permette di interagire con un Observable,
    // cioè una struttura dati che rappresenta un flusso di dati asincrono,
    //  come ad esempio la risposta di una chiamata HTTP
    this.tipologiaSalaService.ottieniTutto().subscribe({
      //prende i DTO che arrivano dal backend e li assegna alla signal tipologieSala
      // , in questo modo la UI si aggiorna automaticamente
      next: (items) => {
        this.tipologieSala.set(items);
        this.staCaricando.set(false);

      },
      
      error : (error: unknown) => {
        this.staCaricando.set(false);
        this.messaggioErrore.set(this.estraiMessaggioErrore(error, 'tipologie non trovate'));
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

    // Determina se eseguire una update (se modificandoId è presente) o una create
    // costante con tipo di dato anonimo che rappresenta la richiesta da inviare al servizio
    // ler parentesi servono per restituirci il dato manipolato
    const request$ = this.modificaId()
    // operatore ternario che restituisce quello alla sinistra dei : 
    // se modificandoId è valido, altrimenti restituisce quello alla destra dei :
      ? this.tipologiaSalaService.modifica(this.modificaId(), this.form.getRawValue())
      : this.tipologiaSalaService.crea(this.form.getRawValue());
    /*
    versione con if-else:
    let request$: Observable<TipologiaSala>;
    if (this.modificandoId()) {
      request$ = this.tipologiaSalaService.modifica(this.modificandoId()!, this.form.getRawValue());
    } else {
      request$ = this.tipologiaSalaService.crea(this.form.getRawValue());
    } 
    */ 

    request$.subscribe({
      next: () => {
        this.staInviando.set(false);
        // Imposta il messaggio di successo in base all'operazione effettuata
        this.messaggioSuccesso.set(this.modificaId() ? 'Tipologia aggiornata.' : 'Tipologia creata.');
        this.ripristinaForm();
        this.caricaTipologieSala(); // Ricarica la lista per mostrare i dati aggiornati
      },
      error: (error: unknown) => {
        this.staInviando.set(false);
        this.messaggioErrore.set(this.estraiMessaggioErrore(error, 'Operazione non riuscita.'));
      }
    });
  }

  inizioModifica(item: TipologiaSala): void {
    // Verifica i permessi prima di permettere l'inizio della modifica
    if (!this.modificabileDa()) {
      return;
    }
    this.modificaId.set(item.id);
    // dizionario che rappresenta la mappa del modulo
    this.form.patchValue({ nome: item.nome, maggiorazionePrezzo: item.maggiorazionePrezzo });
    this.messaggioErrore.set('');
    this.messaggioSuccesso.set('');
  }
  
  elimina(item: TipologiaSala) : void {
    if(!this.modificabileDa()) {
      return;
    }
    const confirmed = confirm(`Eliminare la tipologia \" ${item.nome}\"?`);
    if(!confirmed){
      return;
    }

    this.messaggioErrore.set('');
    this.messaggioSuccesso.set('');

    this.tipologiaSalaService.elimina(item.id).subscribe({
      next : () => {
        if (this.modificaId() === item.id) {
          this.ripristinaForm();
        }
        this.caricaTipologieSala();
      },
      error: (error: unknown) => {
       
      this.messaggioErrore.set(this.estraiMessaggioErrore(error, 'eliminazione non riuscita.'));
      }
    });  
  }

  ripristinaForm() : void {
    this.modificaId.set('');
    this.form.reset({ nome: '', maggiorazionePrezzo: 0 });
  }
  // il _ indica che il primo parametro non viene utilizzato,
  //  è una convenzione per indicare che è presente ma non serve
  tracciaPerId(_: string, item: TipologiaSala) : string {
    return item.id;
  }

  private estraiMessaggioErrore(error: unknown, fallback: string): string {
    
    if (error instanceof HttpErrorResponse) {
      return error.error?.message ?? fallback;
    }
    return fallback;
  }
}
