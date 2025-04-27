using E_Commerce_Draft.API.Models.Domain;
using static E_Commerce_Draft.API.Models.Domain.Product;

namespace E_Commerce_Draft.API.Repositories
{
    public interface IProductRepository
    {
        Task<GetAllProductsResponseModel> GetAllProductsAsync();
        Task<ProductResponseModel> GetProductByIdAsync(int id);
        Task<ProductResponseModel> CreateProductAsync(Product product);
        Task<ProductResponseModel> UpdateProductAsync(Product product);
    }
}
