using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Services
{
    public class SegnalazioneProblemaService : EntityService<SegnalazioneProblema>, ISegnalazioneProblemaService
    {
        public SegnalazioneProblemaService(ApplicationDbContext context) : base(context)
        {
        }

        public override async Task<SegnalazioneProblema> GetByIdAsync(Guid id)
        {
            return await _dbSet
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<IEnumerable<SegnalazioneProblema>> GetByStrutturaIdAsync(Guid strutturaId)
        {
            return await _dbSet
                .Where(s => EF.Property<Guid>(s, "StruttureId") == strutturaId)
                .ToListAsync();
        }
    }
} 