using System.Collections.Generic;
using System.Threading.Tasks;
using Karia.Api.DbContexts;
using Karia.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace Karia.Api.Services
{
    public class KariaRepository:IKariaRepository
    {
        private readonly KariaDbContext _context;

        public KariaRepository(KariaDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Category>> GetCategories()
        {
            var categories=await _context.Categories.ToListAsync();
            return categories;
        }
    }
}