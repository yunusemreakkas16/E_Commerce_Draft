using E_Commerce_Draft.API.Models.Domain;
using E_Commerce_Draft.API.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static E_Commerce_Draft.API.Models.Domain.Order;
using static E_Commerce_Draft.API.Models.Domain.OrderDetail;

namespace E_Commerce_Draft.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderDetailController : ControllerBase
    {
        private readonly IOrderDetailsRepository orderDetailsRepository;

        public OrderDetailController(IOrderDetailsRepository orderDetailsRepository)
        {
            this.orderDetailsRepository = orderDetailsRepository;
        }

        [HttpPost]
        [Route("CreateOrderDetail")]
        public async Task<ActionResult<OrderDetailResponseModel>> CreateOrderDetail([FromBody] OrderDetail orderDetail)
        {
            //(Validation)
            if (orderDetail == null || orderDetail.OrderID <= 0 || orderDetail.ProductID <= 0 || orderDetail.Quantity <= 0)
                return BadRequest(new { MessageId = -2, MessageDescription = "Valid order detail data is required." });

            var responseModel = await orderDetailsRepository.CreateOrderDetailAsync(orderDetail);

            if (responseModel.MessageId == -99)
                return StatusCode(500, new { MessageId = -99, MessageDescription = responseModel.MessageDescription });

            if (responseModel.MessageId == -100)
                return StatusCode(500, new { MessageId = -100, MessageDescription = responseModel.MessageDescription });
            return Ok(responseModel);
        }

        [HttpPost]
        [Route("OrderDetailsList")]
        public async Task<ActionResult<GetAllOrderDetailsResponseModel>> GetAllOrderDetails()
        {
            var responseModel = await orderDetailsRepository.GetAllOrderDetailsAsync();

            if (responseModel.MessageId == -99)
                return StatusCode(500, new { MessageId = -99, MessageDescription = responseModel.MessageDescription });
            if (responseModel.MessageId == -100)
                return StatusCode(500, new { MessageId = -100, MessageDescription = responseModel.MessageDescription });
            return Ok(responseModel);
        }

        [HttpPost]
        [Route("Delete")]
        public async Task<ActionResult<bool>> DeleteOrderDetail(OrderDetailDetailParamModel orderDetailParamModel)
        {
            if (orderDetailParamModel.OrderID <= 0 || orderDetailParamModel.ProductID <= 0)
                return BadRequest(new { MessageId = -2, MessageDescription = "Valid order detail data is required." });
            var response = await orderDetailsRepository.DeleteOrderDetailAsync(orderDetailParamModel.OrderID, orderDetailParamModel.ProductID);
            if (response == false)
                return NotFound(new { MessageId = -1, MessageDescription = "Order detail not found." });
            return Ok(new { MessageId = 1, MessageDescription = "Order detail deleted successfully." });
        }

        [HttpPost]
        [Route("OrderDetailById")]
        public async Task<ActionResult<OrderDetailResponseModel>> GetOrderDetailsByOrderId(int orderId)
        {
            if (orderId <= 0)
                return BadRequest(new { MessageId = -2, MessageDescription = "Valid order detail data is required." });
            var responseModel = await orderDetailsRepository.GetOrderDetailsByOrderIdAsync(orderId);
            if (responseModel.MessageId == -99)
                return StatusCode(500, new { MessageId = -99, MessageDescription = responseModel.MessageDescription });
            if (responseModel.MessageId == -100)
                return StatusCode(500, new { MessageId = -100, MessageDescription = responseModel.MessageDescription });
            return Ok(responseModel);
        }

        [HttpPost]
        [Route("UpdateOrderDetail")]
        public async Task<ActionResult<OrderDetailResponseModel>> UpdateOrderDetail([FromBody] UpdateOrderDetailRequestModel updateOrderDetailRequestModel)
        {
            if (updateOrderDetailRequestModel == null || updateOrderDetailRequestModel.OrderId <= 0 || updateOrderDetailRequestModel.ProductId <= 0 || updateOrderDetailRequestModel.NewQuantity <= 0)
                return BadRequest(new { MessageId = -2, MessageDescription = "Valid order detail data is required." });
            var orderDetail = new OrderDetail
            {
                OrderID = updateOrderDetailRequestModel.OrderId,
                ProductID = updateOrderDetailRequestModel.ProductId,
                Quantity = updateOrderDetailRequestModel.NewQuantity
            };
            var responseModel = await orderDetailsRepository.UpdateOrderDetailAsync(orderDetail);
            if (responseModel.MessageId == -99)
                return StatusCode(500, new { MessageId = -99, MessageDescription = responseModel.MessageDescription });
            if (responseModel.MessageId == -100)
                return StatusCode(500, new { MessageId = -100, MessageDescription = responseModel.MessageDescription });
            return Ok(responseModel);
        }

    }
}
