
namespace NuovoCinemaParadiso.Helpers;

public static class CalcolaPrezzo
{
    public static Decimal CalcolaPrezzoFinale(Decimal prezzoMovie, Decimal prezzoSala, int numeroBiglietti)
    {
        Decimal prezzoFinale = (prezzoMovie + prezzoSala) * numeroBiglietti;
        return prezzoFinale;
    }
}