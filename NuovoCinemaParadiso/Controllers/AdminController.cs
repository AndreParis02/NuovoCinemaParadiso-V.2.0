using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using NuovoCinemaParadiso.Services;
using NuovoCinemaParadiso.Dtos;

namespace NuovoCinemaParadiso.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AdminController : ControllerBase
{
    private readonly AdminService _adminService;
    private readonly LogAzioniService _logAzioniService;

    public AdminController(AdminService adminService, LogAzioniService logAzioniService)
    {
        _adminService = adminService;
        _logAzioniService = logAzioniService;
    }

    [HttpGet("listaUtenti")]
    [Authorize(Roles = Ruoli.GestoreOrOperatore)]
    public async Task<IActionResult> OttieniTuttiIProfili()
    {
        List<DtoUtente> utenti = await _adminService.OttieniUtentiAsync();
        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId,"Ricerca profili",true);

        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ottieni tutti i profili",true);

        return Ok(utenti);
    }

    [HttpGet("ricercaProfilo/{id}")]
    [Authorize(Roles = Ruoli.GestoreOrOperatore)]
    public async Task<IActionResult> RicercaProfiloTramiteId(string id)
    {
        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        DtoUtente? utente = await _adminService.OttieniUtenteTramiteIdAsync(id);

        if (utente == null)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId,"Ricerca profilo",false);
            return NotFound(new { messaggio = "Utente non trovato." });
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId,"Ricerca profilo",true);
        return Ok(utente);
    }

    [HttpDelete("eliminaUtente/{id}")]
    [Authorize(Roles = Ruoli.GestoreOrOperatore)]
    public async Task<IActionResult> EliminaTramiteId(string Id)
    {
        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var risultato = await _adminService.EliminaUtentePerIdAsync(Id);

        if (risultato == null)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Eliminazione profilo",false);

            return NotFound(new { messaggio = "Utente non trovato." });
        }
         await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Eliminazione profilo",true);
        return Ok(risultato);
    }

    [HttpGet("acquisto")]
    [Authorize(Roles = Ruoli.GestoreOrOperatore)]
    public async Task<IActionResult> OttieniTuttiGliAcquisti()
    {
        List<DtoAcquisto> acquisti = await _adminService.OttieniAcquisti();
        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId,"Ottieni tutti gli acquisti admin",true);

        return Ok(acquisti);
    }

    [HttpGet("acquisto/{id}")]
    [Authorize(Roles = Ruoli.GestoreOrOperatore)]
    public async Task<IActionResult> OttieniAcquistoTramiteId(string id)
    {
        var risultato = await _adminService.OttieniAcquistoTramiteIdAsync(id);
        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (risultato == null)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId,"Ottieni acquisti tramite id admin",false);

            return NotFound($"Acquisto con id {id} non trovato");
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId,"Ottieni acquisti tramite id admin",true);

        return Ok(risultato);
    }

    [HttpGet("utenti/abbonamento/{id}")]
    [Authorize(Roles = Ruoli.GestoreOrOperatore)]
    public async Task<ActionResult<List<DtoUtente>>> OttieniUtentiTramiteAbbonamentoAsync(string abbonamentoId)
    {
        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrWhiteSpace(abbonamentoId))
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId,"Ottieni gli utenti per abbonamento",false);

            return BadRequest("AbbonamentoId non valido");
        }

        var risultato = await _adminService.OttieniUtentiTramiteAbbonamentoAsync(abbonamentoId);

        if (risultato == null || risultato.Count == 0)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId,"Ottieni gli utenti per abbonamento",false);

            return NotFound("Nessun utente trovato per questo abbonamento");
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId,"Ottieni gli utenti per abbonamento",true);

        return Ok(risultato);
    }

    [HttpGet("abbonamento/{id}")]
    [Authorize(Roles = Ruoli.GestoreOrOperatore)]
    public async Task<IActionResult> OttieniAbbonamentoTramiteIdPerAdmin(string id)
    {
        var risultato = await _adminService.OttieniAbbonamentoTramiteIdPerAdminAsync(id);
        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (risultato == null)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId,"Ottieni abbonamenti tramite id admin",false);

            return NotFound($"Abbonamento con id {id} non trovato");
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId,"Ottieni abbonamenti tramite id admin",true);

        return Ok(risultato);
    }
    
    [HttpGet("giftcard/{id}")]
    [Authorize(Roles = Ruoli.GestoreOrOperatore)]
    public async Task<IActionResult> OttieniGiftCardTramiteIdPerAdmin(string id)
    {
        var risultato = await _adminService.OttieniGiftCardTramiteIdPerAdminAsync(id);
        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (risultato == null)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ottieni giftcard tramite id admin",false);
            return NotFound($"Giftcard con id {id} non trovato");
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ottieni giftcard tramite id admin",true);
        return Ok(risultato);
    }

    [HttpGet("utenti/giftCard/{giftcardId}")]
    [HttpGet("utenti/giftCard/{Id}")]
    [Authorize(Roles = Ruoli.GestoreOrOperatore)]
    public async Task<ActionResult<List<DtoUtente>>> OttieniUtentiTramiteGiftCardPerAdmin(string giftcardId)
    {
        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrWhiteSpace(giftcardId))
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId,"Ottieni gli utenti per giftcard",false);

            return BadRequest("giftcardId non valido");
        }

        var risultato = await _adminService.OttieniUtentiTramiteGiftCardAsync(giftcardId);

        if (risultato == null)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId,"Ottieni gli utenti per giftcard",false);

            return NotFound("Nessun utente trovato per questa giftcard");
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId,"Ottieni gli utenti per giftcard",true);

        return Ok(risultato);
    }

    [HttpGet("log")]
    [Authorize(Roles = Ruoli.Gestore)]
    public async Task<IActionResult> OttieniLogAzioni()
    {
        List<DtoLogAzioni> risultatiLog = await _logAzioniService.LetturaLogAzioneAsync();
        return Ok(risultatiLog);
    }   
}