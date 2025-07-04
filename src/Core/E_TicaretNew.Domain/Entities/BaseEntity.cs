namespace E_TicaretNew.Domain.Entities;

public class BaseEntity
{
    public Guid Id { get; set; }
    public string? CreatedUser { get; set; }
    public DateTime? CreatedAt { get; set; }
    public string? UpdatedUser { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
