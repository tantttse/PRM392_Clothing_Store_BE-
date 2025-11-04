using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using ClothingStore.Domain.Entities;
using ClothingStore.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClothingStore.Infrastructure.Persistence.Configurations
{
       public class UserConfiguration : IEntityTypeConfiguration<Users>
       {
              public void Configure(EntityTypeBuilder<Users> builder)
              {
                     builder.ToTable("users");

                     // Primary key
                     builder.HasKey(u => u.Id);

                     builder.Property(u => u.Id)
                            .HasColumnName("id")
                            .HasColumnType("uuid")
                            .HasDefaultValueSql("gen_random_uuid()");

                     // Properties
                     builder.Property(u => u.Email)
                            .HasColumnName("email")
                            .HasMaxLength(255);

                     builder.Property(u => u.UserName)
                            .HasColumnName("username")
                            .IsRequired()
                            .HasMaxLength(100);

                     builder.Property(u => u.PasswordHash)
                            .HasColumnName("password_hash")
                            .IsRequired();

                     builder.Property(u => u.FirstName)
                            .HasColumnName("first_name")
                            .HasMaxLength(100);

                     builder.Property(u => u.LastName)
                            .HasColumnName("last_name")
                            .HasMaxLength(100);

                     builder.Property(u => u.PhoneNumber)
                            .HasColumnName("phone_number")
                            .HasMaxLength(20);

                     builder.Property(u => u.Address)
                            .HasColumnName("address")
                            .HasMaxLength(255);

                     builder.Property(u => u.IsActive)
                            .HasColumnName("is_active")
                            .HasDefaultValue(true);

                     builder.Property(u => u.ProfileImageUrl)
                            .HasColumnName("profile_image_url")
                            .HasMaxLength(500);

                     builder.Property(u => u.RefreshToken)
                            .HasColumnName("refresh_token");

                     builder.Property(u => u.RefreshTokenExpiry)
                            .HasColumnName("refresh_token_expiry");

                     // Auditing fields
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

                     // Roles configuration
                     builder.Property(u => u.Roles)
                            .HasColumnName("roles")
                            .HasColumnType("text[]")
                            .HasConversion(
                                v => v.Select(r => r.ToString()).ToArray(),
                                v => v.Select(s => Enum.Parse<RoleType>(s)).ToList(),
                                new ValueComparer<ICollection<RoleType>>(
                                    (c1, c2) => (c1 == null && c2 == null) ||
                                                (c1 != null && c2 != null && c1.SequenceEqual(c2)),
                                    c => c == null ? 0 : c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                                    c => c == null ? new List<RoleType>() : c.ToList()
                                )
                            );

                     builder.HasMany(u => u.Carts)
                            .WithOne(c => c.User)
                            .HasForeignKey(c => c.UserId);
                     
                     builder.HasMany(u => u.ExternalIdentities)
                            .WithOne(e => e.User)
                            .HasForeignKey(e => e.UserId)
                            .OnDelete(DeleteBehavior.Cascade);
              }
       }
}