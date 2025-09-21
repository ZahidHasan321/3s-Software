using AutoMapper;
using ProductApi.Application.DTOs;
using ProductApi.Application.Interfaces;
using ProductApi.Domain.Entities;
using ProductApi.Infrastructure.Repositories;
using ProductApi.Infrastructure.Services;

namespace ProductApi.Application.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IFileService _fileService;
    private readonly IMapper _mapper;

    public ProductService(
        IProductRepository productRepository,
        ICategoryRepository categoryRepository,
        IFileService fileService,
        IMapper mapper)
    {
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
        _fileService = fileService;
        _mapper = mapper;
    }

    public async Task<ProductResponseDto> CreateAsync(ProductCreateDto dto, IFormFile? image = null)
    {
        // Validate category exists
        var category = await _categoryRepository.GetByIdAsync(dto.CategoryId);
        if (category == null)
            throw new ArgumentException("Category not found");

        var product = _mapper.Map<Product>(dto);
        product.CreatedAt = DateTime.UtcNow;

        if (image != null && _fileService.IsValidImage(image))
        {
            product.ImagePath = await _fileService.SaveImageAsync(image);
        }

        var createdProduct = await _productRepository.AddAsync(product);
        return _mapper.Map<ProductResponseDto>(createdProduct);
    }

    public async Task<IEnumerable<ProductResponseDto>> GetAllAsync(
        int? categoryId = null,
        decimal? minPrice = null,
        decimal? maxPrice = null,
        int page = 1,
        int limit = 10)
    {
        var products = await _productRepository.GetAllAsync(categoryId, minPrice, maxPrice, page, limit);
        return _mapper.Map<IEnumerable<ProductResponseDto>>(products);
    }

    public async Task<ProductResponseDto?> GetByIdAsync(int id)
    {
        var product = await _productRepository.GetByIdAsync(id);
        return product != null ? _mapper.Map<ProductResponseDto>(product) : null;
    }

    public async Task<ProductResponseDto> UpdateAsync(int id, ProductUpdateDto dto, IFormFile? image = null)
    {
        var existingProduct = await _productRepository.GetByIdAsync(id);
        if (existingProduct == null)
            throw new ArgumentException("Product not found");

        // Validate category if provided
        if (dto.CategoryId.HasValue)
        {
            var category = await _categoryRepository.GetByIdAsync(dto.CategoryId.Value);
            if (category == null)
                throw new ArgumentException("Category not found");
        }

        _mapper.Map(dto, existingProduct);

        if (image != null && _fileService.IsValidImage(image))
        {
            // Delete old image if exists
            if (!string.IsNullOrEmpty(existingProduct.ImagePath))
            {
                await _fileService.DeleteImageAsync(existingProduct.ImagePath);
            }
            existingProduct.ImagePath = await _fileService.SaveImageAsync(image);
        }

        existingProduct.UpdatedAt = DateTime.UtcNow;
        await _productRepository.UpdateAsync(existingProduct);

        var updatedProduct = await _productRepository.GetByIdAsync(id);
        return _mapper.Map<ProductResponseDto>(updatedProduct);
    }

    public async Task DeleteAsync(int id)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if (product == null)
            throw new ArgumentException("Product not found");

        if (!string.IsNullOrEmpty(product.ImagePath))
        {
            await _fileService.DeleteImageAsync(product.ImagePath);
        }

        await _productRepository.DeleteAsync(id);
    }
}