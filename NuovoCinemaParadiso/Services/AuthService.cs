using Microsoft.AspNetCore.Identity;
using NuovoCinemaParadiso.Models;
using NuovoCinemaParadiso.Dtos;
using NuovoCinemaParadiso.Helpers;
using NuovoCinemaParadiso.Data;
using Microsoft.EntityFrameworkCore;

namespace NuovoCinemaParadiso.Services;

public class AuthService
{
    private readonly UserManager<Utente> _gestioneUtenti;
    private readonly SignInManager<Utente> _gestioneAccesso;
    private readonly JwtHelper _jwtHelper;
    private readonly ContestoDb _contesto;

    public AuthService(UserManager<Utente> gestioneUtenti, SignInManager<Utente> gestioneAccesso, JwtHelper jwtHelper, ContestoDb contesto)
    {
        _gestioneUtenti = gestioneUtenti;
        _gestioneAccesso = gestioneAccesso;
        _jwtHelper = jwtHelper;
        _contesto = contesto;
    }

    public async Task<IdentityResult> RegistrazioneAsync(DtoRegistrazione dto)
    {
        Utente? esisteUtente = await _gestioneUtenti.FindByEmailAsync(dto.Email);

        if (esisteUtente != null)
        {
            IdentityError errore = new IdentityError();
            errore.Description = "Utente già registrato.";

            List<IdentityError> errori = new List<IdentityError>();
            errori.Add(errore);

            return IdentityResult.Failed(errori.ToArray());
        }

        Utente utente = new Utente();
        utente.UserName = dto.Email;
        utente.Email = dto.Email;
        utente.NomeCompleto = dto.NomeCompleto;
        utente.Eta = dto.Eta;

        IdentityResult risultato = await _gestioneUtenti.CreateAsync(utente, dto.Password);

        if (!risultato.Succeeded)
        {
            return risultato;
        }
        IdentityResult aggiuntaRisultatoRuolo = await _gestioneUtenti.AddToRoleAsync(utente, Ruoli.Utente);

        if (!aggiuntaRisultatoRuolo.Succeeded)
            return aggiuntaRisultatoRuolo;

        return risultato;
    }

    public async Task<DtoAuthResponse?> LoginAsync(DtoLogin dto)
    {
        Utente? utente = await _gestioneUtenti.FindByEmailAsync(dto.Email);
        
        if (utente == null)
        {
            return null;
        }

        if (utente.SeAbbonato == true)
        {
            DateTimeOffset? scadenza = Calcoli.CalcolaScadenzaAbbonamento(utente.DataInizioAbbonamento, utente.Abbonamento.Durata);
            int giorniMancanti = Calcoli.GiorniAllaScadenza(utente.DataInizioAbbonamento, utente.Abbonamento.Durata);

            if (giorniMancanti == 0)
            {
                utente.SeAbbonato = false;
            }
        }
        
        SignInResult result = await _gestioneAccesso.CheckPasswordSignInAsync(utente, dto.Password, false);

        if (!result.Succeeded)
        {
            return null;
        }
        IList<string> ruoli = await _gestioneUtenti.GetRolesAsync(utente);

        string token = _jwtHelper.GenerateToken(utente, ruoli);

        DtoAuthResponse response = new DtoAuthResponse();
        response.Token = token;
        response.Id = utente.Id;
        response.NomeCompleto = utente.NomeCompleto;
        response.Email = utente.Email ?? string.Empty;

        if (ruoli.Count > 0)
        {
            response.Ruolo = ruoli[0];
        }
        else
        {
            response.Ruolo = "";
        }

        return response;
    }

    public async Task<DtoUtente?> OttieniTramiteIdAsync(string id)
    {
        Utente? utente = await _gestioneUtenti.FindByIdAsync(id);

        if (utente == null)
        {
            return null;
        }

        DtoUtente dto = new DtoUtente();
        dto.Id = utente.Id;
        dto.Email = utente.Email ?? string.Empty;
        dto.NomeCompleto = utente.NomeCompleto ?? string.Empty;
        dto.Eta = utente.Eta;

        return dto;
    }

    public async Task<IdentityResult> ModificaAsync(DtoCreazioneUtente dto, string idUtente)
    {
        Utente? utente = await _gestioneUtenti.FindByIdAsync(idUtente);

        if (utente == null)
        {
            IdentityError error = new IdentityError();
            return IdentityResult.Failed(error);
        }

        utente.NomeCompleto = dto.NomeCompleto;
        utente.Eta = dto.Eta;
        IdentityResult result = await _gestioneUtenti.UpdateAsync(utente);

        return result;
    }

    public async Task<IdentityResult> EliminaAsync(string userId)
    {
        Utente? utente = await _gestioneUtenti.FindByIdAsync(userId);

        if (utente == null)
        {
            IdentityError errore = new IdentityError();
            return IdentityResult.Failed(errore);
        }

        IdentityResult risultato = await _gestioneUtenti.DeleteAsync(utente);

        return risultato;
    }
}