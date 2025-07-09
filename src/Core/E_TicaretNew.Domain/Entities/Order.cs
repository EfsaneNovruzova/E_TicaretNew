using E_TicaretNew.Domain.Entities;
using E_TicaretNew.Domain.Enums.OrderEnum;

namespace E_TicaretNew.Domain.Entities;


public class Order:BaseEntity
{
    public string UserId { get; set; }
    public User User { get; set; }
    public Payment Payment { get; set; }
    public Guid? PaymentId { get; set; }
    public OrderStatus Status { get; set; } = OrderStatus.PendingPayment;

    public decimal TotalAmount { get; set; } // Ümumi məbləğ
    public ICollection<OrderProduct> OrderProducts { get; set; }
}
