using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using NuovoCinemaParadiso.Services;
using NuovoCinemaParadiso.Dtos;

namespace NuovoCinemaParadiso.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class BigliettoController : ControllerBase
{
    private readonly BigliettoService _bigliettoService;
    private readonly LogAzioniService _logAzioniService;

    public BigliettoController(BigliettoService bigliettoService, LogAzioniService logAzioniService)
    {
        _bigliettoService = bigliettoService;
        _logAzioniService = logAzioniService;
    }

    [HttpGet]
    [Authorize (Roles = Ruoli.Gestore)]
    public async Task<IActionResult> OttieniTutti()
    {
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null) return Unauthorized("Utente non autenticato.");

        var (risultato, errore) = await _bigliettoService.OttieniTutto(utenteId);
        
        if (errore != null) {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ottieni biglietti", false);
            return BadRequest(new { messaggio = errore });
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ottieni biglietti", true);
        return Ok(risultato);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> OttieniTramiteId(string id)
    {
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null) return Unauthorized("Utente non autenticato.");

        var (risultato, errore) = await _bigliettoService.OttieniTramiteIdAsync(id, utenteId);

        if (errore != null) {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ottieni biglietto ID", false);
            if (errore.Contains("non trovato")) return NotFound(new { messaggio = errore });
            return BadRequest(new { messaggio = errore });
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ottieni biglietto ID", true);
        return Ok(risultato);
    }

    [HttpPost]
    public async Task<IActionResult> Creazione([FromBody] DtoCreazioneBiglietto dto)
    {
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null) return Unauthorized("Utente non autenticato.");

        var (risultato, errore) = await _bigliettoService.CreazioneAsync(dto, utenteId);

        if (errore != null) {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Creazione biglietto", false);
            if (errore.Contains("non trovat")) return NotFound(new { messaggio = errore });
            return BadRequest(new { messaggio = errore });
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Creazione biglietto", true);
        return Ok(risultato);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = Ruoli.Operatore)]
    public async Task<IActionResult> Modifica(string id, [FromBody] DtoCreazioneBiglietto dto)
    {
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null) return Unauthorized("Utente non autenticato.");

        var (risultato, errore) = await _bigliettoService.ModificaAsync(id, dto);

        if (errore != null) {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Modifica biglietto", false);
            if (errore == "Biglietto non trovato.") return NotFound(new { messaggio = errore });
            return BadRequest(new { messaggio = errore });
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Modifica biglietto", true);
        return Ok(risultato);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = Ruoli.Operatore)]
    public async Task<IActionResult> Elimina(string id)
    {
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null) return Unauthorized("Utente non autenticato.");

        var (successo, errore) = await _bigliettoService.EliminazioneAsync(id);

        if (!successo) {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Elimina biglietto", false);
            return NotFound(new { messaggio = errore });
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Elimina biglietto", true);
        return NoContent();
    }
}