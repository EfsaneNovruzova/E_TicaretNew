namespace E_Ticaret.Domain.Entities;

public class Notification : BaseEntity
{
    public string UserId { get; set; } = null!;
    public User User { get; set; } = null!;

    public string Message { get; set; } = null!;

    public bool IsRead { get; set; } = false;

    public Guid? ProductId { get; set; }
    public Product? Product { get; set; }

    public Guid? OrderId { get; set; }
    public Order? Order { get; set; }

    public Guid? FavoriteId { get; set; }
    public Favorite? Favorite { get; set; }
}
