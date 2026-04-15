using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace NuovoCinemaParadiso.Models;

[Table("LogsAzioni")]
public class LogAzioni
{
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString();

    public string IdUtente {get;set;} = string.Empty;

    public string NomeAzione {get;set;} = string.Empty;

    public bool Effettuato {get;set;}

    public string Messaggio {get;set;} = string.Empty;

    public DateTimeOffset TimeStamp {get;set;}
}