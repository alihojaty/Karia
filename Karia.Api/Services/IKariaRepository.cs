using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Karia.Api.Entities;
using Karia.Api.Helpers;
using Karia.Api.ResourceParameters;

namespace Karia.Api.Services
{
    public interface IKariaRepository
    {
        Task<IEnumerable<Category>> GetCategoriesAsync();
        Task<bool> ExistsCategoryAsync(Guid categoryId);
        Task<PagedList<Expert>> GetExpertsAsync(Guid categoryId,ExpertsResourceParameters expertsResourceParameters);
        Task<Expert> GetExpertAsync(Guid categoryId, int expertId);
        Task<int> GetTotalCommentsForExperts(int expertId);
        Task<IEnumerable<Survey>> GetPollStatisticsAsync(int expertId);
        Task<IEnumerable<WorkSample>> GetSampleJobsAsync(int expertId);
        Task<bool> ExistsExpertAsync(int expertId);
    }
}