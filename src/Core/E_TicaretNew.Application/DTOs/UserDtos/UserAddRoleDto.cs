namespace E_TicaretNew.Application.DTOs.UserDtos;

public class UserAddRoleDto
{
    public Guid UserId{ get; set; }
    public List<Guid> RolesId{ get; set; }
}
