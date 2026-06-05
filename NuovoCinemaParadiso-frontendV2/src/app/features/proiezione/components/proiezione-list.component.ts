  import { Component, computed, inject, signal } from '@angular/core';
  import { HttpErrorResponse } from '@angular/common/http';
  import { ProiezioneService } from '../../../services/proiezione.service';
  import { AuthService } from '../../../services/auth.service';
  import { Proiezione } from '../../../models/proiezione.model';
  import { BigliettoService } from '../../../services/biglietto.service';
  import { RouterLink } from '@angular/router';
  import { CommonModule } from '@angular/common';
  import { FormsModule } from '@angular/forms';
  import { ProiezioneFormComponent } from "./proiezione-form.component";

  @Component({
    selector: 'proiezione-list',
    standalone: true,
    imports: [RouterLink, CommonModule, FormsModule, ProiezioneFormComponent],
    templateUrl: './proiezione-list.html',
  })
  export class ProiezioneList {

    private readonly proiezioneService = inject(ProiezioneService);
    private readonly authService = inject(AuthService);
    private readonly bigliettoService = inject(BigliettoService);

    
    readonly listaProiezioni = signal<Proiezione[]>([]);
    readonly proiezioneScelta = signal<Proiezione | null>(null);
    readonly staCaricando = signal(false);
    readonly staInviando = signal(false);
    readonly messaggioErrore = signal('');
    readonly messaggioSuccesso = signal('');
    readonly modificaId = signal<string | null>(null);

    readonly isOperatore   = computed(() => this.authService.isOperatore());
    readonly isGestore     = computed(() => this.authService.isGestore());
    readonly isAutenticato = computed(() => this.authService.isAutenticato());


    quantitaSelezionata: Record<string, number> = {};

    readonly loadingMap = signal<Record<string, boolean>>({});

    constructor() {
      this.ottieniTutto();
    }

   
    ottieniTutto(): void {
      this.staCaricando.set(true);
      this.messaggioErrore.set('');

      this.proiezioneService.ottieniTutto().subscribe({
        next: (data) => {
          this.listaProiezioni.set(data);

          const init: Record<string, number> = {};
          data.forEach(p => {
            init[p.id] = 1;
          });
          this.quantitaSelezionata = init;
          this.staCaricando.set(false);
        },
        error: (err) => {
          this.staCaricando.set(false);
          this.messaggioErrore.set(
            this.estraiMessaggioErrore(err, 'Errore caricamento proiezioni')
          );
        }
      });
    }

    
    acquista(proiezioneId: string) {

        const numeroBiglietti = this.quantitaSelezionata[proiezioneId] ?? 1;

      this.loadingMap.update(m => ({
        ...m,
        [proiezioneId]: true
      }));

      this.messaggioErrore.set('');
      this.messaggioSuccesso.set('');

      this.bigliettoService.crea({
        proiezioneId,
        numeroBiglietti
      }).subscribe({
        next: () => {
          this.loadingMap.update(m => ({
            ...m,
            [proiezioneId]: false
          }));

          this.messaggioSuccesso.set('Biglietti acquistati con successo!');
        },
        error: (err) => {
          this.loadingMap.update(m => ({
            ...m,
            [proiezioneId]: false
          }));

          this.messaggioErrore.set(
            this.estraiMessaggioErrore(err, 'Errore creazione biglietto')
          );
        }
      });
    }

    elimina(item: Proiezione): void {
    if (!this.isOperatore()) {
      return;
    }

    const confirmed = confirm(`Eliminare la proiezione\"${item.id}\"?`)

    if (!confirmed) {
      return;
    }

    this.messaggioErrore.set('');
    this.messaggioSuccesso.set('');

    this.proiezioneService.elimina(item.id).subscribe({
      next: () => {
        this.messaggioSuccesso.set('Proiezione eliminata');

        this.ottieniTutto();
      },
      error: (error: unknown) => {
        this.messaggioErrore.set(this.estraiMessaggioErrore(error, 'Eliminazione non riuscita'));

      }
    });
  }

    private estraiMessaggioErrore(error: unknown, fallback: string): string {
      if (error instanceof HttpErrorResponse) {
        return error.error?.message ?? error.error?.messaggio ?? fallback;
      }

      return fallback;
    }
  }