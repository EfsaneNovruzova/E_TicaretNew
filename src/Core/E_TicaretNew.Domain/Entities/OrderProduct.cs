namespace E_Ticaret.Domain.Entities;

public class OrderProduct:BaseEntity
{
    public int Id { get; set; }

    public int OrderId { get; set; }
    public Order Order { get; set; }

    public int ProductId { get; set; }
    public Product Product { get; set; }
}
