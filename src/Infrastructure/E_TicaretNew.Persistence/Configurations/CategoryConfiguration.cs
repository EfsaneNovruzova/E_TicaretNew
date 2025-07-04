using E_TicaretNew.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace E_TicaretNew.Persistence.Configurations;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.Property(ct => ct.Name)
           .IsRequired();

        builder.HasIndex(ct => ct.Name)
            .IsUnique();

        builder.HasMany(c => c.Products)
                       .WithOne(p => p.Category)
                       .HasForeignKey(p => p.CategoryId);

    }
}
