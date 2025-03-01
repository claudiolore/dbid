using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Services
{
    public class EdificioService : EntityService<Edificio>, IEdificioService
    {
        public EdificioService(ApplicationDbContext context) : base(context)
        {
        }

        public override async Task<Edificio> GetByIdAsync(Guid id)
        {
            return await _dbSet
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<IEnumerable<UnitaImmobiliare>> GetUnitaImmobiliariByEdificioIdAsync(Guid edificioId)
        {
            return await _context.UnitaImmobiliari
                .Where(u => EF.Property<Guid>(u, "EdificioId") == edificioId)
                .ToListAsync();
        }
    }
} 