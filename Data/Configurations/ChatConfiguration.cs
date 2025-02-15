using BackTelega.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace BackTelega.Data.Configurations
{
    public class ChatConfiguration : IEntityTypeConfiguration<Chat>
    {
        public void Configure(EntityTypeBuilder<Chat> builder)
        {
            builder.ToTable("chats");

            builder.HasKey(e => e.Id).HasName("chats_pkey");

            builder.HasIndex(e => e.ChatType, "idx_chats_type");

            builder.Property(e => e.Id).HasColumnName("id");
            builder.Property(e => e.ChatName)
                .HasMaxLength(100)
                .HasColumnName("chat_name");
            builder.Property(e => e.ChatType)
                .HasMaxLength(20)
                .HasColumnName("chat_type");
        }
    }
}
