using System.ComponentModel.DataAnnotations;

namespace NuovoCinemaParadiso.Dtos;

public class DtoCreazioneAcquisto
{
    [Required]
    public string ProiezioneId { get; set; } = string.Empty;
    [Required]
    public int NumeroBiglietti { get; set; }
    
}