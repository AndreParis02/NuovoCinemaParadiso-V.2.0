using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using NuovoCinemaParadiso.Services;
using NuovoCinemaParadiso.Dtos;

namespace NuovoCinemaParadiso.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class GestoreController : ControllerBase
{
    private readonly GestoreService _gestoreService;
    private readonly LogAzioniService _logAzioniService;

    public GestoreController(GestoreService gestoreService, LogAzioniService logAzioniService)
    {
        _gestoreService = gestoreService;
        _logAzioniService = logAzioniService;
    }


    [HttpGet("log")]
    [Authorize(Roles = Ruoli.Gestore)]
    public async Task<IActionResult> OttieniLogAzioni()
    {
        List<DtoLogAzioni> risultatiLog = await _logAzioniService.LetturaLogAzioneAsync();
        return Ok(risultatiLog);
    }
}