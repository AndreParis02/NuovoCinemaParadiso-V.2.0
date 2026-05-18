namespace NuovoCinemaParadiso.Dtos;

public class DtoGiftCard
{
    public string? Id {get; set;}
    public string Nome {get; set;} = string.Empty;
    public int Saldo {get; set;}
    public string CodiceRiscatto {get; set;} = string.Empty;
}