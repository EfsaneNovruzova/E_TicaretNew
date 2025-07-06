using E_TicaretNew.Application.Abstracts.Services;
using E_TicaretNew.Application.DTOs.ProductDTOs;
using E_TicaretNew.Application.Shared;
using E_TicaretNew.Application.Shared.Responses;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace E_TicaretNew.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private  IProductService _productService {  get;  }

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

   
        [HttpGet]
        [ProducesResponseType(typeof(BaseResponse<List<ProductGetDto>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAll([FromQuery] ProductFilterDto filter)
        {
            var result = await _productService.GetAllAsync(filter);
            return StatusCode((int)result.StatusCode, result);
        }

      
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(BaseResponse<ProductGetDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(BaseResponse<ProductGetDto>), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _productService.GetByIdAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }

       
        [HttpGet("my")]
        [Authorize(Policy = Permissions.Product.GetMy)]
        [ProducesResponseType(typeof(BaseResponse<List<ProductGetDto>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetMyProducts()
        {
            var userId = User.FindFirst("sub")?.Value;
            var result = await _productService.GetMyProductsAsync(userId!);
            return StatusCode((int)result.StatusCode, result);
        }

       
        [HttpPost]
       [Authorize(Policy = Permissions.Product.Create)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Create([FromBody] ProductCreateDto dto)
        {
            dto.UserId = User.FindFirst("sub")?.Value!;
            var result = await _productService.CreateAsync(dto);
            return StatusCode((int)result.StatusCode, result);
        }

 
        [HttpPut("{id}")]
        [Authorize(Policy = Permissions.Product.Update)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Update(Guid id, [FromBody] ProductUpdateDto dto)
        {
            dto.Id = id;
            var userId = User.FindFirst("sub")?.Value!;
            var result = await _productService.UpdateAsync(dto, userId);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = Permissions.Product.Delete)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var userId = User.FindFirst("sub")?.Value!;
            var result = await _productService.DeleteAsync(id, userId);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}
