using E_Commerce_Draft.API.Models.Domain;

namespace E_Commerce_Draft.API.Repositories
{
    public class CartItemRepository : ICartItemRepository
    {
        public Task<CartItem> CreateCartItemAsync(CartItem cartItem)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteCartItemAsync(int userId, int productId)
        {
            throw new NotImplementedException();
        }

        public Task<List<CartItem>> GetAllCartItemsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<CartItem?>> GetCartItemsByUserIdAsync(int userId)
        {
            throw new NotImplementedException();
        }

        public Task<CartItem?> UpdateCartItemAsync(CartItem cartItem)
        {
            throw new NotImplementedException();
        }
    }
}
