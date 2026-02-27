using BladeVault.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace BladeVault.Infrastructure.Persistence.Configurations
{
    public class StockConfiguration : IEntityTypeConfiguration<Stock>
    {
        public void Configure(EntityTypeBuilder<Stock> builder)
        {
            builder.ToTable("stocks");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("id");

            builder.Property(x => x.ProductId)
                .HasColumnName("product_id")
                .IsRequired();

            builder.Property(x => x.Quantity)
                .HasColumnName("quantity")
                .HasDefaultValue(0)
                .IsRequired();

            builder.Property(x => x.ReservedQuantity)
                .HasColumnName("reserved_quantity")
                .HasDefaultValue(0)
                .IsRequired();

            builder.Property(x => x.CreatedAt)
                .HasColumnName("created_at");

            builder.Property(x => x.UpdatedAt)
                .HasColumnName("updated_at");
        }
    }
}
