using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using NuovoCinemaParadiso.Services;
using NuovoCinemaParadiso.Dtos;
using Microsoft.AspNetCore.Authorization;

namespace NuovoCinemaParadiso.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;
    private readonly LogAzioniService _logAzioniService;

    public AuthController(AuthService authService, LogAzioniService logAzioniService)
    {
        _authService = authService;
        _logAzioniService = logAzioniService;
    }

    [HttpPost("registrazione")]
    public async Task<IActionResult> Registrazione(DtoRegistrazione dto)
    {
        IdentityResult result = await _authService.RegistrazioneAsync(dto);
        string? utenteId = null;

        if (!result.Succeeded)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Registrazione utente", false);
            return BadRequest(result.Errors);
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Registrazione utente", true);
        return Ok(new { messaggio = "Registrazione avvenuta con successo!" });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] DtoLogin dto)
    {
        DtoAuthResponse? risposta = await _authService.LoginAsync(dto);

        if (risposta == null)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(null, "Login", false);
            return Unauthorized(new { messaggio = "Email o password non validi." });
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(risposta.Id, "Login", true);
        return Ok(risposta);
    }

    [HttpGet("profilo")]
    public async Task<IActionResult> RicercaProfiloLoggato()
    {
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");

        DtoUtente? utente = await _authService.OttieniTramiteIdAsync(utenteId);

        if (utente == null)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ricerca profilo loggato", false);
            return NotFound(new { messaggio = "Utente non trovato." });
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ricerca profilo loggato", true);
        return Ok(utente);
    }

    [HttpPut("modifica")]
    public async Task<IActionResult> Modifica([FromBody] DtoCreazioneUtente dto)
    {
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");

        var risultato = await _authService.ModificaAsync(dto, utenteId);

        if (risultato == null)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Modifica utente", false);
            return NotFound(new { messaggio = "Utente non trovato." });
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Modifica utente", true);
        return Ok(risultato);
    }

    [HttpDelete("elimina")]
    public async Task<IActionResult> Elimina()
    {
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");

        var risultato = await _authService.EliminaAsync(utenteId);

        if (risultato == null)
        {
            return NotFound(new { messaggio = "Utente non trovato." });
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Eliminazione profilo", true);
        return Ok(risultato);
    }
}