using E_Ticaret.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace E_TicaretNew.Persistence.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {



        builder.HasKey(p => p.Id);

 
        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(100);

        // Price: decimal(18,2) formatı ilə
        builder.Property(p => p.Price)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

     
        builder.HasOne(p => p.User)
            .WithMany(u => u.Products)
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.Restrict);

     
        builder.HasOne(p => p.Category)
            .WithMany(c => c.Products)
            .HasForeignKey(p => p.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

      
        builder.HasMany(p => p.Images)
            .WithOne(i => i.Product)
            .HasForeignKey(i => i.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

       
        builder.HasMany(p => p.Favorites)
            .WithOne(f => f.Product)
            .HasForeignKey(f => f.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(p => p.OrderProducts)
            .WithOne(op => op.Product)
            .HasForeignKey(op => op.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        // Reviews ilə əlaqə
        builder.HasMany(p => p.Reviews)
            .WithOne(r => r.Product)
            .HasForeignKey(r => r.ProductId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}