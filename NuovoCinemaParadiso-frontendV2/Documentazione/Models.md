# MODELS

## abbonamento.model.ts

<details>
<summary>abbonamento.model.ts V1.0</summary>

Francesco Lorenzi 26/05/2026
```ts

export interface Abbonamento {
    id: string; //la chiave primaria non può essere null
    nome: string;
    durata: number;
    prezzo: number;
    sconto: number;
}

export interface AbbonamentoCreazione {
    nome: string;
    durata: number;
    prezzo: number;
    sconto: number;
} 
```
</details>

## auth.model.ts
<details>
<summary> auth.model.ts V1.0</summary>

```ts
// modello per la creazione della sessione dell'utente
export interface Auth {
  token: string;
  id: string;
  email: string;
  nomeCompleto: string;
  ruolo: string;
}
```
</details>

## biglietto.model.ts

<details><summary> biglietto.model.ts V1.0 </summary>

```ts
// modello di ritorno che rappresenta un Biglietto
export interface Biglietto {
  id: string | null;
  proiezioneId: string;
  utenteId: string;
  prezzoFinale: number;
  orarioCreazione: string;
  numeroBiglietti: number;
  metodoPagamento: string;
}
// modello di inserimento necessario per la creazione o la modifica di un modello Biglietto
export interface BigliettoCreazione {
  proiezioneId: string;
  numeroBiglietti: number;
  metodoPagamento: string;
}
```
</details>

## genere-movie.model.ts

<details><summary> genere-movie.model V1.0</summary>

```ts
// modello di ritorno che rappresenta un Genere
export interface GenereMovie {
  id: string;
  genere: string;
}
// modello di inserimento necessario per la creazione o la modifica di un modello Genere
export interface GenereMovieCreazione {
  genere: string;
}
```
</details>

### Aggiornamento Codice

Utente: Marco Strazzeri
Data: 25/05/2026
Descrizione: Rimosso GenereMovieCreazione perché non creiamo generi

<details><summary> genere-movie.model V1.1</summary>

```ts
// modello di ritorno che rappresenta un Genere
export interface GenereMovie {
  id: string | null;
  genere: string;
}

```
</details>

## gestione.model.ts

<details><summary> gestione.model.ts </summary>
- Utente: Simeone
- Data: 28/05/2026
- Descrizione: creazione modello per pagina di gestione dedita al ruolo gestore

```ts
export interface LogAzioni {
  id: string | null;
  idUtente: string | null;
  nomeAzione: string;
  effettuato: boolean;
  messaggio: string;
  timeStamp: string;
}

export interface LogAzioniCreazione {
    id: string | null;
    idUtente: string;
    nomeAzione: string;
    effettuato: boolean;
    messaggio: string;
    timeStamp: string;
}

export interface ContoCinema {
  id: string;
  iban: string;
  titolareConto: string;
  saldo: number;
}

export interface Biglietto {
  id: string | null;
  utenteId: string;
  prezzoFinale: number;
  orarioCreazione: string;
  proiezioneId: string;
  numeroBiglietti: number;
  nomeSala: string;
  titoloMovie: string;
  nomeTipologiaSala: string;
  oraInizio: string; 
  dataProiezione: string; 
}

export interface GiftCard {
  id: string | null;
  nome: string;
  valore: number;
  codiceRiscatto: string;
}
```
</details>

## giftcard.model.ts

<details><summary> giftcard.model V1.0 </summary>

```ts
// modello di ritorno che rappresenta una GiftCard

export interface GiftCard {
  id: string | null;
  nome: string;
  durata: number;
  prezzo: number;
  numeroMovie: number;
}
// modello di inserimento necessario per la creazione o la modifica di un modello GiftCard
export interface GiftCardCreazione {
  nome: string;
  durata: number;
  prezzo: number;
  numeroMovie: number;
}
```
</details >

### Aggiornamento Codice

