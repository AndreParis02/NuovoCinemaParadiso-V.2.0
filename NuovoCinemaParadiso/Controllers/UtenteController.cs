using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using NuovoCinemaParadiso.Services;
using NuovoCinemaParadiso.Dtos;
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
        try
        {
            var risultato = await _utenteService.AbbonatiAsync(dto.IdAbbonamento, utenteId);
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Abbonati", true);
            return Ok(risultato);
        }
        catch (NotFoundException ex)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(dto.IdAbbonamento, "Abbonati", false);
            return NotFound(new { errore = ex.Message });
        }
        catch (ItemAlredyexist ex)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(dto.IdAbbonamento, "Abbonati", false);
            return NotFound(new { errore = ex.Message });
        }
        catch (Exception ex)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Abbonati", false);
            return BadRequest(new { errore = ex.Message });
        }
    }

    [HttpPost("giftCard/ricarica")]
    public async Task<IActionResult> RicaricaGiftCard([FromBody] DtoRicaricaGiftCard dto)
    {
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");

        // Validazione importo
        if (dto == null || dto.Importo <= 0)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "RicaricaGiftCard", false);
            return BadRequest(new { errore = "L'importo della ricarica deve essere maggiore di zero." });
        }

        try
        {
            var risultato = await _utenteService.RicaricaGiftCardAsync(utenteId, dto);

            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "RicaricaGiftCard", true);

            return Ok(risultato);
        }
        catch (Exception ex)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "RicaricaGiftCard", false);
            return BadRequest(new { errore = ex.Message });
        }
    }

    [HttpPut("giftCard/riscatta")]
    public async Task<IActionResult> RiscattaGiftCard([FromBody] DtoCodiceRiscatto dto)
    {
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (utenteId == null)
        {
            return Unauthorized("Utente non autenticato.");
        }

        if (dto == null)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "RiscattoGiftCard", false);

            return BadRequest("Body richiesta non valido.");
        }

        if (string.IsNullOrWhiteSpace(dto.CodiceRiscatto))
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "RiscattoGiftCard", false);

            return BadRequest("Codice riscatto non valido.");
        }

        try
        {
            DtoGiftCard risultato =
                await _utenteService.RiscattaGiftCardAsync(dto, utenteId);

            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "RiscattoGiftCard", true);

            return Ok(risultato);
        }
        catch (NotFoundException ex)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "RiscattoGiftCard", false);

            return NotFound(new
            {
                errore = ex.Message
            });
        }
        catch (Exception ex)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "RiscattoGiftCard", false);

            return BadRequest(new
            {
                errore = ex.Message
            });
        }
    }
}