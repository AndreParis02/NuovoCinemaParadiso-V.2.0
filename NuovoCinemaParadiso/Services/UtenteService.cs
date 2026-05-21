using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using NuovoCinemaParadiso.Data;
using NuovoCinemaParadiso.Dtos;
using NuovoCinemaParadiso.Models;
using NuovoCinemaParadiso.Exceptions;
using NuovoCinemaParadiso.Helpers;
using System.Linq.Expressions;

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

        if (utenteTrovato.SeAbbonato)
        {
            throw new ItemAlredyexist("Abbonamento");
        }

        if (utenteTrovato.Saldo < abbonamentoTrovato.Prezzo)
        {
            throw new Exception("Impossibile abbonarsi. Credito insufficiente sul saldo.");
        }

        utenteTrovato.Saldo -= abbonamentoTrovato.Prezzo;
        utenteTrovato.AbbonamentoId = abbonamentoTrovato.Id;
        utenteTrovato.SeAbbonato = true;
        utenteTrovato.DataInizioAbbonamento = DateTimeOffset.UtcNow;
        await _contesto.SaveChangesAsync();

        return new DtoUtente()
        {
            Id = utenteTrovato.Id,
            NomeCompleto = utenteTrovato.NomeCompleto,
            Email = utenteTrovato.Email ?? string.Empty,
            Eta = utenteTrovato.Eta,
            SeAbbonato = utenteTrovato.SeAbbonato,
            DataInizioAbbonamento = utenteTrovato.DataInizioAbbonamento,
            AbbonamentoId = utenteTrovato.AbbonamentoId,
            TipoAbbonamento = abbonamentoTrovato.Nome
        };
    }

    public async Task<DtoGiftCard> RicaricaGiftCardAsync(string utenteId, DtoRicaricaGiftCard dto)
    {
        Utente? utenteCorrente = await _gestioneUtenti.FindByIdAsync(utenteId);

        if (utenteCorrente?.Id == null)
        {
            throw new NotFoundException("Utente", utenteId);
        }

        if (dto.Importo <= 0)
        {
            throw new Exception("Impossibile ricaricare la giftcard. Importo non valido.");
        }

        if (utenteCorrente.Saldo < dto.Importo)
        {
            throw new Exception("Impossibile caricare la giftcard. Importo superiore al saldo");
        }

        utenteCorrente.Saldo -= dto.Importo;

        GiftCard? nuovaGiftCard = new GiftCard()
        {
            Nome = "GiftCard",
            Valore = dto.Importo,
            CodiceRiscatto = GiftCardHelper.GeneraCodice()
        };

        await _contesto.GiftCards.AddAsync(nuovaGiftCard);
        await _contesto.SaveChangesAsync();

        return new DtoGiftCard()
        {
            Id = nuovaGiftCard.Id,
            Nome = nuovaGiftCard.Nome,
            Valore = nuovaGiftCard.Valore,
            CodiceRiscatto = nuovaGiftCard.CodiceRiscatto
        };
    }

    public async Task<DtoGiftCard> RiscattaGiftCardAsync(DtoCodiceRiscatto dto, string utenteId)
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

            if (giftCardCorrente.CodiceRiscatto == dto.CodiceRiscatto)
            {
                giftCardTrovata = giftCardCorrente;
                break;
            }
        }

        if (giftCardTrovata == null)
        {
            throw new Exception("Codice riscatto non disponibile.");
        }

        if(giftCardTrovata.CodiceRiscatto != dto.CodiceRiscatto)

        if (giftCardTrovata.Riscattata == true)
        {
            throw new Exception("Il codice riscatto è già stato utilizzato.");
        }

        utenteCorrente.Saldo += giftCardTrovata.Valore;
        giftCardTrovata.Riscattata = true;

        await _contesto.SaveChangesAsync();

        return new DtoGiftCard()
        {
            Id = giftCardTrovata.Id,
            Nome = giftCardTrovata.Nome,
            Valore = giftCardTrovata.Valore,
            CodiceRiscatto = giftCardTrovata.CodiceRiscatto
        };
    }
}