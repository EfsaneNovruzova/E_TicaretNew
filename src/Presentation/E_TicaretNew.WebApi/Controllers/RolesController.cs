using E_TicaretNew.Application.Abstracts.Services;
using E_TicaretNew.Application.DTOs.RoleDTO;
using E_TicaretNew.Application.DTOs.RoleDTOs;
using E_TicaretNew.Application.Shared;
using E_TicaretNew.Application.Shared.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace E_TicaretNew.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private IRoleService _roleService {  get;  }
        public RolesController(IRoleService roleService)
        { 
            _roleService = roleService;
        }
        [HttpGet("permissions")]
        public IActionResult GetAllPermissions()
        {
            var permissions = PermissionHelper.GetAllPermissions();
            return Ok(permissions);
        }
        [HttpPost]
        [Authorize(Policy = Permissions.Role.Create)]
        public async Task<IActionResult> Create(RoleCreateDto dto)
        {
            var result =await _roleService.CreateRole(dto);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRoles()
        {
            var result = await _roleService.GetAllRoles();
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpDelete("{roleName}")]
        [Authorize(Policy = Permissions.Role.Delete)]
        public async Task<IActionResult> DeleteRole(string roleName)
        {
            var result = await _roleService.DeleteRole(roleName);
            return StatusCode((int)result.StatusCode, result);
        }


        [HttpPut("{roleName}")]
        [Authorize(Policy = Permissions.Role.Update)]
        public async Task<IActionResult> UpdateRole(string roleName, [FromBody] RoleUpdateDto dto)
        {
            var result = await _roleService.UpdateRoleAsync(roleName, dto);
            return StatusCode((int)result.StatusCode, result);
        }


    }
}
