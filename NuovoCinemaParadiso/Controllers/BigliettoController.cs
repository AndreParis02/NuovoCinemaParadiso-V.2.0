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
   
    [HttpPost]
    public async Task<IActionResult> Creazione([FromBody] DtoCreazioneBiglietto dto)
    {
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null) return Unauthorized("Utente non autenticato.");

        var (risultato, errore) = await _bigliettoService.CreazioneAsync(dto, utenteId);

        if (errore != null) {
            Console.WriteLine("Errore di creazione biglietto");
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Creazione biglietto", false);
            if (errore.Contains("non trovat")) return NotFound(new { messaggio = errore });
            return BadRequest(new { messaggio = errore });
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Creazione biglietto", true);
        return Ok(risultato);
    }

    [HttpDelete("{id}")]
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
        return Ok(new {message = "Biglietto eliminato con successo"});
    }
}