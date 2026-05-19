using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using NuovoCinemaParadiso.Dtos;
using NuovoCinemaParadiso.Services;
using NuovoCinemaParadiso.Exceptions;

namespace NuovoCinemaParadiso.Controllers;

[ApiController]
[Route("api/[controller]")]
  [Authorize (Roles = Ruoli.Operatore)]
public class ContoCinemaController : ControllerBase
{
    private readonly ContoCinemaService _contoCinemaService;
    private readonly LogAzioniService _logAzioniService;

    public ContoCinemaController(ContoCinemaService contoCinemaService, LogAzioniService logAzioniService)
    {
        _contoCinemaService = contoCinemaService;
        _logAzioniService = logAzioniService;
    }

    [HttpGet]
    public async Task<IActionResult> OttieniDatiConto()
    {
        DtoContoCinema contoCinema = await _contoCinemaService.OttieniDatiContoAsync();
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

    //
    // PORZIONE DI CODICE ELIMINATA. FARE RIFERIMENTA AL README
    //
}