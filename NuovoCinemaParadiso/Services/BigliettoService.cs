using Microsoft.EntityFrameworkCore;
using NuovoCinemaParadiso.Data;
using NuovoCinemaParadiso.Dtos;
using NuovoCinemaParadiso.Models;
using NuovoCinemaParadiso.Helpers;
using NuovoCinemaParadiso.Exceptions;

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
            Proiezione? proiezione = await _contesto.Proiezioni.FindAsync(bigliettoCorrente.ProiezioneId)
                ?? throw new NotFoundException("Proiezione", bigliettoCorrente.ProiezioneId);
            Movie? movie = await _contesto.Movies.FindAsync(proiezione.MovieId)
                ?? throw new NotFoundException("Movie", proiezione.MovieId);
            Sala? sala = await _contesto.Sale.FindAsync(proiezione.SalaId)
                ?? throw new NotFoundException("Sala", proiezione.SalaId);
            TipologiaSala? tipologiaSala = await _contesto.TipologieSala.FindAsync(sala.TipologiaSalaId)
                ?? throw new NotFoundException("TipologiaSala", sala.TipologiaSalaId);
            Utente? utente = await _contesto.Utenti.FindAsync(bigliettoCorrente.UtenteId)
                ?? throw new NotFoundException("Utente", bigliettoCorrente.UtenteId);

            if (utente.AbbonamentoId == null)
                throw new NotFoundException("Abbonamento", "Nessun abbonamento associato all'utente");

            if (utente.Abbonamento == null)
                throw new NotFoundException("Abbonamento", utente.AbbonamentoId);

            DtoBiglietto dto = new DtoBiglietto();
            dto.Id = bigliettoCorrente.Id;
            dto.UtenteId = bigliettoCorrente.UtenteId;
            dto.ProiezioneId = bigliettoCorrente.ProiezioneId;
            dto.OrarioCreazione = bigliettoCorrente.OrarioCreazione;
            dto.NumeroBiglietti = bigliettoCorrente.NumeroBiglietti;
            dto.PrezzoFinale = Calcoli.CalcolaPrezzoFinale(
                movie.PrezzoMovie,
                tipologiaSala.MaggiorazionePrezzo,
                bigliettoCorrente.NumeroBiglietti,
                utente.Abbonamento,
                utente.DataInizioAbbonamento
            );

            risultato.Add(dto);
        }
        return risultato;
    }

    public async Task<(DtoBiglietto? Dto, string? Errore)> OttieniTramiteIdAsync(string id, string utenteId)
    {
        Biglietto? biglietto = await _contesto.Biglietti.FindAsync(id);
        if (biglietto == null) return (null, "Biglietto non trovato.");

        if (biglietto.UtenteId != utenteId)
            return (null, "Accesso negato: questo biglietto non ti appartiene.");

        var utente = await _contesto.Utenti.FindAsync(biglietto.UtenteId)
            ?? throw new NotFoundException("Utente", biglietto.UtenteId);

        if (utente.AbbonamentoId == null)
            throw new NotFoundException("Abbonamento", "Nessun abbonamento associato all'utente");
        if (utente.Abbonamento == null)
            throw new NotFoundException("Abbonamento", utente.AbbonamentoId);

        utente.Abbonamento = await _contesto.Abbonamenti.FindAsync(utente.AbbonamentoId)
            ?? throw new NotFoundException("Abbonamento", utente.AbbonamentoId);


        var proiezione = await _contesto.Proiezioni.FindAsync(biglietto.ProiezioneId); // <-- AGGIUNTA
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
            PrezzoFinale = Calcoli.CalcolaPrezzoFinale(movie.PrezzoMovie,
                tipologiaSala.MaggiorazionePrezzo,
                biglietto.NumeroBiglietti,
                utente.Abbonamento,
                utente.DataInizioAbbonamento
            )
        };

        return (dto, null);
    }

    public async Task<(DtoBiglietto? Dto, string? Errore)> CreazioneAsync(DtoCreazioneBiglietto dto, string utenteId)
    {
        
        /*controlla che l'utente esista*/
        var utente = await _contesto.Utenti.FindAsync(utenteId);
        if (utente == null) return (null, "Utente non trovato.");

        utente.Abbonamento = await _contesto.Abbonamenti.FindAsync(utente.AbbonamentoId);
        if (utente.Abbonamento == null) return (null, "Abbonamento non trovato.");

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
        /*if (utente.Saldo < Calcoli.CalcolaPrezzoFinale(movie.PrezzoMovie, tipologiaSala.MaggiorazionePrezzo, dto.NumeroBiglietti, utente, dto.MetodoPagamento))
            return (null, "Saldo insufficiente per acquistare i biglietti.");*/

        


        Biglietto biglietto = new Biglietto
        {
            UtenteId = utenteId,
            ProiezioneId = proiezione.Id,
            NumeroBiglietti = dto.NumeroBiglietti,
            OrarioCreazione = DateTimeOffset.UtcNow,
            PrezzoFinale = Calcoli.CalcolaPrezzoFinale(movie.PrezzoMovie, tipologiaSala.MaggiorazionePrezzo, dto.NumeroBiglietti, utente.Abbonamento, utente.DataInizioAbbonamento)
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
            OrarioCreazione = biglietto.OrarioCreazione
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
        var utente = await _contesto.Utenti.FindAsync(bigliettoEsistente.UtenteId)
            ?? throw new NotFoundException("Utente", bigliettoEsistente.UtenteId);
        if (utente.AbbonamentoId == null)
            throw new NotFoundException("Abbonamento", "Nessun abbonamento associato all'utente");
        utente.Abbonamento = await _contesto.Abbonamenti.FindAsync(utente.AbbonamentoId);
        if (utente.Abbonamento == null)
            throw new NotFoundException("Abbonamento", utente.AbbonamentoId);

        var tipologiaSala = await _contesto.TipologieSala.FindAsync(sala?.TipologiaSalaId);

        if (movie == null || sala == null || utente == null || tipologiaSala == null)
            return (null, "Dati correlati all'biglietto non trovati o non validi.");

        bigliettoEsistente.NumeroBiglietti = dto.NumeroBiglietti;
        bigliettoEsistente.PrezzoFinale = Calcoli.CalcolaPrezzoFinale(movie.PrezzoMovie,
            tipologiaSala.MaggiorazionePrezzo,
            bigliettoEsistente.NumeroBiglietti,
            utente.Abbonamento,
            utente.DataInizioAbbonamento);

        await _contesto.SaveChangesAsync();

        return (new DtoBiglietto
        {
            Id = bigliettoEsistente.Id,
            UtenteId = bigliettoEsistente.UtenteId,
            ProiezioneId = bigliettoEsistente.ProiezioneId,
            NumeroBiglietti = bigliettoEsistente.NumeroBiglietti,
            PrezzoFinale = bigliettoEsistente.PrezzoFinale,
            OrarioCreazione = bigliettoEsistente.OrarioCreazione,
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