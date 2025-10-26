using ClothingStore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClothingStore.Infrastructure.Persistence.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("orders");

            builder.HasKey(o => o.Id);
            builder.Property(o => o.Id).HasColumnName("order_id");

            builder.Property(o => o.CartId)
                   .HasColumnName("cart_id")
                   .IsRequired();

            builder.Property(o => o.UserId)
                   .HasColumnName("user_id")
                   .IsRequired();

            builder.Property(o => o.PaymentMethod)
                   .HasColumnName("payment_method")
                   .HasMaxLength(100)
                   .IsRequired();

            builder.Property(o => o.BillingAddress)
                   .HasColumnName("billing_address")
                   .HasMaxLength(500)
                   .IsRequired();

            builder.Property(o => o.Status)
                   .HasColumnName("status")
                   .HasMaxLength(50)
                   .IsRequired();

            builder.Property(o => o.PaymentStatus)
                   .HasColumnName("payment_status")
                   .HasMaxLength(50)
                   .IsRequired();

            builder.Property(o => o.OrderDate)
                   .HasColumnName("order_date")
                   .HasColumnType("timestamp with time zone")
                   .HasDefaultValueSql("NOW()");

            builder.Property(o => o.TotalAmount)
                   .HasColumnName("total_amount")
                   .HasColumnType("numeric(18,2)")
                   .IsRequired();

            builder.Property(o => o.CreatedAt)
                   .HasColumnName("created_at")
                   .HasColumnType("timestamp with time zone")
                   .HasDefaultValueSql("NOW()");

            builder.Property(o => o.CreatedBy)
                   .HasColumnName("created_by")
                   .HasMaxLength(100);

            builder.Property(o => o.ModifiedAt)
                   .HasColumnName("modified_at")
                   .HasColumnType("timestamp with time zone");

            builder.Property(o => o.ModifiedBy)
                   .HasColumnName("modified_by")
                   .HasMaxLength(100);

            builder.Property(p => p.IsActive)
                   .HasColumnName("is_active");

            builder.HasMany(o => o.Items)
                   .WithOne()
                   .HasForeignKey(oi => oi.OrderId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(o => o.UserId)
                   .HasDatabaseName("ix_orders_user_id");
        }
    }
}
