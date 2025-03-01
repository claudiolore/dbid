using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Models;

namespace Services
{
    public interface IComplessoService : IEntityService<Complesso>
    {
        Task<IEnumerable<Edificio>> GetEdificiByComplessoIdAsync(Guid complessoId);
        Task<bool> AddStrutturaToComplessoAsync(Guid complessoId, Guid strutturaId);
        Task<bool> RemoveStrutturaFromComplessoAsync(Guid complessoId, Guid strutturaId);

        // IdraulicoAdduzione
        Task<bool> AddIdraulicoAdduzioneToComplessoAsync(Guid complessoId, Guid idraulicoAdduzioneId);
        Task<bool> RemoveIdraulicoAdduzioneFromComplessoAsync(Guid complessoId, Guid idraulicoAdduzioneId);

        // ScarichiIdriciFognari
        Task<bool> AddScarichiIdriciFognariToComplessoAsync(Guid complessoId, Guid scarichiIdriciFognariId);
        Task<bool> RemoveScarichiIdriciFognariFromComplessoAsync(Guid complessoId, Guid scarichiIdriciFognariId);

        // ImpiantiElettrici
        Task<bool> AddImpiantiElettriciToComplessoAsync(Guid complessoId, Guid impiantiElettriciId);
        Task<bool> RemoveImpiantiElettriciFromComplessoAsync(Guid complessoId, Guid impiantiElettriciId);

        // ImpiantoClimaAcs
        Task<bool> AddImpiantoClimaAcsToComplessoAsync(Guid complessoId, Guid impiantoClimaAcsId);
        Task<bool> RemoveImpiantoClimaAcsFromComplessoAsync(Guid complessoId, Guid impiantoClimaAcsId);

        // AltriImpianti
        Task<bool> AddAltriImpiantiToComplessoAsync(Guid complessoId, Guid altriImpiantiId);
        Task<bool> RemoveAltriImpiantiFromComplessoAsync(Guid complessoId, Guid altriImpiantiId);

        // DocumentiGenerali
        Task<bool> AddDocumentiGeneraliToComplessoAsync(Guid complessoId, Guid documentiGeneraliId);
        Task<bool> RemoveDocumentiGeneraliFromComplessoAsync(Guid complessoId, Guid documentiGeneraliId);
    }
}