using System;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace Karia.Api.Services
{
    public class KariaServices:IKariaServices
    {
        
        private readonly IDatabase _database;

        public KariaServices(IConnectionMultiplexer connection)
        {

            _database = connection.GetDatabase();
        }

        public async Task<int> GetCode(string phoneNumber)
        {
            var key = "auth:" + phoneNumber;
            var code=(int)await _database.StringGetAsync(key);
            return code;
        }

        public async Task<bool> SetCode(string phoneNumber, int code)
        {
            try
            {
                var key = "auth" + phoneNumber;
                await _database.StringSetAsync(key, code, TimeSpan.FromSeconds(130));
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}