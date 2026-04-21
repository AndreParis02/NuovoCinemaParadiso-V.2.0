using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace NuovoCinemaParadiso.Models;

[Table("Proiezioni")]
public class Proiezione
{
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString();

    [Required]
    public DateOnly DataProiezione { get; set; }

    [Required]
    public string MovieId { get; set; } = string.Empty;

    [ForeignKey("MovieId")]
    public Movie? Movie { get; set; }

    [Required]
    public string SalaId { get; set; } = string.Empty;

    [ForeignKey("SalaId")]
    public Sala? Sala { get; set; }

    [Required]
    public string TurnoId { get; set; } = string.Empty;

    [ForeignKey("TurnoId")]
    public Turno? Turno { get; set; }

    public List<Acquisto> Acquisti { get; set; } = new List<Acquisto>();
}