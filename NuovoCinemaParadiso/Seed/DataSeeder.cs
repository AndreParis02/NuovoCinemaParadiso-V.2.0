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
            60);

        Utente operatore = await AssicuraEsistenzaUtenteAsync(
            gestioneUtenti,
            "operatore@gmail.com",
            "123456",
            "Operatore",
            35);

        Utente utente = await AssicuraEsistenzaUtenteAsync(
        gestioneUtenti,
            "utente1@gmail.com",
            "123456",
            "Utente Uno",
            15);

        await ImpostaRuoloUnicoAsync(gestioneUtenti, gestore, Ruoli.Gestore);
        await ImpostaRuoloUnicoAsync(gestioneUtenti, operatore, Ruoli.Operatore);
        await ImpostaRuoloUnicoAsync(gestioneUtenti, utente, Ruoli.Utente);

        await AssicuraEsistenzaGenereMovie(contestoDb,"Azione");
        await AssicuraEsistenzaGenereMovie(contestoDb,"Horror");
        await AssicuraEsistenzaGenereMovie(contestoDb,"Commedia");

        await AssicuraEsistenzaTipologiaSala(contestoDb,"2D",2);
        await AssicuraEsistenzaTipologiaSala(contestoDb,"3D",3);
        await AssicuraEsistenzaTipologiaSala(contestoDb,"IMAX",4);
        
        await AssicuraEsistenzaFasciaOraria(contestoDb,TimeSpan.FromHours(10), TimeSpan.FromHours(13),"Mattina");
        await AssicuraEsistenzaFasciaOraria(contestoDb,TimeSpan.FromHours(13), TimeSpan.FromHours(18),"Pomeriggio");
        await AssicuraEsistenzaFasciaOraria(contestoDb,TimeSpan.FromHours(18), TimeSpan.FromHours(22),"Sera");
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
        int eta
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

  private static async Task AssicuraEsistenzaFasciaOraria(
   ContestoDb context,
   TimeSpan oraInizio, TimeSpan oraFine, string nome)
  {
    List<FasciaOraria> fasceOrarie = await context.FasceOrarie.ToListAsync();
    for (int i = 0; i < fasceOrarie.Count; i++)
    {
        FasciaOraria fasciaOrariaCorrente = fasceOrarie[i];
        bool nomeUguale = string.Equals(
            fasciaOrariaCorrente.Nome,
            nome,
            StringComparison.OrdinalIgnoreCase);
        if(nomeUguale || (fasciaOrariaCorrente.OraInizio == oraInizio && fasciaOrariaCorrente.OraFine == oraFine))
        {
            return;
        }

    }

    FasciaOraria nuovaFasciaOraria = new FasciaOraria
    {
        Nome      = nome,
        OraInizio = oraInizio,
        OraFine   = oraFine
    };

    context.FasceOrarie.Add(nuovaFasciaOraria);
    await context.SaveChangesAsync();
  }
}