using E_Commerce_Draft.API.Models.Domain;
using E_Commerce_Draft.API.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce_Draft.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository categoryRepository;

        public CategoryController(ICategoryRepository categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }

        [HttpPost]
        [Route("CategoryDetail")]
        public async Task<ActionResult<object>> GetCategoryById([FromBody]CategoryDetailParamModel categoryDetailParamModel)
        {
            var (messageId, messageDescription, category) = await categoryRepository.GetCategoryByIdAsync(categoryDetailParamModel.ID);

            if (messageId == -1)
                return NotFound(new { MessageId = -1, MessageDescription = messageDescription });

            if (messageId == -99)
                return StatusCode(500, new { MessageId = -99, MessageDescription = messageDescription });

            if (messageId == -100)
                return StatusCode(500, new { MessageId = -100, MessageDescription = messageDescription });

            return Ok(new { MessageId = messageId, MessageDescription = messageDescription, Category = category });
        }


        [HttpPost]
        [Route("CategoryList")]
        public async Task<ActionResult<object>> CategoryList()
        {
            var (messageId, messageDescription, categories) = await categoryRepository.GetAllCategoriesAsync();

            if (messageId == -1)
                return NotFound(new { MessageId = -1, MessageDescription = messageDescription });

            if (messageId == -99)
                return StatusCode(500, new { MessageId = -99, MessageDescription = messageDescription });

            if (messageId == -100)
                return StatusCode(500, new { MessageId = -100, MessageDescription = messageDescription });

            return Ok(new { MessageId = messageId, MessageDescription = messageDescription, Categories = categories });
        }


        [HttpPost]
        [Route("CreateCategory")]
        public async Task<ActionResult<object>> CreateCategory([FromBody] Category category)
        {
            if (category == null || string.IsNullOrWhiteSpace(category.Name))
                return BadRequest(new { MessageId = -2, MessageDescription = "Valid category data is required." });

            var (messageId, messageDescription, newCategory) = await categoryRepository.CreateCategoryAsync(category);

            if (messageId == -99)
                return StatusCode(500, new { MessageId = -99, MessageDescription = messageDescription });

            if (messageId == -100)
                return StatusCode(500, new { MessageId = -100, MessageDescription = messageDescription });

            return Ok(new { MessageId = messageId, MessageDescription = messageDescription, Category = newCategory });
        }

        [HttpPost]
        [Route("CategoryUpdate")]
        public async Task<ActionResult<object>> UpdateCategory([FromBody] Category category)
        {
            if (category == null || category.ID <= 0)
                return BadRequest(new { MessageId = -2, MessageDescription = "Valid category data is required." });

            var (messageId, messageDescription, updatedCategory) = await categoryRepository.UpdateCategoryAsync(category);

            if (messageId == -1)
                return NotFound(new { MessageId = -1, MessageDescription = messageDescription });

            if (messageId == -99)
                return StatusCode(500, new { MessageId = -99, MessageDescription = messageDescription });

            if (messageId == -100)
                return StatusCode(500, new { MessageId = -100, MessageDescription = messageDescription });

            return Ok(new { MessageId = messageId, MessageDescription = messageDescription, Category = updatedCategory });
        }
    }
}
