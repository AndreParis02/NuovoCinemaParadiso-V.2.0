using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using NuovoCinemaParadiso.Data;
using NuovoCinemaParadiso.Dtos;
using NuovoCinemaParadiso.Models;
using NuovoCinemaParadiso.Exceptions;
using NuovoCinemaParadiso.Helpers;

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
            throw new NotFoundException("Abbonamento", abbonamentoId);
        }

        Utente? utenteTrovato = await _gestioneUtenti.FindByIdAsync(utenteId);
        if (utenteTrovato == null)
        {
            throw new NotFoundException("Utente", utenteId);

        }   

        if(utenteTrovato.SeAbbonato)
        {
            throw new ItemAlredyexist("Abbonamento");
        }

        utenteTrovato.AbbonamentoId = abbonamentoTrovato.Id;
        utenteTrovato.SeAbbonato = true;
        utenteTrovato.DataInizioAbbonamento = DateTimeOffset.UtcNow;
        await _contesto.SaveChangesAsync();

        return new DtoUtente()
        {
            Id = utenteTrovato.Id,
            NomeCompleto = utenteTrovato.NomeCompleto,
            Email = utenteTrovato.Email,
            Eta = utenteTrovato.Eta,
            SeAbbonato = utenteTrovato.SeAbbonato,
            DataInizioAbbonamento = utenteTrovato.DataInizioAbbonamento,
            AbbonamentoId = utenteTrovato.AbbonamentoId,
            TipoAbbonamento = abbonamentoTrovato.Nome
        };
    }


    public async Task<DtoGiftCard> RicaricaGiftCardAsync(string giftCardId, string utenteId)
    {
        List<GiftCard> giftCards = await _contesto.GiftCards.ToListAsync();
        GiftCard? giftCardTrovata = null;

        Utente? utenteCorrente = await _gestioneUtenti.FindByIdAsync(utenteId);

        if (utenteCorrente?.Id == null)
        {
            throw new NotFoundException("Utente", utenteId);
        }

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
            throw new NotFoundException("GiftCard", giftCardId);
        }

        giftCardTrovata.Saldo = CalcoliHelper.CaricaGiftCard();
        giftCardTrovata.CodiceRiscatto = GiftCardHelper.GeneraCodice();

        await _contesto.SaveChangesAsync();

        return new DtoGiftCard()
        {
            Id = giftCardTrovata.Id,
            Nome = giftCardTrovata.Nome,
            Saldo = giftCardTrovata.Saldo,
            CodiceRiscatto = giftCardTrovata.CodiceRiscatto
        };
    
    }

    public async Task<DtoGiftCard> RiscattaGiftCardAsync(string giftCardId, string utenteId)
    {
        List<GiftCard> giftCards = await _contesto.GiftCards.ToListAsync();
        GiftCard? giftCardTrovata = null;

        Utente? utenteCorrente = await _gestioneUtenti.FindByIdAsync(utenteId);

        if (utenteCorrente?.Id == null)
        {
            throw new NotFoundException("Utente", utenteId);
        }

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
            throw new NotFoundException("GiftCard", giftCardId);
        }
        
        utenteCorrente.Saldo += giftCardTrovata.Saldo;
        giftCardTrovata.Saldo = 0;
        giftCardTrovata.Riscattata = true;

        await _contesto.SaveChangesAsync();

        return new DtoGiftCard()
        {
            Id = giftCardTrovata.Id,
            Nome = giftCardTrovata.Nome,
            Saldo = giftCardTrovata.Saldo,
            CodiceRiscatto = giftCardTrovata.CodiceRiscatto
        };
    
    }
}