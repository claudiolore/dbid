using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Models;

namespace Services
{
    public interface ISegnalazioneProblemaService : IEntityService<SegnalazioneProblema>
    {
        Task<IEnumerable<SegnalazioneProblema>> GetByStrutturaIdAsync(Guid strutturaId);
    }
} 