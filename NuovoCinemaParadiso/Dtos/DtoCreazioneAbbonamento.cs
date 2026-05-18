namespace NuovoCinemaParadiso.Dtos;

public class DtoCreazioneAbbonamento
{
    public string Nome { get; set; } = string.Empty;
    public int Durata { get; set; } 
    public int Prezzo { get; set; }
    public int Sconto { get; set; }
}