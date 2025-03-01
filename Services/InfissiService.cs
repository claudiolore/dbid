using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Services
{
    public class InfissiService : EntityService<Infissi>, IInfissiService
    {
        public InfissiService(ApplicationDbContext context) : base(context)
        {
        }

        public override async Task<Infissi> GetByIdAsync(Guid id)
        {
            return await _dbSet
                .FirstOrDefaultAsync(i => i.Id == id);
        }
    }
}