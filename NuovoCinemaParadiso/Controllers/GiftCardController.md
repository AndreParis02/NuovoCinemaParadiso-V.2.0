### GiftCardController.cs Versione 1.1

Utente: Marco Strazzeri
Data: 10/06/2026
Descrizione: Modificato il metodo OttieniTramiteId che aveva una riga di logging errata

```c#

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using NuovoCinemaParadiso.Services;
using NuovoCinemaParadiso.Dtos;

namespace NuovoCinemaParadiso.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class GiftCardController : ControllerBase
{
    private readonly GiftCardService _giftCardService;
    private readonly LogAzioniService _logAzioniService;

    public GiftCardController(GiftCardService giftCardService, LogAzioniService logAzioniService)
    {
        _giftCardService = giftCardService;
        _logAzioniService = logAzioniService;
    }

    [HttpGet]
    [Authorize(Roles = Ruoli.Gestore)]
    public async Task<IActionResult> OttieniTutteLeGiftCard()
    {
        List<DtoGiftCard> giftCards = await _giftCardService.OttieniTutto();
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");

        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId,"Ottieni tutte le giftcard" ,true);

        return Ok(giftCards);
    }

    [HttpGet("mie")]
    [Authorize(Roles = Ruoli.Utente)]
    public async Task<IActionResult> OttieniMieGiftCard()
    {
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");

        List<DtoGiftCard> mieGiftCards = await _giftCardService.OttieniPerUtenteAsync(utenteId);

        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Visualizzazione proprie giftcard", true);

        return Ok(mieGiftCards);
    }

    [HttpGet("{id}")]
     [Authorize(Roles = Ruoli.Gestore)]
    public async Task<IActionResult> OttieniTramiteId(string id)
    {
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");

        var risultato = await _giftCardService.OttieniTramiteIdAsync(id, utenteId);

        if (risultato == null)
        {
            //cambiato da true a false
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId,"Ottieni giftcard tramite id" ,false);
            return NotFound($"GiftCard con id {id} non trovato");
        }

        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId,"Ottieni giftcard tramite id" ,true);

        return Ok(risultato);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = Ruoli.GestoreOrOperatore)]
    public async Task<IActionResult> Modifica(string id, [FromBody] DtoCreazioneGiftCard dto)
    {
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");

        DtoGiftCard? risultato = await _giftCardService.ModificaAsync(id,dto);

        if (risultato == null)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync(utenteId,"Modifica giftcard" ,false);
            return NotFound(new { messaggio = "GiftCard non trovata." });
        }
        await _logAzioniService.SalvataggioLogAzioneAsync(utenteId, "Modifica GiftCard", true );

        return Ok(risultato);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = Ruoli.GestoreOrOperatore)]
    public async Task<IActionResult> Elimina(string id)
    {
        string? utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (utenteId == null)
            return Unauthorized("Utente non autenticato.");

        bool eliminato = await _giftCardService.EliminazioneAsync(id);

        if (!eliminato)
        {
            await _logAzioniService.SalvataggioLogAzioneAsync (utenteId, "eliminazione GiftCard", false );
            return NotFound(new { messaggio = "GiftCard non trovata." });
        }

        await _logAzioniService.SalvataggioLogAzioneAsync (utenteId, "eliminazione GiftCard", true );

        return NoContent();
    }
}

```