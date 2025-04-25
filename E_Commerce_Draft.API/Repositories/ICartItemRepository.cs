using E_Commerce_Draft.API.Models.Domain;

namespace E_Commerce_Draft.API.Repositories
{
    public interface ICartItemRepository
    {
        Task<(int MessageId, string MessageDescription, List<CartItem>?)> GetAllCartItemsAsync();
        Task<(int MessageId, string MessageDescription, List<CartItem>?)> GetCartItemsByUserIdAsync(int userId); 
        Task<(int MessageId, string MessageDescription, CartItem?)> CreateCartItemAsync(CartItem cartItem);  
        Task<(int MessageId, string MessageDescription, CartItem?)> UpdateCartItemAsync(CartItem cartItem); 
        Task<(int MessageId, string MessageDescription)> DeleteCartItemAsync(int cartItemId);  
    }
}