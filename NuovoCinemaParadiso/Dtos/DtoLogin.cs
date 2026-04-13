using System.ComponentModel.DataAnnotations;

namespace NuovoCinemaParadiso.Dtos;

public class DtoLogin
{
    [Required]
    [EmailAddress]
    public string Email {get; set;} = string.Empty;

    [Required]
    [StringLength(100, MinimumLength = 6)]
    public string Password {get;set;} = string.Empty;
}