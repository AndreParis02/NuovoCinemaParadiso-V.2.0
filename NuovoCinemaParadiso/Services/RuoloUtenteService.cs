using Microsoft.AspNetCore.Identity;
using NuovoCinemaParadiso.Dtos;
using NuovoCinemaParadiso.Models;

namespace NuovoCinemaParadiso.Services;

public class RuoloUtenteService
{
    private readonly UserManager<Utente> _gestioneUtenti;
    public RuoloUtenteService(UserManager<Utente> gestioneUtenti)
    {
        _gestioneUtenti = gestioneUtenti; // dependency injection per l'utilizzo di UserManager
    }

    public async Task<string?> ModificaRuoloUtente(DtoModificaRuoloUtente dto)
    {
        //controllo base sul nome ruolo
        if(dto.NuovoRuolo != Ruoli.Gestore && dto.NuovoRuolo != Ruoli.Operatore && dto.NuovoRuolo != Ruoli.Utente)
        {
            return null;
        }

        Utente? utente = await _gestioneUtenti.FindByEmailAsync(dto.Email);
        if(utente == null)
        {
            return null;
        }
        IList<string> ruoloCorrente = await _gestioneUtenti.GetRolesAsync(utente);

        // rimuoviamo i ruoli classici già presenti
        for(int i = 0; i < ruoloCorrente.Count; i++)
        {
            string currentRole = ruoloCorrente[i];
            if(currentRole == Ruoli.Gestore || currentRole == Ruoli.Operatore || currentRole == Ruoli.Utente)
             {
                await _gestioneUtenti.RemoveFromRoleAsync(utente, currentRole);
             }
        }

        // assegniamo il nuovo ruolo

        IdentityResult addResult = await _gestioneUtenti.AddToRoleAsync(utente, dto.NuovoRuolo);
        if(!addResult.Succeeded)
        {
            return null;
        }

        return dto.NuovoRuolo;
    }
}