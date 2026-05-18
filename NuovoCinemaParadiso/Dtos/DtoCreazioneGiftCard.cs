namespace NuovoCinemaParadiso.Dtos;

public class DtoCreazioneGiftCard
{
    public string Nome { get; set; } = string.Empty;
    public decimal Saldo { get; set; }
    public int CodiceRiscatto { get; set; }
}