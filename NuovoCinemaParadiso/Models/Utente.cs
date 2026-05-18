using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NuovoCinemaParadiso.Models;

[Table("Utenti")]
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
    [Required]
    public bool PossiedeGiftCard { get; set; } = false;
    public DateTimeOffset DataInizioAbbonamento { get; set; }
    public DateTimeOffset DataInizioGiftCard { get; set; }
    public List<Biglietto> Biglietti { get; set; } = new List<Biglietto>();
    public string? AbbonamentoId { get; set; }
    [ForeignKey("AbbonamentoId")]
    public Abbonamento? Abbonamento { get; set; }
    public string? GiftCardId { get; set; }
    [ForeignKey("GiftCardId")]
    public GiftCard? GiftCard { get; set; }
}