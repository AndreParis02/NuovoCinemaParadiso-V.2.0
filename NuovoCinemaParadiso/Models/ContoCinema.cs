using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;
namespace NuovoCinemaParadiso.Models;

[Table("ContoCinema")]
public class ContoCinema
{
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString();
    
    [Required]
    [StringLength(34, MinimumLength = 22, ErrorMessage = "La lunghezza dell'iban deve essere compresa tra 22 e 34.")]
    public string Iban { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string TitolareConto {get;set;} = string.Empty;

    [Required]
    public int Conto {get;set;}
}