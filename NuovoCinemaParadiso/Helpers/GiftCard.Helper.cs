
namespace NuovoCinemaParadiso.Helpers;

public static class GiftCardHelper
{
    public static string GeneraCodice()
    {
        return Guid.NewGuid()
        .ToString()
        .Substring(0, 8)
        .ToUpper();
    }

}