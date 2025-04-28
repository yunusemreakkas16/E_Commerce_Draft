using E_Commerce_Draft.API.Models.Domain;
using E_Commerce_Draft.API.Repositories;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize(Roles = "Admin")]
        [Route("CategoryDetail")]
        public async Task<ActionResult<object>> GetCategoryByIdAsync([FromBody]CategoryDetailParamModel categoryDetailParamModel)
        {
            var responseModel = await categoryRepository.GetCategoryByIdAsync(categoryDetailParamModel.ID);

            if (responseModel.MessageId == -1)
                return NotFound(new { MessageId = -1, MessageDescription = responseModel.MessageDescription});

            if (responseModel.MessageId == -99)
                return StatusCode(500, new { MessageId = -99, MessageDescription = responseModel.MessageDescription });

            if (responseModel.MessageId == -100)
                return StatusCode(500, new { MessageId = -100, MessageDescription = responseModel.MessageDescription });

            return Ok(new { MessageId = responseModel.MessageId, MessageDescription = responseModel.MessageDescription, Category = responseModel.Category});
        }


        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("CategoryList")]
        public async Task<ActionResult<object>> CategoryListAsync()
        {
            var responseModel = await categoryRepository.GetAllCategoriesAsync();

            if (responseModel.MessageId == -1)
                return NotFound(new { MessageId = -1, MessageDescription = responseModel.MessageDescription });

            if (responseModel.MessageId == -99)
                return StatusCode(500, new { MessageId = -99, MessageDescription = responseModel.MessageDescription });

            if (responseModel.MessageId == -100)
                return StatusCode(500, new { MessageId = -100, MessageDescription = responseModel.MessageDescription });

            return Ok(new { MessageId = responseModel.MessageId, MessageDescription = responseModel.MessageDescription, Categories = responseModel.Categories });
        }


        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("CreateCategory")]
        public async Task<ActionResult<object>> CreateCategoryAsync([FromBody] Category category)
        {
            if (category == null || string.IsNullOrWhiteSpace(category.Name))
                return BadRequest(new { MessageId = -2, MessageDescription = "Valid category data is required." });

            var responseModel = await categoryRepository.CreateCategoryAsync(category);

            if (responseModel.MessageId == -99)
                return StatusCode(500, new { MessageId = -99, MessageDescription = responseModel.MessageDescription});

            if (responseModel.MessageId == -100)
                return StatusCode(500, new { MessageId = -100, MessageDescription = responseModel.MessageDescription});

            return Ok(new { MessageId = responseModel.MessageId, MessageDescription = responseModel.MessageDescription, Category = responseModel.Category});
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("CategoryUpdate")]
        public async Task<ActionResult<CategoryResponseModel>> UpdateCategoryAsync([FromBody] Category category)
        {
            if (category == null || category.ID <= 0)
                return BadRequest(new { MessageId = -2, MessageDescription = "Valid category data is required." });

            var responseModel = await categoryRepository.UpdateCategoryAsync(category);

            if (responseModel.MessageId == -1)
                return NotFound(new { MessageId = -1, MessageDescription = responseModel.MessageDescription});

            if (responseModel.MessageId == -99)
                return StatusCode(500, new { MessageId = -99, MessageDescription = responseModel.MessageDescription });

            if (responseModel.MessageId == -100)
                return StatusCode(500, new { MessageId = -100, MessageDescription = responseModel.MessageDescription });

            return Ok(new { MessageId = responseModel.MessageId, MessageDescription = responseModel.MessageDescription, Category = responseModel.Category });
        }
    }
}
