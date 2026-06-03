using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using NuovoCinemaParadiso.Services;
using NuovoCinemaParadiso.Dtos;

namespace NuovoCinemaParadiso.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class OperatoreController : ControllerBase
{
    private readonly OperatoreService _operatoreService;
    private readonly LogAzioniService _logAzioniService;

    public OperatoreController(OperatoreService operatoreService, LogAzioniService logAzioniService)
    {
        _operatoreService = operatoreService;
        _logAzioniService = logAzioniService;
    }

    [HttpPost("ricarica")]
    [Authorize(Roles = Ruoli.Operatore)]
    public async Task<IActionResult> RicaricaSaldoUtente([FromBody] DtoRicaricaSaldoUtente dtoRicaricaSaldoUtente)
    {
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");
        bool successo = await _operatoreService.RicaricaAsync(dtoRicaricaSaldoUtente);
        if (!successo)        {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, $"Ricarica fallita per {dtoRicaricaSaldoUtente.Email}", false);
            return NotFound($"Utente con email {dtoRicaricaSaldoUtente.Email} non trovato.");
        }
        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, $"Ricarica riuscita per {dtoRicaricaSaldoUtente.Email}", true);

        return Ok("Ricarica effettuata con successo.");
    }

    [HttpGet("listaUtenti")]
    [Authorize(Roles = Ruoli.Operatore)]
    public async Task<IActionResult> OttieniTuttiIProfili()
    {
        List<DtoUtente> utenti = await _operatoreService.OttieniUtentiAsync();

        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");

        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ricerca profili", true);

        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ottieni tutti i profili", true);

        return Ok(utenti);
    }

    [HttpGet("ricercaProfilo/{id}")]
    [Authorize(Roles = Ruoli.Operatore)]
    public async Task<IActionResult> RicercaProfiloTramiteId(string id)
    {
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");
        try
        {
            DtoUtente? utente = await _operatoreService.OttieniUtenteTramiteIdAsync(id);
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
    [Authorize(Roles = Ruoli.Operatore)]
    public async Task<IActionResult> EliminaTramiteId(string Id)
    {
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");
        try
        {
            await _operatoreService.EliminaUtentePerIdAsync(Id);
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Eliminazione profilo", true);
            return Ok();
        }
        catch
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Eliminazione profilo", false);
            return NotFound(new { messaggio = "Utente non trovato." });
        }
    }

    [HttpGet("biglietto")]
    [Authorize(Roles = Ruoli.Operatore)]
    public async Task<IActionResult> OttieniTuttiGliBiglietti()
    {
        List<DtoBiglietto> biglietti = await _operatoreService.OttieniBiglietti();

        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");

        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ottieni tutti gli biglietti gestore", true);
        return Ok(biglietti);
    }

    [HttpGet("biglietto/{id}")]
    [Authorize(Roles = Ruoli.Operatore)]
    public async Task<IActionResult> OttieniBigliettoTramiteId(string id)
    {

        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");
        try
        {
            var risultato = await _operatoreService.OttieniBigliettoTramiteIdAsync(id);
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
    [Authorize(Roles = Ruoli.Operatore)]
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

        var risultato = await _operatoreService.OttieniUtentiTramiteAbbonamentoAsync(abbonamentoId);

        if (risultato == null || risultato.Count == 0)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ottieni gli utenti per abbonamento", false);

            return NotFound("Nessun utente trovato per questo abbonamento");
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ottieni gli utenti per abbonamento", true);

        return Ok(risultato);
    }

    [HttpPost("giftCard/ricarica")]
    [Authorize(Roles = Ruoli.Operatore)]
    public async Task<IActionResult> RicaricaGiftCard([FromBody] DtoRicaricaGiftCard dto)
    {
        string? operatoreId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        if (operatoreId == null)
        {
            return Unauthorized("Utente non autenticato.");
        }

        if (dto == null || dto.Importo <= 0)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(operatoreId, "RicaricaGiftCard", false);
            return BadRequest(new { errore = "L'importo della Gift Card deve essere maggiore di zero." });
        }

        try
        {
            await _operatoreService.RicaricaGiftCardAsync(dto);

            await _logAzioniService.SalvataggioLogAzioneAsync(operatoreId, "RicaricaGiftCard", true);
            
            return Ok();
        }
        catch (Exception ex)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(operatoreId, "RicaricaGiftCard", false);
            return BadRequest(new { errore = ex.Message });
        }
    }
}