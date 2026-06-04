using System.ComponentModel.DataAnnotations;

namespace NuovoCinemaParadiso.Dtos;
public class DtoCreazioneMovie
{
    [Required]
    [StringLength(50)]
    public string Titolo { get; set; } = string.Empty;
    [Required]
    [StringLength(200)]
    public string Descrizione { get; set; } = string.Empty;
    [Range(1, int.MaxValue, ErrorMessage = "La durata deve essere un numero intero positivo maggiore di 0.")]
    public int DurataMinuti { get; set; }
    [Range(0, double.MaxValue, ErrorMessage = "Il prezzo deve essere un numero positivo.")]
    public int PrezzoMovie { get; set; }
    [Required]
    public string GenereId { get; set; } = string.Empty;
}