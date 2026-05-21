using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NuovoCinemaParadiso.Services;
using NuovoCinemaParadiso.Dtos;

namespace NuovoCinemaParadiso.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class GestoreController : ControllerBase
{
    private readonly GestoreService _gestoreService;

    public GestoreController(GestoreService gestoreService, LogAzioniService logAzioniService)
    {
        _gestoreService = gestoreService;
    }


    [HttpGet("logs")]
    [Authorize(Roles = Ruoli.Gestore)]
    public async Task<IActionResult> OttieniLogAzioni()
    {
        List<DtoLogAzioni> risultatiLog = await _gestoreService.LetturaLogAzioneAsync();
        return Ok(risultatiLog);
    }
}