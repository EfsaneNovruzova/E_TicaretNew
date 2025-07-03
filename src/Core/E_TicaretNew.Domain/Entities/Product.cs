using static System.Net.Mime.MediaTypeNames;

namespace E_Ticaret.Domain.Entities;

public  class Product:BaseEntity
{
    public string Name { get; set; } = null!;
    public decimal Price { get; set; }
    public string UserId { get; set; }
    public User User { get; set; }
    public Guid CategoryId { get; set; }
    public Category Category { get; set; }
   // public decimal CommissionRate { get; set; } = 0m;

    public ICollection<Image> Images { get; set; }

    public ICollection<Favorite> Favorites { get; set; }

    public ICollection<OrderProduct> OrderProducts { get; set; }
    public ICollection<Review> Reviews { get; set; }

}
