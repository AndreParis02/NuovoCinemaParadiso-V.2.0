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
    [Range(1, int.MaxValue)]
    public int DurataMinuti {get;set;} 
    [Range(typeof(decimal), "0.01", "999999999")]
    public decimal PrezzoMovie {get;set;}
    public List<Proiezione> Proiezioni {get;set;} = new List<Proiezione>();
    public string GenereId {get;set;} = string.Empty;
    
    [ForeignKey("GenereId")]
    public GenereMovie? Genere {get;set;}
}