using Microsoft.EntityFrameworkCore;
using NuovoCinemaParadiso.Data;
using NuovoCinemaParadiso.Dtos;
using NuovoCinemaParadiso.Models;
using NuovoCinemaParadiso.Helpers;

namespace NuovoCinemaParadiso.Services;

public class BigliettoService
{
    private readonly ContestoDb _contesto;
    public BigliettoService(ContestoDb contesto)
    {
        _contesto = contesto;
    }

 public async Task<List<DtoBiglietto>> OttieniTutto()
    {
        List<Biglietto> biglietti = await _contesto.Biglietti.ToListAsync();

        List<DtoBiglietto> risultato = new List<DtoBiglietto>();

        for (int i = 0; i < biglietti.Count; i++)
        {
            if (bigliettoCorrente.UtenteId == utenteId)
            {
                // Recupero entità per i calcoli
                var utente = await _contesto.Utenti.FindAsync(bigliettoCorrente.UtenteId);
                utente.Abbonamento = await _contesto.Abbonamenti.FindAsync(utente.AbbonamentoId);

                var proiezione = await _contesto.Proiezioni.FindAsync(bigliettoCorrente.ProiezioneId);

                if (utente == null || proiezione == null) continue; // Salta se mancano dati integri

                var movie = await _contesto.Movies.FindAsync(proiezione.MovieId);
                var sala = await _contesto.Sale.FindAsync(proiezione.SalaId);

                if (movie == null || sala == null) continue;

                var tipologiaSala = await _contesto.TipologieSala.FindAsync(sala.TipologiaSalaId);
                if (tipologiaSala == null) continue;

                DtoBiglietto dto = new DtoBiglietto
                {
                    Id = bigliettoCorrente.Id,
                    UtenteId = utente.Id,
                    ProiezioneId = bigliettoCorrente.ProiezioneId,
                    OrarioCreazione = bigliettoCorrente.OrarioCreazione,
                    NumeroBiglietti = bigliettoCorrente.NumeroBiglietti,
                    MetodoPagamento = bigliettoCorrente.MetodoPagamento,
                    PrezzoFinale = Calcoli.CalcolaPrezzoFinale(
                        movie.PrezzoMovie,
                        tipologiaSala.MaggiorazionePrezzo,
                        bigliettoCorrente.NumeroBiglietti,
                        utente.Abbonamento,
                        utente.DataInizioAbbonamento)
                };
                risultato.Add(dto);
            }
        }
        return risultato;
    }



    public async Task<(DtoBiglietto? Dto, string? Errore)> OttieniTramiteIdAsync(string id, string utenteId)
    {
        Biglietto? biglietto = await _contesto.Biglietti.FindAsync(id);
        if (biglietto == null) return (null, "Biglietto non trovato.");

        if (biglietto.UtenteId != utenteId)
            return (null, "Accesso negato: questo biglietto non ti appartiene.");


        var utente = await _contesto.Utenti.FindAsync(biglietto.UtenteId);
        utente.Abbonamento = await _contesto.Abbonamenti.FindAsync(utente.AbbonamentoId);

        var proiezione = await _contesto.Proiezioni.FindAsync(biglietto.ProiezioneId);
        if (utente == null || proiezione == null) return (null, "Dati della proiezione o utente non trovati.");

        var movie = await _contesto.Movies.FindAsync(proiezione.MovieId);
        var sala = await _contesto.Sale.FindAsync(proiezione.SalaId);
        if (movie == null || sala == null) return (null, "Dati del film o sala non trovati.");

        var tipologiaSala = await _contesto.TipologieSala.FindAsync(sala.TipologiaSalaId);
        if (tipologiaSala == null) return (null, "Tipologia sala non trovata.");

        DtoBiglietto dto = new DtoBiglietto
        {
            Id = biglietto.Id,
            UtenteId = utente.Id,
            ProiezioneId = biglietto.ProiezioneId,
            OrarioCreazione = biglietto.OrarioCreazione,
            NumeroBiglietti = biglietto.NumeroBiglietti,
            MetodoPagamento = biglietto.MetodoPagamento,
            PrezzoFinale = Calcoli.CalcolaPrezzoFinale(movie.PrezzoMovie, tipologiaSala.MaggiorazionePrezzo, biglietto.NumeroBiglietti, utente.Abbonamento, utente.DataInizioAbbonamento)
        };

        return (dto, null);
    }

