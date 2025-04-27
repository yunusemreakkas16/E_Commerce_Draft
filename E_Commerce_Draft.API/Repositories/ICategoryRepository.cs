using E_Commerce_Draft.API.Models.Domain;

namespace E_Commerce_Draft.API.Repositories
{
    public interface ICategoryRepository
    {
        Task<CategoryListResponseModel> GetAllCategoriesAsync();
        Task<CategoryResponseModel> GetCategoryByIdAsync(int categoryId);
        Task<CategoryResponseModel> CreateCategoryAsync(Category category);
        Task<CategoryResponseModel> UpdateCategoryAsync(Category category);
    }
}