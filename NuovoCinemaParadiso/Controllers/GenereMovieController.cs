using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using NuovoCinemaParadiso.Dtos;
using NuovoCinemaParadiso.Services;
using NuovoCinemaParadiso.Exceptions;

namespace NuovoCinemaParadiso.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class GenereMovieController : ControllerBase
{
    private readonly GenereMovieService _genereMovieService;
    private readonly LogAzioniService _logAzioniService;

    public GenereMovieController(GenereMovieService genereMovieService, LogAzioniService logAzioniService)
    {
        _genereMovieService = genereMovieService;
        _logAzioniService = logAzioniService;
    }

    [HttpGet]
    public async Task<IActionResult> OttieniTutti()
    {
        List<DtoGenereMovie> generiMovie = await _genereMovieService.OttieniTuttoAsync();
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");

        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ottieni tutti i generi", true);

        return Ok(generiMovie);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> OttieniTramiteId(string id)
    {
        var risultato = await _genereMovieService.OttieniTramiteIdAsync(id);
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");

        if (risultato == null)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ottieni genere tramite id", false);

            return NotFound($"GenereMovie con id {id} non trovato");
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ottieni genere tramite id", true);

        return Ok(risultato);
    }

    /*[HttpPost]
    [Authorize(Roles = Ruoli.Operatore)]
    public async Task<IActionResult> Creazione([FromBody] DtoCreazioneGenereMovie dto)
    {
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");

        List<DtoGenereMovie> generiMovie = await _genereMovieService.OttieniTuttoAsync();

        foreach (var generiMovies in generiMovie)
        {
            if (generiMovies.Genere.Contains(dto.Genere))
            {
                await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Creazione genere", false);

                return BadRequest(new { messaggio = "Genere già presente." });
            }
        }

        DtoGenereMovie? risultato = await _genereMovieService.CreazioneAsync(dto);

        if (risultato == null)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Creazione genere", false);

            return BadRequest(new { messaggio = "Genere non valido." });
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Creazione genere", true);

        return Ok(risultato);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = Ruoli.Operatore)]
    public async Task<IActionResult> Modifica(string id, [FromBody] DtoCreazioneGenereMovie dto)
    {
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");

        try
        {
            DtoGenereMovie? risultato = await _genereMovieService.ModificaAsync(id, dto);
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Modifica genere", true);

            return Ok(risultato);

        }
        catch (ModificaException ex)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Modifica genere", false);
            return BadRequest(new { message = ex.Message });
        }

        catch (ItemNotFoundException ex)
        {

            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Modifica genere", false);

            return NotFound(new { message = ex.Message });

        }
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = Ruoli.Operatore)]
    public async Task<IActionResult> Elimina(string id)
    {
        bool eliminato = await _genereMovieService.EliminaAsync(id);
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");

        if (!eliminato)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Eliminazione genere", false);

            return NotFound(new { messaggio = "Genere non trovato." });
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Eliminazione genere", true);

        return Ok(new { message = "Il Genere è stato eliminato correttamente" });
    }*/
}