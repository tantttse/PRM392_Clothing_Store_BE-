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
