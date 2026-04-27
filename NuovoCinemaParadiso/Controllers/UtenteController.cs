using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using NuovoCinemaParadiso.Services;
using NuovoCinemaParadiso.Dtos;
using NuovoCinemaParadiso.Models;
using NuovoCinemaParadiso.Exceptions;

namespace NuovoCinemaParadiso.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UtenteController : ControllerBase
{
    private readonly UtenteService _utenteService;
    private readonly LogAzioniService _logAzioniService;

    public UtenteController(UtenteService utenteService, LogAzioniService logAzioniService)
    {
        _utenteService = utenteService;
        _logAzioniService = logAzioniService;
    }

    [HttpPost("abbonati")]
    public async Task<IActionResult> Abbonati([FromBody] DtoUtente dto)
    {
        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (dto == null || string.IsNullOrEmpty(dto.AbbonamentoId))
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Abbonati", false);
            return BadRequest("Dati non validi");
        }

        var risultato = await _utenteService.AbbonatiAsync(dto.AbbonamentoId, utenteId);

        if (risultato == null)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Abbonati", false);
            return NotFound("Utente o abbonamento non trovato");
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Abbonati", true);
        return Ok(risultato);
    }

    [HttpPost("giftCard")]
    public async Task<IActionResult> GiftCard([FromBody] DtoUtente dto)
    {
        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (dto == null || string.IsNullOrEmpty(dto.GiftCardId))
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "GiftCard", false);
            return BadRequest("Dati non validi");
        }
        try
        {
            var risultato = await _utenteService.GiftCardAsync(dto.GiftCardId, utenteId);
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "GiftCard", true);
            return Ok(risultato);
        }
        catch (NotFoundException ex)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "GiftCard", false);
            return NotFound(new { errore = ex.Message });
        }
        catch (GiftCardAlredyexis ex)
        {
        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "GiftCard", false);
            return NotFound(new { errore = ex.Message });
        }
    }
}
