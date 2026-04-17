using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NuovoCinemaParadiso.Models;

[Table("Movies")]
public class Movie
{
    [Key]
    public string Id {get;set;} = Guid.NewGuid().ToString();
    [Required]
    [StringLength(50)]
    public string Titolo {get;set;} = string.Empty;
    [Required]
    [StringLength(2000)]
    public string Descrizione {get;set;} = string.Empty;
    public int DurataMinuti {get;set;} 
    public decimal PrezzoMovie {get;set;}
    public List<Acquisto> Acquisti {get;set;} = new List<Acquisto>();
    public string GenereId {get;set;} = string.Empty;
    
    [ForeignKey("GenereId")]
    public GenereMovie? Genere {get;set;}
}