using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NuovoCinemaParadiso.Models;

[Table("Utente")]
public class Utente : IdentityUser
{
    [Required]
    [StringLength(100)]
    public string NomeCompleto {get; set;} = string.Empty; 
    [Required]
    [Range(14, 100, ErrorMessage = "L'età deve essere compresa tra 14 e 100")]
    public int Eta {get; set;}
    public List <Acquisto> Acquisti {get; set;} = new List <Acquisto>();
} 