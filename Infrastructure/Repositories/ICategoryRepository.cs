using ProductApi.Domain.Entities;

namespace ProductApi.Infrastructure.Repositories;

public interface ICategoryRepository
{
    Task<Category?> GetByIdAsync(int id);
    Task<IEnumerable<Category>> GetAllAsync();
    Task<Category> AddAsync(Category category);
    Task UpdateAsync(Category category);
    Task DeleteAsync(int id);
    Task<bool> HasProductsAsync(int categoryId);
    Task<bool> NameExistsAsync(string name, int? excludeId = null);
}