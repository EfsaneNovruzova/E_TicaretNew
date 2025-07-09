using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;
using E_TicaretNew.Application.Abstracts.Services;
using E_TicaretNew.Application.DTOs.ReviewDTOs;
using E_TicaretNew.Application.Shared;

[Route("api/[controller]")]
[ApiController]
public class ReviewsController : ControllerBase
{
    private readonly IReviewService _reviewService;

    public ReviewsController(IReviewService reviewService)
    {
        _reviewService = reviewService;
    }

    /// <summary>
    /// Yeni review əlavə et (yalnız məhsul almış istifadəçi).
    /// </summary>
    [HttpPost]
    [Authorize(Policy = Permissions.Review.Create)]
    public async Task<IActionResult> Create([FromBody] ReviewCreateDto dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var response = await _reviewService.CreateAsync(dto, userId);
        return StatusCode((int)response.StatusCode, response);
    }

    /// <summary>
    /// Müəyyən məhsula aid bütün review-ları getir (ictimai).
    /// </summary>
    [HttpGet("product/{productId}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetByProduct(Guid productId)
    {
        var response = await _reviewService.GetByProductIdAsync(productId);
        return StatusCode((int)response.StatusCode, response);
    }

    /// <summary>
    /// Review sil (yalnız admin və ya sahibi).
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize(Policy = Permissions.Review.Delete)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var isAdmin = User.IsInRole("Admin");

        var response = await _reviewService.DeleteAsync(id, userId, isAdmin);
        return StatusCode((int)response.StatusCode, response);
    }
}

