namespace NuovoCinemaParadiso.Dtos;
public class DtoCreazioneProiezione
{
    public string MovieId { get; set; } = string.Empty;
    public string SalaId { get; set; } = string.Empty;
    public string TurnoId { get; set; } = string.Empty;
    public DateOnly DataProiezione { get; set; }
}