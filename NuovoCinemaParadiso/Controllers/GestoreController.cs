using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using NuovoCinemaParadiso.Services;
using NuovoCinemaParadiso.Dtos;

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
        _gestoreService = gestoreService;
        _logAzioniService = logAzioniService;
    }

    [HttpGet("listaUtenti")]
    [Authorize(Roles = Ruoli.GestoreOrOperatore)]
    public async Task<IActionResult> OttieniTuttiIProfili()
    {
        List<DtoUtente> utenti = await _gestoreService.OttieniUtentiAsync();

        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");

        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ricerca profili", true);

        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ottieni tutti i profili", true);

        return Ok(utenti);
    }

    [HttpGet("ricercaProfilo/{id}")]
    [Authorize(Roles = Ruoli.GestoreOrOperatore)]
    public async Task<IActionResult> RicercaProfiloTramiteId(string id)
    {
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");
        try
        {
            DtoUtente? utente = await _gestoreService.OttieniUtenteTramiteIdAsync(id);
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ricerca profilo", true);
            return Ok(utente);
        }
        catch
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ricerca profilo", false);
            return NotFound(new { messaggio = "Utente non trovato." });
        }
    }

    [HttpDelete("eliminaUtente/{id}")]
    [Authorize(Roles = Ruoli.GestoreOrOperatore)]
    public async Task<IActionResult> EliminaTramiteId(string Id)
    {
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");
        try
        {
            var risultato = await _gestoreService.EliminaUtentePerIdAsync(Id);
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Eliminazione profilo", true);
            return Ok(risultato);
        }
        catch
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Eliminazione profilo", false);
            return NotFound(new { messaggio = "Utente non trovato." });
        }
    }

    [HttpGet("biglietto")]
    [Authorize(Roles = Ruoli.GestoreOrOperatore)]
    public async Task<IActionResult> OttieniTuttiGliBiglietti()
    {
        List<DtoBiglietto> biglietti = await _gestoreService.OttieniBiglietti();

        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");

        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ottieni tutti gli biglietti gestore", true);
        return Ok(biglietti);
    }

    [HttpGet("biglietto/{id}")]
    [Authorize(Roles = Ruoli.GestoreOrOperatore)]
    public async Task<IActionResult> OttieniBigliettoTramiteId(string id)
    {

        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");
        try
        {
            var risultato = await _gestoreService.OttieniBigliettoTramiteIdAsync(id);
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ottieni biglietti tramite id gestore", true);
            return Ok(risultato);
        }
        catch
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ottieni biglietti tramite id gestore", false);
            return NotFound($"Biglietto con id {id} non trovato");
        }


    }

    [HttpGet("utenti/abbonamento/{abbonamentoId}")]
    [Authorize(Roles = Ruoli.GestoreOrOperatore)]
    public async Task<ActionResult<List<DtoUtente>>> OttieniUtentiTramiteAbbonamentoAsync(string abbonamentoId)
    {
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");

        if (string.IsNullOrWhiteSpace(abbonamentoId))
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ottieni gli utenti per abbonamento", false);

            return BadRequest("AbbonamentoId non valido");
        }

        var risultato = await _gestoreService.OttieniUtentiTramiteAbbonamentoAsync(abbonamentoId);

        if (risultato == null || risultato.Count == 0)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ottieni gli utenti per abbonamento", false);

            return NotFound("Nessun utente trovato per questo abbonamento");
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ottieni gli utenti per abbonamento", true);

        return Ok(risultato);
    }

    [HttpGet("utenti/giftCard/{giftcardId}")]
    [Authorize(Roles = Ruoli.GestoreOrOperatore)]
    public async Task<ActionResult<List<DtoUtente>>> OttieniUtentiTramiteGiftCardPerGestore(string giftcardId)
    {
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");

        if (string.IsNullOrWhiteSpace(giftcardId))
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ottieni gli utenti per giftcard", false);

            return BadRequest("giftcardId non valido");
        }
        try
        {
            var risultato = await _gestoreService.OttieniUtentiTramiteGiftCardAsync(giftcardId);
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ottieni gli utenti per giftcard", true);
            return Ok(risultato);
        }
        catch
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ottieni gli utenti per giftcard", false);
            return NotFound("Nessun utente trovato per questa giftcard");
        }
    }

    [HttpGet("log")]
    [Authorize(Roles = Ruoli.Gestore)]
    public async Task<IActionResult> OttieniLogAzioni()
    {
        List<DtoLogAzioni> risultatiLog = await _logAzioniService.LetturaLogAzioneAsync();
        return Ok(risultatiLog);
    }
}