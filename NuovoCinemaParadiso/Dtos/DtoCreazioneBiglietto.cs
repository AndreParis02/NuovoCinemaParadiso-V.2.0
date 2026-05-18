using System.ComponentModel.DataAnnotations;

namespace NuovoCinemaParadiso.Dtos;

public class DtoCreazioneBiglietto
{
    [Required]
    public string ProiezioneId { get; set; } = string.Empty;
    [Required]
    [Range(1, 100, ErrorMessage = "Il numero di biglietti deve essere maggiore di zero e massimo 100.")]
    public int NumeroBiglietti { get; set; }
    public string MetodoPagamento { get; set; } = "standard";
}