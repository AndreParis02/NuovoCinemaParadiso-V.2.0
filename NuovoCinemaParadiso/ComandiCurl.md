# Comandi Curl (cambiare la porta localhost per farlo funzionare):

# Utente:

## Curl di Registrazione utente

```bash
curl -s -X POST "http://localhost:5226/api/Auth/registrazione" \
-H "Content-Type: application/json" \
-d '{"email":"mariorossi@gmail.com","password":"123456","nomeCompleto":"Mario Rossi","eta":"55"}'
```


## Curl di Login utente: 

```bash
TOKEN=$(curl -s -X POST "http://localhost:5226/api/Auth/login" \
-H "Content-Type: application/json" \
-d '{"email":"gestore@gmail.com","password":"123456"}' | jq -r '.token')
```

## stampa della variabile token
```bash
echo $TOKEN
```
## Lettura del profilo in sessione
```bash
curl -s -X GET http://localhost:5226/api/Auth/profilo \
-H "Authorization: Bearer $TOKEN" \
-H "Accept: application/json"
```

## Lettura di un profilo tramite id inserito (solo da Gestore o Operatore)
```bash
curl -s -X GET http://localhost:5226/api/Auth/(idUtente)\
-H "Authorization: Bearer $TOKEN" \
-H "Accept: application/json"
```

## Curl di Modifica utente

```bash
curl -s -X PUT "http://localhost:5226/api/Auth/modifica" \
-H "Content-Type: application/json" \
-H "Authorization: Bearer $TOKEN" \
-d '{"nomeCompleto":"Mario Rossi","eta": 55}' | jq
```

## Curl di Elimina utente loggato

```bash
curl -s -X DELETE "http://localhost:5226/api/Auth/elimina" \
-H "Authorization: Bearer $TOKEN" 
```

## Curl di Elimina utente (Solo da gestore o operatore) passando Id utente

```bash
curl -s  -X DELETE "http://localhost:5226/api/Auth/id utente da eliminare" \
-H "Authorization: Bearer $TOKEN" 
```

## Curl di Cambio ruolo utente (solo se Gestore)

```bash
curl -s -X PUT "http://localhost:5226/api/GestoreUtenti/cambia-ruolo" \
-H "Content-Type: application/json" \
-H "Authorization: Bearer $TOKEN" \
-d '{
    "email":"mariorossi@gmail.com", 
    "nuovoRuolo":"Opera"
    }' | jq
```

# Generi Movies

## Leggi Generi Movies:

```bash
curl -s -X GET "http://localhost:5226/api/GeneriMovies" -H "Authorization: Bearer $TOKEN"
```

## Leggi Genere Movie tramite Id del genere:

```bash
curl -s -X GET "http://localhost:5226/api/GeneriMovies/Id Genere" \
-H "Authorization: Bearer $TOKEN"
```

## Crea Genere Movie:

```bash
curl -s -X POST "http://localhost:5226/api/GeneriMovies" \
-H "Content-Type: application/json" \
-H "Authorization: Bearer $TOKEN" \
-d '{"genere":"Commedia"}'| jq
```

## Modifica Genere Movie tramite Id del genere:

```bash
curl -s -X PUT "http://localhost:5226/api/GeneriMovies/Id Genere" -H "Content-Type: application/json" -H "Authorization: Bearer $TOKEN" -d '{"genere":"Horror"}'
```

## Elimina Genere Movie tramite Id del genere:
```bash
curl -s -X DELETE "http://localhost:5226/api/GeneriMovies/Id Genere" -H "Authorization: Bearer $TOKEN" 
```

# Tipologia Sala

## Leggi TipologieSala:

```bash
curl -s -X GET "http://localhost:5226/api/TipologieSala" -H "Authorization: Bearer $TOKEN"
```

## Leggi Tipologia Sala tramite Id della tipologia della sala:

