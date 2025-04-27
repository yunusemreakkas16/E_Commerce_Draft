using E_Commerce_Draft.API.Models.Domain;
using E_Commerce_Draft.API.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static E_Commerce_Draft.API.Models.Domain.Product;

namespace E_Commerce_Draft.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository productRepository;

        public ProductController(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }

        [HttpPost]
        [Route("AddProduct")]
        public async Task<ActionResult<ProductResponseModel>> CreateProductAsync([FromBody] Product product)
        {
            if (product == null)
                return BadRequest(new { MessageId = -2, MessageDescription = "Product data is required." });

            var responseModel = await productRepository.CreateProductAsync(product);

            if (responseModel.MessageId == -99)
                return StatusCode(500, new { MessageId = -99, MessageDescription = responseModel.MessageDescription });

            if (responseModel.MessageId == -100)
                return StatusCode(500, new { MessageId = -100, MessageDescription = responseModel.MessageDescription });

            return Ok(new { MessageId = responseModel.MessageId, MessageDescription = responseModel.MessageDescription, Product = responseModel.Product });
        }

        [HttpPost]
        [Route("ProductList")]
        public async Task<ActionResult<object>> GetAllProducts()
        {
            var responseModel = await productRepository.GetAllProductsAsync();

            if (responseModel.MessageId == -1)
                return NotFound(new { MessageId = -1, MessageDescription = responseModel.MessageDescription });

            if (responseModel.MessageId == -99)
                return StatusCode(500, new { MessageId = -99, MessageDescription = responseModel.MessageDescription });

            if (responseModel.MessageId == -100)
                return StatusCode(500, new { MessageId = -100, MessageDescription = responseModel.MessageDescription });

            return Ok(new { MessageId = responseModel.MessageId, MessageDescription = responseModel.MessageDescription, Products = responseModel.Products });
        }

        [HttpPost]
        [Route("ProductDetail")]
        public async Task<ActionResult<object>> GetProductById([FromBody] ProductDetailParamModel productDetailParamModel)
        {
            var responseModel = await productRepository.GetProductByIdAsync(productDetailParamModel.ID);

            if (responseModel.MessageId == -1)
                return NotFound(new { MessageId = -1, MessageDescription = responseModel.MessageDescription });

            if (responseModel.MessageId == -99)
                return StatusCode(500, new { MessageId = -99, MessageDescription = responseModel.MessageDescription });

            if (responseModel.MessageId == -100)
                return StatusCode(500, new { MessageId = -100, MessageDescription = responseModel.MessageDescription });

            return Ok(new { MessageId = responseModel.MessageId, MessageDescription = responseModel.MessageDescription, Product = responseModel.Product });
        }
        [HttpPost]
        [Route("ProductUpdate")]
        public async Task<ActionResult<object>> UpdateProduct([FromBody] Product product)
        {
            if (product == null || product.ID <= 0)
                return BadRequest(new { MessageId = -2, MessageDescription = "Valid product data is required." });

            var responseModel = await productRepository.UpdateProductAsync(product);

            if (responseModel.MessageId == -1)
                return NotFound(new { MessageId = -1, MessageDescription = responseModel.MessageDescription });

            if (responseModel.MessageId == -99)
                return StatusCode(500, new { MessageId = -99, MessageDescription = responseModel.MessageDescription });

            if (responseModel.MessageId == -100)
                return StatusCode(500, new { MessageId = -100, MessageDescription = responseModel.MessageDescription });

            return Ok(new { MessageId = responseModel.MessageId, MessageDescription = responseModel.MessageDescription, Product = responseModel.Product });
        }
    }
}
