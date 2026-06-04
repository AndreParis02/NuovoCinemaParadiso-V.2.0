using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using NuovoCinemaParadiso.Dtos;
using NuovoCinemaParadiso.Services;

namespace NuovoCinemaParadiso.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = Ruoli.Gestore)]
public class GestoreUtentiController : ControllerBase
{
    private readonly RuoloUtenteService _ruoloUtenteService;
    private readonly LogAzioniService _logAzioniService;

    public GestoreUtentiController(RuoloUtenteService ruoloUtenteService, LogAzioniService logAzioniService)
    {
        _ruoloUtenteService = ruoloUtenteService;
        _logAzioniService = logAzioniService;
    }

    [HttpPut("cambia-ruolo")]
    public async Task<IActionResult> CambiaRuolo([FromBody] DtoModificaRuoloUtente dto)
    {
        string? nuovoRuolo = await _ruoloUtenteService.ModificaRuoloUtente(dto);
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");

        if (nuovoRuolo == null)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId,"Cambio ruolo",false);

            return BadRequest(new { messaggio = "Utente o ruolo non valido." });
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId,"Cambio ruolo",true);
        
        return Ok(new
        {
            messaggio = "Ruolo aggiornato correttamente.",
            email = dto.Email,
            ruolo = nuovoRuolo
        });
    }
}