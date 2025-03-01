using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Services
{
    public class ScarichiIdriciFognariService : EntityService<ScarichiIdriciFognari>, IScarichiIdriciFognariService
    {
        public ScarichiIdriciFognariService(ApplicationDbContext context) : base(context)
        {
        }

        public override async Task<ScarichiIdriciFognari> GetByIdAsync(Guid id)
        {
            return await _dbSet
                .FirstOrDefaultAsync(s => s.Id == id);
        }
    }
}