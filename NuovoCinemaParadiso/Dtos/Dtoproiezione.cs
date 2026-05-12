namespace NuovoCinemaParadiso.Dtos;

public class DtoProiezione
{
    public string Id {get; set; } = string.Empty;
    public DateOnly DataProiezione {get; set; }
    public string MovieId {get; set; } = string.Empty;
    public string SalaId {get; set; } = string.Empty;
    public string TurnoId {get; set; } = string.Empty;

    public bool Attivo { get; set; }=true;
}