using E_Commerce_Draft.API.Models.Domain;

namespace E_Commerce_Draft.API.Repositories
{
    public interface IOrderDetailsRepository
        {
            Task<List<OrderDetail>> GetAllOrderDetailsAsync();
            Task<List<OrderDetail?>> GetOrderDetailsByOrderIdAsync(int orderId);
            Task<OrderDetail> CreateOrderDetailAsync(OrderDetail orderDetail);
            Task<OrderDetail?> UpdateOrderDetailAsync(OrderDetail orderDetail);
            Task<bool> DeleteOrderDetailAsync(int orderId, int productId); 
        }

}
