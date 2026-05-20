using System.ComponentModel.DataAnnotations;

namespace NuovoCinemaParadiso.Dtos;
public class DtoCreazioneTurno
{    
    [Required]
    public TimeOnly OraInizio { get; set; }

    [Required]
    public TimeOnly OraFine { get; set; }
    
    [StringLength(50)]
    public string Nome { get; set; } = string.Empty; // es: "Sera", "Pomeriggio"
}