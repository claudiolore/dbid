using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Models;

namespace Services
{
    public interface IUnitaImmobiliareService : IEntityService<UnitaImmobiliare>
    {
        // Infissi
        Task<bool> AddInfissiToUnitaImmobiliareAsync(Guid unitaImmobiliareId, Guid infissiId);
        Task<bool> RemoveInfissiFromUnitaImmobiliareAsync(Guid unitaImmobiliareId, Guid infissiId);

        // IdraulicoAdduzione
        Task<bool> AddIdraulicoAdduzioneToUnitaImmobiliareAsync(Guid unitaImmobiliareId, Guid idraulicoAdduzioneId);
        Task<bool> RemoveIdraulicoAdduzioneFromUnitaImmobiliareAsync(Guid unitaImmobiliareId, Guid idraulicoAdduzioneId);

        // ScarichiIdriciFognari
        Task<bool> AddScarichiIdriciFognariToUnitaImmobiliareAsync(Guid unitaImmobiliareId, Guid scarichiIdriciFognariId);
        Task<bool> RemoveScarichiIdriciFognariFromUnitaImmobiliareAsync(Guid unitaImmobiliareId, Guid scarichiIdriciFognariId);

        // ImpiantiElettrici
        Task<bool> AddImpiantiElettriciToUnitaImmobiliareAsync(Guid unitaImmobiliareId, Guid impiantiElettriciId);
        Task<bool> RemoveImpiantiElettriciFromUnitaImmobiliareAsync(Guid unitaImmobiliareId, Guid impiantiElettriciId);

        // ImpiantoClimaAcs
        Task<bool> AddImpiantoClimaAcsToUnitaImmobiliareAsync(Guid unitaImmobiliareId, Guid impiantoClimaAcsId);
        Task<bool> RemoveImpiantoClimaAcsFromUnitaImmobiliareAsync(Guid unitaImmobiliareId, Guid impiantoClimaAcsId);

        // AltriImpianti
        Task<bool> AddAltriImpiantiToUnitaImmobiliareAsync(Guid unitaImmobiliareId, Guid altriImpiantiId);
        Task<bool> RemoveAltriImpiantiFromUnitaImmobiliareAsync(Guid unitaImmobiliareId, Guid altriImpiantiId);

        // DocumentiGenerali
        Task<bool> AddDocumentiGeneraliToUnitaImmobiliareAsync(Guid unitaImmobiliareId, Guid documentiGeneraliId);
        Task<bool> RemoveDocumentiGeneraliFromUnitaImmobiliareAsync(Guid unitaImmobiliareId, Guid documentiGeneraliId);
    }
}