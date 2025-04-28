using E_Commerce_Draft.API.Models.Domain;
using E_Commerce_Draft.API.Repositories;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize(Roles = "Admin,User")]
        [Route("AddCartItem")]
        public async Task<ActionResult<object>> AddCartItem([FromBody] CartItem cartItem)
        {
            // Validation 
            if (cartItem.UserId <= 0 || cartItem.ProductId <= 0 || cartItem.Quantity <= 0)
            {
                return BadRequest(new
                {
                    MessageId = -1,
                    MessageDescription = "Valid UserId, ProductId and Quantity are required."
                });
            }
            var response = await cartItemRepository.CreateCartItemAsync(cartItem);
            if (response.MessageId == -99)
                return StatusCode(500, new { MessageId = -99, MessageDescription = response.MessageDescription });
            if (response.MessageId == -100)
                return StatusCode(500, new { MessageId = -100, MessageDescription = response.MessageDescription });
            return Ok(new { MessageId = response.MessageId, MessageDescription = response.MessageDescription, AddedCartItem = response.CartItem });
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("CartItemList")]
        public async Task<ActionResult<GetAllCartItemsResponseModel>> GetAllCartItems()
        {

            var getAllCartItemsResponseModel = await cartItemRepository.GetAllCartItemsAsync();

            if (getAllCartItemsResponseModel.MessageId == -99)
                return StatusCode(500, new { MessageId = -99, MessageDescription = getAllCartItemsResponseModel.MessageDescription});

            if (getAllCartItemsResponseModel.MessageId == -100)
                return StatusCode(500, new { MessageId = -100, MessageDescription = getAllCartItemsResponseModel.MessageDescription});

            return Ok(new { MessageId = getAllCartItemsResponseModel.MessageId, MessageDescription = getAllCartItemsResponseModel.MessageDescription, CartItems = getAllCartItemsResponseModel.CartItems});
        }


        [HttpPost]
        [Authorize(Roles = "Admin,User")]
        [Route("GetCartItemsByUserId")]
        public async Task<ActionResult<GetCartItemsByUserIdResponseModel>> GetCartItemsByUserId([FromBody] CartItemDetailParamModel cartItemDetailParamModel)
        {
            var cartItemsByUserIdResponseModel = await cartItemRepository.GetCartItemsByUserIdAsync(cartItemDetailParamModel.ID);

            if (cartItemsByUserIdResponseModel.MessageId == -99)
                return StatusCode(500, new { MessageId = -99, MessageDescription = cartItemsByUserIdResponseModel.MessageDescription });

            if (cartItemsByUserIdResponseModel.MessageId == -100)
                return StatusCode(500, new { MessageId = -100, MessageDescription = cartItemsByUserIdResponseModel.MessageDescription });

            if (cartItemsByUserIdResponseModel.MessageId == 0)
                return NotFound(new { MessageId = 0, MessageDescription = cartItemsByUserIdResponseModel.MessageDescription, CartItems = cartItemsByUserIdResponseModel.CartItems });

            return Ok(new { MessageId = cartItemsByUserIdResponseModel.MessageId, MessageDescription = cartItemsByUserIdResponseModel.MessageDescription, CartItems = cartItemsByUserIdResponseModel.CartItems });
        }


        [HttpPost]
        [Authorize(Roles = "Admin,User")]
        [Route("UpdateCartItem")]
        public async Task<ActionResult<CartItemResponseModel>> UpdateCartItem([FromBody] CartItem cartItem)
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

            var cartItemResponseModel = await cartItemRepository.UpdateCartItemAsync(cartItem);

            if (cartItemResponseModel.MessageId == -99)
            {
                return StatusCode(500, new { MessageId = -99, MessageDescription = cartItemResponseModel.MessageDescription });
            }

            if (cartItemResponseModel.MessageId == -100)
            {
                return StatusCode(500, new { MessageId = -100, MessageDescription = cartItemResponseModel.MessageDescription });
            }

            return Ok(new { MessageId = cartItemResponseModel.MessageId, MessageDescription = cartItemResponseModel.MessageDescription, UpdatedCartItem = cartItemResponseModel.CartItem });
        }

        [HttpPost]
        [Authorize(Roles = "Admin,User")]
        [Route("DeleteCartItem")]
        public async Task<ActionResult<CartItemResponseModel>> DeleteCartItem([FromBody] CartItemDetailParamModel cartItemDetailParamModel)
        {
            var responseModel = await cartItemRepository.DeleteCartItemAsync(cartItemDetailParamModel.ID);
            if (responseModel.MessageId == -99)
                return StatusCode(500, new { MessageId = -99, MessageDescription = responseModel.MessageDescription});
            if (responseModel.MessageId == -100)
                return StatusCode(500, new { MessageId = -100, MessageDescription = responseModel.MessageDescription});
            return Ok(new { MessageId = responseModel.MessageId, MessageDescription = responseModel.MessageDescription });
        }
    }
}
