namespace E_Ticaret.Domain.Entities;

public class Notification:BaseEntity
{
    public int UserId { get; set; }
    public User User { get; set; }

    public string Message { get; set; }

    public bool IsRead { get; set; } = false;
    public int? ProductId { get; set; }
    public Product Product { get; set; }

    public int? OrderId { get; set; }
    public Order Order { get; set; }

    public int? FavouriteId { get; set; }
    public Favourite Favourite { get; set; }
}
