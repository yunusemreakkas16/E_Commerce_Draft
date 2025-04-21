using E_Commerce_Draft.API.Models.Domain;

namespace E_Commerce_Draft.API.Repositories
{
    public interface ICategoryRepository
    {
        Task<List<Category>> GetAllCategoriesAsync();
        Task<Category?> GetCategoryByIdAsync(CategoryDetailParamModel categoryDetailParamModel);
        Task<Category?> CreateCategoryAsync(Category category);
        Task<Category?> UpdateCategoryAsync(Category category);
    }
}