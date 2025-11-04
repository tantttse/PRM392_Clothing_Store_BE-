using ClothingStore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClothingStore.Infrastructure.Persistence.Configurations
{
    public class ExternalIdentityConfiguration : IEntityTypeConfiguration<ExternalIdentity>
    {
        public void Configure(EntityTypeBuilder<ExternalIdentity> builder)
        {
            // Table name
            builder.ToTable("external_identities");

            // Primary key
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id)
                   .HasColumnName("external_identity_id");

            // Properties
            builder.Property(e => e.UserId)
                   .HasColumnName("user_id")
                   .IsRequired();

            builder.Property(e => e.Provider)
                    .HasColumnName("provider")
                    .HasConversion<string>()   
                    .HasMaxLength(50)
                    .IsRequired();

            builder.Property(e => e.ProviderUserId)
                   .HasColumnName("provider_user_id")
                   .HasMaxLength(256)
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

            // Relationships
            builder.HasOne(e => e.User)
                    .WithMany(u => u.ExternalIdentities)
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);


            // Indexes
            builder.HasIndex(e => new { e.Provider, e.ProviderUserId })
                   .IsUnique()
                   .HasDatabaseName("uq_external_identity_provider_user");

            builder.HasIndex(e => e.UserId)
                   .HasDatabaseName("ix_external_identity_user_id");
        }
    }
}
