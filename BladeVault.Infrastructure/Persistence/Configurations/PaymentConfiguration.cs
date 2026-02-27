using BladeVault.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace BladeVault.Infrastructure.Persistence.Configurations
{
    public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.ToTable("payments");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("id");

            builder.Property(x => x.OrderId)
                .HasColumnName("order_id")
                .IsRequired();

            builder.Property(x => x.Amount)
                .HasColumnName("amount")
                .HasPrecision(18, 2)
                .IsRequired();

            builder.Property(x => x.Status)
                .HasColumnName("status")
                .HasConversion<string>()
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.PaymentMethod)
                .HasColumnName("payment_method")
                .HasMaxLength(50);

            builder.Property(x => x.PaidAt)
                .HasColumnName("paid_at");

            builder.Property(x => x.CreatedAt)
                .HasColumnName("created_at");

            builder.Property(x => x.UpdatedAt)
                .HasColumnName("updated_at");
        }
    }
}
