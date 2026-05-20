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

    public async Task<(List<DtoBiglietto>? Dati, string? Errore)> OttieniTutto(string utenteId)
    {
        List<Biglietto> biglietti = await _contesto.Biglietti.ToListAsync();
        List<DtoBiglietto> risultato = new List<DtoBiglietto>();

        foreach (var bigliettoCorrente in biglietti)
        {
            if (bigliettoCorrente.UtenteId == utenteId)
            {
                // Recupero entità per i calcoli
                var utente = await _contesto.Utenti.FindAsync(bigliettoCorrente.UtenteId);
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
                        utente,
                        bigliettoCorrente.MetodoPagamento)
                };
                risultato.Add(dto);
            }
        }
        return (risultato, null);
    }

    public async Task<(DtoBiglietto? Dto, string? Errore)> OttieniTramiteIdAsync(string id, string utenteId)
    {
        Biglietto? biglietto = await _contesto.Biglietti.FindAsync(id);
        if (biglietto == null) return (null, "Biglietto non trovato.");

        if (biglietto.UtenteId != utenteId)
            return (null, "Accesso negato: questo biglietto non ti appartiene.");


        var utente = await _contesto.Utenti.FindAsync(biglietto.UtenteId);
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
            PrezzoFinale = Calcoli.CalcolaPrezzoFinale(movie.PrezzoMovie, tipologiaSala.MaggiorazionePrezzo, biglietto.NumeroBiglietti, utente, biglietto.MetodoPagamento)
        };

        return (dto, null);
    }

    public async Task<(DtoBiglietto? Dto, string? Errore)> CreazioneAsync(DtoCreazioneBiglietto dto, string utenteId)
    {
        
        /*controlla che l'utente esista*/
        var utente = await _contesto.Utenti.FindAsync(utenteId);
        if (utente == null) return (null, "Utente non trovato.");

        /*controlla che la proiezione esista*/
        var proiezione = await _contesto.Proiezioni.FindAsync(dto.ProiezioneId);
        if (proiezione == null) return (null, "Proiezione non trovata.");

        /*controlla che ci siano abbastanza posti in sala per il numero di biglietti richiesti*/
        var sala = await _contesto.Sale.FindAsync(proiezione.SalaId);
        if (sala == null) return (null, "Sala non trovata.");
        int postiOccupati = await _contesto.Biglietti.Where(b => b.ProiezioneId == dto.ProiezioneId).SumAsync(b => b.NumeroBiglietti);
        if (postiOccupati + dto.NumeroBiglietti > sala.Capienza) return (null, "Posti insufficienti per la proiezione selezionata.");
        
        /*controlla che il numero di biglietti sia positivo e non superiore a 100*/
        if (dto.NumeroBiglietti <= 0 || dto.NumeroBiglietti > 100) return (null, "Il numero di biglietti deve essere compreso tra 1 e 100.");  
        
        var movie = await _contesto.Movies.FindAsync(proiezione.MovieId);
        var tipologiaSala = await _contesto.TipologieSala.FindAsync(sala.TipologiaSalaId);
        
        /*controlla che l'utente abbia un saldo sufficiente*/
        if (utente.Saldo < Calcoli.CalcolaPrezzoFinale(movie.PrezzoMovie, tipologiaSala.MaggiorazionePrezzo, dto.NumeroBiglietti, utente, dto.MetodoPagamento))
            return (null, "Saldo insufficiente per acquistare i biglietti.");

        


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
        var contoCinema = await _contesto.ContoCinema.FirstOrDefaultAsync();
        var saldi = await Calcoli.CalcolaSaldo(biglietto.PrezzoFinale, utente,contoCinema);
        utente.Saldo = saldi[0];
        contoCinema.Conto = saldi[1];
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

        var utente = await _contesto.Utenti.FindAsync(biglietto.UtenteId);
        var contoCinema = await _contesto.ContoCinema.FirstOrDefaultAsync();

        if (utente == null || contoCinema == null)
            return (false, "Dati correlati all'biglietto non trovati.");
        var saldi = await Calcoli.CalcolaSaldo(-biglietto.PrezzoFinale, utente, contoCinema);
        utente.Saldo = saldi[0];
        contoCinema.Conto = saldi[1];

        _contesto.Biglietti.Remove(biglietto);
        await _contesto.SaveChangesAsync();
        return (true, null);
    }
}