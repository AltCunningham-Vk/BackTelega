using System;
using System.Collections.Generic;
using BackTelega.Data.Configurations;
using BackTelega.Models;
using Microsoft.EntityFrameworkCore;

namespace BackTelega.Data;

public partial class ClontelegramContext : DbContext
{
    public ClontelegramContext(DbContextOptions<ClontelegramContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Chat> Chats { get; set; }
    public virtual DbSet<ChatMember> ChatMembers { get; set; }
    public virtual DbSet<Message> Messages { get; set; }
    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ChatConfiguration());
        modelBuilder.ApplyConfiguration(new ChatMemberConfiguration());
        modelBuilder.ApplyConfiguration(new MessageConfiguration());
        modelBuilder.ApplyConfiguration(new UserConfiguration());
    }
}