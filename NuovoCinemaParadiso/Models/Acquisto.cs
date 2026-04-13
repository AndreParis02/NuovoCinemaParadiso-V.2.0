using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace NuovoCinemaParadiso.Models;

[Table("Acquisto")]
public class Acquisto
{
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString();

    [Required]
    public string MovieId { get; set; } = string.Empty;
    // collegamento a film, sala e utente.

    [ForeignKey("MovieId")]
    public Movie Movie { get; set; }

    [Required]
    public string SalaId { get; set; } = string.Empty;

    [ForeignKey("SalaId")]
    public Sala Sala { get; set; }

    [Required]
    public string UtenteId { get; set; } = string.Empty;

    [ForeignKey("UtenteId")]
    public Utente Utente { get; set; }

    [Required]
    public int NumeroBiglietti {get;set;}
    public DateTime OrarioCreazione { get; set; } = DateTime.UtcNow;
    [Required]
    public decimal PrezzoFinale {get; set;}
}