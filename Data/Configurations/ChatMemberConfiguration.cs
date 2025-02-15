using BackTelega.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace BackTelega.Data.Configurations
{
    public class ChatMemberConfiguration : IEntityTypeConfiguration<ChatMember>
    {
        public void Configure(EntityTypeBuilder<ChatMember> builder)
        {
            builder.ToTable("chat_members");

            builder.HasKey(e => e.Id).HasName("chat_members_pkey");

            builder.HasIndex(e => e.ChatId, "idx_chat_members_chat_id");
            builder.HasIndex(e => e.UserId, "idx_chat_members_user_id");

            builder.Property(e => e.Id).HasColumnName("id");
            builder.Property(e => e.ChatId).HasColumnName("chat_id");
            builder.Property(e => e.UserId).HasColumnName("user_id");

            builder.HasOne(d => d.Chat)
                .WithMany(p => p.ChatMembers)
                .HasForeignKey(d => d.ChatId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("chat_members_chat_id_fkey");

            builder.HasOne(d => d.User)
                .WithMany(p => p.ChatMembers)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("chat_members_user_id_fkey");
        }
    }
}
