
# Backend

## Utente.cs 

Andrea Bruno 28-05-2026
Aggiunta stringa TipologiaAbbonamento

```c#
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NuovoCinemaParadiso.Models;

[Table("Utenti")]
public class Utente : IdentityUser
{
    [Required]
    [StringLength(100)]
    public string NomeCompleto { get; set; } = string.Empty;
    [Required]
    [Range(14, 100, ErrorMessage = "L'età deve essere compresa tra 14 e 100")]
    public int Eta { get; set; }
    [Required]
    public bool SeAbbonato { get; set; } = false;
    [Required]
    public DateTimeOffset DataInizioAbbonamento { get; set; }
    public List<Biglietto> Biglietti { get; set; } = new List<Biglietto>();
    public string? AbbonamentoId { get; set; }
    [ForeignKey("AbbonamentoId")]
    public Abbonamento? Abbonamento { get; set; }

    public string TipologiaAbbonamento { get; set; } = string.Empty;     //  <-- AGGIUNTA

    [Range(0,10000)]
    public int Saldo {get;set;}
}
```


# Frontend

## giftCard.model.ts 

Andrea Bruno 28-05-2026
Aggiunta modello CodiceRiscatto 

```c#
export interface GiftCard {
    id: string | null;
    nome: string;
    valore: number;
    codiceRiscatto: string;
}

export interface GiftCardCreazione {
    nome: string;
    valore: number;
    codiceRiscatto: string;
}

export interface RicaricaGiftCard {
    importo: number;
}

export interface CodiceRiscatto {    // <-- AGGIUNTA
    CodiceRiscatto: string;
}
```

## utente.service.ts 

Andrea Bruno 28-05-2026
Cambio di rotta da Utente a Auth 
Modifiche nel richiamo dei servizi 

```c#
import { environment } from '../../environments/environment';
import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Utente, UtenteCreazione } from '../models/utente.model';

@Injectable({
    providedIn: 'root'
})
export class UtenteService {

    // HttpClient viene iniettato tramite inject() (metodo moderno Angular)
    private readonly http = inject(HttpClient);

    // Base URL dell'API: punta al controller Auth del backend
    // /Auth è corretto perché il controller è AuthController
    private readonly baseUrl = `${environment.apiBaseUrl}/Auth`;

    // Recupera il profilo dell'utente loggato
    // Ritorna un Observable<Utente> che il componente sottoscrive
    profilo(): Observable<Utente> {
        return this.http.get<Utente>(`${this.baseUrl}/profilo`);
    }

    // Modifica i dati dell'utente (nome + età)
    // Usa PUT perché aggiorna risorse esistenti
    modifica(payload: UtenteCreazione): Observable<any> {
        return this.http.put<any>(`${this.baseUrl}/modifica`, payload);
    }

    // Elimina definitivamente l'account dell'utente
    // Il backend gestisce la rimozione e la logout viene fatta nel componente
    eliminaProfilo(): Observable<any> {
        return this.http.delete<any>(`${this.baseUrl}/elimina`);
    }
}
```

## utente.page.ts 

Andrea Bruno 28-05-2026
Creazione della pagina dell'area riservata dell'utente

