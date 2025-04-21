using E_Commerce_Draft.API.Models.Domain;

namespace E_Commerce_Draft.API.Repositories
{
    public interface ICartItemRepository
    {
        Task<List<CartItem>> GetAllCartItemsAsync();
        Task<List<CartItem?>> GetCartItemsByUserIdAsync(int userId); 
        Task<CartItem> CreateCartItemAsync(CartItem cartItem);  
        Task<CartItem?> UpdateCartItemAsync(CartItem cartItem); 
        Task<bool> DeleteCartItemAsync(int userId, int productId);  
    }
}