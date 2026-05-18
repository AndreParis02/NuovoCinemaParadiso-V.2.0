using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NuovoCinemaParadiso.Models;

[Table("GiftCard")]
public class GiftCard
{
    [Key]
    public string Id {get;set;} = Guid.NewGuid().ToString();
    [Required]
    [StringLength(50)]
    public string Nome {get;set;} = string.Empty;

    [Required]
    public bool Riscattata{get;set;} = false;

    [Required]
    [Range(0, 1000)]
    public int Saldo{get;set;}

    [Required]
    public string CodiceRiscatto{get;set;} = string.Empty;

}