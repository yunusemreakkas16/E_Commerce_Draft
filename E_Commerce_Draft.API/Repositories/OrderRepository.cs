using E_Commerce_Draft.API.Models.Domain;

namespace E_Commerce_Draft.API.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        public Task<Order> CreateOrderAsync(Order order)
        {
            throw new NotImplementedException();
        }

        public Task<List<Order>> GetAllOrdersAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Order?> GetOrderByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Order?> UpdateOrderAsync(Order order)
        {
            throw new NotImplementedException();
        }
    }
}
