using E_Commerce_Draft.API.Models.Domain;

namespace E_Commerce_Draft.API.Repositories
{
    public class OrderDetailRepository : IOrderDetailsRepository
    {
        public Task<OrderDetail> CreateOrderDetailAsync(OrderDetail orderDetail)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteOrderDetailAsync(int orderId, int productId)
        {
            throw new NotImplementedException();
        }

        public Task<List<OrderDetail>> GetAllOrderDetailsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<OrderDetail?>> GetOrderDetailsByOrderIdAsync(int orderId)
        {
            throw new NotImplementedException();
        }

        public Task<OrderDetail?> UpdateOrderDetailAsync(OrderDetail orderDetail)
        {
            throw new NotImplementedException();
        }
    }
}
