using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Karia.Api.DbContexts;
using Karia.Api.Entities;
using Karia.Api.Helpers;
using Karia.Api.Models;
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

        public async Task<bool> ExistsCategoryAsync(int categoryId)
        {
            return await _context.Categories.AnyAsync(c => c.Id == categoryId);
        }

        
        public async Task<PagedList<Expert>> GetExpertsAsync(int categoryId, ExpertsResourceParameters expertsResourceParameters)
        {
            var experts = _context.Groupings
                .Where(g => g.CategoryId == categoryId)
                .Include(g => g.Expert)
                .Select(g => g.Expert)
                .Where(e => e.IsValid == true);

            #region Filters and Searching
            experts = experts.WhereIf(expertsResourceParameters.IsMaster,
                e => e.IsMaster==true);
            experts = experts.WhereIf(expertsResourceParameters.IsHaveVehicle,
                e => e.IsHasVehicle == true);
            experts = experts.WhereIf(!(String.IsNullOrWhiteSpace(expertsResourceParameters.SearchQuery)),
                e => e.FirstName.Contains(expertsResourceParameters.SearchQuery)
                     || e.LastName.Contains(expertsResourceParameters.SearchQuery)
                     || e.Description.Contains(expertsResourceParameters.SearchQuery)
                     || e.Orientation.Contains(expertsResourceParameters.SearchQuery));
            // experts = experts.Where(e => e.IsValid == true);
            #endregion

            experts = ApplySort(experts,expertsResourceParameters.OrderBy);
            
            return await PagedList<Expert>.Create(experts,
                expertsResourceParameters.PageNumber,
                expertsResourceParameters.PageSize);

        }

        public async Task<Expert> GetExpertAsync(int categoryId, int expertId)
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
                .Where(c=>c.IsValid==true)
                .OrderByDescending(c=>c.DateOfRegistration);

            return await PagedList<Commenting>.Create(comments, commentsResourceParameters.PageNumber,
                commentsResourceParameters.PageSize);
        }

        public async Task<Employer> GetEmployerAsync(int employerId)
        {
            return await _context.Employers.SingleOrDefaultAsync(e => e.Id == employerId);
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
                case "name-asc,rate-desc" :
                {
                    experts = experts.OrderBy(e => e.FirstName).ThenBy(e => e.LastName).ThenByDescending(e => e.Rate);
                    break;
                }
                case "rate-desc,name" :
                {
                    experts = experts.OrderByDescending(e => e.Rate).ThenBy(e => e.FirstName).ThenBy(e => e.LastName);
                    break;
                }
                case "rate-desc,name-asc" :
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

        public async Task<IEnumerable<Question>> GetQuestionsAsync()
        {
            return await _context.Questions.ToListAsync();
        }

        public void InsertComment(Commenting comment)
        {
            _context.Commentings.Add(comment);
        }

        public async Task<bool> ExistsEmployerAsync(int employerId)
        {
            return await _context.Employers.AnyAsync(e => e.Id == employerId);
        }

        public async Task<bool> UpdateStatisticsAsync(int expertId,StatisticsForUpdateDto statisticsForUpdateDto)
        {
            var result = await _context.Database.ExecuteSqlRawAsync($"RateThePoll {statisticsForUpdateDto.QuestionId},{expertId},{statisticsForUpdateDto.Answer}");
            return true;
        }

        public void InsertFeedback(Critic critic)
        {
            _context.Critics.Add(critic);
        }

        public void UpdateEmployerAsync(Employer employer)
        {
            
        }

        public void InsertEmployer(Employer employer)
        {
            _context.Employers.Add(employer);
        }

        public async Task<bool> ExistsEmployerByPhoneNumber(string phoneNumber)
        {
            return await _context.Employers.AnyAsync(e=>e.PhoneNumber==phoneNumber);
        }

        public async Task<bool> Save()
        {
            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}