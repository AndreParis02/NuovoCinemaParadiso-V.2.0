using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NuovoCinemaParadiso.Models;

[Table("Utente")]
public class Utente : IdentityUser
{
    [Required]
    [StringLength(100)]
    public string NomeCompleto { get; set; } = string.Empty;
    [Required]
    [Range(14, 100, ErrorMessage = "L'età deve essere compresa tra 14 e 100")]
    public int Eta { get; set; }
    [Required]
    public bool SeAbbonato { get; set; } = false;

    public DateTime DataInizio { get; set; } = DateTime.UtcNow;
    public List<Acquisto> Acquisti { get; set; } = new List<Acquisto>();

    public string? AbbonamentoId { get; set; } = string.Empty;
    [ForeignKey("AbbonamentoId")]
    public Abbonamento? Abbonamento { get; set; }
}