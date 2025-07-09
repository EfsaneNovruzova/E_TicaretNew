using System.Net;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using E_TicaretNew.Application.Abstracts.Repositories;
using E_TicaretNew.Application.Abstracts.Services;
using E_TicaretNew.Application.DTOs.ReviewDTOs;
using E_TicaretNew.Application.Shared.Responses;
using E_TicaretNew.Domain.Entities;
using E_TicaretNew.Domain.Enums.OrderEnum;

public class ReviewService : IReviewService
{
    private readonly IRepository<Review> _reviewRepository;
    private readonly IRepository<Product> _productRepository;
    private readonly IRepository<Order> _orderRepository;
    private readonly IMapper _mapper;

    public ReviewService(
        IRepository<Review> reviewRepository,
        IRepository<Product> productRepository,
        IRepository<Order> orderRepository,
        IMapper mapper)
    {
        _reviewRepository = reviewRepository;
        _productRepository = productRepository;
        _orderRepository = orderRepository;
        _mapper = mapper;
    }

    public async Task<BaseResponse<string>> CreateAsync(ReviewCreateDto dto, string userId)
    {
        var product = await _productRepository.GetByIdAsync(dto.ProductId);
        if (product == null)
            return new BaseResponse<string>("Product not found.", HttpStatusCode.NotFound);

        // İstifadəçi məhsulu alıbsa və status completed-dirsə
        var hasPurchased = _orderRepository.GetByFiltered(
            o => o.UserId == userId &&
                 o.Status == OrderStatus.Completed &&
                 o.OrderProducts.Any(op => op.ProductId == dto.ProductId),
            new Expression<Func<Order, object>>[] { o => o.OrderProducts }
        ).Any();

        if (!hasPurchased)
            return new BaseResponse<string>("You can only review products you have purchased.", HttpStatusCode.Forbidden);

        // Eyni məhsula təkrar rəyin qarşısını al
        var alreadyReviewed = _reviewRepository.GetByFiltered(r =>
            r.ProductId == dto.ProductId && r.UserId == userId).Any();

        if (alreadyReviewed)
            return new BaseResponse<string>("You have already reviewed this product.", HttpStatusCode.Conflict);

        var review = _mapper.Map<Review>(dto);
        review.UserId = userId;

        await _reviewRepository.AddAsync(review);
        await _reviewRepository.SaveChangeAsync();

        return new BaseResponse<string>("Review successfully added.", "OK", HttpStatusCode.Created);
    }

    public async Task<BaseResponse<List<ReviewGetDto>>> GetByProductIdAsync(Guid productId)
    {
        var reviews = _reviewRepository.GetByFiltered(
            r => r.ProductId == productId,
            new Expression<Func<Review, object>>[] { r => r.User }
        );

        var dtoList = await reviews.Select(r => new ReviewGetDto
        {
            Id = r.Id,
            Comment = r.Comment,
            Rating = r.Rating,
            UserName = r.User.UserName
        }).ToListAsync();

        return new BaseResponse<List<ReviewGetDto>>("Fetched successfully.", dtoList, HttpStatusCode.OK);
    }

    public async Task<BaseResponse<string>> DeleteAsync(Guid id, string userId, bool isAdmin)
    {
        var review = await _reviewRepository.GetByFiltered(
            r => r.Id == id,
            null,
            true
        ).FirstOrDefaultAsync();

        if (review == null)
            return new BaseResponse<string>("Review not found.", HttpStatusCode.NotFound);

        if (review.UserId != userId && !isAdmin)
            return new BaseResponse<string>("You are not authorized to delete this review.", HttpStatusCode.Forbidden);

        _reviewRepository.Delete(review);
        await _reviewRepository.SaveChangeAsync();

        return new BaseResponse<string>("Review deleted successfully.", "OK", HttpStatusCode.OK);
    }
}



