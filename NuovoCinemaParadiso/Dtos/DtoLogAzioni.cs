namespace NuovoCinemaParadiso.Dtos;
public class DtoLogAzioni
{
    public string Id { get; set; } = string.Empty;
    public string IdUtente { get; set; } = string.Empty;
    public string NomeAzione { get; set; } = string.Empty;
    public bool Effettuato { get; set; }
    public string Messaggio { get; set; } = string.Empty;

    public DateTimeOffset TimeStamp { get; set; }

}