using System.ComponentModel.DataAnnotations;

namespace NuovoCinemaParadiso.Dtos;
public class DtoCreazioneTipologiaSala
{
    [Required]
    public string Nome { get; set; } = string.Empty;
    public int MaggiorazionePrezzo { get; set; }
}