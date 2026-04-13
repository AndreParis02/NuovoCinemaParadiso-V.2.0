namespace NuovoCinemaParadiso.Dtos;

public class DtoAbbonamento
{
    public string id {get; set;}
    public string Nome {get; set;} = string.Empty;
    public DateTime DataDiInizio {get; set;}
    public int Durata{get; set;}
    public DateTime DataDiFine {get; set;}
    public decimal Prezzo {get; set;}
    public int Sconto {get; set;}

}