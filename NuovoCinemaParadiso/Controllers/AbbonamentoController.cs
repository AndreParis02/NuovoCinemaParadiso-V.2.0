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
public class AbbonamentoController : ControllerBase
{
    private readonly AbbonamentoService _abbonamentoService;
    private readonly AdminService _adminService;

    private readonly LogAzioniService _logAzioniService;

    public AbbonamentoController(AbbonamentoService abbonamentoService, LogAzioniService logAzioniService, AdminService adminService)
    {
        _abbonamentoService = abbonamentoService;
        _logAzioniService = logAzioniService;
        _adminService = adminService;

    }

    [HttpGet]
    public async Task<IActionResult> OttieniTuttiGliAbbonamenti()
    {
        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        List<DtoAbbonamento> abbonamenti = await _abbonamentoService.OttieniTutto();

        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId,"Ottieni tutti gli abbonamenti utente" ,true);
       

        return Ok(abbonamenti);
    }

    [HttpGet("admin/{id}")]
    [Authorize(Roles = Ruoli.GestoreOrOperatore)]
    public async Task<IActionResult> OttieniTramiteIdPerAdmin(string id)
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

    [HttpGet("{id}")]
    public async Task<IActionResult> OttieniTramiteIdPerUtente(string id)
    {
        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var risultato = await _abbonamentoService.OttieniTramiteIdAsync(id, utenteId);

        if (risultato == null)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId,"Ottieni abbonamenti tramite id utente",false);

            return NotFound($"Abbonamento con id {id} non trovato");
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId,"Ottieni abbonamenti tramite id utente",true);

        return Ok(risultato);
    }

    [HttpPost]
    [Authorize(Roles = Ruoli.GestoreOrOperatore)]
    public async Task<IActionResult> Creazione([FromBody] DtoCreazioneAbbonamento dto)
    {
        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        DtoAbbonamento? risultato = await _abbonamentoService.CreazioneAsync(dto);

        if (risultato == null)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync( utenteId,"Creazione abbonamento",false);

            return BadRequest(new { messaggio = "Abbonamento già presente oppure non valido." });
        }

        await _logAzioniService.SalvataggioLogAzioneAsync( utenteId,"Creazione abbonamento",true);

        return Ok(risultato);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = Ruoli.GestoreOrOperatore)]
    public async Task<IActionResult> Modifica(string id, [FromBody] DtoCreazioneAbbonamento dto)
    {
        DtoAbbonamento? risultato = await _abbonamentoService.ModificaAsync(id, dto);
        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (risultato == null)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId,"Modifica abbonamento",false);

            return NotFound(new { messaggio = "Abbonamento non trovato." });
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId,"Modifica abbonamento",true);

        return Ok(risultato);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = Ruoli.GestoreOrOperatore)]
    public async Task<IActionResult> Elimina(string id)
    {
        bool eliminato = await _abbonamentoService.EliminazioneAsync(id);
        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!eliminato)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId,"Elimina abbonamento", false);

            return NotFound(new { messaggio = "Abbonamento non trovato." });
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId,"Elimina abbonamento", true);

        return NoContent();
    }
}