using E_TicaretNew.Application.Abstracts.Services;
using E_TicaretNew.Application.DTOs.OrderDTOs;
using E_TicaretNew.Domain.Enums.OrderEnum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public OrdersController(IOrderService orderService, IHttpContextAccessor httpContextAccessor)
    {
        _orderService = orderService;
        _httpContextAccessor = httpContextAccessor;
    }

    private string GetUserId()
    {
        return _httpContextAccessor.HttpContext.User.FindFirst("nameid")?.Value;
    }

    [HttpPost]
    [Authorize(Policy = "BuyerPolicy")]  // Yalnız Buyer sifariş verə bilər
    public async Task<IActionResult> Create([FromBody] OrderCreateDto dto)
    {
        var userId = GetUserId();
        var result = await _orderService.CreateAsync(dto, userId);
        return StatusCode((int)result.StatusCode, result);
    }

    [HttpGet("my")]
    [Authorize(Policy = "BuyerPolicy")]
    public async Task<IActionResult> GetMyOrders(int pageNumber = 1, int pageSize = 10)
    {
        var userId = GetUserId();
        var result = await _orderService.GetMyOrdersAsync(userId, pageNumber, pageSize);
        return Ok(result);
    }

    [HttpGet("my-sales")]
    [Authorize(Policy = "SellerPolicy")]
    public async Task<IActionResult> GetMySales(int pageNumber = 1, int pageSize = 10)
    {
        var userId = GetUserId();
        var result = await _orderService.GetMySalesAsync(userId, pageNumber, pageSize);
        return Ok(result);
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<IActionResult> GetById(Guid id)
    {
        var userId = GetUserId();
        var result = await _orderService.GetByIdAsync(id, userId);
        if (!result.Success)
            return StatusCode((int)result.StatusCode, result);
        return Ok(result);
    }

    [HttpPut("{id}/status")]
    [Authorize(Policy = "OrderStatusChangePolicy")] // Status dəyişmək üçün xüsusi policy
    public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] OrderStatus newStatus)
    {
        var userId = GetUserId();
        var result = await _orderService.UpdateStatusAsync(id, newStatus, userId);
        return StatusCode((int)result.StatusCode, result);
    }

    [HttpPost("{id}/cancel")]
    [Authorize(Policy = "BuyerPolicy")]
    public async Task<IActionResult> CancelOrder(Guid id)
    {
        var userId = GetUserId();
        var result = await _orderService.CancelOrderAsync(id, userId);
        return StatusCode((int)result.StatusCode, result);
    }
}
