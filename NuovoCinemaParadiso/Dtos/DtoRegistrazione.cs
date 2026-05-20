using System.ComponentModel.DataAnnotations;

namespace NuovoCinemaParadiso.Dtos;
public class DtoRegistrazione
{
    [Required]
    [EmailAddress]
    public string Email {get; set;} = string.Empty;

    [Required]
    [StringLength(100, MinimumLength = 6)]
    public string Password {get;set;} = string.Empty;

    [Required]
    [StringLength(100)]
    public string NomeCompleto {get; set;} = string.Empty;
    public int Eta {get;set;}

    [Range(0,10000)]
    public int Saldo{get;set;}
}