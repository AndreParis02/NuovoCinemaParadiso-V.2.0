using System.ComponentModel.DataAnnotations;

namespace NuovoCinemaParadiso.Dtos;

public class DtoCreazioneAcquisto
{
    [Required]
    public string MovieId { get; set; } = string.Empty;
    [Required]
    public string SalaId { get; set; } = string.Empty;

    public string? UtenteId { get; set; } = string.Empty;
    [Required]
    public int NumeroBiglietti { get; set; }
    [Required]
    public decimal PrezzoFinale { get; set; }

}