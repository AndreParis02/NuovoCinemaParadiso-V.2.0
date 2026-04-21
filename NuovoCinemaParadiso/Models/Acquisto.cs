using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace NuovoCinemaParadiso.Models;

[Table("Acquisti")]
public class Acquisto
{
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString();

    [Required]
    public string ProiezioneId { get; set; } = string.Empty;

    [ForeignKey("ProiezioneId")]
    public Proiezione? Proiezione { get; set; }

    [Required]
    public string UtenteId { get; set; } = string.Empty;

    [ForeignKey("UtenteId")]
    public Utente? Utente { get; set; }

    [Required]
    public int NumeroBiglietti {get;set;}
    public DateTimeOffset OrarioCreazione { get; set; } = DateTimeOffset.UtcNow;

    [Required]
    public decimal PrezzoFinale {get; set;}
    
    [Required]
    public string MetodoPagamento { get; set; } = "standard";
}