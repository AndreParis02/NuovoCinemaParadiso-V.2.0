# Comandi Curl:

# Utente:

## Curl di Registrazione utente

```bash
curl -s -X POST "http://localhost:5226/api/Auth/registrazione" \
-H "Content-Type: application/json" \
-d '{"email":"mariorossi@gmail.com","password":"123456","nomeCompleto":"Mario Rossi","eta":"55", "SeAbbonato": false, "DataInizio": null, "AbbonamentoId":null}'
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

## Curl di Cambio ruolo utente (solo se Gestore)

```bash
curl -s -X PUT "http://localhost:5226/api/GestoreUtenti/cambia-ruolo" \
-H "Content-Type: application/json" \
-H "Authorization: Bearer $TOKEN" \
-d '{
    "email":"mariorossi@gmail.com", 
    "nuovoRuolo":"Operatore"
    }' | jq
```

## Curl di lettura log

```bash
curl -s -X GET "http://localhost:5226/api/Admin/log" -H "Authorization: Bearer $TOKEN"

```

# Generi Movies

## Leggi Generi Movies:

```bash
curl -s -X GET "http://localhost:5226/api/GenereMovie" -H "Authorization: Bearer $TOKEN"
```

## Leggi Genere Movie tramite Id del genere:

```bash
curl -s -X GET "http://localhost:5226/api/GenereMovie/Id Genere" \
-H "Authorization: Bearer $TOKEN"
```

## Crea Genere Movie:

```bash
curl -s -X POST "http://localhost:5226/api/GenereMovie" \
-H "Content-Type: application/json" \
-H "Authorization: Bearer $TOKEN" \
-d '{"genere":"Commedia"}'| jq
```

## Modifica Genere Movie tramite Id del genere:

```bash
curl -s -X PUT "http://localhost:5226/api/GenereMovie/Id Genere" -H "Content-Type: application/json" -H "Authorization: Bearer $TOKEN" -d '{"genere":"Horror"}'
```

## Elimina Genere Movie tramite Id del genere:
```bash
curl -s -X DELETE "http://localhost:5226/api/GenereMovie/Id Genere" -H "Authorization: Bearer $TOKEN" 
```

# Tipologia Sala

## Leggi TipologieSala:

```bash
curl -s -X GET "http://localhost:5226/api/TipologiaSala" -H "Authorization: Bearer $TOKEN"
```

## Leggi Tipologia Sala tramite Id della tipologia della sala:

```bash
curl -s -X GET "http://localhost:5226/api/TipologiaSala/Id Tipologia Sala" \
-H "Authorization: Bearer $TOKEN"
```

## Crea TipologiaSala:

```bash
curl -s -X POST "http://localhost:5226/api/TipologiaSala" \
-H "Content-Type: application/json" \
-H "Authorization: Bearer $TOKEN" \
-d '{"nome":"3D", "maggiorazionePrezzo": 2.00}' | jq
```

## Modifica Tipologia Sala tramite Id della tipologia della sala:

```bash
curl -s -X PUT "http://localhost:5226/api/TipologiaSala/Id Tipologia Sala" -H "Content-Type: application/json" -H "Authorization: Bearer $TOKEN" -d '{"nome":"3D 2.0", "maggiorazionePrezzo": 3.00}'
```

## Elimina Tipologia Sala tramite Id della tipologia della sala:

```bash
curl -s -X DELETE "http://localhost:5226/api/TipologiaSala/Id Tipologia Sala" -H "Authorization: Bearer $TOKEN" 
```

# Turno

## Leggi Turni:

```bash
curl -s -X GET "http://localhost:5226/api/Turno" -H "Authorization: Bearer $TOKEN"
```

## Leggi Turno tramite Id:

```bash
curl -s -X GET "http://localhost:5226/api/Turno/Id Turno" \
-H "Authorization: Bearer $TOKEN"
```

## Crea Turno:

```bash
curl -s -X POST "http://localhost:5226/api/Turno" \
-H "Content-Type: application/json" \
-H "Authorization: Bearer $TOKEN" \
-d '{
    "oraInizio": "13:00:00",
    "oraFine": "18:00:00",
    "nome": "Pomeriggio"
}' | jq
```
## Modifica Turno con id del Turno:

```bash
curl -s -X PUT "http://localhost:5226/api/Turno/Id Turno" \
-H "Content-Type: application/json" \
-H "Authorization: Bearer $TOKEN" \
-d '{
    "oraInizio": "16:30:00",
    "oraFine": "18:00:00",
    "nome": "Pomeriggio"
}' | jq
```

## Elimina Turno con id della Turno:
```bash
curl -s -X DELETE "http://localhost:5226/api/Turno/Id Turno" -H "Authorization: Bearer $TOKEN" 
```

# Sale

## Leggi Sale:

```bash
curl -s -X GET "http://localhost:5226/api/Sala" -H "Authorization: Bearer $TOKEN"
```

## Leggi Sala tramite Id della sala:

```bash
curl -s -X GET "http://localhost:5226/api/Sala/Id Sala" \
-H "Authorization: Bearer $TOKEN"
```

## Leggi Sale tramite IdTipologiaSala:

```bash
curl -s -X GET "http://localhost:5226/api/Sala/tipologia/Id Tipologia Sala" \
-H "Authorization: Bearer $TOKEN"
```

## Crea Sala:

```bash
curl -s -X POST "http://localhost:5226/api/Sala" \
-H "Content-Type: application/json" \
-H "Authorization: Bearer $TOKEN" \
-d '{
    "Nome": "Sala 1",
    "Capienza": 100,
    "TipologiaSalaId": "Id_Tipologiasala"
}' | jq
```
## Modifica Sala con id della sala:

```bash
curl -s -X PUT "http://localhost:5226/api/Sala/Id_sala" \
-H "Content-Type: application/json" \
-H "Authorization: Bearer $TOKEN" \
-d '{
    "Nome": "Sala 1 modificata",
    "Capienza": 100,
    "TipologiaSalaId": "Id_tipologiaSala"
}' | jq
```

## Elimina Sala con id della sala:
```bash
curl -s -X DELETE "http://localhost:5226/api/Sala/Id Sala" -H "Authorization: Bearer $TOKEN" 
```

# Movie

## Leggi tutti i film
```bash
curl -s -X GET "http://localhost:5226/api/Movie" -H "Authorization: Bearer $TOKEN"
```

## leggi informazioni film per id
```bash
curl -s -X GET "http://localhost:5226/api/Movie/Id Movies" \
-H "Authorization: Bearer $TOKEN"
```

## ottieni film per genere
```bash
curl -s -X GET "http://localhost:5226/api/Movie/genere/Id Genere" \
-H "Authorization: Bearer $TOKEN"
```

## Crea Movie:

```bash
curl -s -X POST "http://localhost:5226/api/Movie" \
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
curl -s -X PUT "http://localhost:5226/api/Movie/Id Movie" \
-H "Content-Type: application/json" \
-H "Authorization: Bearer $TOKEN" \
-d '{
    "Titolo": "Lo squalo 2", 
    "Descrizione": "Uno squalo ammazza molta piu gente",
    "DurataMinuti": 90,"Prezzo": 10,"GenereId": " Id Genere "}' | jq 
