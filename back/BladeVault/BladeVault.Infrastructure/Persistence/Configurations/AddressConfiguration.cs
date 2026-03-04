using BladeVault.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace BladeVault.Infrastructure.Persistence.Configurations
{
    public class AddressConfiguration : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.ToTable("addresses");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("id");

            builder.Property(x => x.UserId)
                .HasColumnName("user_id")
                .IsRequired();

            builder.Property(x => x.City)
                .HasColumnName("city")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(x => x.Street)
                .HasColumnName("street")
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(x => x.HouseNumber)
                .HasColumnName("house_number")
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(x => x.Apartment)
                .HasColumnName("apartment")
                .HasMaxLength(20);

            builder.Property(x => x.PostalCode)
                .HasColumnName("postal_code")
                .HasMaxLength(20);

            builder.Property(x => x.IsDefault)
                .HasColumnName("is_default")
                .HasDefaultValue(false);

            builder.Property(x => x.CreatedAt)
                .HasColumnName("created_at");

            builder.Property(x => x.UpdatedAt)
                .HasColumnName("updated_at");
        }
    }
}
