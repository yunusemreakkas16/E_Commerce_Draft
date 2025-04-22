using E_Commerce_Draft.API.Models.Domain;

namespace E_Commerce_Draft.API.Repositories
{
    public interface ICategoryRepository
    {
        Task<(int MessageId, string MessageDescription, List<Category>)> GetAllCategoriesAsync();
        Task<(int MessageId, string MessageDescription, Category?)> GetCategoryByIdAsync(int categoryId);
        Task<(int MessageId, string MessageDescription, Category?)> CreateCategoryAsync(Category category);
        Task<(int MessageId, string MessageDescription, Category?)> UpdateCategoryAsync(Category category);
    }
}