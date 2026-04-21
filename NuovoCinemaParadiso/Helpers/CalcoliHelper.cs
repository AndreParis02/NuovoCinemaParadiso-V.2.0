using NuovoCinemaParadiso.Models;

namespace NuovoCinemaParadiso.Helpers;

public static class Calcoli
{
    public static Decimal CalcolaPrezzoFinale(decimal prezzoMovie, decimal maggiorazione, int numeroBiglietti, Utente utente, string metodoPagamento)
    {
        decimal prezzoBiglietto = prezzoMovie + maggiorazione;

        if (metodoPagamento == "abbonamento" && utente.SeAbbonato)
        {
            decimal sconto = (prezzoBiglietto / 100) * utente.Abbonamento.Sconto;
            decimal prezzoScontato = prezzoBiglietto - sconto;
            return prezzoScontato * numeroBiglietti;
        }
        else if (metodoPagamento == "giftcard" && utente.PossiedeGiftCard)
        {
            if (utente.GiftCard.NumeroMovie > numeroBiglietti)
            {
                utente.GiftCard.NumeroMovie = utente.GiftCard.NumeroMovie - numeroBiglietti;
                return 0;
            }
            else if (utente.GiftCard.NumeroMovie == numeroBiglietti)
            {
                utente.GiftCard.NumeroMovie = 0;
                utente.PossiedeGiftCard = false;
                return 0;
            }
            else
            {
                int bigliettiRimanenti = numeroBiglietti - utente.GiftCard.NumeroMovie;
                utente.GiftCard.NumeroMovie = 0;
                utente.PossiedeGiftCard = false;
                return prezzoBiglietto * bigliettiRimanenti;
            }
        }
        else
        {
            return prezzoBiglietto * numeroBiglietti;   
        }
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
}