using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using NuovoCinemaParadiso.Services;
using NuovoCinemaParadiso.Dtos;
using NuovoCinemaParadiso.Exceptions;

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
        try
        {
            //ritorna true o false e un eventuale messaggio di errore
            var (result, errors) = await _authService.RegistrazioneAsync(dto);
            if (!result)
            {
                await _logAzioniService.SalvataggioLogAzioneAsync(null, "Registrazione utente", false);
                return BadRequest(errors);
            }
            await _logAzioniService.SalvataggioLogAzioneAsync(null, "Registrazione utente", true);
            return Ok(new { messaggio = "Registrazione avvenuta con successo!" });
        }
        catch (InvalidEmail ex)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(null, "Registrazione utente", false);
            return BadRequest(new { errore = ex.Message });
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] DtoLogin dto)
    {
        try
        {
            DtoAuthResponse? risposta = await _authService.LoginAsync(dto);
            if (risposta == null)
            {
                await _logAzioniService.SalvataggioLogAzioneAsync(null, "Login", false);
                return BadRequest(new { messaggio = "Credenziali non valide." });
            }
            await _logAzioniService.SalvataggioLogAzioneAsync(risposta.Id, "Login", true);
            return Ok(risposta);
        }
        catch (NotFoundException ex)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(null, "Login", false);
            return NotFound(new { messaggio = ex.Message });
        }
        catch (ConflictException ex)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(null, "Login", false);
            return BadRequest(new { messaggio = ex.Message });
        }
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
        //ritorna true se la modifica ha successo, altrimenti false
        return Ok(new { messaggio = "Utente modificato con successo." });
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
        //ritorna true se la modifica ha successo, altrimenti false
        return Ok(new { messaggio = "Utente eliminato con successo." });
    }
}