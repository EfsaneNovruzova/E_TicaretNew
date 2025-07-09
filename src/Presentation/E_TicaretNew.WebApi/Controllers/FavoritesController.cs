using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

using System.Net;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using E_TicaretNew.Application.Abstracts.Services;
using E_TicaretNew.Application.DTOs.FavoriteDTOs;
using E_TicaretNew.Application.Shared.Responses;
using E_TicaretNew.Application.Shared;

namespace E_TicaretNew.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FavoritesController : ControllerBase
    {
        private readonly IFavoriteService _favoriteService;

        public FavoritesController(IFavoriteService favoriteService)
        {
            _favoriteService = favoriteService;
        }

        [HttpPost("{productId}")]
        [Authorize(Policy = Permissions.Favorite.Toggle)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.Conflict)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Toggle(Guid productId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId is null)
                return StatusCode((int)HttpStatusCode.Unauthorized,
                    new BaseResponse<string>("Unauthorized user.", HttpStatusCode.Unauthorized));

            var result = await _favoriteService.ToggleFavoriteAsync(productId, userId);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("my")]
        [Authorize(Policy = Permissions.Favorite.View)]
        [ProducesResponseType(typeof(BaseResponse<List<FavoriteGetDto>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(BaseResponse<List<FavoriteGetDto>>), (int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> GetMyFavorites()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId is null)
                return StatusCode((int)HttpStatusCode.Unauthorized,
                    new BaseResponse<string>("Unauthorized user.", HttpStatusCode.Unauthorized));

            var result = await _favoriteService.GetMyFavoritesAsync(userId);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}
