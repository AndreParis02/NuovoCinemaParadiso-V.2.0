using NuovoCinemaParadiso.Models;
namespace NuovoCinemaParadiso.Helpers;

public static class CalcolaPrezzo
{
    public static Decimal CalcolaPrezzoFinale(Decimal prezzoMovie, Decimal prezzoSala, int numeroBiglietti, Utente utente)
    {
        if (utente.AbbonamentoId == null)
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
}