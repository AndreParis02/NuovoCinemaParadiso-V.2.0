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
        else
        {
            Decimal prezzoBiglietto = prezzoMovie + maggiorazione;
            Decimal sconto = (prezzoBiglietto / 100) * utente.Abbonamento.Sconto;
            Decimal prezzoScontato = prezzoBiglietto - sconto;
            Decimal prezzoFinale = prezzoScontato * numeroBiglietti;
            return prezzoFinale;
        }
    }

    public static DateTimeOffset? CalcolaScadenzaAbbonamento(DateTimeOffset dataInizio, int durata)
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