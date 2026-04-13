namespace NuovoCinemaParadiso.Dtos;

public class DtoCreazioneAbbonamento
{
    public string Nome { get; set; } = string.Empty;
    public int Durata { get; set; } 
    public DateTime DataInizio { get; set; }
    public DateTime DataFine { get; set; }
    public decimal Prezzo { get; set; }
    public int Sconto { get; set; }
}