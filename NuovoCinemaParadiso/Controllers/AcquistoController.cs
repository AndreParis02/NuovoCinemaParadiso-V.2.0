using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using NuovoCinemaParadiso.Services;
using NuovoCinemaParadiso.Dtos;

namespace NuovoCinemaParadiso.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AcquistoController : ControllerBase
{
    private readonly AcquistoService _acquistoService;
    private readonly LogAzioniService _logAzioniService;

    public AcquistoController(AcquistoService acquistoService, LogAzioniService logAzioniService)
    {
        _acquistoService = acquistoService;
        _logAzioniService = logAzioniService;
    }

    [HttpGet]
    public async Task<IActionResult> OttieniTutti()
    {
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null) return Unauthorized("Utente non autenticato.");

        var (risultato, errore) = await _acquistoService.OttieniTutto(utenteId);
        
        if (errore != null) {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ottieni acquisti", false);
            return BadRequest(new { messaggio = errore });
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ottieni acquisti", true);
        return Ok(risultato);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> OttieniTramiteId(string id)
    {
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null) return Unauthorized("Utente non autenticato.");

        var (risultato, errore) = await _acquistoService.OttieniTramiteIdAsync(id, utenteId);

        if (errore != null) {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ottieni acquisto ID", false);
            if (errore.Contains("non trovato")) return NotFound(new { messaggio = errore });
            return BadRequest(new { messaggio = errore });
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ottieni acquisto ID", true);
        return Ok(risultato);
    }

    [HttpPost]
    public async Task<IActionResult> Creazione([FromBody] DtoCreazioneAcquisto dto)
    {
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null) return Unauthorized("Utente non autenticato.");

        var (risultato, errore) = await _acquistoService.CreazioneAsync(dto, utenteId);

        if (errore != null) {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Creazione acquisto", false);
            if (errore.Contains("non trovat")) return NotFound(new { messaggio = errore });
            return BadRequest(new { messaggio = errore });
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Creazione acquisto", true);
        return Ok(risultato);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = Ruoli.GestoreOrOperatore)]
    public async Task<IActionResult> Modifica(string id, [FromBody] DtoCreazioneAcquisto dto)
    {
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null) return Unauthorized("Utente non autenticato.");

        var (risultato, errore) = await _acquistoService.ModificaAsync(id, dto);

        if (errore != null) {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Modifica acquisto", false);
            if (errore == "Acquisto non trovato.") return NotFound(new { messaggio = errore });
            return BadRequest(new { messaggio = errore });
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Modifica acquisto", true);
        return Ok(risultato);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = Ruoli.GestoreOrOperatore)]
    public async Task<IActionResult> Elimina(string id)
    {
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null) return Unauthorized("Utente non autenticato.");

        var (successo, errore) = await _acquistoService.EliminazioneAsync(id);

        if (!successo) {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Elimina acquisto", false);
            return NotFound(new { messaggio = errore });
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Elimina acquisto", true);
        return NoContent();
    }
}