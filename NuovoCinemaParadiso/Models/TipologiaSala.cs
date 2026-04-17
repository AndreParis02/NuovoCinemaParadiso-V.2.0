using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NuovoCinemaParadiso.Models;

[Table("TipologieSala")]
public class TipologiaSala
{
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString();
    [Required]
    public string Nome { get; set; } = string.Empty;
    public decimal MaggiorazionePrezzo { get; set; }
    public List<Sala> Sale { get; set; } = new List<Sala>();
}