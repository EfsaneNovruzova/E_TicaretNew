using System.Net;
using System.Security.Claims;
using E_TicaretNew.Application.Abstracts.Services;
using E_TicaretNew.Application.DTOs.RoleDTO;
using E_TicaretNew.Application.DTOs.RoleDTOs;
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
    
        var isRoleExist = await _roleManager.RoleExistsAsync(dto.Name);
        if (isRoleExist)
        {
            return new BaseResponse<string?>("Role already exists", null, HttpStatusCode.BadRequest);
        }

        
        var identityRole = new IdentityRole(dto.Name);
        var result = await _roleManager.CreateAsync(identityRole);

        if (!result.Succeeded)
        {
            var errors = string.Join(" | ", result.Errors.Select(e => e.Description));
            return new BaseResponse<string?>($"Failed to create role: {errors}", null, HttpStatusCode.BadRequest);
        }

       
        foreach (var permission in dto.PermissionList.Distinct())
        {
            await _roleManager.AddClaimAsync(identityRole, new Claim("Permission", permission));
        }

        return new BaseResponse<string?>("Role created successfully", null, HttpStatusCode.Created);
    }


    public async Task<BaseResponse<List<IdentityRole>>> GetAllRoles()
    {
        var roles = _roleManager.Roles.ToList();
        return new BaseResponse<List<IdentityRole>>("Roles retrieved successfully", roles, HttpStatusCode.OK);
    }

    public async Task<BaseResponse<string?>> DeleteRole(string roleName)
    {
        var role = await _roleManager.FindByNameAsync(roleName);
        if (role == null)
        {
            return new BaseResponse<string?>("Role not found", false, HttpStatusCode.NotFound);
        }

        var result = await _roleManager.DeleteAsync(role);
        if (!result.Succeeded)
        {
            var errors = string.Join(" | ", result.Errors.Select(e => e.Description));
            return new BaseResponse<string?>($"Failed to delete role: {errors}", false, HttpStatusCode.BadRequest);
        }

        return new BaseResponse<string?>("Role deleted successfully", true, HttpStatusCode.OK);
    }

    public async Task<BaseResponse<string?>> UpdateRoleAsync(string roleName, RoleUpdateDto dto)
    {
        var role = await _roleManager.FindByNameAsync(roleName);
        if (role == null)
            return new BaseResponse<string?>("Role not found", null, HttpStatusCode.NotFound);

        
        if (!string.IsNullOrWhiteSpace(dto.NewName) && dto.NewName != role.Name)
        {
            role.Name = dto.NewName;
            var nameUpdateResult = await _roleManager.UpdateAsync(role);
            if (!nameUpdateResult.Succeeded)
            {
                var nameErrors = string.Join(" | ", nameUpdateResult.Errors.Select(e => e.Description));
                return new BaseResponse<string?>($"Failed to update role name: {nameErrors}", null, HttpStatusCode.BadRequest);
            }
        }

        var currentClaims = await _roleManager.GetClaimsAsync(role);
        foreach (var claim in currentClaims.Where(c => c.Type == "Permission"))
        {
            await _roleManager.RemoveClaimAsync(role, claim);
        }

        
        foreach (var permission in dto.PermissionList.Distinct())
        {
            await _roleManager.AddClaimAsync(role, new Claim("Permission", permission));
        }

        return new BaseResponse<string?>("Role updated successfully", null, HttpStatusCode.OK);
    }


}

