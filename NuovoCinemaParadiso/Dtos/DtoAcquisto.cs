namespace NuovoCinemaParadiso.Dtos;
public class DtoAcquisto
{
    public string Id { get; set; }
    public string MovieId { get; set; } = string.Empty;
    public string Titolo{get;set;} = string.Empty;
    public string SalaId { get; set; } = string.Empty;
    public string Nome {get;set;} = string.Empty;
    public string UtenteId { get; set; } = string.Empty;
    public string NomeCompleto{get;set;} = string.Empty;
    public decimal PrezzoFinale {get;set;}
    public DateTimeOffset OrarioCreazione {get;set;} 
    public int NumeroBiglietti {get;set;}
}