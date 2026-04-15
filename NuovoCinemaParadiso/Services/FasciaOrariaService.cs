using Microsoft.EntityFrameworkCore;
using NuovoCinemaParadiso.Data;
using NuovoCinemaParadiso.Dtos;
using NuovoCinemaParadiso.Models;

namespace NuovoCinemaParadiso.Services
{
    public class FasciaOrariaService
    {
        private readonly ContestoDb _contesto;
        public FasciaOrariaService(ContestoDb contesto)
        {
            _contesto = contesto;
        }

        public async Task<List<DtoFasciaOraria>> OttieniTuttoAsync()
        {
            List<FasciaOraria> fasceOrarie = await _contesto.FasceOrarie.ToListAsync();

            List<DtoFasciaOraria> risultato = new List<DtoFasciaOraria>();

            for (int i = 0; i < fasceOrarie.Count; i++)
            {
                FasciaOraria fasciaCorrente = fasceOrarie[i];

                DtoFasciaOraria dto = new DtoFasciaOraria();
                dto.Id = fasciaCorrente.Id;
                dto.Data = fasciaCorrente.Data;
                dto.OraInizio = fasciaCorrente.OraInizio;
                dto.OraFine = fasciaCorrente.OraFine;
                dto.Nome = fasciaCorrente.Nome;

                risultato.Add(dto);
            }

            return risultato;
        }

        public async Task<DtoFasciaOraria> OttieniTramiteIdAsync(string id) 
        {
            FasciaOraria? fasciaOraria = await _contesto.FasceOrarie.FindAsync(id);
            if (fasciaOraria == null)
            {
                return null;
            }

            DtoFasciaOraria dto = new DtoFasciaOraria();
            dto.Id = fasciaOraria.Id;
            dto.Nome = fasciaOraria.Nome;
            dto.Data = fasciaOraria.Data;
            dto.OraInizio = fasciaOraria.OraInizio;
            dto.OraFine = fasciaOraria.OraFine;

            return dto;
        }

        public async Task<DtoFasciaOraria> CreazioneAsync(DtoCreazioneFasciaOraria dto)
        {
            FasciaOraria fasciaOraria = new FasciaOraria();
            fasciaOraria.Nome = dto.Nome;
            fasciaOraria.Data =  dto.Data;
            fasciaOraria.OraInizio = dto.OraInizio;
            fasciaOraria.OraFine = dto.OraFine;

            _contesto.FasceOrarie.Add(fasciaOraria);
            await _contesto.SaveChangesAsync();

            DtoFasciaOraria risultato = new DtoFasciaOraria();
            risultato.Id = fasciaOraria.Id;
            risultato.Nome = fasciaOraria.Nome;
            risultato.Data = fasciaOraria.Data;
            risultato.OraInizio = fasciaOraria.OraInizio;
            risultato.OraFine = fasciaOraria.OraFine;

            return risultato;
        }

        public async Task<DtoFasciaOraria?> ModificaAsync(string id, DtoCreazioneFasciaOraria dto)
        {
            FasciaOraria? fasciaEsistente = await _contesto.FasceOrarie.FindAsync(id);

            if (fasciaEsistente == null)
            {
                return null;
            }

            fasciaEsistente.Data = dto.Data;
            fasciaEsistente.OraInizio = dto.OraInizio;
            fasciaEsistente.OraFine = dto.OraFine;
            fasciaEsistente.Nome = dto.Nome;

            await _contesto.SaveChangesAsync();

            DtoFasciaOraria risultato = new DtoFasciaOraria();
            risultato.Id = fasciaEsistente.Id;
            risultato.Data = fasciaEsistente.Data;
            risultato.OraInizio = fasciaEsistente.OraInizio;
            risultato.OraFine = fasciaEsistente.OraFine;
            risultato.Nome = fasciaEsistente.Nome;

            return risultato;
        }

        public async Task<bool> EliminaAsync(string id) 
        {
            FasciaOraria? fasciaOraria = await _contesto.FasceOrarie.FindAsync(id);

            if (fasciaOraria == null)
            {
                return false;
            }

            _contesto.FasceOrarie.Remove(fasciaOraria);
            await _contesto.SaveChangesAsync();

            return true;
        }
    }
}