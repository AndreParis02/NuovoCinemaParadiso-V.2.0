namespace NuovoCinemaParadiso.Dtos;
public class DtoTipologiaSala
{
    public string? Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public int MaggiorazionePrezzo { get; set; }
    public bool IsDeleted {get; set;} = false;

}