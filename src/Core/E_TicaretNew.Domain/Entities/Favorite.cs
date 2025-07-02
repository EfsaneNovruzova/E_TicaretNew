namespace E_Ticaret.Domain.Entities;

public class Favourite:BaseEntity
{
    public int UserId { get; set; }
    public User User { get; set; }
    public int ProductId { get; set; }
    public Product Product { get; set; }
}
