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
    }
}