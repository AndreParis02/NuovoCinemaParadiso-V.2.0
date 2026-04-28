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
public class ProiezioneController : ControllerBase
{
    private readonly ProiezioneService _proiezioneService;
    private readonly LogAzioniService _logAzioniService;

    public ProiezioneController(ProiezioneService proiezioneService, LogAzioniService logAzioniService)
    {
        _proiezioneService = proiezioneService;
        _logAzioniService = logAzioniService;
    }

    [HttpGet]
    public async Task<IActionResult> OttieniTutteLeProiezioni()
    {
        List<DtoProiezione> proiezioni = await _proiezioneService.OttieniTuttoAsync();
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");

        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ottieni tutte le proieioni",true);

        return Ok(proiezioni);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> OttieniTramiteId(string id)
    {
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");

        var risultato = await _proiezioneService.OttieniTramiteIdAsync(id);

        if (risultato == null)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ottieni proiezione tramite id",false);

            return NotFound($"Proiezione con id {id} non trovato");
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ottieni proiezione tramite id", true);

        return Ok(risultato);
    }

    [HttpGet("turno/{turnoId}")]
    public async Task<ActionResult<List<DtoProiezione>>> OttieniPerTurno(string turnoId)
    {
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");

        if (string.IsNullOrEmpty(turnoId))
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ottieni proiezione per turnoId", false);

            return BadRequest("TurnoId non valido");
        }

        var risultato = await _proiezioneService.OttieniTramiteTurnoAsync(turnoId);

        if (risultato == null)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ottieni proiezione per turnoid", false);

            return NotFound("Nessuna proiezione trovata per questo turno");
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ottieni proiezione per turnoId", true);
        return Ok(risultato);
    }

    [HttpGet("sala/{salaId}")]
    public async Task<ActionResult<List<DtoProiezione>>> OttieniPerSala(string salaId)
    {
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");

        if (string.IsNullOrEmpty(salaId))
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ottieni proiezione per sala", false);
            return BadRequest("SalaId non valido");
        }

        var risultato = await _proiezioneService.OttieniTramiteSalaAsync(salaId);

        if (risultato == null)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ottieni proiezione per sala", false);
            return NotFound("Nessuna proiezione trovata per questa sala");
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ottieni proiezione per sala", true);

        return Ok(risultato);
    }

    [HttpGet("movie/{movieId}")]
    public async Task<ActionResult<List<DtoProiezione>>>OttieniPerFilm(string movieId)
    {
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");

        if (string.IsNullOrEmpty(movieId))
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ottieni proiezioni per film", false);
            return BadRequest("MovieId non valido");
        }

        var risultato = await _proiezioneService.OttieniTramiteMovieAsync(movieId);

        if (risultato == null)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ottieni proiezioni per film", false);
            return NotFound("Nessuna proiezione trovata per questo film");
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ottieni proiezioni per film", true);

        return Ok(risultato);
    }


    [HttpPost]
    [Authorize(Roles = Ruoli.GestoreOrOperatore)]
    public async Task<IActionResult> Creazione([FromBody] DtoCreazioneProiezione dto)
    {
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");

        List<DtoProiezione> proiezioni = await _proiezioneService.OttieniTuttoAsync();

        foreach (var proiezione in proiezioni)
        {
            if (proiezione.TurnoId == dto.TurnoId && proiezione.SalaId == dto.SalaId && proiezione.DataProiezione == dto.DataProiezione)
            {
                await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Crea proiezione", false);

                return BadRequest(new { messaggio = "Proiezione già presente." });
            }
        }

        DtoProiezione? risultato = await _proiezioneService.CreazioneAsync(dto);

        if (risultato == null)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Crea proiezione", false);
            return BadRequest(new { messaggio = "Proiezione non valida." });
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Crea proiezione", true);
        return Ok(risultato);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = Ruoli.GestoreOrOperatore)]
    public async Task<IActionResult> Modifica(string id, [FromBody] DtoCreazioneProiezione dto)
    {
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");

        DtoProiezione? risultato = await _proiezioneService.ModificaAsync(id,dto);

        if (risultato == null)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Modifica proiezione", false);

            return NotFound(new { messaggio = "Proiezione non trovata." });
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Modifica proiezione", true);

        return Ok(risultato);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = Ruoli.GestoreOrOperatore)]
    public async Task<IActionResult> Elimina(string id)
    {
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");

        bool eliminato = await _proiezioneService.EliminaAsync(id);

        if (!eliminato)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Elimina proiezione", false);

            return NotFound(new { messaggio = "Proiezione non trovato." });
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Elimina proiezione", true);

        return NoContent();
    }
}