using AutoMapper;
using AutoMapper;
using E_TicaretNew.Application.Abstracts.Repositories;
using E_TicaretNew.Application.Abstracts.Services;
using E_TicaretNew.Application.DTOs.OrderDTOs;
using E_TicaretNew.Application.Shared.Responses;
using E_TicaretNew.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Linq.Expressions;
using E_TicaretNew.Domain.Enums.OrderEnum;



public class OrderService : IOrderService
{
    private readonly IRepository<Order> _orderRepository;
    private readonly IRepository<Product> _productRepository;
    private readonly IRepository<Payment> _paymentRepository;
    private readonly IMapper _mapper;

    public OrderService(
        IRepository<Order> orderRepository,
        IRepository<Product> productRepository,
        IRepository<Payment> paymentRepository,
        IMapper mapper)
    {
        _orderRepository = orderRepository;
        _productRepository = productRepository;
        _paymentRepository = paymentRepository;
        _mapper = mapper;
    }

    // 1. Sifariş yaratmaq
    public async Task<BaseResponse<Order>> CreateAsync(OrderCreateDto dto, string userId)
    {
        decimal totalAmount = 0;
        var orderItems = new List<OrderProduct>();

        foreach (var item in dto.Products)
        {
            var product = await _productRepository.GetByIdAsync(item.ProductId);
            if (product == null)
                return new BaseResponse<Order>($"Product {item.ProductId} not found", HttpStatusCode.BadRequest);

            // Qiymət hesablanır
            totalAmount += product.Price * item.Quantity;

            orderItems.Add(new OrderProduct
            {
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                UnitPrice = product.Price
            });
        }

        var order = new Order
        {
            UserId = userId,
            OrderProducts = orderItems,
            TotalAmount = totalAmount,
            CreatedAt = DateTime.UtcNow
        };

        // Əgər ödəniş ID-si varsa, status "Paid" olur, yoxdursa "PendingPayment"
        if (dto.PaymentId is not null)
        {
            order.PaymentId = dto.PaymentId;
            order.Status = OrderStatus.Paid;
        }
        else
        {
            order.Status = OrderStatus.PendingPayment;
        }

        await _orderRepository.AddAsync(order);
        await _orderRepository.SaveChangeAsync();

        string message = order.Status == OrderStatus.Paid
            ? "Sifariş və ödəniş uğurla tamamlandı."
            : "Sifariş uğurla yaradıldı. Ödəniş gözlənilir.";

        return new BaseResponse<Order>(message, order, HttpStatusCode.Created);
    }

    // 2. İstifadəçinin sifarişlərini almaq
    public async Task<BaseResponse<List<OrderGetDto>>> GetMyOrdersAsync(string userId, int pageNumber, int pageSize)
    {
        var query = _orderRepository.GetByFiltered(o => o.UserId == userId);

        var orders = await query
            .Include(o => o.OrderProducts)
                .ThenInclude(op => op.Product)
            .OrderByDescending(o => o.CreatedAt)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var result = orders.Select(o => _mapper.Map<OrderGetDto>(o)).ToList();

        return new BaseResponse<List<OrderGetDto>>("Success", result, HttpStatusCode.OK);
    }

    // 3. Satıcının satışlarını almaq (onun məhsullarına gələn sifarişlər)
    public async Task<BaseResponse<List<OrderGetDto>>> GetMySalesAsync(string userId, int pageNumber, int pageSize)
    {
        var query = _orderRepository.GetByFiltered(
            o => o.OrderProducts.Any(op => op.Product.UserId == userId));

        var orders = await query
            .Include(o => o.OrderProducts)
                .ThenInclude(op => op.Product)
            .OrderByDescending(o => o.CreatedAt)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var result = orders.Select(o => _mapper.Map<OrderGetDto>(o)).ToList();

        return new BaseResponse<List<OrderGetDto>>("Success", result, HttpStatusCode.OK);
    }

    // 4. İstifadəçi üçün sifarişin detallarını almaq
    public async Task<BaseResponse<OrderGetDto>> GetByIdAsync(Guid id, string userId)
    {
        var order = await _orderRepository.GetByFiltered(o => o.Id == id)
            .Include(o => o.OrderProducts)
                .ThenInclude(op => op.Product)
            .FirstOrDefaultAsync();

        if (order == null)
            return new BaseResponse<OrderGetDto>("Order not found", HttpStatusCode.NotFound);

        bool isOwner = order.UserId == userId;
        bool isSeller = order.OrderProducts.Any(op => op.Product.UserId == userId);

        if (!isOwner && !isSeller)
            return new BaseResponse<OrderGetDto>("Unauthorized", HttpStatusCode.Unauthorized);

        var dto = _mapper.Map<OrderGetDto>(order);

        return new BaseResponse<OrderGetDto>("Success", dto, HttpStatusCode.OK);
    }

    // 5. Sifariş statusunu yeniləmək (satıcı və ya alıcı üçün uyğun statuslar)
    public async Task<BaseResponse<string>> UpdateStatusAsync(Guid orderId, OrderStatus newStatus, string userId)
    {
        var includes = new Expression<Func<Order, object>>[]
        {
        o => o.OrderProducts,
        o => o.OrderProducts.Select(op => op.Product)
        };

        var order = _orderRepository.GetByFiltered(o => o.Id == orderId, includes).FirstOrDefault();

        if (order == null)
            return new BaseResponse<string>("Order not found", HttpStatusCode.NotFound);

        // order.OrderProducts null yoxlama
        if (order.OrderProducts == null || !order.OrderProducts.Any())
            return new BaseResponse<string>("Order products not loaded", HttpStatusCode.InternalServerError);

        bool isSeller = order.OrderProducts.Any(op => op.Product.UserId == userId);
        bool isBuyer = order.UserId == userId;

        if (!isSeller && !isBuyer)
            return new BaseResponse<string>("Unauthorized", HttpStatusCode.Unauthorized);

        if (isSeller)
        {
            if (newStatus != OrderStatus.Processing && newStatus != OrderStatus.Shipped && newStatus != OrderStatus.Delivered)
                return new BaseResponse<string>("Seller cannot set this status", HttpStatusCode.Forbidden);
        }
        else if (isBuyer)
        {
            if (newStatus != OrderStatus.Delivered)
                return new BaseResponse<string>("Buyer can only confirm delivery", HttpStatusCode.Forbidden);
        }

        order.Status = newStatus;
        _orderRepository.Update(order);
        await _orderRepository.SaveChangeAsync();

        return new BaseResponse<string>("Order status updated", HttpStatusCode.OK);
    }

    // 6. Sifarişi ləğv etmək (yalnız PendingPayment statusunda olan və sahibi tərəfindən)
    public async Task<BaseResponse<string>> CancelOrderAsync(Guid orderId, string userId)
    {
        var order = await _orderRepository.GetByIdAsync(orderId);
        if (order == null)
            return new BaseResponse<string>("Order not found", HttpStatusCode.NotFound);

        if (order.UserId != userId)
            return new BaseResponse<string>("Unauthorized", HttpStatusCode.Unauthorized);

        if (order.Status != OrderStatus.PendingPayment)
            return new BaseResponse<string>("Only pending orders can be cancelled", HttpStatusCode.BadRequest);

        order.Status = OrderStatus.Cancelled;
        _orderRepository.Update(order);
        await _orderRepository.SaveChangeAsync();

        return new BaseResponse<string>("Order cancelled", HttpStatusCode.OK);
    }
}

