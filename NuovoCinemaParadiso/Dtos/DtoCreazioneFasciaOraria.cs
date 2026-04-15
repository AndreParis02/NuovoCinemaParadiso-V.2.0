using System.ComponentModel.DataAnnotations;

namespace NuovoCinemaParadiso.Dtos;

public class DtoCreazioneFasciaOraria
{    
    [Required]
    public DateTime Data { get; set; }
    [Required]
    public TimeSpan OraInizio { get; set; }

    [Required]
    public TimeSpan OraFine { get; set; }
    
    [StringLength(50)]
    public string Nome { get; set; } = string.Empty; // es: "Sera", "Pomeriggio"
}