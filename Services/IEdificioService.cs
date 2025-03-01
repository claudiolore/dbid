using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Models;

namespace Services
{
    public interface IEdificioService : IEntityService<Edificio>
    {
        Task<IEnumerable<UnitaImmobiliare>> GetUnitaImmobiliariByEdificioIdAsync(Guid edificioId);
    }
} 