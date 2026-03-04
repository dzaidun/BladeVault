using BladeVault.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BladeVault.Infrastructure.Persistence.Configurations
{
    public class CallLogConfiguration : IEntityTypeConfiguration<CallLog>
    {
        public void Configure(EntityTypeBuilder<CallLog> builder)
        {
            builder.ToTable("call_logs");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("id");

            builder.Property(x => x.CustomerId)
                .HasColumnName("customer_id")
                .IsRequired();

            builder.Property(x => x.OrderId)
                .HasColumnName("order_id");

            builder.Property(x => x.Status)
                .HasColumnName("status")
                .HasConversion<string>()
                .HasMaxLength(40)
                .IsRequired();

            builder.Property(x => x.Comment)
                .HasColumnName("comment")
                .HasMaxLength(500);

            builder.Property(x => x.NextCallAt)
                .HasColumnName("next_call_at");

            builder.Property(x => x.PerformedByUserId)
                .HasColumnName("performed_by_user_id")
                .IsRequired();

            builder.Property(x => x.CreatedAt)
                .HasColumnName("created_at");

            builder.Property(x => x.UpdatedAt)
                .HasColumnName("updated_at");

            builder.HasOne<User>()
                .WithMany()
                .HasForeignKey(x => x.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<User>()
                .WithMany()
                .HasForeignKey(x => x.PerformedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<Order>()
                .WithMany()
                .HasForeignKey(x => x.OrderId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasIndex(x => x.CustomerId);
            builder.HasIndex(x => x.OrderId);
            builder.HasIndex(x => x.CreatedAt);
        }
    }
}
