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
    public List<Biglietto> Biglietti { get; set; } = new List<Biglietto>();

    [Range(0, 10000)]
    public int Saldo { get; set; }
    public List<UtenteAbbonamento> UtentiAbbonamenti { get; set; } = new();
}