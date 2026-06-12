<details>
<summary>versione1.2</summary>

Francesco Lorenzi 12/06/2026

aggiunta un campo stringa per registrare la tipologia di abbonamento

```ts
export interface SessioneUtente {
    id: string;
    nomeCompleto: string;
    token: string;
    eta: number;
    email: string;
    ruolo: string;
    abbonamento: string | null; //aggiunta una stringa per registrare la tipologia di abbonamento
    dataInizioAbbonamento: string;
    seAbbonato: boolean;
}

```
</details>