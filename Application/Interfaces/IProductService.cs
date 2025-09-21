using ProductApi.Application.DTOs;

namespace ProductApi.Application.Interfaces;

public interface IProductService
{
    Task<ProductResponseDto> CreateAsync(ProductCreateDto dto, IFormFile? image = null);
    Task<IEnumerable<ProductResponseDto>> GetAllAsync(
        int? categoryId = null, 
        decimal? minPrice = null, 
        decimal? maxPrice = null, 
        int page = 1, 
        int limit = 10);
    Task<ProductResponseDto?> GetByIdAsync(int id);
    Task<ProductResponseDto> UpdateAsync(int id, ProductUpdateDto dto, IFormFile? image = null);
    Task DeleteAsync(int id);
}