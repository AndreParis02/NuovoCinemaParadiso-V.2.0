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
            Biglietto bigliettoCorrente = biglietti[i];
            GenereMovie? genereMovie = await _contesto.GeneriMovies.FindAsync(bigliettoCorrente);
            Proiezione? proiezione = await _contesto.Proiezioni.FindAsync(bigliettoCorrente.ProiezioneId);
            Movie? movie = await _contesto.Movies.FindAsync(proiezione.MovieId);
            Sala? sala = await _contesto.Sale.FindAsync(proiezione.SalaId);
            TipologiaSala? tipologiaSala = await _contesto.TipologieSala.FindAsync(sala.TipologiaSalaId);
            Utente? utente = await _contesto.Utenti.FindAsync(bigliettoCorrente.UtenteId);
            

            DtoBiglietto dto = new DtoBiglietto();
            dto.Id = bigliettoCorrente.Id;
            dto.UtenteId = bigliettoCorrente.UtenteId;
            dto.ProiezioneId = bigliettoCorrente.ProiezioneId;
            dto.OrarioCreazione = bigliettoCorrente.OrarioCreazione;
            dto.NumeroBiglietti = bigliettoCorrente.NumeroBiglietti;
            dto.PrezzoFinale = Calcoli.CalcolaPrezzoFinale(movie.PrezzoMovie, tipologiaSala.MaggiorazionePrezzo, bigliettoCorrente.NumeroBiglietti, utente.Abbonamento  );

            risultato.Add(dto);
        }
        return risultato;
    } 



    public async Task<(DtoBiglietto? Dto, string? Errore)> CreazioneAsync(DtoCreazioneBiglietto dto, string utenteId)
    {

        if (dto.NumeroBiglietti <= 0 || dto.NumeroBiglietti > 100)
            return (null, "Il numero di biglietti deve essere compreso tra 1 e 100.");

        var utente = await _contesto.Utenti.FindAsync(utenteId);
        if (utente == null) return (null, "Utente non trovato.");

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
            PrezzoFinale = Calcoli.CalcolaPrezzoFinale(movie.PrezzoMovie, tipologiaSala.MaggiorazionePrezzo, dto.NumeroBiglietti, utente, dto.MetodoPagamento)
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
        var tipologiaSala = await _contesto.TipologieSala.FindAsync(sala?.TipologiaSalaId);

        if (movie == null || sala == null || utente == null || tipologiaSala == null)
            return (null, "Dati correlati all'biglietto non trovati o non validi.");

        bigliettoEsistente.NumeroBiglietti = dto.NumeroBiglietti;
        bigliettoEsistente.PrezzoFinale = Calcoli.CalcolaPrezzoFinale(movie.PrezzoMovie, tipologiaSala.MaggiorazionePrezzo, bigliettoEsistente.NumeroBiglietti, utente, bigliettoEsistente.MetodoPagamento);

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