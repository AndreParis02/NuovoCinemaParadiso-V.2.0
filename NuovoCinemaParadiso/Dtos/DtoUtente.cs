namespace NuovoCinemaParadiso.Dtos;
public class DtoUtente
{
    public string Id { get; set; } = string.Empty;
    public string NomeCompleto { get; set; } = string.Empty;
    public DateTimeOffset? DataInizioAbbonamento { get; set; }
    public bool SeAbbonato { get; set; }
    public string Email { get; set; } = string.Empty;
    public int Eta { get; set; }
    public string AbbonamentoId { get; set; } = string.Empty;
    public string TipoAbbonamento { get; set; } = string.Empty;
    public int Saldo{get;set;}
}