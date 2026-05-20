namespace NuovoCinemaParadiso.Dtos;
public class DtoBiglietto
{
    public string? Id { get; set; }
    public string ProiezioneId { get; set; } = string.Empty;
    public string UtenteId { get; set; } = string.Empty;
    public int PrezzoFinale {get;set;}
    public DateTimeOffset OrarioCreazione {get;set;} 
    public int NumeroBiglietti {get;set;}
}