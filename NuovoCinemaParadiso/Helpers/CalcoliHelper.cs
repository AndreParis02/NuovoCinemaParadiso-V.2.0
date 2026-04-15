using NuovoCinemaParadiso.Models;

namespace NuovoCinemaParadiso.Helpers;

public static class Calcoli
{
    public static Decimal CalcolaPrezzoFinale(Decimal prezzoMovie, Decimal prezzoSala, int numeroBiglietti, Utente utente)
    {
        if (utente.SeAbbonato == false)
        {
            Decimal prezzoFinale = (prezzoMovie + prezzoSala) * numeroBiglietti;
            return prezzoFinale;
        }
        else
        {
            Decimal prezzoBiglietto = prezzoMovie + prezzoSala;
            Decimal prezzoScontato = (prezzoBiglietto / 100) * utente.Abbonamento.Sconto;
            Decimal prezzoFinale = prezzoScontato * numeroBiglietti;
            return prezzoFinale;
        }
    }

    public static DateTime? CalcolaScadenzaAbbonamento(DateOnly dataInizio, int durata)
    {
       return dataInizio.AddMonths(durata).ToDateTime(TimeOnly.MinValue);
    }

    public static int GiorniAllaScadenza(DateTime dataInizio, int durata)
    {
        DateTime dataScadenza = dataInizio.AddMonths(durata);
        TimeSpan differenza = dataScadenza - DateTime.Now;
        return (int)differenza.TotalDays;
    }
}