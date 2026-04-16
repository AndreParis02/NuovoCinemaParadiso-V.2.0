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
        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
        {
            IdUtente = utenteId,
            NomeAzione = "Ottieni tutte le proiezioni",
            Effettuato = true,
            Messaggio = "Operazione eseguita"
        });

        return Ok(proiezioni);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> OttieniTramiteId(string id)
    {
        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var risultato = await _proiezioneService.OttieniTramiteIdAsync(id);

        if (risultato == null)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
            {
                IdUtente = utenteId,
                NomeAzione = "Ottieni proiezione tramite id",
                Effettuato = false,
                Messaggio = "Operazione fallita"
            });

            return NotFound($"Proiezione con id {id} non trovato");
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
        {
            IdUtente = utenteId,
            NomeAzione = "Ottieni proiezione tramite id",
            Effettuato = true,
            Messaggio = "Operazione eseguita"
        });

        return Ok(risultato);
    }
    [HttpGet("turno/{turnoId}")]
    public async Task<ActionResult<List<DtoProiezione>>> OttieniPerTurno(string turnoId)
    {
        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (turnoId == null)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
            {
                IdUtente = utenteId,
                NomeAzione = "Ottieni proiezioni per turno",
                Effettuato = false,
                Messaggio = "Operazione fallita"
            });

            return BadRequest("TurnoId non valido");
        }

        var risultato = await _proiezioneService.OttieniTramiteTurnoAsync(turnoId);

        if (risultato == null || risultato.Count == 0)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
            {
                IdUtente = utenteId,
                NomeAzione = "Ottieni proiezioni per turno",
                Effettuato = false,
                Messaggio = "Operazione fallita"
            });

            return NotFound("Nessuna proiezione trovata per questo turno");
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
        {
            IdUtente = utenteId,
            NomeAzione = "Ottieni proiezioni per turno",
            Effettuato = true,
            Messaggio = "Operazione eseguita"
        });

        return Ok(risultato);
    }

    [HttpPost]
    [Authorize(Roles = Ruoli.GestoreOrOperatore)]
    public async Task<IActionResult> Creazione([FromBody] DtoCreazioneProiezione dto)
    {
        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        List<DtoProiezione> proiezioni = await _proiezioneService.OttieniTuttoAsync();

        foreach (var proiezione in proiezioni)
        {
            if (proiezione.TurnoId == dto.TurnoId && proiezione.SalaId == dto.SalaId && proiezione.DataProiezione == dto.DataProiezione)
            {
                await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
                {
                    IdUtente = utenteId,
                    NomeAzione = "Creazione movie",
                    Effettuato = false,
                    Messaggio = "Operazione fallita"
                });

                return BadRequest(new { messaggio = "Proiezione già presente." });
            }
        }

        DtoProiezione? risultato = await _proiezioneService.CreazioneAsync(dto);

        if (risultato == null)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
            {
                IdUtente = utenteId,
                NomeAzione = "Creazione proiezione",
                Effettuato = false,
                Messaggio = "Operazione fallita"
            });

            return BadRequest(new { messaggio = "Proiezione non valida." });
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
        {
            IdUtente = utenteId,
            NomeAzione = "Creazione proiezione",
            Effettuato = true,
            Messaggio = "Operazione eseguita"
        });

        return Ok(risultato);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = Ruoli.GestoreOrOperatore)]
    public async Task<IActionResult> Modifica(string id, [FromBody] DtoCreazioneProiezione dto)
    {
        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        DtoProiezione? risultato = await _proiezioneService.ModificaAsync(id,dto);

        if (risultato == null)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
            {
                IdUtente = utenteId,
                NomeAzione = "Modifica proiezione",
                Effettuato = false,
                Messaggio = "Operazione fallita"
            });

            return NotFound(new { messaggio = "Proiezione non trovata." });
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
        {
            IdUtente = utenteId,
            NomeAzione = "Modifica proiezione",
            Effettuato = true,
            Messaggio = "Operazione eseguita"
        });

        return Ok(risultato);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = Ruoli.GestoreOrOperatore)]
    public async Task<IActionResult> Elimina(string id)
    {
        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        bool eliminato = await _proiezioneService.EliminaAsync(id);

        if (!eliminato)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
            {
                IdUtente = utenteId,
                NomeAzione = "Elimina proiezione",
                Effettuato = false,
                Messaggio = "Operazione fallita"
            });

            return NotFound(new { messaggio = "Proiezione non trovato." });
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
        {
            IdUtente = utenteId,
            NomeAzione = "Elimina proiezione",
            Effettuato = true,
            Messaggio = "Operazione eseguita"
        });

        return NoContent();
    }
}