```

## Elimina Movie con id del movie:
```bash
curl -s -X DELETE "http://localhost:5226/api/Movie/Id Movie" -H "Authorization: Bearer $TOKEN" 
```

# Acquisti

## Leggi tutti gli Acquisti dell'utente loggato
```bash
curl -s -X GET "http://localhost:5226/api/Acquisto" -H "Authorization: Bearer $TOKEN"
```

## leggi informazioni Acquisto per id (accesso a tutti gli acquisti dell'utente loggato)
```bash
curl -s -X GET "http://localhost:5226/api/Acquisto/Id Acquisto" \
-H "Authorization: Bearer $TOKEN"
```

## Crea Acquisto:

```bash
curl -s -X POST "http://localhost:5226/api/Acquisto" \
-H "Content-Type: application/json" \
-H "Authorization: Bearer $TOKEN" \
-d '{
    "ProiezioneId": "Id_proiezione",
    "NumeroBiglietti": 3
}' | jq
```

## Modifica Acquisto con id dell'acquisto:

```bash
curl -s -X PUT "http://localhost:5226/api/Acquisto/Id_acquisto" \
-H "Content-Type: application/json" \
-H "Authorization: Bearer $TOKEN" \
-d '{
    "ProiezioneId": "Id_proiezione",
    "NumeroBiglietti": 2
    }' | jq 
