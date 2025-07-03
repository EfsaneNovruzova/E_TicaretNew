using System.Net;
using E_TicaretNew.Application.Abstracts.Services;
using E_TicaretNew.Application.DTOs.UserDtos;
using Microsoft.AspNetCore.Mvc;
using E_TicaretNew.Application.Shared.Setting.Responses;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace E_TicaretNew.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private IUserService _userService { get; }
        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto dto)
        {
            var result=await _userService.Register(dto);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(BaseResponse<TokenResponse>), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> login([FromBody] UserLoginDto dto)
        {
            var result = await _userService.Login(dto);
            return StatusCode((int)result.StatusCode, result);
        }


    }
}
