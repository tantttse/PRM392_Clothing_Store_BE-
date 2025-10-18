using ClothingStore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClothingStore.Infrastructure.Persistence.Configurations
{
    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.ToTable("order_items");

            // Primary key
            builder.HasKey(oi => oi.Id);
            builder.Property(oi => oi.Id)
                   .HasColumnName("order_item_id");

            // Foreign key
            builder.Property(oi => oi.OrderId)
                   .HasColumnName("order_id")
                   .IsRequired();

            // Properties
            builder.Property(oi => oi.ProductId)
                   .HasColumnName("product_id")
                   .IsRequired();

            builder.Property(oi => oi.Quantity)
                   .HasColumnName("quantity")
                   .IsRequired();

            builder.Property(oi => oi.UnitPrice)
                   .HasColumnName("unit_price")
                   .HasColumnType("numeric(18,2)")
                   .IsRequired();

            // Audit fields
            builder.Property(u => u.CreatedAt)
                   .HasColumnName("created_at")
                   .HasColumnType("timestamp with time zone")
                   .HasDefaultValueSql("NOW()");

            builder.Property(u => u.CreatedBy)
                   .HasColumnName("created_by")
                   .HasMaxLength(100);

            builder.Property(u => u.ModifiedAt)
                   .HasColumnName("modified_at")
                   .HasColumnType("timestamp with time zone");

            builder.Property(u => u.ModifiedBy)
                   .HasColumnName("modified_by")
                   .HasMaxLength(100);

            builder.Property(p => p.IsActive)
                   .HasColumnName("is_active");

            // SubTotal is calculated in domain, not persisted
            builder.Ignore(oi => oi.SubTotal);

            // Indexes
            builder.HasIndex(oi => oi.OrderId)
                   .HasDatabaseName("ix_order_items_order_id");
        }
    }
}
