using E_TicaretNew.Application.Abstracts.Services;
using E_TicaretNew.Application.DTOs.OrderDTOs;
using E_TicaretNew.Application.Shared.Responses;
using E_TicaretNew.Application.Shared;
using E_TicaretNew.Domain.Enums.OrderEnum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;
using E_TicaretNew.Domain.Entities;

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
        return _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;
    }

    [HttpPost]
    [Authorize(Policy = Permissions.Order.Create)]
    [ProducesResponseType(typeof(BaseResponse<Order>), (int)HttpStatusCode.Created)]
    [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> Create(OrderCreateDto dto)
    {
        var userId = GetUserId();
        if (string.IsNullOrEmpty(userId))
            return Unauthorized(new BaseResponse<string>("UserId not found in token.", HttpStatusCode.Unauthorized));

        var response = await _orderService.CreateAsync(dto, userId);
        return StatusCode((int)response.StatusCode, response);
    }

    [HttpGet("my")]
    [Authorize(Policy = Permissions.Order.GetMyOrders)]
    [ProducesResponseType(typeof(BaseResponse<List<OrderGetDto>>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> GetMyOrders(int pageNumber = 1, int pageSize = 10)
    {
        var userId = GetUserId();
        var result = await _orderService.GetMyOrdersAsync(userId, pageNumber, pageSize);
        return StatusCode((int)result.StatusCode, result);
    }

    [HttpGet("my-sales")]
    [Authorize(Policy = Permissions.Order.GetMySales)]
    [ProducesResponseType(typeof(BaseResponse<List<OrderGetDto>>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> GetMySales(int pageNumber = 1, int pageSize = 10)
    {
        var userId = GetUserId();
        var result = await _orderService.GetMySalesAsync(userId, pageNumber, pageSize);
        return StatusCode((int)result.StatusCode, result);
    }

    [HttpGet("{id}")]
    [Authorize]
    [ProducesResponseType(typeof(BaseResponse<OrderGetDto>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var userId = GetUserId();
        var result = await _orderService.GetByIdAsync(id, userId);
        return StatusCode((int)result.StatusCode, result);
    }

    [HttpPut("{id}/status")]
    [Authorize(Policy = Permissions.Order.UpdateStatus)]
    [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.Forbidden)]
    [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] OrderStatus newStatus)
    {
        var userId = GetUserId();
        var result = await _orderService.UpdateStatusAsync(id, newStatus, userId);
        return StatusCode((int)result.StatusCode, result);
    }

    [HttpPost("{id}/cancel")]
    [Authorize(Policy = Permissions.Order.Cancel)]
    [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> CancelOrder(Guid id)
    {
        var userId = GetUserId();
        var result = await _orderService.CancelOrderAsync(id, userId);
        return StatusCode((int)result.StatusCode, result);
    }
}

