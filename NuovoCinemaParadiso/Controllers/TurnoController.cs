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
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null) return Unauthorized("Utente non autenticato.");

        List<DtoTurno> turni = await _turnoService.OttieniTuttoAsync();
        
        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ottieni tutti i turni", true);
        return Ok(turni);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> OttieniTramiteId(string id)
    {
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null) return Unauthorized("Utente non autenticato.");

        var risultato = await _turnoService.OttieniTramiteIdAsync(id);

        if (risultato == null)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ottieni turno tramite id", false);
            return NotFound(new { messaggio = $"Turno con id {id} non trovato" });
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ottieni turno tramite id", true);
        return Ok(risultato);
    }

    [HttpPost]
    [Authorize(Roles = Ruoli.Operatore)]
    public async Task<IActionResult> Creazione([FromBody] DtoCreazioneTurno dto)
    {
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null) return Unauthorized("Utente non autenticato.");

        bool creato = await _turnoService.CreazioneAsync(dto); 

        if (!creato) 
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Creazione Turno", false);
            return BadRequest(new { messaggio = "Turno già presente con questo nome." });
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Creazione Turno", true); 
        return Ok(new { messaggio = "Turno creato con successo!"}); 
    }

    [HttpPut("{id}")]
    [Authorize(Roles = Ruoli.Operatore)]
    public async Task<IActionResult> Modifica(string id, [FromBody] DtoCreazioneTurno dto)
    {
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null) return Unauthorized("Utente non autenticato.");

        bool modificato = await _turnoService.ModificaAsync(id, dto); 

        if (!modificato) 
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Modifica Turno", false);
            return BadRequest(new { messaggio = "Impossibile modificare: Turno non trovato o nome già in uso." });
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Modifica Turno", true); 
        return Ok(new { messaggio = "Turno modificato con successo!"}); 
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = Ruoli.Operatore)]
    public async Task<IActionResult> Elimina(string id)
    {
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null) return Unauthorized("Utente non autenticato.");

        var (successo, errore) = await _turnoService.EliminaAsync(id);

        if (!successo)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Elimina Turno", false);
            
            if (errore == "Turno non trovato.")
            {
                return NotFound(new { messaggio = errore });
            }
            else
            {
                return BadRequest(new { messaggio = errore });
            }
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Elimina Turno", true);
        return Ok(new { messaggio = "Turno cancellato correttamente!"}); 
    }
}