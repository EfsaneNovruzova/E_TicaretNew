using E_TicaretNew.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace E_TicaretNew.Persistence.Configurations;

public class ImageConfiguration : IEntityTypeConfiguration<Image>
{
    public void Configure(EntityTypeBuilder<Image> builder)
    {
        builder.HasKey(i => i.Id);

        // ImageUrl - required və max uzunluq
        builder.Property(i => i.ImageUrl)
               .IsRequired()
               .HasMaxLength(300);

        // IsMain - required
        builder.Property(i => i.IsMain)
               .IsRequired();

        // Many-to-one: Image → Product
        builder
            .HasOne(i => i.Product)
            .WithMany(p => p.Images)
            .HasForeignKey(i => i.ProductId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
