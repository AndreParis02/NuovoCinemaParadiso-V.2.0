using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using NuovoCinemaParadiso.Data;
using NuovoCinemaParadiso.Dtos;
using NuovoCinemaParadiso.Models;

namespace NuovoCinemaParadiso.Services;

public class GiftCardService
{
    private readonly ContestoDb _contesto;
    public GiftCardService(ContestoDb contesto)
    {
        _contesto = contesto;
    }

    public async Task<List<DtoGiftCard>> OttieniTutto()
    {
        List<GiftCard> giftCards = await _contesto.GiftCards.ToListAsync();

        return giftCards
            .Select(giftCardCorrente => new DtoGiftCard
            {
                Id = giftCardCorrente.Id,
                Nome = giftCardCorrente.Nome,
                Valore = giftCardCorrente.Valore,
                CodiceRiscatto = giftCardCorrente.CodiceRiscatto,
                UtenteId = giftCardCorrente.UtenteId
            })
            .ToList();
    }

    public async Task<List<DtoGiftCard>> OttieniPerUtenteAsync(string utenteId)
    {
        List<GiftCard> giftCards = await _contesto.GiftCards
            .Where(g => g.UtenteId == utenteId)
            .ToListAsync();

        return giftCards
            .Where(giftCardCorrente => !giftCardCorrente.Riscattata)
            .Select(giftCardCorrente => new DtoGiftCard
            {
                Id = giftCardCorrente.Id,
                Nome = giftCardCorrente.Nome,
                Valore = giftCardCorrente.Valore,
                CodiceRiscatto = giftCardCorrente.CodiceRiscatto,
                UtenteId = giftCardCorrente.UtenteId
            })
            .ToList();
    }

    public async Task<DtoGiftCard?> OttieniTramiteIdAsync(string id, string utenteId)
    {
        GiftCard? giftCard = await _contesto.GiftCards.FindAsync(id);

        if (giftCard == null)
        {
            return null;
        }

        DtoGiftCard risultato = new DtoGiftCard();
        risultato.Id = giftCard.Id;
        risultato.Nome = giftCard.Nome;
        risultato.Valore = giftCard.Valore;
        risultato.CodiceRiscatto = giftCard.CodiceRiscatto;
        risultato.UtenteId = giftCard.UtenteId;

        return risultato;
    }

    public async Task<DtoGiftCard?> ModificaAsync(string id, DtoCreazioneGiftCard dto)
    {
        GiftCard? giftCard = await _contesto.GiftCards.FindAsync(id);

        if (giftCard == null)
            return null;

        giftCard.Nome = dto.Nome;
        giftCard.Valore = dto.Valore;
        giftCard.CodiceRiscatto = dto.CodiceRiscatto;

        await _contesto.SaveChangesAsync();

        return new DtoGiftCard
        {
            Id = giftCard.Id,
            Nome = giftCard.Nome,
            Valore = giftCard.Valore,
            CodiceRiscatto = giftCard.CodiceRiscatto,
        };
    }

    public async Task<bool> EliminazioneAsync(string id)
    {
        GiftCard? giftCard = await _contesto.GiftCards.FindAsync(id);

        if (giftCard == null)
        {
            return false;
        }

        _contesto.GiftCards.Remove(giftCard);
        await _contesto.SaveChangesAsync();

        return true;
    }
}     