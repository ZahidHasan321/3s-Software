using AutoMapper;
using ProductApi.Application.DTOs;
using ProductApi.Application.Interfaces;
using ProductApi.Domain.Entities;
using ProductApi.Infrastructure.Repositories;

namespace ProductApi.Application.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public CategoryService(
        ICategoryRepository categoryRepository,
        IProductRepository productRepository,
        IMapper mapper)
    {
        _categoryRepository = categoryRepository;
        _productRepository = productRepository;
        _mapper = mapper;
    }

    public async Task<CategoryResponseDto> CreateAsync(CategoryCreateDto dto)
    {
        if (await _categoryRepository.NameExistsAsync(dto.Name))
            throw new ArgumentException("Category name already exists");

        var category = _mapper.Map<Category>(dto);
        category.CreatedAt = DateTime.UtcNow;

        var createdCategory = await _categoryRepository.AddAsync(category);
        return _mapper.Map<CategoryResponseDto>(createdCategory);
    }

    public async Task<IEnumerable<CategoryResponseDto>> GetAllAsync()
    {
        var categories = await _categoryRepository.GetAllAsync();
        var dtos = _mapper.Map<IEnumerable<CategoryResponseDto>>(categories);

        foreach (var dto in dtos)
        {
            var count = await _productRepository.CountByCategoryIdAsync(dto.Id);
            dto.ProductCount = count;
        }

        return dtos;
    }

    public async Task<CategoryResponseDto?> GetByIdAsync(int id)
    {
        var category = await _categoryRepository.GetByIdAsync(id);
        if (category == null) return null;

        var dto = _mapper.Map<CategoryResponseDto>(category);
        dto.ProductCount = await _productRepository.CountByCategoryIdAsync(id);

        return dto;
    }

    public async Task<CategoryResponseDto> UpdateAsync(int id, CategoryUpdateDto dto)
    {
        var existingCategory = await _categoryRepository.GetByIdAsync(id);
        if (existingCategory == null)
            throw new ArgumentException("Category not found");

        if (dto.Name != null && await _categoryRepository.NameExistsAsync(dto.Name, id))
            throw new ArgumentException("Category name already exists");

        _mapper.Map(dto, existingCategory);
        existingCategory.UpdatedAt = DateTime.UtcNow;
        await _categoryRepository.UpdateAsync(existingCategory);

        var updatedCategory = await _categoryRepository.GetByIdAsync(id);
        var responseDto = _mapper.Map<CategoryResponseDto>(updatedCategory);
        responseDto.ProductCount = await _productRepository.CountByCategoryIdAsync(id);

        return responseDto;
    }

    public async Task DeleteAsync(int id)
    {
        var category = await _categoryRepository.GetByIdAsync(id);
        if (category == null)
            throw new ArgumentException("Category not found");

        var hasProducts = await _categoryRepository.HasProductsAsync(id);
        if (hasProducts)
            throw new InvalidOperationException("Cannot delete category with associated products");

        await _categoryRepository.DeleteAsync(id);
    }
}