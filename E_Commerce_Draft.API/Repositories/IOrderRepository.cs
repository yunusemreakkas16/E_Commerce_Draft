using E_Commerce_Draft.API.Models.Domain;
using static E_Commerce_Draft.API.Models.Domain.Order;

namespace E_Commerce_Draft.API.Repositories
{
    public interface IOrderRepository
    {
        Task<OrderListResponseModel> GetAllOrdersAsync();
        Task<OrderResponseModel> GetOrderByIdAsync(int id);
        Task<OrderResponseModel> CreateOrderAsync(Order order);
        Task<OrderResponseModel> UpdateOrderAsync(Order order);
    }

}
