using AutoMapper;
using E_TicaretNew.Application.Abstracts.Repositories;
using E_TicaretNew.Application.Abstracts.Services;
using E_TicaretNew.Application.DTOs.ProductDTOs;
using E_TicaretNew.Application.Shared.Responses;
using E_TicaretNew.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Net;

public class ProductService : IProductService
{
    private readonly IRepository<Product> _productRepository;
    private readonly IRepository<Category> _categoryRepository;
    private readonly IMapper _mapper;

    public ProductService(IRepository<Product> productRepository,
                          IRepository<Category> categoryRepository,
                          IMapper mapper)
    {
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
        _mapper = mapper;
    }

    public async Task<BaseResponse<string>> CreateAsync(ProductCreateDto dto, string userId)
    {
        var product = _mapper.Map<Product>(dto);

        product.UserId = userId; // <<<< BU MÜTLƏQDİR!!!

        await _productRepository.AddAsync(product);
        await _productRepository.SaveChangeAsync();

        return new BaseResponse<string>("Product created", "Success", HttpStatusCode.Created);
    }



    public async Task<BaseResponse<string>> UpdateAsync(ProductUpdateDto dto, string userId)
    {
        var product = await _productRepository.GetByIdAsync(dto.Id);
        if (product == null)
            return new BaseResponse<string>("Product not found", HttpStatusCode.NotFound);

        if (product.UserId != userId)
            return new BaseResponse<string>("Unauthorized", HttpStatusCode.Unauthorized);

        _mapper.Map(dto, product); // avtomatik map et

        _productRepository.Update(product);
        await _productRepository.SaveChangeAsync();

        return new BaseResponse<string>("Product updated", "Success", HttpStatusCode.OK);
    }


    public async Task<BaseResponse<string>> DeleteAsync(Guid id, string userId)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if (product == null)
            return new BaseResponse<string>("Product not found", HttpStatusCode.NotFound);

        if (product.UserId != userId)
            return new BaseResponse<string>("Unauthorized", HttpStatusCode.Unauthorized);

        _productRepository.Delete(product);
        await _productRepository.SaveChangeAsync();

        return new BaseResponse<string>("Product deleted", "Success", HttpStatusCode.OK);
    }


    public async Task<BaseResponse<ProductGetDto>> GetByIdAsync(Guid id)
    {
        Expression<Func<Product, object>>[] includes = { p => p.Category, p => p.Favorites, p => p.Reviews };
        var product = _productRepository
            .GetByFiltered(p => p.Id == id, includes)
            .FirstOrDefault();

        if (product == null)
            return new BaseResponse<ProductGetDto>("Product not found", HttpStatusCode.NotFound);

        var dto = _mapper.Map<ProductGetDto>(product);
        dto.CategoryName = product.Category?.Name;
        dto.FavoritesCount = product.Favorites?.Count ?? 0;
        dto.ReviewsCount = product.Reviews?.Count ?? 0;

        return new BaseResponse<ProductGetDto>("Success", dto, HttpStatusCode.OK);
    }


    public async Task<BaseResponse<List<ProductGetDto>>> GetAllAsync(ProductFilterDto filter)
    {
        Expression<Func<Product, object>>[] includes = { p => p.Category, p => p.Favorites, p => p.Reviews };
        var query = _productRepository.GetByFiltered(null, includes);

        if (filter.CategoryId.HasValue)
            query = query.Where(p => p.CategoryId == filter.CategoryId.Value);

        if (filter.MinPrice.HasValue)
            query = query.Where(p => p.Price >= filter.MinPrice.Value);

        if (filter.MaxPrice.HasValue)
            query = query.Where(p => p.Price <= filter.MaxPrice.Value);

        if (!string.IsNullOrWhiteSpace(filter.Search))
            query = query.Where(p => p.Name.Contains(filter.Search));

        var products = await query.ToListAsync();

        var result = products.Select(p =>
        {
            var dto = _mapper.Map<ProductGetDto>(p);
            dto.CategoryName = p.Category?.Name;
            dto.FavoritesCount = p.Favorites?.Count ?? 0;
            dto.ReviewsCount = p.Reviews?.Count ?? 0;
            return dto;
        }).ToList();

        return new BaseResponse<List<ProductGetDto>>("Success", result, HttpStatusCode.OK);
    }


    public async Task<BaseResponse<List<ProductGetDto>>> GetMyProductsAsync(string userId)
    {
        Expression<Func<Product, object>>[] includes = { p => p.Category, p => p.Favorites, p => p.Reviews };
        var products = await _productRepository
            .GetByFiltered(p => p.UserId == userId, includes)
            .ToListAsync();

        var result = products.Select(p =>
        {
            var dto = _mapper.Map<ProductGetDto>(p);
            dto.CategoryName = p.Category?.Name;
            dto.FavoritesCount = p.Favorites?.Count ?? 0;
            dto.ReviewsCount = p.Reviews?.Count ?? 0;
            return dto;
        }).ToList();

        return new BaseResponse<List<ProductGetDto>>("Success", result, HttpStatusCode.OK);
    }

}

