using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using NuovoCinemaParadiso.Services;
using NuovoCinemaParadiso.Dtos;
using NuovoCinemaParadiso.Models;

namespace NuovoCinemaParadiso.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TipologiaSalaController : ControllerBase
{
    private readonly TipologiaSalaService _tipologiaSalaService;
    private readonly LogAzioniService _logAzioniService;

    public TipologiaSalaController(TipologiaSalaService tipologiaSalaService, LogAzioniService logAzioniService)
    {
        _tipologiaSalaService = tipologiaSalaService;
        _logAzioniService = logAzioniService;
    }

    [HttpGet]
    public async Task<IActionResult> OttieniTutti()
    {
        List<DtoTipologiaSala> tipologieSala = await _tipologiaSalaService.OttieniTuttoAsync();
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");

        List<DtoTipologiaSala> tipologieSaleTrovate = tipologieSala
            .Where(temp => !temp.IsDeleted)
            .ToList();

        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ottieni tutte le tipologie", true);


        return Ok(tipologieSaleTrovate);
    }

    [HttpGet("storico")]
    public async Task<IActionResult> OttieniTuttiStorico()
    {
        List<DtoTipologiaSala> tipologieSala = await _tipologiaSalaService.OttieniTuttoAsync();
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");

        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ottieni tutte le tipologie", true);

        return Ok(tipologieSala);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> OttieniTramiteId(string id)
    {
        var risultato = await _tipologiaSalaService.OttieniTramiteIdAsync(id);
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");

        if (risultato == null)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ottieni tipologie tramite id", false);

            return NotFound($"TipologiaSala con id {id} non trovato");
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ottieni tipologie tramite id", true);

        return Ok(risultato);
    }

    [HttpPost]
    [Authorize(Roles = Ruoli.Operatore)]
    public async Task<IActionResult> Creazione([FromBody] DtoCreazioneTipologiaSala dto)
    {
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");

        List<DtoTipologiaSala> tipologieSala = await _tipologiaSalaService.OttieniTuttoAsync();
        // Verifica se esiste già una tipologia con lo stesso nome (case-insensitive)
        if (tipologieSala.Any(ts => ts.Nome.Equals(dto.Nome, StringComparison.OrdinalIgnoreCase)))
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Creazione tipologia", false);
            return BadRequest(new { messaggio = "Tipologia sala già presente." });
        }

        bool risultato = await _tipologiaSalaService.CreazioneAsync(dto);

        if (!risultato)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Creazione tipologia", false);
            return BadRequest(new { messaggio = "Tipologia sala non valida." });
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Creazione tipologia", true);
        return Ok(new { messaggio = "Tipologia sala aggiunta con successo!" });
    }

    [HttpPut("{id}")]
    [Authorize(Roles = Ruoli.Operatore)]
    public async Task<IActionResult> Modifica(string id, [FromBody] DtoCreazioneTipologiaSala dto)
    {
        bool risultato = await _tipologiaSalaService.ModificaAsync(id, dto);
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");

        if (!risultato)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Modifica tipologia", false);

            return NotFound(new { messaggio = "Tipologia sala non trovata." });
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Modifica tipologia", true);

        return Ok(new { messaggio = "Tipologia sala modificata con successo!" });
    }

    [HttpPut("elimina/{id}")]
    [Authorize(Roles = Ruoli.Operatore)]
    public async Task<IActionResult> Elimina(string id)
    {
        bool eliminato = await _tipologiaSalaService.EliminaAsync(id);
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");

        if (!eliminato)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Elimina tipologia", false);

            return NotFound(new { messaggio = "Tipologia sala non trovata." });
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Elimina tipologia", true);

        return Ok(new { messaggio = "Tipologia sala eliminata con successo!" });
    }
}
