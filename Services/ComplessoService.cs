using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Services
{
    public class ComplessoService : EntityService<Complesso>, IComplessoService
    {
        public ComplessoService(ApplicationDbContext context) : base(context)
        {
        }

        public override async Task<Complesso> GetByIdAsync(Guid id)
        {
            var complesso = await _dbSet.FirstOrDefaultAsync(c => c.Id == id);
            return complesso ?? throw new KeyNotFoundException($"Complesso with ID {id} not found.");
        }

        public async Task<IEnumerable<Edificio>> GetEdificiByComplessoIdAsync(Guid complessoId)
        {
            return await _context.Edifici
                .Where(e => EF.Property<Guid>(e, "ComplessoId") == complessoId)
                .ToListAsync();
        }

        public async Task<bool> AddStrutturaToComplessoAsync(Guid complessoId, Guid strutturaId)
        {
            // Verifica che entrambe le entità esistano
            var complesso = await _dbSet.FindAsync(complessoId);
            var struttura = await _context.Strutture.FindAsync(strutturaId);

            if (complesso == null || struttura == null)
                return false;

            // Verifica se la relazione già esiste nel database
            var exists = await _context.Set<ComplessoStrutture>()
                .AnyAsync(cs => cs.ComplessoId == complessoId && cs.StruttureId == strutturaId);

            if (exists)
                return true; // La relazione esiste già

            // Crea l'entità di join
            _context.Set<ComplessoStrutture>().Add(new ComplessoStrutture
            {
                ComplessoId = complessoId,
                StruttureId = strutturaId
            });

            // Aggiorna anche la lista di ID nell'oggetto Complesso
            if (complesso.StruttureIds == null)
                complesso.StruttureIds = new List<Guid>();

            if (!complesso.StruttureIds.Contains(strutturaId))
                complesso.StruttureIds.Add(strutturaId);

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveStrutturaFromComplessoAsync(Guid complessoId, Guid strutturaId)
        {
            // Verifica che entrambe le entità esistano
            var complesso = await _dbSet.FindAsync(complessoId);
            var struttura = await _context.Strutture.FindAsync(strutturaId);

            if (complesso == null || struttura == null)
                return false;

            // Trova la relazione nel database
            var relation = await _context.Set<ComplessoStrutture>()
                .FirstOrDefaultAsync(cs => cs.ComplessoId == complessoId && cs.StruttureId == strutturaId);

            if (relation == null)
                return true; // La relazione non esiste, quindi non c'è nulla da rimuovere

            // Rimuovi l'entità di join
            _context.Set<ComplessoStrutture>().Remove(relation);

            // Aggiorna anche la lista di ID nell'oggetto Complesso
            if (complesso.StruttureIds != null && complesso.StruttureIds.Contains(strutturaId))
                complesso.StruttureIds.Remove(strutturaId);

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AddIdraulicoAdduzioneToComplessoAsync(Guid complessoId, Guid idraulicoAdduzioneId)
        {
            // Verifica che entrambe le entità esistano
            var complesso = await _dbSet.FindAsync(complessoId);
            var idraulicoAdduzione = await _context.IdraulicoAdduzione.FindAsync(idraulicoAdduzioneId);

            if (complesso == null || idraulicoAdduzione == null)
                return false;

            // Verifica se la relazione già esiste nel database
            var exists = await _context.Set<ComplessoIdraulicoAdduzione>()
                .AnyAsync(ci => ci.ComplessoId == complessoId && ci.IdraulicoAdduzioneId == idraulicoAdduzioneId);

            if (exists)
                return true; // La relazione esiste già

            // Crea l'entità di join
            _context.Set<ComplessoIdraulicoAdduzione>().Add(new ComplessoIdraulicoAdduzione
            {
                ComplessoId = complessoId,
                IdraulicoAdduzioneId = idraulicoAdduzioneId
            });

            // Aggiorna anche la lista di ID nell'oggetto Complesso
            if (complesso.IdraulicoAdduzioneIds == null)
                complesso.IdraulicoAdduzioneIds = new List<Guid>();

            if (!complesso.IdraulicoAdduzioneIds.Contains(idraulicoAdduzioneId))
                complesso.IdraulicoAdduzioneIds.Add(idraulicoAdduzioneId);

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveIdraulicoAdduzioneFromComplessoAsync(Guid complessoId, Guid idraulicoAdduzioneId)
        {
            // Verifica che entrambe le entità esistano
            var complesso = await _dbSet.FindAsync(complessoId);
            var idraulicoAdduzione = await _context.IdraulicoAdduzione.FindAsync(idraulicoAdduzioneId);

            if (complesso == null || idraulicoAdduzione == null)
                return false;

            // Trova la relazione nel database
            var relation = await _context.Set<ComplessoIdraulicoAdduzione>()
                .FirstOrDefaultAsync(ci => ci.ComplessoId == complessoId && ci.IdraulicoAdduzioneId == idraulicoAdduzioneId);

            if (relation == null)
                return true; // La relazione non esiste, quindi non c'è nulla da rimuovere

            // Rimuovi l'entità di join
            _context.Set<ComplessoIdraulicoAdduzione>().Remove(relation);

            // Aggiorna anche la lista di ID nell'oggetto Complesso
            if (complesso.IdraulicoAdduzioneIds != null && complesso.IdraulicoAdduzioneIds.Contains(idraulicoAdduzioneId))
                complesso.IdraulicoAdduzioneIds.Remove(idraulicoAdduzioneId);

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AddScarichiIdriciFognariToComplessoAsync(Guid complessoId, Guid scarichiIdriciFognariId)
        {
            // Verifica che entrambe le entità esistano
            var complesso = await _dbSet.FindAsync(complessoId);
            var scarichiIdriciFognari = await _context.ScarichiIdriciFognari.FindAsync(scarichiIdriciFognariId);

            if (complesso == null || scarichiIdriciFognari == null)
                return false;

            // Verifica se la relazione già esiste nel database
            var exists = await _context.Set<ComplessoScarichiIdriciFognari>()
                .AnyAsync(cs => cs.ComplessoId == complessoId && cs.ScarichiIdriciFognariId == scarichiIdriciFognariId);

            if (exists)
                return true; // La relazione esiste già

            // Crea l'entità di join
            _context.Set<ComplessoScarichiIdriciFognari>().Add(new ComplessoScarichiIdriciFognari
            {
                ComplessoId = complessoId,
                ScarichiIdriciFognariId = scarichiIdriciFognariId
            });

            // Aggiorna anche la lista di ID nell'oggetto Complesso
            if (complesso.ScarichiIdriciFognariIds == null)
                complesso.ScarichiIdriciFognariIds = new List<Guid>();

            if (!complesso.ScarichiIdriciFognariIds.Contains(scarichiIdriciFognariId))
                complesso.ScarichiIdriciFognariIds.Add(scarichiIdriciFognariId);

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveScarichiIdriciFognariFromComplessoAsync(Guid complessoId, Guid scarichiIdriciFognariId)
        {
            // Verifica che entrambe le entità esistano
            var complesso = await _dbSet.FindAsync(complessoId);
            var scarichiIdriciFognari = await _context.ScarichiIdriciFognari.FindAsync(scarichiIdriciFognariId);

            if (complesso == null || scarichiIdriciFognari == null)
                return false;

            // Trova la relazione nel database
            var relation = await _context.Set<ComplessoScarichiIdriciFognari>()
                .FirstOrDefaultAsync(cs => cs.ComplessoId == complessoId && cs.ScarichiIdriciFognariId == scarichiIdriciFognariId);

            if (relation == null)
                return true; // La relazione non esiste, quindi non c'è nulla da rimuovere

            // Rimuovi l'entità di join
            _context.Set<ComplessoScarichiIdriciFognari>().Remove(relation);

            // Aggiorna anche la lista di ID nell'oggetto Complesso
            if (complesso.ScarichiIdriciFognariIds != null && complesso.ScarichiIdriciFognariIds.Contains(scarichiIdriciFognariId))
                complesso.ScarichiIdriciFognariIds.Remove(scarichiIdriciFognariId);

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AddImpiantiElettriciToComplessoAsync(Guid complessoId, Guid impiantiElettriciId)
        {
            // Verifica che entrambe le entità esistano
            var complesso = await _dbSet.FindAsync(complessoId);
            var impiantiElettrici = await _context.ImpiantiElettrici.FindAsync(impiantiElettriciId);

            if (complesso == null || impiantiElettrici == null)
                return false;

            // Verifica se la relazione già esiste nel database
            var exists = await _context.Set<ComplessoImpiantiElettrici>()
                .AnyAsync(ci => ci.ComplessoId == complessoId && ci.ImpiantiElettriciId == impiantiElettriciId);

            if (exists)
                return true; // La relazione esiste già

            // Crea l'entità di join
            _context.Set<ComplessoImpiantiElettrici>().Add(new ComplessoImpiantiElettrici
            {
                ComplessoId = complessoId,
                ImpiantiElettriciId = impiantiElettriciId
            });

            // Aggiorna anche la lista di ID nell'oggetto Complesso
            if (complesso.ImpiantiElettriciIds == null)
                complesso.ImpiantiElettriciIds = new List<Guid>();

            if (!complesso.ImpiantiElettriciIds.Contains(impiantiElettriciId))
                complesso.ImpiantiElettriciIds.Add(impiantiElettriciId);

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveImpiantiElettriciFromComplessoAsync(Guid complessoId, Guid impiantiElettriciId)
        {
            // Verifica che entrambe le entità esistano
            var complesso = await _dbSet.FindAsync(complessoId);
            var impiantiElettrici = await _context.ImpiantiElettrici.FindAsync(impiantiElettriciId);

            if (complesso == null || impiantiElettrici == null)
                return false;

            // Trova la relazione nel database
            var relation = await _context.Set<ComplessoImpiantiElettrici>()
                .FirstOrDefaultAsync(ci => ci.ComplessoId == complessoId && ci.ImpiantiElettriciId == impiantiElettriciId);

            if (relation == null)
                return true; // La relazione non esiste, quindi non c'è nulla da rimuovere

            // Rimuovi l'entità di join
            _context.Set<ComplessoImpiantiElettrici>().Remove(relation);

            // Aggiorna anche la lista di ID nell'oggetto Complesso
            if (complesso.ImpiantiElettriciIds != null && complesso.ImpiantiElettriciIds.Contains(impiantiElettriciId))
                complesso.ImpiantiElettriciIds.Remove(impiantiElettriciId);

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AddImpiantoClimaAcsToComplessoAsync(Guid complessoId, Guid impiantoClimaAcsId)
        {
            // Verifica che entrambe le entità esistano
            var complesso = await _dbSet.FindAsync(complessoId);
            var impiantoClimaAcs = await _context.ImpiantoClimaAcs.FindAsync(impiantoClimaAcsId);

            if (complesso == null || impiantoClimaAcs == null)
                return false;

            // Verifica se la relazione già esiste nel database
            var exists = await _context.Set<ComplessoImpiantoClimaAcs>()
                .AnyAsync(ci => ci.ComplessoId == complessoId && ci.ImpiantoClimaAcsId == impiantoClimaAcsId);

            if (exists)
                return true; // La relazione esiste già

            // Crea l'entità di join
            _context.Set<ComplessoImpiantoClimaAcs>().Add(new ComplessoImpiantoClimaAcs
            {
                ComplessoId = complessoId,
                ImpiantoClimaAcsId = impiantoClimaAcsId
            });

            // Aggiorna anche la lista di ID nell'oggetto Complesso
            if (complesso.ImpiantoClimaAcsIds == null)
                complesso.ImpiantoClimaAcsIds = new List<Guid>();

            if (!complesso.ImpiantoClimaAcsIds.Contains(impiantoClimaAcsId))
                complesso.ImpiantoClimaAcsIds.Add(impiantoClimaAcsId);

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveImpiantoClimaAcsFromComplessoAsync(Guid complessoId, Guid impiantoClimaAcsId)
        {
            // Verifica che entrambe le entità esistano
            var complesso = await _dbSet.FindAsync(complessoId);
            var impiantoClimaAcs = await _context.ImpiantoClimaAcs.FindAsync(impiantoClimaAcsId);

            if (complesso == null || impiantoClimaAcs == null)
                return false;

            // Trova la relazione nel database
            var relation = await _context.Set<ComplessoImpiantoClimaAcs>()
                .FirstOrDefaultAsync(ci => ci.ComplessoId == complessoId && ci.ImpiantoClimaAcsId == impiantoClimaAcsId);

            if (relation == null)
                return true; // La relazione non esiste, quindi non c'è nulla da rimuovere

            // Rimuovi l'entità di join
            _context.Set<ComplessoImpiantoClimaAcs>().Remove(relation);

            // Aggiorna anche la lista di ID nell'oggetto Complesso
            if (complesso.ImpiantoClimaAcsIds != null && complesso.ImpiantoClimaAcsIds.Contains(impiantoClimaAcsId))
                complesso.ImpiantoClimaAcsIds.Remove(impiantoClimaAcsId);

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AddAltriImpiantiToComplessoAsync(Guid complessoId, Guid altriImpiantiId)
        {
            // Verifica che entrambe le entità esistano
            var complesso = await _dbSet.FindAsync(complessoId);
            var altriImpianti = await _context.AltriImpianti.FindAsync(altriImpiantiId);

            if (complesso == null || altriImpianti == null)
                return false;

            // Verifica se la relazione già esiste nel database
            var exists = await _context.Set<ComplessoAltriImpianti>()
                .AnyAsync(ca => ca.ComplessoId == complessoId && ca.AltriImpiantiId == altriImpiantiId);

            if (exists)
                return true; // La relazione esiste già

            // Crea l'entità di join
            _context.Set<ComplessoAltriImpianti>().Add(new ComplessoAltriImpianti
            {
                ComplessoId = complessoId,
                AltriImpiantiId = altriImpiantiId
            });

            // Aggiorna anche la lista di ID nell'oggetto Complesso
            if (complesso.AltriImpiantiIds == null)
                complesso.AltriImpiantiIds = new List<Guid>();

            if (!complesso.AltriImpiantiIds.Contains(altriImpiantiId))
                complesso.AltriImpiantiIds.Add(altriImpiantiId);

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveAltriImpiantiFromComplessoAsync(Guid complessoId, Guid altriImpiantiId)
        {
            // Verifica che entrambe le entità esistano
            var complesso = await _dbSet.FindAsync(complessoId);
            var altriImpianti = await _context.AltriImpianti.FindAsync(altriImpiantiId);

            if (complesso == null || altriImpianti == null)
                return false;

            // Trova la relazione nel database
            var relation = await _context.Set<ComplessoAltriImpianti>()
                .FirstOrDefaultAsync(ca => ca.ComplessoId == complessoId && ca.AltriImpiantiId == altriImpiantiId);

            if (relation == null)
                return true; // La relazione non esiste, quindi non c'è nulla da rimuovere

            // Rimuovi l'entità di join
            _context.Set<ComplessoAltriImpianti>().Remove(relation);

            // Aggiorna anche la lista di ID nell'oggetto Complesso
            if (complesso.AltriImpiantiIds != null && complesso.AltriImpiantiIds.Contains(altriImpiantiId))
                complesso.AltriImpiantiIds.Remove(altriImpiantiId);

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AddDocumentiGeneraliToComplessoAsync(Guid complessoId, Guid documentiGeneraliId)
        {
            // Verifica che entrambe le entità esistano
            var complesso = await _dbSet.FindAsync(complessoId);
            var documentiGenerali = await _context.DocumentiGenerali.FindAsync(documentiGeneraliId);

            if (complesso == null || documentiGenerali == null)
                return false;

            // Verifica se la relazione già esiste nel database
            var exists = await _context.Set<ComplessoDocumentiGenerali>()
                .AnyAsync(cd => cd.ComplessoId == complessoId && cd.DocumentiGeneraliId == documentiGeneraliId);

            if (exists)
                return true; // La relazione esiste già

            // Crea l'entità di join
            _context.Set<ComplessoDocumentiGenerali>().Add(new ComplessoDocumentiGenerali
            {
                ComplessoId = complessoId,
                DocumentiGeneraliId = documentiGeneraliId
            });

            // Aggiorna anche la lista di ID nell'oggetto Complesso
            if (complesso.DocumentiGeneraliIds == null)
                complesso.DocumentiGeneraliIds = new List<Guid>();

            if (!complesso.DocumentiGeneraliIds.Contains(documentiGeneraliId))
                complesso.DocumentiGeneraliIds.Add(documentiGeneraliId);

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveDocumentiGeneraliFromComplessoAsync(Guid complessoId, Guid documentiGeneraliId)
        {
            // Verifica che entrambe le entità esistano
            var complesso = await _dbSet.FindAsync(complessoId);
            var documentiGenerali = await _context.DocumentiGenerali.FindAsync(documentiGeneraliId);

            if (complesso == null || documentiGenerali == null)
                return false;

            // Trova la relazione nel database
            var relation = await _context.Set<ComplessoDocumentiGenerali>()
                .FirstOrDefaultAsync(cd => cd.ComplessoId == complessoId && cd.DocumentiGeneraliId == documentiGeneraliId);

            if (relation == null)
                return true; // La relazione non esiste, quindi non c'è nulla da rimuovere

            // Rimuovi l'entità di join
            _context.Set<ComplessoDocumentiGenerali>().Remove(relation);

            // Aggiorna anche la lista di ID nell'oggetto Complesso
            if (complesso.DocumentiGeneraliIds != null && complesso.DocumentiGeneraliIds.Contains(documentiGeneraliId))
                complesso.DocumentiGeneraliIds.Remove(documentiGeneraliId);

            await _context.SaveChangesAsync();
            return true;
        }
    }
}