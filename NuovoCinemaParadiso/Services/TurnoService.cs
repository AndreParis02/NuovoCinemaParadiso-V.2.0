using Microsoft.EntityFrameworkCore;
using NuovoCinemaParadiso.Data;
using NuovoCinemaParadiso.Dtos;
using NuovoCinemaParadiso.Models;

namespace NuovoCinemaParadiso.Services
{
    public class TurnoService
    {
        private readonly ContestoDb _contesto;
        public TurnoService(ContestoDb contesto)
        {
            _contesto = contesto;
        }

        public async Task<List<DtoTurno>> OttieniTuttoAsync()
        {
            List<Turno> turni = await _contesto.Turni.ToListAsync();
            List<DtoTurno> risultato = new List<DtoTurno>();

            foreach (var turnoCorrente in turni)
            {
                DtoTurno dto = new DtoTurno();
                dto.Id = turnoCorrente.Id;
                dto.OraInizio = turnoCorrente.OraInizio;
                dto.OraFine = turnoCorrente.OraFine;
                dto.Nome = turnoCorrente.Nome;
                risultato.Add(dto);
            }

            return risultato;
        }

        public async Task<DtoTurno?> OttieniTramiteIdAsync(string id)
        {
            Turno? turno = await _contesto.Turni.FindAsync(id);
            if (turno == null)
            {
                return null;
            }

            DtoTurno dto = new DtoTurno();
            dto.Id = turno.Id;
            dto.Nome = turno.Nome;
            dto.OraInizio = turno.OraInizio;
            dto.OraFine = turno.OraFine;

            return dto;
        }

        public async Task<bool> CreazioneAsync(DtoCreazioneTurno dto)
        {

            bool turnoGiaPresente = await _contesto.Turni.AnyAsync(t => t.Nome.ToLower() == dto.Nome.ToLower());
            if (turnoGiaPresente)
            {
                return false;
            }

            Turno turno = new Turno();
            turno.Nome = dto.Nome;
            turno.OraInizio = dto.OraInizio;
            turno.OraFine = dto.OraFine;

            _contesto.Turni.Add(turno);
            await _contesto.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ModificaAsync(string id, DtoCreazioneTurno dto) 
        {

            Turno? turnoEsistente = await _contesto.Turni.FindAsync(id);
            if (turnoEsistente == null)
            {
                return false;
            }

            bool nomeGiaUsato = await _contesto.Turni.AnyAsync(t => t.Nome.ToLower() == dto.Nome.ToLower() && t.Id != id);
            if (nomeGiaUsato)
            {
                return false;
            }

            turnoEsistente.OraInizio = dto.OraInizio;
            turnoEsistente.OraFine = dto.OraFine;
            turnoEsistente.Nome = dto.Nome;

            await _contesto.SaveChangesAsync();

            return true;
        }

        public async Task<(bool Successo, string? Errore)> EliminaAsync(string id)
        {
            Turno? turno = await _contesto.Turni.FindAsync(id);
            if (turno == null)
            {
                return (false, "Turno non trovato.");
            }

            bool haProiezioniCollegate = await _contesto.Proiezioni.AnyAsync(p => p.TurnoId == id);
            if (haProiezioniCollegate)
            {
                return (false, "Impossibile eliminare il turno: ci sono ancora delle proiezioni assegnate a questo orario.");
            }

            _contesto.Turni.Remove(turno);
            await _contesto.SaveChangesAsync();

            return (true, null);
        }
    }
}