    public async Task<(DtoBiglietto? Dto, string? Errore)> CreazioneAsync(DtoCreazioneBiglietto dto, string utenteId)
    {

        if (dto.NumeroBiglietti <= 0 || dto.NumeroBiglietti > 100)
            return (null, "Il numero di biglietti deve essere compreso tra 1 e 100.");

        var utente = await _contesto.Utenti.FindAsync(utenteId);
        if (utente == null) return (null, "Utente non trovato.");

        utente.Abbonamento= await _contesto.Abbonamenti.FindAsync(utente.AbbonamentoId);
        if (utente.Abbonamento == null) return (null, "Abbonamento non trovato.");

        var proiezione = await _contesto.Proiezioni.FindAsync(dto.ProiezioneId);
        if (proiezione == null) return (null, "Proiezione non trovata.");

        var movie = await _contesto.Movies.FindAsync(proiezione.MovieId);
        var sala = await _contesto.Sale.FindAsync(proiezione.SalaId);
        if (movie == null || sala == null) return (null, "Dati del film o della sala non validi.");

        var tipologiaSala = await _contesto.TipologieSala.FindAsync(sala.TipologiaSalaId);
        if (tipologiaSala == null) return (null, "Tipologia sala non trovata.");

        var contoCinema = await _contesto.ContoCinema.FirstOrDefaultAsync();
        if (contoCinema == null) return (null, "Dati conto non disponibili");

        Biglietto biglietto = new Biglietto
        {
            UtenteId = utenteId,
            ProiezioneId = proiezione.Id,
            NumeroBiglietti = dto.NumeroBiglietti,
            OrarioCreazione = DateTimeOffset.UtcNow,
            MetodoPagamento = dto.MetodoPagamento,
            PrezzoFinale = Calcoli.CalcolaPrezzoFinale(movie.PrezzoMovie, tipologiaSala.MaggiorazionePrezzo, dto.NumeroBiglietti, utente.Abbonamento, utente.DataInizioAbbonamento)
        };

        _contesto.Biglietti.Add(biglietto);
        Calcoli.CalcolaSaldo(biglietto.PrezzoFinale, utente, contoCinema);
        await _contesto.SaveChangesAsync();



        DtoBiglietto risultato = new DtoBiglietto
        {
            Id = biglietto.Id,
            ProiezioneId = biglietto.ProiezioneId,
            UtenteId = biglietto.UtenteId,
            NumeroBiglietti = biglietto.NumeroBiglietti,
            PrezzoFinale = biglietto.PrezzoFinale,
            OrarioCreazione = biglietto.OrarioCreazione,
            MetodoPagamento = biglietto.MetodoPagamento
        };

        return (risultato, null);
    }

    public async Task<(DtoBiglietto? Dto, string? Errore)> ModificaAsync(string id, DtoCreazioneBiglietto dto)
    {
        if (dto.NumeroBiglietti <= 0 || dto.NumeroBiglietti > 100)
            return (null, "Il numero di biglietti deve essere compreso tra 1 e 100.");

        var bigliettoEsistente = await _contesto.Biglietti.FindAsync(id);
        if (bigliettoEsistente == null) return (null, "Biglietto non trovato.");

        var proiezione = await _contesto.Proiezioni.FindAsync(dto.ProiezioneId);
        if (proiezione == null) return (null, "Proiezione non trovata.");

        var movie = await _contesto.Movies.FindAsync(proiezione.MovieId);
        var sala = await _contesto.Sale.FindAsync(proiezione.SalaId);
        var utente = await _contesto.Utenti.FindAsync(bigliettoEsistente.UtenteId);
        utente.Abbonamento = await _contesto.Abbonamenti.FindAsync(utente.AbbonamentoId);

        var tipologiaSala = await _contesto.TipologieSala.FindAsync(sala?.TipologiaSalaId);

        if (movie == null || sala == null || utente == null || tipologiaSala == null)
            return (null, "Dati correlati all'biglietto non trovati o non validi.");

        bigliettoEsistente.NumeroBiglietti = dto.NumeroBiglietti;
        bigliettoEsistente.PrezzoFinale = Calcoli.CalcolaPrezzoFinale(movie.PrezzoMovie, tipologiaSala.MaggiorazionePrezzo, bigliettoEsistente.NumeroBiglietti, utente.Abbonamento, utente.DataInizioAbbonamento);

        await _contesto.SaveChangesAsync();

        return (new DtoBiglietto
        {
            Id = bigliettoEsistente.Id,
            UtenteId = bigliettoEsistente.UtenteId,
            ProiezioneId = bigliettoEsistente.ProiezioneId,
            NumeroBiglietti = bigliettoEsistente.NumeroBiglietti,
            PrezzoFinale = bigliettoEsistente.PrezzoFinale,
            OrarioCreazione = bigliettoEsistente.OrarioCreazione,
            MetodoPagamento = bigliettoEsistente.MetodoPagamento
        }, null);
    }

    public async Task<(bool Successo, string? Errore)> EliminazioneAsync(string id)
    {
        var biglietto = await _contesto.Biglietti.FindAsync(id);
        if (biglietto == null) return (false, "Biglietto non trovato.");

        _contesto.Biglietti.Remove(biglietto);
        await _contesto.SaveChangesAsync();
        return (true, null);
    }
}