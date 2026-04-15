using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using NuovoCinemaParadiso.Dtos;
using NuovoCinemaParadiso.Services;

namespace NuovoCinemaParadiso.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class FasciaOrariaController : ControllerBase
{
    private readonly FasciaOrariaService _fasciaOrariaService;
    private readonly LogAzioniService _logAzioniService;

    public FasciaOrariaController(FasciaOrariaService fasciaOrariaService, LogAzioniService logAzioniService)
    {
        _fasciaOrariaService = fasciaOrariaService;
        _logAzioniService = logAzioniService;
    }

    [HttpGet]
    public async Task<IActionResult> OttieniTutti()
    {
        List<DtoFasciaOraria> fasceOrarie = await _fasciaOrariaService.OttieniTuttoAsync();
        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
        {
            IdUtente = utenteId,
            NomeAzione = "Ottieni tutte le fasce orarie",
            Effettuato = true,
            Messaggio = "Operazione eseguita"
        });

        return Ok(fasceOrarie);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> OttieniTramiteId(string id)
    {
        var risultato = await _fasciaOrariaService.OttieniTramiteIdAsync(id);
        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (risultato == null)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
            {
                IdUtente = utenteId,
                NomeAzione = "Ottieni fascia oraria tramite id",
                Effettuato = false,
                Messaggio = "Operazione fallita"
            });

            return NotFound($"TipologiaSala con id {id} non trovato");
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
        {
            IdUtente = utenteId,
            NomeAzione = "Ottieni fascia oraria tramite id",
            Effettuato = true,
            Messaggio = "Operazione eseguita"
        });

        return Ok(risultato);
    }

    [HttpPost]
    [Authorize(Roles = Ruoli.GestoreOrOperatore)]
    public async Task<IActionResult> Creazione([FromBody] DtoCreazioneFasciaOraria dto)
    {
        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        List<DtoFasciaOraria> fasceOrarie = await _fasciaOrariaService.OttieniTuttoAsync();

        foreach (var fasciaOraria in fasceOrarie)
        {
            if (fasciaOraria.Nome.Contains(dto.Nome))
            {
                await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
                {
                    IdUtente = utenteId,
                    NomeAzione = "Creazione fascia oraria",
                    Effettuato = false,
                    Messaggio = "Operazione fallita"
                });

                return BadRequest(new { messaggio = "Fascia oraria già presente." });
            }
        }
        
        DtoFasciaOraria? risultato = await _fasciaOrariaService.CreazioneAsync(dto);

        if (risultato == null)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
            {
                IdUtente = utenteId,
                NomeAzione = "Creazione fasce oraria",
                Effettuato = false,
                Messaggio = "Operazione fallita"
            });

            return BadRequest(new { messaggio = "Fascia oraria non valida." });
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
        {
            IdUtente = utenteId,
            NomeAzione = "Creazione fasce oraria",
            Effettuato = true,
            Messaggio = "Operazione eseguita"
        });

        return Ok(risultato);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = Ruoli.GestoreOrOperatore)]
    public async Task<IActionResult> Modifica(string id, [FromBody] DtoCreazioneFasciaOraria dto)
    {
        DtoFasciaOraria? risultato = await _fasciaOrariaService.ModificaAsync(id, dto);
        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (risultato == null)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
            {
                IdUtente = utenteId,
                NomeAzione = "Modifica fasce oraria",
                Effettuato = false,
                Messaggio = "Operazione fallita"
            });

            return NotFound(new { messaggio = "Fascia oraria non trovata." });
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
        {
            IdUtente = utenteId,
            NomeAzione = "Modifica fasce oraria",
            Effettuato = true,
            Messaggio = "Operazione eseguita"
        });

        return Ok(risultato);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = Ruoli.GestoreOrOperatore)]
    public async Task<IActionResult> Elimina(string id)
    {
        bool eliminato = await _fasciaOrariaService.EliminaAsync(id);
        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!eliminato)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
            {
                IdUtente = utenteId,
                NomeAzione = "Elimina fascia oraria",
                Effettuato = false,
                Messaggio = "Operazione fallita"
            });

            return NotFound(new { messaggio = "Fascia oraria non trovata." });
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
        {
            IdUtente = utenteId,
            NomeAzione = "Elimina fascia oraria",
            Effettuato = true,
            Messaggio = "Operazione eseguita"
        });
        return NoContent();
    }
}