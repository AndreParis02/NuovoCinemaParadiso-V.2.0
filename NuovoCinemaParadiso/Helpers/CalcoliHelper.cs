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
            Decimal sconto = (prezzoBiglietto / 100) * utente.Abbonamento.Sconto;
            Decimal prezzoScontato = prezzoBiglietto - sconto;
            Decimal prezzoFinale = prezzoScontato * numeroBiglietti;
            return prezzoFinale;
        }
    }
}