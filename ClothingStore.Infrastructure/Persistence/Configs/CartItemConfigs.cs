using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ClothingStore.Domain.Entities;

namespace ClothingStore.Infrastructure.Persistence.Configurations
{
       public class CartItemConfiguration : IEntityTypeConfiguration<CartItem>
       {
              public void Configure(EntityTypeBuilder<CartItem> builder)
              {
                     // Table name
                     builder.ToTable("cart_items");

                     // Primary key
                     builder.HasKey(ci => ci.Id);
                     builder.Property(ci => ci.Id)
                            .HasColumnName("cart_item_id");

                     // Properties
                     builder.Property(ci => ci.CartId)
                            .HasColumnName("cart_id")
                            .IsRequired();

                     builder.Property(ci => ci.ProductId)
                            .HasColumnName("product_id")
                            .IsRequired();

                     builder.Property(ci => ci.Quantity)
                            .HasColumnName("quantity")
                            .IsRequired();

                     builder.Property(ci => ci.UnitPrice)
                            .HasColumnName("unit_price")
                            .HasColumnType("numeric(18,2)")
                            .IsRequired();

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
                     // SubTotal is a computed property, not mapped to DB
                     builder.Ignore(ci => ci.SubTotal);

                     // Relationships
                     builder.HasOne(ci => ci.Cart)
                            .WithMany(c => c.Items)
                            .HasForeignKey(ci => ci.CartId)
                            .OnDelete(DeleteBehavior.Cascade);

                     builder.HasOne(ci => ci.Product)
                            .WithMany() // if Product has a collection of CartItems, replace with .WithMany(p => p.CartItems)
                            .HasForeignKey(ci => ci.ProductId)
                            .OnDelete(DeleteBehavior.Restrict);

                     // Indexes
                     builder.HasIndex(ci => ci.CartId)
                            .HasDatabaseName("ix_cart_items_cart_id");

                     builder.HasIndex(ci => ci.ProductId)
                            .HasDatabaseName("ix_cart_items_product_id");
              }
       }
}
