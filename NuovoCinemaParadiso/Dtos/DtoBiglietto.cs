namespace NuovoCinemaParadiso.Dtos;
public class DtoBiglietto
{
    public string? Id { get; set; }
    public string UtenteId { get; set; } = string.Empty;
    public int PrezzoFinale {get;set;}
    public DateTimeOffset OrarioCreazione {get;set;} 
    public string ProiezioneId {get;set;} = string.Empty;
    public int NumeroBiglietti {get;set;}
    public string NomeSala {get;set;} = string.Empty;
    public string TitoloMovie {get;set;} = string.Empty;
    public string NomeTipologiaSala {get;set;} = string.Empty;
    public TimeOnly OraInizio {get;set;}
    public DateOnly DataProiezione {get;set;}
}