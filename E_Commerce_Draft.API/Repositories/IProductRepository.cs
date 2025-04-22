using E_Commerce_Draft.API.Models.Domain;

namespace E_Commerce_Draft.API.Repositories
{
    public interface IProductRepository
    {
        Task<(int MessageId, string MessageDescription, List<Product>)> GetAllProductsAsync();
        Task<(int MessageId, string MessageDescription, Product?)> GetProductByIdAsync(int id);
        Task<(int MessageId, string MessageDescription, Product?)> CreateProductAsync(Product product);
        Task<(int MessageId, string MessageDescription, Product?)> UpdateProductAsync(Product product);
    }
}
