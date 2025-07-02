using E_TicaretNew.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace E_TicaretNew.Persistence.Configurations;

public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.ToTable("Payments");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Amount)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(p => p.PaymentMethod)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(p => p.IsSuccessful)
            .IsRequired();

        builder.Property(p => p.TransactionId)
            .HasMaxLength(100)
            .IsRequired(false);

        // Order ilə 1:1 əlaqə
        builder.HasOne(p => p.Order)
            .WithOne(o => o.Payment)
            .HasForeignKey<Payment>(p => p.OrderId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
