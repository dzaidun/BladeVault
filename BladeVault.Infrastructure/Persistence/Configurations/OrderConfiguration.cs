using BladeVault.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace BladeVault.Infrastructure.Persistence.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("orders");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("id");

            builder.Property(x => x.OrderNumber)
                .HasColumnName("order_number")
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.UserId)
                .HasColumnName("user_id")
                .IsRequired();

            builder.Property(x => x.Status)
                .HasColumnName("status")
                .HasConversion<string>()
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.DeliveryMethod)
                .HasColumnName("delivery_method")
                .HasConversion<string>()
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.DeliveryAddress)
                .HasColumnName("delivery_address")
                .HasMaxLength(500)
                .IsRequired();

            builder.Property(x => x.TrackingNumber)
                .HasColumnName("tracking_number")
                .HasMaxLength(100);

            builder.Property(x => x.Comment)
                .HasColumnName("comment")
                .HasMaxLength(500);

            builder.Property(x => x.TotalAmount)
                .HasColumnName("total_amount")
                .HasPrecision(18, 2)
                .IsRequired();

            builder.Property(x => x.IsCallCenterOrder)
                .HasColumnName("is_call_center_order")
                .HasDefaultValue(false);

            builder.Property(x => x.CreatedAt)
                .HasColumnName("created_at");

            builder.Property(x => x.UpdatedAt)
                .HasColumnName("updated_at");

            // Індекс на номер замовлення
            builder.HasIndex(x => x.OrderNumber)
                .IsUnique();

            // Зв'язок з користувачем
            builder.HasOne(x => x.User)
                .WithMany(x => x.Orders)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Зв'язок з позиціями
            builder.HasMany(x => x.Items)
                .WithOne(x => x.Order)
                .HasForeignKey(x => x.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            // Зв'язок з оплатою
            builder.HasOne(x => x.Payment)
                .WithOne(x => x.Order)
                .HasForeignKey<Payment>(x => x.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
