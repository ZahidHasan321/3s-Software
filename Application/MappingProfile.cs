using AutoMapper;
using ProductApi.Application.DTOs;
using ProductApi.Domain.Entities;

namespace ProductApi.Application;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Product mappings
        CreateMap<ProductCreateDto, Product>();
        CreateMap<ProductUpdateDto, Product>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

        CreateMap<Product, ProductResponseDto>()
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category!.Name))
            .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.ImagePath));

        // Category mappings
        CreateMap<CategoryCreateDto, Category>();
        CreateMap<CategoryUpdateDto, Category>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        CreateMap<Category, CategoryResponseDto>();

        // User mappings (if needed)
        // CreateMap<UserRegisterDto, User>();
    }
}