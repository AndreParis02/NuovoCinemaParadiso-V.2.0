
### MovieController.cs Versione 1.4

Utente: Marco Strazzeri
Data: 10/06/2026
Descrizione: Modificato l'if che controlla se un film è già esistente. Corretto messaggio d'errore nell' eliminazione movie

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
[Authorize]
public class MovieController : ControllerBase
{
    private readonly MovieService _movieService;
    private readonly LogAzioniService _logAzioniService;
    

    public MovieController(MovieService movieService, LogAzioniService logAzioniService)
    {
        _movieService = movieService;
        _logAzioniService = logAzioniService;
    }


    [HttpGet]
    public async Task<IActionResult> OttieniTuttiIMovies()
    {
        List<DtoMovie> movies = await _movieService.OttieniTutto();
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");

        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId,"Ottieni tutti i movies" ,true);
        return Ok(movies);
    }

    [HttpGet("storico")]
    public async Task<IActionResult> OttieniTuttiIMoviesStorico()
    {
        List<DtoMovie> movies = await _movieService.OttieniTuttoStorico();
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");

        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId,"Ottieni tutti i movies" ,true);
        return Ok(movies);
    }
    

    [HttpGet("genere/{genereId}")]
    public async Task<ActionResult<List<DtoMovie>>> OttieniPerGenere(string genereId)
    {
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");

        if (string.IsNullOrWhiteSpace(genereId))
        {
          await _logAzioniService.SalvataggioLogAzioneAsync(utenteId,"Ottieni movie per genereId" ,false);
          return BadRequest("GenereId non valido");
        }

        var risultato = await _movieService.OttieniTramiteGenere(genereId);

        if (risultato.Count() == 0)
        {
          await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ottieni movie per genereid", false);

          return NotFound("Nessun film trovato per questo genere");
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ottieni movie per genereid", true);;

        return Ok(risultato);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> OttieniTramiteId(string id)
    {
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");

        DtoMovie? risultato = await _movieService.OttieniTramiteIdAsync(id);

        if (risultato == null)
        {
          await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ottieni movie per id", false);
          return NotFound($"Film con id {id} non trovato");
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Ottieni movie per id", true);
        return Ok(risultato);
    }

    [HttpPost]
    [Authorize(Roles = Ruoli.Operatore)]
    public async Task<IActionResult> Creazione([FromBody] DtoCreazioneMovie dto)
    {
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");

        List<DtoMovie> movies = await _movieService.OttieniTutto();

        foreach (var movie in movies)
        {
            //controlla se esiste già un film con lo stesso titolo (ignorando maiuscole/minuscole)
            if (movie.Titolo.Equals(dto.Titolo, StringComparison.OrdinalIgnoreCase))
            {
              await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Creazione movie", false);
              return BadRequest(new { messaggio = "Film già presente." });
            }
        }
        
        bool risultato = await _movieService.CreazioneAsync(dto);

        if (!risultato)
        {
          await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Creazione movie", false);
          return BadRequest(new { messaggio = "id del genere non valido." });
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Creazione movie", true);
        return Ok();
    }

    [HttpPut("{id}")]
    [Authorize(Roles = Ruoli.Operatore)]
    public async Task<IActionResult> Modifica(string id, [FromBody] DtoCreazioneMovie dto)
    {
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");

        List<DtoMovie> movies = await _movieService.OttieniTutto();

        foreach (var movie in movies)
        {
            //controlla se esiste già un film con lo stesso titolo (ignorando maiuscole/minuscole)
            if (movie.Titolo.Equals(dto.Titolo, StringComparison.OrdinalIgnoreCase) && movie.Id != id)
            {
              await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Creazione movie", false);
              return BadRequest(new { messaggio = "non è possibile modificare il titolo con uno già esistente." });
            }
        }
        try
        {
            await _movieService.ModificaAsync(id, dto);
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Modifica movie", true);
            return Ok();
        }
        catch(NotFoundException ex)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Modifica movie", false);
            return NotFound(new { messaggio = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = Ruoli.Operatore)]
    public async Task<IActionResult> Elimina(string id)
    {
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");

        bool eliminato = await _movieService.EliminaAsync(id);

        if (!eliminato)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Eliminazione movie", false);
            //corretto messaggio d'errore
            return NotFound(new { messaggio = "Film non trovato o già eliminato." });
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Eliminazione movie", true);
        return Ok(new { messaggio = "Film eliminato con successo!" });
    }
}

```