using E_Commerce_Draft.API.Models.Domain;
using static E_Commerce_Draft.API.Models.Domain.Order;

namespace E_Commerce_Draft.API.Repositories
{
    public interface IOrderRepository
    {
        Task<(int MessageId, string MessageDescription, List<Order>?)> GetAllOrdersAsync();
        Task<(int MessageId, string MessageDescription, Order?)> GetOrderByIdAsync(int id);
        Task<OrderResponseModel> CreateOrderAsync(Order order);
        Task<(int MessageId, string MessageDescription, Order?)> UpdateOrderAsync(Order order);
    }

}
