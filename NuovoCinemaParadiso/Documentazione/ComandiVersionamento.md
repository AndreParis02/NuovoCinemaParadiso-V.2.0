# Esercitazione pratica: Git branching, PR, risoluzione conflitti

Lead (A), Dev1 (B), Dev2 (C).

Repo già creato su GitHub con branch main (stabile) e developer (integrazione).

**Regola base:** mai lavorare su main; ciascuno lavora su feature branch e fa PR verso developer.

- Il lead (A) crea il repo e i branch, assegna i ruoli.
- Dev1 (B) e Dev2 (C) fanno PR e si scambiano i ruoli di reviewer.
- Il reviewer sarebbe il “secondo paio di occhi” che controlla la PR prima del merge.

# Passi iniziali Lead (A)

- 1 Crea il repo su GitHub (es. cs-playground).
- 2 Clona il repo in locale.
- 3 Crea e pubblica il branch developer `git checkout -b developer && git push -u origin developer`.
- 4 Assegna i ruoli (A=lead, B=dev1, C=dev2).
- 5 Crea il file GitIgnore (es. VisualStudio, Node, Python).
- 6 Crea il file README.md con istruzioni base (es. questa guida).

Verifica i branch che appaiono su GitHub `git branch -a`. I branch locali sono in verde, quelli remoti in rosso.

# Passi per tutti (A, B, C)

- 1 Allineati al branch developer prima di iniziare `git checkout developer && git pull --ff-only` in modo da portare il branch locale alla stessa versione di quello remoto (evita conflitti).
- 2 Crea un feature branch per ogni task (da developer) 'git checkout -b feature/<breve-descrizione>'.
- 3 Fai commit e push sul feature branch. 'git add <file>, git commit -m "...", git push -u origin feature/<...>'.
- 4 Apri PR verso developer, assegna reviewer.
- 5 Il reviewer approva la PR (o chiede modifiche).
- 6 Il lead (A) fa il merge della PR su developer.

**Se il branch non si dovesse vedere basta fare checkout su developer e poi tornare di nuovo al branch.**

- **git clone:** scarica il repository.
- **git checkout developer:** passa al branch developer.
- **git pull --ff-only:** aggiorna il branch solo se può “avanzare” senza creare merge automatici in modo da mantenere la storia lineare. Senza l'attributo --ff-only, git potrebbe creare un commit di merge automatico che complica la storia.

# **Caso 1 — (PR semplice, senza conflitti)**

B:
```bash
git checkout developer                    # passa a developer
git pull --ff-only                      # aggiorna developer locale basandoti sul remoto
git checkout -b feature/nome-b            # crea e passa al branch
echo "B" >> README.md                   # aggiungi il tuo nome
git add README.md
git commit -m "feat: aggiunge B ai partecipanti"
git push -u origin feature/nome-b
```

Su GitHub: apri Pull Request verso developer e assegna come reviewer C.
C: approva la PR. A: fa il merge.

Adesso si può eliminare il branch con il comando:
```bash
git branch -d feature/nome-b          # elimina locale (solo se mergiato)
git push origin --delete feature/nome-b  # elimina su remoto
```
pulisci la cache locale e remoto con il comando:
```bash
git fetch --prune
```
fare il merge diretto su GitHub (senza CLI) va bene per PR semplici come questa con il comando:
```bash
git merge --no-ff feature/nome-b
```
Ripeti per C (reviewer: B).

-c: crea un nuovo branch.

-m "...": messaggio del commit.

-u origin feature/...: collega il branch locale a quello remoto.

# **Caso 2 — Tenersi aggiornati (evitare “pestoni”)**

**Regola d’oro prima di iniziare un task (tutti):**
```bash
git fetch --all     # scarica info su tutti i branch remoti (non modifica i tuoi file). 
git checkout developer
git pull --ff-only
git checkout -b feature/<breve-descrizione>
```
Le info riguardano i commit fatti da altri.

git fetch --all: scarica info su tutti i branch remoti (non modifica i tuoi file).

> Crea sempre la feature partendo da developer aggiornato.

# **Caso 3 — Due modifiche in parallelo (nessun conflitto)**

B (nuova sezione nel README):
```bash
git checkout developer && git pull --ff-only
git checkout -b feature/readme-sezione-b
# modifica una NUOVA sezione/linea (non toccare le stesse righe di C)
git add README.md
git commit -m "docs: aggiunge sezione B"
git push -u origin feature/readme-sezione-b
```

