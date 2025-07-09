namespace E_TicaretNew.Persistence.Services;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net;
using Microsoft.EntityFrameworkCore;
using E_TicaretNew.Application.Abstracts.Repositories;
using E_TicaretNew.Application.Abstracts.Services;
using E_TicaretNew.Application.Shared.Responses;
using E_TicaretNew.Domain.Entities;
using E_TicaretNew.Application.DTOs.PaymentDTOs;
using E_TicaretNew.Domain.Enums.OrderEnum;
using E_TicaretNew.Persistence.Repositories;

public class PaymentService : IPaymentService
{
    private readonly IRepository<Payment> _paymentRepository;
    private readonly IOrderRepository _orderRepository;

    public PaymentService(IRepository<Payment> paymentRepository, IOrderRepository orderRepository)
    {
        _paymentRepository = paymentRepository;
        _orderRepository = orderRepository;
    }

    public async Task<BaseResponse<List<Payment>>> GetAllAsync()
    {
        var payments = await _paymentRepository.GetAll().ToListAsync();
        return new BaseResponse<List<Payment>>("Payments retrieved successfully", payments, HttpStatusCode.OK);
    }

    public async Task<BaseResponse<Payment>> GetByIdAsync(Guid id)
    {
        var payment = await _paymentRepository.GetByIdAsync(id);
        if (payment == null)
            return new BaseResponse<Payment>("Payment not found", HttpStatusCode.NotFound);

        return new BaseResponse<Payment>("Payment retrieved successfully", payment, HttpStatusCode.OK);
    }

    public async Task<BaseResponse<Payment>> CreateAsync(PaymentCreateDto dto)
    {
        var order = await _orderRepository.GetByIdAsync(dto.OrderId);
        if (order == null)
            return new BaseResponse<Payment>("Order not found", HttpStatusCode.BadRequest);

        if (order.Status != OrderStatus.PendingPayment)
            return new BaseResponse<Payment>("Order is not awaiting payment", HttpStatusCode.BadRequest);

        if (dto.Amount != order.TotalAmount)
            return new BaseResponse<Payment>("Payment amount does not match order total", HttpStatusCode.BadRequest);

        var payment = new Payment
        {
            Id = Guid.NewGuid(),
            OrderId = dto.OrderId,
            Amount = dto.Amount,
            PaymentMethod = dto.PaymentMethod,
            TransactionId = dto.TransactionId,
            IsSuccessful = true, // Əslində ödəniş sistemi ilə təsdiqlənməlidir
            CreatedAt = DateTime.UtcNow
        };

        await _paymentRepository.AddAsync(payment);

        // Order statusunu yenilə
        order.Status = OrderStatus.Paid;
        _orderRepository.Update(order);

        await _paymentRepository.SaveChangeAsync();
        await _orderRepository.SaveChangeAsync();

        return new BaseResponse<Payment>("Payment successful and order updated", payment, HttpStatusCode.Created);
    }

    public async Task<BaseResponse<Payment>> UpdateAsync(Payment payment)
    {
        var existingPayment = await _paymentRepository.GetByIdAsync(payment.Id);
        if (existingPayment == null)
            return new BaseResponse<Payment>("Payment not found", HttpStatusCode.NotFound);

        existingPayment.Amount = payment.Amount;
        existingPayment.PaymentMethod = payment.PaymentMethod;
        existingPayment.IsSuccessful = payment.IsSuccessful;
        existingPayment.TransactionId = payment.TransactionId;
        // OrderId və Order əgər dəyişdirilməməlidirsə, burda dəyişmə

        _paymentRepository.Update(existingPayment);
        await _paymentRepository.SaveChangeAsync();

        return new BaseResponse<Payment>("Payment updated successfully", existingPayment, HttpStatusCode.OK);
    }

    public async Task<BaseResponse<string>> DeleteAsync(Guid id)
    {
        var payment = await _paymentRepository.GetByIdAsync(id);
        if (payment == null)
            return new BaseResponse<string>("Payment not found", HttpStatusCode.NotFound);

        _paymentRepository.Delete(payment);
        await _paymentRepository.SaveChangeAsync();

        return new BaseResponse<string>("Payment deleted successfully", HttpStatusCode.OK);
    }
}

