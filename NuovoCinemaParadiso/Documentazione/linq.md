# LINQ usato nel progetto

Questo documento descrive gli elementi LINQ utilizzati nelle modifiche fatte al progetto. Per ogni elemento viene indicato a cosa serve nel contesto delle query e delle trasformazioni dati.

## Where
- Filtra una sequenza in base a una condizione.
- Viene usato per selezionare solo gli oggetti che rispettano un criterio, ad esempio:
  - utenti con `UtenteId` specifico
  - proiezioni attive
  - sale di una determinata tipologia
  - gift card non riscattate
- Sintassi tipica: `lista.Where(x => condizione)`

## Select
- Mappa ogni elemento di una sequenza in una nuova forma.
- Usato per creare i DTO dai modelli Entity:
  - `DtoMovie`, `DtoSala`, `DtoBiglietto`, `DtoUtente`, etc.
- Trasforma i risultati della query in oggetti più leggeri e specifici per l'API.
- Sintassi tipica: `lista.Select(x => new Dto { ... })`

## ToList / ToListAsync
- Materializza una query in una lista concreta.
- `ToListAsync()` è la versione asincrona per Entity Framework Core, utile per eseguire la query nel database senza bloccare il thread.
- Usato quando serve lavorare con una raccolta in memoria o passare i risultati a `Select`, `Where`, ecc.
- Sintassi tipica: `await query.ToListAsync()` oppure `lista.ToList()`.

## FirstOrDefault / FirstOrDefaultAsync
- Restituisce il primo elemento di una sequenza o `null` se non ne esiste nessuno.
- `FirstOrDefaultAsync()` esegue la query in modo asincrono su EF Core.
- Utilizzato per ottenere:
  - un utente specifico
  - una gift card dal codice di riscatto
  - un abbonamento esistente
- Sintassi tipica: `await query.FirstOrDefaultAsync(x => x.Id == id)`

## Any / AnyAsync
- Controlla se esiste almeno un elemento nella sequenza che soddisfa la condizione.
- Utilizzato per:
  - verificare se un nome esiste già
  - controllare se una tipologia esiste
  - controllare se un turno già esiste
- `AnyAsync()` è la versione EF asincrona.
- Sintassi tipica: `await query.AnyAsync(x => x.Nome == nome)`

## OrderByDescending
- Ordina la sequenza in ordine decrescente in base a una chiave.
- Utilizzato per prendere l'abbonamento più recente dell'utente quando ci sono più record `UtenteAbbonamento`.
- Sintassi tipica: `sequence.OrderByDescending(x => x.DataInizioAbbonamento)`

## GroupBy
- Raggruppa gli elementi della sequenza in base a una chiave comune.
- Utilizzato per ottenere, per ogni utente, l'ultimo record di `UtenteAbbonamento` nella query `OttieniUtentiTramiteAbbonamentoAsync`.
- Sintassi tipica: `utentiAbbonamenti.GroupBy(ua => ua.UtenteId)`

## Task.WhenAll + Select async
- Non è strettamente un operatore LINQ, ma è stato usato insieme a `Select` per eseguire più operazioni asincrone in parallelo.
- Serve a trasformare più elementi con chiamate asynchronous in un unico array di risultati.
- Esempio: `await Task.WhenAll(elementi.Select(async item => { ... }))`

## Include / ThenInclude
- Estensioni di Entity Framework Core per includere relazioni nella query.
- Usato per caricare insieme a `Utente` anche i record `UtentiAbbonamenti` e la relativa entità `Abbonamento`.
- Questo evita chiamate separate e permette di usare le proprietà correlate direttamente.
- Sintassi tipica: `context.Utenti.Include(u => u.UtentiAbbonamenti).ThenInclude(ua => ua.Abbonamento)`

## ToLower / StringComparison in query
- Non è un operatore LINQ, ma è spesso usato nella condizione per confronti case-insensitive.
- Esempio: `x.Nome.ToLower() == nome.ToLower()`

## Esempi di pattern ricorrenti
- `lista.Where(...).Select(...).ToList()`
  - Filtra, trasforma e materializza i dati.
- `await query.FirstOrDefaultAsync(...)` 
  - Cerca un singolo elemento nel database.
- `await query.AnyAsync(...)`
  - Verifica esistenza senza scaricare tutti gli elementi.
- `await Task.WhenAll(lista.Select(async ...))`
  - Esegue più trasformazioni asincrone in parallelo.

## Perché questi elementi sono importanti
- `Where` riduce i dati a quelli rilevanti prima di trasformarli.
- `Select` costruisce DTO specifici, mantenendo separata la logica di dominio dai modelli.
- `ToListAsync` e `FirstOrDefaultAsync` permettono operazioni efficienti con EF Core.
- `Any` evita controlli manuali con cicli e condizionali.
- `GroupBy` gestisce aggregazioni e raggruppamenti, utili quando serve l'ultimo stato per utente.
- `OrderByDescending` aiuta a scegliere il record più recente.

## Conclusione
Queste modifiche hanno ridotto i cicli manuali e migliorato la leggibilità del codice usando le funzionalità principali di LINQ. Il risultato è un codice più dichiarativo, più facile da leggere e più vicino al modello delle query sui dati.