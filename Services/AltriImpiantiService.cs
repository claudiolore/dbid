using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Services
{
    public class AltriImpiantiService : EntityService<AltriImpianti>, IAltriImpiantiService
    {
        public AltriImpiantiService(ApplicationDbContext context) : base(context)
        {
        }

        public override async Task<AltriImpianti> GetByIdAsync(Guid id)
        {
            return await _dbSet
                .FirstOrDefaultAsync(a => a.Id == id);
        }
    }
} 