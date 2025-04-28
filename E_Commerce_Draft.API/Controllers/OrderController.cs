using E_Commerce_Draft.API.Models.Domain;
using E_Commerce_Draft.API.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static E_Commerce_Draft.API.Models.Domain.Order;

namespace E_Commerce_Draft.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository orderRepository;

        public OrderController(IOrderRepository orderRepository)
        {
            this.orderRepository = orderRepository;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("OrderDetail")]
        public async Task<ActionResult<OrderResponseModel>> GetOrderById([FromBody] OrderDetailParamModel orderDetailParamModel)
        {
            var responseModel = await orderRepository.GetOrderByIdAsync(orderDetailParamModel.OrderID);

            if (responseModel.MessageId == -1)
                return NotFound(new { MessageId = -1, MessageDescription = responseModel.MessageDescription });

            if (responseModel.MessageId == -99)
                return StatusCode(500, new { MessageId = -99, MessageDescription = responseModel.MessageDescription });

            if (responseModel.MessageId == -100)
                return StatusCode(500, new { MessageId = -100, MessageDescription = responseModel.MessageDescription });

            return Ok(new { MessageId = responseModel.MessageId, MessageDescription = responseModel.MessageDescription, Order = responseModel.Order });
        }

        [HttpPost]
        [Authorize(Roles = "Admin,User")]
        [Route("OrderCreate")]
        public async Task<ActionResult<OrderResponseModel>> CreateOrderAsync([FromBody] Order order)
        {
            OrderResponseModel orderResponseModel = new OrderResponseModel();

            if (order == null || order.UserID <= 0 || order.TotalPrice <= 0)
                return BadRequest(new { MessageId = -2, MessageDescription = "Valid order data is required." });

            orderResponseModel = await orderRepository.CreateOrderAsync(order);

            if (orderResponseModel.MessageId == -99)
                return StatusCode(500, new { MessageId = -99, MessageDescription = orderResponseModel.MessageDescription });

            if (orderResponseModel.MessageId == -100)
                return StatusCode(500, new { MessageId = -100, MessageDescription = orderResponseModel.MessageDescription });

            return Ok(orderResponseModel);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("OrderList")]
        public async Task<ActionResult<OrderListResponseModel>> GetAllOrders()
        {
            var responseModel = await orderRepository.GetAllOrdersAsync();

            if (responseModel.MessageId == -1)
                return NotFound(new { MessageId = -1, MessageDescription = responseModel.MessageDescription });

            if (responseModel.MessageId == -99)
                return StatusCode(500, new { MessageId = -99, MessageDescription = responseModel.MessageDescription });

            if (responseModel.MessageId == -100)
                return StatusCode(500, new { MessageId = -100, MessageDescription = responseModel.MessageDescription });

            return Ok(new { MessageId = responseModel.MessageId, MessageDescription = responseModel.MessageDescription, Orders = responseModel.Orders });
        }

        [HttpPost]
        [Authorize(Roles = "Admin,User")]
        [Route("OrderUpdate")]
        public async Task<ActionResult<OrderResponseModel>> UpdateOrder([FromBody] Order order)
        {
            if (order == null || order.ID <= 0 || order.UserID <= 0 || order.TotalPrice <= 0)
                return BadRequest(new { MessageId = -2, MessageDescription = "Valid order data is required." });

            var responseModel = await orderRepository.UpdateOrderAsync(order);

            if (responseModel.MessageId == -1)
                return NotFound(new { MessageId = -1, MessageDescription = responseModel.MessageDescription });

            if (responseModel.MessageId == -99)
                return StatusCode(500, new { MessageId = -99, MessageDescription = responseModel.MessageDescription });

            if (responseModel.MessageId == -100)
                return StatusCode(500, new { MessageId = -100, MessageDescription = responseModel.MessageDescription });

            return Ok(new { MessageId = responseModel.MessageId, MessageDescription = responseModel.MessageDescription, Order = responseModel.Order });
        }
    }
}
