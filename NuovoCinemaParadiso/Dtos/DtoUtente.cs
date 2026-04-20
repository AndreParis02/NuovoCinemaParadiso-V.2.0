namespace NuovoCinemaParadiso.Dtos;

public class DtoUtente
{
    public string Id { get; set; } = string.Empty;
    public string NomeCompleto { get; set; } = string.Empty;
    public DateTimeOffset DataInizio { get; set; }
    public DateTimeOffset DataInizioGiftCard { get; set; }
    public bool SeAbbonato { get; set; }
    public bool PossiedeGiftCard { get; set; } = false;

    public string Email { get; set; } = string.Empty;
    public int Eta { get; set; }
    public string AbbonamentoId { get; set; } = string.Empty;
    public string GiftCardId { get; set; } = string.Empty;
    public string TipoAbbonamento { get; set; } = string.Empty;
    public string TipoGiftCard { get; set; } = string.Empty;
}