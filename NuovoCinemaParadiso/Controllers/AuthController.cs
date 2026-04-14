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

        if (!result.Succeeded)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
            {
                NomeAzione = "Registrazione utente",
                Effettuato = false,
                Messaggio = "Registrazione fallita"
            });
            return BadRequest(result.Errors);
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
        {
            NomeAzione = "Registrazione utente",
            Effettuato = true,
            Messaggio = "Registrazione avvenuta"
        });

        return Ok(new { messaggio = "Registrazione avvenuta con successo!" });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] DtoLogin dto)
    {
        DtoAuthResponse? risposta = await _authService.LoginAsync(dto);
        if (risposta == null)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
            {
                NomeAzione = "Login",
                Effettuato = false,
                Messaggio = "Login fallito"
            });
            return Unauthorized(new { messaggio = "Email o password non validi." });
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
        {
            IdUtente = risposta.Id,
            NomeAzione = "Login",
            Effettuato = true,
            Messaggio = "Login avvenuto"
        });
        return Ok(risposta);
    }

    [HttpGet("profilo")]
    public async Task<IActionResult> RicercaProfiloLoggato()
    {
        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        DtoUtente? utente = await _authService.OttieniTramiteIdAsync(utenteId);

        if (utente == null)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
            {
                IdUtente = utenteId,
                NomeAzione = "Ricerca profilo loggato",
                Effettuato = false,
                Messaggio = "Operazione fallita"
            });

            return NotFound(new { messaggio = "Utente non trovato." });
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
            {   
                IdUtente   = utente.Id,  
                NomeAzione = "Ricerca profilo loggato",
                Effettuato = true,
                Messaggio  = "Ricerca avvenuta"
            });

        return Ok(utente);
    }

    [HttpPut("modifica")]
    public async Task<IActionResult> Modifica([FromBody] DtoCreazioneUtente dto)
    {
        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var risultato = await _authService.ModificaAsync(dto, utenteId);

        if (risultato == null)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
            {
                IdUtente = utenteId,
                NomeAzione = "Modifica utente",
                Effettuato = false,
                Messaggio = "Operazione fallita"
            });

            return NotFound(new { messaggio = "Utente non trovato." });
        }
        await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
            {   
                IdUtente   = utenteId,
                NomeAzione = "Modifica profilo",
                Effettuato = true,
                Messaggio  = "Modifica profilo avvenuta"
            });

        await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
        {
            IdUtente = utenteId,
            NomeAzione = "Modifica utente",
            Effettuato = true,
            Messaggio = "Operazione eseguita"
        });

        return Ok(risultato);
    }

    [HttpDelete("elimina")]
    public async Task<IActionResult> Elimina()
    {
        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var risultato = await _authService.EliminaAsync(utenteId);

        if (risultato == null)
        {   
            return NotFound(new { messaggio = "Utente non trovato." });
        }
        
        await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
            {   
                IdUtente   = utenteId,
                NomeAzione = "Eliminazione profilo",
                Effettuato = true,
                Messaggio  = "Eliminazione profilo avvenuta"
            });
        return Ok(risultato);
    }
}