```bash
curl -s -X GET "http://localhost:5226/api/TipologieSala/Id Tipologia Sala" \
-H "Authorization: Bearer $TOKEN"
```

## Crea TipologiaSala:

```bash
curl -s -X POST "http://localhost:5226/api/TipologieSala" \
-H "Content-Type: application/json" \
-H "Authorization: Bearer $TOKEN" \
-d '{"nome":"3D", "maggiorazionePrezzo": 2.00}' | jq
```

## Modifica Tipologia Sala tramite Id della tipologia della sala:

```bash
curl -s -X PUT "http://localhost:5226/api/TipologieSala/Id Tipologia Sala" -H "Content-Type: application/json" -H "Authorization: Bearer $TOKEN" -d '{"nome":"3D 2.0", "maggiorazionePrezzo": 3.00}'
```

## Elimina Tipologia Sala tramite Id della tipologia della sala:

```bash
curl -s -X DELETE "http://localhost:5226/api/TipologieSala/Id Tipologia Sala" -H "Authorization: Bearer $TOKEN" 
```

# Fasce Orarie

## Leggi FasceOrarie:

```bash
curl -s -X GET "http://localhost:5226/api/FasciaOraria" -H "Authorization: Bearer $TOKEN"
```

## Leggi Fascia Oraria tramite Id della fascia oraria:

```bash
curl -s -X GET "http://localhost:5226/api/FasciaOraria/Id Fascia Oraria" \
-H "Authorization: Bearer $TOKEN"
```

## Crea Fascia Oraria:

```bash
curl -s -X POST "http://localhost:5226/api/FasciaOraria" \
-H "Content-Type: application/json" \
-H "Authorization: Bearer $TOKEN" \
-d '{
    "oraInizio": "15:30:00",
    "oraFine": "18:00:00",
    "nome": "Pomeriggio"
}' | jq
```
## Modifica Fascia Oraria con id della fascia oraria:

```bash
curl -s -X PUT "http://localhost:5226/api/FasciaOraria/Id Fascia Oraria" \
-H "Content-Type: application/json" \
-H "Authorization: Bearer $TOKEN" \
-d '{
    "oraInizio": "16:30:00",
    "oraFine": "18:00:00",
    "nome": "Pomeriggio"
}' | jq
```

## Elimina Fascia Oraria con id della fascia oraria:
```bash
curl -s -X DELETE "http://localhost:5226/api/FasciaOraria/Id Fascia Oraria" -H "Authorization: Bearer $TOKEN" 
```

# Sale

## Leggi Sale:

```bash
curl -s -X GET "http://localhost:5226/api/Sale" -H "Authorization: Bearer $TOKEN"
```

## Leggi Sala tramite Id della sala:

```bash
curl -s -X GET "http://localhost:5226/api/Sale/Id Sala" \
-H "Authorization: Bearer $TOKEN"
```

## Leggi Sale tramite IdFasciaOraria :

```bash
curl -s -X GET "http://localhost:5226/api/Sale/fascia-oraria/Id Fascia Oraria" \
-H "Authorization: Bearer $TOKEN"
```

## Leggi Sale tramite IdTipologiaSala:

```bash
curl -s -X GET "http://localhost:5226/api/Sale/tipologia/Id Tipologia Sala" \
-H "Authorization: Bearer $TOKEN"
```

## Crea Sala:

```bash
curl -s -X POST "http://localhost:5226/api/Sale" \
-H "Content-Type: application/json" \
-H "Authorization: Bearer $TOKEN" \
-d '{
    "Nome": "Sala 3",
    "Capienza": 100,
    "TipologiaSalaId": "Id Tipologia Sala",
    "FasciaOrariaId": "Id Fascia Oraria"
}' | jq
```
## Modifica Sala con id della sala:

```bash
curl -s -X PUT "http://localhost:5226/api/Sale/Id Sala" \
-H "Content-Type: application/json" \
-H "Authorization: Bearer $TOKEN" \
-d '{
    "Nome": "Sala 2 modificata",
    "Capienza": 100,
    "TipologiaSalaId": "Id Tipologia Sala",
    "FasciaOrariaId": "Id Fascia Oraria"
}' | jq
```

