using System.ComponentModel.DataAnnotations;

namespace NuovoCinemaParadiso.Dtos;

public class DtoCreazioneGenereMovie
{
    [Required]
    [StringLength(15)]
    public string Genere {get;set;} = string.Empty;
}