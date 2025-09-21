using ProductApi.Application.DTOs;

namespace ProductApi.Application.Interfaces;

public interface ICategoryService
{
    Task<CategoryResponseDto> CreateAsync(CategoryCreateDto dto);
    Task<IEnumerable<CategoryResponseDto>> GetAllAsync();
    Task<CategoryResponseDto?> GetByIdAsync(int id);
    Task<CategoryResponseDto> UpdateAsync(int id, CategoryUpdateDto dto);
    Task DeleteAsync(int id);
}