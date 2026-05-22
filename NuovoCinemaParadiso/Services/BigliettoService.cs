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

    public async Task<(DtoBiglietto? Dto, string? Errore)> OttieniTramiteIdAsync(string id)
    {
        Biglietto? biglietto = await _contesto.Biglietti.FindAsync(id)
            ?? throw new Exception("Biglietto non trovato");
        var proiezione = await _contesto.Proiezioni.FindAsync(biglietto.ProiezioneId)
            ?? throw new Exception("Proiezione non trovato");
        var movie = await _contesto.Movies.FindAsync(proiezione.MovieId)
            ?? throw new Exception("Movie non trovato");
        var sala = await _contesto.Sale.FindAsync(proiezione.SalaId)
            ?? throw new Exception("Sala non trovato");
        var tipologiaSala = await _contesto.TipologieSala.FindAsync(sala.TipologiaSalaId)
            ?? throw new Exception("TipologiaSala non trovato");
        var turno = await _contesto.Turni.FindAsync(proiezione.TurnoId)
            ?? throw new Exception("Turno non trovato");
        if (biglietto == null) return (null, "Biglietto non trovato.");

        DtoBiglietto dto = new DtoBiglietto
        {
            Id = biglietto.Id,
            UtenteId = biglietto.UtenteId,
            ProiezioneId = biglietto.ProiezioneId,
            OrarioCreazione = biglietto.OrarioCreazione,
            NumeroBiglietti = biglietto.NumeroBiglietti,
            PrezzoFinale = biglietto.PrezzoFinale,
            NomeSala = sala.Nome,
            TitoloMovie = movie.Titolo,
            NomeTipologiaSala = tipologiaSala.Nome,
            OraInizio = turno.OraInizio,
            DataProiezione = proiezione.DataProiezione,
        };

        return (dto, null);
    }

    public async Task<List<DtoBiglietto>> OttieniTramiteProiezioneAsync(string proiezioneId)
    {
        List<Biglietto> biglietti = await _contesto.Biglietti.Where(b => b.ProiezioneId == proiezioneId).ToListAsync();
        Biglietto biglietto = new Biglietto();
        var proiezione = await _contesto.Proiezioni.FindAsync(biglietto.ProiezioneId)
            ?? throw new Exception("Proiezione non trovato");
        var movie = await _contesto.Movies.FindAsync(proiezione.MovieId)
            ?? throw new Exception("Movie non trovato");
        var sala = await _contesto.Sale.FindAsync(proiezione.SalaId)
            ?? throw new Exception("Sala non trovato");
        var tipologiaSala = await _contesto.TipologieSala.FindAsync(sala.TipologiaSalaId)
            ?? throw new Exception("TipologiaSala non trovato");
        var turno = await _contesto.Turni.FindAsync(proiezione.TurnoId)
            ?? throw new Exception("Turno non trovato");
        List<DtoBiglietto> risultato = new List<DtoBiglietto>();

        for (int i = 0; i < biglietti.Count; i++)
        {
            Biglietto bigliettoCorrente = biglietti[i];
            DtoBiglietto dto = new DtoBiglietto
            {
                Id = bigliettoCorrente.Id,
                UtenteId = bigliettoCorrente.UtenteId,
                ProiezioneId = bigliettoCorrente.ProiezioneId,
                OrarioCreazione = bigliettoCorrente.OrarioCreazione,
                NumeroBiglietti = bigliettoCorrente.NumeroBiglietti,
                PrezzoFinale = bigliettoCorrente.PrezzoFinale,
                NomeSala = sala.Nome,
                TitoloMovie = movie.Titolo,
                NomeTipologiaSala = tipologiaSala.Nome,
                OraInizio = turno.OraInizio,
                DataProiezione = proiezione.DataProiezione,
            };
            risultato.Add(dto);
        }
        return risultato;
    }

    public async Task<(string? successo, string? Errore)> CreazioneAsync(DtoCreazioneBiglietto dto, string utenteId)
    {
        /*controlla che l'utente esista*/
        var utente = await _contesto.Utenti.FindAsync(utenteId);
        if (utente == null) return (null, "Utente non trovato.");

        utente.Abbonamento = await _contesto.Abbonamenti.FindAsync(utente.AbbonamentoId);

        var proiezione = await _contesto.Proiezioni.FindAsync(dto.ProiezioneId);
        if (proiezione == null) return (null, "Proiezione non trovata.");

        /*controlla che ci siano abbastanza posti in sala per il numero di biglietti richiesti*/
        var sala = await _contesto.Sale.FindAsync(proiezione.SalaId);
        if (sala == null) return (null, "Sala non trovata.");

        //i posti occupati sono definiti tramite il numero di biglietti venduti per quella proiezione, quindi si somma il numero di biglietti di tutti i biglietti venduti per quella proiezione
        int postiOccupati = (await OttieniTramiteProiezioneAsync(dto.ProiezioneId)).Count;
        if (postiOccupati + dto.NumeroBiglietti > sala.Capienza) return (null, "Posti insufficienti per la proiezione selezionata.");

        /*controlla che il numero di biglietti sia positivo e non superiore a 100*/
        if (dto.NumeroBiglietti <= 0 || dto.NumeroBiglietti > 100) return (null, "Il numero di biglietti deve essere compreso tra 1 e 100.");

        var movie = await _contesto.Movies.FindAsync(proiezione.MovieId);
        if (movie == null) return (null, "Film non trovato.");

        var tipologiaSala = await _contesto.TipologieSala.FindAsync(sala.TipologiaSalaId);
        if (tipologiaSala == null) return (null, "Tipologia di sala non trovata.");

        /*controlla che l'utente abbia un saldo sufficiente*/
        if (utente.Saldo < Calcoli.CalcolaPrezzoFinale(movie.PrezzoMovie, tipologiaSala.MaggiorazionePrezzo, dto.NumeroBiglietti, utente.Abbonamento, utente.DataInizioAbbonamento))
            return (null, "Saldo insufficiente per acquistare i biglietti.");

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
        var saldi = await Calcoli.CalcolaSaldo(biglietto.PrezzoFinale, utente, contoCinema);
        utente.Saldo = saldi[0];
        contoCinema.Saldo = saldi[1];
        await _contesto.SaveChangesAsync();

        return ("Biglietto creato con successo.", null);
    }

    public async Task<(string? successo, string? Errore)> ModificaAsync(string id, DtoCreazioneBiglietto dto)
    {
        var bigliettoEsistente = await _contesto.Biglietti.FindAsync(id);
        if (bigliettoEsistente == null) return (null, "Biglietto non trovato.");

        var utente = await _contesto.Utenti.FindAsync(bigliettoEsistente.UtenteId);
        if (utente == null) return (null, "Utente non trovato.");

        var proiezione = await _contesto.Proiezioni.FindAsync(dto.ProiezioneId);
        if (proiezione == null) return (null, "Proiezione non trovata.");


        var sala = await _contesto.Sale.FindAsync(proiezione.SalaId);
        if (sala == null) return (null, "Sala non trovata.");
        int postiOccupati = await _contesto.Biglietti.Where(b => b.ProiezioneId == dto.ProiezioneId).SumAsync(b => b.NumeroBiglietti);

        if (proiezione.Id != bigliettoEsistente.ProiezioneId)
        {
            if (postiOccupati + dto.NumeroBiglietti > sala.Capienza) return (null, "Posti insufficienti per la proiezione selezionata.");
        }
        else
        {
            postiOccupati = postiOccupati - bigliettoEsistente.NumeroBiglietti;
        }
        if (postiOccupati + dto.NumeroBiglietti > sala.Capienza) return (null, "Posti insufficienti per la proiezione selezionata.");

        if (dto.NumeroBiglietti <= 0 || dto.NumeroBiglietti > 100) return (null, "Il numero di biglietti deve essere compreso tra 1 e 100.");

        var movie = await _contesto.Movies.FindAsync(proiezione.MovieId);
        if (movie == null) return (null, "Film non trovato.");

        var tipologiaSala = await _contesto.TipologieSala.FindAsync(sala.TipologiaSalaId);
        if (tipologiaSala == null) return (null, "Tipologia di sala non trovata.");

        if (utente.AbbonamentoId == null)
            throw new NotFoundException("Abbonamento", "Nessun abbonamento associato all'utente");
        if (utente.Abbonamento == null)
            throw new NotFoundException("Abbonamento", utente.AbbonamentoId);

        if (utente.Saldo + bigliettoEsistente.PrezzoFinale < Calcoli.CalcolaPrezzoFinale(movie.PrezzoMovie, tipologiaSala.MaggiorazionePrezzo, dto.NumeroBiglietti, utente.Abbonamento, utente.DataInizioAbbonamento))
            return (null, "Saldo insufficiente per acquistare i biglietti.");

        Biglietto biglietto = new Biglietto
        {
            UtenteId = utente.Id,
            ProiezioneId = proiezione.Id,
            NumeroBiglietti = dto.NumeroBiglietti,
            OrarioCreazione = DateTimeOffset.UtcNow,
            PrezzoFinale = Calcoli.CalcolaPrezzoFinale(movie.PrezzoMovie, tipologiaSala.MaggiorazionePrezzo, dto.NumeroBiglietti, utente.Abbonamento, utente.DataInizioAbbonamento)
        };

        var differenzaPrezzo = biglietto.PrezzoFinale - bigliettoEsistente.PrezzoFinale;

        _contesto.Biglietti.Add(biglietto);
        var contoCinema = await _contesto.ContoCinema.FirstOrDefaultAsync();
        var saldi = await Calcoli.CalcolaSaldo(differenzaPrezzo, utente, contoCinema);
        utente.Saldo = saldi[0];
        contoCinema.Saldo = saldi[1];
        await _contesto.SaveChangesAsync();

        return ("Biglietto modificato con successo.", null);
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
        contoCinema.Saldo = saldi[1];

        _contesto.Biglietti.Remove(biglietto);
        await _contesto.SaveChangesAsync();
        return (true, null);
    }
}