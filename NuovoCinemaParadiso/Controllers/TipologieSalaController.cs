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
public class TipologieSalaController : ControllerBase
{
    private readonly TipologiaSalaService _tipologiaSalaService;
    private readonly LogAzioniService _logAzioniService;

    public TipologieSalaController(TipologiaSalaService tipologiaSalaService, LogAzioniService logAzioniService)
    {
        _tipologiaSalaService = tipologiaSalaService;
        _logAzioniService = logAzioniService;
    }

    [HttpGet]
    public async Task<IActionResult> OttieniTutti()
    {
        List<DtoTipologiaSala> tipologieSala = await _tipologiaSalaService.OttieniTuttoAsync();
        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
        {
            IdUtente = utenteId,
            NomeAzione = "Ottieni tutte le tipologie",
            Effettuato = true,
            Messaggio = "Operazione eseguita"
        });

        return Ok(tipologieSala);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> OttieniTramiteId(string id)
    {
        var risultato = await _tipologiaSalaService.OttieniTramiteIdAsync(id);
        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (risultato == null)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
            {
                IdUtente = utenteId,
                NomeAzione = "Ottieni tipologia tramite id",
                Effettuato = false,
                Messaggio = "Operazione fallita"
            });

            return NotFound($"TipologiaSala con id {id} non trovato");
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
        {
            IdUtente = utenteId,
            NomeAzione = "Ottieni tipologia tramite id",
            Effettuato = true,
            Messaggio = "Operazione eseguita"
        });

        return Ok(risultato);
    }

    [HttpPost]
    [Authorize(Roles = Ruoli.GestoreOrOperatore)]
    public async Task<IActionResult> Creazione([FromBody] DtoCreazioneTipologiaSala dto)
    {
        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        List<DtoTipologiaSala> tipologieSala = await _tipologiaSalaService.OttieniTuttoAsync();
        
        foreach (var tipologiaSala in tipologieSala)
        {
            if (tipologiaSala.Nome.Contains(dto.Nome))
            {
                    await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
                    {
                        IdUtente = utenteId,
                        NomeAzione = "Creazione tipologia",
                        Effettuato = false,
                        Messaggio = "Operazione fallita"
                    });

                return BadRequest(new { messaggio = "Tipologia sala già presente." });
            }
        }

        DtoTipologiaSala? risultato = await _tipologiaSalaService.CreazioneAsync(dto);

        if (risultato == null)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
            {
                IdUtente = utenteId,
                NomeAzione = "Creazione tipologia",
                Effettuato = false,
                Messaggio = "Operazione fallita"
            });

            return BadRequest(new { messaggio = "Tipologia sala non valida." });
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
        {
            IdUtente = utenteId,
            NomeAzione = "Creazione tipologia",
            Effettuato = true,
            Messaggio = "Operazione eseguita"
        });

        return Ok(risultato);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = Ruoli.GestoreOrOperatore)]
    public async Task<IActionResult> Modifica(string id, [FromBody] DtoCreazioneTipologiaSala dto)
    {
        DtoTipologiaSala? risultato = await _tipologiaSalaService.ModificaAsync(id, dto);
        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (risultato == null)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
            {
                IdUtente = utenteId,
                NomeAzione = "Modifica tipologia",
                Effettuato = false,
                Messaggio = "Operazione fallita"
            });

            return NotFound(new { messaggio = "Tipologia sala non trovata." });
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
        {
            IdUtente = utenteId,
            NomeAzione = "Modifica tipologia",
            Effettuato = true,
            Messaggio = "Operazione eseguita"
        });

        return Ok(risultato);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = Ruoli.GestoreOrOperatore)]
    public async Task<IActionResult> Elimina(string id)
    {
        bool eliminato = await _tipologiaSalaService.EliminaAsync(id);
        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!eliminato)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
            {
                IdUtente = utenteId,
                NomeAzione = "Elimina tipologia",
                Effettuato = false,
                Messaggio = "Operazione fallita"
            });

            return NotFound(new { messaggio = "Tipologia sala non trovata." });
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
        {
            IdUtente = utenteId,
            NomeAzione = "Elimina tipologia",
            Effettuato = true,
            Messaggio = "Operazione eseguita"
        });

        return NoContent();
    }
}