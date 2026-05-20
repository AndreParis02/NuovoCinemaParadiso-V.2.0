using Microsoft.AspNetCore.Http.HttpResults;
using NuovoCinemaParadiso.Models;
using NuovoCinemaParadiso.Data;

namespace NuovoCinemaParadiso.Helpers;

public static class Calcoli
{
    public static int CalcolaPrezzoFinale(int prezzoMovie, int maggiorazione, int numeroBiglietti, Utente utente, string metodoPagamento)
    {
        int prezzoBiglietto = prezzoMovie + maggiorazione;

        if (metodoPagamento == "abbonamento" && utente.SeAbbonato && utente.Abbonamento != null)
        {
            int sconto = (prezzoBiglietto / 100) * utente.Abbonamento.Sconto;
            int prezzoScontato = prezzoBiglietto - sconto;
            return prezzoScontato * numeroBiglietti;
        }
        else
        {
            return prezzoBiglietto * numeroBiglietti;   
        }
    }

    public static async Task<int[]> CalcolaSaldo(int prezzo, Utente utente, ContoCinema contoCinema)
    {
        utente.Saldo = utente.Saldo - prezzo;
        contoCinema.Conto = contoCinema.Conto + prezzo;
        return new int[] { utente.Saldo, contoCinema.Conto };
    }
   
    public static DateTimeOffset? CalcolaScadenza(DateTimeOffset dataInizio, int durata)
    {
        return dataInizio.AddMonths(durata);
    }

    public static int GiorniAllaScadenza(DateTimeOffset dataInizio, int durata)
    {
        DateTimeOffset dataScadenza = dataInizio.AddMonths(durata);
        TimeSpan differenza = dataScadenza - DateTime.Now;
        return (int)differenza.TotalDays;
    }

    public static int CaricaGiftCard(Utente utente, GiftCard giftCard)
    {
        if(giftCard.Valore > utente.Saldo)
        {
           throw new Exception("Saldo utente non sufficente");
        }
        
        return utente.Saldo = utente.Saldo - giftCard.Valore;    
    }
}