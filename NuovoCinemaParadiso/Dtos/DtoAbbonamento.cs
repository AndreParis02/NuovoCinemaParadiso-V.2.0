namespace NuovoCinemaParadiso.Dtos;

public class DtoAbbonamento
{
    public string Id {get; set;}
    public string Nome {get; set;} = string.Empty;
    public int Durata{get; set;}
    public decimal Prezzo {get; set;}
    public int Sconto {get; set;}
}