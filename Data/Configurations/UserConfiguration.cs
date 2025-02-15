using BackTelega.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace BackTelega.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("users");

            builder.HasKey(e => e.Id).HasName("users_pkey");

            builder.HasIndex(e => e.Email, "idx_users_email");
            builder.HasIndex(e => e.Username, "idx_users_username");

            builder.HasIndex(e => e.Email, "users_email_key").IsUnique();
            builder.HasIndex(e => e.Username, "users_username_key").IsUnique();

            builder.Property(e => e.Id).HasColumnName("id");
            builder.Property(e => e.CreatedAt)
                .HasDefaultValueSql("NOW()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            builder.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            builder.Property(e => e.PasswordHash).HasColumnName("password_hash");
            builder.Property(e => e.Username)
                .HasMaxLength(50)
                .HasColumnName("username");
        }
    }
}