```

## Elimina Movie con id dell'Acquisto:
```bash
curl -s -X DELETE "http://localhost:5226/api/Acquisto/Id Acquisto" -H "Authorization: Bearer $TOKEN" 
```

# Abbonamenti

## Leggi tutti gli abbonamenti
```bash
curl -s -X GET "http://localhost:5226/api/Abbonamento" -H "Authorization: Bearer $TOKEN" | jq
```

## leggi informazioni Abbonamento per id (accesso a tutti gli acquisti dell'utente loggato)
```bash
curl -s -X GET "http://localhost:5226/api/Abbonamento/Id Abbonamento" -H "Authorization: Bearer $TOKEN"
```

## Crea Abbonamento:

```bash
curl -s -X POST "http://localhost:5226/api/Abbonamento" \
-H "Content-Type: application/json" \
-H "Authorization: Bearer $TOKEN" \
-d '{
    "Nome": "TEST",
    "Durata": 420,
    "Prezzo": 32.50,
    "Sconto" : 5
}' | jq
```

## Modifica Abbonamento con id dell'abbonamento:

```bash
curl -s -X PUT "http://localhost:5226/api/Abbonamento/Id Abbonamento" \
-H "Content-Type: application/json" \
-H "Authorization: Bearer $TOKEN" \
-d '{
    "Nome": "TEST MODIFICATO",
    "Durata": 520,
    "Prezzo": 36.99,
    "Sconto" : 47
}' | jq
```

## Elimina l'Abbonamento con id dell'Abbonamento:
```bash
curl -s -X DELETE "http://localhost:5226/api/Abbonamento/Id Abbonamento" -H "Content-Type: application/json" -H "Authorization: Bearer $TOKEN" | jq 
```


# GiftCard

## Leggi tutte le giftCard
```bash
curl -s -X GET "http://localhost:5226/api/GiftCard" -H "Authorization: Bearer $TOKEN" | jq
```

## leggi informazioni GiftCard per id (accesso a tutti gli acquisti dell'utente loggato)
```bash
curl -s -X GET "http://localhost:5226/api/GiftCard/Id GiftCard" -H "Authorization: Bearer $TOKEN"
```

## Crea GiftCard:

```bash
curl -s -X POST "http://localhost:5226/api/GiftCard" \
-H "Content-Type: application/json" \
-H "Authorization: Bearer $TOKEN" \
-d '{
    "Nome": "GiftCard 1",
    "Durata": 420,
    "Prezzo": 32.50,
    "NUmeroMovie" : 10
}' | jq
```

## Modifica GiftCard con id dell'abbonamento:

```bash
curl -s -X PUT "http://localhost:5226/api/GiftCard/Id GiftCard" \
-H "Content-Type: application/json" \
-H "Authorization: Bearer $TOKEN" \
-d '{
    "Nome": "GiftCard 1 modificata",
    "Durata": 520,
    "Prezzo": 36.99,
    "NUmeroMovie" : 20
}' | jq
```

## Elimina la GiftCard con id della GiftCard:
```bash
curl -s -X DELETE "http://localhost:5226/api/GiftCard/Id GiftCard" -H "Content-Type: application/json" -H "Authorization: Bearer $TOKEN" | jq 
```

# Proiezione

## Leggi tutte le proiezioni

```bash
curl -s -X GET "http://localhost:5226/api/Proiezione" -H "Authorization: Bearer $TOKEN"
```

## Leggi proiezione tramite id

```bash
curl -s -X GET "http://localhost:5226/api/Proiezione/Id Proiezione" \
-H "Authorization: Bearer $TOKEN"
```

## Leggi proiezione tramite id turno

```bash
curl -s -X GET "http://localhost:5226/api/Proiezione/turno/Id Turno" \
-H "Authorization: Bearer $TOKEN"
```

## Leggi proiezione tramite id sala

```bash
curl -s -X GET "http://localhost:5226/api/Proiezione/sala/Id Sala" \
```

## ottieni proiezione per movie

```bash
curl -s -X GET "http://localhost:5226/api/Proiezione/movie/Id Movie" \
-H "Authorization: Bearer $TOKEN"
```

## Crea Proiezione:

```bash
curl -s -X POST "http://localhost:5226/api/Proiezione" \
-H "Content-Type: application/json" \
-H "Authorization: Bearer $TOKEN" \
-d '{
    "MovieId": "Id del movie",
    "SalaId": "Id della sala",
    "TurnoId": "Id del turno",
    "DataProiezione": "AAAA-MM-GG"
}' | jq
```

## Modifica Proiezione con id della Proiezione:

```bash
curl -s -X PUT "http://localhost:5226/api/Proiezione/Id Proiezione" \
-H "Content-Type: application/json" \
-H "Authorization: Bearer $TOKEN" \
-d '{
    "MovieId": "Id Movie",
    "SalaId": "Id Sala",
    "TurnoId": "Id del turno",
    "DataProiezione": "AAAA-MM-GG"
    }' | jq 
