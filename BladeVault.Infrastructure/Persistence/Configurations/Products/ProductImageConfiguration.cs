using BladeVault.Domain.Entities.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace BladeVault.Infrastructure.Persistence.Configurations.Products
{
    public class ProductImageConfiguration : IEntityTypeConfiguration<ProductImage>
    {
        public void Configure(EntityTypeBuilder<ProductImage> builder)
        {
            builder.ToTable("product_images");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("id");

            builder.Property(x => x.ProductId)
                .HasColumnName("product_id")
                .IsRequired();

            builder.Property(x => x.Url)
                .HasColumnName("url")
                .HasMaxLength(500)
                .IsRequired();

            builder.Property(x => x.IsMain)
                .HasColumnName("is_main")
                .HasDefaultValue(false);

            builder.Property(x => x.SortOrder)
                .HasColumnName("display_order")
                .HasDefaultValue(0);

            builder.Property(x => x.CreatedAt)
                .HasColumnName("created_at");

            builder.Property(x => x.UpdatedAt)
                .HasColumnName("updated_at");
        }
    }
}
