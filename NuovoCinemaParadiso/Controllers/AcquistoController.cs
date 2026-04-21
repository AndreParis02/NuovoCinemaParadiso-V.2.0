using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using NuovoCinemaParadiso.Services;
using NuovoCinemaParadiso.Dtos;
using NuovoCinemaParadiso.Models;

namespace NuovoCinemaParadiso.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AcquistoController : ControllerBase
{
    private readonly AcquistoService _acquistoService;
    private readonly LogAzioniService _logAzioniService;

    public AcquistoController(AcquistoService acquistoService, LogAzioniService logAzioniService)
    {
        _acquistoService = acquistoService;
        _logAzioniService = logAzioniService;
    }

    [HttpGet]
    public async Task<IActionResult> OttieniTutti()
    {
        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        List<DtoAcquisto> acquisti = await _acquistoService.OttieniTutto(utenteId);

        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId,"Ottieni tutti gli acquisti utente",true);

        return Ok(acquisti);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> OttieniTramiteId(string id)
    {
        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var risultato = await _acquistoService.OttieniTramiteIdAsync(id, utenteId);

        if (risultato == null)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId,"Ottieni acquisti tramite id utente",false);

            return NotFound($"Acquisto con id {id} non trovato");
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId,"Ottieni acquisti tramite id utente",true);

        return Ok(risultato);
    }

    [HttpPost]
    public async Task<IActionResult> Creazione([FromBody] DtoCreazioneAcquisto dto)
    {
        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        DtoAcquisto? risultato = await _acquistoService.CreazioneAsync(dto, utenteId);

        if (risultato == null)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Creazione acquisto",false);

            return BadRequest(new { messaggio = "Acquisto già presente oppure non valido." });
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Creazione acquisto",true);

        return Ok(risultato);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = Ruoli.GestoreOrOperatore)]
    public async Task<IActionResult> Modifica(string id, [FromBody] DtoCreazioneAcquisto dto)
    {
        DtoAcquisto? risultato = await _acquistoService.ModificaAsync(id, dto);
        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (risultato == null)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId,"Modifica acquisto",false);
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId,"Modifica acquisto",true);

        return Ok(risultato);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = Ruoli.GestoreOrOperatore)]
    public async Task<IActionResult> Elimina(string id)
    {
        bool eliminato = await _acquistoService.EliminazioneAsync(id);
        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!eliminato)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId,"Elimina acquisto",false);

            return NotFound(new { messaggio = "Acquisto non trovato." });
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId,"Elimina acquisto",true);

        return NoContent();
    }
}