using System.ComponentModel.DataAnnotations;

namespace NuovoCinemaParadiso.Dtos;

public class DtoContoCinema
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Iban { get; set; }
    public string TitolareConto {get;set;}
    public int Conto {get;set;}


}