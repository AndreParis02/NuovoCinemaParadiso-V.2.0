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

        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ottieni dati Conto", true);

        if(contoCinema == null)
        {
             await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ottieni dati Conto", false);

            return BadRequest(new { messaggio = "Non è presente nessun conto." });
        }

        return Ok(contoCinema);
    }

    [HttpPost]
    public async Task<IActionResult> Creazione([FromBody] DtoCreazioneContoCinema dto)
    {
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");

        DtoContoCinema contoCinema = await _contoCinemaService.OttieniDatiContoAsync();

        if(contoCinema != null)
        {
             await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Creazione dati conto", false);

            return BadRequest(new { messaggio = "Conto già presente." });
        }

        DtoContoCinema? risultato = await _contoCinemaService.CreazioneAsync(dto);

        if (risultato == null)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Creazione dati conto", false);

            return BadRequest(new { messaggio = "Dati conto non validi." });
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Creazione dati conto", true);

        return Ok(risultato);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Modifica(string id, [FromBody] DtoCreazioneContoCinema dto)
    {
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");

        try
        {
            DtoContoCinema? risultato = await _contoCinemaService.ModificaAsync(id, dto);
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Modifica dati conto", true);

            return Ok(risultato);

        }
        catch (ModificaException ex)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Modifica dati conto", false);
            return BadRequest(new { message = ex.Message });
        }

        catch (ItemNotFoundException ex)
        {

            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Modifica dati conto", false);

            return NotFound(new { message = ex.Message });

        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Elimina(string id)
    {
        bool eliminato = await _contoCinemaService.EliminaAsync(id);
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");

        if (!eliminato)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Eliminazione dati conto", false);

            return NotFound(new { messaggio = "Dati conto non trovati." });
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Eliminazione dati conto", true);

        return Ok(new { message = "I dati del conto sono stati eliminati correttamente" });
    }
}