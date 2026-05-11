using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NuovoCinemaParadiso.Models;

[Table("GiftCards")]
public class GiftCard
{
    [Key]
    public string Id {get;set;} = Guid.NewGuid().ToString();
    [Required]
    [StringLength(50)]
    public string Nome {get;set;} = string.Empty;
    [Required]
    public int Durata {get;set;} 
    [Required]
    public decimal Prezzo {get;set;}
    [Required]
    public int NumeroMovie {get;set;}
    public List<Utente> Utenti {get;set;} = new List<Utente>();
}