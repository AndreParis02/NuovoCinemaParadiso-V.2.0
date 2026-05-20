using System.ComponentModel.DataAnnotations;

namespace NuovoCinemaParadiso.Dtos;
public class DtoCreazioneContoCinema
{
    [Required]
    [StringLength(34, MinimumLength = 22, ErrorMessage = "La lunghezza dell'iban deve essere compresa tra 22 e 34.")]
    public string Iban { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string TitolareConto {get;set;} = string.Empty;

    [Required]
    public int Conto {get;set;}
}