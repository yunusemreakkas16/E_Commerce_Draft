using E_Commerce_Draft.API.Models.Domain;
using E_Commerce_Draft.API.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static E_Commerce_Draft.API.Models.Domain.CartItem;

namespace E_Commerce_Draft.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartItemController : ControllerBase
    {
        private readonly ICartItemRepository cartItemRepository;

        public CartItemController(ICartItemRepository cartItemRepository)
        {
            this.cartItemRepository = cartItemRepository;
        }

        [HttpPost]
        [Route("CreateCartItem")]
        public async Task<ActionResult<object>> CreateCartItem([FromBody] CartItem cartItem)
        {
            if (cartItem == null || cartItem.UserId <= 0 || cartItem.ProductId <= 0 || cartItem.Quantity <= 0)
                return BadRequest(new { MessageId = -2, MessageDescription = "Valid cart item data is required." });

            var (messageId, messageDescription, newCartItem) = await cartItemRepository.CreateCartItemAsync(cartItem);

            if (messageId == -99)
                return StatusCode(500, new { MessageId = -99, MessageDescription = messageDescription });

            if (messageId == -100)
                return StatusCode(500, new { MessageId = -100, MessageDescription = messageDescription });

            return Ok(new { MessageId = messageId, MessageDescription = messageDescription, CartItem = newCartItem });
        }

        [HttpPost]
        [Route("CartItemList")]
        public async Task<ActionResult<object>> GetAllCartItems()
        {

            var (messageId, messageDescription, cartItems) = await cartItemRepository.GetAllCartItemsAsync();

            if (messageId == -99)
                return StatusCode(500, new { MessageId = -99, MessageDescription = messageDescription });

            if (messageId == -100)
                return StatusCode(500, new { MessageId = -100, MessageDescription = messageDescription });

            return Ok(new { MessageId = messageId, MessageDescription = messageDescription, CartItems = cartItems });
        }


        [HttpPost]
        [Route("GetCartItemsByUserId")]
        public async Task<ActionResult<object>> GetCartItemsByUserId([FromBody] CartItemDetailParamModel cartItemDetailParamModel)
        {
            var (messageId, messageDescription, cartItems) = await cartItemRepository.GetCartItemsByUserIdAsync(cartItemDetailParamModel.ID);

            if (messageId == -99)
                return StatusCode(500, new { MessageId = -99, MessageDescription = messageDescription });

            if (messageId == -100)
                return StatusCode(500, new { MessageId = -100, MessageDescription = messageDescription });

            return Ok(new { MessageId = messageId, MessageDescription = messageDescription, CartItems = cartItems });
        }

        [HttpPost]
        [Route("UpdateCartItem")]
        public async Task<ActionResult<object>> UpdateCartItem([FromBody] CartItem cartItem)
        {
            // Validation 
            if (cartItem.ID <= 0 || cartItem.Quantity <= 0)
            {
                return BadRequest(new
                {
                    MessageId = -1,
                    MessageDescription = "Valid CartItemId and Quantity are required."
                });
            }

            var (messageId, messageDescription, updatedCartItem) = await cartItemRepository.UpdateCartItemAsync(cartItem);

            if (messageId == -99)
            {
                return StatusCode(500, new { MessageId = -99, MessageDescription = messageDescription });
            }

            if (messageId == -100)
            {
                return StatusCode(500, new { MessageId = -100, MessageDescription = messageDescription });
            }

            return Ok(new
            {
                MessageId = messageId,
                MessageDescription = messageDescription,
                UpdatedCartItem = updatedCartItem
            });
        }

        [HttpPost]
        [Route("DeleteCartItem")]
        public async Task<ActionResult<object>> DeleteCartItem([FromBody] CartItemDetailParamModel cartItemDetailParamModel)
        {
            var (messageId, messageDescription) = await cartItemRepository.DeleteCartItemAsync(cartItemDetailParamModel.ID);
            if (messageId == -99)
                return StatusCode(500, new { MessageId = -99, MessageDescription = messageDescription });
            if (messageId == -100)
                return StatusCode(500, new { MessageId = -100, MessageDescription = messageDescription });
            return Ok(new { MessageId = messageId, MessageDescription = messageDescription });
        }
    }
}
