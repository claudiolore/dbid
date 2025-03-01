using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Services
{
    public class ComplessoService : EntityService<Complesso>, IComplessoService
    {
        public ComplessoService(ApplicationDbContext context) : base(context)
        {
        }

        public override async Task<Complesso> GetByIdAsync(Guid id)
        {
            return await _dbSet
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<Edificio>> GetEdificiByComplessoIdAsync(Guid complessoId)
        {
            return await _context.Edifici
                .Where(e => EF.Property<Guid>(e, "ComplessoId") == complessoId)
                .ToListAsync();
        }
    }
} 