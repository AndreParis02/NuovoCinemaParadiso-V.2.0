using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NuovoCinemaParadiso.Services;
using NuovoCinemaParadiso.Dtos;
using System.Security.Claims;

namespace NuovoCinemaParadiso.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class GestoreController : ControllerBase
{
    private readonly GestoreService _gestoreService;
    private readonly LogAzioniService _logAzioniService;

    public GestoreController(GestoreService gestoreService, LogAzioniService logAzioniService)
    {
        _gestoreService   = gestoreService;
        _logAzioniService = logAzioniService;
    }
    
    [HttpGet("conto")]
    public async Task<IActionResult> OttieniDatiConto()
    {
        DtoContoCinema contoCinema = await _gestoreService.OttieniDatiContoAsync();
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");


        if(contoCinema == null)
        {
             await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ottieni dati Conto", false);

            return BadRequest(new { messaggio = "Non è presente nessun conto." });
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ottieni dati Conto", true);


        return Ok(contoCinema);
    }

    [HttpGet("logs")]
    [Authorize(Roles = Ruoli.Gestore)]
    public async Task<IActionResult> OttieniLogAzioni()
    {
        List<DtoLogAzioni> risultatiLog = await _gestoreService.LetturaLogAzioneAsync();
        return Ok(risultatiLog);
    }
}