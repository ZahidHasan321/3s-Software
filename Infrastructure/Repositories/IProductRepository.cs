using ProductApi.Domain.Entities;

namespace ProductApi.Infrastructure.Repositories;

public interface IProductRepository
{
    Task<Product?> GetByIdAsync(int id);
    Task<IEnumerable<Product>> GetAllAsync(int? categoryId = null, decimal? minPrice = null, decimal? maxPrice = null, int page = 1, int limit = 10);
    Task<Product> AddAsync(Product product);
    Task UpdateAsync(Product product);
    Task DeleteAsync(int id);
    Task<int> CountByCategoryIdAsync(int categoryId);
}