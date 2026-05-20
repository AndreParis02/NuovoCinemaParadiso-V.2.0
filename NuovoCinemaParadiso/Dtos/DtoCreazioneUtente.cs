using System.ComponentModel.DataAnnotations;

namespace NuovoCinemaParadiso.Dtos;
public class DtoCreazioneUtente
{
    [Required]
    [StringLength(100)]
    public string NomeCompleto {get; set;} = string.Empty;
    
    [Required]
    [Range(14, 100, ErrorMessage = "L'età deve essere compresa tra 14 e 100")]
    public int Eta {get; set;} 
}