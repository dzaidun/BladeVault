using BladeVault.Domain.Entities.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace BladeVault.Infrastructure.Persistence.Configurations.Products
{
    public class KnifeConfiguration : IEntityTypeConfiguration<Knife>
    {
        public void Configure(EntityTypeBuilder<Knife> builder)
        {
            // TPT — окрема таблиця тільки для специфічних полів ножа
            // спільні поля (Name, Price тощо) — в таблиці "products"
            builder.ToTable("knives");

            // Явно визначаємо FK на батьківський тип Product
            builder.HasOne<Product>()
                .WithMany()
                .HasForeignKey(x => x.Id)
                .HasConstraintName("FK_knives_products_id")
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(x => x.KnifeType)
                .HasColumnName("knife_type")
                .HasConversion<string>()
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.SteelType)
                .HasColumnName("steel_type")
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.BladeLengthMm)
                .HasColumnName("blade_length_mm")
                .IsRequired();

            builder.Property(x => x.BladeThicknessMm)
                .HasColumnName("blade_thickness_mm")
                .IsRequired();

            builder.Property(x => x.BladeShape)
                .HasColumnName("blade_shape")
                .HasConversion<string>()
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.EdgeType)
                .HasColumnName("edge_type")
                .HasConversion<string>()
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.HandleMaterial)
                .HasColumnName("handle_material")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(x => x.HandleLengthMm)
                .HasColumnName("handle_length_mm");

            builder.Property(x => x.LockType)
                .HasColumnName("lock_type")
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(x => x.OpeningMechanism)
                .HasColumnName("opening_mechanism")
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(x => x.IsCoated)
                .HasColumnName("is_coated")
                .HasDefaultValue(false);

            builder.Property(x => x.CoatingType)
                .HasColumnName("coating_type")
                .HasMaxLength(100);

            builder.Property(x => x.HasClip)
                .HasColumnName("has_clip")
                .HasDefaultValue(false);

            builder.Property(x => x.HasGuard)
                .HasColumnName("has_guard")
                .HasDefaultValue(false);

            builder.Property(x => x.HasPommel)
                .HasColumnName("has_pommel")
                .HasDefaultValue(false);

            builder.Property(x => x.IncludesSheath)
                .HasColumnName("includes_sheath")
                .HasDefaultValue(false);

            builder.Property(x => x.SheathMaterial)
                .HasColumnName("sheath_material")
                .HasMaxLength(100);
        }
    }
}
