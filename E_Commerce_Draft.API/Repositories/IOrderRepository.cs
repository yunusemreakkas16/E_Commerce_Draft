using E_Commerce_Draft.API.Models.Domain;

namespace E_Commerce_Draft.API.Repositories
{
    public interface IOrderRepository
    {
        Task<List<Order>> GetAllOrdersAsync();
        Task<Order?> GetOrderByIdAsync(int id);
        Task<Order> CreateOrderAsync(Order order);
        Task<Order?> UpdateOrderAsync(Order order);
    }

}
