using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Services
{
    public class EdificioService : EntityService<Edificio>, IEdificioService
    {
        public EdificioService(ApplicationDbContext context) : base(context)
        {
        }

        public override async Task<Edificio> GetByIdAsync(Guid id)
        {
            return await _dbSet
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<IEnumerable<UnitaImmobiliare>> GetUnitaImmobiliariByEdificioIdAsync(Guid edificioId)
        {
            return await _context.UnitaImmobiliari
                .Where(u => EF.Property<Guid>(u, "EdificioId") == edificioId)
                .ToListAsync();
        }

        public async Task<bool> AddStrutturaToEdificioAsync(Guid edificioId, Guid strutturaId)
        {
            // Verifica che entrambe le entità esistano
            var edificio = await _dbSet.FindAsync(edificioId);
            var struttura = await _context.Strutture.FindAsync(strutturaId);

            if (edificio == null || struttura == null)
                return false;

            // Verifica se la relazione già esiste nel database
            var exists = await _context.Set<EdificioStrutture>()
                .AnyAsync(es => es.EdificioId == edificioId && es.StruttureId == strutturaId);

            if (exists)
                return true; // La relazione esiste già

            // Crea l'entità di join
            _context.Set<EdificioStrutture>().Add(new EdificioStrutture
            {
                EdificioId = edificioId,
                StruttureId = strutturaId
            });

            // Aggiorna anche la lista di ID nell'oggetto Edificio
            if (edificio.StruttureIds == null)
                edificio.StruttureIds = new List<Guid>();

            if (!edificio.StruttureIds.Contains(strutturaId))
                edificio.StruttureIds.Add(strutturaId);

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveStrutturaFromEdificioAsync(Guid edificioId, Guid strutturaId)
        {
            // Verifica che entrambe le entità esistano
            var edificio = await _dbSet.FindAsync(edificioId);
            var struttura = await _context.Strutture.FindAsync(strutturaId);

            if (edificio == null || struttura == null)
                return false;

            // Trova la relazione nel database
            var relation = await _context.Set<EdificioStrutture>()
                .FirstOrDefaultAsync(es => es.EdificioId == edificioId && es.StruttureId == strutturaId);

            if (relation == null)
                return true; // La relazione non esiste, quindi non c'è nulla da rimuovere

            // Rimuovi l'entità di join
            _context.Set<EdificioStrutture>().Remove(relation);

            // Aggiorna anche la lista di ID nell'oggetto Edificio
            if (edificio.StruttureIds != null && edificio.StruttureIds.Contains(strutturaId))
                edificio.StruttureIds.Remove(strutturaId);

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AddInfissiToEdificioAsync(Guid edificioId, Guid infissiId)
        {
            // Verifica che entrambe le entità esistano
            var edificio = await _dbSet.FindAsync(edificioId);
            var infissi = await _context.Infissi.FindAsync(infissiId);

            if (edificio == null || infissi == null)
                return false;

            // Verifica se la relazione già esiste nel database
            var exists = await _context.Set<EdificioInfissi>()
                .AnyAsync(ei => ei.EdificioId == edificioId && ei.InfissiId == infissiId);

            if (exists)
                return true; // La relazione esiste già

            // Crea l'entità di join
            _context.Set<EdificioInfissi>().Add(new EdificioInfissi
            {
                EdificioId = edificioId,
                InfissiId = infissiId
            });

            // Aggiorna anche la lista di ID nell'oggetto Edificio
            if (edificio.InfissiIds == null)
                edificio.InfissiIds = new List<Guid>();

            if (!edificio.InfissiIds.Contains(infissiId))
                edificio.InfissiIds.Add(infissiId);

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveInfissiFromEdificioAsync(Guid edificioId, Guid infissiId)
        {
            // Verifica che entrambe le entità esistano
            var edificio = await _dbSet.FindAsync(edificioId);
            var infissi = await _context.Infissi.FindAsync(infissiId);

            if (edificio == null || infissi == null)
                return false;

            // Trova la relazione nel database
            var relation = await _context.Set<EdificioInfissi>()
                .FirstOrDefaultAsync(ei => ei.EdificioId == edificioId && ei.InfissiId == infissiId);

            if (relation == null)
                return true; // La relazione non esiste, quindi non c'è nulla da rimuovere

            // Rimuovi l'entità di join
            _context.Set<EdificioInfissi>().Remove(relation);

            // Aggiorna anche la lista di ID nell'oggetto Edificio
            if (edificio.InfissiIds != null && edificio.InfissiIds.Contains(infissiId))
                edificio.InfissiIds.Remove(infissiId);

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AddIdraulicoAdduzioneToEdificioAsync(Guid edificioId, Guid idraulicoAdduzioneId)
        {
            // Verifica che entrambe le entità esistano
            var edificio = await _dbSet.FindAsync(edificioId);
            var idraulicoAdduzione = await _context.IdraulicoAdduzione.FindAsync(idraulicoAdduzioneId);

            if (edificio == null || idraulicoAdduzione == null)
                return false;

            // Verifica se la relazione già esiste nel database
            var exists = await _context.Set<EdificioIdraulicoAdduzione>()
                .AnyAsync(eia => eia.EdificioId == edificioId && eia.IdraulicoAdduzioneId == idraulicoAdduzioneId);

            if (exists)
                return true; // La relazione esiste già

            // Crea l'entità di join
            _context.Set<EdificioIdraulicoAdduzione>().Add(new EdificioIdraulicoAdduzione
            {
                EdificioId = edificioId,
                IdraulicoAdduzioneId = idraulicoAdduzioneId
            });

            // Aggiorna anche la lista di ID nell'oggetto Edificio
            if (edificio.IdraulicoAdduzioneIds == null)
                edificio.IdraulicoAdduzioneIds = new List<Guid>();

            if (!edificio.IdraulicoAdduzioneIds.Contains(idraulicoAdduzioneId))
                edificio.IdraulicoAdduzioneIds.Add(idraulicoAdduzioneId);

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveIdraulicoAdduzioneFromEdificioAsync(Guid edificioId, Guid idraulicoAdduzioneId)
        {
            // Verifica che entrambe le entità esistano
            var edificio = await _dbSet.FindAsync(edificioId);
            var idraulicoAdduzione = await _context.IdraulicoAdduzione.FindAsync(idraulicoAdduzioneId);

            if (edificio == null || idraulicoAdduzione == null)
                return false;

            // Trova la relazione nel database
            var relation = await _context.Set<EdificioIdraulicoAdduzione>()
                .FirstOrDefaultAsync(eia => eia.EdificioId == edificioId && eia.IdraulicoAdduzioneId == idraulicoAdduzioneId);

            if (relation == null)
                return true; // La relazione non esiste, quindi non c'è nulla da rimuovere

            // Rimuovi l'entità di join
            _context.Set<EdificioIdraulicoAdduzione>().Remove(relation);

            // Aggiorna anche la lista di ID nell'oggetto Edificio
            if (edificio.IdraulicoAdduzioneIds != null && edificio.IdraulicoAdduzioneIds.Contains(idraulicoAdduzioneId))
                edificio.IdraulicoAdduzioneIds.Remove(idraulicoAdduzioneId);

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AddScarichiIdriciFognariToEdificioAsync(Guid edificioId, Guid scarichiIdriciFognariId)
        {
            // Verifica che entrambe le entità esistano
            var edificio = await _dbSet.FindAsync(edificioId);
            var scarichiIdriciFognari = await _context.ScarichiIdriciFognari.FindAsync(scarichiIdriciFognariId);

            if (edificio == null || scarichiIdriciFognari == null)
                return false;

            // Verifica se la relazione già esiste nel database
            var exists = await _context.Set<EdificioScarichiIdriciFognari>()
                .AnyAsync(es => es.EdificioId == edificioId && es.ScarichiIdriciFognariId == scarichiIdriciFognariId);

            if (exists)
                return true; // La relazione esiste già

            // Crea l'entità di join
            _context.Set<EdificioScarichiIdriciFognari>().Add(new EdificioScarichiIdriciFognari
            {
                EdificioId = edificioId,
                ScarichiIdriciFognariId = scarichiIdriciFognariId
            });

            // Aggiorna anche la lista di ID nell'oggetto Edificio
            if (edificio.ScarichiIdriciFognariIds == null)
                edificio.ScarichiIdriciFognariIds = new List<Guid>();

            if (!edificio.ScarichiIdriciFognariIds.Contains(scarichiIdriciFognariId))
                edificio.ScarichiIdriciFognariIds.Add(scarichiIdriciFognariId);

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveScarichiIdriciFognariFromEdificioAsync(Guid edificioId, Guid scarichiIdriciFognariId)
        {
            // Verifica che entrambe le entità esistano
            var edificio = await _dbSet.FindAsync(edificioId);
            var scarichiIdriciFognari = await _context.ScarichiIdriciFognari.FindAsync(scarichiIdriciFognariId);

            if (edificio == null || scarichiIdriciFognari == null)
                return false;

            // Trova la relazione nel database
            var relation = await _context.Set<EdificioScarichiIdriciFognari>()
                .FirstOrDefaultAsync(es => es.EdificioId == edificioId && es.ScarichiIdriciFognariId == scarichiIdriciFognariId);

            if (relation == null)
                return true; // La relazione non esiste, quindi non c'è nulla da rimuovere

            // Rimuovi l'entità di join
            _context.Set<EdificioScarichiIdriciFognari>().Remove(relation);

            // Aggiorna anche la lista di ID nell'oggetto Edificio
            if (edificio.ScarichiIdriciFognariIds != null && edificio.ScarichiIdriciFognariIds.Contains(scarichiIdriciFognariId))
                edificio.ScarichiIdriciFognariIds.Remove(scarichiIdriciFognariId);

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AddImpiantiElettriciToEdificioAsync(Guid edificioId, Guid impiantiElettriciId)
        {
            var edificio = await _dbSet.FindAsync(edificioId);
            var impiantiElettrici = await _context.ImpiantiElettrici.FindAsync(impiantiElettriciId);

            if (edificio == null || impiantiElettrici == null)
                return false;

            var exists = await _context.Set<EdificioImpiantiElettrici>()
                .AnyAsync(ei => ei.EdificioId == edificioId && ei.ImpiantiElettriciId == impiantiElettriciId);

            if (exists)
                return true;

            _context.Set<EdificioImpiantiElettrici>().Add(new EdificioImpiantiElettrici
            {
                EdificioId = edificioId,
                ImpiantiElettriciId = impiantiElettriciId
            });

            if (edificio.ImpiantiElettriciIds == null)
                edificio.ImpiantiElettriciIds = new List<Guid>();

            if (!edificio.ImpiantiElettriciIds.Contains(impiantiElettriciId))
                edificio.ImpiantiElettriciIds.Add(impiantiElettriciId);

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveImpiantiElettriciFromEdificioAsync(Guid edificioId, Guid impiantiElettriciId)
        {
            var edificio = await _dbSet.FindAsync(edificioId);
            var impiantiElettrici = await _context.ImpiantiElettrici.FindAsync(impiantiElettriciId);

            if (edificio == null || impiantiElettrici == null)
                return false;

            var relation = await _context.Set<EdificioImpiantiElettrici>()
                .FirstOrDefaultAsync(ei => ei.EdificioId == edificioId && ei.ImpiantiElettriciId == impiantiElettriciId);

            if (relation == null)
                return true;

            _context.Set<EdificioImpiantiElettrici>().Remove(relation);

            if (edificio.ImpiantiElettriciIds != null && edificio.ImpiantiElettriciIds.Contains(impiantiElettriciId))
                edificio.ImpiantiElettriciIds.Remove(impiantiElettriciId);

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AddImpiantoClimaAcsToEdificioAsync(Guid edificioId, Guid impiantoClimaAcsId)
        {
            var edificio = await _dbSet.FindAsync(edificioId);
            var impiantoClimaAcs = await _context.ImpiantoClimaAcs.FindAsync(impiantoClimaAcsId);

            if (edificio == null || impiantoClimaAcs == null)
                return false;

            var exists = await _context.Set<EdificioImpiantoClimaAcs>()
                .AnyAsync(ei => ei.EdificioId == edificioId && ei.ImpiantoClimaAcsId == impiantoClimaAcsId);

            if (exists)
                return true;

            _context.Set<EdificioImpiantoClimaAcs>().Add(new EdificioImpiantoClimaAcs
            {
                EdificioId = edificioId,
                ImpiantoClimaAcsId = impiantoClimaAcsId
            });

            if (edificio.ImpiantoClimaAcsIds == null)
                edificio.ImpiantoClimaAcsIds = new List<Guid>();

            if (!edificio.ImpiantoClimaAcsIds.Contains(impiantoClimaAcsId))
                edificio.ImpiantoClimaAcsIds.Add(impiantoClimaAcsId);

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveImpiantoClimaAcsFromEdificioAsync(Guid edificioId, Guid impiantoClimaAcsId)
        {
            var edificio = await _dbSet.FindAsync(edificioId);
            var impiantoClimaAcs = await _context.ImpiantoClimaAcs.FindAsync(impiantoClimaAcsId);

            if (edificio == null || impiantoClimaAcs == null)
                return false;

            var relation = await _context.Set<EdificioImpiantoClimaAcs>()
                .FirstOrDefaultAsync(ei => ei.EdificioId == edificioId && ei.ImpiantoClimaAcsId == impiantoClimaAcsId);

            if (relation == null)
                return true;

            _context.Set<EdificioImpiantoClimaAcs>().Remove(relation);

            if (edificio.ImpiantoClimaAcsIds != null && edificio.ImpiantoClimaAcsIds.Contains(impiantoClimaAcsId))
                edificio.ImpiantoClimaAcsIds.Remove(impiantoClimaAcsId);

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AddAltriImpiantiToEdificioAsync(Guid edificioId, Guid altriImpiantiId)
        {
            var edificio = await _dbSet.FindAsync(edificioId);
            var altriImpianti = await _context.AltriImpianti.FindAsync(altriImpiantiId);

            if (edificio == null || altriImpianti == null)
                return false;

            var exists = await _context.Set<EdificioAltriImpianti>()
                .AnyAsync(ea => ea.EdificioId == edificioId && ea.AltriImpiantiId == altriImpiantiId);

            if (exists)
                return true;

            _context.Set<EdificioAltriImpianti>().Add(new EdificioAltriImpianti
            {
                EdificioId = edificioId,
                AltriImpiantiId = altriImpiantiId
            });

            if (edificio.AltriImpiantiIds == null)
                edificio.AltriImpiantiIds = new List<Guid>();

            if (!edificio.AltriImpiantiIds.Contains(altriImpiantiId))
                edificio.AltriImpiantiIds.Add(altriImpiantiId);

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveAltriImpiantiFromEdificioAsync(Guid edificioId, Guid altriImpiantiId)
        {
            var edificio = await _dbSet.FindAsync(edificioId);
            var altriImpianti = await _context.AltriImpianti.FindAsync(altriImpiantiId);

            if (edificio == null || altriImpianti == null)
                return false;

            var relation = await _context.Set<EdificioAltriImpianti>()
                .FirstOrDefaultAsync(ea => ea.EdificioId == edificioId && ea.AltriImpiantiId == altriImpiantiId);

            if (relation == null)
                return true;

            _context.Set<EdificioAltriImpianti>().Remove(relation);

            if (edificio.AltriImpiantiIds != null && edificio.AltriImpiantiIds.Contains(altriImpiantiId))
                edificio.AltriImpiantiIds.Remove(altriImpiantiId);

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AddDocumentiGeneraliToEdificioAsync(Guid edificioId, Guid documentiGeneraliId)
        {
            var edificio = await _dbSet.FindAsync(edificioId);
            var documentiGenerali = await _context.DocumentiGenerali.FindAsync(documentiGeneraliId);

            if (edificio == null || documentiGenerali == null)
                return false;

            var exists = await _context.Set<EdificioDocumentiGenerali>()
                .AnyAsync(ed => ed.EdificioId == edificioId && ed.DocumentiGeneraliId == documentiGeneraliId);

            if (exists)
                return true;

            _context.Set<EdificioDocumentiGenerali>().Add(new EdificioDocumentiGenerali
            {
                EdificioId = edificioId,
                DocumentiGeneraliId = documentiGeneraliId
            });

            if (edificio.DocumentiGeneraliIds == null)
                edificio.DocumentiGeneraliIds = new List<Guid>();

            if (!edificio.DocumentiGeneraliIds.Contains(documentiGeneraliId))
                edificio.DocumentiGeneraliIds.Add(documentiGeneraliId);

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveDocumentiGeneraliFromEdificioAsync(Guid edificioId, Guid documentiGeneraliId)
        {
            var edificio = await _dbSet.FindAsync(edificioId);
            var documentiGenerali = await _context.DocumentiGenerali.FindAsync(documentiGeneraliId);

            if (edificio == null || documentiGenerali == null)
                return false;

            var relation = await _context.Set<EdificioDocumentiGenerali>()
                .FirstOrDefaultAsync(ed => ed.EdificioId == edificioId && ed.DocumentiGeneraliId == documentiGeneraliId);

            if (relation == null)
                return true;

            _context.Set<EdificioDocumentiGenerali>().Remove(relation);

            if (edificio.DocumentiGeneraliIds != null && edificio.DocumentiGeneraliIds.Contains(documentiGeneraliId))
                edificio.DocumentiGeneraliIds.Remove(documentiGeneraliId);

            await _context.SaveChangesAsync();
            return true;
        }
    }
}