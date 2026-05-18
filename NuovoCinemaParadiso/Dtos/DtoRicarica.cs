using System.ComponentModel.DataAnnotations;

namespace NuovoCinemaParadiso.Dtos;

public class DtoRicarica
{
    [Required]
    [EmailAddress]
    public string Email {get; set;} = string.Empty;

    [Range(0,1000)]
    public int Ricarica{get;set;}
}