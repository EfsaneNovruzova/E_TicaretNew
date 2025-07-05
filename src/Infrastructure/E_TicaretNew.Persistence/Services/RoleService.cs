using System.Net;
using System.Security.Claims;
using E_TicaretNew.Application.Abstracts.Services;
using E_TicaretNew.Application.DTOs.RoleDTO;
using E_TicaretNew.Application.Shared.Responses;
using Microsoft.AspNetCore.Identity;

namespace E_TicaretNew.Persistence.Services;

public class RoleService : IRoleService
{
    private RoleManager<IdentityRole> _roleManager { get; }
    public RoleService(RoleManager<IdentityRole> roleManager)
    {
        _roleManager = roleManager;
    }


    public async Task<BaseResponse<string?>> CreateRole(RoleCreateDto dto)
    {
        // Əvvəlcə yoxla: belə adla rol var?
        var isRoleExist = await _roleManager.RoleExistsAsync(dto.Name);
        if (isRoleExist)
        {
            return new BaseResponse<string?>("Role already exists", null, HttpStatusCode.BadRequest);
        }

        // Yeni rol obyektini yarat
        var identityRole = new IdentityRole(dto.Name);
        var result = await _roleManager.CreateAsync(identityRole);

        if (!result.Succeeded)
        {
            var errors = string.Join(" | ", result.Errors.Select(e => e.Description));
            return new BaseResponse<string?>($"Failed to create role: {errors}", null, HttpStatusCode.BadRequest);
        }

        // ⚠️ BURADA: Permission-ləri rola claim kimi əlavə et
        foreach (var permission in dto.PermissionList.Distinct())
        {
            await _roleManager.AddClaimAsync(identityRole, new Claim("Permission", permission));
        }

        return new BaseResponse<string?>("Role created successfully", null, HttpStatusCode.Created);
    }

}

