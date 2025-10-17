using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ClothingStore.Domain.Entities;

namespace ClothingStore.Infrastructure.Persistence.Configurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            // Table
            builder.ToTable("categories");

            // Primary key
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id)
                   .HasColumnName("category_id");

            // Properties
            builder.Property(c => c.CategoryName)
                   .HasColumnName("category_name")
                   .HasMaxLength(150)
                   .IsRequired();

            builder.Property(c => c.CreatedAt)
                   .HasColumnName("created_at");

            builder.Property(c => c.ModifiedAt)
                   .HasColumnName("modified_at");

            builder.Property(c => c.IsActive)
                   .HasColumnName("is_active");

            // Relationships
            builder.HasMany(c => c.Products)
                   .WithOne(p => p.Category)
                   .HasForeignKey(p => p.CategoryId)
                   .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            builder.HasIndex(c => c.CategoryName)
                   .IsUnique()
                   .HasDatabaseName("uq_categories_category_name");
        }
    }
}
