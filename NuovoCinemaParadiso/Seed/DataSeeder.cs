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

        await AssicuraEsistenzaGenereMovie(contestoDb, "Azione");
        await AssicuraEsistenzaGenereMovie(contestoDb, "Horror");
        await AssicuraEsistenzaGenereMovie(contestoDb, "Commedia");

        await AssicuraEsistenzaTipologiaSala(contestoDb, "2D", 2);
        await AssicuraEsistenzaTipologiaSala(contestoDb, "3D", 3);
        await AssicuraEsistenzaTipologiaSala(contestoDb, "IMAX", 4);

        await AssicuraEsistenzaTurno(contestoDb, new TimeOnly(10, 0, 0), new TimeOnly(13, 0, 0), "Mattina");
        await AssicuraEsistenzaTurno(contestoDb, new TimeOnly(13, 0, 0), new TimeOnly(18, 0, 0), "Pomeriggio");
        await AssicuraEsistenzaTurno(contestoDb, new TimeOnly(18, 0, 0), new TimeOnly(22, 0, 0), "Sera");

        await AssicuraEsistenzaAbbonamento(contestoDb, "Mensile", 70, 25, 1);
        await AssicuraEsistenzaAbbonamento(contestoDb, "Semestrale", 210, 50, 6);
        await AssicuraEsistenzaAbbonamento(contestoDb, "Annuale", 300, 75, 12);

        await AssicuraEsistenzaGiftCard(contestoDb, "10 Film", 85, 10, 12);
        await AssicuraEsistenzaGiftCard(contestoDb, "25 Film", 190, 25, 12);
        await AssicuraEsistenzaGiftCard(contestoDb, "50 Film", 325, 50, 12);

        await AssicuraEsistenzaSala(contestoDb, "Sala 1", 100, "2D");
        await AssicuraEsistenzaSala(contestoDb, "Sala 2", 80, "3D");
        await AssicuraEsistenzaSala(contestoDb, "Sala 3", 50, "4D");
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

    private static async Task AssicuraEsistenzaGenereMovie(
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
                return;
            }
        }

        GenereMovie nuovoGenere = new GenereMovie
        {
            Genere = genere
        };

        context.GeneriMovies.Add(nuovoGenere);
        await context.SaveChangesAsync();
    }

    private static async Task AssicuraEsistenzaTipologiaSala(
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
                return;
            }
        }

        TipologiaSala nuovaTipologia = new TipologiaSala
        {
            Nome = nome,
            MaggiorazionePrezzo = maggiorazioneprezzo,
        };

        context.TipologieSala.Add(nuovaTipologia);
        await context.SaveChangesAsync();
    }

    private static async Task AssicuraEsistenzaTurno(
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
                return;
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

    private static async Task AssicuraEsistenzaSala(ContestoDb context, string nome, int capienza, string tipologiaSalaId)
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
                return;
            }

        }

        Sala nuovaSala = new Sala
        {
            Nome = nome,
            Capienza = capienza,
            TipologiaSalaId = tipologiaSalaId
        };

        context.Sale.Add(nuovaSala);
        await context.SaveChangesAsync();
    }
}