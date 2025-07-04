using E_TicaretNew.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace E_TicaretNew.Persistence.Configurations;

public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        builder.HasKey(n => n.Id);


        builder.Property(n => n.Message)
               .IsRequired()
               .HasMaxLength(500);


        builder.Property(n => n.IsRead)
               .IsRequired();


        builder.HasOne(n => n.User)
               .WithMany(u => u.Notifications)
               .HasForeignKey(n => n.UserId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(n => n.Product)
               .WithMany()
               .HasForeignKey(n => n.ProductId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(n => n.Order)
               .WithMany()
               .HasForeignKey(n => n.OrderId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(n => n.Favorite)
               .WithMany()
               .HasForeignKey(n => n.FavoriteId)
               .OnDelete(DeleteBehavior.Restrict);
    }

}
