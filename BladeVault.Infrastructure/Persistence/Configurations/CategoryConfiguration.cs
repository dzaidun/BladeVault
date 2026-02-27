using BladeVault.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace BladeVault.Infrastructure.Persistence.Configurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("categories");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("id");

            builder.Property(x => x.Name)
                .HasColumnName("name")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(x => x.Slug)
                .HasColumnName("slug")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(x => x.Description)
                .HasColumnName("description")
                .HasMaxLength(500);

            builder.Property(x => x.IsActive)
                .HasColumnName("is_active")
                .HasDefaultValue(true);

            builder.Property(x => x.CreatedAt)
                .HasColumnName("created_at");

            builder.Property(x => x.UpdatedAt)
                .HasColumnName("updated_at");

            // Унікальний slug
            builder.HasIndex(x => x.Slug)
                .IsUnique();

            // Самопосилання — підкатегорії
            builder.HasOne(x => x.ParentCategory)
                .WithMany(x => x.SubCategories)
                .HasForeignKey(x => x.ParentCategoryId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(false);
        }
    }
}
