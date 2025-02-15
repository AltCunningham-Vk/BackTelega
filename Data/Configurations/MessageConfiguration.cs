using BackTelega.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace BackTelega.Data.Configurations
{
    public class MessageConfiguration : IEntityTypeConfiguration<Message>
    {
        public void Configure(EntityTypeBuilder<Message> builder)
        {
            builder.ToTable("messages");

            builder.HasKey(e => e.Id).HasName("messages_pkey");

            builder.HasIndex(e => e.ChatId, "idx_messages_chat_id");
            builder.HasIndex(e => e.SenderId, "idx_messages_sender_id");
            builder.HasIndex(e => e.SentAt, "idx_messages_sent_at").IsDescending();

            builder.Property(e => e.Id).HasColumnName("id");
            builder.Property(e => e.ChatId).HasColumnName("chat_id");
            builder.Property(e => e.MediaType)
                .HasMaxLength(20)
                .HasColumnName("media_type");
            builder.Property(e => e.MediaUrl).HasColumnName("media_url");
            builder.Property(e => e.MessageText).HasColumnName("message_text");
            builder.Property(e => e.SenderId).HasColumnName("sender_id");
            builder.Property(e => e.SentAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("sent_at");

            builder.HasOne(d => d.Chat)
                .WithMany(p => p.Messages)
                .HasForeignKey(d => d.ChatId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("messages_chat_id_fkey");

            builder.HasOne(d => d.Sender)
                .WithMany(p => p.Messages)
                .HasForeignKey(d => d.SenderId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("messages_sender_id_fkey");
        }
    }
}
