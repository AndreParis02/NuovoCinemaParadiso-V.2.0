using NuovoCinemaParadiso.Models;

namespace NuovoCinemaParadiso.Helpers;

public static class Calcoli
{
    public static Decimal CalcolaPrezzoFinale(decimal prezzoMovie, decimal maggiorazione, int numeroBiglietti, Utente utente)
    {
        if (utente.SeAbbonato == false)
        {
            Decimal prezzoFinale = (prezzoMovie + maggiorazione) * numeroBiglietti;
            return prezzoFinale;
        }
        else if (utente.PossiedeGiftCard == true)
        {
            if (utente.GiftCard.NumeroMovie > numeroBiglietti)
            {
                Decimal prezzoFinale = 0;
                utente.GiftCard.NumeroMovie = utente.GiftCard.NumeroMovie - numeroBiglietti;
                return prezzoFinale;
            }
            else if (utente.GiftCard.NumeroMovie == numeroBiglietti)
            {
                Decimal prezzoFinale = 0;
                utente.GiftCard.NumeroMovie = utente.GiftCard.NumeroMovie - numeroBiglietti;
                utente.PossiedeGiftCard = false;
                return prezzoFinale;
            }
            else
            {
                int bigliettiRimanenti = numeroBiglietti - utente.GiftCard.NumeroMovie;
                Decimal prezzoFinale = (prezzoMovie + maggiorazione) * bigliettiRimanenti;
                utente.PossiedeGiftCard = false;
                return prezzoFinale;
            }
        }
        else
        {
            Decimal prezzoFinale = (prezzoMovie + maggiorazione) * numeroBiglietti;
            return prezzoFinale;
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