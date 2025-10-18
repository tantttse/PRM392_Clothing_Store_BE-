using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ClothingStore.Domain.Entities;

namespace ClothingStore.Infrastructure.Persistence.Configurations
{
       public class ProductConfiguration : IEntityTypeConfiguration<Product>
       {
              public void Configure(EntityTypeBuilder<Product> builder)
              {
                     // Table
                     builder.ToTable("products");

                     // Primary key
                     builder.HasKey(p => p.Id);
                     builder.Property(p => p.Id)
                            .HasColumnName("product_id");

                     // Properties
                     builder.Property(p => p.ProductName)
                            .HasColumnName("product_name")
                            .HasMaxLength(200)
                            .IsRequired();

                     builder.Property(p => p.BriefDescription)
                            .HasColumnName("brief_description");

                     builder.Property(p => p.FullDescription)
                            .HasColumnName("full_description");

                     builder.Property(p => p.TechnicalSpecifications)
                            .HasColumnName("technical_specifications");

                     builder.Property(p => p.Price)
                            .HasColumnName("price")
                            .HasColumnType("numeric(18,2)")
                            .IsRequired();

                     builder.Property(p => p.ImageUrl)
                            .HasColumnName("image_url");

                     builder.Property(p => p.CategoryId)
                            .HasColumnName("category_id")
                            .IsRequired();
                     builder.Property(u => u.StockQuantity)
                            .HasColumnName("stock_quantity")
                            .HasColumnType("integer");

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

                     // Relationships
                     builder.HasOne(p => p.Category)
                            .WithMany(c => c.Products)
                            .HasForeignKey(p => p.CategoryId)
                            .OnDelete(DeleteBehavior.Cascade);

                     // Indexes
                     builder.HasIndex(p => p.CategoryId)
                            .HasDatabaseName("ix_products_category_id");

                     builder.HasIndex(p => p.ProductName)
                            .HasDatabaseName("ix_products_product_name");
              }
       }
}
