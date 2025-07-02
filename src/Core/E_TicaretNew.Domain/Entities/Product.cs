using static System.Net.Mime.MediaTypeNames;

namespace E_Ticaret.Domain.Entities;

public  class Product:BaseEntity
{
    public string Name { get; set; } = null!;
    public int UserId { get; set; }
    public User User { get; set; }
    public int CategoryId { get; set; }
    public Category Category { get; set; }

    public ICollection<Image> Images { get; set; }

    public ICollection<Favourite> Favourites { get; set; }

    public ICollection<OrderProduct> OrderProducts { get; set; }
    public ICollection<Review> Reviews { get; set; }

}
