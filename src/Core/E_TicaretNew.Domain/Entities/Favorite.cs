namespace E_Ticaret.Domain.Entities;

public class Favorite : BaseEntity
{
    public string UserId { get; set; }
    public User User { get; set; }
    public Guid ProductId { get; set; }
    public Product Product { get; set; }
}
