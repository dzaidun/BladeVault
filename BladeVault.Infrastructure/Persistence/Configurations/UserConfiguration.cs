using BladeVault.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace BladeVault.Infrastructure.Persistence.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("users");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("id");

            builder.Property(x => x.FirstName)
                .HasColumnName("first_name")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(x => x.LastName)
                .HasColumnName("last_name")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(x => x.Email)
                .HasColumnName("email")
                .HasMaxLength(256)
                .IsRequired();

            builder.Property(x => x.PhoneNumber)
                .HasColumnName("phone_number")
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(x => x.PasswordHash)
                .HasColumnName("password_hash")
                .HasMaxLength(500)
                .IsRequired();

            builder.Property(x => x.Role)
                .HasColumnName("role")
                .HasConversion<string>() // зберігаємо як рядок: "Customer", "Admin"
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.IsActive)
                .HasColumnName("is_active")
                .HasDefaultValue(true);

            builder.Property(x => x.CreatedAt)
                .HasColumnName("created_at");

            builder.Property(x => x.UpdatedAt)
                .HasColumnName("updated_at");

            // Унікальні поля
            builder.HasIndex(x => x.Email)
                .IsUnique();

            builder.HasIndex(x => x.PhoneNumber)
                .IsUnique();

            // Зв'язок з адресами
            builder.HasMany(x => x.Addresses)
                .WithOne(x => x.User)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
