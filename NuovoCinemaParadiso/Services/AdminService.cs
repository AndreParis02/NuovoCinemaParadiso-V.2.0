using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NuovoCinemaParadiso.Data;
using NuovoCinemaParadiso.Dtos;
using NuovoCinemaParadiso.Models;
using NuovoCinemaParadiso.Helpers;

namespace NuovoCinemaParadiso.Services;

public class AdminService
{
    private readonly ContestoDb _contesto;
    private readonly UserManager<Utente> _gestioneUtenti;
    public AdminService(ContestoDb contestoDb)
    {
        _contesto = contestoDb;

    }
    
    public async Task<List<DtoUtente>> OttieniUtentiAsync()
    {
        List<Utente> utenti = await _contesto.Utenti.ToListAsync();

        List<DtoUtente> risultato = new List<DtoUtente>();

        for (int i = 0; i < utenti.Count; i++)
        {
            Utente utenteCorrente = utenti[i];

            DtoUtente dto = new DtoUtente();
            dto.Id = utenteCorrente.Id;
            dto.Email = utenteCorrente.Email ?? string.Empty;
            dto.NomeCompleto = utenteCorrente.NomeCompleto ?? string.Empty;
            dto.Eta = utenteCorrente.Eta;

            risultato.Add(dto);
        }

        return risultato;
    }
    public async Task<DtoUtente?> OttieniUtenteTramiteIdAsync(string id)
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

    public async Task<IdentityResult> EliminaUtentePerIdAsync(string id)
    {
        Utente? utente = await _gestioneUtenti.FindByIdAsync(id);
        if (utente == null)
        {
            IdentityError errore = new IdentityError();  //
            return IdentityResult.Failed(errore);  //
        }
        IdentityResult risultato = await _gestioneUtenti.DeleteAsync(utente);

        return risultato;
    }

    public async Task<List<DtoAcquisto>> OttieniAcquisti()
    {
        List<Acquisto> acquisti = await _contesto.Acquisti.ToListAsync();

        List<DtoAcquisto> risultato = new List<DtoAcquisto>();

        for (int i = 0; i < acquisti.Count; i++)
        {
            Acquisto acquistoCorrente = acquisti[i];
            Movie? movie = await _contesto.Movies.FindAsync(acquistoCorrente.MovieId);
            Sala? sala = await _contesto.Sale.FindAsync(acquistoCorrente.SalaId);
            Utente? utente = await _contesto.Utenti.FindAsync(acquistoCorrente.UtenteId);
            TipologiaSala? tipologiaSala = await _contesto.TipologieSala.FindAsync(sala.TipologiaSalaId);

            DtoAcquisto dto = new DtoAcquisto();
            dto.Id = acquistoCorrente.Id;
            dto.MovieId = acquistoCorrente.MovieId;
            dto.Titolo = movie.Titolo;
            dto.SalaId = acquistoCorrente.SalaId;
            dto.Nome = sala.Nome;
            dto.UtenteId = acquistoCorrente.UtenteId;
            dto.NomeCompleto = utente.NomeCompleto;
            dto.PrezzoFinale = CalcolaPrezzo.CalcolaPrezzoFinale(movie.PrezzoMovie, tipologiaSala.MaggiorazionePrezzo, acquistoCorrente.NumeroBiglietti, utente);
            dto.OrarioCreazione = acquistoCorrente.OrarioCreazione;
            dto.NumeroBiglietti = acquistoCorrente.NumeroBiglietti;

            risultato.Add(dto);
        }

        return risultato;
    }

    public async Task<DtoAcquisto> OttieniAcquistoTramiteIdAsync(string id)
    {
        Acquisto? acquisto = await _contesto.Acquisti.FindAsync(id);
        Movie? movie = await _contesto.Movies.FindAsync(acquisto.MovieId);
        Sala? sala = await _contesto.Sale.FindAsync(acquisto.SalaId);
        Utente? utente = await _contesto.Users.FindAsync(acquisto.UtenteId);
        TipologiaSala? tipologiaSala = await _contesto.TipologieSala.FindAsync(sala.TipologiaSalaId);

        if (acquisto == null)
        {
            return null;
        }

        DtoAcquisto dto = new DtoAcquisto();
        dto.Id = acquisto.Id;
        dto.MovieId = acquisto.MovieId;
        dto.Titolo = movie.Titolo;
        dto.SalaId = acquisto.SalaId;
        dto.Nome = sala.Nome;
        dto.UtenteId = acquisto.UtenteId;
        dto.NomeCompleto = utente.NomeCompleto;
        dto.PrezzoFinale = CalcolaPrezzo.CalcolaPrezzoFinale(movie.PrezzoMovie, tipologiaSala.MaggiorazionePrezzo, acquisto.NumeroBiglietti, utente);
        dto.OrarioCreazione = acquisto.OrarioCreazione;
        dto.NumeroBiglietti = acquisto.NumeroBiglietti;

        return dto;
    }

    public async Task<List<DtoUtente>> OttieniTramiteAbbonamentoAsync(string abbonamentoId)
    {

        List<Utente> utenti = await _contesto.Utenti.ToListAsync();
        List<Abbonamento> abbonamenti = await _contesto.Abbonamenti.ToListAsync();

        Abbonamento? abbonamentoTrovato = null;

        for (int i = 0; i < abbonamenti.Count; i++)
        {
            Abbonamento abbonamentoCorrente = abbonamenti[i];

            if (abbonamentoCorrente.Id == abbonamentoId)
            {
                abbonamentoTrovato = abbonamentoCorrente;
                break;
            }
        }

        if (abbonamentoTrovato == null)
        {
            return new List<DtoUtente>();
        }

        List<DtoUtente> risultato = new List<DtoUtente>();

        for (int i = 0; i < utenti.Count; i++)
        {
            Utente utenteCorrente = utenti[i];

            if (utenteCorrente.Abbonamento == abbonamentoTrovato)
            {
                DtoUtente dto = new DtoUtente();
                dto.Id = utenteCorrente.Id;
                dto.NomeCompleto = utenteCorrente.NomeCompleto;
                dto.Email = utenteCorrente.Email;
                dto.Eta = utenteCorrente.Eta;

                risultato.Add(dto);
            }
        }

        return risultato;
    }


}