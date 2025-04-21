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
        public async Task<ActionResult<Product>> AddProduct([FromBody] Product product)
        {
            if (product == null)
                return BadRequest("Product data is required.");

            var addedProduct = await productRepository.AddProductAsync(product);

            if (addedProduct == null)
                return StatusCode(500, "An error occurred while adding the product.");

            return CreatedAtAction(nameof(AddProduct), new { id = addedProduct.ID }, addedProduct);
        }

        [HttpPost]
        [Route("ProductList")]
        public async Task<ActionResult<List<Product>>> GetAllProducts()
        {
            var products = await productRepository.GetAllProductsAsync();
            if (products == null || products.Count == 0)
                return NotFound("No products found.");
            return Ok(products);
        }

        [HttpPost]
        [Route("ProductDetail")]
        public async Task<ActionResult<Product>> GetProductById([FromBody] ProductDetailParamModel productDetailParamModel)
        {
            var product = await productRepository.GetProductByIdAsync(productDetailParamModel.ID);
            if (product == null)
                return NotFound("Product not found.");
            return Ok(product);
        }
        [HttpPost]
        [Route("ProductUpdate")]
        public async Task<ActionResult<Product>> UpdateProduct([FromBody] Product product)
        {
            if (product == null)
                return BadRequest("Product data is required.");
            var updatedProduct = await productRepository.UpdateProductAsync(product);
            if (updatedProduct == null)
                return StatusCode(500, "An error occurred while updating the product.");
            return Ok(updatedProduct);
        }


    }
}
