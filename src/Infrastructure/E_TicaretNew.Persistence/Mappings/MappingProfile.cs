using AutoMapper;
using E_TicaretNew.Application.DTOs.ProductDTOs;
using E_TicaretNew.Domain.Entities;

using AutoMapper;
using E_TicaretNew.Application.DTOs.ProductDTOs;
using E_TicaretNew.Domain.Entities;
using E_TicaretNew.Application.DTOs.OrderDTOs;
using E_TicaretNew.Application.DTOs.OrderProductDTOs;

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


        // Order mappings
        CreateMap<OrderCreateDto, Order>()
            // UserId Controller-da verilir, dto-da olmasa da olar
            .ForMember(dest => dest.Status, opt => opt.Ignore()) // Status yaratmada Pending olacaq servis tərəfindən
            .ForMember(dest => dest.TotalAmount, opt => opt.Ignore()) // Hesablanacaq servis tərəfindən
            .ForMember(dest => dest.OrderProducts, opt => opt.Ignore()); // Manual dolduracaqsan servisdə

        CreateMap<Order, OrderGetDto>()
            .ForMember(dest => dest.Products, opt => opt.MapFrom(src => src.OrderProducts));

        CreateMap<OrderUpdateDto, Order>()
            .ForMember(dest => dest.OrderProducts, opt => opt.Ignore()); // Manual idarə etmək lazımdır

        // OrderProduct mappings
        CreateMap<OrderProductCreateDto, OrderProduct>();
        CreateMap<OrderProduct, OrderProductGetDto>();
        CreateMap<OrderProductUpdateDto, OrderProduct>();
    }
}
 



