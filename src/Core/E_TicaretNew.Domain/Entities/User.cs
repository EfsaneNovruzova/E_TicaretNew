using Microsoft.AspNetCore.Identity;
namespace E_TicaretNew.Domain.Entities;


public class User : IdentityUser
{
    public string FulName { get; set; } = null!;  
    public string RefreshToken { get; set; }=null!;
    public DateTime ExpiryDate { get; set; }

    public ICollection<Product> Products { get; set; } = new List<Product>();

    public ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();

    public ICollection<Order> Orders { get; set; } = new List<Order>();

    public ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    public ICollection<Review> Reviews { get; set; } = new List<Review>();
}
