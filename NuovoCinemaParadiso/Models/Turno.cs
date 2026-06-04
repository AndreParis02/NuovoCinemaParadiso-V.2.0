using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NuovoCinemaParadiso.Models;

[Table("Turni")]
public class Turno
{
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString();
    
    [Required]
    public TimeOnly OraInizio { get; set; }

    [Required]
    public TimeOnly OraFine { get; set; }
    public List<Proiezione> Proiezioni {get;set;} = new List<Proiezione>();

    [StringLength(50)]
    public string Nome { get; set; } = string.Empty; // es: "Sera", "Pomeriggio"

}