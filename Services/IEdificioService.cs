using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Models;

namespace Services
{
    public interface IEdificioService : IEntityService<Edificio>
    {
        Task<IEnumerable<UnitaImmobiliare>> GetUnitaImmobiliariByEdificioIdAsync(Guid edificioId);
        Task<bool> AddStrutturaToEdificioAsync(Guid edificioId, Guid strutturaId);
        Task<bool> RemoveStrutturaFromEdificioAsync(Guid edificioId, Guid strutturaId);
        Task<bool> AddInfissiToEdificioAsync(Guid edificioId, Guid infissiId);
        Task<bool> RemoveInfissiFromEdificioAsync(Guid edificioId, Guid infissiId);
        Task<bool> AddIdraulicoAdduzioneToEdificioAsync(Guid edificioId, Guid idraulicoAdduzioneId);
        Task<bool> RemoveIdraulicoAdduzioneFromEdificioAsync(Guid edificioId, Guid idraulicoAdduzioneId);
        Task<bool> AddScarichiIdriciFognariToEdificioAsync(Guid edificioId, Guid scarichiIdriciFognariId);
        Task<bool> RemoveScarichiIdriciFognariFromEdificioAsync(Guid edificioId, Guid scarichiIdriciFognariId);
        Task<bool> AddImpiantiElettriciToEdificioAsync(Guid edificioId, Guid impiantiElettriciId);
        Task<bool> RemoveImpiantiElettriciFromEdificioAsync(Guid edificioId, Guid impiantiElettriciId);
        Task<bool> AddImpiantoClimaAcsToEdificioAsync(Guid edificioId, Guid impiantoClimaAcsId);
        Task<bool> RemoveImpiantoClimaAcsFromEdificioAsync(Guid edificioId, Guid impiantoClimaAcsId);
        Task<bool> AddAltriImpiantiToEdificioAsync(Guid edificioId, Guid altriImpiantiId);
        Task<bool> RemoveAltriImpiantiFromEdificioAsync(Guid edificioId, Guid altriImpiantiId);
        Task<bool> AddDocumentiGeneraliToEdificioAsync(Guid edificioId, Guid documentiGeneraliId);
        Task<bool> RemoveDocumentiGeneraliFromEdificioAsync(Guid edificioId, Guid documentiGeneraliId);
    }
}