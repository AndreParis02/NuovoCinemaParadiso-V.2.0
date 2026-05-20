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

    public async Task<DtoContoCinema?> CreazioneAsync(DtoCreazioneContoCinema dto)
    {
        ContoCinema risutato = await _contesto.ContoCinema.FirstOrDefaultAsync()
            ?? throw new NotFoundException("Conto Cinema", "");


        if(risutato != null)
        {
            return null;
        }
        ContoCinema contoCinema = new ContoCinema();
        contoCinema.Iban = dto.Iban;
        contoCinema.TitolareConto = dto.TitolareConto;
        contoCinema.Conto = dto.Conto;

        _contesto.ContoCinema.Add(contoCinema);
        await _contesto.SaveChangesAsync();

        DtoContoCinema risultato = new DtoContoCinema();
        risultato.Id = contoCinema.Id;
        risultato.Iban = contoCinema.Iban;
        risultato.TitolareConto = contoCinema.TitolareConto;
        risultato.Conto = contoCinema.Conto;

        return risultato;
    } 

    public async Task<DtoContoCinema?> ModificaAsync(string id, DtoCreazioneContoCinema dto)
    {
        ContoCinema? contoCinema = await _contesto.ContoCinema.FindAsync(id);

        if (contoCinema == null)
        {
            throw new ItemNotFoundException("Dati conto");
        }

        contoCinema.Iban = dto.Iban;
        contoCinema.TitolareConto = dto.TitolareConto;

        await _contesto.SaveChangesAsync();

        DtoContoCinema risultato = new DtoContoCinema();
        risultato.Id = contoCinema.Id;
        risultato.Iban = contoCinema.Iban;
        risultato.TitolareConto = contoCinema.TitolareConto;
        risultato.Conto = contoCinema.Conto;

        return risultato;
    }

    public async Task<bool> EliminaAsync(string id)
    {
        ContoCinema? contoCinema = await _contesto.ContoCinema.FindAsync(id);

        if (contoCinema == null)
        {
            return false;
        }

        _contesto.ContoCinema.Remove(contoCinema);
        await _contesto.SaveChangesAsync();

        return true;
    }
}