## Elimina Sala con id della sala:
```bash
curl -s -X DELETE "http://localhost:5226/api/Sale/Id Sala" -H "Authorization: Bearer $TOKEN" 
```

# Movies

## Leggi tutti i film
```bash
curl -s -X GET "http://localhost:5226/api/Movies" -H "Authorization: Bearer $TOKEN"
```

## leggi informazioni film per id
```bash
curl -s -X GET "http://localhost:5226/api/Movies/Id Movies" \
-H "Authorization: Bearer $TOKEN"
```

## ottieni film per genere
```bash
curl -s -X GET "http://localhost:5226/api/Movies/genere/Id Genere" \
-H "Authorization: Bearer $TOKEN"
```

## Crea Movie:

```bash
curl -s -X POST "http://localhost:5226/api/Movies" \
-H "Content-Type: application/json" \
-H "Authorization: Bearer $TOKEN" \
-d '{
    "Titolo": "Film 2",
    "Descrizione": "Descrizione Film 2",
    "DurataMinuti": 120,
    "PrezzoMovie": 12,
    "GenereId":"Id Genere"
}' | jq
```

## Modifica Movie con id del movie:

```bash
curl -s -X PUT "http://localhost:5226/api/Movies/Id Movie" \
-H "Content-Type: application/json" \
-H "Authorization: Bearer $TOKEN" \
-d '{
    "Titolo": "Lo squalo 2", 
    "Descrizione": "Uno squalo ammazza molta piu gente",
    "DurataMinuti": 90,"Prezzo": 10,"GenereId": " Id Genere "}' | jq 
```

## Elimina Movie con id del movie:
```bash
curl -s -X DELETE "http://localhost:5226/api/Movies/Id Movie" -H "Authorization: Bearer $TOKEN" 
```

# Acquisti

## Leggi tutti gli Acquisti di tutti gli utenti(solo gestore o operatore)
```bash
curl -s -X GET "http://localhost:5226/api/Acquisti/admin" -H "Authorization: Bearer $TOKEN"
```

## Leggi tutti gli Acquisti dell'utente loggato
```bash
curl -s -X GET "http://localhost:5226/api/Acquisti" -H "Authorization: Bearer $TOKEN"
```

## leggi informazioni Acquisto per id (accesso a tutti gli acquisti di ogni utente) (solo gestore o operatore)
```bash
curl -s -X GET "http://localhost:5226/api/Acquisti/admin/Id Acquisto" \
-H "Authorization: Bearer $TOKEN"
```

## leggi informazioni Acquisto per id (accesso a tutti gli acquisti dell'utente loggato)
```bash
curl -s -X GET "http://localhost:5226/api/Acquisti/Id Acquisto" \
-H "Authorization: Bearer $TOKEN"
```

## Crea Acquisto:

```bash
curl -s -X POST "http://localhost:5226/api/Acquisti" \
-H "Content-Type: application/json" \
-H "Authorization: Bearer $TOKEN" \
-d '{
    "MovieId": "Id Movie",
    "SalaId": "Id Sala",
    "NumeroBiglietti": 3
}' | jq
```

## Modifica Acquisto con id dell'acquisto:

```bash
curl -s -X PUT "http://localhost:5226/api/Acquisti/Id Acquisto" \
-H "Content-Type: application/json" \
-H "Authorization: Bearer $TOKEN" \
-d '{
    "MovieId": "Id Movie",
    "SalaId": "Id Sala",
    "NumeroBiglietti": 2
    }' | jq 
```

## Elimina Movie con id dell'Acquisto:
```bash
curl -s -X DELETE "http://localhost:5226/api/Acquisti/Id Acquisto" -H "Authorization: Bearer $TOKEN" 
```


