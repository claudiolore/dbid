using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Services
{
    public class DocumentiGeneraliService : EntityService<DocumentiGenerali>, IDocumentiGeneraliService
    {
        public DocumentiGeneraliService(ApplicationDbContext context) : base(context)
        {
        }

        public override async Task<DocumentiGenerali> GetByIdAsync(Guid id)
        {
            return await _dbSet
                .FirstOrDefaultAsync(d => d.Id == id);
        }
    }
} 