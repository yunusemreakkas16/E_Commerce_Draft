using E_Commerce_Draft.API.Models.Domain;
using static E_Commerce_Draft.API.Models.Domain.CartItem;

namespace E_Commerce_Draft.API.Repositories
{
    public interface ICartItemRepository
    {
        Task<GetAllCartItemsResponseModel> GetAllCartItemsAsync();
        Task<GetCartItemsByUserIdResponseModel> GetCartItemsByUserIdAsync(int userId); 
        Task<CartItemResponseModel> CreateCartItemAsync(CartItem cartItem);  
        Task<CartItemResponseModel> UpdateCartItemAsync(CartItem cartItem); 
        Task<CartItemResponseModel> DeleteCartItemAsync(int cartItemId);  
    }
}