using E_TicaretNew.Application.DTOs.ReviewDTOs;
using E_TicaretNew.Application.Shared.Responses;

namespace E_TicaretNew.Application.Abstracts.Services;

public interface IReviewService
{
    Task<BaseResponse<string>> CreateAsync(ReviewCreateDto dto, string userId);
    Task<BaseResponse<List<ReviewGetDto>>> GetByProductIdAsync(Guid productId);
    Task<BaseResponse<string>> DeleteAsync(Guid id, string userId, bool isAdmin);
}
