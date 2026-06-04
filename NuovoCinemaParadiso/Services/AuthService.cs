using Microsoft.AspNetCore.Identity;
using NuovoCinemaParadiso.Models;
using NuovoCinemaParadiso.Dtos;
using NuovoCinemaParadiso.Helpers;
using NuovoCinemaParadiso.Data;
using NuovoCinemaParadiso.Exceptions;

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

    public async Task<(bool successo, string? Errore)> RegistrazioneAsync(DtoRegistrazione dto)
    {
        Utente? esisteUtente = await _gestioneUtenti.FindByEmailAsync(dto.Email);

        if (esisteUtente != null)
        {
            IdentityError errore = new IdentityError();
            errore.Description = "Utente già registrato.";

            List<IdentityError> errori = new List<IdentityError>();
            errori.Add(errore);

            return (false, "Utente già registrato.");
        }

        if (!dto.Email.Contains('.'))
        {
            throw new InvalidEmail(dto.Email);
        }

        Utente utente = new Utente();
        utente.UserName = dto.Email;
        utente.Email = dto.Email;
        utente.NomeCompleto = dto.NomeCompleto;
        utente.Eta = dto.Eta;
        utente.Saldo = 1000;

        IdentityResult risultato = await _gestioneUtenti.CreateAsync(utente, dto.Password);

        if (!risultato.Succeeded)
        {
            return (false, "Errore durante la registrazione.");
        }
        await _gestioneUtenti.AddToRoleAsync(utente, Ruoli.Utente);
        //ritorna true perché la registrazione ha avuto successo
        return (true,null);
    }

    public async Task<DtoAuthResponse?> LoginAsync(DtoLogin dto)
    {
        Utente? utente = await _gestioneUtenti.FindByEmailAsync(dto.Email);

        if (utente == null)
        {
            throw new NotFoundException("Utente", dto.Email);
        }

        Abbonamento? abbonamento = null;
        if (!string.IsNullOrEmpty(utente.AbbonamentoId))
        {
            abbonamento = await _contesto.Abbonamenti.FindAsync(utente.AbbonamentoId);
        }

        if (utente.SeAbbonato == true && abbonamento != null)
        {
            DateTimeOffset? scadenzaAbbonamento = Calcoli.CalcolaScadenza(utente.DataInizioAbbonamento, abbonamento.Durata);
            int giorniMancanti = Calcoli.GiorniAllaScadenza(utente.DataInizioAbbonamento, abbonamento.Durata);

            if (giorniMancanti <= 0)
            {
                utente.SeAbbonato = false;
            }
        }

        SignInResult result = await _gestioneAccesso.CheckPasswordSignInAsync(utente, dto.Password, false);

        if (!result.Succeeded)
        {
            throw new ConflictException("Password errata");
        }

        IList<string> ruoli = await _gestioneUtenti.GetRolesAsync(utente);

        string token = _jwtHelper.GenerateToken(utente, ruoli);

        DtoAuthResponse response = new DtoAuthResponse();
        response.Token = token;
        response.Id = utente.Id;
        response.NomeCompleto = utente.NomeCompleto;
        response.Email = utente.Email ?? string.Empty;
        response.Eta = utente.Eta;
        response.DataInizioAbbonamento = utente.DataInizioAbbonamento;
        response.SeAbbonato = utente.SeAbbonato;
        response.Saldo = utente.Saldo;

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
        Utente? utente = await _gestioneUtenti.FindByIdAsync(id)
            ?? throw new NotFoundException("Utente", id);

        Abbonamento? abbonamento = await _contesto.Abbonamenti.FindAsync(utente.AbbonamentoId);

        DtoUtente dto = new DtoUtente();
        dto.Id = utente.Id;
        dto.Email = utente.Email ?? string.Empty;
        dto.NomeCompleto = utente.NomeCompleto ?? string.Empty;
        dto.Eta = utente.Eta;
        dto.SeAbbonato = utente.SeAbbonato;
        dto.AbbonamentoId = utente?.AbbonamentoId ?? string.Empty;
        dto.TipoAbbonamento = abbonamento?.Nome ?? string.Empty;
        dto.DataInizioAbbonamento = utente.DataInizioAbbonamento;
        dto.Saldo = utente.Saldo;

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