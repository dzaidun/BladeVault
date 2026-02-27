using BladeVault.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace BladeVault.Infrastructure.Persistence.Configurations
{
    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.ToTable("order_items");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("id");

            builder.Property(x => x.OrderId)
                .HasColumnName("order_id")
                .IsRequired();

            builder.Property(x => x.ProductId)
                .HasColumnName("product_id")
                .IsRequired();

            builder.Property(x => x.ProductName)
                .HasColumnName("product_name")
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(x => x.ProductSKU)
                .HasColumnName("product_sku")
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.Quantity)
                .HasColumnName("quantity")
                .IsRequired();

            builder.Property(x => x.UnitPrice)
                .HasColumnName("unit_price")
                .HasPrecision(18, 2)
                .IsRequired();

            // TotalPrice — обчислюване поле, не зберігаємо в БД
            builder.Ignore(x => x.TotalPrice);

            // Зв'язок з продуктом
            builder.HasOne(x => x.Product)
                .WithMany()
                .HasForeignKey(x => x.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
