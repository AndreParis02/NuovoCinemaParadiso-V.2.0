TASK AGGIUNTIVO monetario (Andrea Paris):

Verificare che tutta la parte di ricarica, riscatta (giftcard), rimborso, acquisto (biglietto), saldo.

TASK AGGIUNTIVO 2  eliminazioni (Greg):

Verificare che venga attuata la procedura corretta o di eliminazione o di disattivazione dell'entità.

TASK a bassa priorità 3:

Creare una nuova entità "UtenteAbbonamento", correggere di conseguenza la tabella utenti. 


Task saldo aggiurnato in tempo reale nella navbar:


biglietto, giftcard, abbonamento

creare service condiviso che abbia nella export class navbarStateService

conterrà il saldo



teniamo conto per le modifiche legate al credito quindi,

il service condiviso mantiene lo stato della navbar

navbar component importiamo il service condiviso (NavbarStateService)

export class navbarComponent

aggiungere OnInit


