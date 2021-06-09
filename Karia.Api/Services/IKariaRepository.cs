using System.Collections.Generic;
using System.Threading.Tasks;
using Karia.Api.Entities;

namespace Karia.Api.Services
{
    public interface IKariaRepository
    {
        Task<IEnumerable<Category>> GetCategories();
    }
}