using System.Linq.Expressions;
using System.Net;
using E_TicaretNew.Application.Abstracts.Repositories;
using E_TicaretNew.Application.Abstracts.Services;
using E_TicaretNew.Application.DTOs.FavoriteDTOs;
using E_TicaretNew.Application.Shared.Responses;
using E_TicaretNew.Domain.Entities;
using Microsoft.EntityFrameworkCore;

public class FavoriteService : IFavoriteService
{
    private readonly IRepository<Favorite> _favoriteRepository;
    private readonly IRepository<Product> _productRepository;

    public FavoriteService(IRepository<Favorite> favoriteRepository,
                           IRepository<Product> productRepository)
    {
        _favoriteRepository = favoriteRepository;
        _productRepository = productRepository;
    }

    public async Task<BaseResponse<string>> ToggleFavoriteAsync(Guid productId, string userId)
    {
        var product = await _productRepository.GetByIdAsync(productId);
        if (product == null)
            return new BaseResponse<string>("Product not found.", HttpStatusCode.NotFound);

        var existing = _favoriteRepository.GetByFiltered(
            f => f.UserId == userId && f.ProductId == productId,
            null,
            true
        ).FirstOrDefault();

        if (existing != null)
        {
            _favoriteRepository.Delete(existing);
            await _favoriteRepository.SaveChangeAsync();
            return new BaseResponse<string>("Removed from favorites.", "OK", HttpStatusCode.OK);
        }

        var newFavorite = new Favorite
        {
            UserId = userId,
            ProductId = productId
        };

        await _favoriteRepository.AddAsync(newFavorite);
        await _favoriteRepository.SaveChangeAsync();
        return new BaseResponse<string>("Added to favorites.", "OK", HttpStatusCode.Created);
    }

    public async Task<BaseResponse<List<FavoriteGetDto>>> GetMyFavoritesAsync(string userId)
    {
        var favorites = _favoriteRepository.GetByFiltered(
            f => f.UserId == userId,
            new Expression<Func<Favorite, object>>[] { f => f.Product }
        );

        var list = await favorites.Select(f => new FavoriteGetDto
        {
            Id = f.Id,
            ProductId = f.ProductId,
            ProductName = f.Product.Name
        }).ToListAsync();

        return new BaseResponse<List<FavoriteGetDto>>("Fetched successfully.", list, HttpStatusCode.OK);
    }
}
