using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Services
{
    public class IdraulicoAdduzioneService : EntityService<IdraulicoAdduzione>, IIdraulicoAdduzioneService
    {
        public IdraulicoAdduzioneService(ApplicationDbContext context) : base(context)
        {
        }

        public override async Task<IdraulicoAdduzione> GetByIdAsync(Guid id)
        {
            return await _dbSet
                .FirstOrDefaultAsync(i => i.Id == id);
        }
    }
}