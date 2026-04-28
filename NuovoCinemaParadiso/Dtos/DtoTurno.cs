namespace NuovoCinemaParadiso.Dtos;

public class DtoTurno
{
    public string? Id { get; set; }
    public TimeOnly OraInizio { get; set; }
    public TimeOnly OraFine { get; set; }
    public string Nome { get; set; } = string.Empty; // es: "Sera", "Pomeriggio"
}