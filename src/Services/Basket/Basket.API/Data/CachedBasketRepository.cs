using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Basket.API.Data
{
    public class CachedBasketRepository
        (IBasketRepository basketRepository, IDistributedCache cache)
        : IBasketRepository
    {
        public async Task<bool> DeleteBasket(string userName, CancellationToken cancellationToken = default)
        {
            var cachedBasket = await cache.GetStringAsync(userName, cancellationToken);
            if (cachedBasket is not null)
            {
                await cache.RemoveAsync(userName, cancellationToken);
            }
            return await basketRepository.DeleteBasket(userName, cancellationToken);
        }

        public async Task<ShoppingCart> GetBasket(string userName, CancellationToken cancellationToken = default)
        {
            var cachedBasket = await cache.GetStringAsync(userName, cancellationToken);
            if (!string.IsNullOrEmpty(cachedBasket))
            {
                return JsonSerializer.Deserialize<ShoppingCart>(cachedBasket)!;
            }
            var basket = await basketRepository.GetBasket(userName, cancellationToken);
            await cache.SetStringAsync(userName, JsonSerializer.Serialize(basket), cancellationToken);
            return basket;
        }

        public async Task<ShoppingCart> StoreBasket(ShoppingCart cart, CancellationToken cancellationToken = default)
        {
            await basketRepository.StoreBasket(cart, cancellationToken);
            await cache.SetStringAsync(cart.UserName, JsonSerializer.Serialize(cart), cancellationToken);
            return cart;
        }

        public async Task<ShoppingCart> UpdateBasket(ShoppingCart cart, CancellationToken cancellationToken = default)
        {
            return await basketRepository.UpdateBasket(cart, cancellationToken);
        }
    }
}
