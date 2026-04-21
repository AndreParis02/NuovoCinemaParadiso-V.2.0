using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using NuovoCinemaParadiso.Dtos;
using NuovoCinemaParadiso.Services;

namespace NuovoCinemaParadiso.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TurnoController : ControllerBase
{
    private readonly TurnoService _turnoService;
    private readonly LogAzioniService _logAzioniService;

    public TurnoController(TurnoService turnoService, LogAzioniService logAzioniService)
    {
        _turnoService = turnoService;
        _logAzioniService = logAzioniService;
    }

    [HttpGet]
    public async Task<IActionResult> OttieniTutti()
    {
        List<DtoTurno> turni = await _turnoService.OttieniTuttoAsync();
        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        await Log(utenteId, "Ottieni tutti i turni", true);

        return Ok(turni);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> OttieniTramiteId(string id)
    {
        var risultato = await _turnoService.OttieniTramiteIdAsync(id);
        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (risultato == null)
        {
            await Log(utenteId, "Ottieni turno tramite id", false);

            return NotFound($"Turno con id {id} non trovato");
        }

        await Log(utenteId, "Ottieni turno tramite id", true);

        return Ok(risultato);
    }

    [HttpPost]
    [Authorize(Roles = Ruoli.GestoreOrOperatore)]
    public async Task<IActionResult> Creazione([FromBody] DtoCreazioneTurno dto)
    {

        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        List<DtoTurno> turni = await _turnoService.OttieniTuttoAsync();//legge la lista di tutti i turni


        //controlla che il nome turno fornito dal DTO non sia già presente nella lista dei turni del db. Se trovato, restituisce errore
        foreach (var turno in turni)
        {
            bool stringheUguali = string.Equals(turno.Nome, dto.Nome, StringComparison.OrdinalIgnoreCase);
            if (stringheUguali)
            {
                await Log(utenteId, "Creazione Turno", false);

                return BadRequest(new { messaggio = "Turno già presente." });
            }
        }

        DtoTurno? risultato = await _turnoService.CreazioneAsync(dto);//creazione nuovo turno. Se fallisce, ritorna errore

        if (risultato == null)
        {
            await Log(utenteId, "Creazione Turno", false);

            return BadRequest(new { messaggio = "Turno non valido." });
        }

        await Log(utenteId, "Creazione Turno", true);

        return Ok(risultato);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = Ruoli.GestoreOrOperatore)]
    public async Task<IActionResult> Modifica(string id, [FromBody] DtoCreazioneTurno dto)
    {
        DtoTurno? risultato = await _turnoService.ModificaAsync(id, dto);
        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (risultato == null)
        {
            await Log(utenteId, "Modifica Turno", false);

            return NotFound(new { messaggio = "Turno non trovato." });
        }

        await Log(utenteId, "Modifica Turno", true);

        return Ok(risultato);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = Ruoli.GestoreOrOperatore)]
    public async Task<IActionResult> Elimina(string id)
    {
        bool eliminato = await _turnoService.EliminaAsync(id);
        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!eliminato)
        {
            await Log(utenteId, "Elimina Turno", false);

            return NotFound(new { messaggio = "Turno non trovato." });
        }

        await Log(utenteId, "Elimina Turno", true);

        return NoContent();
    }
    public async Task Log(string utenteId, string azione, bool risultato)
    {
        string messaggio = "Operazione fallita";
        if (risultato) messaggio = "Operazione eseguita";

        await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
        {
            IdUtente = utenteId,
            NomeAzione = azione,
            Effettuato = risultato,
            Messaggio = messaggio
        });
    }
}