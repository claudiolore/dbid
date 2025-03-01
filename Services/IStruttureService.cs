using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Models;

namespace Services
{
    public interface IStruttureService : IEntityService<Strutture>
    {
        Task<IEnumerable<SegnalazioneProblema>> GetSegnalazioniByStrutturaIdAsync(Guid strutturaId);
    }
}