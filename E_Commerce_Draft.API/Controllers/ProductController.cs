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
        public async Task<ActionResult<object>> CreateProductAsync([FromBody] Product product)
        {
            if (product == null)
                return BadRequest(new { MessageId = -2, MessageDescription = "Product data is required." });

            var (messageId, messageDescription, newProduct) = await productRepository.CreateProductAsync(product);

            if (messageId == -99)
                return StatusCode(500, new { MessageId = -99, MessageDescription = messageDescription });

            if (messageId == -100)
                return StatusCode(500, new { MessageId = -100, MessageDescription = messageDescription });

            return Ok(new { MessageId = messageId, MessageDescription = messageDescription, Product = newProduct });
        }

        [HttpPost]
        [Route("ProductList")]
        public async Task<ActionResult<object>> GetAllProducts()
        {
            var (messageId, messageDescription, products) = await productRepository.GetAllProductsAsync();

            if (messageId == -1)
                return NotFound(new { MessageId = -1, MessageDescription = messageDescription });

            if (messageId == -99)
                return StatusCode(500, new { MessageId = -99, MessageDescription = messageDescription });

            if (messageId == -100)
                return StatusCode(500, new { MessageId = -100, MessageDescription = messageDescription });

            return Ok(new { MessageId = messageId, MessageDescription = messageDescription, Products = products });
        }

        [HttpPost]
        [Route("ProductDetail")]
        public async Task<ActionResult<object>> GetProductById([FromBody] ProductDetailParamModel productDetailParamModel)
        {
            var (messageId, messageDescription, product) = await productRepository.GetProductByIdAsync(productDetailParamModel.ID);

            if (messageId == -1)
                return NotFound(new { MessageId = -1, MessageDescription = messageDescription });

            if (messageId == -99)
                return StatusCode(500, new { MessageId = -99, MessageDescription = messageDescription });

            if (messageId == -100)
                return StatusCode(500, new { MessageId = -100, MessageDescription = messageDescription });

            return Ok(new { MessageId = messageId, MessageDescription = messageDescription, Product = product });
        }
        [HttpPost]
        [Route("ProductUpdate")]
        public async Task<ActionResult<object>> UpdateProduct([FromBody] Product product)
        {
            if (product == null || product.ID <= 0)
                return BadRequest(new { MessageId = -2, MessageDescription = "Valid product data is required." });

            var (messageId, messageDescription, updatedProduct) = await productRepository.UpdateProductAsync(product);

            if (messageId == -1)
                return NotFound(new { MessageId = -1, MessageDescription = messageDescription });

            if (messageId == -99)
                return StatusCode(500, new { MessageId = -99, MessageDescription = messageDescription });

            if (messageId == -100)
                return StatusCode(500, new { MessageId = -100, MessageDescription = messageDescription });

            return Ok(new { MessageId = messageId, MessageDescription = messageDescription, Product = updatedProduct });
        }



    }
}
