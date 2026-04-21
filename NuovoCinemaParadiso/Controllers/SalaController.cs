using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using NuovoCinemaParadiso.Services;
using NuovoCinemaParadiso.Dtos;

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
        List<DtoSala> sale = await _salaService.OttieniTuttoAsync();
        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        await Log(utenteId, "Ottieni tutte le sale", true);

        return Ok(sale);
    }

    [HttpGet("tipologia/{tipologiaId}")]
    public async Task<ActionResult<List<DtoSala>>> OttieniPerTipologia(string tipologiaId)
    {
        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrWhiteSpace(tipologiaId))
        {
            await Log(utenteId, "Ottieni sala per tipologia", false);

            return BadRequest("TipologiaId non valido");
        }

        var risultato = await _salaService.OttieniTramiteTipologiaAsync(tipologiaId);

        if (risultato == null || risultato.Count == 0)
        {
            await Log(utenteId, "Ottieni sala per tipologia", false);

            return NotFound("Nessuna sala trovata per questa tipologia");
        }

        await Log(utenteId, "Ottieni sala per tipologia", true);

        return Ok(risultato);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> OttieniTramiteId(string id)
    {
        var risultato = await _salaService.OttieniTramiteIdAsync(id);
        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (risultato == null)
        {
            await Log(utenteId, "Ottieni sala tramite id", false);

            return NotFound($"Sala con id {id} non trovato");
        }

        await Log(utenteId, "Ottieni sala tramite id", true);

        return Ok(risultato);
    }

    [HttpPost]
    [Authorize(Roles = Ruoli.GestoreOrOperatore)]
    public async Task<IActionResult> Creazione([FromBody] DtoCreazioneSala dto)
    {
        DtoSala? risultato = await _salaService.CreazioneAsync(dto);
        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (risultato == null)
        {
            await Log(utenteId, "Creazione sala", false);

            return BadRequest(new { messaggio = "Sala non valida." });
        }

        await Log(utenteId, "Creazione sala", true);

        return Ok(risultato);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = Ruoli.GestoreOrOperatore)]
    public async Task<IActionResult> Modifica(string id, [FromBody] DtoCreazioneSala dto)
    {
        DtoSala? risultato = await _salaService.ModificaAsync(id, dto);
        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (risultato == null)
        {
            await Log(utenteId, "Modifica sala", false);

            return NotFound(new { messaggio = "Sala non trovata." });
        }

        await Log(utenteId, "Modifica sala", true);

        return Ok(risultato);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = Ruoli.GestoreOrOperatore)]
    public async Task<IActionResult> Elimina(string id)
    {
        bool eliminato = await _salaService.EliminaAsync(id);
        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!eliminato)
        {
            await Log(utenteId, "Elimina sala", false);

            return NotFound(new { messaggio = "Sala non trovata." });
        }

        await Log(utenteId, "Elimina sala", true);

        return NoContent();
    }
    public async Task Log(string utenteId, string azione, bool risultato)
    {
        string messaggio = "Operazione fallita";
        if (risultato) messaggio = "Operazione eseguita";

        await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
        {
            IdUtente = utenteId,
            NomeAzione = azione,
            Effettuato = risultato,
            Messaggio = messaggio
        });
    }
}