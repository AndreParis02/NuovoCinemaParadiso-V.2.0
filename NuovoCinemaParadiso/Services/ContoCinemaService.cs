using Microsoft.EntityFrameworkCore;
using NuovoCinemaParadiso.Dtos;
using NuovoCinemaParadiso.Data;
using NuovoCinemaParadiso.Models;
using NuovoCinemaParadiso.Exceptions;
using NuovoCinemaParadiso.Controllers;

namespace NuovoCinemaParadiso.Services;

public class ContoCinemaService
{
    private readonly ContestoDb _contesto;
    public ContoCinemaService(ContestoDb contesto)
    {
        _contesto = contesto;
    }
    public async Task<DtoContoCinema> OttieniDatiContoAsync()
    {

        ContoCinema contoCinema = await _contesto.ContoCinema.FirstOrDefaultAsync()
            ?? throw new NotFoundException("Conto Cinema", "");

        DtoContoCinema dto = new DtoContoCinema();

        dto.Id = contoCinema.Id;
        dto.Iban = contoCinema.Iban;
        dto.TitolareConto = contoCinema.TitolareConto;
        dto.Conto = contoCinema.Conto;

        return dto;
    }
}