namespace NuovoCinemaParadiso.Dtos;

public class DtoUtente
{
    public string Id { get; set; } = string.Empty;
    public string NomeCompleto { get; set; } = string.Empty;
    public DateTime DataInizio {get; set;}
    public bool Abbonato {get; set;}
    public string Email { get; set; } = string.Empty;
    public int Eta { get; set; }
}