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
            dto.Durata = giftCardCorrente.Durata;
            dto.Prezzo = giftCardCorrente.Prezzo;
            dto.NumeroMovie = giftCardCorrente.NumeroMovie;

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

        foreach (var utente in giftCard.Utenti)
        {
            if (utente.Id == utenteId)
            {
                DtoGiftCard risultato = new DtoGiftCard();
                risultato.Id = giftCard.Id;
                risultato.Nome = giftCard.Nome;
                risultato.Durata = giftCard.Durata;
                risultato.Prezzo = giftCard.Prezzo;
                risultato.NumeroMovie = giftCard.NumeroMovie;

                return risultato;
            }
        }

        return null;
    }

    public async Task<DtoGiftCard> CreazioneAsync(DtoCreazioneGiftCard dto)
    {
        GiftCard giftCard = new GiftCard();
        giftCard.Nome = dto.Nome;
        giftCard.Durata = dto.Durata;
        giftCard.Prezzo = dto.Prezzo;
        giftCard.NumeroMovie = dto.NumeroMovie;

        _contesto.GiftCards.Add(giftCard);
        await _contesto.SaveChangesAsync();

        DtoGiftCard risultato = new DtoGiftCard();
        risultato.Id = giftCard.Id;
        risultato.Nome = giftCard.Nome;
        risultato.Durata = giftCard.Durata;
        risultato.Prezzo = giftCard.Prezzo;
        risultato.NumeroMovie = giftCard.NumeroMovie;

        return risultato;
    }

    public async Task<DtoGiftCard?> ModificaAsync(string id, DtoCreazioneGiftCard dto)
    {
        GiftCard? giftCard = await _contesto.GiftCards.FindAsync(id);

        if (giftCard == null)
            return null;


        giftCard.Nome = dto.Nome;
        giftCard.Durata = dto.Durata;
        giftCard.Prezzo = dto.Prezzo;
        giftCard.NumeroMovie = dto.NumeroMovie;


        await _contesto.SaveChangesAsync();

        return new DtoGiftCard
        {
            Id = giftCard.Id,
            Nome = giftCard.Nome,
            Durata = giftCard.Durata,
            Prezzo = giftCard.Prezzo,
            NumeroMovie = giftCard.NumeroMovie,
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

      