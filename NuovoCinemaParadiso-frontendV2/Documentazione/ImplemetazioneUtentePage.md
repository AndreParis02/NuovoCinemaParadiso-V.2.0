
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

### utente.page.html V1.0

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