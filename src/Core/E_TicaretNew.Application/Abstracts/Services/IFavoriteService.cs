using E_TicaretNew.Application.DTOs.FavoriteDTOs;
using E_TicaretNew.Application.Shared.Responses;

namespace E_TicaretNew.Application.Abstracts.Services;

public interface IFavoriteService
{
    Task<BaseResponse<string>> ToggleFavoriteAsync(Guid productId, string userId);
    Task<BaseResponse<List<FavoriteGetDto>>> GetMyFavoritesAsync(string userId);
}
