using System.Threading.Tasks;

namespace Karia.Api.Services
{
    public interface IKariaServices
    {
        Task<int> GetCode(string phoneNumber);
        Task<bool> SetCode(string phoneNumber, int code);
    }
}