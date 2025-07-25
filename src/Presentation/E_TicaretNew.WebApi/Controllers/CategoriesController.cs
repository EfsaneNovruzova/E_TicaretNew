﻿using System.Net;
using E_TicaretNew.Application.Abstracts.Services;
using E_TicaretNew.Application.DTOs.CategoryDtos;
using E_TicaretNew.Application.Shared;
using E_TicaretNew.Application.Shared.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace E_TicaretNew.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private ICategoryService _categoryService { get; }
        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        [HttpPost]

        [Authorize(Policy = Permissions.Category.Create)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Post([FromBody] CategoryCreateDto dto)
        {
            var result = await _categoryService.AddAsync(dto);
            return StatusCode((int)result.StatusCode, result);
        }
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(BaseResponse<CategoryUpdateDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(BaseResponse<CategoryUpdateDto>), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(BaseResponse<CategoryUpdateDto>), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Put(Guid id, [FromBody] CategoryUpdateDto dto)
        {
            var result = await _categoryService.UpdateAsync(dto);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = Permissions.Category.Delete)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _categoryService.DeleteAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }
        [HttpGet]
        [ProducesResponseType(typeof(BaseResponse<List<CategoryGetDto>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(BaseResponse<List<CategoryGetDto>>), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Get()
        {
            var result = await _categoryService.GetAll();
            return StatusCode((int)result.StatusCode, result);
        }










    }
}
