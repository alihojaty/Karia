using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Karia.Api.Entities;
using Karia.Api.Helpers;
using Karia.Api.Models;
using Karia.Api.ResourceParameters;

namespace Karia.Api.Services
{
    public interface IKariaRepository
    {
        Task<IEnumerable<Category>> GetCategoriesAsync();
        Task<bool> ExistsCategoryAsync(int categoryId);
        Task<PagedList<Expert>> GetExpertsAsync(int categoryId,ExpertsResourceParameters expertsResourceParameters);
        Task<Expert> GetExpertAsync(int categoryId, int expertId);
        Task<int> GetTotalCommentsForExperts(int expertId);
        Task<IEnumerable<Survey>> GetPollStatisticsAsync(int expertId);
        Task<IEnumerable<WorkSample>> GetSampleJobsAsync(int expertId);
        Task<bool> ExistsExpertAsync(int expertId);
        Task<PagedList<Commenting>> GetCommentsAsync(int expertId,CommentsResourceParameters commentsResourceParameters);
        Task<Employer> GetEmployerAsync(int employerId);
        Task<IEnumerable<Question>> GetQuestionsAsync();
        void InsertComment(Commenting comment);
        Task<bool> ExistsEmployerAsync(int employerId);
        Task<bool> UpdateStatisticsAsync(int expertId,StatisticsForUpdateDto statisticsForUpdateDto);
        void InsertFeedback(Critic critic);
        void UpdateEmployerAsync(Employer employer);
        Task<bool> Save();
    }
}