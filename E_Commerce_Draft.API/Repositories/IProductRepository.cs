using E_Commerce_Draft.API.Models.Domain;

namespace E_Commerce_Draft.API.Repositories
{
    public interface IProductRepository
    {
        Task<List<Product>> GetAllProductsAsync();
        Task<Product?> GetProductByIdAsync(int id);
        Task<Product?> AddProductAsync(Product product);
        Task<Product?> UpdateProductAsync(Product product);
    }
}
