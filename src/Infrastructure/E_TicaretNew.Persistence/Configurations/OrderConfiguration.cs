using E_Ticaret.Domain.Entities;
using E_TicaretNew.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace E_TicaretNew.Persistence.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("Orders");

        builder.HasKey(o => o.Id);

        // User ilə əlaqə: bir user-in çox sifarişi ola bilər
        builder.HasOne(o => o.User)
            .WithMany(u => u.Orders)
            .HasForeignKey(o => o.UserId)
            .OnDelete(DeleteBehavior.Restrict); // İstifadəçi silinəndə sifariş silinməsin

        // Payment ilə əlaqə: 1:1
        builder.HasOne(o => o.Payment)
            .WithOne(p => p.Order)
            .HasForeignKey<Payment>(p => p.OrderId)
            .OnDelete(DeleteBehavior.Restrict);

        // OrderProducts ilə əlaqə: 1:n
        builder.HasMany(o => o.OrderProducts)
            .WithOne(op => op.Order)
            .HasForeignKey(op => op.OrderId)
            .OnDelete(DeleteBehavior.Restrict);

        // Başqa sahələr üçün konfiqurasiya lazım olarsa əlavə edə bilərsən
    }
}
