using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NuovoCinemaParadiso.Data;
using NuovoCinemaParadiso.Dtos;
using NuovoCinemaParadiso.Models;
using NuovoCinemaParadiso.Helpers;
using NuovoCinemaParadiso.Exceptions;


namespace NuovoCinemaParadiso.Services;

public class OperatoreService
{
    private readonly ContestoDb _contesto;
    private readonly UserManager<Utente> _gestioneUtenti;
    public OperatoreService(ContestoDb contestoDb, UserManager<Utente> gestioneUtenti)
    {
        _contesto = contestoDb;
        _gestioneUtenti = gestioneUtenti;

    }

    public async Task<bool> RicaricaAsync(DtoRicarica dtoRicarica)
    {
        Utente? utente = await _gestioneUtenti.FindByEmailAsync(dtoRicarica.Email);
        if(utente==null)
            return false;
        utente.Saldo += dtoRicarica.Ricarica;
        await _gestioneUtenti.UpdateAsync(utente);
        return true;
    }

}