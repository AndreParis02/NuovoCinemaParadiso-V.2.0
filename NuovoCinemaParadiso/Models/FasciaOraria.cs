using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NuovoCinemaParadiso.Models;

[Table("FasciaOraria")]
public class FasciaOraria
{
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString();

    [Required]
    public TimeOnly OraInizio { get; set; }

    [Required]
    public TimeOnly OraFine { get; set; }

    [StringLength(50)]
    public string Nome { get; set; } = string.Empty; // es: "Sera", "Pomeriggio"

    public List<Sala> Sale { get; set; } = new();
}