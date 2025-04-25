using E_Commerce_Draft.API.Models.Domain;
using E_Commerce_Draft.API.Repositories;
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
        [Route("OrderDetail")]
        public async Task<ActionResult<object>> GetOrderById([FromBody] OrderDetailParamModel orderDetailParamModel)
        {
            var (messageId, messageDescription, order) = await orderRepository.GetOrderByIdAsync(orderDetailParamModel.OrderID);

            if (messageId == -1)
                return NotFound(new { MessageId = -1, MessageDescription = messageDescription });

            if (messageId == -99)
                return StatusCode(500, new { MessageId = -99, MessageDescription = messageDescription });

            if (messageId == -100)
                return StatusCode(500, new { MessageId = -100, MessageDescription = messageDescription });

            return Ok(new { MessageId = messageId, MessageDescription = messageDescription, Order = order });
        }

        [HttpPost]
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
        [Route("OrderList")]
        public async Task<ActionResult<object>> GetAllOrders()
        {
            var (messageId, messageDescription, orders) = await orderRepository.GetAllOrdersAsync();

            if (messageId == -1)
                return NotFound(new { MessageId = -1, MessageDescription = messageDescription });

            if (messageId == -99)
                return StatusCode(500, new { MessageId = -99, MessageDescription = messageDescription });

            if (messageId == -100)
                return StatusCode(500, new { MessageId = -100, MessageDescription = messageDescription });

            return Ok(new { MessageId = messageId, MessageDescription = messageDescription, Orders = orders });
        }

        [HttpPost]
        [Route("OrderUpdate")]
        public async Task<ActionResult<object>> UpdateOrder([FromBody] Order order)
        {
            if (order == null || order.ID <= 0 || order.UserID <= 0 || order.TotalPrice <= 0)
                return BadRequest(new { MessageId = -2, MessageDescription = "Valid order data is required." });

            var (messageId, messageDescription, updatedOrder) = await orderRepository.UpdateOrderAsync(order);

            if (messageId == -1)
                return NotFound(new { MessageId = -1, MessageDescription = messageDescription });

            if (messageId == -99)
                return StatusCode(500, new { MessageId = -99, MessageDescription = messageDescription });

            if (messageId == -100)
                return StatusCode(500, new { MessageId = -100, MessageDescription = messageDescription });

            return Ok(new { MessageId = messageId, MessageDescription = messageDescription, Order = updatedOrder });
        }
    }
}