- Apri PR verso developer.
- C fa lo stesso su righe diverse.
- A fa il merge di entrambe.

# **Caso 4 — Conflitto “pilotato” e risoluzione semplice**

Generiamo il conflitto: B e C cambiano la stessa riga in Program.cs.

B:
```bash
git checkout developer && git pull --ff-only
git checkout -b feature/messaggio-b
# cambia Console.WriteLine("Hello") in Console.WriteLine("Hello from B")
git add src/Hello/Program.cs
git commit -m "feat: messaggio B"
git push -u origin feature/messaggio-b
# Apri PR e fai merge su developer
```

C:
```bash
git checkout developer && git pull --ff-only
git checkout -b feature/messaggio-c
# cambia la STESSA riga in "Hello from C"
git add src/Hello/Program.cs
git commit -m "feat: messaggio C"
git push -u origin feature/messaggio-c
# Apri PR: GitHub segnala conflitto
```

C risolve il conflitto in locale:

```bash
git checkout feature/messaggio-c
git fetch
git merge origin/developer   # porta dentro le modifiche già mergiate (di B)
# apri il file, risolvi i marcatori <<<<<<< ======= >>>>>> scegliendo il testo finale
git add src/Hello/Program.cs
git commit -m "chore: risolto conflitto su Program.cs"
git push
```

- Torna alla PR di C: ora è “mergeable”. A approva e merge.
- merge origin/developer: fonde nel tuo branch le novità di developer.

> Marcatori conflitto <<<<<<<, =======, >>>>>>>: parti da scegliere.

# **Caso 5 — Rinominare ed eliminare branch**

B ha un branch chiamato feature/hello-logging, vuole rinominarlo in feature/hello-logs:
```bash
git checkout feature/hello-logging
git branch -m feature/hello-logs          # rinomina locale
git push origin :feature/hello-logging    # elimina il vecchio nome remoto
git push -u origin feature/hello-logs     # pubblica il nuovo
```

Dopo il merge su developer, elimina i rami vecchi:
```bash
git branch -d feature/hello-logs          # elimina locale (solo se mergiato)
git push origin --delete feature/hello-logs
```

- branch -m: rename.
- branch -d: delete locale (sicuro, rifiuta se non mergiato).
- --delete: elimina su remoto.

# **Caso 6 — Revert di un commit sbagliato (sicuro)**

Si può eliminare l ultimo commit sul branch locale (non ancora pubblicato) con:
```bash
git checkout feature/<...>
git log --oneline               # copia lo SHA del commit da annullare
git reset --hard <SHA>          # riporta il branch a quel commit, cancellando tutto il resto
```
Veloce e pulito se si lavora da soli su un branch (es. feature).

- Si può fare il revert di un commit giusto (es. bugfix) se si scopre che ha introdotto un problema.
- Il vantaggio e che non si perde la storia dei commit.
- Questo è utile se il commit è già stato mergiato su developer o main.

A annulla l’ultimo commit “rotto” già mergiato su developer:
```bash
git checkout developer
git pull --ff-only
git log --oneline               # copia lo SHA del commit da annullare
git revert <SHA>
git push
```

**revert:** crea un nuovo commit che annulla quello indicato (storia pulita, niente rischi).

In alternativa si può fare il reset (pericoloso)
```bash
git checkout developer
git pull --ff-only
git log --oneline               # copia lo SHA del commit PRIMA di quello da annullare
git reset --hard <SHA>          # riporta developer a quel commit, cancellando tutto il resto
git push --force                # forza l’aggiornamento del remoto (pericoloso)
```
È pericoloso perchè riscrive la storia del branch e può cancellare il lavoro di altri se fatto su branch condivisi (developer, main).

> Nota: non usare reset --hard su branch condivisi (rischio di perdita lavoro altrui).

# **Caso 7 — Hotfix veloce su main e riallineo**

Bug urgente in produzione.

A:
```bash
git checkout main
git pull --ff-only
git checkout -b hotfix/null-check
# fai il fix minimo
git add .
git commit -m "fix: null check su X"
git push -u origin hotfix/null-check
```

Apri PR verso main. Dopo il merge:

