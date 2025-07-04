using E_TicaretNew.Domain.Entities;

namespace E_TicaretNew.Domain.Entities;


public class Order:BaseEntity
{
    public string UserId { get; set; }
    public User User { get; set; }
    public Payment Payment { get; set; }
    public ICollection<OrderProduct> OrderProducts { get; set; }
}
