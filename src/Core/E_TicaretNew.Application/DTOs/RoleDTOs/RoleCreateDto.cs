namespace E_TicaretNew.Application.DTOs.RoleDTO;

public class RoleCreateDto
{
    public string Name { get; set; } = null!;
    public List<string> PermissionList { get; set; }
}
