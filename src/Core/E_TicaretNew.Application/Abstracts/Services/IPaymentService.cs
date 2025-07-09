using E_TicaretNew.Application.DTOs.PaymentDTOs;
using E_TicaretNew.Application.Shared.Responses;
using E_TicaretNew.Domain.Entities;

namespace E_TicaretNew.Application.Abstracts.Services;

public interface IPaymentService
{
    Task<BaseResponse<Payment>> GetByIdAsync(Guid id);
    Task<BaseResponse<List<Payment>>> GetAllAsync();
    Task<BaseResponse<Payment>> CreateAsync(PaymentCreateDto dto);
    Task<BaseResponse<Payment>> UpdateAsync(Payment payment);
    Task<BaseResponse<string>> DeleteAsync(Guid id);
}