Andrea Bruno 28-05-2026
Aggiunta modello CodiceRiscatto

<details><summary> giftcard.model V1.1 </summary>

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

## logAzioni.model.ts

<details><summary> logAzioni.model </summary>

```ts
// modello di ritorno che rappresenta una Log di un'azione di un utente
export interface LogAzioni {
  id: string | null;
  idUtente: string | null;
  nomeAzione: string;
  effettuato: boolean;
  messaggio: string;
  timeStamp: string;
}
// modello di inserimento necessario per la creazione dei Log delle azioni degli utenti
export interface LogAzioniCreazione {
  id: string | null;
  idUtente: string;
  nomeAzione: string;
  effettuato: boolean;
  messaggio: string;
  timeStamp: string;
}
```
</details>

## login.model.ts
<details><summary> login.model V1.0 </summary>

```ts
//modello necessario per confermare i dati d'accesso dell'utente e ricevere il token
export interface Login {
  email: string;
  password: string;
}
```
</details>

## movie.model.ts
<details><summary> movie.model V1.0 </summary>

```ts
// modello di ritorno che rappresenta un Movie (film)
export interface Movie {
  id: string | null;
  titolo: string;
  descrizione: string;
  durataMinuti: number;
  prezzoMovie: number;
  genereId: string;
  genere: string;
}
// modello di inserimento necessario per la creazione o la modifica di un Movie (film)
export interface MovieCreazione {
  titolo: string;
  descrizione: string;
  durataMinuti: number;
  prezzoMovie: number;
  genereId: string;
}
```
</details >


### Aggiornamento Codice

Francesco Lorenzi
25/05/2026
ho dovuto modificare il campo id perchè giustamente questo non dovrebbe essere nullabile

<details><summary>movie.model V1.1 </summary>

```ts
export interface Movie {
  id: string; //la chiave primaria non può essere null
  titolo: string;
  descrizione: string;
  durataMinuti: number;
  prezzoMovie: number;
  genereId: string;
  genere: string;
}

export interface MovieCreazione {
  titolo: string;
  descrizione: string;
  durataMinuti: number;
  prezzoMovie: number;
  genereId: string;
}
```
</details>

## proiezione.model.ts
<details>
<summary> proiezione.model V1.0 </summary>

```ts
// modello di ritorno che rappresenta una Proiezione
export interface Proiezione {
  id: string;
  dataProiezione: Date;
  movieId: string;
  salaId: string;
  turnoId: string;
}
// modello di inserimento necessario per la creazione o la modifica di una Proiezione
export interface ProiezioneCreazione {
  movieId: string;
  salaId: string;
  turnoId: string;
  dataProiezione: Date;
}
```
</details>

### Aggiornamento Codice

Utente: Fabio Tammaro
Data: 03/06/2026
Descrizione: aggiunte proprietà necessarie alla visualizzazione nella lista delle proiezioni.
<details><summary> proiezione.model V1.1 </summary>

```ts
export interface Proiezione {
    id: string;
    dataProiezione: Date;
    movieId: string;
    titoloMovie: string;
    salaId: string;
    nomeSala: string;
    turnoId: string;
    nomeTurno: string;
}

export interface ProiezioneCreazione {
    movieId: string;
    salaId: string;
    turnoId: string;
    dataProiezione: string;
}
```
</details>

## registrazione.model.ts
<details><summary> registrazione.model V1.0 </summary>

```ts
// modello di inserimento necessario per la creazione di un nuovo Utente
export interface Registrazione {
  email: string;
  password: string;
  nomeCompleto: string;
  eta: number;
}
```
</details>

## sala.model.ts
<details>
<summary> sala.model V1.0 </summary>

```ts
// modello di ritorno che rappresenta una Sala
export interface SalaCreazione {
  nome: string;
  capienza: number;
  tipologiaSalaId: string;
}
// modello di inserimento necessario per la creazione o la modifica di una Sala
export interface Sala {
  id: string | null;
  nome: string;
  capienza: number;
  tipologiaSalaId: string;
  nomeTipologia: string;
}
```
</details>

