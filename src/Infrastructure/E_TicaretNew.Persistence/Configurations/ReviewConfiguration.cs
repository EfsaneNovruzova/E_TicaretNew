using E_Ticaret.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace E_TicaretNew.Persistence.Configurations;
public class ReviewConfiguration : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> builder)
    {
        builder.HasKey(nameof(Review.ProductId), nameof(Review.UserId));


        builder.Property(r => r.Comment)
               .HasMaxLength(500);


        builder.Property(r => r.Rating)
               .IsRequired();


        builder.HasOne(r => r.Product)
               .WithMany(p => p.Reviews)
               .HasForeignKey(r => r.ProductId)
               .OnDelete(DeleteBehavior.Restrict);


        builder.HasOne(r => r.User)
               .WithMany(u => u.Reviews)
               .HasForeignKey(r => r.UserId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
