using AutoMapper;
using E_TicaretNew.Application.DTOs.ProductDTOs;
using E_TicaretNew.Domain.Entities;

using AutoMapper;
using E_TicaretNew.Application.DTOs.ProductDTOs;
using E_TicaretNew.Domain.Entities;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<ProductCreateDto, Product>()
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId));

        CreateMap<Product, ProductGetDto>()
            .ForMember(dest => dest.CategoryName, opt => opt.Ignore())  // Manual dolduracaqsan
            .ForMember(dest => dest.FavoritesCount, opt => opt.Ignore())
            .ForMember(dest => dest.ReviewsCount, opt => opt.Ignore());

        CreateMap<ProductUpdateDto, Product>();
    }
}


