using E_TicaretNew.Application.DTOs.RoleDTO;
using E_TicaretNew.Application.Shared.Responses;

namespace E_TicaretNew.Application.Abstracts.Services;

public interface IRoleService
{
    Task<BaseResponse<string?>> CreateRole(RoleCreateDto dto);
}
