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
        public async Task<ActionResult<Category>> GetById([FromBody] CategoryDetailParamModel categoryDetailParamModel)
        {
            var category = await categoryRepository.GetCategoryByIdAsync(categoryDetailParamModel);

            if (category == null)
                return NotFound();

            return Ok(category);
        }

        [HttpPost]
        [Route("CategoryList")]
        public async Task<ActionResult<Category>> CategoryList()
        {
            var categories = await categoryRepository.GetAllCategoriesAsync();
            if (categories == null || categories.Count == 0)
                return NotFound();
            return Ok(categories);
        }

        [HttpPost]
        [Route("CategoryCreate")]
        public async Task<IActionResult> Create([FromBody] Category category)
        {
            if (string.IsNullOrWhiteSpace(category.Name))
                return BadRequest("Category name is required.");

            var newCategory = await categoryRepository.CreateCategoryAsync(category);

            if (newCategory == null)
                return StatusCode(500, "Error creating category.");

            return Ok(newCategory);
        }

        [HttpPost]
        [Route("CategoryUpdate")]
        public async Task<IActionResult> Update([FromBody] Category category)
        {
            var updatedCategory = await categoryRepository.UpdateCategoryAsync(category);
            if (updatedCategory == null)
                return StatusCode(500, "Error updating category.");
            return Ok(updatedCategory);
        }

    }
}
