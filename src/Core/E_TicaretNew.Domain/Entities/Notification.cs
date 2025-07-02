namespace E_Ticaret.Domain.Entities;

public class Notification:BaseEntity
{
    public Guid UserId { get; set; }
    public User User { get; set; }

    public string Message { get; set; }

    public bool IsRead { get; set; } = false;
    public Guid? ProductId { get; set; }
    public Product Product { get; set; }

    public Guid? OrderId { get; set; }
    public Order Order { get; set; }

    public Guid? FavoriteId { get; set; }
    public Favorite Favorite { get; set; }
}
