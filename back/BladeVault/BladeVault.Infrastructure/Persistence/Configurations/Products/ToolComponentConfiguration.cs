using BladeVault.Domain.Entities.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace BladeVault.Infrastructure.Persistence.Configurations.Products
{
    public class ToolComponentConfiguration : IEntityTypeConfiguration<ToolComponent>
    {
        public void Configure(EntityTypeBuilder<ToolComponent> builder)
        {
            builder.ToTable("tool_components");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("id");

            builder.Property(x => x.MultiToolId)
                .HasColumnName("multi_tool_id")
                .IsRequired();

            builder.Property(x => x.Type)
                .HasColumnName("type")
                .HasConversion<string>()
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.Description)
                .HasColumnName("description")
                .HasMaxLength(200);
        }
    }
}