```c#
import { Component, inject, signal, OnInit } from '@angular/core';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { HttpErrorResponse } from '@angular/common/http';
import { DatePipe } from '@angular/common';
import { UtenteService } from '../../services/utente.service';
import { AuthService } from '../../services/auth.service';
import { Utente } from '../../models/utente.model';

@Component({
  selector: 'utente-page',
  standalone: true, // Componente standalone (senza modulo)
  imports: [
    ReactiveFormsModule, // Necessario per i form reattivi
    DatePipe             // Per formattare date nel template
  ],
  templateUrl: './utente.page.html',
})
export class UtentePage implements OnInit {

  // Iniezione dei servizi tramite inject() (stile Angular moderno)
  private readonly formBuilder = inject(FormBuilder);
  private readonly utenteService = inject(UtenteService);
  private readonly authService = inject(AuthService);

  // Signals: stato reattivo del componente
  readonly utente = signal<Utente | null>(null); // Dati dell’utente
  readonly staCaricando = signal(false);         // Stato caricamento profilo
  readonly staInviando = signal(false);          // Stato invio form
  readonly messaggioErrore = signal('');         // Messaggi di errore
  readonly messaggioSuccesso = signal('');       // Messaggi di successo

  // Form reattivo con validazioni
  readonly form = this.formBuilder.nonNullable.group({
    nomeCompleto: ['', [Validators.required, Validators.maxLength(100)]],
    eta: [0, [Validators.required, Validators.min(14), Validators.max(100)]]
  });

  // Angular chiama questo metodo quando il componente viene inizializzato
  ngOnInit(): void {
    this.caricaUtente(); // Carica i dati del profilo all’avvio
  }
  
  // Controlla se l’utente ha un ruolo che permette modifiche limitate
  modificabileDa(): boolean {
    return this.authService.possiedeQualsiasiRuolo(['Operatore']);
  }

  // Recupera i dati del profilo dal backend
  caricaUtente(): void {
    this.staCaricando.set(true);
    this.messaggioErrore.set('');

    this.utenteService.profilo().subscribe({
      next: (item) => {
        this.utente.set(item); // Salva i dati nel signal

        // Precompila il form con i dati ricevuti
        this.form.patchValue({
          nomeCompleto: item.nomeCompleto,
          eta: item.eta
        });

        this.staCaricando.set(false);
      },
      error: (error) => {
        this.staCaricando.set(false);
        this.messaggioErrore.set(
          this.estraiMessaggioErrore(error, 'Accedi per vedere il profilo')
        );
      }
    });
  }

  // Invia al backend le modifiche del profilo
  invia(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched(); // Mostra errori di validazione
      return;
    }

    this.staInviando.set(true);
    this.messaggioErrore.set('');
    this.messaggioSuccesso.set('');

    this.utenteService.modifica(this.form.getRawValue()).subscribe({
      next: () => {
        this.staInviando.set(false);
        this.messaggioSuccesso.set('Profilo aggiornato');

        // Ricarica i dati aggiornati
        this.caricaUtente();
      },
      error: (error) => {
        this.staInviando.set(false);
        this.messaggioErrore.set(
          this.estraiMessaggioErrore(error, 'Modifica non riuscita.')
        );
      }
    });
  }

  // Elimina definitivamente l’account dell’utente
  elimina(): void {
    const confirmed = confirm('Vuoi davvero eliminare il tuo account?');
    if (!confirmed) return;

    this.messaggioErrore.set('');
    this.messaggioSuccesso.set('');

    this.utenteService.eliminaProfilo().subscribe({
      next: () => {
        this.authService.logout(); // Rimuove token e sessione
        this.messaggioSuccesso.set('Account eliminato');

        // Redirect manuale alla pagina di login
        window.location.href = '/login';
      },
      error: (error) => {
        this.messaggioErrore.set(
          this.estraiMessaggioErrore(error, 'Eliminazione non riuscita.')
        );
      }
    });
  }

  // Estrae un messaggio leggibile dagli errori HTTP
  private estraiMessaggioErrore(error: unknown, fallback: string): string {
    if (error instanceof HttpErrorResponse) {
      return error.error?.message ?? fallback;
    }
    return fallback;
  }
}
```

## utente.page.html

Andrea Bruno 28-05-2026
Creazione pagina html dell'area riservata dell'utente
Se l'utente non è abbonato non viene visualizzata la card e 
ovviamento vale anche per operatore e gestore  