# riallinea developer
```bash
git checkout developer
git pull --ff-only
git merge origin/main
git push
```

Così developer “vede” la stessa correzione.

# **Caso 8 — Piccola release taggata**

È possibile creare un tag di versione (es. v1.0.0) su main per marcare una release stabile.
A crea un tag sulla versione rilasciata su main:
```bash
git checkout main
git pull --ff-only
git tag -a v1.0.0 -m "Prima release esercitazioni"
git push origin v1.0.0
```
Il vantaggio di taggare le release è che si può sempre tornare a quella versione in modo semplice con `git checkout v1.0.0`.

**tag -a:** tag annotato con messaggio (utile per storicizzare le release).

# Mini-glossario dei comandi usati

- **`git checkout <branch>`** passa a un branch esistente.
- **`git checkout -b <branch>`** crea e passa al nuovo branch.
- **`git pull --ff-only`** aggiorna il branch solo se può “avanzare dritto” (evita merge automatici).
- **`git fetch (--all)`** scarica aggiornamenti dal remoto senza toccare i file locali.
- **`git add <file>`** prepara i file per il commit.
- **`git commit -m "messaggio"`** registra le modifiche con un messaggio breve e chiaro.
- **`git push -u origin <branch>`** pubblica il branch e imposta il tracciamento con origin.
- **`git branch -m <nuovo-nome>`** rinomina un branch locale.
- **`git push origin :<branch>`** elimina un branch sul remoto.
- **`git branch -d <branch>`** elimina un branch locale già mergiato (sicuro).
- **`git revert <SHA>`** annulla un commit già pubblicato creando un commit inverso.
- **`git merge origin/<branch>`** unisce nel tuo branch le ultime modifiche del branch remoto.
- **`git tag -a vX.Y.Z -m "note"`** crea un tag “di versione” con descrizione.

# Regole semplici per non pestarsi i piedi

- Solo una persona crea il repository e i branch principali (main, developer).
- La persona che crea il repository (es. lead) è autorizzata a fare i merge.
- Gli altri sviluppatori quando hanno fatto la modifica o il fix aprono una PR verso developer (pull request).
- Ogni task o modifica o fix deve essere più semplice possibile, cioè se è troppo complessa deve essere separata in più task semplici.
- Bisogna necessariamente far procede il lavoro e di conseguenza fare il merge di piccoli task semplici, invece di aspettare di avere un task complesso completo per fare un unico merge.
- Bisogna necessariamente rispettare le convenzioni per i nomi dei branch ma anche per le variabili del codice, per i messaggi dei commit, per i nomi dei file, ecc.
- Il branch main è solo per il codice stabile e rilasciato, non ci si lavora direttamente.
- Solo una persona del team (es. lead) fa merge su main, dopo aver testato e verificato che è tutto ok.
- Creare sempre un branch developer per integrare le modifiche prima di portarle su main.
- Lavora sempre su feature branch, mai su developer o main.
- Prima di creare una feature: git checkout developer && git pull --ff-only.
- Una feature = un branch = una PR verso developer.
- Evita di modificare le stesse righe: se serve, parlatevi prima.
- Risolvi i conflitti in locale, poi aggiorna la PR.
- Niente push forzati su developer o main.
- Elimina i branch feature dopo il merge (locale e remoto).

# Gli stash

Gli stash servono a salvare temporaneamente modifiche non pronte per il commit, in modo da poter cambiare branch senza perdere il lavoro in corso.
Esempio:
```bash
git stash               # salva le modifiche non committate
git checkout developer   # passa a developer
git pull --ff-only     # aggiorna developer
git checkout feature/nuova-feature  # torna al tuo branch
git stash pop          # recupera le modifiche salvate
```

- **git stash:** salva temporaneamente le modifiche non committate.
- **git stash pop:** recupera le modifiche salvate e le rimuove dallo stash.
- **git stash list:** mostra gli stash salvati.
- **git stash apply <stash@{n}>:** applica uno stash specifico senza rimuoverlo dallo stash list.

Il vantaggio di usare gli stash è che puoi cambiare branch senza dover fare commit incompleti o perdere il lavoro in corso.

Tipo se devi fare un pull su developer ma hai modifiche non pronte per il commit, puoi metterle nello stash, aggiornare developer, e poi tornare al tuo branch e recuperarle.


aggiornato