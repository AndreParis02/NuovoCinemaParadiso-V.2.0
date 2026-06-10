using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using NuovoCinemaParadiso.Services;
using NuovoCinemaParadiso.Dtos;
using NuovoCinemaParadiso.Models;

namespace NuovoCinemaParadiso.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AbbonamentoController : ControllerBase
{
    private readonly AbbonamentoService _abbonamentoService;
    private readonly LogAzioniService _logAzioniService;
    private readonly GestoreService _gestoreService;

    public AbbonamentoController(
        AbbonamentoService abbonamentoService,
        LogAzioniService logAzioniService,
        GestoreService gestoreService)
    {
        _abbonamentoService = abbonamentoService;
        _logAzioniService = logAzioniService;
        _gestoreService = gestoreService;
    }

    [HttpGet]
    public async Task<IActionResult> OttieniTuttiGliAbbonamenti()
    {
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        List<DtoAbbonamento> abbonamenti = await _abbonamentoService.OttieniTutto();

        await _logAzioniService.SalvataggioLogAzioneAsync(
            utenteId,
            "Ottieni tutti gli abbonamenti utente",
            true
        );

        return Ok(abbonamenti);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> OttieniTramiteId(string id)
    {
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");

        var risultato = await _abbonamentoService.OttieniTramiteIdAsync(id);

        if (risultato == null)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(
                utenteId,
                "Ottieni abbonamenti tramite id utente",
                false
            );

            return NotFound($"Abbonamento con id {id} non trovato");
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(
            utenteId,
            "Ottieni abbonamenti tramite id utente",
            true
        );

        return Ok(risultato);
    }

    [HttpPost]
    [Authorize(Roles = Ruoli.Operatore)]
    public async Task<IActionResult> Creazione([FromBody] DtoCreazioneAbbonamento dto)
    {
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");

        var risultato = await _abbonamentoService.CreazioneAsync(dto);

        if (!risultato.Successo)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(
                utenteId,
                "Creazione abbonamento",
                false
            );

            return BadRequest(new { messaggio = risultato.Messaggio });
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(
            utenteId,
            "Creazione abbonamento",
            true
        );

        return Ok(new { messaggio = risultato.Messaggio });
    }

    [HttpPut("{id}")]
    [Authorize(Roles = Ruoli.Operatore)]
    public async Task<IActionResult> Modifica(string id, [FromBody] DtoCreazioneAbbonamento dto)
    {
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");

        var risultato = await _abbonamentoService.ModificaAsync(id, dto);

        if (!risultato.Successo)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(
                utenteId,
                "Modifica abbonamento",
                false
            );

            return NotFound(new { messaggio = risultato.Messaggio });
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(
            utenteId,
            "Modifica abbonamento",
            true);

        return Ok(new { messaggio = risultato.Messaggio });
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = Ruoli.Operatore)]
    public async Task<IActionResult> Elimina(string id)
    {
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");

        var risultato = await _abbonamentoService.EliminazioneAsync(id);

        if (!risultato.Successo)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(
                utenteId,
                "Elimina abbonamento",
                false
            );

            return NotFound(new { messaggio = risultato.Messaggio });
        }
            await _logAzioniService.SalvataggioLogAzioneAsync(
            utenteId,
            "Elimina abbonamento",
            true
        );

        return NoContent();
    }
}
