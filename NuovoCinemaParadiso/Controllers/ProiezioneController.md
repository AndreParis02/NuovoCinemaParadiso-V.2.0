### ProiezioneController.cs Versione 1.3

Utente: Marco Strazzeri
Data: 11/06/2026
Descrizione: Modificati i metodi di lettura OttieniPerTurno, OttieniPerSala, OttieniPerFilm



```c#

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using NuovoCinemaParadiso.Services;
using NuovoCinemaParadiso.Dtos;
using NuovoCinemaParadiso.Exceptions;

namespace NuovoCinemaParadiso.Controllers;

[ApiController]
[Route("api/[controller]")]
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

        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ottieni tutte le proieioni", true);

        return Ok(proiezioni);
    }

    [HttpGet("storico")]
    [Authorize(Roles = Ruoli.Operatore)]
    public async Task<IActionResult> OttieniStoricoProiezioni()
    {
        List<DtoProiezione> proiezioni = await _proiezioneService.OttieniStoricoAsync();
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");

        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ottieni tutte le proieioni", true);

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
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ottieni proiezione tramite id", false);

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

        if (risultato.Count == 0)   //controlla se sono stati trovati zero risultati per il turnoId specificato
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

        if (risultato.Count == 0) //controlla se sono stati trovati zero risultati per il salaId specificato
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ottieni proiezione per sala", false);
            return NotFound("Nessuna proiezione trovata per questa sala");
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ottieni proiezione per sala", true);

        return Ok(risultato);
    }

    [HttpGet("movie/{movieId}")]
    public async Task<ActionResult<List<DtoProiezione>>> OttieniPerFilm(string movieId)
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

        if (risultato.Count == 0)//controlla se sono stati trovati zero risultati per il movieId specificato
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ottieni proiezioni per film", false);
            return NotFound("Nessuna proiezione trovata per questo film");
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ottieni proiezioni per film", true);

        return Ok(risultato);
    }


    [HttpPost]
    [Authorize(Roles = Ruoli.Operatore)]
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
        try
        {
            bool creato = await _proiezioneService.CreazioneAsync(dto);
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Crea proiezione", true);
            return Ok(new { messaggio = "Creazione avvenuta con successo!" });
        }
        catch (NotFoundException ex)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Crea proiezione", false);

            return BadRequest(new { messaggio = $"Errore durante la creazione della proiezione: {ex.Message}" });
        }


    }

    [HttpPut("{id}")]
    [Authorize(Roles = Ruoli.Operatore)]
    public async Task<IActionResult> Modifica(string id, [FromBody] DtoCreazioneProiezione dto)
    {
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");

        try{
        bool modificato = await _proiezioneService.ModificaAsync(id, dto);

        if (!modificato)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Modifica proiezione", false);

            return NotFound(new { messaggio = "Proiezione non trovata." });
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Modifica proiezione", true);

        return Ok(new { messaggio = "Proiezione modificata con successo!" });
        }
        catch(NotFoundException ex)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Modifica Proiezione", false);
            return NotFound(new { messaggio = ex.Message });
        }
        
    }

    [HttpPut("elimina/{id}")]
    public async Task<IActionResult> Elimina(string id)
    {
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");

        bool eliminato = await _proiezioneService.EliminaAsync(id);

        if (!eliminato)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Elimina proiezione", false);

            return NotFound(new { messaggio = "Proiezione non trovata." });
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Elimina proiezione", true);

        return Ok(new { messaggio = "Proiezione eliminata con successo!" });
    }
}

```