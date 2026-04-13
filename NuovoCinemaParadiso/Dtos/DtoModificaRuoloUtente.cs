using System.ComponentModel.DataAnnotations;

namespace NuovoCinemaParadiso.Dtos;

public class DtoModificaRuoloUtente
{
    [Required]
    [EmailAddress]
    public string Email {get; set;} = string.Empty;
    [Required]
    public string NuovoRuolo {get; set;} = string.Empty;
}