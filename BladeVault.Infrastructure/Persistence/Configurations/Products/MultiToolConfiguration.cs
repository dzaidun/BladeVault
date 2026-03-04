using BladeVault.Domain.Entities.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BladeVault.Infrastructure.Persistence.Configurations.Products
{
    public class MultiToolConfiguration : IEntityTypeConfiguration<MultiTool>
    {
        public void Configure(EntityTypeBuilder<MultiTool> builder)
        {
            builder.ToTable("multi_tools");

            // Явно визначаємо FK на батьківський тип Product
            builder.HasOne<Product>()
                .WithMany()
                .HasForeignKey(x => x.Id)
                .HasConstraintName("FK_multi_tools_products_id")
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(x => x.HandleMaterial)
                .HasColumnName("handle_material")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(x => x.HasPliers)
                .HasColumnName("has_pliers")
                .HasDefaultValue(false);

            builder.Property(x => x.ReplaceableWireCutters)
                .HasColumnName("replaceable_wire_cutters")
                .HasDefaultValue(false);

            builder.Property(x => x.HasLocking)
                .HasColumnName("has_locking")
                .HasDefaultValue(false);

            builder.Property(x => x.IsOneHandOpenable)
                .HasColumnName("is_one_hand_openable")
                .HasDefaultValue(false);

            builder.Property(x => x.IncludesPouch)
                .HasColumnName("includes_pouch")
                .HasDefaultValue(false);

            builder.Property(x => x.PouchMaterial)
                .HasColumnName("pouch_material")
                .HasMaxLength(100);

            builder.Property(x => x.HasBitSet)
                .HasColumnName("has_bit_set")
                .HasDefaultValue(false);

            builder.Property(x => x.BitCount)
                .HasColumnName("bit_count");

            // Зв'язок з інструментами
            builder.HasMany(x => x.IncludedTools)
                .WithOne(x => x.MultiTool)
                .HasForeignKey(x => x.MultiToolId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
