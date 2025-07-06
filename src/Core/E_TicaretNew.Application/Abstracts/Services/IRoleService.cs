using E_TicaretNew.Application.DTOs.RoleDTO;
using E_TicaretNew.Application.Shared.Responses;
using Microsoft.AspNetCore.Identity;

namespace E_TicaretNew.Application.Abstracts.Services;

public interface IRoleService
{
    Task<BaseResponse<string?>> CreateRole(RoleCreateDto dto);
    Task<BaseResponse<List<IdentityRole>>> GetAllRoles();

    Task<BaseResponse<string?>> DeleteRole(string roleName);
}
