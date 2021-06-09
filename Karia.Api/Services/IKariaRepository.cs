using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Karia.Api.Entities;
using Karia.Api.ResourceParameters;

namespace Karia.Api.Services
{
    public interface IKariaRepository
    {
        Task<IEnumerable<Category>> GetCategories();
        Task<bool> ExistsCategory(Guid categoryId);
        Task<IEnumerable<Expert>> GetExperts(Guid categoryId,ExpertsResourceParameters expertsResourceParameters);
    }
}