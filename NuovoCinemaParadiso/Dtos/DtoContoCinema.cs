namespace NuovoCinemaParadiso.Dtos;
public class DtoContoCinema
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Iban { get; set; } = string.Empty;
    public string TitolareConto {get;set;} = string.Empty;
    public int Conto {get;set;}
}