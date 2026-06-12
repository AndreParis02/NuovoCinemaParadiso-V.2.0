using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NuovoCinemaParadiso.Models;

[Table("UtenteAbbonamento")]
public class UtenteAbbonamento
{

    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString();

    [Required]
    public string AbbonamentoId { get; set; } = string.Empty;
    [ForeignKey("AbbonamentoId")]
    public Abbonamento? Abbonamento { get; set; }

    [Required]
    public string UtenteId { get; set; } = string.Empty;
    [ForeignKey("UtenteId")]
    public Utente? Utente { get; set; }

    public DateTimeOffset DataInizioAbbonamento { get; set; }
    public DateTimeOffset DataFine { get; set; }

}