namespace NuovoCinemaParadiso.Dtos;

public class DtoAuthResponse
{
   public string Id { get; set; } = string.Empty;
   public string NomeCompleto { get; set; } = string.Empty;
   public string Token { get; set; } = string.Empty;
   public int Eta { get; set; }
   public string Email { get; set; } = string.Empty;
   public string Ruolo { get; set; } = string.Empty;
   public DateTimeOffset DataInizioAbbonamento { get; set; }
   public DateTimeOffset DataInizioGiftCard { get; set; }
   public bool SeAbbonato { get; set; }
   public bool PossiedeGiftCard { get; set; } = false;


}