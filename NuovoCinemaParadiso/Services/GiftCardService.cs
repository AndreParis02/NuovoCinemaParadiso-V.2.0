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

        List<DtoGiftCard> risultato = new List<DtoGiftCard>();

        for (int i = 0; i < giftCards.Count; i++)
        {
            GiftCard giftCardCorrente = giftCards[i];

            DtoGiftCard dto = new DtoGiftCard();
            dto.Id = giftCardCorrente.Id;
            dto.Nome = giftCardCorrente.Nome;
            dto.Valore = giftCardCorrente.Valore;
            dto.CodiceRiscatto = giftCardCorrente.CodiceRiscatto;

            risultato.Add(dto);
        }

        return risultato;
    }

    public async Task<List<DtoGiftCard>> OttieniPerUtenteAsync(string utenteId)
    {
        List<GiftCard> giftCards = await _contesto.GiftCards
            .Where(g => g.UtenteId == utenteId)
            .ToListAsync();

        List<DtoGiftCard> risultato = new List<DtoGiftCard>();

        foreach (var giftCardCorrente in giftCards)
        {
            DtoGiftCard dto = new DtoGiftCard();
            dto.Id = giftCardCorrente.Id;
            dto.Nome = giftCardCorrente.Nome;
            dto.Valore = giftCardCorrente.Valore;
            dto.CodiceRiscatto = giftCardCorrente.CodiceRiscatto;

            risultato.Add(dto);
        }

        return risultato;
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