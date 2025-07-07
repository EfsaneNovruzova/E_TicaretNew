using System.Net;
using E_TicaretNew.Application.Abstracts.Services;
using E_TicaretNew.Application.DTOs.UserDtos;
using Microsoft.AspNetCore.Mvc;
using E_TicaretNew.Application.Shared.Responses;
using E_TicaretNew.Application.Shared;
using Microsoft.AspNetCore.Authorization;

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
        [ProducesResponseType(typeof(BaseResponse<TokenResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> login([FromBody] UserLoginDto dto)
        {
            var result = await _userService.Login(dto);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(BaseResponse<TokenResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest dto)
        {
            var result = await _userService.RefreshTokenAsync(dto);
            return StatusCode((int)result.StatusCode, result);
        }


        [HttpPost("assign-roles")]
        [Authorize(Policy = Permissions.Account.AddRole)]
        [ProducesResponseType(typeof(BaseResponse<TokenResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> AddRole([FromBody] UserAddRoleDto dto)
        {
            var result = await _userService.AddRole(dto);
            return StatusCode((int)result.StatusCode, result);
        }



        [HttpGet]
        [ProducesResponseType(typeof(BaseResponse<TokenResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> ConfirmEmail([FromQuery] string userId, [FromQuery] string token)
        {
            var result = await _userService.ConfirmEmail(userId,token);
            return StatusCode((int)result.StatusCode, result);
        }



        [HttpGet]
        [Authorize(Policy = Permissions.Account.GetAllUsers)]
        [ProducesResponseType(typeof(BaseResponse<List<UserGetDto>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.Forbidden)]
        public async Task<IActionResult> GetAllUsers()
        {
            var result = await _userService.GetAllUsersAsync();
            return StatusCode((int)result.StatusCode, result);
        }

        // Hər kəs görə bilər
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(BaseResponse<UserGetDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.NotFound)]

        public async Task<IActionResult> GetUserById(string id)
        {
            var result = await _userService.GetByIdAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}
