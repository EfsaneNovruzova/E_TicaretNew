namespace E_Ticaret.Domain.Entities;

public class User:BaseEntity
{
    public string Name { get; set; }
    public string Email { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public string? PhoneNumber { get; set; }
    public ICollection<Product> Products { get; set; }

    public ICollection<Favorite> Favorites { get; set; }

    public ICollection<Order> Orders { get; set; }
    public ICollection<Notification> Notifications { get; set; }
    public ICollection<Review> Reviews { get; set; }
}
