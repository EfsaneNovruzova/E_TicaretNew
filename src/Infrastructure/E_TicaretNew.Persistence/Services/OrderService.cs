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

    public OrderService(IRepository<Order> orderRepository,
                        IRepository<Product> productRepository,
                        IRepository<Payment> paymentRepository,
                        IMapper mapper)
    {
        _orderRepository = orderRepository;
        _productRepository = productRepository;
        _paymentRepository = paymentRepository;
        _mapper = mapper;
    }

    public async Task<BaseResponse<Order>> CreateAsync(OrderCreateDto dto, string userId)
    {
        decimal totalAmount = 0;
        var orderItems = new List<OrderProduct>();

        foreach (var item in dto.Products)
        {
            var product = await _productRepository.GetByIdAsync(item.ProductId);
            if (product == null)
                return new BaseResponse<Order>($"Product {item.ProductId} not found", HttpStatusCode.BadRequest);

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
            Status = OrderStatus.PendingPayment,
            CreatedAt = DateTime.UtcNow
        };

        await _orderRepository.AddAsync(order);
        await _orderRepository.SaveChangeAsync();

        return new BaseResponse<Order>("Order created successfully", order, HttpStatusCode.Created);
    }

    public async Task<BaseResponse<List<OrderGetDto>>> GetMyOrdersAsync(string userId, int pageNumber, int pageSize)
    {
        Expression<Func<Order, object>>[] includes = { o => o.OrderProducts, o => o.OrderProducts.Select(op => op.Product) };

        var query = _orderRepository.GetByFiltered(o => o.UserId == userId, includes);
        var orders = await query
            .OrderByDescending(o => o.CreatedAt)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var result = orders.Select(o => _mapper.Map<OrderGetDto>(o)).ToList();

        return new BaseResponse<List<OrderGetDto>>("Success", result, HttpStatusCode.OK);
    }

    public async Task<BaseResponse<List<OrderGetDto>>> GetMySalesAsync(string userId, int pageNumber, int pageSize)
    {
        // Satıcı kimi məhsullarına gələn sifarişlər
        Expression<Func<Order, object>>[] includes = { o => o.OrderProducts, o => o.OrderProducts.Select(op => op.Product) };

        var query = _orderRepository.GetByFiltered(
            o => o.OrderProducts.Any(op => op.Product.UserId == userId),
            includes);

        var orders = await query
            .OrderByDescending(o => o.CreatedAt)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var result = orders.Select(o => _mapper.Map<OrderGetDto>(o)).ToList();

        return new BaseResponse<List<OrderGetDto>>("Success", result, HttpStatusCode.OK);
    }

    public async Task<BaseResponse<OrderGetDto>> GetByIdAsync(Guid id, string userId)
    {
        Expression<Func<Order, object>>[] includes = { o => o.OrderProducts, o => o.OrderProducts.Select(op => op.Product) };

        var order = _orderRepository.GetByFiltered(o => o.Id == id, includes).FirstOrDefault();

        if (order == null)
            return new BaseResponse<OrderGetDto>("Order not found", HttpStatusCode.NotFound);

        // Yalnız order sahibi və ya satıcı görə bilər
        bool isOwner = order.UserId == userId;
        bool isSeller = order.OrderProducts.Any(op => op.Product.UserId == userId);

        if (!isOwner && !isSeller)
            return new BaseResponse<OrderGetDto>("Unauthorized", HttpStatusCode.Unauthorized);

        var dto = _mapper.Map<OrderGetDto>(order);

        return new BaseResponse<OrderGetDto>("Success", dto, HttpStatusCode.OK);
    }

    public async Task<BaseResponse<string>> UpdateStatusAsync(Guid orderId, OrderStatus newStatus, string userId)
    {
        var order = await _orderRepository.GetByIdAsync(orderId);
        if (order == null)
            return new BaseResponse<string>("Order not found", HttpStatusCode.NotFound);

        // Statusu dəyişmək hüququ:
        // Seller yalnız öz məhsullarının sifarişlərinə status dəyişə bilər
        bool isSeller = order.OrderProducts.Any(op => op.Product.UserId == userId);
        // Buyer yalnız öz sifarişində status dəyişə bilər
        bool isBuyer = order.UserId == userId;

        if (!isSeller && !isBuyer)
            return new BaseResponse<string>("Unauthorized", HttpStatusCode.Unauthorized);

        // Sadə qayda: seller statusu Processing, Shipped, Delivered edə bilər
        // buyer isə yalnız Delivered statusunu təsdiqləyə bilər
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

