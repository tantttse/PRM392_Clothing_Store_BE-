using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ClothingStore.Domain.Entities;

namespace ClothingStore.Infrastructure.Persistence.Configurations
{
    public class CartConfiguration : IEntityTypeConfiguration<Cart>
    {
        public void Configure(EntityTypeBuilder<Cart> builder)
        {
            // Table name in snake_case
            builder.ToTable("carts");

            // Primary key
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id)
                   .HasColumnName("cart_id");

            // Properties
            builder.Property(c => c.UserId)
                   .HasColumnName("user_id")
                   .IsRequired();

            builder.Property(c => c.TotalPrice)
                   .HasColumnName("total_price")
                   .HasColumnType("numeric(18,2)")
                   .IsRequired();

            builder.Property(c => c.Status)
                   .HasColumnName("status")
                   .HasMaxLength(50)
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

            // Relationships
            builder.HasOne(c => c.User)
                   .WithMany() // or .WithOne(u => u.Cart) if 1-to-1
                   .HasForeignKey(c => c.UserId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(c => c.Items)
                   .WithOne(i => i.Cart)
                   .HasForeignKey(i => i.CartId)
                   .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            builder.HasIndex(c => c.UserId)
                   .HasDatabaseName("ix_carts_user_id");

            // Optional: enforce only one active cart per user
            // builder.HasIndex(c => new { c.UserId, c.Status })
            //        .IsUnique()
            //        .HasFilter("status = 'Active'")
            //        .HasDatabaseName("uq_carts_user_active");
        }
    }
}
