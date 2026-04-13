namespace NuovoCinemaParadiso.Dtos;

public class DtoFasciaOraria
{
    public string Id { get; set; }
    public TimeSpan OraInizio { get; set; }
    public TimeSpan OraFine { get; set; }
    public string Nome { get; set; } = string.Empty; // es: "Sera", "Pomeriggio"
}