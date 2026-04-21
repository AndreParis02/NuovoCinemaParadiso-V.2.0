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
    public async Task<IActionResult> OttieniTutteLeGiftCard()
    {
        List<DtoGiftCard> giftCards = await _giftCardService.OttieniTutto();
        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier)?? "";

        await Log (utenteId, "Ottieni tutte le GiftCard", true );

        return Ok(giftCards);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> OttieniTramiteId(string id)
    {
        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier)?? "";

        var risultato = await _giftCardService.OttieniTramiteIdAsync(id, utenteId);

        if (risultato == null)
        {
            await Log (utenteId, "Ottieni GiftCard tramite id", false );
            return NotFound($"GiftCard con id {id} non trovato");
        }

        await Log (utenteId, "Ottieni GiftCard tramite id", true );
        return Ok(risultato);
    }
    

    [HttpPost]
    [Authorize(Roles = Ruoli.GestoreOrOperatore)]
    public async Task<IActionResult> Creazione([FromBody] DtoCreazioneGiftCard dto)
    {
        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier)?? "";
        
        DtoGiftCard? risultato = await _giftCardService.CreazioneAsync(dto);

        await Log (utenteId, "Creazione GiftCard", true );

        return Ok(risultato);
    }


    [HttpPut("{id}")]
    [Authorize(Roles = Ruoli.GestoreOrOperatore)]
    public async Task<IActionResult> Modifica(string id, [FromBody] DtoCreazioneGiftCard dto)
    {
        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier)?? "";

        DtoGiftCard? risultato = await _giftCardService.ModificaAsync(id,dto);

        if (risultato == null)
        {
            await Log (utenteId, "Modifica GiftCard", false );
            return NotFound(new { messaggio = "GiftCard non trovata." });
        }
        await Log (utenteId, "Modifica GiftCard", true );

        return Ok(risultato);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = Ruoli.GestoreOrOperatore)]
    public async Task<IActionResult> Elimina(string id)
    {
        string utenteId = User.FindFirstValue(ClaimTypes.NameIdentifier)?? "";

        bool eliminato = await _giftCardService.EliminazioneAsync(id);

        if (!eliminato)
        {
            await Log (utenteId, "eliminazione GiftCard", false );


            return NotFound(new { messaggio = "GiftCard non trovata." });
        }

        await Log (utenteId, "eliminazione GiftCard", true );

        return NoContent();
    }

    public async Task Log(string utenteId, string azione, bool risultato)
    {
        string messaggio =  "Operazione fallita";
        if (risultato) messaggio =  "Operazione eseguita";
       
        await _logAzioniService.SalvataggioLogAzioneAsync(new DtoCreazioneLogAzioni
        {
            IdUtente = utenteId,
            NomeAzione = azione,
            Effettuato = risultato,
            Messaggio = messaggio
        });
    }
}