```c#
<section>

    <!-- HEADER: mostrato solo se l'utente è stato caricato -->
    @if (utente()) {
        <!-- Titolo principale con il nome dell'utente -->
        <h1 class="page-title">Ciao, {{ utente()?.nomeCompleto }}</h1>

        <!-- Sottotitolo descrittivo -->
        <p class="page-subtitle">Gestisci le informazioni del tuo account.</p>
    }

    <!-- MESSAGGI DI ERRORE E SUCCESSO -->
    @if (messaggioErrore()) {
        <div class="alert alert-warning">{{ messaggioErrore() }}</div>
    }

    @if (messaggioSuccesso()) {
        <div class="alert alert-success">{{ messaggioSuccesso() }}</div>
    }

    <!-- STATO DI CARICAMENTO -->
    @if (staCaricando()) {
        <p class="muted">Caricamento profilo...</p>

    <!-- NESSUN UTENTE TROVATO -->
    } @else if (!utente()) {
        <p class="muted">Nessun profilo trovato.</p>

    <!-- CONTENUTO PRINCIPALE -->
    } @else {

    <!-- GRID CON DUE CARD AFFIANCATE -->
    <div class="grid grid-2" style="margin-top: 1.5rem;">

        <!-- CARD SINISTRA: INFORMAZIONI UTENTE -->
        <article class="card">
            <h2>Informazioni Utente</h2>

            <div class="list">

                <!-- Email -->
                <div class="list-item">
                    <strong>Email</strong>
                    <p>{{ utente()?.email }}</p>
                </div>

                <!-- Età -->
                <div class="list-item">
                    <strong>Età</strong>
                    <p>{{ utente()?.eta }}</p>
                </div>

                <!-- Saldo: mostrato solo ai clienti (non operatori) -->
                @if (!modificabileDa()) {
                    <div class="list-item">
                        <strong>Saldo</strong>
                        <p>{{ utente()?.saldo }} €</p>
                    </div>
                }
            </div>
        </article>

        <!-- CARD DESTRA: MODIFICA + ELIMINAZIONE -->
        <article class="card">
            <h2>Modifica Profilo</h2>

            <!-- Form reattivo Angular -->
            <form class="form-grid" [formGroup]="form" (ngSubmit)="invia()">

                <!-- Campi del form -->
                <div>
                    <label for="nomeCompleto">Nome completo</label>
                    <input id="nomeCompleto" type="text" formControlName="nomeCompleto">

                    <label for="eta">Età</label>
                    <input id="eta" type="number" formControlName="eta">
                </div>

                <!-- Pulsanti: aggiorna + elimina -->
                <div class="btn-row">

                    <!-- Pulsante AGGIORNA -->
                    <button class="btn btn-primary" type="submit" [disabled]="staInviando()">
                        {{ staInviando() ? 'Salvataggio...' : 'Aggiorna' }}
                    </button>

                    <!-- Pulsante ELIMINA ACCOUNT (rosso pieno) -->
                    <button class="btn btn-danger-solid" type="button" (click)="elimina()">
                        Elimina account
                    </button>
                </div>

            </form>
        </article>

    </div>

    <!-- CARD ABBONAMENTO (solo per clienti e solo se abbonati) -->
    @if (!modificabileDa()) {
        @if (utente()?.seAbbonato) {

            <article class="card" style="margin-top: 2rem;">
                <h2>Abbonamento</h2>

                <!-- Riga orizzontale con i dati dell'abbonamento -->
                <div class="abbonamento-row">

                    <!-- Stato -->
                    <div>
                        <strong>Stato:</strong>
                        {{ utente()?.seAbbonato ? 'Attivo' : 'Non attivo' }}
                    </div>

                    <!-- Data inizio abbonamento -->
                    <div>
                        <strong>Inizio:</strong>
                        {{ utente()?.dataInizioAbbonamento | date }}
                    </div>

                    <!-- Tipo abbonamento -->
                    <div>
                        <strong>Tipo:</strong>
                        {{ utente()?.tipoAbbonamento }}
                    </div>

                </div>
            </article>

        }
    }

    } <!-- Fine else principale -->

</section>
```