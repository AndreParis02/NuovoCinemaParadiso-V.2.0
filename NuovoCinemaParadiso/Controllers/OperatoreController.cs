using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using NuovoCinemaParadiso.Services;
using NuovoCinemaParadiso.Dtos;

namespace NuovoCinemaParadiso.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class OperatoreController : ControllerBase
{
    private readonly OperatoreService _operatoreService;
    private readonly LogAzioniService _logAzioniService;

    public OperatoreController(OperatoreService operatoreService, LogAzioniService logAzioniService)
    {
        _operatoreService = operatoreService;
        _logAzioniService = logAzioniService;
    }

    [HttpPost("ricarica")]
    [Authorize(Roles = Ruoli.Operatore)]
    public async Task<IActionResult> Ricarica([FromBody] DtoRicarica dtoRicarica)
    {
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");
        bool successo = await _operatoreService.RicaricaAsync(dtoRicarica);
        if (!successo)        {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, $"Ricarica fallita per {dtoRicarica.Email}", false);
            return NotFound($"Utente con email {dtoRicarica.Email} non trovato.");
        }
        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, $"Ricarica riuscita per {dtoRicarica.Email}", true);

        return Ok("Ricarica effettuata con successo.");
    }

    
}