using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Karia.Api.DbContexts;
using Karia.Api.Entities;
using Karia.Api.Helpers;
using Karia.Api.ResourceParameters;
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

        public async Task<bool> ExistsCategory(Guid categoryId)
        {
            return await _context.Categories.AnyAsync(c => c.Id == categoryId);
        }

        
        public async Task<IEnumerable<Expert>> GetExperts(Guid categoryId, ExpertsResourceParameters expertsResourceParameters)
        {
            var experts = _context.Groupings
                .Where(g => g.CategoryId == categoryId)
                .Include(g => g.Expert)
                .Select(g => g.Expert);

            #region Filters and Searching
            experts = experts.WhereIf(expertsResourceParameters.IsMaster,
                e => e.IsMaster==true);
            experts = experts.WhereIf(expertsResourceParameters.IsHaveVehicle,
                e => e.IsHaveVehicle == true);
            experts = experts.WhereIf(!(String.IsNullOrWhiteSpace(expertsResourceParameters.SearchQuery)),
                e => e.FirstName.Contains(expertsResourceParameters.SearchQuery)
                     || e.LastName.Contains(expertsResourceParameters.SearchQuery)
                     || e.Description.Contains(expertsResourceParameters.SearchQuery)
                     || e.Orientation.Contains(expertsResourceParameters.SearchQuery));

            #endregion

            experts = ApplySort(experts,expertsResourceParameters.OrderBy);
            
            return await PagedList<Expert>.Create(experts,
                expertsResourceParameters.PageNumber,
                expertsResourceParameters.PageSize);

        }

        private IQueryable<Expert> ApplySort(IQueryable<Expert> experts,string orderBy)
        {
            switch (orderBy)
            {
                case "rate-desc":
                {
                    experts = experts.OrderByDescending(e => e.Rate);
                    break;
                }
                case "name,rate-desc" :
                {
                    experts = experts.OrderBy(e => e.FirstName).ThenBy(e => e.LastName).ThenByDescending(e => e.Rate);
                    break;
                }
                case "rate-desc,name" :
                {
                    experts = experts.OrderByDescending(e => e.Rate).ThenBy(e => e.FirstName).ThenBy(e => e.LastName);
                    break;
                }
                default:
                {
                    experts=experts.OrderBy(e => e.FirstName).ThenBy(e => e.LastName);
                    break;
                }
            }

            return experts;
        }
        

    }
}