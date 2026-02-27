using BladeVault.Domain.Entities;
using BladeVault.Domain.Entities.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace BladeVault.Infrastructure.Persistence.Configurations.Products
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            // TPT — кожен тип має свою таблицю
            // Product → таблиця "products" (спільні поля)
            // Knife   → таблиця "knives"   (специфічні поля)
            // MultiTool → таблиця "multi_tools"
            builder.ToTable("products");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("id");

            builder.Property(x => x.Name)
                .HasColumnName("name")
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(x => x.Slug)
                .HasColumnName("slug")
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(x => x.SKU)
                .HasColumnName("sku")
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.Brand)
                .HasColumnName("brand")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(x => x.Model)
                .HasColumnName("model")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(x => x.CountryOfOrigin)
                .HasColumnName("country_of_origin")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(x => x.Description)
                .HasColumnName("description")
                .HasMaxLength(2000);

            builder.Property(x => x.Price)
                .HasColumnName("price")
                .HasPrecision(18, 2)
                .IsRequired();

            builder.Property(x => x.DiscountPrice)
                .HasColumnName("discount_price")
                .HasPrecision(18, 2);

            builder.Property(x => x.IsActive)
                .HasColumnName("is_active")
                .HasDefaultValue(true);

            builder.Property(x => x.WeightGrams)
                .HasColumnName("weight_grams");

            builder.Property(x => x.OverallLengthMm)
                .HasColumnName("overall_length_mm");

            builder.Property(x => x.ClosedLengthMm)
                .HasColumnName("closed_length_mm");

            builder.Property(x => x.CategoryId)
                .HasColumnName("category_id")
                .IsRequired();

            builder.Property(x => x.CreatedAt)
                .HasColumnName("created_at");

            builder.Property(x => x.UpdatedAt)
                .HasColumnName("updated_at");

            // Індекси
            builder.HasIndex(x => x.Slug).IsUnique();
            builder.HasIndex(x => x.SKU).IsUnique();

            // Зв'язок з категорією
            builder.HasOne(x => x.Category)
                .WithMany(x => x.Products)
                .HasForeignKey(x => x.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            // Зв'язок зі Stock
            builder.HasOne(x => x.Stock)
                .WithOne(x => x.Product)
                .HasForeignKey<Stock>(x => x.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            // Зв'язок з зображеннями
            builder.HasMany(x => x.Images)
                .WithOne(x => x.Product)
                .HasForeignKey(x => x.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
