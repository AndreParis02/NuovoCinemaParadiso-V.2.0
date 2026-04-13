using System.ComponentModel.DataAnnotations;

namespace NuovoCinemaParadiso.Dtos;

public class DtoCreazioneMovie
{
    [Required]
    [StringLength(50)]
    public string Titolo { get; set; } = string.Empty;
    [Required]
    [StringLength(200)]
    public string Descrizione { get; set; } = string.Empty;
    public int DurataMinuti { get; set; }
    public decimal PrezzoMovie {get;set;}
    public string GenereId {get;set;} = string.Empty;
}