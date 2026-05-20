using NuovoCinemaParadiso.Models;

namespace NuovoCinemaParadiso.Helpers;

public static class Calcoli
{
    public static int CalcolaPrezzoFinale(int prezzoMovie, int maggiorazione, int numeroBiglietti, Abbonamento abbonamento, DateTimeOffset dataInizioAbbonamento)
    {
        int prezzoBiglietto = prezzoMovie + maggiorazione;
        DateTimeOffset dataScadenzaAbbonamento = dataInizioAbbonamento.AddMonths(abbonamento.Durata);
        if (abbonamento != null && DateTimeOffset.UtcNow < dataScadenzaAbbonamento)
        {
            
           int sconto = (prezzoBiglietto * abbonamento.Sconto) / 100;
           int prezzoScontato = prezzoBiglietto - sconto;
            
            return prezzoScontato * numeroBiglietti;
        }
        else
        {
            return prezzoBiglietto * numeroBiglietti;   
        }
    }

    public static void CalcolaSaldo(int prezzo, Utente utente, ContoCinema contoCinema)
    {
        utente.Saldo = utente.Saldo - prezzo;
        contoCinema.Conto = contoCinema.Conto + prezzo;
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