
using Ecom.Core.Entities;
using Ecom.Core.Interfaces;
using StackExchange.Redis;
using System.Text.Json;

namespace Ecom.infrastructure.Repositories
{
    public class CustomerBasketRepository : ICustomerBasketRepository
    {

        private readonly IDatabase _database;

        public CustomerBasketRepository(IConnectionMultiplexer redis)
        {
            _database = redis.GetDatabase();
        }

        public async Task<bool> DeleteBasketAsync(string id)
        {
            return await _database.KeyDeleteAsync(id);
        }

        public async Task<CustomerBasket> GetBasketAsync(string id)
        {
            var result = await _database.StringGetAsync(id);
            if (!string.IsNullOrEmpty(result))
            {

                return   JsonSerializer.Deserialize<CustomerBasket>(result);
            }
            return null;
        }

        public async Task<CustomerBasket> UpdateBasketAsync(CustomerBasket basket)
        {
            var _basket=await _database.StringSetAsync(basket.Id,JsonSerializer.Serialize(basket),TimeSpan.FromDays(3));
            
            if(_basket)
            {
                return await GetBasketAsync(basket.Id);
            }
            return null;

        }
    }
}
