using E_Ticaret.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace E_TicaretNew.Persistence.Contexts;

public class E_TicaretNewDbContext : DbContext
{
    public E_TicaretNewDbContext(DbContextOptions<E_TicaretNewDbContext> options)
    {

    }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Favourite> Favourites { get; set; }
    public DbSet<Image> Images { get; set; }
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderProduct> OrderProducts { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public DbSet<User> Users { get; set; }


}
