﻿using E_Ticaret.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace E_TicaretNew.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {

        builder.ToTable("Users");

        // Primary Key
        builder.HasKey(u => u.Id);

     
        builder.Property(u => u.Name)
            .IsRequired()
            .HasMaxLength(100);

      
        builder.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(150);

        builder.HasIndex(u => u.Email)
            .IsUnique(); 

    
        builder.Property(u => u.PasswordHash)
            .IsRequired();

        builder.Property(u => u.PhoneNumber)
            .HasMaxLength(20);

    }
}
