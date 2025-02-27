﻿

namespace Basket.API.Data
{
    public class BasketRepository(IDocumentSession session)
        : IBasketRepository
    {
        public async Task<bool> DeleteBasket(string userName, CancellationToken cancellationToken = default)
        {
            session.Delete<ShoppingCart>(userName);
            await session.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task<ShoppingCart> GetBasket(string userName, CancellationToken cancellationToken = default)
        {
            var basket = await session.LoadAsync<ShoppingCart>(userName, cancellationToken);

            return basket ?? throw new BasketNotFoundException(userName);
        }

        public async Task<ShoppingCart> StoreBasket(ShoppingCart cart, CancellationToken cancellationToken = default)
        {
            session.Store(cart);
            await session.SaveChangesAsync(cancellationToken);
            return cart;
        }

        public async Task<ShoppingCart> UpdateBasket(ShoppingCart cart, CancellationToken cancellationToken = default)
        {
            var basket = await session.LoadAsync<ShoppingCart>(cart.UserName, cancellationToken);
            basket.UserName = cart.UserName;
            basket.Items = cart.Items;
            session.Update(basket);
            await session.SaveChangesAsync(cancellationToken);
            return basket;
        }
    }
}
