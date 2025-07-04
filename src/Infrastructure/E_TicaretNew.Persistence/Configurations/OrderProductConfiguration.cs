using E_TicaretNew.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace E_TicaretNew.Persistence.Configurations;

public class OrderProductConfiguration : IEntityTypeConfiguration<OrderProduct>
{
    public void Configure(EntityTypeBuilder<OrderProduct> builder)
    {
        // Cədvəl adı
        builder.ToTable("OrderProducts");

        // Primary Key
        builder.HasKey(op => op.Id);

        // Quantity sahəsi - mütləqdir və sıfırdan kiçik ola bilməz
        builder.Property(op => op.Quantity)
            .IsRequired();

        // UnitPrice sahəsi - decimal(18,2)
        builder.Property(op => op.UnitPrice)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        // TotalPrice - hesablanan dəyərdir, EF bunu mapped etməsin
        builder.Ignore(op => op.TotalPrice); // Çünki bu dəyər bazada saxlanmır

        // Order ilə əlaqə
        builder.HasOne(op => op.Order)
            .WithMany(o => o.OrderProducts)
            .HasForeignKey(op => op.OrderId)
            .OnDelete(DeleteBehavior.Restrict);

        // Product ilə əlaqə
        builder.HasOne(op => op.Product)
            .WithMany(p => p.OrderProducts)
            .HasForeignKey(op => op.ProductId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
