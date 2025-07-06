using E_TicaretNew.Application.DTOs.ProductDTOs;
using E_TicaretNew.Application.Shared.Responses;
using Microsoft.AspNetCore.Http;

namespace E_TicaretNew.Application.Abstracts.Services;

public interface IProductService
{
    Task<BaseResponse<ProductGetDto>> GetByIdAsync(Guid id);
    Task<BaseResponse<List<ProductGetDto>>> GetAllAsync(ProductFilterDto filter);
    Task<BaseResponse<List<ProductGetDto>>> GetMyProductsAsync(string userId);
    Task<BaseResponse<string>> CreateAsync(ProductCreateDto dto);
    Task<BaseResponse<string>> UpdateAsync(ProductUpdateDto dto, string userId);
    Task<BaseResponse<string>> DeleteAsync(Guid id, string userId);
   
}
