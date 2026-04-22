using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using NuovoCinemaParadiso.Data;
using NuovoCinemaParadiso.Dtos;
using NuovoCinemaParadiso.Models;

namespace NuovoCinemaParadiso.Services;

public class UtenteService
{
    private readonly ContestoDb _contesto;
    private readonly UserManager<Utente> _gestioneUtenti;

    public UtenteService(ContestoDb contestoDb, UserManager<Utente> gestioneUtenti)
    {
        _contesto = contestoDb;
        _gestioneUtenti = gestioneUtenti;
    }

    public async Task<DtoUtente> AbbonatiAsync(string abbonamentoId, string utenteId)
    {
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
            return null;
        }

        Utente? utenteTrovato = await _gestioneUtenti.FindByIdAsync(utenteId);

        if (utenteTrovato == null)
        {
            return null;
        }

        utenteTrovato.AbbonamentoId = abbonamentoTrovato.Id;
        utenteTrovato.SeAbbonato = true;
        utenteTrovato.DataInizioAbbonamento = DateTimeOffset.UtcNow;
        await _contesto.SaveChangesAsync();

        GiftCard giftCard = await _contesto.GiftCards.FindAsync(utenteTrovato.GiftCardId);
        return new DtoUtente()
        {
            Id = utenteTrovato.Id,
            NomeCompleto = utenteTrovato.NomeCompleto,
            Email = utenteTrovato.Email,
            Eta = utenteTrovato.Eta,
            SeAbbonato = utenteTrovato.SeAbbonato,
            DataInizioAbbonamento = utenteTrovato.DataInizioAbbonamento,
            AbbonamentoId = utenteTrovato.AbbonamentoId,
            GiftCardId = utenteTrovato.GiftCardId,
            TipoGiftCard = giftCard?.Nome ?? string.Empty,
            TipoAbbonamento = abbonamentoTrovato.Nome
        };
    }
    public async Task<DtoUtente> GiftCardAsync(string giftCardId, string utenteId)
    {
        List<GiftCard> giftCards = await _contesto.GiftCards.ToListAsync();
        GiftCard? giftCardTrovata = null;

        for (int i = 0; i < giftCards.Count; i++)
        {
            GiftCard giftCardCorrente = giftCards[i];

            if (giftCardCorrente.Id == giftCardId)
            {
                giftCardTrovata = giftCardCorrente;
                break;
            }
        }

        if (giftCardTrovata == null)
        {
            return null;
        }

        Utente? utenteTrovato = await _gestioneUtenti.FindByIdAsync(utenteId);
        Abbonamento abbonamento = await _contesto.Abbonamenti.FindAsync(utenteTrovato.AbbonamentoId);

        if (utenteTrovato == null)
        {
            return null;
        }

        utenteTrovato.GiftCardId = giftCardTrovata.Id;
        utenteTrovato.PossiedeGiftCard = true;
        utenteTrovato.DataInizioGiftCard = DateTimeOffset.UtcNow;
        await _contesto.SaveChangesAsync();

        return new DtoUtente()
        {
            Id = utenteTrovato.Id,
            NomeCompleto = utenteTrovato.NomeCompleto,
            Email = utenteTrovato.Email,
            Eta = utenteTrovato.Eta,
            PossiedeGiftCard = utenteTrovato.PossiedeGiftCard,
            DataInizioAbbonamento = utenteTrovato.DataInizioAbbonamento,
            DataInizioGiftCard = utenteTrovato.DataInizioGiftCard,
            AbbonamentoId = utenteTrovato.AbbonamentoId,
            GiftCardId = utenteTrovato.GiftCardId,
            TipoGiftCard = giftCardTrovata.Nome,
            TipoAbbonamento = abbonamento?.Nome = string.Empty
        };
    }
}