### Aggiornamento Codice

<details><summary> sala.model V1.1 </summary>

```ts
export interface SalaCreazione {
  nome: string;
  capienza: number;
  tipologiaSalaId: string;
}

export interface Sala {
  id: string; //la chiave primaria non può essere nullabile
  nome: string;
  capienza: number;
  tipologiaSalaId: string;
  nomeTipologia: string;
}
```
</details>

## sessione-utente.model.ts
<details>
<summary> sessione-utente.model V1.0 </summary>

```ts
// modello per la gestione della sessione dell'utente
export interface SessioneUtente {
  id: string;
  nomeCompleto: string;
  token: string;
  eta: number;
  email: string;
  ruolo: string;
  dataInizioAbbonamento: string;
  dataInizioGiftCard: string;
  seAbbonato: boolean;
  possiedeGiftCard: boolean;
}
```
</details>

## tipologia-sala.model.ts
<details>
<summary> tipologia-sala.model V1.0 </summary>

```ts
// modello di ritorno che rappresenta una Tipologia di una Sala
export interface TipologiaSala {
  id: string;
  nome: string;
  maggiorazionePrezzo: number;
}

// modello di inserimento necessario per la creazione o la modifica di una Tipologia Sala
export interface TipologiaSalaCreazione {
  nome: string;
  maggiorazionePrezzo: number;
}
```
</details>

## turno.model.ts
<details>
<summary> turno.model V1.0 </summary>

```ts
// modello di ritorno che rappresenta un Turno
export interface Turno {
  id: string | null;
  oraInizio: string;
  oraFine: string;
  nome: string;
}
// modello di inserimento necessario per la creazione o la modifica di un Turno
export interface TurnoCreazione {
  oraInizio: string;
  oraFine: string;
  nome: string;
}
```
</details>

### Aggiornamento Codice

<details><summary> turno.model V1.1 </summary>

```ts

export interface Turno {
    id: string;// la chiave primario non può essere nullabile
    oraInizio: string;
    oraFine: string;
    nome: string;
}
export interface TurnoCreazione {
    oraInizio: string;
    oraFine: string;
    nome: string;
}
```
</details>

## utente.model.ts
<details>
<summary> utente.model V1.0 </summary>

```ts
// Modello di ritrono che rappresenta un Utente
export interface Utente {
  id: string;
  nomeCompleto: string;
  dataInizioAbbonamento: string;
  dataInizioGiftCard: string;
  seAbbonato: boolean;
  possiedeGiftCard: boolean;
  email: string;
  eta: number;
  abbonamentoId: string;
  giftCardId: string;
  tipoAbbonamento: string;
  tipoGiftCard: string;
}

// modello necessario per la modifica di un Utente
export interface UtenteCreazione {
  nomeCompleto: string;
  eta: number;
}

// modello necessario per la modifica del ruolo di un Utente
export interface UtenteModificaRuolo {
  email: string;
  nuovoRuolo: string;
}
```
</details>

### Aggiornamento Codice

<details><summary> utente.model V1.1 </summary>

```ts
export interface Utente {
    id: string;
    nomeCompleto: string;
    dataInizioAbbonamento: string;
    seAbbonato: boolean;
    email: string;
    eta: number;
    abbonamentoId: string;
    tipoAbbonamento: string;
    Saldo: number;
}

export interface UtenteCreazione {
    nomeCompleto: string;
    eta: number;
}

export interface UtenteModificaRuoloRichiesta {
    email: string;
    nuovoRuolo: string;
}
// aggiunta UtenteModificaRuoloRisposta
export interface UtenteModificaRuoloRisposta {
    messaggio: string;
    email: string;
    ruolo: string;
}
```
</details>

