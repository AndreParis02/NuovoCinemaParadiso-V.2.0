using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NuovoCinemaParadiso.Models;

[Table("Sala")]
public class Sala
{
    [Key]
    public string Id {get; set;} = Guid.NewGuid().ToString();
    [Required]
    [StringLength(100)]
    public string Nome {get; set;} = string.Empty;
    public int Capienza {get; set;}
    public string FasciaOrariaId { get; set; }
    [ForeignKey("FasciaOrariaId")]
    public FasciaOraria FasciaOraria { get; set; }
    public List<Acquisto> Acquisti {get; set;} = new List<Acquisto>();
    public string TipologiaSalaId {get; set;} = string.Empty;
    [ForeignKey("TipologiaSalaId")]
    public TipologiaSala TipologiaSala {get; set;}
}