using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NuovoCinemaParadiso.Models;

[Table("Generi")]
public class GenereMovie
{
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString();
    [Required]
    [StringLength(15)]
    public string Genere {get;set;} = string.Empty;
    public List<Movie> Movies {get;set;} = new List<Movie>();
}