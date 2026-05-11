using System.ComponentModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NuovoCinemaParadiso.Data;
using NuovoCinemaParadiso.Models;

namespace NuovoCinemaParadiso.Seed;

public static class DataSeeder
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        using IServiceScope scope = serviceProvider.CreateScope();

        DateTime oggi = DateTime.Today;
        ContestoDb contestoDb = scope.ServiceProvider.GetRequiredService<ContestoDb>();
        UserManager<Utente> gestioneUtenti = scope.ServiceProvider.GetRequiredService<UserManager<Utente>>();
        RoleManager<IdentityRole> gestioneRuoli = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        await AssicuraEsistenzaRuoloAsync(gestioneRuoli, Ruoli.Gestore);
        await AssicuraEsistenzaRuoloAsync(gestioneRuoli, Ruoli.Operatore);
        await AssicuraEsistenzaRuoloAsync(gestioneRuoli, Ruoli.Utente);

        Utente gestore = await AssicuraEsistenzaUtenteAsync(
            gestioneUtenti,
            "gestore@gmail.com",
            "123456",
            "Gestore",
            60
            , false);

        Utente operatore = await AssicuraEsistenzaUtenteAsync(
            gestioneUtenti,
            "operatore@gmail.com",
            "123456",
            "Operatore",
            35,
            false);

        Utente utente = await AssicuraEsistenzaUtenteAsync(
        gestioneUtenti,
            "utente1@gmail.com",
            "123456",
            "Utente Uno",
            15,
            false);

        await ImpostaRuoloUnicoAsync(gestioneUtenti, gestore, Ruoli.Gestore);
        await ImpostaRuoloUnicoAsync(gestioneUtenti, operatore, Ruoli.Operatore);
        await ImpostaRuoloUnicoAsync(gestioneUtenti, utente, Ruoli.Utente);

        var genereAzione = await AssicuraEsistenzaGenereMovie(contestoDb, "Azione");
        var genereHorror = await AssicuraEsistenzaGenereMovie(contestoDb, "Horror");
        var genereCommedia = await AssicuraEsistenzaGenereMovie(contestoDb, "Commedia");

        var movie1 = await AssicuraEsistenzaMovie(contestoDb, "Movie1", "Film del drago", 60, 10, genereAzione.Id);
        var movie2 = await AssicuraEsistenzaMovie(contestoDb, "Movie2", "Film del lupo", 80, 12, genereHorror.Id);
        var movie3 = await AssicuraEsistenzaMovie(contestoDb, "Movie3", "Film del cane", 100, 14, genereCommedia.Id);

        var tipologia2D = await AssicuraEsistenzaTipologiaSala(contestoDb, "2D", 2);
        var tipologia3D = await AssicuraEsistenzaTipologiaSala(contestoDb, "3D", 3);
        var tipologiaImax = await AssicuraEsistenzaTipologiaSala(contestoDb, "IMAX", 4);

        var turnoMattina = await AssicuraEsistenzaTurno(contestoDb, new TimeOnly(10, 0, 0), new TimeOnly(13, 0, 0), "Mattina");
        var turnoPomeriggio = await AssicuraEsistenzaTurno(contestoDb, new TimeOnly(13, 0, 0), new TimeOnly(18, 0, 0), "Pomeriggio");
        var turnoSera = await AssicuraEsistenzaTurno(contestoDb, new TimeOnly(18, 0, 0), new TimeOnly(22, 0, 0), "Sera");

        var sala1 = await AssicuraEsistenzaSala(contestoDb, "Sala1", 30, tipologia2D.Id);
        var sala2 = await AssicuraEsistenzaSala(contestoDb, "Sala2", 40, tipologia2D.Id);
        var sala3 = await AssicuraEsistenzaSala(contestoDb, "Sala3", 50, tipologia2D.Id);

        await AssicuraEsistenzaAbbonamento(contestoDb, "Mensile", 70, 25, 1);
        await AssicuraEsistenzaAbbonamento(contestoDb, "Semestrale", 210, 50, 6);
        await AssicuraEsistenzaAbbonamento(contestoDb, "Annuale", 300, 75, 12);

        await AssicuraEsistenzaGiftCard(contestoDb, "10 Film", 85, 10, 12);
        await AssicuraEsistenzaGiftCard(contestoDb, "25 Film", 190, 25, 12);
        await AssicuraEsistenzaGiftCard(contestoDb, "50 Film", 325, 50, 12);

        var proiezione1 = await AssicuraEsistenzaProiezione(contestoDb, new DateOnly(2027, 1, 1), movie1.Id, sala1.Id, turnoMattina.Id);
        var proiezione2 = await AssicuraEsistenzaProiezione(contestoDb, new DateOnly(2027, 1, 1), movie2.Id, sala2.Id, turnoPomeriggio.Id);
        var proiezione3 = await AssicuraEsistenzaProiezione(contestoDb, new DateOnly(2027, 1, 1), movie3.Id, sala3.Id, turnoSera.Id);

        await AssicuraEsistenzaAcquisto(contestoDb, proiezione1.Id, utente.Id, 1, new DateTimeOffset(DateTime.Now), 10, "standard");
        await AssicuraEsistenzaAcquisto(contestoDb, proiezione2.Id, utente.Id, 2, new DateTimeOffset(DateTime.Now), 20, "abbonamento");
        await AssicuraEsistenzaAcquisto(contestoDb, proiezione3.Id, utente.Id, 3, new DateTimeOffset(DateTime.Now), 30, "giftcard");
    }

    private static async Task AssicuraEsistenzaRuoloAsync(RoleManager<IdentityRole> managerRuolo, string nomeRuolo)
    {
        bool seEsiste = await managerRuolo.RoleExistsAsync(nomeRuolo);
        if (!seEsiste)
        {
            IdentityRole ruolo = new IdentityRole();
            ruolo.Name = nomeRuolo;

            await managerRuolo.CreateAsync(ruolo);
        }
    }

    private static async Task<Utente> AssicuraEsistenzaUtenteAsync(
        UserManager<Utente> gestioneUtenti,
        string email,
        string password,
        string nomeCompleto,
        int eta,
        bool abbonato
        )
    {
        Utente? utenteEsistente = await gestioneUtenti.FindByEmailAsync(email);

        if (utenteEsistente != null)
        {
            return utenteEsistente;
        }

        Utente utente = new Utente();
        utente.UserName = email;
        utente.Email = email;
        utente.NomeCompleto = nomeCompleto;
        utente.Eta = eta;
        utente.SeAbbonato = abbonato;
        utente.AbbonamentoId = null;

        IdentityResult risultato = await gestioneUtenti.CreateAsync(utente, password);

        if (!risultato.Succeeded)
        {
            List<string> errori = new List<string>();

            foreach (IdentityError errore in risultato.Errors)
            {
                errori.Add(errore.Description);
            }
            string messaggio = string.Join("|", errori);
            throw new Exception($"Errore durante il seed dell'utente {email} : {messaggio}");
        }
        return utente;
    }

    private static async Task ImpostaRuoloUnicoAsync(UserManager<Utente> gestioneUtenti, Utente utente, string ruoloTarget)
    {
        IList<string> ruoliCorrenti = await gestioneUtenti.GetRolesAsync(utente);

        for (int i = 0; i < ruoliCorrenti.Count; i++)
        {
            string ruoloCorrente = ruoliCorrenti[i];

            if (ruoloCorrente == Ruoli.Gestore || ruoloCorrente == Ruoli.Operatore || ruoloCorrente == Ruoli.Utente)
            {
                await gestioneUtenti.RemoveFromRoleAsync(utente, ruoloCorrente);
            }
        }
        bool alreadyInTargetRole = await gestioneUtenti.IsInRoleAsync(utente, ruoloTarget);

        if (!alreadyInTargetRole)
        {
            await gestioneUtenti.AddToRoleAsync(utente, ruoloTarget);
        }
    }

    private static async Task<GenereMovie> AssicuraEsistenzaGenereMovie(
    ContestoDb context,
    string genere)
    {
        List<GenereMovie> generiMovies = await context.GeneriMovies.ToListAsync();
        for (int i = 0; i < generiMovies.Count; i++)
        {
            GenereMovie genereCorrente = generiMovies[i];
            bool nomeUguale = string.Equals(
                genereCorrente.Genere,
                genere,
                StringComparison.OrdinalIgnoreCase);
            if (nomeUguale)
            {
                return genereCorrente;
            }
        }

        GenereMovie nuovoGenere = new GenereMovie
        {
            Genere = genere
        };

        context.GeneriMovies.Add(nuovoGenere);
        await context.SaveChangesAsync();

        return nuovoGenere;
    }

    private static async Task<Movie> AssicuraEsistenzaMovie(
    ContestoDb context,
    string titolo,
    string descrizione,
    int durataMinuti,
    decimal prezzoMovie,
    string genereId)
    {
        List<Movie> movies = await context.Movies.ToListAsync();
        for (int i = 0; i < movies.Count; i++)
        {
            Movie movieCorrente = movies[i];
            bool nomeUguale = string.Equals(
                movieCorrente.Titolo,
                titolo,
                StringComparison.OrdinalIgnoreCase);
            if (nomeUguale)
            {
                return movieCorrente;
            }
        }

        Movie nuovoMovie = new Movie
        {
            Titolo = titolo,
            Descrizione = descrizione,
            DurataMinuti = durataMinuti,
            PrezzoMovie = prezzoMovie,
            GenereId = genereId,
        };

        context.Movies.Add(nuovoMovie);
        await context.SaveChangesAsync();

        return nuovoMovie;
    }

    private static async Task<TipologiaSala> AssicuraEsistenzaTipologiaSala(
     ContestoDb context,
     string nome, decimal maggiorazioneprezzo)
    {
        List<TipologiaSala> tipologieSala = await context.TipologieSala.ToListAsync();
        for (int i = 0; i < tipologieSala.Count; i++)
        {
            TipologiaSala tipologiaCorrente = tipologieSala[i];
            bool nomeUguale = string.Equals(
                tipologiaCorrente.Nome,
                nome,
                StringComparison.OrdinalIgnoreCase);
            if (nomeUguale)
            {
                return tipologiaCorrente;
            }
        }

        TipologiaSala nuovaTipologia = new TipologiaSala
        {
            Nome = nome,
            MaggiorazionePrezzo = maggiorazioneprezzo,
        };

        context.TipologieSala.Add(nuovaTipologia);
        await context.SaveChangesAsync();

        return nuovaTipologia;
    }

    private static async Task<Sala> AssicuraEsistenzaSala(
        ContestoDb context,
        string nome,
        int capienza,
        string tipologiaSalaId)
    {
        List<Sala> sale = await context.Sale.ToListAsync();
        for (int i = 0; i < sale.Count; i++)
        {
            Sala salaCorrente = sale[i];
            bool nomeUguale = string.Equals(
                salaCorrente.Nome,
                nome,
                StringComparison.OrdinalIgnoreCase);
            if (nomeUguale)
            {
                return salaCorrente;
            }
        }

        Sala nuovaSala = new Sala
        {
            Nome = nome,
            Capienza = capienza,
            TipologiaSalaId = tipologiaSalaId,
        };

        context.Sale.Add(nuovaSala);
        await context.SaveChangesAsync();

        return nuovaSala;
    }

    private static async Task<Turno> AssicuraEsistenzaTurno(
     ContestoDb context,
     TimeOnly oraInizio, TimeOnly oraFine, string nome)
    {
        List<Turno> turni = await context.Turni.ToListAsync();
        for (int i = 0; i < turni.Count; i++)
        {
            Turno turnoCorrente = turni[i];
            bool nomeUguale = string.Equals(
                turnoCorrente.Nome,
                nome,
                StringComparison.OrdinalIgnoreCase);
            if (nomeUguale || (turnoCorrente.OraInizio == oraInizio && turnoCorrente.OraFine == oraFine))
            {
                return turnoCorrente;
            }
        }

        Turno nuovoTurno = new Turno
        {
            Nome = nome,
            OraInizio = oraInizio,
            OraFine = oraFine
        };

        context.Turni.Add(nuovoTurno);
        await context.SaveChangesAsync();

        return nuovoTurno;
    }

    private static async Task AssicuraEsistenzaAbbonamento(ContestoDb context, string nome, decimal prezzo, int sconto, int durata)
    {
        List<Abbonamento> abbonamenti = await context.Abbonamenti.ToListAsync();
        for (int i = 0; i < abbonamenti.Count; i++)
        {
            Abbonamento abbonamentoCorrente = abbonamenti[i];
            bool nomeUguale = string.Equals(
                abbonamentoCorrente.Nome,
                nome,
                StringComparison.OrdinalIgnoreCase);
            if (nomeUguale)
            {
                return;
            }

        }

        Abbonamento nuovoAbbonamento = new Abbonamento
        {
            Nome = nome,
            Prezzo = prezzo,
            Sconto = sconto,
            Durata = durata
        };

        context.Abbonamenti.Add(nuovoAbbonamento);
        await context.SaveChangesAsync();
    }
    private static async Task AssicuraEsistenzaGiftCard(ContestoDb context, string nome, decimal prezzo, int numeroMovie, int durata)
    {
        List<GiftCard> giftCards = await context.GiftCards.ToListAsync();
        for (int i = 0; i < giftCards.Count; i++)
        {
            GiftCard giftCardCorrente = giftCards[i];
            bool nomeUguale = string.Equals(
                giftCardCorrente.Nome,
                nome,
                StringComparison.OrdinalIgnoreCase);
            if (nomeUguale)
            {
                return;
            }
        }

        GiftCard nuovaGiftCard = new GiftCard
        {
            Nome = nome,
            Prezzo = prezzo,
            NumeroMovie = numeroMovie,
            Durata = durata
        };

        context.GiftCards.Add(nuovaGiftCard);
        await context.SaveChangesAsync();
    }

    

    private static async Task<Proiezione> AssicuraEsistenzaProiezione(
    ContestoDb context,
    DateOnly dataProiezione,
    string movieId,
    string salaId,
    string turnoId)
    {
        List<Proiezione> proiezioni = await context.Proiezioni.ToListAsync();
        for (int i = 0; i < proiezioni.Count; i++)
        {
            Proiezione proiezione = proiezioni[i];

            bool stessaSala = proiezione.SalaId == salaId;
            bool stessoMovie = proiezione.MovieId == movieId;
            bool stessoTurno = proiezione.TurnoId == turnoId;

            if (stessaSala && stessoMovie && stessoTurno)
            {
                return proiezione;
            }
        }

        Proiezione nuovaProiezione = new Proiezione
        {
            DataProiezione = dataProiezione,
            MovieId = movieId,
            SalaId = salaId,
            TurnoId = turnoId,
            Attivo=true
        };

        context.Proiezioni.Add(nuovaProiezione);
        await context.SaveChangesAsync();

        return nuovaProiezione;
    }

    private static async Task AssicuraEsistenzaAcquisto(
    ContestoDb context,
    string proiezioneId,
    string utenteId,
    int numeroBiglietti,
    DateTimeOffset orarioCreazione,
    decimal prezzoFinale,
    string metodoPagamento)
    {
        List<Acquisto> acquisti = await context.Acquisti.ToListAsync();

        Acquisto nuovoAcquisto = new Acquisto
        {
            ProiezioneId = proiezioneId,
            UtenteId = utenteId,
            NumeroBiglietti = numeroBiglietti,
            OrarioCreazione = orarioCreazione,
            PrezzoFinale = prezzoFinale,
            MetodoPagamento = metodoPagamento,
        };

        context.Acquisti.Add(nuovoAcquisto);
        await context.SaveChangesAsync();
    }
}