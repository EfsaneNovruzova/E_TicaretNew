using E_TicaretNew.Application.DTOs.OrderDTOs;
using E_TicaretNew.Application.Shared.Responses;
using E_TicaretNew.Domain.Entities;
using E_TicaretNew.Domain.Enums.OrderEnum;

namespace E_TicaretNew.Application.Abstracts.Services;

public interface IOrderService
{
    Task<BaseResponse<Order>> CreateAsync(OrderCreateDto dto, string userId);

    Task<BaseResponse<List<OrderGetDto>>> GetMyOrdersAsync(string userId, int pageNumber, int pageSize);

    Task<BaseResponse<List<OrderGetDto>>> GetMySalesAsync(string userId, int pageNumber, int pageSize);

    Task<BaseResponse<OrderGetDto>> GetByIdAsync(Guid id, string userId);

    Task<BaseResponse<string>> UpdateStatusAsync(Guid orderId, OrderStatus newStatus, string userId);

    Task<BaseResponse<string>> CancelOrderAsync(Guid orderId, string userId);
}


