using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using NuovoCinemaParadiso.Services;
using NuovoCinemaParadiso.Dtos;

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
    public async Task<IActionResult> Abbonati([FromBody] DtoAbbonati dto)
    {
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");

        if (string.IsNullOrEmpty(dto.IdAbbonamento))
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Abbonati", false);
            return BadRequest("Dati non validi");
        }

        var risultato = await _utenteService.AbbonatiAsync(dto.IdAbbonamento, utenteId);

        if (!risultato.Successo)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Abbonati", false);
            return BadRequest(new { errore = risultato.Messaggio });
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Abbonati", true);

        return Ok(new { messaggio = risultato.Messaggio });
    }

    [HttpPut("giftCard/ricarica")]
    public async Task<IActionResult> RicaricaGiftCard([FromBody] DtoRicaricaGiftCard dto)
    {
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");

        if (dto == null || dto.Importo <= 0)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "RicaricaGiftCard", false);
            return BadRequest("Importo non valido.");
        }

        var risultato = await _utenteService.RicaricaGiftCardAsync(utenteId, dto);

        if (!risultato.Successo)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "RicaricaGiftCard", false);
            return BadRequest(new { errore = risultato.Messaggio });
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "RicaricaGiftCard", true);

        return Ok(new { messaggio = risultato.Messaggio });
    }

    [HttpPut("giftCard/riscatta")]
    public async Task<IActionResult> RiscattaGiftCard([FromBody] DtoCodiceRiscatto dto)
    {
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");

        if (dto == null || string.IsNullOrWhiteSpace(dto.CodiceRiscatto))
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "RiscattoGiftCard", false);
            return BadRequest("Codice riscatto non valido.");
        }

        var risultato = await _utenteService.RiscattaGiftCardAsync(dto, utenteId);

        if (!risultato.Successo)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "RiscattoGiftCard", false);
            return BadRequest(new { errore = risultato.Messaggio });
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "RiscattoGiftCard", true);

        return Ok(new { messaggio = risultato.Messaggio });
    }
}