using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Models;

namespace Services
{
    public interface IComplessoService : IEntityService<Complesso>
    {
        Task<IEnumerable<Edificio>> GetEdificiByComplessoIdAsync(Guid complessoId);
    }
} 