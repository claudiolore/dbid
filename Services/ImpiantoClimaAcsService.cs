using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Services
{
    public class ImpiantoClimaAcsService : EntityService<ImpiantoClimaAcs>, IImpiantoClimaAcsService
    {
        public ImpiantoClimaAcsService(ApplicationDbContext context) : base(context)
        {
        }

        public override async Task<ImpiantoClimaAcs> GetByIdAsync(Guid id)
        {
            return await _dbSet
                .FirstOrDefaultAsync(i => i.Id == id);
        }
    }
}