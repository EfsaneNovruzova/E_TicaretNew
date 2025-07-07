namespace E_TicaretNew.Application.DTOs.RoleDTOs;

public class RoleUpdateDto
{
    public string? NewName { get; set; }  // Optional
    public List<string> PermissionList { get; set; } = new();
}