```

## Elimina Proiezione con id della Proiezione:

```bash
curl -s -X DELETE "http://localhost:5226/api/Proiezione/Id Proiezione" -H "Authorization: Bearer $TOKEN" 
```


# Comandi Admin: 

## Leggi tutti gli utenti
```bash
curl -s -X GET "http://localhost:5226/api/Admin/listaUtenti" -H "Authorization: Bearer $TOKEN"
```

## Lettura di un profilo tramite id inserito (solo da Gestore o Operatore)
```bash
curl -s -X GET http://localhost:5226/api/Admin/ricercaProfilo/Id utente \
-H "Authorization: Bearer $TOKEN" \
-H "Accept: application/json"
```

## Curl di Elimina utente (Solo da gestore o operatore) passando Id utente

```bash
curl -s  -X DELETE "http://localhost:5226/api/Admin/eliminaUtente/27315238-a01a-4879-b771-0c8130e20d0b" \
-H "Authorization: Bearer $TOKEN" 
```

## Admin Acquisti

## leggi informazioni Acquisto per id (accesso a tutti gli acquisti di ogni utente) (solo gestore o operatore)
```bash
curl -s -X GET "http://localhost:5226/api/Admin/acquisto/Id Acquisto" \
-H "Authorization: Bearer $TOKEN"
```

## Leggi tutti gli Acquisti di tutti gli utenti(solo gestore o operatore)
```bash
curl -s -X GET "http://localhost:5226/api/Admin/acquisto" -H "Authorization: Bearer $TOKEN"
```

## Leggi Utenti Per Id Abbonamento
```bash
curl -X GET "http://localhost:5226/Admin/utenti/abbonamento/Id_abbonamento" \
-H "Authorization: Bearer YOUR_JWT_TOKEN" \
-H "Content-Type: application/json"
  ```

## Admin GiftCard

## Leggi Utenti Per Id GiftCard
```bash
curl -X GET "http://localhost:5226/Admin/utenti/giftCard/Id_giftCard" \
-H "Authorization: Bearer YOUR_JWT_TOKEN" \
-H "Content-Type: application/json"
  ```

# Comandi Utente:

## Abbonati

```bash
curl -s -X POST "http://localhost:5226/api/Utente/abbonati" \
-H "Content-Type: application/json" \
-H "Authorization: Bearer $TOKEN" \
-d '{
  "abbonamentoId": "id abbonamento"
}'
```

## GiftCard:

```bash
curl -s -X POST "http://localhost:5226/api/Utente/giftcard" \
-H "Content-Type: application/json" \
-H "Authorization: Bearer $TOKEN" \
-d '{
  "giftCardId": "id giftCardId"
}'
```