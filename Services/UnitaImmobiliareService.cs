using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Services
{
    public class UnitaImmobiliareService : EntityService<UnitaImmobiliare>, IUnitaImmobiliareService
    {
        public UnitaImmobiliareService(ApplicationDbContext context) : base(context)
        {
        }

        public override async Task<UnitaImmobiliare> GetByIdAsync(Guid id)
        {
            return await _dbSet
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        // Infissi
        public async Task<bool> AddInfissiToUnitaImmobiliareAsync(Guid unitaImmobiliareId, Guid infissiId)
        {
            // Verifica che entrambe le entità esistano
            var unitaImmobiliare = await _dbSet.FindAsync(unitaImmobiliareId);
            var infissi = await _context.Infissi.FindAsync(infissiId);

            if (unitaImmobiliare == null || infissi == null)
                return false;

            // Verifica se la relazione già esiste nel database
            var exists = await _context.Set<UnitaImmobiliareInfissi>()
                .AnyAsync(ui => ui.UnitaImmobiliareId == unitaImmobiliareId && ui.InfissiId == infissiId);

            if (exists)
                return true; // La relazione esiste già

            // Crea l'entità di join
            _context.Set<UnitaImmobiliareInfissi>().Add(new UnitaImmobiliareInfissi
            {
                UnitaImmobiliareId = unitaImmobiliareId,
                InfissiId = infissiId
            });

            // Aggiorna anche la lista di ID nell'oggetto UnitaImmobiliare
            if (unitaImmobiliare.InfissiIds == null)
                unitaImmobiliare.InfissiIds = new List<Guid>();

            if (!unitaImmobiliare.InfissiIds.Contains(infissiId))
                unitaImmobiliare.InfissiIds.Add(infissiId);

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveInfissiFromUnitaImmobiliareAsync(Guid unitaImmobiliareId, Guid infissiId)
        {
            // Verifica che entrambe le entità esistano
            var unitaImmobiliare = await _dbSet.FindAsync(unitaImmobiliareId);
            var infissi = await _context.Infissi.FindAsync(infissiId);

            if (unitaImmobiliare == null || infissi == null)
                return false;

            // Trova la relazione nel database
            var relation = await _context.Set<UnitaImmobiliareInfissi>()
                .FirstOrDefaultAsync(ui => ui.UnitaImmobiliareId == unitaImmobiliareId && ui.InfissiId == infissiId);

            if (relation == null)
                return true; // La relazione non esiste, quindi non c'è nulla da rimuovere

            // Rimuovi l'entità di join
            _context.Set<UnitaImmobiliareInfissi>().Remove(relation);

            // Aggiorna anche la lista di ID nell'oggetto UnitaImmobiliare
            if (unitaImmobiliare.InfissiIds != null && unitaImmobiliare.InfissiIds.Contains(infissiId))
                unitaImmobiliare.InfissiIds.Remove(infissiId);

            await _context.SaveChangesAsync();
            return true;
        }

        // IdraulicoAdduzione
        public async Task<bool> AddIdraulicoAdduzioneToUnitaImmobiliareAsync(Guid unitaImmobiliareId, Guid idraulicoAdduzioneId)
        {
            // Verifica che entrambe le entità esistano
            var unitaImmobiliare = await _dbSet.FindAsync(unitaImmobiliareId);
            var idraulicoAdduzione = await _context.IdraulicoAdduzione.FindAsync(idraulicoAdduzioneId);

            if (unitaImmobiliare == null || idraulicoAdduzione == null)
                return false;

            // Verifica se la relazione già esiste nel database
            var exists = await _context.Set<UnitaImmobiliareIdraulicoAdduzione>()
                .AnyAsync(uia => uia.UnitaImmobiliareId == unitaImmobiliareId && uia.IdraulicoAdduzioneId == idraulicoAdduzioneId);

            if (exists)
                return true; // La relazione esiste già

            // Crea l'entità di join
            _context.Set<UnitaImmobiliareIdraulicoAdduzione>().Add(new UnitaImmobiliareIdraulicoAdduzione
            {
                UnitaImmobiliareId = unitaImmobiliareId,
                IdraulicoAdduzioneId = idraulicoAdduzioneId
            });

            // Aggiorna anche la lista di ID nell'oggetto UnitaImmobiliare
            if (unitaImmobiliare.IdraulicoAdduzioneIds == null)
                unitaImmobiliare.IdraulicoAdduzioneIds = new List<Guid>();

            if (!unitaImmobiliare.IdraulicoAdduzioneIds.Contains(idraulicoAdduzioneId))
                unitaImmobiliare.IdraulicoAdduzioneIds.Add(idraulicoAdduzioneId);

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveIdraulicoAdduzioneFromUnitaImmobiliareAsync(Guid unitaImmobiliareId, Guid idraulicoAdduzioneId)
        {
            // Implementazione simile a RemoveInfissiFromUnitaImmobiliareAsync
            var unitaImmobiliare = await _dbSet.FindAsync(unitaImmobiliareId);
            var idraulicoAdduzione = await _context.IdraulicoAdduzione.FindAsync(idraulicoAdduzioneId);

            if (unitaImmobiliare == null || idraulicoAdduzione == null)
                return false;

            var relation = await _context.Set<UnitaImmobiliareIdraulicoAdduzione>()
                .FirstOrDefaultAsync(uia => uia.UnitaImmobiliareId == unitaImmobiliareId && uia.IdraulicoAdduzioneId == idraulicoAdduzioneId);

            if (relation == null)
                return true;

            _context.Set<UnitaImmobiliareIdraulicoAdduzione>().Remove(relation);

            if (unitaImmobiliare.IdraulicoAdduzioneIds != null && unitaImmobiliare.IdraulicoAdduzioneIds.Contains(idraulicoAdduzioneId))
                unitaImmobiliare.IdraulicoAdduzioneIds.Remove(idraulicoAdduzioneId);

            await _context.SaveChangesAsync();
            return true;
        }

        // ScarichiIdriciFognari
        public async Task<bool> AddScarichiIdriciFognariToUnitaImmobiliareAsync(Guid unitaImmobiliareId, Guid scarichiIdriciFognariId)
        {
            // Implementazione simile a AddInfissiToUnitaImmobiliareAsync
            var unitaImmobiliare = await _dbSet.FindAsync(unitaImmobiliareId);
            var scarichiIdriciFognari = await _context.ScarichiIdriciFognari.FindAsync(scarichiIdriciFognariId);

            if (unitaImmobiliare == null || scarichiIdriciFognari == null)
                return false;

            var exists = await _context.Set<UnitaImmobiliareScarichiIdriciFognari>()
                .AnyAsync(usif => usif.UnitaImmobiliareId == unitaImmobiliareId && usif.ScarichiIdriciFognariId == scarichiIdriciFognariId);

            if (exists)
                return true;

            _context.Set<UnitaImmobiliareScarichiIdriciFognari>().Add(new UnitaImmobiliareScarichiIdriciFognari
            {
                UnitaImmobiliareId = unitaImmobiliareId,
                ScarichiIdriciFognariId = scarichiIdriciFognariId
            });

            if (unitaImmobiliare.ScarichiIdriciFognariIds == null)
                unitaImmobiliare.ScarichiIdriciFognariIds = new List<Guid>();

            if (!unitaImmobiliare.ScarichiIdriciFognariIds.Contains(scarichiIdriciFognariId))
                unitaImmobiliare.ScarichiIdriciFognariIds.Add(scarichiIdriciFognariId);

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveScarichiIdriciFognariFromUnitaImmobiliareAsync(Guid unitaImmobiliareId, Guid scarichiIdriciFognariId)
        {
            // Implementazione simile a RemoveInfissiFromUnitaImmobiliareAsync
            var unitaImmobiliare = await _dbSet.FindAsync(unitaImmobiliareId);
            var scarichiIdriciFognari = await _context.ScarichiIdriciFognari.FindAsync(scarichiIdriciFognariId);

            if (unitaImmobiliare == null || scarichiIdriciFognari == null)
                return false;

            var relation = await _context.Set<UnitaImmobiliareScarichiIdriciFognari>()
                .FirstOrDefaultAsync(usif => usif.UnitaImmobiliareId == unitaImmobiliareId && usif.ScarichiIdriciFognariId == scarichiIdriciFognariId);

            if (relation == null)
                return true;

            _context.Set<UnitaImmobiliareScarichiIdriciFognari>().Remove(relation);

            if (unitaImmobiliare.ScarichiIdriciFognariIds != null && unitaImmobiliare.ScarichiIdriciFognariIds.Contains(scarichiIdriciFognariId))
                unitaImmobiliare.ScarichiIdriciFognariIds.Remove(scarichiIdriciFognariId);

            await _context.SaveChangesAsync();
            return true;
        }

        // ImpiantiElettrici, ImpiantoClimaAcs, AltriImpianti, DocumentiGenerali
        // Implementare gli altri metodi seguendo lo stesso pattern
        public async Task<bool> AddImpiantiElettriciToUnitaImmobiliareAsync(Guid unitaImmobiliareId, Guid impiantiElettriciId)
        {
            // Implementazione simile a AddInfissiToUnitaImmobiliareAsync
            var unitaImmobiliare = await _dbSet.FindAsync(unitaImmobiliareId);
            var impiantiElettrici = await _context.ImpiantiElettrici.FindAsync(impiantiElettriciId);

            if (unitaImmobiliare == null || impiantiElettrici == null)
                return false;

            var exists = await _context.Set<UnitaImmobiliareImpiantiElettrici>()
                .AnyAsync(uie => uie.UnitaImmobiliareId == unitaImmobiliareId && uie.ImpiantiElettriciId == impiantiElettriciId);

            if (exists)
                return true;

            _context.Set<UnitaImmobiliareImpiantiElettrici>().Add(new UnitaImmobiliareImpiantiElettrici
            {
                UnitaImmobiliareId = unitaImmobiliareId,
                ImpiantiElettriciId = impiantiElettriciId
            });

            if (unitaImmobiliare.ImpiantiElettriciIds == null)
                unitaImmobiliare.ImpiantiElettriciIds = new List<Guid>();

            if (!unitaImmobiliare.ImpiantiElettriciIds.Contains(impiantiElettriciId))
                unitaImmobiliare.ImpiantiElettriciIds.Add(impiantiElettriciId);

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveImpiantiElettriciFromUnitaImmobiliareAsync(Guid unitaImmobiliareId, Guid impiantiElettriciId)
        {
            // Implementazione simile a RemoveInfissiFromUnitaImmobiliareAsync
            var unitaImmobiliare = await _dbSet.FindAsync(unitaImmobiliareId);
            var impiantiElettrici = await _context.ImpiantiElettrici.FindAsync(impiantiElettriciId);

            if (unitaImmobiliare == null || impiantiElettrici == null)
                return false;

            var relation = await _context.Set<UnitaImmobiliareImpiantiElettrici>()
                .FirstOrDefaultAsync(uie => uie.UnitaImmobiliareId == unitaImmobiliareId && uie.ImpiantiElettriciId == impiantiElettriciId);

            if (relation == null)
                return true;

            _context.Set<UnitaImmobiliareImpiantiElettrici>().Remove(relation);

            if (unitaImmobiliare.ImpiantiElettriciIds != null && unitaImmobiliare.ImpiantiElettriciIds.Contains(impiantiElettriciId))
                unitaImmobiliare.ImpiantiElettriciIds.Remove(impiantiElettriciId);

            await _context.SaveChangesAsync();
            return true;
        }

        // Implementazioni simili per gli altri metodi...
        public async Task<bool> AddImpiantoClimaAcsToUnitaImmobiliareAsync(Guid unitaImmobiliareId, Guid impiantoClimaAcsId)
        {
            // Implementazione...
            var unitaImmobiliare = await _dbSet.FindAsync(unitaImmobiliareId);
            var impiantoClimaAcs = await _context.ImpiantoClimaAcs.FindAsync(impiantoClimaAcsId);

            if (unitaImmobiliare == null || impiantoClimaAcs == null)
                return false;

            var exists = await _context.Set<UnitaImmobiliareImpiantoClimaAcs>()
                .AnyAsync(uica => uica.UnitaImmobiliareId == unitaImmobiliareId && uica.ImpiantoClimaAcsId == impiantoClimaAcsId);

            if (exists)
                return true;

            _context.Set<UnitaImmobiliareImpiantoClimaAcs>().Add(new UnitaImmobiliareImpiantoClimaAcs
            {
                UnitaImmobiliareId = unitaImmobiliareId,
                ImpiantoClimaAcsId = impiantoClimaAcsId
            });

            if (unitaImmobiliare.ImpiantoClimaAcsIds == null)
                unitaImmobiliare.ImpiantoClimaAcsIds = new List<Guid>();

            if (!unitaImmobiliare.ImpiantoClimaAcsIds.Contains(impiantoClimaAcsId))
                unitaImmobiliare.ImpiantoClimaAcsIds.Add(impiantoClimaAcsId);

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveImpiantoClimaAcsFromUnitaImmobiliareAsync(Guid unitaImmobiliareId, Guid impiantoClimaAcsId)
        {
            // Implementazione...
            var unitaImmobiliare = await _dbSet.FindAsync(unitaImmobiliareId);
            var impiantoClimaAcs = await _context.ImpiantoClimaAcs.FindAsync(impiantoClimaAcsId);

            if (unitaImmobiliare == null || impiantoClimaAcs == null)
                return false;

            var relation = await _context.Set<UnitaImmobiliareImpiantoClimaAcs>()
                .FirstOrDefaultAsync(uica => uica.UnitaImmobiliareId == unitaImmobiliareId && uica.ImpiantoClimaAcsId == impiantoClimaAcsId);

            if (relation == null)
                return true;

            _context.Set<UnitaImmobiliareImpiantoClimaAcs>().Remove(relation);

            if (unitaImmobiliare.ImpiantoClimaAcsIds != null && unitaImmobiliare.ImpiantoClimaAcsIds.Contains(impiantoClimaAcsId))
                unitaImmobiliare.ImpiantoClimaAcsIds.Remove(impiantoClimaAcsId);

            await _context.SaveChangesAsync();
            return true;
        }

        // AltriImpianti e DocumentiGenerali implementati in modo simile
        public async Task<bool> AddAltriImpiantiToUnitaImmobiliareAsync(Guid unitaImmobiliareId, Guid altriImpiantiId)
        {
            // Implementazione...
            var unitaImmobiliare = await _dbSet.FindAsync(unitaImmobiliareId);
            var altriImpianti = await _context.AltriImpianti.FindAsync(altriImpiantiId);

            if (unitaImmobiliare == null || altriImpianti == null)
                return false;

            var exists = await _context.Set<UnitaImmobiliareAltriImpianti>()
                .AnyAsync(uai => uai.UnitaImmobiliareId == unitaImmobiliareId && uai.AltriImpiantiId == altriImpiantiId);

            if (exists)
                return true;

            _context.Set<UnitaImmobiliareAltriImpianti>().Add(new UnitaImmobiliareAltriImpianti
            {
                UnitaImmobiliareId = unitaImmobiliareId,
                AltriImpiantiId = altriImpiantiId
            });

            if (unitaImmobiliare.AltriImpiantiIds == null)
                unitaImmobiliare.AltriImpiantiIds = new List<Guid>();

            if (!unitaImmobiliare.AltriImpiantiIds.Contains(altriImpiantiId))
                unitaImmobiliare.AltriImpiantiIds.Add(altriImpiantiId);

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveAltriImpiantiFromUnitaImmobiliareAsync(Guid unitaImmobiliareId, Guid altriImpiantiId)
        {
            // Implementazione...
            var unitaImmobiliare = await _dbSet.FindAsync(unitaImmobiliareId);
            var altriImpianti = await _context.AltriImpianti.FindAsync(altriImpiantiId);

            if (unitaImmobiliare == null || altriImpianti == null)
                return false;

            var relation = await _context.Set<UnitaImmobiliareAltriImpianti>()
                .FirstOrDefaultAsync(uai => uai.UnitaImmobiliareId == unitaImmobiliareId && uai.AltriImpiantiId == altriImpiantiId);

            if (relation == null)
                return true;

            _context.Set<UnitaImmobiliareAltriImpianti>().Remove(relation);

            if (unitaImmobiliare.AltriImpiantiIds != null && unitaImmobiliare.AltriImpiantiIds.Contains(altriImpiantiId))
                unitaImmobiliare.AltriImpiantiIds.Remove(altriImpiantiId);

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AddDocumentiGeneraliToUnitaImmobiliareAsync(Guid unitaImmobiliareId, Guid documentiGeneraliId)
        {
            // Implementazione...
            var unitaImmobiliare = await _dbSet.FindAsync(unitaImmobiliareId);
            var documentiGenerali = await _context.DocumentiGenerali.FindAsync(documentiGeneraliId);

            if (unitaImmobiliare == null || documentiGenerali == null)
                return false;

            var exists = await _context.Set<UnitaImmobiliareDocumentiGenerali>()
                .AnyAsync(udg => udg.UnitaImmobiliareId == unitaImmobiliareId && udg.DocumentiGeneraliId == documentiGeneraliId);

            if (exists)
                return true;

            _context.Set<UnitaImmobiliareDocumentiGenerali>().Add(new UnitaImmobiliareDocumentiGenerali
            {
                UnitaImmobiliareId = unitaImmobiliareId,
                DocumentiGeneraliId = documentiGeneraliId
            });

            if (unitaImmobiliare.DocumentiGeneraliIds == null)
                unitaImmobiliare.DocumentiGeneraliIds = new List<Guid>();

            if (!unitaImmobiliare.DocumentiGeneraliIds.Contains(documentiGeneraliId))
                unitaImmobiliare.DocumentiGeneraliIds.Add(documentiGeneraliId);

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveDocumentiGeneraliFromUnitaImmobiliareAsync(Guid unitaImmobiliareId, Guid documentiGeneraliId)
        {
            // Implementazione...
            var unitaImmobiliare = await _dbSet.FindAsync(unitaImmobiliareId);
            var documentiGenerali = await _context.DocumentiGenerali.FindAsync(documentiGeneraliId);

            if (unitaImmobiliare == null || documentiGenerali == null)
                return false;

            var relation = await _context.Set<UnitaImmobiliareDocumentiGenerali>()
                .FirstOrDefaultAsync(udg => udg.UnitaImmobiliareId == unitaImmobiliareId && udg.DocumentiGeneraliId == documentiGeneraliId);

            if (relation == null)
                return true;

            _context.Set<UnitaImmobiliareDocumentiGenerali>().Remove(relation);

            if (unitaImmobiliare.DocumentiGeneraliIds != null && unitaImmobiliare.DocumentiGeneraliIds.Contains(documentiGeneraliId))
                unitaImmobiliare.DocumentiGeneraliIds.Remove(documentiGeneraliId);

            await _context.SaveChangesAsync();
            return true;
        }
    }
}