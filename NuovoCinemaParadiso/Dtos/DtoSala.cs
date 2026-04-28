using System.ComponentModel.DataAnnotations;

namespace NuovoCinemaParadiso.Dtos;

public class DtoSala
{
    public string? Id {get; set;}

    [StringLength(100)]
    public string Nome {get; set;} = string.Empty;
    public int Capienza {get; set;}
    public string TipologiaSalaId {get; set;} = string.Empty;
    public string NomeTipologia {get; set;} = string.Empty;
}