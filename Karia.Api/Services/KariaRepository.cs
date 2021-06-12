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

        public async Task<IEnumerable<Category>> GetCategoriesAsync()
        {
            var categories=await _context.Categories.ToListAsync();
            return categories;
        }

        public async Task<bool> ExistsCategoryAsync(Guid categoryId)
        {
            return await _context.Categories.AnyAsync(c => c.Id == categoryId);
        }

        
        public async Task<PagedList<Expert>> GetExpertsAsync(Guid categoryId, ExpertsResourceParameters expertsResourceParameters)
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

        public async Task<Expert> GetExpertAsync(Guid categoryId, int expertId)
        {
            var expert = _context.Groupings
                .Where(g => g.CategoryId == categoryId && g.ExpertId == expertId)
                .Include(g => g.Expert)
                .Select(g => g.Expert);
            
            return await expert.SingleOrDefaultAsync();
        }

        public async Task<int> GetTotalCommentsForExperts(int expertId)
        {
            return await _context.Commentings.CountAsync(c => c.ExpertId == expertId);
        }

        public async Task<IEnumerable<Survey>> GetPollStatisticsAsync(int expertId)
        {
            var statistics = await _context.Surveys
                .Where(s => s.ExpertId == expertId)
                .Include(s=>s.Question)
                .ToListAsync();
            return statistics ;
        }

        public async Task<IEnumerable<WorkSample>> GetSampleJobsAsync(int expertId)
        {
            return await _context.WorkSamples.Where(w => w.ExpertId == expertId).ToListAsync();
        }

        public async Task<bool> ExistsExpertAsync(int expertId)
        {
            return await _context.Experts.AnyAsync(e => e.Id == expertId);
        }

        public async Task<PagedList<Commenting>> GetCommentsAsync(int expertId,CommentsResourceParameters commentsResourceParameters)
        {
            var comments =  _context.Commentings
                .Where(c => c.ExpertId == expertId)
                .Include(c => c.Employer)
                .OrderByDescending(c=>c.DateOfRegistration);

            return await PagedList<Commenting>.Create(comments, commentsResourceParameters.PageNumber,
                commentsResourceParameters.PageSize);
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