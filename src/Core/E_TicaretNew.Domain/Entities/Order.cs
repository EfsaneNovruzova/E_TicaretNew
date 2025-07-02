namespace E_Ticaret.Domain.Entities;

public class Order:BaseEntity
{
    public int UserId { get; set; }
    public User User { get; set; }
    public ICollection<OrderProduct> OrderProducts { get; set; }
}
