using E_Commerce_Draft.API.Models.Domain;
using static E_Commerce_Draft.API.Models.Domain.OrderDetail;

namespace E_Commerce_Draft.API.Repositories
{
    public interface IOrderDetailsRepository
        {
            Task<GetAllOrderDetailsResponseModel> GetAllOrderDetailsAsync();
            Task<GetAllOrderDetailsResponseModel> GetOrderDetailsByOrderIdAsync(int orderId);
            Task<OrderDetailResponseModel> CreateOrderDetailAsync(OrderDetail orderDetail);
            Task<OrderDetailResponseModel> UpdateOrderDetailAsync(OrderDetail orderDetail);
            Task<bool> DeleteOrderDetailAsync(int orderId, int productId); 
        }

}
