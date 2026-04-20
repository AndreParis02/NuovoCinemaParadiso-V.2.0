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

        await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
        {
            IdUtente = utenteId,
            NomeAzione = "Ricerca profili",
            Effettuato = true,
            Messaggio = "Ricerca avvenuta"
        });

        await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
        {
            IdUtente = utenteId,
            NomeAzione = "Ottieni tutti i profili",
            Effettuato = true,
            Messaggio = "Operazione eseguita"
        });

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
            await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
            {
                IdUtente = utenteId,
                NomeAzione = "Ricerca profilo",
                Effettuato = false,
                Messaggio = "Ricerca fallita"
            });
            return NotFound(new { messaggio = "Utente non trovato." });
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
        {
            IdUtente = utenteId,
            NomeAzione = "Ricerca profilo",
            Effettuato = true,
            Messaggio = "Ricerca avvenuta"
        });
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
            await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
            {
                IdUtente = utenteId,
                NomeAzione = "Eliminazione profilo",
                Effettuato = false,
                Messaggio = "Eliminazione profilo fallita"
            });

            return NotFound(new { messaggio = "Utente non trovato." });
        }
        await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
        {
            IdUtente = utenteId,
            NomeAzione = "Eliminazione profilo",
            Effettuato = true,
            Messaggio = "Eliminazione profilo avvenuta"
        });
        return Ok(risultato);
    }

    [HttpGet("acquisti")]
    [Authorize(Roles = Ruoli.GestoreOrOperatore)]
    public async Task<IActionResult> OttieniTuttiGliAcquisti()
    {
        List<DtoAcquisto> acquisti = await _adminService.OttieniAcquisti();
        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
        {
            IdUtente = utenteId,
            NomeAzione = "Ottieni tutti gli acquisti admin",
            Effettuato = true,
            Messaggio = "Operazione eseguita"
        });

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
            await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
            {
                IdUtente = utenteId,
                NomeAzione = "Ottieni acquisti tramite id admin",
                Effettuato = false,
                Messaggio = "Operazione fallita"
            });

            return NotFound($"Acquisto con id {id} non trovato");
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
        {
            IdUtente = utenteId,
            NomeAzione = "Ottieni acquisti tramite id admin",
            Effettuato = true,
            Messaggio = "Operazione eseguita"
        });

        return Ok(risultato);
    }

    [HttpGet("utenti/{abbonamentoid}")]
    [Authorize(Roles = Ruoli.GestoreOrOperatore)]
    public async Task<ActionResult<List<DtoUtente>>> OttieniUtentiTramiteAbbonamentoAsync(string abbonamentoId)
    {
        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrWhiteSpace(abbonamentoId))
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
            {
                IdUtente= utenteId,
                NomeAzione = "Ottieni gli utenti per abbonamento",
                Effettuato = false,
                Messaggio = "Operazione fallita"
            });

            return BadRequest("AbbonamentoId non valido");
        }

        var risultato = await _adminService.OttieniUtentiTramiteAbbonamentoAsync(abbonamentoId);

        if (risultato == null || risultato.Count == 0)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
            {
                IdUtente = utenteId,
                NomeAzione = "Ottieni gli utenti per abbonamento",
                Effettuato = false,
                Messaggio = "Operazione fallita"
            });

            return NotFound("Nessun utente trovato per questo abbonamento");
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
        {
            IdUtente = utenteId,
            NomeAzione = "Ottieni gli utenti per abbonamento",
            Effettuato = true,
            Messaggio = "Operazione eseguita"
        });

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
            await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
            {
                IdUtente = utenteId,
                NomeAzione = "Ottieni abbonamenti tramite id admin",
                Effettuato = false,
                Messaggio = "Operazione fallita"
            });

            return NotFound($"Abbonamento con id {id} non trovato");
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
        {
            IdUtente = utenteId,
            NomeAzione = "Ottieni abbonamenti tramite id admin",
            Effettuato = true,
            Messaggio = "Operazione eseguita"
        });

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
            await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
            {
                IdUtente = utenteId,
                NomeAzione = "Ottieni giftcard tramite id admin",
                Effettuato = false,
                Messaggio = "Operazione fallita"
            });

            return NotFound($"Giftcard con id {id} non trovato");
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
        {
            IdUtente = utenteId,
            NomeAzione = "Ottieni giftcard tramite id admin",
            Effettuato = true,
            Messaggio = "Operazione eseguita"
        });

        return Ok(risultato);
    }

    [HttpGet("utenti/{giftcardId}")]
    [Authorize(Roles = Ruoli.GestoreOrOperatore)]
    public async Task<ActionResult<List<DtoUtente>>> OttieniUtentiTramiteGiftCardPerAdmin(string giftcardId)
    {
        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrWhiteSpace(giftcardId))
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
            {
                IdUtente= utenteId,
                NomeAzione = "Ottieni gli utenti per giftcard",
                Effettuato = false,
                Messaggio = "Operazione fallita"
            });

            return BadRequest("giftcardId non valido");
        }

        var risultato = await _adminService.OttieniUtentiTramiteGiftCardAsync(giftcardId);

        if (risultato == null)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
            {
                IdUtente = utenteId,
                NomeAzione = "Ottieni gli utenti per giftcard",
                Effettuato = false,
                Messaggio = "Operazione fallita"
            });

            return NotFound("Nessun utente trovato per questa giftcard");
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
        {
            IdUtente = utenteId,
            NomeAzione = "Ottieni gli utenti per giftcard",
            Effettuato = true,
            Messaggio = "Operazione eseguita"
        });

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
        
    
