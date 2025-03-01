using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Services
{
    public class StruttureService : EntityService<Strutture>, IStruttureService
    {
        public StruttureService(ApplicationDbContext context) : base(context)
        {
        }

        public override async Task<Strutture> GetByIdAsync(Guid id)
        {
            return await _dbSet
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<IEnumerable<SegnalazioneProblema>> GetSegnalazioniByStrutturaIdAsync(Guid strutturaId)
        {
            return await _context.SegnalazioneProblema
                .Where(s => EF.Property<Guid>(s, "StruttureId") == strutturaId)
                .ToListAsync();
        }
    }
}