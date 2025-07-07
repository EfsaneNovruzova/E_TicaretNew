using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using E_TicaretNew.Application.Abstracts.Services;

using E_TicaretNew.Domain.Entities;
using E_TicaretNew.Application.Shared.Responses;
using E_TicaretNew.Application.Shared;
using System.Net;

[Route("api/[controller]")]
[ApiController]

public class PaymentsController : ControllerBase
{
    private readonly IPaymentService _paymentService;

    public PaymentsController(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    [HttpGet]
    [Authorize(Policy = Permissions.Payment.View)]
    [ProducesResponseType(typeof(BaseResponse<List<Payment>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<string>), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetAll()
    {
        var response = await _paymentService.GetAllAsync();
        return StatusCode((int)response.StatusCode, response);
    }

    [HttpGet("{id}")]
    [Authorize(Policy = Permissions.Payment.View)]
    [ProducesResponseType(typeof(BaseResponse<Payment>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<string>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(BaseResponse<string>), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var response = await _paymentService.GetByIdAsync(id);
        return StatusCode((int)response.StatusCode, response);
    }

    [HttpPost]
    [Authorize(Policy = Permissions.Payment.Create)]
    [ProducesResponseType(typeof(BaseResponse<Payment>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(BaseResponse<string>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<string>), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Create([FromBody] Payment payment)
    {
        var response = await _paymentService.CreateAsync(payment);
        return StatusCode((int)response.StatusCode, response);
    }

    [HttpPut("{id}")]
    [Authorize(Policy = Permissions.Payment.Update)]
    [ProducesResponseType(typeof(BaseResponse<Payment>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<string>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<string>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(BaseResponse<string>), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Update(Guid id, [FromBody] Payment payment)
    {
        if (id != payment.Id)
            return BadRequest(new BaseResponse<string>("Payment ID mismatch", HttpStatusCode.BadRequest));

        var response = await _paymentService.UpdateAsync(payment);
        return StatusCode((int)response.StatusCode, response);
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = Permissions.Payment.Delete)]
    [ProducesResponseType(typeof(BaseResponse<string>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<string>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(BaseResponse<string>), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var response = await _paymentService.DeleteAsync(id);
        return StatusCode((int)response.StatusCode, response);
    }
}

