namespace NuovoCinemaParadiso.Dtos;
public class DtoMovie
{
    public string? Id {get;set;}
    public string Titolo {get;set;} = string.Empty;
    public string Descrizione {get;set;} = string.Empty;
    public int DurataMinuti {get;set;}
    public int PrezzoMovie {get;set;}
    public string GenereId {get;set;} = string.Empty;
    public string Genere {get;set;} = string.Empty;
    public bool IsDeleted {get;set;} = false;
}