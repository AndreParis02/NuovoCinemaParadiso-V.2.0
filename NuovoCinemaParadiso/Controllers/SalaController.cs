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
public class SalaController : ControllerBase
{
    private readonly SalaService _salaService;
    private readonly LogAzioniService _logAzioniService;

    public SalaController(SalaService salaService, LogAzioniService logAzioniService)
    {
        _salaService = salaService;
        _logAzioniService = logAzioniService;
    }

    [HttpGet]
    public async Task<IActionResult> OttieniTutti()
    {
        
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");
        List<DtoSala> sale = await _salaService.OttieniTuttoAsync();
        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ottieni tutte le sale", true);

        return Ok(sale);
    }

    [HttpGet("tipologia/{tipologiaId}")]
    public async Task<ActionResult<List<DtoSala>>> OttieniPerTipologia(string tipologiaId)
    {
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");

        if (string.IsNullOrWhiteSpace(tipologiaId))
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ottieni sala per tipologia", false);

            return BadRequest("TipologiaId non valida");
        }

        var risultato = await _salaService.OttieniTramiteTipologiaAsync(tipologiaId);

        if (risultato == null || risultato.Count == 0)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ottieni sala per tipologia", false);

            return NotFound("Nessuna sala trovata per questa tipologia");
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ottieni sala per tipologia", true);

        return Ok(risultato);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> OttieniTramiteId(string id)
    {
        
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");
        var risultato = await _salaService.OttieniTramiteIdAsync(id);
        if (risultato == null)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ottieni sala tramite id", false);

            return NotFound($"Sala con id {id} non trovato");
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ottieni sala tramite id", true);

        return Ok(risultato);
    }

    [HttpPost]
    [Authorize(Roles = Ruoli.Operatore)]
    public async Task<IActionResult> Creazione([FromBody] DtoCreazioneSala dto)
    {
        
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");
        DtoSala? risultato = await _salaService.CreazioneAsync(dto);
        if (risultato == null)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Creazione sala", false);

            return BadRequest(new { messaggio = "Sala non valida." });
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Creazione sala", true);

        return Ok(risultato);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = Ruoli.Operatore)]
    public async Task<IActionResult> Modifica(string id, [FromBody] DtoCreazioneSala dto)
    {

        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");

        try
        {
            DtoSala? risultato = await _salaService.ModificaAsync(id, dto);
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Modifica sala", true);

            return Ok(risultato);
        }

        catch (ModificaException ex)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Modifica sala", false);
            return BadRequest(new { message = ex.Message });
        }

        catch (ItemNotFoundException ex)
        {

            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Modifica sala", false);

            return NotFound(new { message = ex.Message });

        }

        catch (NotFoundException ex)
        {

            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Modifica sala", false);

            return NotFound(new { message = ex.Message });

        }

    }

    [HttpDelete("{id}")]
    [Authorize(Roles = Ruoli.Operatore)]
    public async Task<IActionResult> Elimina(string id)
    {
        
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");
        bool eliminato = await _salaService.EliminaAsync(id);
        if (!eliminato)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Elimina sala", false);

            return NotFound(new { messaggio = "Sala non trovata." });
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Elimina sala", true);

        return Ok(new { messaggio = "Sala eliminata con successo!" });
    }
}